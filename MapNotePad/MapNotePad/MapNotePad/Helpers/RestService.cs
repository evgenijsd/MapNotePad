using MapNotePad.Helpers.ProcessHelpers;
using MapNotePad.Models;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace MapNotePad.Helpers
{
    public class RestService
    {
        HttpClient _client;

        public RestService()
        {
            _client = new HttpClient();
        }

        public async Task<AOResult<WeatherData>> GetWeatherData(string query)
        {
            var result = new AOResult<WeatherData>();
            try
            {
                var response = await _client.GetAsync(query);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var weatherData = JsonConvert.DeserializeObject<WeatherData>(content);
                    result.SetSuccess(weatherData);
                }
                else
                    result.SetFailure();
            }
            catch (Exception ex)
            {
                result.SetError($"Exception: {nameof(GetWeatherData)}", "Wrong result", ex);
            }

            return result;
        }

        public async Task<AOResult<ForecastData>> GetForecastData(string query)
        {
            var result = new AOResult<ForecastData>();
            try
            {
                var response = await _client.GetAsync(query);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var forecastData = JsonConvert.DeserializeObject<ForecastData>(content);
                    result.SetSuccess(forecastData);
                }
            }
            catch (Exception ex)
            {
                result.SetError($"Exception: {nameof(GetForecastData)}", "Wrong result", ex);
            }

            return result;
        }
    }
}
