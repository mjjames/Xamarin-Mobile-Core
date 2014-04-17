using MKS.Mobile.Core.Interfaces;
using MonoTouch.CoreFoundation;
using MonoTouch.SystemConfiguration;
using System;
using System.Net;

// Class pinched from https://github.com/xamarin/monotouch-samples/blob/master/ReachabilitySample/reachability.cs
// however reworked to be non static as I want it as an interface so we can pass it into viewmodels etc

namespace MKS.Mobile.Core.iOS
{
    /// <summary>
    /// Reachability class for testing network connection
    /// </summary>
    public class Reachability : IReachability
    {
        public string HostName = "www.google.com";

        public bool IsReachableWithoutRequiringConnection(NetworkReachabilityFlags flags)
        {
            // Is it reachable with the current network configuration?
            var isReachable = (flags & NetworkReachabilityFlags.Reachable) != 0;

            // Do we need a connection to reach it?
            var noConnectionRequired = (flags & NetworkReachabilityFlags.ConnectionRequired) == 0;

            // Since the network stack will automatically try to get the WAN up,
            // probe that
            if ((flags & NetworkReachabilityFlags.IsWWAN) != 0)
                noConnectionRequired = true;

            return isReachable && noConnectionRequired;
        }

        // Is the host reachable with the current network configuration
        public bool IsHostReachable(string host)
        {
            if (string.IsNullOrEmpty(host))
                return false;

            using (var r = new NetworkReachability(host))
            {
                NetworkReachabilityFlags flags;

                if (r.TryGetFlags(out flags))
                {
                    return IsReachableWithoutRequiringConnection(flags);
                }
            }
            return false;
        }

        // 
        // Raised every time there is an interesting reachable event, 
        // we do not even pass the info as to what changed, and 
        // we lump all three status we probe into one
        //
        public event EventHandler ReachabilityChanged;

        void OnChange(NetworkReachabilityFlags flags)
        {
            var h = ReachabilityChanged;
            if (h != null)
                h(null, EventArgs.Empty);
        }

        //
        // Returns true if it is possible to reach the AdHoc WiFi network
        // and optionally provides extra network reachability flags as the
        // out parameter
        //
        static NetworkReachability _adHocWiFiNetworkReachability;
        public bool IsAdHocWiFiNetworkAvailable(out NetworkReachabilityFlags flags)
        {
            if (_adHocWiFiNetworkReachability == null)
            {
                _adHocWiFiNetworkReachability = new NetworkReachability(new IPAddress(new byte[] { 169, 254, 0, 0 }));
                _adHocWiFiNetworkReachability.SetCallback(OnChange);
                _adHocWiFiNetworkReachability.Schedule(CFRunLoop.Current, CFRunLoop.ModeDefault);
            }

            if (!_adHocWiFiNetworkReachability.TryGetFlags(out flags))
                return false;

            return IsReachableWithoutRequiringConnection(flags);
        }

        NetworkReachability _defaultRouteReachability;
        bool IsNetworkAvailable(out NetworkReachabilityFlags flags)
        {
            if (_defaultRouteReachability == null)
            {
                _defaultRouteReachability = new NetworkReachability(new IPAddress(0));
                _defaultRouteReachability.SetCallback(OnChange);
                _defaultRouteReachability.Schedule(CFRunLoop.Current, CFRunLoop.ModeDefault);
            }
            if (!_defaultRouteReachability.TryGetFlags(out flags))
                return false;
            return IsReachableWithoutRequiringConnection(flags);
        }

        NetworkReachability _remoteHostReachability;
        public NetworkStatus RemoteHostStatus()
        {
            NetworkReachabilityFlags flags;
            bool reachable;

            if (_remoteHostReachability == null)
            {
                _remoteHostReachability = new NetworkReachability(HostName);

                // Need to probe before we queue, or we wont get any meaningful values
                // this only happens when you create NetworkReachability from a hostname
                reachable = _remoteHostReachability.TryGetFlags(out flags);

                _remoteHostReachability.SetCallback(OnChange);
                _remoteHostReachability.Schedule(CFRunLoop.Current, CFRunLoop.ModeDefault);
            }
            else
                reachable = _remoteHostReachability.TryGetFlags(out flags);

            if (!reachable)
                return NetworkStatus.NotReachable;

            if (!IsReachableWithoutRequiringConnection(flags))
                return NetworkStatus.NotReachable;

            if ((flags & NetworkReachabilityFlags.IsWWAN) != 0)
                return NetworkStatus.ReachableViaCarrierDataNetwork;

            return NetworkStatus.ReachableViaWiFiNetwork;
        }

        public NetworkStatus InternetConnectionStatus()
        {
            NetworkReachabilityFlags flags;
            bool defaultNetworkAvailable = IsNetworkAvailable(out flags);
            if (defaultNetworkAvailable)
            {
                if ((flags & NetworkReachabilityFlags.IsDirect) != 0)
                    return NetworkStatus.NotReachable;
            }
            else if ((flags & NetworkReachabilityFlags.IsWWAN) != 0)
                return NetworkStatus.ReachableViaCarrierDataNetwork;
            else if (flags == 0)
                return NetworkStatus.NotReachable;
            return NetworkStatus.ReachableViaWiFiNetwork;
        }

        public NetworkStatus LocalWifiConnectionStatus()
        {
            NetworkReachabilityFlags flags;
            if (IsAdHocWiFiNetworkAvailable(out flags))
            {
                if ((flags & NetworkReachabilityFlags.IsDirect) != 0)
                    return NetworkStatus.ReachableViaWiFiNetwork;
            }
            return NetworkStatus.NotReachable;
        }
    }
}
