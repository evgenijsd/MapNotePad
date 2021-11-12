using Prism.Mvvm;
using Prism.Navigation;

namespace MapNotePad.ViewModels
{
    public abstract class BaseContentPage : BindableBase
    {
        protected INavigationService _navigationService { get; }
        public BaseContentPage(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        #region -- Public properties --
        #endregion
        #region -- InterfaceName implementation --
        #endregion
        #region -- Overrides --
        #endregion
        #region -- Public helpers --
        #endregion
        #region -- Private helpers --
        #endregion
    }
}
