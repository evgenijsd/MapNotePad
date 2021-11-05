using MapNotePad.Helpers;
using Prism.Navigation;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace MapNotePad.ViewModels
{
    public class StartPageViewModel : BaseContentPage
    {
        public StartPageViewModel(INavigationService navigationService) : base(navigationService)
        {
        }


        #region -- Public properties --

        private ICommand _LogInCommand;
        public ICommand LogInCommand => _LogInCommand ??= SingleExecutionCommand.FromFunc(OnLogInCommandAsync);
        private ICommand _RegisterCommand;
        public ICommand RegisterCommand => _RegisterCommand ??= SingleExecutionCommand.FromFunc(OnRegisterCommandAsync);
        #endregion
        #region -- InterfaceName implementation --
        #endregion
        #region -- Overrides --
        #endregion
        #region -- Public helpers --

        #endregion
        #region -- Private helpers --
        private Task OnLogInCommandAsync()
        {
            _navigationService.NavigateAsync("LogIn");
            return Task.CompletedTask;
        }
        private Task OnRegisterCommandAsync()
        {
            _navigationService.NavigateAsync("Register");
            return Task.CompletedTask;
        }
        #endregion
    }
}
