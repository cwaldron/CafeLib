using System;
using System.Linq;
using Android.Content;
using Android.Support.V4.App;
using CafeLib.Mobile.Android.Renderers;
using CafeLib.Mobile.Extensions;
using CafeLib.Mobile.Views;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using View = Android.Views.View;

[assembly: ExportRenderer(typeof(NavigationPage), typeof(NavigationPageRenderer))]

namespace CafeLib.Mobile.Android.Renderers
{
    public class NavigationPageRenderer : Xamarin.Forms.Platform.Android.AppCompat.NavigationPageRenderer, View.IOnClickListener
    {
        public NavigationPageRenderer(Context context)
            : base(context)
        {
        }

        protected override void Dispose(bool disposing)
        {
            try
            {
                base.Dispose(disposing);
            }
            catch (Exception ex)
            {
                // If you have taken a photo then cancel the inspection screen this throws an exception
                // so handle it and not crash the app. This happens every time so no need to log to
                // insights as it will just be noise.
                System.Diagnostics.Debug.WriteLine($"Exception when disposing navigation page - {ex.Message}");
            }
        }

        public new void OnClick(View v)
        {
            var result = false;

            if (Element?.Navigation.Peek() is ISoftNavigationPage page)
            {
                result = page.OnSoftBackButtonPressed();
            }

            if (!result)
            {
                Element?.PopAsync();
            }
        }

        protected override void SetupPageTransition(FragmentTransaction transaction, bool isPush)
        {
            if (isPush)
                transaction.SetCustomAnimations(global::Android.Resource.Animation.SlideInLeft, global::Android.Resource.Animation.SlideInLeft, 0, 0);
            else
                transaction.SetCustomAnimations(0, global::Android.Resource.Animation.SlideOutRight, 0, 0);
        }
    }
}