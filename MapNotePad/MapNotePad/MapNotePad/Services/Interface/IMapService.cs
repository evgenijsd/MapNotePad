using MapNotePad.Enum;
using MapNotePad.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.GoogleMaps;

namespace MapNotePad.Services.Interface
{
    public interface IMapService
    {
        Task<int> AddEditExecute(AddEditType choise, PinModel pin);
        Task DeletePinAsync(ObservableCollection<PinView> collectPin, object contactObj);
        Task<ObservableCollection<PinView>> GetPinsViewAsync(int userId);
        void SetPinsFavouriteAsync(ObservableCollection<Pin> pins, ObservableCollection<PinView> pinviews);
        Task SetPinsAsync(ObservableCollection<Pin> pins, int userId);
        public Task<int> SetPinsFavourite(PinModel pin);
        public Task<WeatherData> GetWeather(double latitude, double longitude);
        public Task<ForecastData> GetForecast(double latitude, double longitude);
    }
}
