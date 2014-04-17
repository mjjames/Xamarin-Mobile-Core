using System;

namespace MKS.Mobile.Core.Interfaces
{
    public interface IQueueItem
    {
        Guid Id { get; }
        DateTime TimeStampUtc { get; }
        string SerialisedItem { get; }
        string SerialisedItemType { get; }
        object Item {get;}
    }
}
