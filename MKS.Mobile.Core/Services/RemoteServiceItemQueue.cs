using MKS.Mobile.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MKS.Mobile.Core.Models
{
    public class RemoteServiceItemQueue : IRemoteServiceItemQueue
    {
        private List<IQueueItemProcessor> _processors;
        private IItemQueueStorage _queueStorage;
        private Queue<IQueueItem> _queue;
        private Task _startupTask;
        private bool _isLoaded;
        private ITimer _queueTimer;
        private IReachability _reachability;

        public RemoteServiceItemQueue(IItemQueueStorage queueStorage, IEnumerable<IQueueItemProcessor> queueProcessors, 
                                        ITimer processQueueTimer, IReachability reachability)
        {
            _queueStorage = queueStorage;
            _processors = new List<IQueueItemProcessor>(queueProcessors);
            _queue = new Queue<IQueueItem>();
            _startupTask = LoadQueue();
            
            _queueTimer = processQueueTimer;
            _queueTimer.Elapsed = async (timeStamp) => await ProcessQueue();
            _queueTimer.IsEnabled = false;

            _reachability = reachability;
            _reachability.ReachabilityChanged += (o, e) =>
            {
                _queueTimer.IsEnabled = _reachability.InternetConnectionStatus() != NetworkStatus.NotReachable;
            };
        }

        private Task LoadQueue()
        {
            return Task.Factory.StartNew(async () =>
            {
                var queueItems = await _queueStorage.EntireQueue();
                foreach (var item in queueItems)
                {
                    _queue.Enqueue(item);
                }
                _isLoaded = true;
            });
        }

        public async Task Enqueue(object item){
            var queueItem = await _queueStorage.Add(item);
            Action<Task> enqueue = (task) => _queue.Enqueue(queueItem);
            if (_isLoaded)
            {
                enqueue(null);
            }
            else
            {
                _startupTask.ContinueWith(enqueue);
            }
        }
        private async Task ProcessQueue()
        {
            var processingError = false;
            while (_queue.Any() && !processingError)
            {
                processingError = !await ProcessItem(_queue.Peek());
            }
            _queueTimer.Reset();
        }

        private async Task<bool> ProcessItem(IQueueItem item)
        {
            var processors = _processors.Where(p => p.CanProcessItem(item.SerialisedItemType)).ToList();
            if (!processors.Any())
            {
                throw new ArgumentException("No processor found for " + item.SerialisedItemType, "item");
            }
            var processTasks= processors.Select(p => p.ProcessItem(item.Item));
            var results = await Task.WhenAll<bool>(processTasks);
            if (results.All(r => r))
            {
                _queue.Dequeue();
                return true;
            }
            return false;
            //TODO: if an item constantly fails in a processor we might permenantly lock a queue
            //do we need a max retry value?
        }

        private async Task Dequeue(IQueueItem item)
        {
            await _queueStorage.Remove(item);
        }

        public void StartQueueProcessing()
        {
            _queueTimer.IsEnabled = true;
        }

        public void StopQueueProcessing()
        {
            _queueTimer.IsEnabled = false;
        }
    }
}
