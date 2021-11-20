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
            get => Preferences.Get(nameof(ThemeSet), (int)ThemeType.LightTheme);
            set => Preferences.Set(nameof(ThemeSet), value);
        }

        public int LangSet
        {
            get => Preferences.Get(nameof(LangSet), (int)LangType.English);
            set => Preferences.Set(nameof(LangSet), value);
        }

        public int ChangeTheme(bool theme)
        {
            int result = (int)ThemeType.LightTheme;
            if (theme)
            {
                App.Current.UserAppTheme = OSAppTheme.Dark;
                result = (int)ThemeType.DarkTheme;
            }
            else
            {
                App.Current.UserAppTheme = OSAppTheme.Unspecified;
            }
            return result;
        }
        #endregion

        public string Language(LangType language)
        {
            string result = "en-US";
            switch (language)
            {
                case LangType.English:
                    result = "en-US";
                    Thread.CurrentThread.CurrentCulture = new CultureInfo(result);
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo(result);
                    break;
                case LangType.Russian:
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
                new LangModel {Key=(int)LangType.English, Lang=Resources.Resource.LangEng},
                new LangModel {Key=(int)LangType.Russian, Lang=Resources.Resource.LangRus}
            };
            return Lang;
        }
    }
}

