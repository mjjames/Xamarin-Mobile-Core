using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
