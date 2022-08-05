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
        Task<bool> AddPlace(
            string Name,
            string Description,
            string Image,
            double PricePerDay,
            int NumberOfAreas,
            bool AnimalsAllowed,
            bool RestaurantExist,
            bool ReceptionExist,
            bool MedicExist,
            bool GrillExist,
            bool WifiExist,
            bool SwimmingpoolExist,
            bool LaundryRoomExist,
            bool BikesExist,
            string PostalCode,
            string City,
            string HouseNumber,
            string LocalNumber,
            string Street,
            string Voivodeship,
            string Country);
        Task<List<PlaceModel>> GetMyPlaces();
        Task<List<PlaceModel>> GetPlaces();
        Task<PlaceModel> GetPlaceById(int placeId);
        Task<List<PlaceModel>> SearchPlaces(string searchString);
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
            string Image,
            double PricePerDay,
            int MaxPeople,
            bool AnimalsAllowed,
            bool RestaurantExist,
            bool ReceptionExist,
            bool MedicExist,
            bool GrillExist,
            bool WifiExist,
            bool SwimmingpoolExist,
            bool LaundryRoomExist,
            bool BikesExist,
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
                { "Image", Image},
                { "PricePerDay", PricePerDay},
                { "MaxPeople", MaxPeople},
                { "AnimalsAllowed", AnimalsAllowed},
                { "RestaurantExist", RestaurantExist},
                { "ReceptionExist", ReceptionExist},
                { "MedicExist", MedicExist},
                { "GrillExist", GrillExist},
                { "WifiExist", WifiExist},
                { "SwimmingpoolExist", SwimmingpoolExist},
                { "LaundryRoomExist", LaundryRoomExist},
                { "BikesExist", BikesExist},
                { "PostalCode", PostalCode },
                { "City", City },
                { "HouseNumber", HouseNumber},
                { "LocalNumber", LocalNumber},
                { "Street", Street},
                { "Voivodeship", Voivodeship},
                { "Country", Country}
            };

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
                //tutaj wysyłane do serwera aby mozna bylo pzretworzyc po stronei serwera i zapisac do bazy danych
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
            var request = new HttpRequestMessage(HttpMethod.Get, $"/place/{placeId}"); //interpolacja stringow // $"/place/{placeId}" to oznacza ze jest to kontroler i w tym przypadku jest to kontroler na serwerowej aplikacji PlaceController
            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Brak obiektu");
            }

            PlaceModel placeModel = await response.Content.ReadFromJsonAsync<PlaceModel>();

            return placeModel;

        }

        public async Task<List<PlaceModel>> SearchPlaces(string searchString) //metoda do wyszkiwania miejsc w 
        {
            //kompletuje zpaytanie do serwera z podanymi parametrami i je wyslać aby dostac dla turysty miejsca jakimi jest zainteresowany

            var request = new HttpRequestMessage(HttpMethod.Get, $"/place/search?{searchString}");
            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Brak obiektów");
            }

            List<PlaceModel> placeModel = await response.Content.ReadFromJsonAsync<List<PlaceModel>>();

            return placeModel;
        }

    }
}