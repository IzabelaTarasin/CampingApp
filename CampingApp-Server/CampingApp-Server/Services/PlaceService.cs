using System;
using CampingApp_Server.Database;
using Microsoft.EntityFrameworkCore;

namespace CampingApp_Server.Services
{
	public interface IPlaceService
	{
		public Task<Place> AddPlace
			(string userId,
			string name,
			string description,
			string imagePath,
			double pricePerDay,
			bool animalsAllowed,
			bool restaurantExist,
			bool receptionExist,
			bool medicExist,
			bool grillExist,
			bool wifiExist,
			bool swimmingpoolExist,
			string postalCode,
			string city,
			string houseNumber,
			string localNumber,
			string street,
			string voivodeship,
			string country
		);
		public Task<List<Place>> GetPlacesByUserId(string userId);
		public Task<Place> GetPlaceById(int placeId);
		public Task<List<Place>> GetAllPlaces();
	}

	public class PlaceService : IPlaceService
	{
		private ApplicationDbContext _applicationDbContext;

		public PlaceService(ApplicationDbContext applicationDbContext) //dependency - wstrzykiwany do konstruktora.
		{
			_applicationDbContext = applicationDbContext;
		}

		public async Task<Place> AddPlace(
			string userId,
			string name,
			string description,
			string imagePath,
			double pricePerDay,
			bool animalsAllowed,
			bool restaurantExist,
			bool receptionExist,
			bool medicExist,
			bool grillExist,
			bool wifiExist,
			bool swimmingpoolExist,
			string postalCode,
			string city,
			string houseNumber,
			string localNumber,
			string street,
			string voivodeship,
			string country)
		{
			Address address = new Address
			{
				City = city,
				Country = country,
				HouseNumber = houseNumber,
				LocalNumber = localNumber,
				Street = street,
				PostalCode = postalCode,
				Voivodeship = voivodeship
			};

			Place place = new Place
			{
				UserId = userId,
				Name = name,
				Description = description,
				ImagePath = imagePath,
				PricePerDay = pricePerDay,
				Address = address,
				AnimalsAllowed = animalsAllowed,
				RestaurantExist = restaurantExist,
				ReceptionExist = receptionExist,
				MedicExist = medicExist,
				GrillExist = grillExist,
				WifiExist = wifiExist,
				SwimmingpoolExist = swimmingpoolExist
			};

			//wstrzyknac do tego placeservice application db context
			//app db context . places.addasync i przekazac obiekt place
			//savecontect by zapisac do bazki
			await _applicationDbContext.Places.AddAsync(place);

			//if (resultAdd == null) //przechwycony wyjatek bedzie w try catch (wyzej) wiec nie ma potzreby dawac  if-y
			//{
			//	return false;
			//}

			await _applicationDbContext.SaveChangesAsync();

    //        if (resultSave == null)
    //        {
				//return false;
    //        }

			return place;

		}

		public async Task<List<Place>> GetPlacesByUserId(string userId)
        {
			List<Place> places;
			//gdy pobieramy zbazy danaych to operujemy na pplicationDbContext obiekcie
			places = await _applicationDbContext
				.Places
				.Where(c => c.UserId == userId)
				.Include(c => c.Address) //zaciagamy adres bo bez tego adres byl null przy pobraniu choc w bazie byl
				.ToListAsync();

			return places;
        }

		public async Task<Place> GetPlaceById(int placeId)
		{
			//gdy pobieramy zbazy danaych to operujemy na pplicationDbContext obiekcie
			return await _applicationDbContext
				.Places
				.Include(c => c.Address)
				.SingleOrDefaultAsync(c => c.Id == placeId); //zaciagamy adres bo bez tego adres byl null przy pobraniu choc w bazie byl
		}

		public async Task<List<Place>> GetAllPlaces()
		{
			List<Place> places;
			//gdy pobieramy zbazy danaych to operujemy na pplicationDbContext obiekcie
			places = await _applicationDbContext.Places.ToListAsync();

			return places;
		}

	}
}

