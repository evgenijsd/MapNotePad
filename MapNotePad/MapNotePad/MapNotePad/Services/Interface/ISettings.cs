using MapNotePad.Enum;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace MapNotePad.Services.Interface
{
    public interface ISettings
    {
        int ThemeSet { get; set; }
        int LangSet { get; set; }
        int ChangeTheme(bool theme);
        string Language(LangType language);
        ObservableCollection<LangModel> GetLanguages();
    }

    public class LangModel
    {
        public int Key { get; set; }
        public string Lang { get; set; }
    }
}
