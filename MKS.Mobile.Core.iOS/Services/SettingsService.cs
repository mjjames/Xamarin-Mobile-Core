using MKS.Mobile.Core.Interfaces;
using MKS.Mobile.Core.Models;
using MonoTouch.Foundation;
using System;
using System.Collections.Generic;

namespace MKS.Mobile.Core.iOS.Services
{
    public abstract class SettingsService<T> : ISettingsService<T> where T : AppSettings
    {
        private T _settings;
        private List<IObserver<T>> _observers;

        public SettingsService()
        {
            _observers = new List<IObserver<T>>();
            SubscribeToSettings();
        }

        public T Settings
        {
            get {
                if (_settings == null)
                {
                    _settings = LoadSettings();
                    _observers.ForEach(o => o.OnNext(_settings));
                }
                return _settings;
            }
        }

        protected abstract T LoadSettings();
        protected abstract void PersistSettings();

        public void Persist()
        {
            UnsubscribeFromSettings();
            PersistSettings();
            SubscribeToSettings();
        }

        private string ObservableKey = "NSUserDefaultsDidChangeNotification";
        private NSObject _observerObject;

        private void SubscribeToSettings()
        {
            _observerObject = NSNotificationCenter.DefaultCenter.AddObserver(ObservableKey, (notification) =>
            {
                _settings = LoadSettings();
                _observers.ForEach(o => o.OnNext(_settings));
            });
        }

        private void UnsubscribeFromSettings()
        {
            NSNotificationCenter.DefaultCenter.RemoveObserver(_observerObject);
        }

        public IDisposable Subscribe(IObserver<T> observer)
        {
            if (!_observers.Contains(observer))
                _observers.Add(observer);
            return new Unsubscriber(_observers, observer);
        }

        private class Unsubscriber : IDisposable
        {
            private List<IObserver<T>> _observers;
            private IObserver<T> _observer;

            public Unsubscriber(List<IObserver<T>> observers, IObserver<T> observer)
            {
                _observers = observers;
                _observer = observer;
            }

            public void Dispose()
            {
                if (_observer != null && _observers.Contains(_observer))
                    _observers.Remove(_observer);
            }
        }
    }
}