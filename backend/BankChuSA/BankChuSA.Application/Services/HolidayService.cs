using BankChuSA.Application.DTOs;
using BankChuSA.Application.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;

namespace BankChuSA.Application.Services
{
    public class HolidayService : IHolidayService
    {
        private readonly HttpClient _httpClient;
        private readonly IMemoryCache _cache;

        public HolidayService(IHttpClientFactory httpClientFactory, IMemoryCache cache)
        {
            _httpClient = httpClientFactory.CreateClient();
            _cache = cache;
        }

        public async Task<bool> IsWorkingDayAsync(DateTime date)
        {
            if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
            {
                return false;
            }

            var holidays = await GetHolidaysAsync(date.Year);
            return !holidays.Any(h => h.Date == date.Date);
        }

        public async Task<List<DateTime>> GetHolidaysAsync(int year)
        {
            var cacheKey = $"Holidays_{year}";

            if (_cache.TryGetValue<List<DateTime>>(cacheKey, out var cachedHolidays))
            {
                return cachedHolidays!;
            }

            try
            {
                var response = await _httpClient.GetAsync($"https://brasilapi.com.br/api/feriados/v1/{year}");

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var holidays = JsonSerializer.Deserialize<List<HolidayApiResponse>>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    var dates = holidays?.Select(h => h.Date).ToList() ?? new List<DateTime>();

                    _cache.Set(cacheKey, dates, TimeSpan.FromHours(24));

                    return dates;
                }
            }
            catch
            {
            }

            return new List<DateTime>();
        }

    }
}

