using CampingApp_Server.Services;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CampingApp_Server.Database;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp;
using System.Collections;
using System.Text;

namespace CampingApp_Server.Controllers
{
    public record ReservationDTO( //nie przekazujemy usera bo jest przzekazywany "w locie" w metodzie
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

        public ReservationController(IReservtionService reservtionService, IPlaceService placeService, IPdfCreatorService pdfCreatorService)
        {
            _reservtionService = reservtionService;
            _placeService = placeService;
            _pdfCreatorService = pdfCreatorService;
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
                    return Ok("Ten użytkownik nie posiada jeszcze żadnej rezerwacji");
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
                var resevation = await _reservtionService.GetReservationsById(id);

                if (resevation is null)
                {
                    throw new Exception("Nie ma rezerwacji o podanym id");
                }

                var path = _pdfCreatorService.CreatePdf(resevation);
                
                return Ok(path); //bo zwracamy przegladarce sciezke do pobraania pliku (w heaaderze powinno byc w pasku aadresu)
            }
            catch (Exception ex)
            {
                return BadRequest("Pobranie pliku pdf nie powiodło się" + ex.Message);
            }
        }
    }
}