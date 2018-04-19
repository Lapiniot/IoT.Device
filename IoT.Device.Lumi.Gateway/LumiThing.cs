using System;
using System.ComponentModel;
using System.Json;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using static System.Threading.Tasks.TaskContinuationOptions;

namespace IoT.Device.Lumi.Gateway
{
    public abstract class LumiThing : INotifyPropertyChanged, IDisposable
    {
        private volatile CancellationTokenSource cancellationTokenSource;
        private bool isOnline;
        private readonly object syncRoot;

        protected LumiThing(string sid)
        {
            (Sid, IsOnline, syncRoot) = (sid, true, new object());
        }

        public abstract string ModelName { get; }

        protected abstract TimeSpan OfflineTimeout { get; }

        public string Sid { get; }

        public bool IsOnline
        {
            get => isOnline;
            protected set
            {
                if (isOnline != value)
                {
                    isOnline = value;
                    OnPropertyChanged();
                }
            }
        }

        protected internal abstract void UpdateState(JsonObject data);

        protected internal virtual void Heartbeat(JsonObject data)
        {
            lock (syncRoot)
            {
                IsOnline = true;

                var source = cancellationTokenSource;

                if (source != null)
                {
                    source.Cancel();
                    source.Dispose();
                }

                cancellationTokenSource = new CancellationTokenSource();

                Task.Delay(OfflineTimeout, cancellationTokenSource.Token)
                    .ContinueWith(t => { IsOnline = false; }, OnlyOnRanToCompletion);
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
            if (!disposed)
            {
                if (disposing)
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