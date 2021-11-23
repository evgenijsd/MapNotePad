using MapNotePad.Enum;
using MapNotePad.Helpers.ProcessHelpers;
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
        Task<int> AddEditExecute(EAddEditType choise, PinModel pin);
        Task DeletePinAsync(ObservableCollection<PinView> collectPin, object contactObj);
        Task<ObservableCollection<PinView>> GetPinsViewAsync(int userId);
        void SetPinsFavouriteAsync(ObservableCollection<Pin> pins, ObservableCollection<PinView> pinviews);
        Task<AOResult> SetPinsAsync(ObservableCollection<Pin> pins, int userId);
        Task<AOResult<int>> SetPinsFavourite(PinModel pin);
        Task<AOResult<WeatherData>> GetWeather(double latitude, double longitude);
        Task<AOResult<ForecastData>> GetForecast(double latitude, double longitude);
        Task<AOResult<MapSpan>> CurrentLocation(MapSpan region);
    }
}
