using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace MapNotePad.Controls
{
    public class CustomSwitch : Button
    {
        public CustomSwitch() : base()
        {
        }
        public CustomSwitch(bool isInit) : base()
        {
            IsInit = isInit;
        }

        public static readonly BindableProperty IsInitProperty =
            BindableProperty.Create(
                propertyName: nameof(IsInit),
                returnType: typeof(bool),
                declaringType: typeof(CustomSwitch),
                defaultValue: true,
                defaultBindingMode: BindingMode.TwoWay);

        public bool IsInit
        {
            get { return (bool)GetValue(IsInitProperty); }
            set { SetValue(IsInitProperty, value); }
        }

        public bool IsToggle { get; set; }

        public event EventHandler Toggled;

        public void OnToggled() => Toggled?.Invoke(this, null);
    }
}
