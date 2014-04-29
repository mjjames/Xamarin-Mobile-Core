using MK.ErrorTracking.DataObjects;
using MKS.ErrorTracking.ServiceClient;
using MKS.Mobile.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MKS.Mobile.Core.QueueProcessor
{
    public class ExceptionItemProcessor : VersionedProcessorBase, IQueueItemProcessor
    {
        private ServiceDetails _serviceDetails;
        private HttpMessageHandler _messageHandler;

        public ExceptionItemProcessor(ServiceDetails serviceDetails, HttpMessageHandler messageHandler = null)
            : base(typeof(ExceptionItem).AssemblyQualifiedName)
        {
            _serviceDetails = serviceDetails;
            _messageHandler = messageHandler;
        }

        public async Task<bool> ProcessItem(object Item)
        {
            var exceptionItem = Item as ExceptionItem;
            if (exceptionItem == null)
            {
                return false;
            }
            var result = await new ExceptionLogger(_serviceDetails, _messageHandler)
                            .LogException(exceptionItem);
            return result.IsSuccess;
        }
    }
}
