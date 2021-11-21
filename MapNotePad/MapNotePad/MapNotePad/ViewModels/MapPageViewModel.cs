using Acr.UserDialogs;
using MapNotePad.Enum;
using MapNotePad.Helpers;
using MapNotePad.Models;
using MapNotePad.Services.Interface;
using MapNotePad.Views;
using Plugin.Geolocator;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace MapNotePad.ViewModels
{
    public class MapPageViewModel : BaseViewModel
    {
        private IPageDialogService _dialogs { get; }
        private IMapService _mapService { get; set; }
        private IAuthentication _authentication { get; }
        private ISettings _settings;

        public MapPageViewModel(INavigationService navigationService, IPageDialogService dialogs, IMapService mapService, IAuthentication authentication, ISettings settings) : base(navigationService)
        {
            _dialogs = dialogs;
            _mapService = mapService;
            _authentication = authentication;
            _settings = settings;
            _settings.Language((LangType)_settings.LangSet);
        }



        #region -- Public properties --
        private ObservableCollection<SearchView> _searchPins;
        public ObservableCollection<SearchView> SearchPins
        {
            get => _searchPins;
            set => SetProperty(ref _searchPins, value);
        }
        private ObservableCollection<Pin> _pins;
        public ObservableCollection<Pin> Pins
        {
            get => _pins;
            set => SetProperty(ref _pins, value);
        }
        private Pin _pin;
        public Pin Pin
        {
            get => _pin;
            set => SetProperty(ref _pin, value);
        }
        private string _searchText = string.Empty;
        public string SearchText
        {
            get => _searchText;
            set => SetProperty(ref _searchText, value);
        }
        private int _userId;
        public int UserId
        {
            get => this._userId;
            set => SetProperty(ref this._userId, value);
        }
        private string _search;
        public string Search
        {
            get => _search;
            set => SetProperty(ref _search, value);
        }
        private GridLength _pinClicked = 0;
        public GridLength PinClicked
        {
            get => _pinClicked;
            set => SetProperty(ref _pinClicked, value);
        }
        private bool _isViewPin = false;
        public bool IsViewPin
        {
            get => _isViewPin;
            set => SetProperty(ref _isViewPin, value);
        }
        private bool _isViewSearch = false;
        public bool IsViewSearch
        {
            get => _isViewSearch;
            set => SetProperty(ref _isViewSearch, value);
        }
        private bool _isViewButton = true;
        public bool IsViewButton
        {
            get => _isViewButton;
            set => SetProperty(ref _isViewButton, value);
        }
        private MapSpan _region;
        public MapSpan Region
        {
            get => _region;
            set => SetProperty(ref _region, value);
        }
        private GridLength _listViewHeight;
        public GridLength ListViewHeight
        {
            get => _listViewHeight;
            set => SetProperty(ref _listViewHeight, value);
        }

        private ObservableCollection<ForecastView> _forecastViews = new();
        public ObservableCollection<ForecastView> ForecastViews
        {
            get => _forecastViews;
            set => SetProperty(ref _forecastViews, value);
        }

        private ICommand _settingsCommand;
        public ICommand SettingsCommand => _settingsCommand ??= SingleExecutionCommand.FromFunc(OnSettingsCommandAsync);
        private ICommand _exitCommand;
        public ICommand ExitCommand => _exitCommand ??= SingleExecutionCommand.FromFunc(OnExitCommandAsync);
        private ICommand _GeoLocCommand;
        public ICommand GeoLocCommand => _GeoLocCommand ??= SingleExecutionCommand.FromFunc(OnGeoLocCommandAsync);
        private ICommand _PinClickedCommand;
        public ICommand PinClickedCommand => _PinClickedCommand ??= SingleExecutionCommand.FromFunc<PinClickedEventArgs>(OnPinClickedCommandAsync);
        private ICommand _MapClickedCommand;
        public ICommand MapClickedCommand => _MapClickedCommand ??= SingleExecutionCommand.FromFunc<MapClickedEventArgs>(OnMapClickedCommandAsync);
        public ICommand SearchTextCommand => new Command(OnSearchTextCommandAsync);
        private ICommand _TapShowCommand;
        public ICommand TapShowCommand => _TapShowCommand ??= SingleExecutionCommand.FromFunc<SearchView>(OnTapShowCommandAsync);

        #endregion
        #region -- InterfaceName implementation --
        #endregion
        #region -- Overrides --
        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            parameters.Add(nameof(this.UserId), this.UserId);
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("UserId"))
            {
                int id = parameters.GetValue<int>("UserId");
                //_dialogs.DisplayAlertAsync("Alert", $"{id}", "Ok");
                UserId = id;
                IsViewPin = false;
                var pinviews = await _mapService.GetPinsViewAsync(UserId);
                Pins.Clear();
                _mapService.SetPinsFavouriteAsync(Pins, pinviews);
            }
        }
        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);

            switch (args.PropertyName)
            {
                case nameof(IsViewPin):
                    if (IsViewPin) IsViewButton = false;
                    else IsViewButton = true;
                    break;
                case nameof(IsViewSearch):
                    if (IsViewSearch)
                    {
                        IsViewButton = false;
                        IsViewPin = false;
                    }
                    else IsViewButton = true;
                    break;
                default:
                    break;
            }
        }
        #endregion
        #region -- Public helpers --
        #endregion
        #region -- Private helpers --
        private async Task OnPinClickedCommandAsync(PinClickedEventArgs args)
        {
            Pin = args.Pin;
            IsViewPin = true;
            IsViewSearch = false;
            if (ForecastViews != null) ForecastViews.Clear();
            //_dialogs.DisplayAlertAsync("Alert", $"{args.Pin.Position.Latitude}", "Ok");
            //return await _navigationService.NavigateAsync($"{nameof(Register)}");
            var forecastData = await _mapService.GetForecast(Pin.Position.Latitude, Pin.Position.Longitude);
            DateTime data = DateTime.Now;
            _settings.Language((LangType)_settings.LangSet);
            //System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            for (int i = 0; i < 4; i++)
            {
                ForecastViews.Add(new ForecastView
                {
                    Day = data.AddDays(i).ToString("ddd"),
                    Image = $"ic_{forecastData.Daily[i].Weather[0].Icon}.png",
                    TempMin = $"{(int)forecastData.Daily[i].Temperature.Min}°",
                    TempMax = $"{(int)forecastData.Daily[i].Temperature.Max}°"
                });
            }
        }
        private Task OnMapClickedCommandAsync(MapClickedEventArgs args)
        {
            //await _dialogs.DisplayAlertAsync("Alert", $"{args.Point.Latitude}", "Ok");
            IsViewPin = false;
            IsViewSearch = false;
            //return await _navigationService.NavigateAsync($"{nameof(Register)}");
            return Task.CompletedTask;
        }
        private async Task OnGeoLocCommandAsync()
        {
            PinClicked = new GridLength(0);
            try
            {
                var locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = 10000;
                var position = await locator.GetPositionAsync(new TimeSpan(0, 0, 5));
                Region = MapSpan.FromCenterAndRadius(
                             new Position(position.Latitude, position.Longitude),
                             Distance.FromKilometers(100));
            }
            catch (Exception ex)
            {
                await _dialogs.DisplayAlertAsync("Alert", $"{ex}", "Ok");
            }
        }

        private void OnSearchTextCommandAsync()
        {
            SearchPins = new ObservableCollection<SearchView>();
            if (!string.IsNullOrEmpty(SearchText))
            {
                IsViewSearch = true;
                var pins = new ObservableCollection<Pin>(Pins.Where(x => x.Label.Contains(SearchText) || x.Address.Contains(SearchText) || x.Position.Latitude.ToString().Contains(SearchText) || x.Position.Longitude.ToString().Contains(SearchText)));
                foreach (Pin pin in pins)
                {
                    var search = new SearchView
                    {
                        Image = "ic_pin_gray.png",
                        Name = pin.Label,
                        Description = pin.Address,
                        Latitude = pin.Position.Latitude,
                        Longitude = pin.Position.Longitude
                    };
                    SearchPins.Add(search);
                }

                if (SearchPins.Count == 0 && SearchText.Length > 0)
                {
                    _dialogs.DisplayAlertAsync("Alert", $"Not Found \"{SearchText}\"", "Ok");
                    ListViewHeight = new GridLength(0);
                    IsViewSearch = false;
                }
                else
                {
                    ListViewHeight = new GridLength(SearchPins.Count * 70);
                }
            }
            else
            {
                IsViewSearch = false;
                ListViewHeight = new GridLength(0);
            }
            //ListViewGrid = new GridLength(SearchPins.Count, GridUnitType.Star);
        }

        private Task OnTapShowCommandAsync(SearchView pin)
        {
            //Pin = args.Pin;
            //IsViewPin = true;
            //await _dialogs.DisplayAlertAsync("Alert", $"{pin.Name}", "Ok");
            Region = MapSpan.FromCenterAndRadius(new Position(pin.Latitude, pin.Longitude), Distance.FromKilometers(1));
            //return await _navigationService.NavigateAsync($"{nameof(Register)}");
            return Task.CompletedTask;
        }

        private async Task OnSettingsCommandAsync()
        {
            
                await _navigationService.NavigateAsync($"{nameof(SettingsPage)}");
            
        }
        private async Task OnExitCommandAsync()
        {
            var confirmConfig = new ConfirmConfig()
            {
                Message = Resources.Resource.ExitUser,
                OkText = Resources.Resource.Exit,
                CancelText = Resources.Resource.Cancel
            };
            var confirm = await UserDialogs.Instance.ConfirmAsync(confirmConfig);
            if (confirm)
            {
                _authentication.UserId = 0;
                await _navigationService.NavigateAsync($"{nameof(StartPage)}");
            }


        }

        #endregion
    }
}
