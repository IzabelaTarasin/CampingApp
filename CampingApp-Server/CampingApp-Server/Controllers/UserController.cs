using System;
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

	}
}

