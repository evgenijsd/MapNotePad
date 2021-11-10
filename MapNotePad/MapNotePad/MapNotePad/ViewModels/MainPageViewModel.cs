using MapNotePad.Helpers;
using MapNotePad.Models;
using Plugin.Geolocator;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
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

        public void Initialize(INavigationParameters parameters)
        {
            Random randomPositionGenerator = new Random();

            for (int i = 0; i < 1500; i++)
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
        private ObservableCollection<Pin> _pins;
        public ObservableCollection<Pin> Pins
        {
            get => _pins;
            set => SetProperty(ref _pins, value);
        }

        private ICommand _GeoLocCommand;
        private ICommand _ViewCommand;

        [Obsolete]
        public ICommand GeoLocCommand => _GeoLocCommand ??= SingleExecutionCommand.FromFunc(OnGeoLocCommandAsync);
        public ICommand ViewCommand => _ViewCommand ??= SingleExecutionCommand.FromFunc(OnViewCommandAsync);

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
        private bool _isViewBox = false;
        private int _pinClickedCount;
        private int _selectedPinChangedCount;
        private int _infoWindowClickedCount;
        private int _infoWindowLongClickedCount;
        private string _pinDragStatus;

        public bool IsViewBox
        {
            get => _isViewBox;
            set => SetProperty(ref _isViewBox, value);
        }
        public int PinClickedCount
        {
            get => _pinClickedCount;
            set => SetProperty(ref _pinClickedCount, value);
        }

        public int SelectedPinChangedCount
        {
            get => _selectedPinChangedCount;
            set => SetProperty(ref _selectedPinChangedCount, value);
        }

        public int InfoWindowClickedCount
        {
            get => _infoWindowClickedCount;
            set => SetProperty(ref _infoWindowClickedCount, value);
        }

        public int InfoWindowLongClickedCount
        {
            get => _infoWindowLongClickedCount;
            set => SetProperty(ref _infoWindowLongClickedCount, value);
        }

        public string PinDragStatus
        {
            get => _pinDragStatus;
            set => SetProperty(ref _pinDragStatus, value);
        }

        public Pin Pin
        {
            get => _pin;
            set => SetProperty(ref _pin, value);
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
        });

        #endregion
        #region -- Private helpers --

        [Obsolete]
        private Task OnGeoLocCommandAsync()
        {
            Task.Run(async () =>
            {
                try
                {
                    var locator = CrossGeolocator.Current;
                    locator.DesiredAccuracy = 10000;
                    var position = await locator.GetPositionAsync(new TimeSpan(0, 0, 5));
                    Region = MapSpan.FromCenterAndRadius(
                                 new Xamarin.Forms.GoogleMaps.Position(position.Latitude, position.Longitude),
                                 Distance.FromKilometers(2000));
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

        #endregion



    }
}
