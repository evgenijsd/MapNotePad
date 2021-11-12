using MapNotePad.Helpers;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MapNotePad.ViewModels
{
    public class PasswordViewModel : BaseContentPage
    {
        public PasswordViewModel(INavigationService navigationService) : base(navigationService)
        {
        }

        #region -- Public properties --
        private ICommand _LogInCommand;
        public ICommand LogInCommand => _LogInCommand ??= SingleExecutionCommand.FromFunc(OnLogInCommandAsync);
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
        private Task OnLogInCommandAsync()
        {
            _navigationService.NavigateAsync("LogIn");
            return Task.CompletedTask;
        }
        private Task OnGoogleMainCommandAsync()
        {
            _navigationService.NavigateAsync("StartPage");
            return Task.CompletedTask;
        }
        #endregion
    }
}
