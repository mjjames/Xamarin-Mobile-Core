using Newtonsoft.Json;
using System;

namespace MKS.Mobile.Core.Models
{
    public struct Location
    {
        [JsonConstructor]
        public Location(LatLon latlon, double altitude, DateTime timestampUtc):this()
        {
            LatLon = latlon;
            Altitude = altitude;
            TimeStampUtc = timestampUtc;
        }

        public LatLon LatLon {get;private set;}
        public double Altitude { get; private set; }
        public DateTime TimeStampUtc { get; private set; }

        public override bool Equals(object obj)
        {
            // If both are null, or both are same instance, return true.
            if (System.Object.ReferenceEquals(this, obj))
            {
                return true;
            }

            // If one is null, but not both, return false.
            if (((object)this == null) || (obj == null))
            {
                return false;
            }

            if (obj.GetType() != this.GetType())
            {
                return false;
            }
            var ll2 = (Location)obj;
            return ll2.Altitude == Altitude && ll2.LatLon == LatLon && ll2.TimeStampUtc == TimeStampUtc;
        }

        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hash = 17;
                hash = hash * 29 + Altitude.GetHashCode();
                hash = hash * 29 + LatLon.GetHashCode();
                hash = hash * 29 + TimeStampUtc.GetHashCode();
                return hash;
            }
        }

        public static bool operator ==(Location c1, Location c2)
        {
            return c1.Equals(c2);
        }

        public static bool operator !=(Location c1, Location c2)
        {
            return !c1.Equals(c2);
        }
    }
}
