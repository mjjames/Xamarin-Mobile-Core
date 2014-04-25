using System;
using System.Threading.Tasks;

namespace MKS.Mobile.Core.Interfaces
{
    public interface IBatteryInfoService
    {
        Task<float> GetRemainingBatteryPercentage();
        Task<TimeSpan> GetEstimatedRemainingBatteryLife();
        bool IsGetRemainingBatteryPercentageSupported { get; }
        bool IsGetEstimatedRemainingBatteryLifeSupported { get; }
    }
}
