using System.Net;

namespace ManageEngine.Api
{
    public class Notes
    {
        private readonly HttpClient _httpClient;
        private readonly Func<HttpResponseMessage, HttpStatusCode, Task> _handleAsErrorIfStatusIsNot;
        public Notes(HttpClient httpClient, Func<HttpResponseMessage, HttpStatusCode, Task> httpErrorHandler)
        {
            _httpClient = httpClient;
            _handleAsErrorIfStatusIsNot = httpErrorHandler;
        }

        public async Task<string> Add(long id, string noteJson)
        {
            var url = $"/api/v3/requests/{id}/notes";
            var parameters = new Dictionary<string, string> { { "input_data", $"{{\"note\":{noteJson}}}" } };
            var encodedContent = new FormUrlEncodedContent(parameters);
            var response = await _httpClient.PostAsync(url, encodedContent);
            await _handleAsErrorIfStatusIsNot(response, HttpStatusCode.Created);

            var responseContent = await response.Content.ReadAsStringAsync();

            return responseContent;
        }
    }
}
