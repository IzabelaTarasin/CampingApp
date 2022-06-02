using System;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Blazored.LocalStorage;
using CampingApp.Domain;

namespace CampingApp.Services
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
            bool SwimmingpoolExist,
            string PostalCode,
            string City,
            string HouseNumber,
            string LocalNumber,
            string Street,
            string Voivodeship,
            string Country);
        public Task<List<PlaceModel>> GetMyPlaces();
        public Task<List<PlaceModel>> GetPlaces();
        public Task<PlaceModel> GetPlaceById(int placeId);
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

        public async Task<bool> AddPlace(
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
            bool SwimmingpoolExist,
            string PostalCode,
            string City,
            string HouseNumber,
            string LocalNumber,
            string Street,
            string Voivodeship,
            string Country)
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
                { "SwimmingpoolExist", SwimmingpoolExist},
                { "PostalCode", PostalCode },
                { "City", City },
                { "HouseNumber", HouseNumber},
                { "LocalNumber", LocalNumber},
                { "Street", Street},
                { "Voivodeship", Voivodeship},
                { "Country", Country}
            };

            foreach (var pair in data)
            {
                Console.WriteLine($"{pair.Key} {pair.Value}");
            }

            //zamiana na format json:
            var json = JsonSerializer.Serialize(data);

            //tworze zapytana http restowe, potrzebuje klienta zapytan sieciowych http
            var request = new HttpRequestMessage(HttpMethod.Post, "place");
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

        public async Task<List<PlaceModel>> GetMyPlaces()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "/user/me/place");
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

            List<PlaceModel> placeModel = await response.Content.ReadFromJsonAsync<List<PlaceModel>>();

            return placeModel;
        }

        public async Task<List<PlaceModel>> GetPlaces()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "/place");
            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Brak obiektów");
            }

            List<PlaceModel> placeModel = await response.Content.ReadFromJsonAsync<List<PlaceModel>>();

            return placeModel;

        }

        public async Task<PlaceModel> GetPlaceById(int placeId)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"/place/{placeId}"); //interpolacja stringow
            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Brak obiektu");
            }

            PlaceModel placeModel = await response.Content.ReadFromJsonAsync<PlaceModel>();

            return placeModel;

        }


    }
}