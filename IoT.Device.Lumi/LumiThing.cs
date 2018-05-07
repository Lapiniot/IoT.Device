using System;
using System.ComponentModel;
using System.Json;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using IoT.Device.Lumi.Interfaces;

namespace IoT.Device.Lumi
{
    public abstract class LumiThing : ConnectedObject, INotifyPropertyChanged, IProvideOnlineInfo
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

        protected override void OnConnect()
        {
            // Empty by design
        }

        protected override void OnClose()
        {
            // Empty by design
        }

        #region IDisposable Support

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if(disposing)
            {
                cancellationTokenSource?.Cancel();
                cancellationTokenSource?.Dispose();
                cancellationTokenSource = null;
            }
        }

        #endregion

        #region INotifyPropertyChanged Support

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        #endregion
    }
}