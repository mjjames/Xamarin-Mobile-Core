using System.Threading.Tasks;

namespace MKS.Mobile.Core.Interfaces
{
    public interface IRemoteServiceItemQueue
    {
        Task Enqueue(object item);
        void StartQueueProcessing();
        void StopQueueProcessing();
    }
}
