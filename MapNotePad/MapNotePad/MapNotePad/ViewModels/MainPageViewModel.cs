using MapNotePad.Helpers;
using MapNotePad.Models;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
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

        public async void Initialize(INavigationParameters parameters)
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
            var location = await Geolocation.GetLastKnownLocationAsync();
            Region = MapSpan.FromCenterAndRadius(
                             new Position(location.Latitude, location.Longitude),
                             Distance.FromKilometers(1000));

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
        public ICommand GeoLocCommand => _GeoLocCommand ??= SingleExecutionCommand.FromFunc(OnGeoLocCommandAsync);

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
        #endregion
        #region -- InterfaceName implementation --
        #endregion
        #region -- Overrides --
        #endregion
        #region -- Public helpers --
        #endregion
        #region -- Private helpers --
        private Task OnGeoLocCommandAsync()
        {
            //_dialogs.DisplayAlertAsync("Alert", "Alert", "Ok");
            var location = Geolocation.GetLastKnownLocationAsync();
            Region = MapSpan.FromCenterAndRadius(
                             new Position(location.Result.Latitude, location.Result.Longitude),
                             Distance.FromKilometers(500));
            return Task.CompletedTask;
        }


        #endregion

    }
}
