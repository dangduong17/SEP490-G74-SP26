using System.Text.Json;
using vn.edu.fpt.dto;
using vn.edu.fpt.service.Interfaces;

namespace vn.edu.fpt.service.Implementations
{
    public class LocationLookupService : ILocationLookupService
    {
        private readonly HttpClient _httpClient;

        public LocationLookupService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<ProvinceLookupDto>> GetProvincesAsync()
        {
            using var response = await _httpClient.GetAsync("/api/v2/");
            response.EnsureSuccessStatusCode();
            await using var stream = await response.Content.ReadAsStreamAsync();
            var root = await JsonSerializer.DeserializeAsync<List<ProvinceApiResponse>>(stream, JsonOptions);

            return root?
                .Select(x => new ProvinceLookupDto { Code = x.Code, Name = x.Name ?? string.Empty })
                .OrderBy(x => x.Name)
                .ToList() ?? new List<ProvinceLookupDto>();
        }

        public async Task<List<WardLookupDto>> GetWardsByProvinceCodeAsync(int provinceCode)
        {
            using var response = await _httpClient.GetAsync($"/api/v2/p/{provinceCode}?depth=2");
            response.EnsureSuccessStatusCode();
            await using var stream = await response.Content.ReadAsStreamAsync();
            var root = await JsonSerializer.DeserializeAsync<ProvinceWithWardsApiResponse>(stream, JsonOptions);

            return root?.Wards?
                .Select(x => new WardLookupDto { Code = x.Code, Name = x.Name ?? string.Empty })
                .OrderBy(x => x.Name)
                .ToList() ?? new List<WardLookupDto>();
        }

        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        private sealed class ProvinceApiResponse
        {
            public int Code { get; set; }
            public string? Name { get; set; }
        }

        private sealed class ProvinceWithWardsApiResponse
        {
            public List<WardApiResponse>? Wards { get; set; }
        }

        private sealed class WardApiResponse
        {
            public int Code { get; set; }
            public string? Name { get; set; }
        }
    }
}
