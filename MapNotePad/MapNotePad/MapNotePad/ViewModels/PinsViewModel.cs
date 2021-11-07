using MapNotePad.Helpers;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms.GoogleMaps;
using Xamarin.Forms.GoogleMaps.Bindings;

namespace MapNotePad.ViewModels
{
    public class PinsViewModel : BindableBase, IInitialize
    {
        public void Initialize(INavigationParameters parameters)
        {

        }

        #region -- Public properties --

        #endregion
        #region -- InterfaceName implementation --
        #endregion
        #region -- Overrides --
        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);

        }
        #endregion
        #region -- Public helpers --
        #endregion
        #region -- Private helpers --

        #endregion
    }
}
