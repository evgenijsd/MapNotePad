using Prism.Mvvm;
using System;
using System.Windows.Input;

namespace MapNotePad.Models
{
    public class PinView : BindableBase
    {
        public PinView()
        {
        }

        private int _id;
        public int Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        private int _user;
        public int User
        {
            get => _user;
            set => SetProperty(ref _user, value);
        }

        private bool _favourite;
        public bool Favourite
        {
            get => _favourite;
            set => SetProperty(ref _favourite, value);
        }

        private string _image;
        public string Image
        {
            get => _image;
            set => SetProperty(ref _image, value);
        }

        private string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private double _latitude;
        public double Latitude
        {
            get => _latitude;
            set => SetProperty(ref _latitude, value);
        }

        private double _longitude;
        public double Longitude
        {
            get => _longitude;
            set => SetProperty(ref _longitude, value);
        }

        private string _description;
        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        private DateTime _date;
        public DateTime Date
        {
            get => _date;
            set => SetProperty(ref _date, value);
        }

        private ICommand _DeleteCommand;
        public ICommand DeleteCommand
        {
            get => _DeleteCommand;
            set => SetProperty(ref _DeleteCommand, value);
        }

        private ICommand _EditCommand;
        public ICommand EditCommand
        {
            get => _EditCommand;
            set => SetProperty(ref _EditCommand, value);
        }
    }
}
