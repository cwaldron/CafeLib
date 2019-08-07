using CafeLib.Mobile.Effects;
using Xamarin.Forms;

// ReSharper disable UnusedMember.Global

namespace CafeLib.Mobile.Views
{
    public class BaseContentView : ContentView
    {
        public BaseContentView()
        {
            var lifecycleEffect = new ViewLifecycleEffect();
            lifecycleEffect.Loaded += (s, e) => OnLoad();
            lifecycleEffect.Unloaded += (s, e) => OnUnload();
            Effects.Add(lifecycleEffect);
        }

        protected virtual void OnLoad()
        {
        }

        protected virtual void OnUnload()
        {
        }
    }
}
