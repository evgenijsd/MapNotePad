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

        private int _userId;
        public int UserId
        {
            get => _userId;
            set => SetProperty(ref _userId, value);
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

        private string _nickname;
        public string Nickname
        {
            get => _nickname;
            set => SetProperty(ref _nickname, value);
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
