using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MKS.Mobile.Core.Interfaces;
using Newtonsoft.Json;
using SQLite;

namespace MKS.Mobile.Core.iOS.Models
{
    public class QueueItem : IQueueItem
    {
        internal QueueItem(object item)
        {
            Id = Guid.NewGuid();
            TimeStampUtc = DateTime.UtcNow;
            SerialisedItemType = item.GetType().AssemblyQualifiedName;
            SerialisedItem = JsonConvert.SerializeObject(item);
        }
        public QueueItem() { }

        [PrimaryKey]
        public Guid Id
        {
            get;
            protected internal set;
        }

        public string SerialisedItem
        {
            get;
            protected set;
        }

        public string SerialisedItemType
        {
            get;
            protected set;
        }

        [Indexed]
        public DateTime TimeStampUtc
        {
            get;
            protected set;
        }

        [Ignore]
        public object Item
        {
            get
            {
                return JsonConvert.DeserializeObject(SerialisedItem, Type.GetType(SerialisedItemType, true));
            }
        }
    }
}