using MapNotePad.Helpers;
using MapNotePad.Services.Interface;
using Prism.Navigation;
using Prism.Services;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MapNotePad.ViewModels
{
    public class SettingsPageViewModel : BaseViewModel
    {
        private IPageDialogService _dialogs { get; }
        private ISettings _settings;

        public SettingsPageViewModel(INavigationService navigationService, IPageDialogService dialogs, ISettings settings) : base(navigationService)
        {
            _dialogs = dialogs;
            _settings = settings;
        }

        #region -- Public properties --
        private bool _theme;
        public bool Theme
        {
            get { return _theme; }
            set { SetProperty(ref _theme, value); }
        }

        private ICommand _goBackCommand;
        public ICommand GoBackCommand => _goBackCommand ??= SingleExecutionCommand.FromFunc(OnGoBackCommandAsync);
        private ICommand _ThemeLightCommand;
        public ICommand ThemeLightCommand => _ThemeLightCommand ??= SingleExecutionCommand.FromFunc(OnThemeLightCommandAsync);
        private ICommand _ThemeDarkCommand;
        public ICommand ThemeDarkCommand => _ThemeDarkCommand ??= SingleExecutionCommand.FromFunc(OnThemeDarkCommandAsync);
        #endregion
        #region -- InterfaceName implementation --
        #endregion
        #region -- Overrides --
        #endregion
        #region -- Public helpers --
        #endregion
        #region -- Private helpers --
        private async Task OnGoBackCommandAsync()
        {
            await _navigationService.GoBackAsync();
        }

        private Task OnThemeLightCommandAsync()
        {
            //await _dialogs.DisplayAlertAsync("Alert", "Light", "Ok");
            _settings.ThemeSet = _settings.ChangeTheme(Theme);

            return Task.CompletedTask;
        }

        private async Task OnThemeDarkCommandAsync()
        {
            await _dialogs.DisplayAlertAsync("Alert", "Dark", "Ok");
        }

        #endregion
    }
}
