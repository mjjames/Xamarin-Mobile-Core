using System.Threading.Tasks;

namespace MKS.Mobile.Core.Interfaces
{
    public interface IQueueItemProcessor
    {
        bool CanProcessItem(string itemType);
        Task<bool> ProcessItem(object item);
    }
}
