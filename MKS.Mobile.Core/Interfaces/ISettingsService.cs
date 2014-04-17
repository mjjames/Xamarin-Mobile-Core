using MKS.Mobile.Core.Models;
using System;

namespace MKS.Mobile.Core.Interfaces
{
    public interface ISettingsService<T> : IObservable<T> where T : AppSettings
    {
        T Settings{get;}
        void Persist();
    }
}
