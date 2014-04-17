using Newtonsoft.Json;

namespace MKS.Mobile.Core.Models
{
    public struct LatLon
    {
        private double _longitude;
        private double _latitude;

        [JsonConstructorAttribute]
        public LatLon(double latitude, double longitude)
        {
            _latitude = latitude;
            _longitude = longitude;
        }

        public double Latitude { get { return _latitude; } }
        public double Longitude { get { return _longitude; } }

        public static LatLon Empty
        {
            get
            {
                return new LatLon(0,0);
            }
        }

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
            var ll2 = (LatLon)obj;
            return ll2.Latitude== Latitude && ll2.Longitude == Longitude;
        }

        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hash = 17;
                hash = hash * 29 + Latitude.GetHashCode();
                hash = hash * 29 + Longitude.GetHashCode();
                return hash;
            }
        }

        public static bool operator ==(LatLon c1, LatLon c2)
        {
            return c1.Equals(c2);
        }

        public static bool operator !=(LatLon c1, LatLon c2)
        {
            return !c1.Equals(c2);
        }
    }
}
