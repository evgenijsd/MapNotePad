using Android.App;
using Android.Runtime;
using Plugin.CurrentActivity;
using System;

namespace MapNotePad.Droid
{
    [Application]
    [MetaData("com.google.android.maps.v2.API_KEY", Value = AppConstants.GOOGLE_API_ACCESS_KEY)]
    public class MainApplication : Application
    {
        public MainApplication(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

        public override void OnCreate()
        {
            base.OnCreate();
            CrossCurrentActivity.Current.Init(this);
        }
    }
}