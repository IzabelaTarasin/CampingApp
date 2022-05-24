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
            DateTime endDate);
        public Task<List<Reservation>> GetReservationsByUserId(string userId);
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
            DateTime endDate)
        {
            Reservation reservation = new Reservation
            {
                PlaceId = placeId,
                StatusId = Status.StatusActive, //nie przekazujemy przy tworzeniu rezerwacji tylko z palca ja ustawiamy
                UserId = userId,
                StartDate = startDate.ToUniversalTime(),
                EndDate = endDate.ToUniversalTime()
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
                .ToListAsync();

            return reservations;
        }

        //public async Task<List<Reservation>> GetReservationsForMyPlacesByPlaceIds(List<Place> myPlaces)
        //{
        //    List<Place> places = myPlaces;

        //    List<Reservation> reservations;
        //    reservations = await _applicationDbContext.Reservations.Where( c => c.PlaceId exsist in places ).groupby(placesId).to list;
            
        //}

    }
}