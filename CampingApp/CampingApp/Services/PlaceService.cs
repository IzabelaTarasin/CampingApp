using System;
using System.Text;
using System.Text.Json;
using Blazored.LocalStorage;

namespace CampingApp.Services
{
	public interface IPlaceService
	{
		public Task<bool> AddPlace(int Id,
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
		public Task<bool> GetAllPlaces();
	}

	public class PlaceService : IPlaceService
	{
		private HttpClient _httpClient;
		private ILocalStorageService _localStorage;

		public PlaceService(HttpClient httpClient, ILocalStorageService localStorage)
		{
			_httpClient = httpClient;
			_localStorage = localStorage;
		}

		public async Task<bool> AddPlace(int Id,
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
			bool SwimmingpoolExist)
		{
			//tworzymy slownik aby zrobic json
			var data = new Dictionary<object, object>
            {	
				{ "Name", Name },
				{ "Description", Description },
				{ "ImagePath", ImagePath},
				{ "PricePerDay", PricePerDay},
				{ "AnimalsAllowed", AnimalsAllowed},
				{ "RestaurantExist", RestaurantExist},
				{ "ReceptionExist", ReceptionExist},
				{ "MedicExist", MedicExist},
				{ "GrillExist", GrillExist},
				{ "WifiExist", WifiExist},
				{ "SwimmingpoolExist", SwimmingpoolExist}
			};

			//zamiana na format json:
			var json = JsonSerializer.Serialize(data);

			//tworze zapytana http restowe, potrzebuje klienta zapytan sieciowych http
			var request = new HttpRequestMessage(HttpMethod.Post, "newobject");
			request.Content = new StringContent(json, Encoding.UTF8, "application/json");
			try
			{
				var result = await _httpClient.SendAsync(request);

				Console.WriteLine("result: " + result);
				return true;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return false;
			}
		}

		public async Task<bool> GetAllPlaces()
		{
			var request = new HttpRequestMessage(HttpMethod.Get, "/user/me");
			var token = await _localStorage.GetItemAsync<string>("token");

			if (string.IsNullOrWhiteSpace(token))
			{
				throw new Exception("Token is null");
			}

			Console.WriteLine("token" + token);

			request.Headers.Add("Authorization", "Bearer " + token); //dodawanie naglowka od zapytania


			//wykonanie zapytanie o me
			var response = await _httpClient.SendAsync(request);

			if (!response.IsSuccessStatusCode)
			{
				throw new Exception("Brak inf o aktualnie zalogowanym uzytkowniku");
			}

			List<Place> place = await response.Content.ReadFromJsonAsync<User>();
			if (place == null)
			{
				throw new Exception("Brak uzytwkonika stworzonwego zJSONa");
			}
			return place;
		}
	}

}