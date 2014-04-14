using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace MKS.Mobile.Core.iOS
{
    public class iOSAutoResetTimer : iOSTimer
    {
        public iOSAutoResetTimer()
            : base()
        {
            _timer.AutoReset = true;
        }
    }
}