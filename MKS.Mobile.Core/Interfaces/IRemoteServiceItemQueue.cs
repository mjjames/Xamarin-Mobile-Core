using System.Threading.Tasks;

namespace MKS.Mobile.Core.Interfaces
{
    public interface IRemoteServiceItemQueue
    {
        Task Enqueue(object item);
        void StartQueueProcessing();
        void StopQueueProcessing();
        bool IsQueueProcessingEnabled { get; }
        int Count { get; }
        /// <summary>
        /// Removes all items from the queue WITHOUT processing them
        /// </summary>
        Task Reset();
    }
}
