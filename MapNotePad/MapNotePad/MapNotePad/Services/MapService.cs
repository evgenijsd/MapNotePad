﻿using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using MapNotePad.Enum;
using MapNotePad.Helpers;
using MapNotePad.Helpers.ProcessHelpers;
using MapNotePad.Models;
using MapNotePad.Services.Interface;
using MapNotePad.Services.Repository;
using Plugin.Geolocator;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Prism.Services;
using Xamarin.Forms.GoogleMaps;

namespace MapNotePad.Services
{
    public class MapService : IMapService
    {
        private IRepository _repository { get; }
        RestService _restService { get; }
        private IPageDialogService _dialogs { get; }

        public MapService(IRepository repository, RestService restService, IPageDialogService dialogs)
        {
            _repository = repository;
            _restService = restService;
            _dialogs = dialogs;
        }

        public async Task DeletePinAsync(ObservableCollection<PinView> collectPin, object pintObj)
        {
            PinView pin = pintObj as PinView;
            PinModel pindel = pin.ToPinModel();
            await _repository.RemoveAsync<PinModel>(pindel);
            collectPin.Remove(pin);
        }


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

        public async Task<AOResult> SetPinsAsync(ObservableCollection<Pin> pins, int userId)
        {
            var result = new AOResult();
            try
            {
                var pm = new ObservableCollection<PinModel>(await _repository.GetAsync<PinModel>(x => x.User == userId));
                if (pm != null)
                    result.SetSuccess();
                else
                    result.SetFailure();
                foreach (PinModel pinm in pm)
                {
                    var pin = pinm.ToPin();
                    pin.Icon = BitmapDescriptorFactory.FromBundle("ic_placeholder.png");
                    pins?.Add(pin);
                }
            }
            catch (Exception ex)
            {
                result.SetError($"Exception: {nameof(SetPinsAsync)}", "Wrong result", ex);
            }
            return result;
        }

        public async Task<AOResult<int>> SetPinsFavourite(PinModel pin)
        {
            var result = new AOResult<int>();
            int count = 0;
            try
            {
                count = await _repository.UpdateAsync(pin);
                if (count != 0)
                    result.SetSuccess(count);
                else
                    result.SetFailure();
            }
            catch (Exception ex)
            {
                result.SetError($"Exception: {nameof(SetPinsFavourite)}", "Wrong result", ex);
            }
            return result;
        }

        public async Task<AOResult<MapSpan>> CurrentLocation(MapSpan region)
        {
            var result = new AOResult<MapSpan>();
            try
            {
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync<LocationPermission>();
                if (status != PermissionStatus.Granted)
                {
                    if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Location))
                    {
                        await _dialogs.DisplayAlertAsync(Resources.Resource.Alert, "Need location", "Ok");
                    }

                    status = await CrossPermissions.Current.RequestPermissionAsync<LocationPermission>();
                }

                if (status == PermissionStatus.Granted)
                {
                    var locator = CrossGeolocator.Current;
                    locator.DesiredAccuracy = 10000;
                    var position = await locator.GetPositionAsync(new TimeSpan(0, 0, 5));
                    region = MapSpan.FromCenterAndRadius(
                                 new Position(position.Latitude, position.Longitude),
                                 Distance.FromKilometers(100));
                    result.SetSuccess(region);
                }
                else if (status != PermissionStatus.Unknown)
                {
                    result.SetFailure();
                }
            }
            catch (Exception ex)
            {
                result.SetError($"Exception: {nameof(CurrentLocation)}", "Wrong result", ex);
            }
            return result;
        }


        //api.openweathermap.org/data/2.5/weather?lat={lat}&lon={lon}&appid={API key}
        public async Task<AOResult<ForecastData>> GetForecast(double latitude, double longitude)
        {
            var result = new AOResult<ForecastData>();
            try
            {
                string requestUri = AppConstants.OpenWeatherMapEndpoint;
                requestUri += $"?lat={latitude}";
                requestUri += $"&lon={longitude}";
                requestUri += "&exclude=minutely,hourly,alerts,current";
                requestUri += "&units=metric";
                requestUri += $"&appid={AppConstants.OpenWeatherMapAPIKey}";
                ForecastData forecastData = await _restService.GetForecastData(requestUri);//https://api.openweathermap.org/data/2.5/onecall?lat=12&lon=12&exclude=minutely,hourly,alerts,current&units=metric&appid=6a13cd8fbbd77a77ff4666d8b6ac1336
                if (forecastData != null)
                    result.SetSuccess(forecastData);
                else
                    result.SetFailure();
            }
            catch (Exception ex)
            {
                result.SetError($"Exception: {nameof(GetForecast)}", "Wrong result", ex);
            }
            return result;
        }

        public async Task<AOResult<WeatherData>> GetWeather(double latitude, double longitude)
        {
            var result = new AOResult<WeatherData>();
            try
            {
                string requestUri = AppConstants.OpenWeatherMapEndpoint;
                requestUri += $"?lat={latitude}";
                requestUri += $"&lon={longitude}";
                requestUri += "&units=metric";
                requestUri += $"&appid={AppConstants.OpenWeatherMapAPIKey}";
                WeatherData weatherData = await _restService.GetWeatherData(requestUri);//https://api.openweathermap.org/data/2.5/weather?lat=35&lon=139&units=metric&appid=6a13cd8fbbd77a77ff4666d8b6ac1336"
                if (weatherData != null)
                    result.SetSuccess(weatherData);
                else
                    result.SetFailure();
            }
            catch (Exception ex)
            {
                result.SetError($"Exception: {nameof(GetWeather)}", "Wrong result", ex);
            }
            return result;
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
