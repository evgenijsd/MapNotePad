using MapNotePad.Helpers;
using MapNotePad.Views;
using Prism.Navigation;
using System.Threading.Tasks;
using System.Windows.Input;

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
        private async Task<INavigationResult> OnLogInCommandAsync()
        {
                return await _navigationService.NavigateAsync($"{nameof(LogIn)}");
        }
        private async Task<INavigationResult> OnRegisterCommandAsync()
        {
            return await _navigationService.NavigateAsync($"{nameof(Register)}");
        }
        #endregion
    }
}
