using Prism.Navigation;
using System.ComponentModel;

namespace MapNotePad.ViewModels
{
    public class PinPageViewModel : BaseContentPage
    {
        public PinPageViewModel(INavigationService navigationService) : base(navigationService)
        {
        }

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
