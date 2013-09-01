#if RAZER
using System.Windows;
using Sharparam.SharpBlade.Razer;
using Sharparam.SharpBlade.Native;

namespace BrandonScott.RazerNotes.Lib
{
    public static class SharpBladeHelper
    {
        private static RazerManager _manager;
       
        public static RazerManager Manager
        {
            get { return _manager ?? (_manager = new RazerManager()); }
        }
        public static void ShutdownListener()
        {
            Manager.AppEvent += OnAppEvent;
        }

        static void OnAppEvent(object sender, Sharparam.SharpBlade.Razer.Events.AppEventEventArgs e)
        {
            if (e.Type == RazerAPI.AppEventType.Deactivated || e.Type == RazerAPI.AppEventType.Close || e.Type == RazerAPI.AppEventType.Exit)
            {
                RenderPoll.Stop();
                Application.Current.Shutdown();
            }
        }
    }
}
#endif
