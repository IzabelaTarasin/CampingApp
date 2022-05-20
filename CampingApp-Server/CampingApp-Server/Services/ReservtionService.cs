using System;
using CampingApp_Server.Database;

namespace CampingApp_Server.Services
{
    public interface IReservtionService
    {
        public Task<Reservation> AddReservation(
            string userId,
            string startDate,
            string endDate);
    }
    public class ReservtionService : IReservtionService
    {
        ApplicationDbContext _applicationDbContext;

        public ReservtionService(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<Reservation> AddReservation(
            //int placeId,
            //User user,
            //Status status,
            string userId,
            string startDate,
            string endDate)
        {
            Reservation reservation = new Reservation
            {
                //PlaceId = placeId,
                //User = user,
                //Status = status,
                UserId = userId,
                StartDate = startDate,
                EndDate = endDate
            };

            //zapis do bazy danych:
            await _applicationDbContext.Reservations.AddAsync(reservation);

            await _applicationDbContext.SaveChangesAsync();

            return reservation;
        }

    }
}