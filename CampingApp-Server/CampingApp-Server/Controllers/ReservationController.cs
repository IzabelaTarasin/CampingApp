using System;
using CampingApp_Server.Database;
using CampingApp_Server.Services;
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
    public record ReservationDTO(
        //int placeId,
        //User user,
        //Status status,
        string userId,
        string startDate,
        string endDate);

    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ReservationController : ControllerBase
    {
        private IReservtionService _reservtionService;

        public ReservationController(ReservtionService reservtionService)
        {
            _reservtionService = reservtionService;
        }

        [HttpPost]
        public async Task<IActionResult> AddReservation(ReservationDTO dto)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                //tu wyciagnac kim jest user i jak jesgo pole to narka nie mozna rezerwowac

                var result = await _reservtionService.AddReservation(
                    //placeId,
                    userId, //przypisanie katualnie zalogowanego uzytkownika
                    //dto.status,
                    dto.startDate,
                    dto.endDate);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest("Dodanie obiektu nie powiodło się" + ex.Message);
            }
        }
    }
}

