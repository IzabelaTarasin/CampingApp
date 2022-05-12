using System;
using CampingApp_Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace CampingApp_Server.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class UserController : ControllerBase
	{
		private IUserService _userService;

		public UserController(IUserService userService)
		{
			_userService = userService;
		}

		[HttpGet]
		public async Task<IActionResult> GetUser(string name)
		{
			//wywolanie metody do pobrania uzytkownika
			var userResult = await _userService.GetUserByName(name);
			if (userResult == null)
			{
				return BadRequest("Nie powiodło się pobieranie użytkownika");
			}
			return Ok("Pobranie użytkownika przebiegło pomyślnie");
		}

	}
}

