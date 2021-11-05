using MapNotePad.Helpers;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
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
        private ICommand _MainPageCommand;
        public ICommand MainPageCommand => _MainPageCommand ??= SingleExecutionCommand.FromFunc(OnMainPageCommandAsync);
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
        private Task OnMainPageCommandAsync()
        {
            _navigationService.NavigateAsync("MainPage");
            return Task.CompletedTask;
        }
        private Task OnGoogleMainCommandAsync()
        {
            _navigationService.NavigateAsync("MainPage");
            return Task.CompletedTask;
        }
        #endregion
    }
}
