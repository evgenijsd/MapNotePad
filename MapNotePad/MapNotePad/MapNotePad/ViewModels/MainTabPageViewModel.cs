using System;
using Prism.Navigation;

namespace MapNotePad.ViewModels
{
    public class MainTabPageViewModel : BaseContentPage, IInitialize
    {
        public MainTabPageViewModel(INavigationService navigationService) : base(navigationService)
        {
        }

        public void Initialize(INavigationParameters parameters)
        {
        }
    }
}
