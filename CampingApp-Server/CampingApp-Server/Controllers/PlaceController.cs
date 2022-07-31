using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CampingApp_Server.Database;
using CampingApp_Server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace CampingApp_Server.Controllers
{
    public record PlaceDTO(string name,
            string description,
            string imagePath,
            double pricePerDay,
            int maxPeople,
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

    [Authorize(Roles = "Business")]
    [ApiController]
    [Route("[controller]")]
    public class PlaceController : ControllerBase
    {
        private IPlaceService _placeService;

        public PlaceController(IPlaceService placeService)
        {
            _placeService = placeService;
        }

        [HttpPost]
        public async Task<IActionResult> AddPlace(PlaceDTO dto)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                var result = await _placeService.AddPlace(
                    userId, //przypisanie katualnie zalogowanego uzytkownika
                    dto.name,
                    dto.description,
                    dto.imagePath,
                    dto.pricePerDay,
                    dto.maxPeople,
                    dto.animalsAllowed,
                    dto.restaurantExist,
                    dto.receptionExist,
                    dto.medicExist,
                    dto.grillExist,
                    dto.wifiExist,
                    dto.swimmingpoolExist,
                    dto.postalCode,
                    dto.city,
                    dto.houseNumber,
                    dto.localNumber,
                    dto.street,
                    dto.voivodeship,
                    dto.country
                );

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest("Dodanie obiektu nie powiodło się" + ex.Message);
            }
        }
        [Route("/user/me/place")]
        [HttpGet]
        public async Task<IActionResult> MyGetAllPlaces()
        {
            try
            {
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                List<Place> places = await _placeService.GetPlacesByUserId(userId);

                if (places.Count == 0)
                {
                    return Ok("Ten użytkownik nie dodał jeszcze żadnego obiektu");
                }

                return Ok(places);
            }
            catch (Exception ex)
            {
                return BadRequest("Pobranie obiektów użytkownika nie powiodło się" + ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpGet("{placeId}")]
        public async Task<IActionResult> GetPlaceById(int placeId) //zwracam obiekt place  z dostepnym area i to bedzie wywietlane na ekranie szczegolow pola
        {
            try
            {
                Place place = await _placeService.GetPlaceById(placeId);

                if (place == null)
                {
                    throw new Exception("Brak obiektu o podanym id");
                }

                return Ok(place); //nie robie tak bo nie bede zwraca obiektu bezposrednio z bazy danych, potzrebuje utworzyc nowa klae (obiekt) ktory bedzie przetwarzac dane place i zwracac go bede ten npwo utworzony "placeResponse""
            }
            catch (Exception ex)
            {
                return BadRequest("Pobranie obiektu nie powiodło się" + ex.Message);
            }
        }


        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAllPlaces()
        {
            try
            {
                List<Place> places = await _placeService.GetAllPlaces();

                if (places.Count == 0)
                {
                    return Ok("Brak obiektów");
                }

                return Ok(places);
            }
            catch (Exception ex)
            {
                return BadRequest("Pobranie obiektów nie powiodło się" + ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpGet("/place/search")]
        public async Task<IActionResult> SearchPlaces(string startDate, string endDate, int numberOfPeople, string voivodeship)
        {
            try
            {
                var start = DateTime.Parse(startDate).ToUniversalTime();
                var end = DateTime.Parse(endDate).ToUniversalTime();

                if (end <= start) //DateTime to obiekt ktory nie ma zadnej wartosci przypisanej ale instieje i ma wszytskie pola ustawione na konkretna date 01/01/0001
                {
                    //minValue to ta data 01/01/0001
                    throw new Exception("Musisz wybrać minimum 1 dobę na wyjazd");
                }

                if (numberOfPeople <= 0) //DateTime to obiekt ktory nie ma zadnej wartosci przypisanej ale instieje i ma wszytskie pola ustawione na konkretna date 01/01/0001
                {
                    //minValue to ta data 01/01/0001
                    throw new Exception("Liczba osób na wyjazd musi wynosić minimum 1");
                }

                List<Place> places = await _placeService.Search(start, end, numberOfPeople, voivodeship);                

                return Ok(places);
            }
            catch (Exception ex)
            {
                return BadRequest("Pobranie obiektów nie powiodło się" + ex.Message);
            }
        }
    }
}

