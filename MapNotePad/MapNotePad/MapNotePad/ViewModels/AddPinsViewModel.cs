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
    public class AddPinsViewModel : BaseViewModel
    {
        private IPageDialogService _dialogs { get; }
        private IMapService _mapService { get; set; }

        public AddPinsViewModel(INavigationService navigationService, IPageDialogService dialogs, IMapService mapService) : base(navigationService)
        {
            _dialogs = dialogs;
            _mapService = mapService;
        }



        #region -- Public properties --
        private MapStyle _mapThemeStyle;
        public MapStyle MapThemeStyle
        {
            get => _mapThemeStyle;
            set => SetProperty(ref _mapThemeStyle, value);
        }

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
        private EAddEditType _choise = EAddEditType.Add;
        public EAddEditType Choise
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

        private ICommand _goBackCommand;
        public ICommand GoBackCommand => _goBackCommand ??= SingleExecutionCommand.FromFunc(OnGoBackCommandAsync);
        private ICommand _GeoLocCommand;
        public ICommand GeoLocCommand => _GeoLocCommand ??= SingleExecutionCommand.FromFunc(OnGeoLocCommandAsync);
        private ICommand _MapClickedCommand;
        public ICommand MapClickedCommand => _MapClickedCommand ??= SingleExecutionCommand.FromFunc<MapClickedEventArgs>(OnMapClickedCommandAsync);
        private ICommand _SaveClickedCommand;
        public ICommand SaveClickedCommand => _SaveClickedCommand ??= SingleExecutionCommand.FromFunc(OnSaveClickedCommandAsync);
        #endregion
        #region -- InterfaceName implementation --

        public override async void Initialize(INavigationParameters parameters)
        {
            await Task.Delay(TimeSpan.FromSeconds(0.1));
        }
        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            parameters.Add(nameof(this.UserId), this.UserId);
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            string parameterName = "UserId";
            if (parameters.ContainsKey(parameterName))
            {
                int id = parameters.GetValue<int>(parameterName);
                UserId = id;
                var result = await _mapService.SetPinsAsync(Pins, UserId);
                if (result.IsSuccess && Pins?.Count > 0) Pins?.Add(Pins[0]);
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
                Choise = EAddEditType.Edit;
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
        private async Task OnGoBackCommandAsync()
        {
            await _navigationService.GoBackAsync();
        }

        private Task OnMapClickedCommandAsync(MapClickedEventArgs args)
        {
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
            return Task.CompletedTask;
        }
        private async Task OnSaveClickedCommandAsync()
        {
            if (!string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Description) && !string.IsNullOrEmpty(Latitude) && !string.IsNullOrEmpty(Longitude))
            {
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
                Longitude = Longitude.Replace(",", ".");
                Latitude = Latitude.Replace(",", ".");
                double latitude;
                double longitude;
                var blon = double.TryParse(Longitude, out longitude);
                var blat = double.TryParse(Latitude, out latitude);
                if (blon)
                {
                    if (blat)
                    {

                        PinModel pin = new PinModel
                        {
                            Id = Id,
                            Name = Name,
                            Description = Description,
                            Latitude = Convert.ToDouble(Latitude),
                            Longitude = Convert.ToDouble(Longitude),
                            User = UserId,
                            Favourite = true,
                            Date = DateTime.Now
                        };
                        await _mapService.AddEditExecute(Choise, pin);
                        if (Choise == EAddEditType.Add) Pins?.Add(pin.ToPin());
                        else await _mapService.SetPinsAsync(Pins, UserId);
                        Region = MapSpan.FromCenterAndRadius(new Position(pin.Latitude, pin.Longitude), Distance.FromKilometers(1));


                    }
                    else
                    {
                        await _dialogs.DisplayAlertAsync(Resources.Resource.Alert, Resources.Resource.IncorrectLatitude, Resources.Resource.Ok);
                    }
                }
                else
                {
                    await _dialogs.DisplayAlertAsync(Resources.Resource.Alert, Resources.Resource.IncorrectLongitude, Resources.Resource.Ok);
                }
            }
            else
            {
               await _dialogs.DisplayAlertAsync(Resources.Resource.Alert, "Fill in all fields with values", Resources.Resource.Ok);
            }
        }

        private async Task OnGeoLocCommandAsync()
        {
            var result = await _mapService.CurrentLocation(Region);
            Region = result.Result;
            if (!result.IsSuccess) await _dialogs.DisplayAlertAsync(Resources.Resource.Alert, "The location denied", "Ok");
        }



        #endregion
    }
}
