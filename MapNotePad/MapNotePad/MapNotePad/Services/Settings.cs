using MapNotePad.Enum;
using MapNotePad.Resources;
using MapNotePad.Services.Interface;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;
using System.Threading;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MapNotePad.Services
{
    public class Settings : ISettings
    {
        #region -- Public properties --
        public int ThemeSet
        {
            get => Preferences.Get(nameof(ThemeSet), (int)EThemeType.LightTheme);
            set => Preferences.Set(nameof(ThemeSet), value);
        }

        public int LangSet
        {
            get => Preferences.Get(nameof(LangSet), (int)ELangType.English);
            set => Preferences.Set(nameof(LangSet), value);
        }
        #endregion


        #region -- Public helpers --
        public int ChangeTheme(bool theme)
        {
            int result = (int)EThemeType.LightTheme;
            if (theme)
            {
                App.Current.UserAppTheme = OSAppTheme.Dark;
                result = (int)EThemeType.DarkTheme;
            }
            else
            {
                App.Current.UserAppTheme = OSAppTheme.Unspecified;
            }
            return result;
        }

        public string Language(ELangType language)
        {
            string result = "en-US";
            switch (language)
            {
                case ELangType.English:
                    result = "en-US";
                    Thread.CurrentThread.CurrentCulture = new CultureInfo(result);
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo(result);
                    break;
                case ELangType.Russian:
                    result = "ru-RU";
                    Thread.CurrentThread.CurrentCulture = new CultureInfo(result);
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo(result);
                    break;
            }
            return result;
        }

        public ObservableCollection<LangModel> GetLanguages()
        {
            var Lang = new ObservableCollection<LangModel>()
            {
                new LangModel {Key=(int)ELangType.English, Lang=Resources.Resource.LangEng},
                new LangModel {Key=(int)ELangType.Russian, Lang=Resources.Resource.LangRus}
            };
            return Lang;
        }
        #endregion

    }
}

