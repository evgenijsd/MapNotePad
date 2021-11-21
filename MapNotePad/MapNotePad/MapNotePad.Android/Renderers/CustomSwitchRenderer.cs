using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Com.Kyleduo.Switchbutton;
using MapNotePad.Controls;
using MapNotePad.Droid.Renderers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomSwitch), typeof(CustomSwitchRenderer))]
namespace MapNotePad.Droid.Renderers
{
    public class CustomSwitchRenderer : ButtonRenderer
    {

        Context context { get; }

        public CustomSwitchRenderer(Context context) : base(context)
        {
            this.context = context;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Button> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                SwitchButton switchButton = new SwitchButton(context);

                //  switchButton.SetHighlightColor(Android.Graphics.Color.Green);

                switchButton.CheckedChange += SwitchButton_CheckedChange;

                SetNativeControl(switchButton);

            }

        }

        private void SwitchButton_CheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            var customSwitch = Element as CustomSwitch;
            customSwitch.IsToggle = e.IsChecked;

            customSwitch.OnToggled();

        }
    }
}