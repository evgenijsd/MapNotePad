using MapNotePad.Helpers;
using MapNotePad.Models;
using MapNotePad.Services.Interface;
using MapNotePad.Views;
using Prism.Navigation;
using Prism.Services;
using System.Threading.Tasks;
using System.Windows.Input;
using static MapNotePad.Enum.CheckType;

namespace MapNotePad.ViewModels
{
    public class RegisterViewModel : BaseViewModel
    {
        private IPageDialogService _dialogs { get; }
        private IRegistration _registration;

        public RegisterViewModel(INavigationService navigationService, IRegistration registration, IPageDialogService dialogs) : base(navigationService)
        {
            _registration = registration;
            _dialogs = dialogs;
        }

        #region -- Public properties --
        private string _name = string.Empty;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }
        private string _email = string.Empty;
        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        private ICommand _goBackCommand;
        public ICommand GoBackCommand => _goBackCommand ??= SingleExecutionCommand.FromFunc(OnGoBackCommandAsync);
        private ICommand _PasswordCommand;
        public ICommand PasswordCommand => _PasswordCommand ??= SingleExecutionCommand.FromFunc(OnPasswordCommandAsync);
        private ICommand _GoogleMainCommand;
        public ICommand GoogleMainCommand => _GoogleMainCommand ??= SingleExecutionCommand.FromFunc(OnGoogleMainCommandAsync);
        #endregion
        #region -- InterfaceName implementation --
        #endregion
        #region -- Overrides --
        #endregion
        #region -- Public helpers --

        #endregion
        #region -- Private helpers --
        private async Task OnPasswordCommandAsync()
        {
            var check = (CheckEnter)await _registration.CheckTheCorrectEmailAsync(Name, Email);
            switch (check)
            {
                case CheckEnter.LoginExist:
                    await _dialogs.DisplayAlertAsync("Alert", "This login is already taken", "Ok");
                    break;
                case CheckEnter.EmailANotVaid:
                    await _dialogs.DisplayAlertAsync("Alert", "In Email there is no symbol @", "Ok");
                    break;
                case CheckEnter.EmailLengthNotValid:
                    await _dialogs.DisplayAlertAsync("Alert", "In email name and domain no more than 64 characters", "Ok");
                    break;
                    
                default:
                    {
                        var user = new Users();
                        user.Email = Email;
                        user.Name = Name;
                        var p = new NavigationParameters { { "User", user } };
                        await _navigationService.NavigateAsync($"{nameof(Password)}", p);
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

        #endregion
    }
}
