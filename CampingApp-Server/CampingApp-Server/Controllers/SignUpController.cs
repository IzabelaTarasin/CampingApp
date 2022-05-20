using System;
using CampingApp_Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace CampingApp_Server.Controllers
{
	public record SignUpDTO(string name, string phoneNumber, string email, string password);

	[ApiController]
	[Route("[controller]")]
	public class SignUpController : ControllerBase //musii dzedziczyc
	{
		private IUserService _userService;

		public SignUpController(IUserService userService)
        {
			_userService = userService;
        }
		//udostepmiany metode do rejestracji, najczesniej rejestruje sie uzytkownikow uzywajac metody POST
		[HttpPost]
		public async Task<IActionResult> SignUp(SignUpDTO dto)
        {
			//wywolanie metody do tworzenia uzytkownika
			var result = await _userService.CreateUser(dto.name, dto.phoneNumber, dto.email, dto.password);
			//kkorzystam z usermanager
			if (result) {
				return Ok("Rejestracja przebiegła pomyślnie");
			}
			return BadRequest("Nie powiodła się rejestracja");
        }

	}
}

