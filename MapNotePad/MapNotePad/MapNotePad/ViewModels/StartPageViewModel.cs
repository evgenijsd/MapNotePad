using Prism.Navigation;
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

        public ICommand LogInCommand => new Command(OnLogInCommandAsync);
        public ICommand RegisterCommand => new Command(OnRegisterCommandAsync);
        #endregion
        #region -- InterfaceName implementation --
        #endregion
        #region -- Overrides --
        #endregion
        #region -- Public helpers --

        #endregion
        #region -- Private helpers --
        private async void OnLogInCommandAsync()
        {
            await _navigationService.NavigateAsync("LogIn");
        }
        private async void OnRegisterCommandAsync()
        {
            await _navigationService.NavigateAsync("Register");
        }
        #endregion
    }
}
