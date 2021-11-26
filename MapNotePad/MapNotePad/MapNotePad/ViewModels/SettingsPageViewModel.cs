using MapNotePad.Enum;
using MapNotePad.Helpers;
using MapNotePad.Services.Interface;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MapNotePad.ViewModels
{
    public class SettingsPageViewModel : BaseViewModel
    {
        private IPageDialogService _dialogs { get; }
        private ISettings _settings;
        private bool isEnable = false;

        public SettingsPageViewModel(INavigationService navigationService, IPageDialogService dialogs, ISettings settings) : base(navigationService)
        {
            _dialogs = dialogs;
            _settings = settings;
            Theme = _settings.ThemeSet != (int)EThemeType.LightTheme;
        }

        #region -- Public properties --
        private bool _theme = false;
        public bool Theme
        {
            get { return _theme; }
            set { SetProperty(ref _theme, value); }
        }

        private ICommand _GoBackCommand;
        public ICommand GoBackCommand => _GoBackCommand ??= SingleExecutionCommand.FromFunc(OnGoBackCommandAsync);
        private ICommand _ThemeiOSCommand;
        public ICommand ThemeiOSCommand => _ThemeiOSCommand ??= SingleExecutionCommand.FromFunc(OnThemeiOSCommandAsync);
        private ICommand _ThemeCommand;
        public ICommand ThemeCommand => _ThemeCommand ??= SingleExecutionCommand.FromFunc(OnThemeCommandAsync);
        #endregion

        #region -- Overrides --
        public override async void Initialize(INavigationParameters parameters)
        {
            await Task.Delay(TimeSpan.FromSeconds(0.1));
            isEnable = true;
        }
        #endregion

        #region -- Private helpers --
        private async Task OnGoBackCommandAsync()
        {
            var p = new NavigationParameters { { "Theme", Theme } };
            await _navigationService.GoBackAsync(p);
        }

        private Task OnThemeiOSCommandAsync()
        {
            _settings.ThemeSet = _settings.ChangeTheme(Theme);
            return Task.CompletedTask;
        }

        private Task OnThemeCommandAsync()
        {
            if (isEnable)
            {
                Theme = !Theme;
                _settings.ThemeSet = _settings.ChangeTheme(Theme);
            }
            return Task.CompletedTask;
        }
        #endregion
    }
}
