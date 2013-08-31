#if RAZER

using System;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Windows;
using Sharparam.SharpBlade.Razer;

namespace BrandonScott.RazerNotes.Lib
{
    public static class RenderPoll
    {
        private static DispatcherTimer _poll;
        private static Window _renderWindow;
        public static Window RenderWindow
        {
            get { return _renderWindow; }
            set { _renderWindow = value; }
        }

        /// <summary>
        /// Sets up a poll dispatcher to render the form at 1ms
        /// </summary>
        /// <param name="windowToDraw">The window to focus rendering on</param>
        static RenderPoll()
        {
            _poll = new DispatcherTimer();
            _poll.Tick += Poll_Tick;
            _poll.Interval = new TimeSpan(0, 0, 0, 0, 1);
        }

        /// <summary>
        /// Starts the polling
        /// </summary>
        public static void Start()
        {
            _poll.Start();
        }

        /// <summary>
        /// Stops the polling
        /// </summary>
        public static void Stop()
        {
            _poll.Stop();
        }

        private static void Poll_Tick(object sender, EventArgs e)
        {
            SharpBladeHelper.Manager.Touchpad.DrawWindow(_renderWindow);
        }
    }
}

#endif