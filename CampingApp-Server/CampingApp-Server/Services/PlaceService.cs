using System;
using CampingApp_Server.Database;
using Microsoft.AspNetCore.Identity;

namespace CampingApp_Server.Services
{
	public interface IPlaceService
	{
		public Task<bool> AddPlace(
			string Name,
			string Description,
			string ImagePath,
			double PricePerDay,
			bool AnimalsAllowed,
			bool RestaurantExist,
			bool ReceptionExist,
			bool MedicExist,
			bool GrillExist,
			bool WifiExist,
			bool SwimmingpoolExist);
	}
		public class PlaceService : IPlaceService
	{
		public IConfiguration _configuration;
		private UserService _userService;
		private ApplicationDbContext _applicationDbContext;

		public PlaceService(UserService userService, ApplicationDbContext applicationDbContext, IConfiguration configuration) //dependency - wstrzykiwany do konstruktora.
		{
			_userService = userService;
			_applicationDbContext = applicationDbContext;
			_configuration = configuration;
		}

		public async Task<bool> AddPlace(
			string name,
			string description,
			string imagePath,
			double pricePerDay,
			bool animalsAllowed,
			bool restaurantExist,
			bool receptionExist,
			bool medicExist,
			bool grillExist,
			bool wifiExist,
			bool swimmingpoolExist)
		{
			Place place = new Place
			{
				Name = name,
				Description = description,
				ImagePath = imagePath,
				PricePerDay = pricePerDay,
				AnimalsAllowed = animalsAllowed,
				RestaurantExist = restaurantExist,
				ReceptionExist = receptionExist,
				MedicExist = medicExist,
				GrillExist = grillExist,
				WifiExist = wifiExist,
				SwimmingpoolExist = swimmingpoolExist
			};

			//wstrzyknac do tego placeservice application db context
			//app db context . places.addasync i przekazac obiekt place
			//savecontect by zapisac do bazki
			_userService.

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
	}
}

