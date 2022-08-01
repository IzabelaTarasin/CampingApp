﻿using CampingApp_Server.Services;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CampingApp_Server.Database;
using System.Net;
using System.Net.Http.Headers;

namespace CampingApp_Server.Controllers
{
    public record ReservationDTO( //nie przekazujemy usera bo jest przekazywany "w locie" w metodzie
        int placeId,
        DateTime startDate,
        DateTime endDate,
        int NumberOfPeople);

    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ReservationController : ControllerBase
    {
        private IReservtionService _reservtionService;
        private IPlaceService _placeService;
        private IPdfCreatorService _pdfCreatorService;
        private ILogger<ReservationController> _logger;

        public ReservationController(
            IReservtionService reservtionService,
            IPlaceService placeService,
            IPdfCreatorService pdfCreatorService,
            ILogger<ReservationController> logger)
        {
            _reservtionService = reservtionService;
            _placeService = placeService;
            _pdfCreatorService = pdfCreatorService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> AddReservation(ReservationDTO dto)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                //tu wyciagnac kim jest user i jak jesgo pole to narka nie mozna rezerwowac

                var result = await _reservtionService.AddReservation(
                    dto.placeId,
                    userId, //przypisanie aktualnie zalogowanego uzytkownika
                            //dto.status,
                    dto.startDate,
                    dto.endDate,
                    dto.NumberOfPeople);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest("Dodanie obiektu nie powiodło się" + ex.Message);
            }
        }

        [Route("/user/me/reservation")]
        [HttpGet]
        public async Task<IActionResult> GetMyAllReservations()
        {
            try
            {
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                List<Reservation> reservations = await _reservtionService.GetReservationsByUserId(userId); //GetPlacesByUserId(userId);

                if (reservations.Count == 0)
                {
                    return BadRequest("Ten użytkownik nie posiada jeszcze żadnej rezerwacji");
                }

                return Ok(reservations);
            }
            catch (Exception ex)
            {
                return BadRequest("Pobranie historii rezerwacji nie powiodło się" + ex.Message);
            }
        }

        //[Authorize(Roles = "Business")]
        //[Route("/user/me/place/reservation")]
        //[HttpGet]
        //public async Task<IActionResult> GetAllReservationsForMyPlaces()
        //{
        //    try
        //    {
        //        string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        //        List<Place> myPlaces = await _placeService.GetPlacesByUserId(userId);

        //        if (myPlaces.Count == 0)
        //        {
        //            return Ok("Ten użytkownik nie dodał jeszcze żadnego obiektu");
        //        }

        //        List<Reservation> reservations = await _reservtionService.GetReservationsForMyPlacesByPlaceIds(myPlaces);
        //        if (reservations.Count == 0)
        //        {
        //            return Ok("Ten użytkownik nie posiada jeszcze zarezerwowanego obiektu");
        //        }

        //        return Ok(reservations);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest("Pobranie rezerwacji dla Twoich obiektów nie powiodło się" + ex.Message);
        //    }
        //}

        [AllowAnonymous]
        [HttpGet("{id}/pdf")]
        public async Task<IActionResult> GeneratePdf(int id)
        {
            try
            {
                var resevation = await _reservtionService.GetReservationById(id);

                if (resevation is null)
                {
                    throw new Exception("Nie ma rezerwacji o podanym id");
                }

                var bytes = _pdfCreatorService.CreatePdf(resevation);

                _logger.LogInformation("Pozytywne pobranie pdf");

                return File(bytes, "application/pdf");
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Nie udało się pobrać pdf. Błąd: {ex.Message}");

                return BadRequest("Pobranie pliku pdf nie powiodło się" + ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetReservationById(int id)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Reservation reservation = await _reservtionService.GetReservationById(id);

            if (reservation is null)
            {
                return BadRequest("Ten użytkownik nie posiada jeszcze żadnej rezerwacji");
            }

            return Ok(reservation);
        }
    }
}