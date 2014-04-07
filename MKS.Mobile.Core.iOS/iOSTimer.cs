using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MKS.Mobile.Core.Interfaces;
using System.Timers;

namespace MKS.Mobile.Core.iOS
{
    public class iOSTimer : ITimer
    {
        private readonly Timer _timer;

        public iOSTimer()
        {
            _timer = new Timer { AutoReset = false };
            _timer.Elapsed += TimerOnElapsed;
        }

        private void TimerOnElapsed(object sender, ElapsedEventArgs args)
        {
            if (Elapsed != null)
            {
                Elapsed(args.SignalTime);
            }
        }

        public void Reset()
        {
            _timer.Stop();
            _timer.Start();
        }

        public Action<DateTime> Elapsed { get; set; }

        public TimeSpan Interval
        {
            get { return TimeSpan.FromMilliseconds(_timer.Interval); }
            set { _timer.Interval = value.TotalMilliseconds; }
        }

        public bool IsEnabled
        {
            get { return _timer.Enabled; }
            set { _timer.Enabled = value; }
        }

        public void Dispose()
        {
            _timer.Elapsed -= TimerOnElapsed;
            _timer.Dispose();
        }
    }
}