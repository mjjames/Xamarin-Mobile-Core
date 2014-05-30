using MKS.Mobile.Core.Interfaces;
using MKS.Mobile.Core.iOS.Models;
using SQLite;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MKS.Mobile.Core.iOS.Services
{

    public class DatabaseStorageService : IItemQueueStorage
    {
        private SQLiteAsyncConnection _connection;
        private Task _tableCreationTask;
        public DatabaseStorageService(string databasePath)
        {
            _connection = new SQLiteAsyncConnection(databasePath);
            _tableCreationTask = _connection.CreateTableAsync<QueueItem>();
        }

        public async Task<IQueueItem> Add(object item)
        {
            await _tableCreationTask;
            var queueItem = new QueueItem(item);
            await _connection.InsertAsync(queueItem);
            return queueItem;
        }

        public async Task<IEnumerable<IQueueItem>> AddRange(IEnumerable<object> items)
        {
            await _tableCreationTask;
            var queueItems = items.Select(i => new QueueItem(i));
            await _connection.InsertAllAsync(queueItems);
            return queueItems;
        }

        public async Task<IList<IQueueItem>> EntireQueue()
        {
            await _tableCreationTask;
            var items = await _connection.Table<QueueItem>()
                                        .ToListAsync();
            return items.Cast<IQueueItem>().ToList();
        }

        public async Task Remove(IQueueItem item)
        {
            await _tableCreationTask;
            await _connection.DeleteAsync(item);
        }


        public async Task Reset()
        {
            await _tableCreationTask;
            if (await _connection.Table<QueueItem>().CountAsync() > 0)
            {
                await _connection.DropTableAsync<QueueItem>();
                await _connection.CreateTableAsync<QueueItem>();
            }
        }
    }
}