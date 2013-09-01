#if RAZER

using System;
using System.Windows.Threading;
using System.Windows;

namespace BrandonScott.RazerNotes.Lib
{
    public static class RenderPoll
    {
        private static readonly DispatcherTimer Poll;

        public static Window RenderWindow { private get; set; }

        /// <summary>
        /// Sets up a poll dispatcher to render the form at 1ms
        /// </summary>
        static RenderPoll()
        {
            Poll = new DispatcherTimer();
            Poll.Tick += PollTick;
            Poll.Interval = new TimeSpan(0, 0, 0, 0, 1);
        }

        /// <summary>
        /// Starts the polling
        /// </summary>
        public static void Start()
        {
            Poll.Start();
        }

        /// <summary>
        /// Stops the polling
        /// </summary>
        public static void Stop()
        {
            Poll.Stop();
        }

        private static void PollTick(object sender, EventArgs e)
        {
            SharpBladeHelper.Manager.Touchpad.DrawWindow(RenderWindow);
        }
    }
}

#endif