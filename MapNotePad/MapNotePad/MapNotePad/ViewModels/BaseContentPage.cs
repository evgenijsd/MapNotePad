using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;

namespace MapNotePad.ViewModels
{
    public class BaseContentPage : BindableBase
    {
        private INavigationService _navigationService { get; }
        public BaseContentPage(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

    }
}
