using System;
using System.ComponentModel;
using System.Json;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using IoT.Device.Lumi.Interfaces;

namespace IoT.Device.Lumi
{
    public abstract class LumiThing : INotifyPropertyChanged, IDisposable, IProvideOnlineInfo
    {
        private readonly object syncRoot;
        private volatile CancellationTokenSource cancellationTokenSource;
        private bool isOnline;

        protected LumiThing(string sid)
        {
            (Sid, IsOnline, syncRoot) = (sid, true, new object());
        }

        public abstract string ModelName { get; }

        protected abstract TimeSpan OfflineTimeout { get; }

        public string Sid { get; }

        public bool IsOnline
        {
            get { return isOnline; }
            protected set
            {
                if(isOnline != value)
                {
                    isOnline = value;
                    OnPropertyChanged();
                }
            }
        }

        protected internal virtual void UpdateState(JsonObject data)
        {
            StartOnlineWatch();
        }

        private void StartOnlineWatch()
        {
            lock(syncRoot)
            {
                IsOnline = true;

                using(var source = cancellationTokenSource)
                {
                    source?.Cancel();
                }

                cancellationTokenSource = new CancellationTokenSource();

                Task.Delay(OfflineTimeout, cancellationTokenSource.Token)
                    .ContinueWith(t => IsOnline = false, TaskContinuationOptions.OnlyOnRanToCompletion);
            }
        }

        #region INotifyPropertyChanged Support

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        #endregion

        #region IDisposable Support

        private bool disposed;

        protected virtual void Dispose(bool disposing)
        {
            if(!disposed)
            {
                if(disposing)
                {
                    cancellationTokenSource?.Cancel();
                    cancellationTokenSource?.Dispose();
                    cancellationTokenSource = null;
                }

                disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        #endregion
    }
}