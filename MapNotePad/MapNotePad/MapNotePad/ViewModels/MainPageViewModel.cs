using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using MapNotePad.Helpers;
using MapNotePad.Models;
using Plugin.Geolocator;
using Prism.Navigation;
using Prism.Services;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using Xamarin.Forms.GoogleMaps.Clustering;
using Map = Xamarin.Forms.GoogleMaps.Map;

namespace MapNotePad.ViewModels
{
    public class MainPageViewModel : BaseContentPage, IInitialize
    {
        private IPageDialogService _dialogs { get; }

        public MainPageViewModel(INavigationService navigationService, IPageDialogService dialogs) : base(navigationService)
        {
            _dialogs = dialogs;

            CustomPin pin = new CustomPin
            {
                Pin = new Pin
                {
                    Type = PinType.Place,
                    Position = new Xamarin.Forms.GoogleMaps.Position(37.79752, -122.40183),
                    Label = "Xamarin San Francisco Office",
                    Address = "394 Pacific Ave, San Francisco CA"
                },
                User = 0
            };
//            customMap.CustomPins = new List<CustomPin> { pin };
        }
        public event EventHandler<string> SearchBarTextChanged;

        public void OnSearchBarTextChanged(in string text) => SearchBarTextChanged?.Invoke(this, text);

        void HandleSearchBarTextChanged(object sender, string searchBarText)
        {
            //Logic to handle updated search bar text
        }

        public void Initialize(INavigationParameters parameters)
        {
            Random randomPositionGenerator = new Random();

            for (int i = 0; i < 10; i++)
            {
                Pin pin = new Pin
                {
                    Type = PinType.Place,
                    Position = RandomPositionGenerator.Next(),
                    Label = $"Name {i}",
                    Address = $"Discription {i}"
                };
                Pins.Add(pin);
            }
            Region = MapSpan.FromCenterAndRadius(
                                 new Xamarin.Forms.GoogleMaps.Position(0, 0),
                                 Distance.FromKilometers(2000));

        }


        #region -- Public properties --
        private MapSpan _region;
        public MapSpan Region
        {
            get => _region;
            set => SetProperty(ref _region, value);
        }
        private bool _animated = false;
        public bool Animated
        {
            get => _animated;
            set => SetProperty(ref _animated, value);
        }
        private string _search;
        public string Search
        {
            get => _search;
            set => SetProperty(ref _search, value);
        }
        private ObservableCollection<Pin> _pins;
        public ObservableCollection<Pin> Pins
        {
            get => _pins;
            set => SetProperty(ref _pins, value);
        }
        private ObservableCollection<Pin> _searchPins;
        public ObservableCollection<Pin> SearchPins
        {
            get => _searchPins;
            set => SetProperty(ref _searchPins, value);
        }

        private ICommand _GeoLocCommand;
        private ICommand _ViewCommand;
        private ICommand _SearchCommand;
        private ICommand _SearchTextCommand;
        private ICommand _AddPinsCommand;

        [Obsolete]
        public ICommand GeoLocCommand => _GeoLocCommand ??= SingleExecutionCommand.FromFunc(OnGeoLocCommandAsync);
        public ICommand ViewCommand => _ViewCommand ??= SingleExecutionCommand.FromFunc(OnViewCommandAsync);
        public ICommand SearchCommand => _SearchCommand ??= SingleExecutionCommand.FromFunc(OnSearchCommandAsync);
        public ICommand SearchTextCommand => _SearchTextCommand ??= SingleExecutionCommand.FromFunc(OnSearchTextCommandAsync);
        public ICommand AddPinsCommand => _AddPinsCommand ??= SingleExecutionCommand.FromFunc(OnAddPinsCommandAsync);

        private ClusteredMap _clustermap;
        public ClusteredMap ClusterMap
        {
            get => _clustermap;
            private set => SetProperty(ref _clustermap, value);
        }
        private Map _map;
        public Map Map
        {
            get => _map;
            private set => SetProperty(ref _map, value);
        }

        private Pin _pin;
        private bool _isViewBox = true;
        private GridLength _pinClicked = 0;
        private int _selectedPinChangedCount;
        private int _infoWindowClickedCount;
        private int _infoWindowLongClickedCount;
        private string _pinDragStatus;
        private string _position;

        public bool IsViewBox
        {
            get => _isViewBox;
            set => SetProperty(ref _isViewBox, value);
        }
        public GridLength PinClicked
        {
            get => _pinClicked;
            set => SetProperty(ref _pinClicked, value);
        }
        private GridLength _listViewGrid;
        public GridLength ListViewGrid
        {
            get => _listViewGrid;
            set => SetProperty(ref _listViewGrid, value);
        }

        public string Position
        {
            get => _position;
            set => SetProperty(ref _position, value);
        }

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

        #endregion
        #region -- InterfaceName implementation --
        #endregion
        #region -- Overrides --
        #endregion
        #region -- Public helpers --
        public Command<MapClickedEventArgs> MapClickedCommand => new Command<MapClickedEventArgs>(args =>
        {
            Pin = new Pin
            {
                Label = $"Pin{Pins.Count}",
                Position = args.Point
            };
            Pins?.Add(Pin);
        });

        public Command<PinClickedEventArgs> PinClickedCommand => new Command<PinClickedEventArgs>(args =>
        {
            //await _navigationService.NavigateAsync("PinView", useModalNavigation: true);
            Pin = args.Pin;
            Position = $"{Pin.Position.Latitude}, {Pin.Position.Longitude}";
            PinClicked = new GridLength(1, GridUnitType.Star);
            IsViewBox = false;
            });

        #endregion
        #region -- Private helpers --

        [Obsolete]
        private Task OnGeoLocCommandAsync()
        {
            Task.Run(async () =>
            {
                PinClicked = new GridLength(0);
                IsViewBox = true;
                try
                {
                    var locator = CrossGeolocator.Current;
                    locator.DesiredAccuracy = 10000;
                    var position = await locator.GetPositionAsync(new TimeSpan(0, 0, 5));
                    Region = MapSpan.FromCenterAndRadius(
                                 new Xamarin.Forms.GoogleMaps.Position(position.Latitude, position.Longitude),
                                 Distance.FromKilometers(500));
                }
                catch (Exception ex)
                {
                    await _dialogs.DisplayAlertAsync("Alert", "Alert", "Ok");
                }
            });
            return Task.CompletedTask;
        }

        private Task OnViewCommandAsync()
        {
            //_dialogs.DisplayAlertAsync("Alert", "Alert", "Ok");
            if (IsViewBox) IsViewBox = false;
            else IsViewBox = true;
            return Task.CompletedTask;
        }

        private Task OnSearchCommandAsync()
        {
            _dialogs.DisplayAlertAsync("Alert", "Alert " + SearchText, "Ok");
            return Task.CompletedTask;
        }

        private Task OnAddPinsCommandAsync()
        {
            Task.Run(async () =>
            {
               await _navigationService.NavigateAsync("PinView");
            });
            
            return Task.CompletedTask;
        }

        private Task OnSearchTextCommandAsync()
        {
            IsViewBox = false;
            SearchPins = new ObservableCollection<Pin>(Pins.Where(x => (x.Label.Contains(SearchText) || x.Address.Contains(SearchText) || x.Position.Latitude.ToString().Contains(SearchText) || x.Position.Longitude.ToString().Contains(SearchText))));
            if (SearchPins.Count == 0 && SearchText.Length > 0) _dialogs.DisplayAlertAsync("Alert", $"Not Found {SearchText} {SearchPins.Count} {SearchText.Length}", "Ok");
            if (SearchText == string.Empty) {
                SearchPins = new ObservableCollection<Pin>();
                IsViewBox = true;
            }
            ListViewGrid = new GridLength(SearchPins.Count, GridUnitType.Star);
            return Task.CompletedTask;
        }

        #endregion



    }
}
