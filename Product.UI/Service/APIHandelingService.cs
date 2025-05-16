
namespace Product.UI.Service
{
    public class APIHandelingService
    {
        private readonly HttpClient _httpClient;

        public APIHandelingService(IHttpClientFactory factory)
        {
            _httpClient = factory.CreateClient("ApiClient");
        }

        public async Task<(T, string)> getData<T>(string apiEndpoint)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, apiEndpoint);

            var response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    var data = await response.Content.ReadFromJsonAsync<T>();
                    return (data, "");
                }
                catch (Exception ex)
                {
                    return (default(T), ex.Message);
                }
            }
            else
            {
                var data = await response.Content.ReadFromJsonAsync<T>();
                return (data, "");
            }
        }

        public async Task<string> deleteData(int id,string apiEndpoint)
        {
            var error = "";

            var request = new HttpRequestMessage(HttpMethod.Delete, apiEndpoint);

            var response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    return "";
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
            else
            {
                error = await response.Content.ReadAsStringAsync();
                return error;
            }
        }

        public async Task<(T, string)> PostData<T>(T data, string apiEndpoint)
        {
            var error = "";

            var request = new HttpRequestMessage(HttpMethod.Post, apiEndpoint) { Content = JsonContent.Create(data) };

            var response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    return (await response.Content.ReadFromJsonAsync<T>(), "");
                }
                catch (Exception ex)
                {
                    return (default(T), ex.Message);
                }
            }
            else
            {
                error = await response.Content.ReadAsStringAsync();
                return (default(T), error);
            }
        }
    }
}
