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
            bool animalsAllowed,
            bool restaurantExist,
            bool receptionExist,
            bool medicExist,
            bool grillExist,
            bool wifiExist,
            bool swimmingpoolExist);

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
                    dto.animalsAllowed,
                    dto.restaurantExist,
                    dto.receptionExist,
                    dto.medicExist,
                    dto.grillExist,
                    dto.wifiExist,
                    dto.swimmingpoolExist);

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

                return Ok(places);
            }
            catch (Exception ex)
            {
                return BadRequest("Pobranie obiektów użytkownika nie powiodło się" + ex.Message);
            }
        }
    }
}

