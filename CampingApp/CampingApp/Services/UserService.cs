using System;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Blazored.LocalStorage;

namespace CampingApp.Services
{
	//tworzymy rekod abysmy mogli pardsowac do niego odpowiedz z serwera
	//odpowiedz z serwera to jest format json ktory zawiera email i id wiec
	//zmienna rekord musi umozliwic zapisanie id i email
	//potrzebne do metody getme
	public record User(string Id, string Email, List<string> Roles);

	public interface IUserService
	{
		public Task<bool> CreateUser(string email, string password);
		public Task<bool> SignInUser(string email, string password);
		public Task<User> GetMe();
		public Task Logout();

	}

	public class UserService : IUserService
	{
		//dependency injection for httpclient from program.cs (dobra praktyka aby byly prywatne, czyli deklaracja zmiennej pod jaka bedzie przechpwywane i wstrzykniecie obektu jako parametr konsruktor a przypisane w konstruktorze do zmiennje
		private HttpClient _httpClient;
		private ILocalStorageService _localStorage;

		public UserService(HttpClient httpClient, ILocalStorageService localStorage)
        {
			_httpClient = httpClient;
			_localStorage = localStorage;
        }

		public async Task<bool> CreateUser(string email, string password) // trzeba tez dac do applcati backendowej program.cs odpowiedni kod app.UseCors(x => x
            //.AllowAnyOrigin()
            //.AllowAnyMethod()
            //.AllowAnyHeader());

		{
			//tworzymy slownik aby zrobic json
			var data = new Dictionary<string, string>
			{
				{ "email", email },
				{ "password", password }
			};

			//zamiana na format json:
			var json = JsonSerializer.Serialize(data);
			

			//tworze zapytana http restowe, potrzebuje klienta zapytan sieciowych http
			var request = new HttpRequestMessage(HttpMethod.Post, "SignUp");
			request.Content = new StringContent(json, Encoding.UTF8, "application/json");
			try
			{
				var result = await _httpClient.SendAsync(request);

				Console.WriteLine("result: " + result);
				return true;
			}
			catch(Exception ex)
			{
				Console.WriteLine(ex.Message);
				return false;
			}


		}

		public async Task<bool> SignInUser(string email, string password)
        {
			var data = new Dictionary<string, string>
			{
				{ "email", email },
				{ "password", password }
			};

			//zamiana na format json:
			var json = JsonSerializer.Serialize(data);

			var request = new HttpRequestMessage(HttpMethod.Post, "signin");
			request.Content = new StringContent(json, Encoding.UTF8, "application/json");

            try
			{
				var result = await _httpClient.SendAsync(request);

				var token = await result.Content.ReadAsStringAsync();
                if (string.IsNullOrWhiteSpace(token))
                {
					throw new Exception("Token is null");
				}

				await _localStorage.SetItemAsync("token", token); //przekazanie tokena dla utrzymania "sesji", klucz wartosc - parametry

				Console.WriteLine("result: " + token);
				return true;

			}
			catch(Exception ex)
            {
				Console.WriteLine(ex.Message);
				return false;
			}
		}

		public async Task<User> GetMe()
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

            if(!response.IsSuccessStatusCode)
			{
				throw new Exception("Brak inf o aktualnie zalogowanym uzytkowniku");
			}

			//odczyta odp z serwera i sprobuje stworzyc obiekt user z polami id i email

			var user = await response.Content.ReadFromJsonAsync<User>();
			if(user == null)
            {
				throw new Exception("Brak uzytwkonika stworzonwego zJSONa");
            }
			return user;
		}

		public async Task Logout()
		{
			//czyszczenie tokena i nawigujemy do strony glownej aby to uczycnic musimy wstrzyknac localStorage(odnosi sie do specjalnego miejsca po stronie klienta gdzie mozna keszowac rzeczy i sa w stanie przetwwac zamkniecie okna) i wstrzyknac navigation manager ktory pozwoli na nawigacje z poziomu kodu a nie z navlinka            
			//czyszczenie przez localstorage:
			await _localStorage.RemoveItemAsync("token");

		}
	}

}

