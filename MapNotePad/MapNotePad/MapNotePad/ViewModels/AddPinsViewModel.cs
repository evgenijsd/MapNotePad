using MapNotePad.Enum;
using MapNotePad.Helpers;
using MapNotePad.Models;
using MapNotePad.Services.Interface;
using Plugin.Geolocator;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms.GoogleMaps;

namespace MapNotePad.ViewModels
{
    public class AddPinsViewModel : BaseContentPage, IInitialize, INavigationAware
    {
        private IPageDialogService _dialogs { get; }
        private IMapService _mapService { get; set; }

        public AddPinsViewModel(INavigationService navigationService, IPageDialogService dialogs, IMapService mapService) : base(navigationService)
        {
            _dialogs = dialogs;
            _mapService = mapService;
        }



        #region -- Public properties --
        private string _title = "Add";
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }
        private ObservableCollection<Pin> _pins;
        public ObservableCollection<Pin> Pins
        {
            get => _pins;
            set => SetProperty(ref _pins, value);
        }
        private int _id = 0;
        public int Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }
        private int _userId;
        public int UserId
        {
            get => this._userId;
            set => SetProperty(ref this._userId, value);
        }
        private string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }
        private string _description;
        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }
        private string _latitude;
        public string Latitude
        {
            get => _latitude;
            set => SetProperty(ref _latitude, value);
        }
        private string _longitude;
        public string Longitude
        {
            get => _longitude;
            set => SetProperty(ref _longitude, value);
        }
        private DateTime _date;
        public DateTime Date
        {
            get => _date;
            set => SetProperty(ref _date, value);
        }
        private AddEditType _choise = AddEditType.Add;
        public AddEditType Choise
        {
            get => _choise;
            set => SetProperty(ref _choise, value);
        }
        private MapSpan _region;
        public MapSpan Region
        {
            get => _region;
            set => SetProperty(ref _region, value);
        }

        private ICommand _GeoLocCommand;
        public ICommand GeoLocCommand => _GeoLocCommand ??= SingleExecutionCommand.FromFunc(OnGeoLocCommandAsync);
        private ICommand _MapClickedCommand;
        public ICommand MapClickedCommand => _MapClickedCommand ??= SingleExecutionCommand.FromFunc<MapClickedEventArgs>(OnMapClickedCommandAsync);
        private ICommand _SaveClickedCommand;
        public ICommand SaveClickedCommand => _SaveClickedCommand ??= SingleExecutionCommand.FromFunc(OnSaveClickedCommandAsync);
        #endregion
        #region -- InterfaceName implementation --

        public async void Initialize(INavigationParameters parameters)
        {
            await Task.Delay(TimeSpan.FromSeconds(0.1));
        }
        public void OnNavigatedFrom(INavigationParameters parameters)
        {
            parameters.Add(nameof(this.UserId), this.UserId);
        }

        public async void OnNavigatedTo(INavigationParameters parameters)
        {
            string parameterName = "UserId";
            if (parameters.ContainsKey(parameterName))
            {
                int id = parameters.GetValue<int>(parameterName);
                //await _dialogs.DisplayAlertAsync("Alert", $"{id}", "Ok");
                UserId = id;
                await _mapService.SetPinsAsync(Pins, UserId);
                if (Pins?.Count > 0) Pins?.Add(Pins[0]);
                else Pins?.Add(new Pin
                {
                    Type = PinType.Place,
                    Position = new Position(0,0),
                    Label = "Add Name",
                    Address = "Add Discription",
                    Icon = BitmapDescriptorFactory.FromBundle("ic_placeholder.png")
            });
            }

            parameterName = "Pin";
            if (parameters.ContainsKey(parameterName))
            {
                Title = "Edit";
                Choise = AddEditType.Edit;
                var pin = parameters.GetValue<PinModel>(parameterName);
                {
                    Id = pin.Id;
                    UserId = pin.User;
                    Description = pin.Description;
                    Name = pin.Name;
                    Latitude = pin.Latitude.ToString();
                    Longitude = pin.Longitude.ToString();
                    Date = pin.Date;
                }
            }
        }
        #endregion
        #region -- Overrides --
        #endregion
        #region -- Public helpers --
        #endregion
        #region -- Private helpers --
        private Task OnMapClickedCommandAsync(MapClickedEventArgs args)
        {
            //await _dialogs.DisplayAlertAsync("Alert", $"{args.Point.Latitude}", "Ok");
            Latitude = args.Point.Latitude.ToString();
            Longitude = args.Point.Longitude.ToString();
            Pins.RemoveAt(Pins.Count - 1);
            Pins?.Add(new Pin
            {
                Type = PinType.Place,
                Position = args.Point,
                Label = "Add Name",
                Address = "Add Discription",
                Icon = BitmapDescriptorFactory.FromBundle("ic_placeholder.png")
            });
            //return await _navigationService.NavigateAsync($"{nameof(Register)}");
            return Task.CompletedTask;
        }
        private async Task OnSaveClickedCommandAsync()
        {
            
            PinModel pin = new PinModel
            {
                Id = Id,
                Name = Name,
                Description = Description,
                Latitude = double.Parse(Latitude),
                Longitude = double.Parse(Longitude),
                User = UserId,
                Favourite = true,
                Date = DateTime.Now
            };
            await _mapService.AddEditExecute(Choise, pin);
            if (Choise == AddEditType.Add) Pins?.Add(pin.ToPin());
            else await _mapService.SetPinsAsync(Pins, UserId);
            //await _dialogs.DisplayAlertAsync("Alert", $"{pin.Id}", "Ok");
            //var p = new NavigationParameters { { "Pin", pin } };
            //await _navigationService.GoBackAsync(p);
        }

        private async Task OnGeoLocCommandAsync()
        {
            try
            {
                var locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = 10000;
                var position = await locator.GetPositionAsync(new TimeSpan(0, 0, 5));
                Region = MapSpan.FromCenterAndRadius(
                             new Position(position.Latitude, position.Longitude),
                             Distance.FromKilometers(500));
            }
            catch (Exception ex)
            {
                await _dialogs.DisplayAlertAsync("Alert", $"{ex}", "Ok");
            }
        }



        #endregion
    }
}
