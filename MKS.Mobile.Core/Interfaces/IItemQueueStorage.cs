using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MKS.Mobile.Core.Interfaces
{
    public interface IItemQueueStorage
    {
        Task<IQueueItem> Add(object item);
        Task<IEnumerable<IQueueItem>> AddRange(IEnumerable<object> items);
        Task Remove(IQueueItem item);
        Task<IList<IQueueItem>> EntireQueue();
    }
}
