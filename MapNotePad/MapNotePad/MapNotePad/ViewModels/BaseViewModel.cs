using Prism.Mvvm;
using Prism.Navigation;
using System.Threading.Tasks;

namespace MapNotePad.ViewModels
{
    public abstract class BaseViewModel : BindableBase, INavigationAware, IInitialize
    {
        protected INavigationService _navigationService { get; }

        public BaseViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        #region -- InterfaceName implementation --
        public virtual void OnNavigatedFrom(INavigationParameters parameters)
        {
        }

        public virtual void OnNavigatedTo(INavigationParameters parameters)
        {
        }

        public virtual void Initialize(INavigationParameters parameters)
        {
        }
        #endregion
    }
}
