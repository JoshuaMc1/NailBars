using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using NailBars.Droid.Resources.statusbar;
using NailBars.VistasModelo;
using Plugin.CurrentActivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(StatusBar))]
namespace NailBars.Droid.Resources.statusbar
{
    class StatusBar : VMStatusBar
    {
        WindowManagerFlags originalFlags;
        Window GetCurrentWindow()
        {
            var window = CrossCurrentActivity.Current.Activity.Window;
            window.ClearFlags(WindowManagerFlags.TranslucentStatus);
            window.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);
            return window;
        }
        public void CambiarColor()
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.M)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    var currentWindow = GetCurrentWindow();
                    currentWindow.DecorView.SystemUiVisibility = (StatusBarVisibility)SystemUiFlags.LayoutStable;
                    currentWindow.SetStatusBarColor(Android.Graphics.Color.Rgb(255, 64, 129));
                });
            }
        }

        public void MostrarStatusBar()
        {
            var activity = (Activity)Forms.Context;
            var attrs = activity.Window.Attributes;
            attrs.Flags = originalFlags;
            activity.Window.Attributes = attrs;
        }

        public void OcultarStatusBar()
        {
            var activity = (Activity)Forms.Context;
            var attrs = activity.Window.Attributes;
            originalFlags = attrs.Flags;
            attrs.Flags = WindowManagerFlags.Fullscreen;
            activity.Window.Attributes = attrs;
        }

        public void Translucido()
        {
            var activity = (Activity)Forms.Context;
            var attrs = activity.Window.Attributes;
            originalFlags = attrs.Flags;
            attrs.Flags = WindowManagerFlags.TranslucentStatus;
            activity.Window.Attributes = attrs;
        }

        public void Transparente()
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.M)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    var currentWindow = GetCurrentWindow();
                    currentWindow.DecorView.SystemUiVisibility = (StatusBarVisibility)SystemUiFlags.LayoutFullscreen;
                    currentWindow.SetStatusBarColor(Android.Graphics.Color.Transparent);
                });
            }
        }
    }
}