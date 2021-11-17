using MapNotePad.Helpers;
using MapNotePad.Models;
using MapNotePad.Services.Interface;
using Prism.Navigation;
using Prism.Services;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MapNotePad.ViewModels
{
    public class LogInViewModel : BaseContentPage, IInitialize, INavigationAware
    {

        private IPageDialogService _dialogs { get; }
        private IAuthentication _authentication { get; }

        public LogInViewModel(INavigationService navigationService, IPageDialogService dialogs,
                       IAuthentication authentication) : base(navigationService)
        {
            _dialogs = dialogs;
            _authentication = authentication;
        }

        #region -- Public properties --
        private string _email = string.Empty;
        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }
        private string _password = string.Empty;
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }



        private ICommand _MainTabPageCommand;
        public ICommand MainTabPageCommand => _MainTabPageCommand ??= SingleExecutionCommand.FromFunc(OnMainTabPageCommandAsync);
        private ICommand _GoogleMainCommand;
        public ICommand GoogleMainCommand => _GoogleMainCommand ??= SingleExecutionCommand.FromFunc(OnGoogleMainCommandAsync);
        #endregion

        #region -- InterfaceName implementation --
        public void OnNavigatedFrom(INavigationParameters parameters)
        {
        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("User"))
            {
                var user = parameters.GetValue<Users>("User");
                Email = user.Email;
            }
        }
        #endregion
        #region -- Overrides --
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
        #endregion
        #region -- Public helpers --
        public async void Initialize(INavigationParameters parameters)
        {
            await Task.Delay(TimeSpan.FromSeconds(0.1));
        }
        #endregion
        #region -- Private helpers --
        private async Task OnMainTabPageCommandAsync()
        {
                try
                {
                    int id = await _authentication.CheckAsync(Email, Password);
                    if (id != 0)
                    {
                        var p = new NavigationParameters { { "UserId", id } };
                        await _navigationService.NavigateAsync("/NavigationPage/MainTabPage", p);
                    }
                    else
                    {
                        await _dialogs.DisplayAlertAsync("Alert", "Invalid login or password!", "Ok");
                        Password = string.Empty;
                    }
                }
                catch (Exception ex)
                {
                    await _dialogs.DisplayAlertAsync("Alert", $"{ex}", "Ok");
                }
        }
        private async Task OnGoogleMainCommandAsync()
        {
            await _navigationService.NavigateAsync("/StartPage");
        }
        #endregion
    }
}
