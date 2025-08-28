using Newtonsoft.Json;
using SmartMealApiClient.Models;
using System.Net.Http.Headers;
using System.Text;

namespace SmartMealApiClient.Services
{
    public class HttpApiClient : IHttpApiClient
    {
        private readonly string _url;
        private readonly string _username;
        private readonly string _password;
        private readonly HttpClient _client;

        public HttpApiClient(string url, string username, string password)
        {
            _url = url;
            _username = username;
            _password = password;
            _client = new HttpClient();

            var authToken = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_username}:{_password}"));
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authToken);
        }

        public async Task<List<MenuItem>> GetMenuAsync(bool withPrice)
        {
            var requestBody = new GetMenuRequest(withPrice);

            var jsonRequest = JsonConvert.SerializeObject(requestBody);
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            //По заданию для всех запросов один общий endpoint.
            var response = await _client.PostAsync(_url, content);
            var jsonResponse = await response.Content.ReadAsStringAsync();

            var serverResponse = JsonConvert.DeserializeObject<GetMenuItemsServerResponse>(jsonResponse);

            if (serverResponse == null || serverResponse.Data == null || !serverResponse!.Success)
            {
                throw new Exception($"Ошибка: {serverResponse?.ErrorMessage ?? "Неизвестная ошибка"}");
            }

            return serverResponse.Data;
        }

        public async Task<bool> SendOrderAsync(string orderId, List<OrderItem> orderItems)
        {
            var requestBody = new SendOrderRequest(orderId, orderItems);

            var jsonRequest = JsonConvert.SerializeObject(requestBody);
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync(_url, content);
            var jsonResponse = await response.Content.ReadAsStringAsync();

            var serverResponse = JsonConvert.DeserializeObject<ServerResponse>(jsonResponse);

            if (serverResponse == null || !serverResponse.Success)
            {
                throw new Exception($"Ошибка: {serverResponse?.ErrorMessage ?? "Неизвестная ошибка"}");
            }
                
            return serverResponse.Success;
        }
    }
}
