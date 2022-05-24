using System;
using Blazored.LocalStorage;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using CampingApp.Domain;

namespace CampingApp.Services
{
	public interface IReservationService
	{
		public Task<bool> AddReservation(
			int PlaceId,
			//UserModel UserModel,
			//StatusModel StatusModel,
			DateTime StartDate,
			DateTime EndDate);

	}

	public class ReservationService : IReservationService
	{
		private HttpClient _httpClient;
		private ILocalStorageService _localStorage;

		public ReservationService(HttpClient httpClient, ILocalStorageService localStorage)
		{
			_httpClient = httpClient;
			_localStorage = localStorage;
		}

		public async Task<bool> AddReservation(
			int PlaceId,
			//UserModel UserModel,
			//StatusModel StatusModel,
			DateTime StartDate,
			DateTime EndDate)
		{
			//tworzymy slownik aby zrobic json
			var data = new Dictionary<object, object>
			{
				{ "PlaceId", PlaceId },
				{ "StartDate", StartDate },
				{ "EndDate", EndDate}
			};

			//zamiana na format json:
			var json = JsonSerializer.Serialize(data);

			//tworze zapytana http restowe, potrzebuje klienta zapytan sieciowych http
			var request = new HttpRequestMessage(HttpMethod.Post, "reservation");
			request.Content = new StringContent(json, Encoding.UTF8, "application/json");

			var token = await _localStorage.GetItemAsync<string>("token");

			if (string.IsNullOrWhiteSpace(token))
			{
				throw new Exception("Token is null");
			}

			request.Headers.Add("Authorization", "Bearer " + token); //dodawanie naglowka od zapytania

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

		public async Task<List<ReservationModel>> GetMyReservation()
        {
			var request = new HttpRequestMessage(HttpMethod.Get, "/user/me/reservation");
			var token = await _localStorage.GetItemAsync<string>("token");

			if (string.IsNullOrWhiteSpace(token))
			{
				throw new Exception("Token is null");
			}

			request.Headers.Add("Authorization", "Bearer " + token); //dodawanie naglowka od zapytania


			//wykonanie zapytanie o me
			var response = await _httpClient.SendAsync(request);


			if (!response.IsSuccessStatusCode)
			{
				throw new Exception("Brak inf o aktualnie zalogowanym uzytkowniku");
			}

			List<ReservationModel> reservationModels = await response.Content.ReadFromJsonAsync<List<ReservationModel>>();

			return reservationModels;
		}

	}
}

