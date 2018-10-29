using System;
using System.ComponentModel;
using System.Json;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using IoT.Device.Lumi.Interfaces;
using static System.Threading.Tasks.TaskContinuationOptions;

namespace IoT.Device.Lumi
{
    public abstract class LumiThing : ConnectedObject, INotifyPropertyChanged, IProvideOnlineInfo
    {
        private bool isOnline;
        private CancellationTokenSource resetWatchTokenSource;

        protected LumiThing(string sid)
        {
            (Sid, isOnline) = (sid, true);
        }

        public abstract string Model { get; }

        protected abstract TimeSpan HeartbeatTimeout { get; }

        public string Sid { get; }

        public bool IsOnline
        {
            get => isOnline;
            private set => Set(ref isOnline, value);
        }

        protected internal virtual void OnHeartbeat(JsonObject state)
        {
            ResetOnlineWatch();

            OnStateChanged(state);
        }

        protected internal virtual void OnStateChanged(JsonObject state)
        {
            IsOnline = true;
        }

        private void ResetOnlineWatch()
        {
            var newCts = new CancellationTokenSource();

            Task.Delay(HeartbeatTimeout, newCts.Token)
                .ContinueWith(t => IsOnline = false, OnlyOnRanToCompletion);

            using(var oldTcs = Interlocked.Exchange(ref resetWatchTokenSource, newCts))
            {
                oldTcs?.Cancel();
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
                resetWatchTokenSource?.Cancel();
                resetWatchTokenSource?.Dispose();
                resetWatchTokenSource = null;
            }
        }

        #endregion

        #region INotifyPropertyChanged Support

        public event PropertyChangedEventHandler PropertyChanged;


        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void Set<T>(ref T field, T value, [CallerMemberName] string name = null)
        {
            if(!Equals(field, value))
            {
                field = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            }
        }

        #endregion
    }
}