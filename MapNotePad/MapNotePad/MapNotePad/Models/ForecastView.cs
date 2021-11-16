using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;

namespace MapNotePad.Models
{
    public class ForecastView : BindableBase
    {
        private string _day;
        public string Day
        {
            get => _day;
            set => SetProperty(ref _day, value);
        }

        private string _image;
        public string Image
        {
            get => _image;
            set => SetProperty(ref _image, value);
        }

        private string _tempMax;
        public string TempMax
        {
            get => _tempMax;
            set => SetProperty(ref _tempMax, value);
        }

        private string _tempMin;
        public string TempMin
        {
            get => _tempMin;
            set => SetProperty(ref _tempMin, value);
        }
    }
}
