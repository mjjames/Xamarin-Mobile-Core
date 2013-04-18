using System;

namespace MKS.Mobile.Core.Interfaces
{
    public interface IReachability
    {
        bool IsHostReachable(string host);
        NetworkStatus RemoteHostStatus();
        NetworkStatus InternetConnectionStatus();
        NetworkStatus LocalWifiConnectionStatus();
        event EventHandler ReachabilityChanged;
    }
}
