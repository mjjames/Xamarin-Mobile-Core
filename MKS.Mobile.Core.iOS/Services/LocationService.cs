using MKS.Mobile.Core.Interfaces;
using MKS.Mobile.Core.Models;
using MonoTouch.CoreLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MKS.Mobile.Core.iOS.Services
{
    public class LocationService : ILocationService
    {
        private readonly CLLocationManager _locationManager;
        private double _desiredAccuracy;
        private double _threshold;

        public LocationService(IEnumerable<LatLon> nonTrackedLocations, double desiredAccuracy = 500, double threshold = 500)
            : this(desiredAccuracy, threshold)
        {
            NonTrackedLocations = nonTrackedLocations;
            RegisterNonTrackedLocations();
        }



        public LocationService(double desiredAccuracy = 500, double threshold = 500)
        {
            _locationManager = new CLLocationManager
            {
                DesiredAccuracy = desiredAccuracy,
                DistanceFilter = threshold,
                ActivityType = CLActivityType.AutomotiveNavigation
            };
            _threshold = threshold;
            _desiredAccuracy = desiredAccuracy;
            _locationManager.LocationsUpdated += locationManager_UpdatedLocation;
        }

        private void locationManager_UpdatedLocation(object sender, CLLocationsUpdatedEventArgs e)
        {
            if (LocationUpdated == null || !e.Locations.Any())
            {
                return;
            }
            LocationUpdated(this, e.Locations.Select(l => new Location(
                                                                        new LatLon(l.Coordinate.Latitude, l.Coordinate.Longitude),
                                                                        l.Altitude,
                                                                        l.Timestamp
                                                                    )
                                                    )
                            );
        }

        public bool IsEnabled
        {
            get { return CLLocationManager.LocationServicesEnabled; }
        }

        public Task<bool> StartLocationUpdates()
        {
            if (!IsEnabled)
            {
                return Task.FromResult(false);
            }
            return Task.FromResult(StartUpdates());
        }

        private bool StartUpdates()
        {
            try
            {
                _locationManager.StartUpdatingLocation();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public Task<bool> StopLocationUpdates()
        {
            if (!IsEnabled)
            {
                return Task.FromResult(false);
            }
            try
            {
                _locationManager.StopUpdatingLocation();
                return Task.FromResult(true);
            }
            catch
            {
                return Task.FromResult(false);
            }
        }

        public Location LastKnownLocation
        {
            get
            {
                var lastLocation = _locationManager.Location;
                return lastLocation == null ? new Location() : new Location(new LatLon(lastLocation.Coordinate.Latitude, lastLocation.Coordinate.Longitude), 
                                                                            lastLocation.Altitude, 
                                                                            lastLocation.Timestamp);
            }
        }

        public event EventHandler<IEnumerable<Location>> LocationUpdated;


        public double DesiredAccuracy
        {
            get
            {
                return _desiredAccuracy;
            }
            set
            {
                _locationManager.DesiredAccuracy = value;
                _desiredAccuracy = value;
            }
        }

        public double Threshold
        {
            get
            {
                return _threshold;
            }
            set
            {
                _locationManager.DistanceFilter = value;
                _threshold = value;
            }
        }

        public IEnumerable<LatLon> NonTrackedLocations
        {
            get;
            private set;
        }

        private void RegisterNonTrackedLocations()
        {
            foreach (var latlon in NonTrackedLocations)
            {
                _locationManager.StartMonitoring(new CLCircularRegion(new CLLocationCoordinate2D(latlon.Latitude, latlon.Longitude), 10, string.Format("{0}-{1}", latlon.Latitude, latlon.Longitude)));
            }
            _locationManager.RegionEntered += (o, e) =>
            {
                _locationManager.StopUpdatingLocation();
            };
            _locationManager.RegionLeft += (o, e) =>
            {
                _locationManager.StartUpdatingLocation();
            };
        }
    }
}