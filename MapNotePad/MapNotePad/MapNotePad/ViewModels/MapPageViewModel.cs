using MapNotePad.Helpers;
using MapNotePad.Models;
using MapNotePad.Services.Interface;
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
    public class MapPageViewModel : BaseContentPage, INavigationAware
    {
        private IPageDialogService _dialogs { get; }
        private IMapService _mapService { get; set; }

        public MapPageViewModel(INavigationService navigationService, IPageDialogService dialogs, IMapService mapService) : base(navigationService)
        {
            _dialogs = dialogs;
            _mapService = mapService;
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

        private ObservableCollection<ForecastView> _forecastViews = new();
        public ObservableCollection<ForecastView> ForecastViews
        {
            get => _forecastViews;
            set => SetProperty(ref _forecastViews, value);
        }

        private ICommand _GeoLocCommand;
        public ICommand GeoLocCommand => _GeoLocCommand ??= SingleExecutionCommand.FromFunc(OnGeoLocCommandAsync);
        private ICommand _PinClickedCommand;
        public ICommand PinClickedCommand => _PinClickedCommand ??= SingleExecutionCommand.FromFunc<PinClickedEventArgs>(OnPinClickedCommandAsync);
        private ICommand _MapClickedCommand;
        public ICommand MapClickedCommand => _MapClickedCommand ??= SingleExecutionCommand.FromFunc<MapClickedEventArgs>(OnMapClickedCommandAsync);
        private ICommand _SearchTextCommand;
        public ICommand SearchTextCommand => _SearchTextCommand ??= SingleExecutionCommand.FromFunc(OnSearchTextCommandAsync);
        private ICommand _TapShowCommand;
        public ICommand TapShowCommand => _TapShowCommand ??= SingleExecutionCommand.FromFunc<SearchView>(OnTapShowCommandAsync);

        #endregion
        #region -- InterfaceName implementation --
        public void OnNavigatedFrom(INavigationParameters parameters)
        {
            parameters.Add(nameof(this.UserId), this.UserId);
        }

        public async void OnNavigatedTo(INavigationParameters parameters)
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
        #endregion
        #region -- Overrides --
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
        // день недели, дождь/ясно, температура днем ночью
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
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            for (int i = 0; i <= 3; i++)
            {
                ForecastViews.Add(new ForecastView
                {
                    Day = data.AddDays(i).ToString("ddd"),
                    Image = $"ic_{forecastData.Daily[i].Weather[0].Icon}.png",
                    TempMin = forecastData.Daily[i].Temperature.Min.ToString(),
                    TempMax = forecastData.Daily[i].Temperature.Max.ToString()
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

        private Task OnSearchTextCommandAsync()
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

                if (SearchPins.Count == 0 && SearchText.Length > 0) _dialogs.DisplayAlertAsync("Alert", $"Not Found \"{SearchText}\"", "Ok");
            }
            else IsViewSearch = false;
            //ListViewGrid = new GridLength(SearchPins.Count, GridUnitType.Star);
            return Task.CompletedTask;
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

        #endregion
    }
}
