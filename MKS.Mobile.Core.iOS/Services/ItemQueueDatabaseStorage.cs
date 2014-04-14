using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using SQLite;
using MKS.Mobile.Core.Interfaces;
using System.Threading.Tasks;
using MKS.Mobile.Core.iOS.Models;

namespace MKS.Mobile.Core.iOS.Services
{

    public class DatabaseStorageService : IItemQueueStorage
    {
        private SQLiteAsyncConnection _connection;
        public DatabaseStorageService(string databasePath)
        {
            _connection = new SQLiteAsyncConnection(databasePath);
            _connection.CreateTableAsync<QueueItem>();
        }

        public async Task<IQueueItem> Add(object item)
        {
            var queueItem = new QueueItem(item);
            await _connection.InsertAsync(queueItem);
            return queueItem;
        }

        public async Task<IEnumerable<IQueueItem>> AddRange(IEnumerable<object> items)
        {
            var queueItems = items.Select(i => new QueueItem(i));
            await _connection.InsertAllAsync(queueItems);
            return queueItems;
        }

        public async Task<IList<IQueueItem>> EntireQueue()
        {
            var items = await _connection.Table<QueueItem>()
                                        .ToListAsync();
            return items.Cast<IQueueItem>().ToList();
        }

        public async Task Remove(IQueueItem item)
        {
            await _connection.DeleteAsync(item);
        }
    }
}