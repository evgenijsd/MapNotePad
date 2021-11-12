﻿using MapNotePad.Helpers;
using Prism.Navigation;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MapNotePad.ViewModels
{
    public class RegisterViewModel : BaseContentPage
    {
        public RegisterViewModel(INavigationService navigationService) : base(navigationService)
        {
        }

        #region -- Public properties --
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
        private Task OnPasswordCommandAsync()
        {
            _navigationService.NavigateAsync("Password");
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
