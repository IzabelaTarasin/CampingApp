using System;
using Microsoft.AspNetCore.Identity;
using CampingApp_Server.Database;

namespace CampingApp_Server.Services
{
	public interface IUserService
    {
		public Task<bool> CreateUser(string email, string password);

	}

	public class UserService : IUserService
	{
		private UserManager<User> _userManager;

		public UserService(UserManager<User> userManager)
		{
			_userManager = userManager;
		}

		public async Task<bool> CreateUser(string email, string password)
        {
			User user = new User {
				Email = email,
				UserName = email
			};

			var result = await _userManager.CreateAsync(user, password); //nastepuje tworzenie uzytkownika

            if (result.Succeeded)
            {
				return true;
            }

			return false;
        }
	}
}
//teraz dependencynjectons:
//pamietac aby dodac do program.cs -> builder.Services.AddScoped<IUserService, UserService>();
//i w signUpconttr. -> private IUserService _userService;

//public SignUpController(IUserService userService)
//{
//	_userService = userService;
//}
