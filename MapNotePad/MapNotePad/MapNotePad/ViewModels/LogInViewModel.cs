﻿using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using MapNotePad.Enum;
using MapNotePad.Helpers;
using MapNotePad.Models;
using MapNotePad.Services.Interface;
using Prism.Navigation;
using Prism.Services;
using Xamarin.Forms;

namespace MapNotePad.ViewModels
{
    public class LogInViewModel : BaseViewModel
    {

        private IPageDialogService _dialogs { get; }
        private IAuthentication _authentication { get; }
        private IRegistration _registration { get; }

        public LogInViewModel(INavigationService navigationService, IPageDialogService dialogs,
                       IAuthentication authentication, IRegistration registration) : base(navigationService)
        {
            _dialogs = dialogs;
            _authentication = authentication;
            UserId = _authentication.UserId;
            _registration = registration;
        }

        #region -- Public properties --
        private int _userId;
        public int UserId
        {
            get => _userId;
            set => SetProperty(ref _userId, value);
        }

        private string _email = string.Empty;
        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }
        private bool _isWrongEmail = false;
        public bool IsWrongEmail
        {
            get => _isWrongEmail;
            set => SetProperty(ref _isWrongEmail, value);
        }
        private string _password = string.Empty;
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }
        private bool _isIncorrectPassword = false;
        public bool IsIncorrectPassword
        {
            get => _isIncorrectPassword;
            set => SetProperty(ref _isIncorrectPassword, value);
        }


        private ICommand _goBackCommand;
        public ICommand GoBackCommand => _goBackCommand ??= SingleExecutionCommand.FromFunc(OnGoBackCommandAsync);
        private ICommand _MainTabPageCommand;
        public ICommand MainTabPageCommand => _MainTabPageCommand ??= SingleExecutionCommand.FromFunc(OnMainTabPageCommandAsync);
        private ICommand _GoogleMainCommand;
        public ICommand GoogleMainCommand => _GoogleMainCommand ??= SingleExecutionCommand.FromFunc(OnGoogleMainCommandAsync);
        public ICommand ErrorCommand => new Command(OnErrorCommand);
        #endregion

        #region -- InterfaceName implementation --

        #endregion
        #region -- Overrides --
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("User"))
            {
                var user = parameters.GetValue<Users>("User");
                Email = user.Email;
            }
        }
        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);

            switch (args.PropertyName)
            {
                case nameof(Email):
                case nameof(Password):
                    break;
            }
        }
        public override async void Initialize(INavigationParameters parameters)
        {
            await Task.Delay(TimeSpan.FromSeconds(0.1));
            if (UserId > 0)
            {
                var p = new NavigationParameters { { "UserId", UserId } };
                await _navigationService.NavigateAsync("/MainTabPage", p);
            }
        }
        #endregion
        #region -- Public helpers --

        #endregion
        #region -- Private helpers --
        private async Task OnGoBackCommandAsync()
        {
           await  _navigationService.GoBackAsync();
        }
        private async Task OnMainTabPageCommandAsync()
        {
            var result = await _authentication.CheckUserAsync(Email, Password);
            if (result.IsSuccess)
            {
                UserId = result.Result;
                _authentication.UserId = UserId;
                var p = new NavigationParameters { { "UserId", UserId } };
                await _navigationService.NavigateAsync("/MainTabPage", p);
            }
            else
            {
                await _dialogs.DisplayAlertAsync("Alert", "Invalid login or password!", "Ok");
                Password = string.Empty;
            }
        }
        private async Task OnGoogleMainCommandAsync()
        {
            await _navigationService.NavigateAsync("/StartPage");
        }

        private void OnErrorCommand()
        {
            switch ((ECheckEnter)_registration.CheckTheCorrectPassword(Password, Password))
            {
                case ECheckEnter.PasswordBigLetterAndDigit:
                case ECheckEnter.PasswordLengthNotValid:
                    IsIncorrectPassword = !string.IsNullOrEmpty(Password);
                    break;
                default:
                    IsIncorrectPassword = false;
                    break;
            }
            switch ((ECheckEnter)_registration.CheckCorrectEmail(Email))
            {
                case ECheckEnter.EmailANotVaid:
                case ECheckEnter.EmailLengthNotValid:
                    IsWrongEmail = !string.IsNullOrEmpty(Email);
                    break;
                default:
                    IsWrongEmail = false;
                    break;
            }
        }
        #endregion
    }
}
