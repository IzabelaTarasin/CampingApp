using System;
using System.Text;
using System.Text.Json;

namespace CampingApp.Services
{
	public interface IUserService
	{
		public Task<bool> CreateUser(string email, string password);
	}

	//klasa ktora bedze sluzyc do pzresyłania danych w ciele POST metoda CreateUser, DTO DataTransferObject
	public class CreateUserDTO
	{
		public string Email { get;}
		public string Password { get;}

		public CreateUserDTO(string email, string password)
        {
			Email = email;
			Password = password;
        }
	}

	public class UserService : IUserService
	{
		//dependency injection for httpclient from program.cs (dobra praktyka aby byly prywatne, czyli deklaracja zmiennej pod jaka bedzie przechpwywane i wstrzykniecie obektu jako parametr konsruktor a przypisane w konstruktorze do zmiennje
		private HttpClient _httpClient;

		public UserService(HttpClient httpClient)
        {
			_httpClient = httpClient;
        }

		public async Task<bool> CreateUser(string email, string password) // trzeba tez dac do applcati backendowej program.cs odpowiedni kod app.UseCors(x => x
            //.AllowAnyOrigin()
            //.AllowAnyMethod()
            //.AllowAnyHeader());

		{
			//CreateUserDTO createUserDTO = new CreateUserDTO(email, password);

			//tworzymy slownik
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
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return false;
			}


		}
	}

}

