using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace MapNotePad.Controls
{
    public class CustomSwitch : Button
    {
        public bool IsToggle { get; set; }


        public event EventHandler Toggled;

        public void OnToggled() =>
        Toggled?.Invoke(this, null);
    }
}
