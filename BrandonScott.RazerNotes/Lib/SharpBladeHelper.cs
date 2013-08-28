#if RAZER

using Sharparam.SharpBlade.Razer;

namespace BrandonScott.RazerNotes.Lib
{
    public static class SharpBladeHelper
    {
        private static RazerManager _manager;

        public static RazerManager Manager
        {
            get { return _manager ?? (_manager = new RazerManager()); }
        }
    }
}

#endif
