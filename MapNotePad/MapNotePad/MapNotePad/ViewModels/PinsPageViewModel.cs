using Acr.UserDialogs;
using MapNotePad.Helpers;
using MapNotePad.Models;
using MapNotePad.Services.Interface;
using MapNotePad.Views;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MapNotePad.ViewModels
{
    public class PinsPageViewModel : BaseViewModel
    {
        private IMapService _mapService { get; set; }

        private IPageDialogService _dialogs { get; }

        public PinsPageViewModel(INavigationService navigationService, IPageDialogService dialogs, IMapService mapService) : base(navigationService)
        {
            _dialogs = dialogs;
            _mapService = mapService;
        }

        #region -- Public properties --
        private ObservableCollection<PinView> _pinViews;
        public ObservableCollection<PinView> PinViews
        {
            get => _pinViews;
            set => SetProperty(ref _pinViews, value);
        }
        private ObservableCollection<PinView> _pinSearch;
        public ObservableCollection<PinView> PinSearch
        {
            get => _pinSearch;
            set => SetProperty(ref _pinSearch, value);
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

        private ICommand _AddPinCommand;
        public ICommand AddPinCommand => _AddPinCommand ??= SingleExecutionCommand.FromFunc(OnAddPinCommandAsync);
        private ICommand _EditCommand;
        public ICommand EditCommand => _EditCommand ??= SingleExecutionCommand.FromFunc<object>(OnEditCommandAsync);
        private ICommand _DeleteCommand;
        public ICommand DeleteCommand => _DeleteCommand ??= SingleExecutionCommand.FromFunc<object>(OnDeleteCommandAsync);
        private ICommand _SearchTextCommand;
        public ICommand SearchTextCommand => _SearchTextCommand ??= SingleExecutionCommand.FromFunc(OnSearchTextCommandAsync);
        private ICommand _FavouriteCommand;
        public ICommand FavouriteCommand => _FavouriteCommand ??= SingleExecutionCommand.FromFunc<object>(OnFavouriteCommandAsync);
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
                UserId = id;
                PinViews = await _mapService.GetPinsViewAsync(UserId);
                foreach (PinView pin in PinViews)
                {
                    pin.EditCommand = EditCommand;
                    pin.DeleteCommand = DeleteCommand;
                }
                PinSearch = PinViews;
            }
        }
        #endregion
        #region -- Public helpers --
        #endregion
        #region -- Private helpers --
        private async Task OnAddPinCommandAsync()
        {
            await _navigationService.NavigateAsync($"{nameof(AddPins)}");
        }

        private async Task OnEditCommandAsync(object args)
        {
            if (args != null)
            {
                PinView pin = args as PinView;
                var p = new NavigationParameters { { "Pin", pin.ToPinModel() } };
                await _navigationService.NavigateAsync($"{nameof(AddPins)}", p);
            }
        }
        private async Task OnDeleteCommandAsync(object args)
        {
            if (args != null)
            {
                var confirmConfig = new ConfirmConfig()
                {
                    Message = "Delete pin",
                    OkText = "Delete",
                    CancelText = "Cancel"
                };
                var confirm = await UserDialogs.Instance.ConfirmAsync(confirmConfig);
                if (confirm)
                {
                    await _mapService.DeletePinAsync(PinSearch, args);
                }
            }
            PinView pinv = args as PinView;
            PinModel pindel = pinv.ToPinModel();
            //_dialogs.DisplayAlertAsync("Alert", $"{pinv.Id}", "Ok");
            
            //return await _navigationService.NavigateAsync($"{nameof(Register)}");
            //return Task.CompletedTask;
        }

        private Task OnSearchTextCommandAsync()
        {
            if (!string.IsNullOrEmpty(SearchText))
            {
                PinSearch = new ObservableCollection<PinView>(PinViews.Where(x => x.Name.Contains(SearchText) || x.Description.Contains(SearchText) || x.Latitude.ToString().Contains(SearchText) || x.Longitude.ToString().Contains(SearchText)));
                if (PinSearch.Count == 0 && SearchText.Length > 0) _dialogs.DisplayAlertAsync("Alert", $"Not Found \"{SearchText}\"", "Ok");
            }
            else
                PinSearch = PinViews;
            //ListViewGrid = new GridLength(SearchPins.Count, GridUnitType.Star);
            return Task.CompletedTask;
        }

        private async Task OnFavouriteCommandAsync(object args)
        {
            //Pin = args.Pin;
            //IsViewPin = true;
            //await _dialogs.DisplayAlertAsync("Alert", $"{pin.Name}", "Ok");
            //Region = MapSpan.FromCenterAndRadius(new Position(pin.Latitude, pin.Longitude), Distance.FromKilometers(1));
            //return await _navigationService.NavigateAsync($"{nameof(Register)}");
            if (args != null)
            {
                PinView pin = args as PinView;
                if (pin.Favourite)
                {
                    pin.Favourite = !pin.Favourite;
                    pin.Image = "ic_like_gray.png";
                }
                else
                {
                    pin.Favourite = !pin.Favourite;
                    pin.Image = "ic_like_blue.png";
                }
                await _mapService.SetPinsFavourite(pin.ToPinModel());
                PinViews = await _mapService.GetPinsViewAsync(UserId);
                foreach (PinView pinv in PinViews)
                {
                    pinv.EditCommand = EditCommand;
                    pinv.DeleteCommand = DeleteCommand;
                }
            }
        }

        #endregion
    }
}
