

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