using System;
using System.Threading.Tasks;
using MonoTouch.UIKit;
using MKS.Mobile.Core.Interfaces;

namespace MKS.Mobile.Core.iOS.Services
{
    public class iOSBatteryInfoService : IBatteryInfoService
    {
        private readonly UIDevice _device;

        public iOSBatteryInfoService()
        {
            _device = UIDevice.CurrentDevice;
            _device.BatteryMonitoringEnabled = true;
        }

        public Task<float> GetRemainingBatteryPercentage()
        {
            return Task.FromResult( _device.BatteryState == UIDeviceBatteryState.Full ? 100 : _device.BatteryLevel*100);
        }

        public Task<TimeSpan> GetEstimatedRemainingBatteryLife()
        {
            throw new NotImplementedException();
        }

        public bool IsGetRemainingBatteryPercentageSupported
        {
            get { return true; }
        }
        public bool IsGetEstimatedRemainingBatteryLifeSupported
        {
            get { return false; }
        }
    }
}
