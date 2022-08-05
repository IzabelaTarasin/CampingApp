using System;
using System.Numerics;
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
            string image,
            double pricePerDay,
            int maxPeople,
            bool animalsAllowed,
            bool restaurantExist,
            bool receptionExist,
            bool medicExist,
            bool grillExist,
            bool wifiExist,
            bool swimmingpoolExist,
            bool laundryRoomExist,
            bool bikesExist,
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
        public Task<List<Place>> Search(DateTime start, DateTime end, int numberOfPeople, string voivodenship);
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
            string image,
            double pricePerDay,
            int maxPeople,
            bool animalsAllowed,
            bool restaurantExist,
            bool receptionExist,
            bool medicExist,
            bool grillExist,
            bool wifiExist,
            bool swimmingpoolExist,
            bool laundryRoomExist,
            bool bikesExist,
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
                Image = image,
                MaxPeople = maxPeople,
                PricePerDay = pricePerDay,
                Address = address,
                AnimalsAllowed = animalsAllowed,
                RestaurantExist = restaurantExist,
                ReceptionExist = receptionExist,
                MedicExist = medicExist,
                GrillExist = grillExist,
                WifiExist = wifiExist,
                SwimmingpoolExist = swimmingpoolExist,
                LaundryRoomExist = laundryRoomExist,
                BikesExist = bikesExist
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

        public async Task<List<Place>> Search(DateTime start, DateTime end, int numberOfPeople, string voivodenship)
        {
            var joinedQuery = from place in _applicationDbContext.Places
                              where place.Address.Voivodeship == voivodenship
                              join res in _applicationDbContext.Reservations.Where(r => r.StartDate >= start && r.EndDate <= end)
                                on place equals res.Place into places
                              from placeRes in places.DefaultIfEmpty()
                              select new
                              {
                                  Place = place,
                                  PeopleCount = placeRes == null ? 0 : placeRes.NumberOfPeople,
                              };

            var x = await joinedQuery.ToListAsync();
            var y = x.GroupBy(x => x.Place)
                .ToDictionary(d => d.Key, d => d.Sum(c => c.PeopleCount))
                .Where(d => d.Key.MaxPeople >= d.Value + numberOfPeople)
                .Select(d => d.Key)
                .ToList();

            return y;
        }
    }
}

