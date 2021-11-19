using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MapNotePad.Controls
{
    public partial class CustomToolBar : ContentView
    {
        public CustomToolBar()
        {
            InitializeComponent();
        }

        #region -- Public properties --

        public static readonly BindableProperty LeftButtonCommandProperty = BindableProperty.Create(
            propertyName: nameof(LeftButtonCommand),
            returnType: typeof(ICommand),
            declaringType: typeof(CustomToolBar),
            defaultValue: null,
            defaultBindingMode: BindingMode.TwoWay);

        public ICommand LeftButtonCommand
        {
            set => SetValue(LeftButtonCommandProperty, value);
            get => (ICommand)GetValue(LeftButtonCommandProperty);
        }

        public static readonly BindableProperty RightButtonCommandProperty = BindableProperty.Create(
            propertyName: nameof(RightButtonCommand),
            returnType: typeof(ICommand),
            declaringType: typeof(CustomToolBar),
            defaultValue: null,
            defaultBindingMode: BindingMode.TwoWay);

        public ICommand RightButtonCommand
        {
            set => SetValue(RightButtonCommandProperty, value);
            get => (ICommand)GetValue(RightButtonCommandProperty);
        }

        public static readonly BindableProperty TitleProperty = BindableProperty.Create(
            propertyName: nameof(Title),
            returnType: typeof(string),
            declaringType: typeof(CustomToolBar),
            defaultValue: string.Empty,
            defaultBindingMode: BindingMode.TwoWay);

        public string Title
        {
            set => SetValue(TitleProperty, value);
            get => (string)GetValue(TitleProperty);
        }

        public static readonly BindableProperty TitleColorProperty = BindableProperty.Create(
            propertyName: nameof(TitleColor),
            returnType: typeof(Color),
            declaringType: typeof(CustomToolBar),
            defaultValue: Color.Black,
            defaultBindingMode: BindingMode.TwoWay);

        public Color TitleColor
        {
            set => SetValue(TitleColorProperty, value);
            get => (Color)GetValue(TitleColorProperty);
        }

        public static readonly BindableProperty FontFamilyProperty = BindableProperty.Create(
            propertyName: nameof(FontFamily),
            returnType: typeof(string),
            declaringType: typeof(CustomToolBar),
            defaultValue: string.Empty,
            defaultBindingMode: BindingMode.TwoWay);

        public string FontFamily
        {
            set => SetValue(FontFamilyProperty, value);
            get => (string)GetValue(FontFamilyProperty);
        }

        public static readonly BindableProperty LeftImageSourceProperty = BindableProperty.Create(
            propertyName: nameof(LeftImageSource),
            returnType: typeof(string),
            declaringType: typeof(CustomToolBar),
            defaultValue: string.Empty,
            defaultBindingMode: BindingMode.TwoWay);

        public string LeftImageSource
        {
            set => SetValue(LeftImageSourceProperty, value);
            get => (string)GetValue(LeftImageSourceProperty);
        }

        public static readonly BindableProperty RightImageSourceProperty = BindableProperty.Create(
            propertyName: nameof(RightImageSource),
            returnType: typeof(string),
            declaringType: typeof(CustomToolBar),
            defaultValue: string.Empty,
            defaultBindingMode: BindingMode.TwoWay);

        public string RightImageSource
        {
            set => SetValue(RightImageSourceProperty, value);
            get => (string)GetValue(RightImageSourceProperty);
        }

        public static readonly BindableProperty IsEnabledRightButtonProperty = BindableProperty.Create(
            propertyName: nameof(IsEnabledRightButton),
            returnType: typeof(bool),
            declaringType: typeof(CustomToolBar),
            defaultValue: true,
            defaultBindingMode: BindingMode.TwoWay);

        public bool IsEnabledRightButton
        {
            set => SetValue(IsEnabledRightButtonProperty, value);
            get => (bool)GetValue(IsEnabledRightButtonProperty);
        }

        public static readonly BindableProperty LineColorProperty = BindableProperty.Create(
            propertyName: nameof(LineColor),
            returnType: typeof(Color),
            declaringType: typeof(CustomToolBar),
            defaultValue: Color.White,
            defaultBindingMode: BindingMode.TwoWay);

        public Color LineColor
        {
            set => SetValue(LineColorProperty, value);
            get => (Color)GetValue(LineColorProperty);
        }

        #endregion

    }
}