using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MKS.Mobile.Core.Interfaces
{
    public interface IQueueItemProcessor
    {
        bool CanProcessItem(string itemType);
        Task<bool> ProcessItem(object item);
    }
}
