using System;
using System.Security.Claims;
using CampingApp_Server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CampingApp_Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("{id}")] //query param do nauki , query sting
        public async Task<IActionResult> GetUser(string id)
        {
            //wywolanie metody do pobrania uzytkownika
            var userResult = await _userService.GetUserById(id);
            if (userResult == null)
            {
                return BadRequest("Nie powiodło się pobieranie użytkownika");
            }
            return Ok("Pobranie użytkownika przebiegło pomyślnie");
        }

        //metoda ktora zwroci aktualnie zalogowanego uzytkownika
        [HttpGet("me")]
        public async Task<IActionResult> GetMe()
        {
            //sprawdzamy czy istneije juz zalogowany uzytkownik
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            //probujemy wyciagnac z bazy danych uzytkownika
            var user = await _userService.GetUserById(userId);

            if (user == null)
            {
                return BadRequest("Nie powiodło się pobieranie użytkownika");
            }

            var roles = await _userService.GetRolesForUserId(user.Id);

            //zwracamy obiekt ananomowy - na razie
            return Ok(new
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                PhoneNumber = user.PhoneNumber,
                Roles = roles //gdy ktos zpayta o me to serwer odpwoie id, email i role jakie ten zalogowany uzytkownik posiada
            });
        }

    }
}

