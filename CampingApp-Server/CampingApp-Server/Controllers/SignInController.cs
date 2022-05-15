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
            try
            {
				//wywolanie metody do logowania uzytkownika
				var resultToken = await _userService.SignIn(dto.email, dto.password);
	
				return Ok(resultToken);

			}
            catch (Exception ex)
            {
				return BadRequest("Logowanie nie powiodło się" + ex.Message);
			}

		}
	}
}

