using System;

namespace MKS.Mobile.Core
{
    public class HostConnectivityException : Exception
    {
        public HostConnectivityException(string hostName, NetworkStatus networkStatus) : base("Unable to connect to " + hostName + " - no network connectivity. Current Network Status:" +
                      networkStatus)
        {
            NetworkStatus = networkStatus;
        }

        public NetworkStatus NetworkStatus { get; private set; }
    }
}
