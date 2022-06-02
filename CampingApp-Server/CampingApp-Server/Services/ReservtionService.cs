using System;
using CampingApp_Server.Database;
using Microsoft.EntityFrameworkCore;

namespace CampingApp_Server.Services
{
    public interface IReservtionService
    {
        public Task<Reservation> AddReservation(
            int placeId,
            string userId,
            DateTime startDate,
            DateTime endDate,
            int NumberOfPeople);
        public Task<List<Reservation>> GetReservationsByUserId(string userId);
        public Task<Reservation?> GetReservationById(int id);
    }
    public class ReservtionService : IReservtionService
    {
        ApplicationDbContext _applicationDbContext;

        public ReservtionService(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<Reservation> AddReservation(
            int placeId,
            string userId,
            DateTime startDate,
            DateTime endDate,
            int numberOfPeople)
        {
            Reservation reservation = new Reservation
            {
                PlaceId = placeId,
                StatusId = Status.StatusActive, //nie przekazujemy przy tworzeniu rezerwacji tylko z palca ja ustawiamy
                UserId = userId,
                StartDate = startDate.ToUniversalTime(),
                EndDate = endDate.ToUniversalTime(),
                NumberOfPeople = numberOfPeople
            };

            //zapis do bazy danych:
            await _applicationDbContext.Reservations.AddAsync(reservation);

            await _applicationDbContext.SaveChangesAsync();

            return reservation;
        }

        public async Task<List<Reservation>> GetReservationsByUserId(string userId)
        {
            List<Reservation> reservations;
            //gdy pobieramy zbazy danaych to operujemy na pplicationDbContext obiekcie
            reservations = await _applicationDbContext
                .Reservations
                .Where(c => c.UserId == userId)
                .Include(r => r.Place)
                .ThenInclude(p => p.Address)
                .Include(r => r.Status)
                .ToListAsync();

            Console.WriteLine("iza" + reservations[0].EndDate);
            Console.WriteLine("iza" + reservations[0].NumberOfPeople);

            return reservations;
        }

        //public async Task<List<Reservation>> GetReservationsForMyPlacesByPlaceIds(List<Place> myPlaces)
        //{
        //    List<Place> places = myPlaces;

        //    List<Reservation> reservations;
        //    reservations = await _applicationDbContext.Reservations.Where( c => c.PlaceId exsist in places ).groupby(placesId).to list;

        //}
        public async Task<Reservation?> GetReservationById(int id)
        {
            //gdy pobieramy zbazy danaych to operujemy na pplicationDbContext obiekcie
            return await _applicationDbContext
                .Reservations
                .Include(r => r.Place) //zaciagamy place bo bez tego adres byl null przy pobraniu choc w bazie byl
                .ThenInclude(p => p.Address)
                .Include(r => r.User)
                .Include(r => r.Status)
                .FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}