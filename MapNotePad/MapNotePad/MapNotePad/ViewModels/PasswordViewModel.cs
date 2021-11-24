using System.Threading.Tasks;
using System.Windows.Input;
using MapNotePad.Enum;
using MapNotePad.Helpers;
using MapNotePad.Models;
using MapNotePad.Services.Interface;
using MapNotePad.Views;
using Prism.Navigation;
using Prism.Services;
using Xamarin.Forms;

namespace MapNotePad.ViewModels
{
    public class PasswordViewModel : BaseViewModel
    {
        private IPageDialogService _dialogs { get; }
        private IRegistration _registration;

        public PasswordViewModel(INavigationService navigationService, IRegistration registration, IPageDialogService dialogs) : base(navigationService)
        {
            _registration = registration;
            _dialogs = dialogs;
        }

        #region -- Public properties --
        private string _password = string.Empty;
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }
        private string _confirmPassword = string.Empty;
        public string ConfirmPassword
        {
            get => _confirmPassword;
            set => SetProperty(ref _confirmPassword, value);
        }
        private bool _isMismatchPassword = false;
        public bool IsMismatchPassword
        {
            get => _isMismatchPassword;
            set => SetProperty(ref _isMismatchPassword, value);
        }
        private Users _user;
        public Users User
        {
            get => _user;
            set => SetProperty(ref _user, value);
        }

        private ICommand _goBackCommand;
        public ICommand GoBackCommand => _goBackCommand ??= SingleExecutionCommand.FromFunc(OnGoBackCommandAsync);
        private ICommand _LogInCommand;
        public ICommand LogInCommand => _LogInCommand ??= SingleExecutionCommand.FromFunc(OnLogInCommandAsync);
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
                User = parameters.GetValue<Users>("User");
            }
        }
        #endregion
        #region -- Public helpers --

        #endregion
        #region -- Private helpers --
        private async Task OnLogInCommandAsync()
        {
            var check = (ECheckEnter)_registration.CheckTheCorrectPassword(Password, ConfirmPassword);
            switch (check)
            {
                case ECheckEnter.PasswordBigLetterAndDigit:
                    await _dialogs.DisplayAlertAsync("Alert", "The password must contain a big letter and digit", "Ok");
                    break;
                case ECheckEnter.PasswordLengthNotValid:
                    await _dialogs.DisplayAlertAsync("Alert", "Login less than 6 characters", "Ok");
                    break;
                case ECheckEnter.PasswordsNotEqual:
                    await _dialogs.DisplayAlertAsync("Alert", "Password and confirm password do not coincide", "Ok");
                    break;
                default:
                    {
                        User.Password = Password;
                        int result = await _registration.UserAddAsync(User);
                        if (result > 0)
                        {
                            var p = new NavigationParameters { { "User", User } };
                            await _navigationService.NavigateAsync($"{nameof(LogIn)}", p);
                        }
                        else await _dialogs.DisplayAlertAsync("Alert", "Database failure!", "Ok");
                    }
                    break;
            }
        }
        private Task OnGoogleMainCommandAsync()
        {
            _navigationService.NavigateAsync("StartPage");
            return Task.CompletedTask;
        }
        private async Task OnGoBackCommandAsync()
        {
            await _navigationService.GoBackAsync();
        }
        private void OnErrorCommand()
        {
            IsMismatchPassword = !string.IsNullOrEmpty(Password) && _registration.CheckTheCorrectPassword(Password, ConfirmPassword) == (int)ECheckEnter.PasswordsNotEqual;
        }

        #endregion
    }
}
