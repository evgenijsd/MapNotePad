using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace MapNotePad.ViewModels
{
    public class LogInViewModel : BaseContentPage
    {
        public LogInViewModel(INavigationService navigationService) : base(navigationService)
        {
        }

        #region -- Public properties --

        public ICommand MainPageCommand => new Command(OnMainPageCommandAsync);
        public ICommand GoogleMainCommand => new Command(OnGoogleMainCommandAsync);
        #endregion
        #region -- InterfaceName implementation --
        #endregion
        #region -- Overrides --
        #endregion
        #region -- Public helpers --

        #endregion
        #region -- Private helpers --
        private async void OnMainPageCommandAsync()
        {
            await _navigationService.NavigateAsync("MainPage");
        }
        private async void OnGoogleMainCommandAsync()
        {
            await _navigationService.NavigateAsync("MainPage");
        }
        #endregion
    }
}
