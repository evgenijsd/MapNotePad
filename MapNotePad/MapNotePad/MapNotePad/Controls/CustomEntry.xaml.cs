using MapNotePad.Helpers;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace MapNotePad.Controls
{
    public partial class CustomEntry : Frame
    {
        public CustomEntry()
        {
            InitializeComponent();
        }

        #region -- Public properties --

        public static readonly BindableProperty TextProperty = BindableProperty.Create(
            propertyName: nameof(Text),
            returnType: typeof(string),
            declaringType: typeof(CustomEntry),
            defaultValue: string.Empty,
            defaultBindingMode: BindingMode.TwoWay);

        public string Text
        {
            set => SetValue(TextProperty, value);
            get => (string)GetValue(TextProperty);
        }

        public static readonly BindableProperty TextColorProperty = BindableProperty.Create(
            propertyName: nameof(TextColor),
            returnType: typeof(Color),
            declaringType: typeof(CustomEntry),
            defaultValue: Color.Silver,
            defaultBindingMode: BindingMode.TwoWay);

        public Color TextColor
        {
            set => SetValue(TextColorProperty, value);
            get => (Color)GetValue(TextColorProperty);
        }

        public static readonly BindableProperty FontFamilyProperty = BindableProperty.Create(
            propertyName: nameof(FontFamily),
            returnType: typeof(string),
            declaringType: typeof(CustomEntry),
            defaultValue: string.Empty,
            defaultBindingMode: BindingMode.TwoWay);

        public string FontFamily
        {
            set => SetValue(FontFamilyProperty, value);
            get => (string)GetValue(FontFamilyProperty);
        }

        public static readonly BindableProperty PlaceholderProperty = BindableProperty.Create(
            propertyName: nameof(Placeholder),
            returnType: typeof(string),
            declaringType: typeof(CustomEntry),
            defaultValue: string.Empty,
            defaultBindingMode: BindingMode.TwoWay);

        public string Placeholder
        {
            set => SetValue(PlaceholderProperty, value);
            get => (string)GetValue(PlaceholderProperty);
        }

        public static readonly BindableProperty PlaceholderColorProperty = BindableProperty.Create(
            propertyName: nameof(PlaceholderColor),
            returnType: typeof(Color),
            declaringType: typeof(CustomEntry),
            defaultValue: Color.Silver,
            defaultBindingMode: BindingMode.TwoWay);

        public Color PlaceholderColor
        {
            set => SetValue(PlaceholderColorProperty, value);
            get => (Color)GetValue(PlaceholderColorProperty);
        }

        public static readonly BindableProperty IsPasswordProperty = BindableProperty.Create(
            propertyName: nameof(IsPassword),
            returnType: typeof(bool),
            declaringType: typeof(CustomEntry),
            defaultValue: false,
            defaultBindingMode: BindingMode.TwoWay);

        public bool IsPassword
        {
            set => SetValue(IsPasswordProperty, value);
            get => (bool)GetValue(IsPasswordProperty);
        }

        public static readonly BindableProperty IsPasswordHideProperty = BindableProperty.Create(
            propertyName: nameof(IsPasswordHide),
            returnType: typeof(bool),
            declaringType: typeof(CustomEntry),
            defaultValue: false,
            defaultBindingMode: BindingMode.TwoWay);

        public bool IsPasswordHide
        {
            set => SetValue(IsPasswordHideProperty, value);
            get => (bool)GetValue(IsPasswordHideProperty);
        }

        public static readonly BindableProperty IsButtonVisibleProperty = BindableProperty.Create(
            propertyName: nameof(IsButtonVisible),
            returnType: typeof(bool),
            declaringType: typeof(CustomEntry),
            defaultValue: false,
            defaultBindingMode: BindingMode.TwoWay);

        public bool IsButtonVisible
        {
            set => SetValue(IsButtonVisibleProperty, value);
            get => (bool)GetValue(IsButtonVisibleProperty);
        }

        public static readonly BindableProperty ClearImageSourceProperty = BindableProperty.Create(
            propertyName: nameof(ClearImageSource),
            returnType: typeof(string),
            declaringType: typeof(CustomEntry),
            defaultValue: string.Empty,
            defaultBindingMode: BindingMode.TwoWay);

        public string ClearImageSource
        {
            set => SetValue(ClearImageSourceProperty, value);
            get => (string)GetValue(ClearImageSourceProperty);
        }

        public static readonly BindableProperty EyeOnImageSourceProperty = BindableProperty.Create(
            propertyName: nameof(EyeOnImageSource),
            returnType: typeof(string),
            declaringType: typeof(CustomEntry),
            defaultValue: string.Empty,
            defaultBindingMode: BindingMode.TwoWay);

        public string EyeOnImageSource
        {
            set => SetValue(EyeOnImageSourceProperty, value);
            get => (string)GetValue(EyeOnImageSourceProperty);
        }

        public static readonly BindableProperty EyeOffImageSourceProperty = BindableProperty.Create(
            propertyName: nameof(EyeOffImageSource),
            returnType: typeof(string),
            declaringType: typeof(CustomEntry),
            defaultValue: string.Empty,
            defaultBindingMode: BindingMode.TwoWay);

        public string EyeOffImageSource
        {
            set => SetValue(EyeOffImageSourceProperty, value);
            get => (string)GetValue(EyeOffImageSourceProperty);
        }

        public static readonly BindableProperty ImageSourceProperty = BindableProperty.Create(
            propertyName: nameof(ImageSource),
            returnType: typeof(string),
            declaringType: typeof(CustomEntry),
            defaultValue: string.Empty,
            defaultBindingMode: BindingMode.TwoWay);

        public string ImageSource
        {
            set => SetValue(ImageSourceProperty, value);
            get => (string)GetValue(ImageSourceProperty);
        }

        private ICommand _buttonCommand;
        public ICommand ButtonCommand => _buttonCommand ??= SingleExecutionCommand.FromFunc(OnButtonCommandAsync);

        private ICommand _focusedCommand;
        public ICommand FocusedCommand => _focusedCommand ??= SingleExecutionCommand.FromFunc(OnFocusedCommandAsync);

        private ICommand _unfocusedCommand;
        public ICommand UnFocusedCommand=> _unfocusedCommand ??= SingleExecutionCommand.FromFunc(OnUnfocusedCommandAsync);

        #endregion

        #region -- Overrides --

        protected override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);

            switch (propertyName)
            {
                case nameof(IsPassword):
                    IsPasswordHide = IsPassword;
                    break;
                case nameof(Text):
                case nameof(ClearImageSource):
                case nameof(EyeOnImageSource):
                case nameof(EyeOffImageSource):
                    
                    if (IsPassword)
                    {
                        if (IsPasswordHide)
                        {
                            ImageSource = EyeOnImageSource;
                        }
                        else
                        {
                            ImageSource = EyeOffImageSource;
                        }

                        if (string.IsNullOrEmpty(Text))
                        {
                            IsButtonVisible = false;
                        }
                        else
                        {
                            IsButtonVisible = true;
                        }
                    }
                    else
                    {
                        ImageSource = ClearImageSource;
                    }

                    break;
            }
        }
        
        #endregion

        #region -- Private methods --

        private Task OnButtonCommandAsync()
        {
            if (IsPassword)
            {
                if (ImageSource == EyeOnImageSource)
                {
                    IsPasswordHide = false;
                    ImageSource = EyeOffImageSource;
                }
                else
                {
                    IsPasswordHide = true;
                    ImageSource = EyeOnImageSource;
                }
            }
            else
            {
                Text = string.Empty;
            }

            return Task.CompletedTask;
        }

        private Task OnFocusedCommandAsync()
        {
            if (!IsPassword)
            {
                IsButtonVisible = true;
            }

            return Task.CompletedTask;
        }

        private Task OnUnfocusedCommandAsync()
        {
            if (!IsPassword)
            {
                IsButtonVisible = false;
            }

            return Task.CompletedTask;
        }

        #endregion

    }
}