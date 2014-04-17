using MKS.Mobile.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MKS.Mobile.Core.Interfaces
{
    public interface ILocationService
    {
        /// <summary>
        /// Whether the location device is enabled
        /// </summary>
        bool IsEnabled { get; }
        /// <summary>
        /// Start Location Updates
        /// </summary>
        /// <returns>Whether location updates were started</returns>
        Task<bool> StartLocationUpdates();
        /// <summary>
        /// Stop Location Updates
        /// </summary>
        /// <returns>Whether location updates were stopped</returns>
        Task<bool> StopLocationUpdates();

        /// <summary>
        /// Last known location of the device
        /// </summary>
        Location LastKnownLocation{get;}
        
        /// <summary>
        /// Fires when the location has been updated and exceeds our threshold
        /// </summary>
        event EventHandler<IEnumerable<Location>> LocationUpdated;

        /// <summary>
        /// The desired location accuracy in meters
        /// </summary>
        double DesiredAccuracy { get; set; }

        /// <summary>
        /// The minimum distance the device has travveled before a location update is raised
        /// </summary>
        double Threshold { get; set; }

        /// <summary>
        /// The lat lon for "Home" locations which indicates location updates should be paused until we leave the location
        /// </summary>
        IEnumerable<LatLon> NonTrackedLocations { get;}
    }
}
