using System.Net.Http.Json;
using System.Text.Json;

using Bunkograph.JNovelClubImport.DTOs;

using Microsoft.Extensions.Options;

namespace Bunkograph.JNovelClubImport
{
    internal class JNovelClubClient
    {
        private readonly HttpClient _httpClient;
        private readonly JNovelClubOptions _options;

        private string? _idToken;

        private static readonly JsonSerializerOptions s_serializerOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);

        public JNovelClubClient(HttpClient httpClient, IOptions<JNovelClubOptions> options)
        {
            _httpClient = httpClient;
            _options = options.Value;
        }

        public async Task LoginAsync()
        {
            LoginRequest? request = new LoginRequest(_options.UserName, _options.Password);
            HttpResponseMessage? message = await _httpClient.PostAsJsonAsync("https://labs.j-novel.club/app/v1/auth/login?format=json&slim=true", request, s_serializerOptions);
            message.EnsureSuccessStatusCode();
            LoginResult? response = await message.Content.ReadFromJsonAsync<LoginResult>(s_serializerOptions);
            _idToken = response?.Id;
        }

        public async Task<IEnumerable<Series>> GetSeriesAsync()
        {
            List<Series>? result = new List<Series>();

            string? baseUrl = "https://labs.j-novel.club/app/v1/series?format=json";
            SeriesResult? message = await _httpClient.GetFromJsonAsync<SeriesResult>(baseUrl, s_serializerOptions);
            while (message != null && !message.Pagination.LastPage)
            {
                result.AddRange(message.Series);
                message = await _httpClient.GetFromJsonAsync<SeriesResult>(baseUrl + "&skip=" + (message.Pagination.Skip + message.Pagination.Limit), s_serializerOptions);
            }
            if (message != null)
            {
                result.AddRange(message.Series);
            }

            return result;
        }

        public async Task<IEnumerable<Volume>> GetSeriesVolumesAsync(Series series)
        {
            List<Volume>? result = new List<Volume>();

            string? baseUrl = "https://labs.j-novel.club/app/v1/series/" + series.Slug + "/volumes?format=json";
            VolumesResult? message = await _httpClient.GetFromJsonAsync<VolumesResult>(baseUrl, s_serializerOptions);
            while (message != null && !message.Pagination.LastPage)
            {
                result.AddRange(message.Volumes);
                message = await _httpClient.GetFromJsonAsync<VolumesResult>(baseUrl + "&skip=" + (message.Pagination.Skip + message.Pagination.Limit), s_serializerOptions);
            }
            if (message != null)
            {
                result.AddRange(message.Volumes);
            }

            return result;
        }
    }
}
