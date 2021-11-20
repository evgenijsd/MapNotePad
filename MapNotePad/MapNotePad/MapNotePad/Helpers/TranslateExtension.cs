using MapNotePad.Enum;
using MapNotePad.Services;
using MapNotePad.Services.Interface;
using System;
using System.Globalization;
using System.Reflection;
using System.Resources;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MapNotePad.Helpers
{
    [ContentProperty("Text")]
    public class TranslateExtension : IMarkupExtension
    {
        private readonly CultureInfo ci;
        const string ResourceId = "MapNotePad.Resources.Resource";
        private ISettings _settings { get; set; }

        public TranslateExtension()
        {
            _settings = new Settings();
            ci = new CultureInfo(_settings.Language((LangType)_settings.LangSet));
        }

        public string Text { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Text == null)
                return "";

            ResourceManager resmgr = new ResourceManager(ResourceId,
                        typeof(TranslateExtension).GetTypeInfo().Assembly);

            var translation = resmgr.GetString(Text, ci);

            if (translation == null)
            {
                translation = Text;
            }
            return translation;
        }
    }
}
