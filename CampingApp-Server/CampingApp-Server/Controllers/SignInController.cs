using System;
using CampingApp_Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace CampingApp_Server.Controllers
{
	public record SignInDTO(string email, string password);

	[ApiController]
	[Route("[controller]")]
	public class SignInController : ControllerBase
	{
		private IUserService _userService;

		public SignInController(IUserService userService)
		{
			_userService = userService;
		}

		[HttpPost]
		public async Task<IActionResult> SignIn(SignInDTO dto)
		{
			//wywolanie metody do logowania uzytkownika
			var result = await _userService.SignIn(dto.email, dto.password);
			//korzystam z usermanager
			if (result == null)
			{
				return BadRequest("Logowanie nie powiodło się");
			}
			return Ok("Logowanie przebiegło pomyślnie");

		}
	}
}

