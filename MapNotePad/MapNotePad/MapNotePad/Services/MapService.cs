using MapNotePad.Enum;
using MapNotePad.Helpers;
using MapNotePad.Models;
using MapNotePad.Services.Interface;
using MapNotePad.Services.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace MapNotePad.Services
{
    public class MapService : IMapService
    {
        private IRepository _repository { get; }
        RestService _restService { get; }

        public MapService(IRepository repository, RestService restService)
        {
            _repository = repository;
            _restService = restService;
        }

        public async Task DeletePinAsync(ObservableCollection<PinView> collectPin, object pintObj)
        {
            PinView pin = pintObj as PinView;
            PinModel pindel = pin.ToPinModel();
            await _repository.RemoveAsync<PinModel>(pindel);
            collectPin.Remove(pin);
        }

        /*public async Task FavouritePinAsync(ObservableCollection<PinView> collectPin, object pintObj)
        {
            PinView pin = pintObj as PinView;
            PinModel pindel = pin.ToPinModel();
            await _repository.RemoveAsync<PinModel>(pindel);
            collectPin.Remove(pin);
        }*/

        public async Task<int> AddEditExecute(AddEditType choise, PinModel pin)
        {
            int result = 0;
            switch (choise)
            {
                case AddEditType.Add:
                    result = await _repository.AddAsync(pin);
                    break;
                case AddEditType.Edit:
                    result = await _repository.UpdateAsync(pin);
                    break;
                default:
                    break;
            }
            return result;
        }

        public async Task<ObservableCollection<PinView>> GetPinsViewAsync(int userId) { 
            var pv = new ObservableCollection<PinView>((await _repository.GetAsync<PinModel>(x => x.User == userId)).Select(x => x.ToPinView()));
            foreach (PinView pin in pv)
                if (pin.Favourite) pin.Image = "ic_like_blue.png";
                else pin.Image = "ic_like_gray.png";
            return pv;
        }

        public void SetPinsFavouriteAsync(ObservableCollection<Pin> pins, ObservableCollection<PinView> pinviews)
        {
            foreach (PinView pinview in pinviews)
            {
                var pin = pinview.ToPin();
                pin.Icon = BitmapDescriptorFactory.FromBundle("ic_placeholder.png");
                if (pinview.Favourite) pins?.Add(pin);
            }
        }

        public async Task SetPinsAsync(ObservableCollection<Pin> pins, int userId)
        {
            var pm = new ObservableCollection<PinModel>(await _repository.GetAsync<PinModel>(x => x.User == userId));
            foreach (PinModel pinm in pm)
            {
                var pin = pinm.ToPin();
                pin.Icon = BitmapDescriptorFactory.FromBundle("ic_placeholder.png");
                pins?.Add(pin);
            }
        }

        public async Task<int> SetPinsFavourite(PinModel pin)
        {
            int result = 0;
            result = await _repository.UpdateAsync(pin);
            return result;
        }

        //api.openweathermap.org/data/2.5/weather?lat={lat}&lon={lon}&appid={API key}
        public async Task<ForecastData> GetForecast(double latitude, double longitude)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            string requestUri = AppConstants.OpenWeatherMapEndpoint;
            requestUri += $"?lat={latitude}";
            requestUri += $"&lon={longitude}";
            requestUri += "&exclude=minutely,hourly,alerts,current";
            requestUri += "&units=metric"; 
            requestUri += $"&appid={AppConstants.OpenWeatherMapAPIKey}";
            ForecastData forecastData = await _restService.GetForecastData(requestUri);//https://api.openweathermap.org/data/2.5/onecall?lat=12&lon=12&exclude=minutely,hourly,alerts,current&units=metric&appid=6a13cd8fbbd77a77ff4666d8b6ac1336
            return forecastData;
        }

        public async Task<WeatherData> GetWeather(double latitude, double longitude)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            string requestUri = AppConstants.OpenWeatherMapEndpoint;
            requestUri += $"?lat={latitude}";
            requestUri += $"&lon={longitude}";
            requestUri += "&units=metric";
            requestUri += $"&appid={AppConstants.OpenWeatherMapAPIKey}";
            WeatherData weatherData = await _restService.GetWeatherData(requestUri);//https://api.openweathermap.org/data/2.5/weather?lat=35&lon=139&units=metric&appid=6a13cd8fbbd77a77ff4666d8b6ac1336"
            return weatherData;
        }

        #region -- Public properties --
        #endregion
        #region -- InterfaceName implementation --
        #endregion
        #region -- Overrides --
        #endregion
        #region -- Public helpers --
        #endregion
        #region -- Private helpers --
        #endregion

    }
}
