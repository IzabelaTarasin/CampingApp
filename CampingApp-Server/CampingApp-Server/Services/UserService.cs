using System;
using Microsoft.AspNetCore.Identity;
using CampingApp_Server.Database;

namespace CampingApp_Server.Services
{
	public interface IUserService
    {
		public Task<bool> CreateUser(string email, string password);
		public Task<User> GetUserByName(string name);
	}

	public class UserService : IUserService
	{
		private UserManager<User> _userManager;

		public UserService(UserManager<User> userManager) //dependency - wstrzykiwany do konstruktora.
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

            if (!result.Succeeded)
            {
				return false;
            }

			//przypanie roli
			var resultRole = await _userManager.AddToRoleAsync(user, "standard");

			if (!resultRole.Succeeded)
			{
				return false;
			}

			return true;

		}

		public async Task<User> GetUserByName(string name) //bo name to email ktory jest unkalny wiec mozna szukac po name
        {
			User user = await _userManager.FindByNameAsync(name);
            if (user == null)
            {
				return null;
			}

			return user;
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
