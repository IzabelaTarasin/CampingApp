using System;
using CampingApp_Server.Database;
using Microsoft.AspNetCore.Identity;

namespace CampingApp_Server.Services
{
	public interface IPlaceService
	{
		public Task<Place> AddPlace(
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
			bool swimmingpoolExist);
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
			bool swimmingpoolExist)
		{
			Place place = new Place
			{
				UserId = userId,
				Name = name,
				Description = description,
				ImagePath = imagePath,
				PricePerDay = pricePerDay,
				Address = new Address { City="Krakow", Country="Poland", HouseNumber="1", LocalNumber="3", Street="toko", PostalCode="1234"},
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
	}
}

