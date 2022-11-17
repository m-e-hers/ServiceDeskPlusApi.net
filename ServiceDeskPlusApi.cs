using ManageEngine.Api;
using System.Net;
using System.Net.Http.Headers;

namespace ManageEngine
{
    public class ServiceDeskPlusApi
    {
        private readonly HttpClient _httpClient;
        private readonly Func<HttpResponseMessage, HttpStatusCode, Task> _httpErrorHandler;
        public readonly Request Request;

        public ServiceDeskPlusApi(string url, string technicianKey) : this(url, technicianKey, null) { }
        public ServiceDeskPlusApi(string url, string technicianKey, Func<HttpResponseMessage, HttpStatusCode, Task>? httpErrorHandler)
        {
            _httpClient = new();
            _httpClient.BaseAddress = new Uri(url);
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.DefaultRequestHeaders.Add("TECHNICIAN_KEY", technicianKey);
            _httpErrorHandler = httpErrorHandler ?? HandleAsErrorIfStatusIsNot; //HandleErrorHttpResponse;

            Request = new(_httpClient, _httpErrorHandler);
        }
        private async Task HandleAsErrorIfStatusIsNot(HttpResponseMessage httpResponseMessage, HttpStatusCode httpStatusCode)
        {
            if (httpResponseMessage.StatusCode != httpStatusCode)
                throw new HttpRequestException("\nError Response From ServiceDesk Plus RestApi: \n" + await httpResponseMessage.Content.ReadAsStringAsync());
        }
        public async Task<byte[]> Download(string url)
        {
            using var response = await _httpClient.GetAsync(url);
            await _httpErrorHandler(response, HttpStatusCode.OK);
            byte[] fileBytes = await response.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
            return fileBytes;
        }
    }
}
