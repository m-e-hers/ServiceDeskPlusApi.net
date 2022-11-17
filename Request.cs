using System.Net;

namespace ManageEngine.Api
{
    public class Request
    {
        private readonly HttpClient _httpClient;
        private readonly Func<HttpResponseMessage, HttpStatusCode, Task> _handleAsErrorIfStatusIsNot;

        public readonly Notes Notes;
        public Request(HttpClient httpClient, Func<HttpResponseMessage, HttpStatusCode, Task> httpErrorHandler)
        {
            _httpClient = httpClient;
            _handleAsErrorIfStatusIsNot = httpErrorHandler;

            Notes = new(_httpClient, _handleAsErrorIfStatusIsNot);
        }
        public string GetUrl(long id)
        {
            return _httpClient.BaseAddress!.ToString() + Constants.WorkOrderPath + id.ToString();
        }
        public async Task<string> View(long id)
        {
            var url = $"/api/v3/requests/{id}";
            var response = await _httpClient.GetAsync(url);
            await _handleAsErrorIfStatusIsNot(response, HttpStatusCode.OK);

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> Update(long id, string requestJson)
        {
            var url = $"/api/v3/requests/{id}";
            var parameters = new Dictionary<string, string> { { "input_data", $"{{\"request\":{requestJson}}}" } };
            var encodedContent = new FormUrlEncodedContent(parameters);

            var response = await _httpClient.PutAsync(url, encodedContent);
            await _handleAsErrorIfStatusIsNot(response, HttpStatusCode.OK);

            return await response.Content.ReadAsStringAsync();
        }
    }
}
