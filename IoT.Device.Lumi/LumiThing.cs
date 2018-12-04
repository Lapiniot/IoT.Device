using System;
using System.ComponentModel;
using System.Json;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using IoT.Device.Lumi.Interfaces;
using static System.Runtime.CompilerServices.MethodImplOptions;
using static System.Threading.Tasks.TaskContinuationOptions;

namespace IoT.Device.Lumi
{
    public abstract class LumiThing : INotifyPropertyChanged, IProvideOnlineInfo, IDisposable
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

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public bool IsOnline
        {
            get => isOnline;
            private set => Set(ref isOnline, value);
        }

        protected internal virtual void OnHeartbeat(JsonObject state)
        {
            IsOnline = true;

            ResetOnlineWatch();

            OnStateChanged(state);
        }

        protected internal abstract void OnStateChanged(JsonObject state);

        private void ResetOnlineWatch()
        {
            var newCts = new CancellationTokenSource();

            Task.Delay(HeartbeatTimeout, newCts.Token)
                .ContinueWith(t => IsOnline = false, OnlyOnRanToCompletion);

            using(var oldCts = Interlocked.Exchange(ref resetWatchTokenSource, newCts))
            {
                oldCts?.Cancel();
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if(disposing)
            {
                var source = resetWatchTokenSource;
                if(source != null)
                {
                    source.Cancel();
                    source.Dispose();
                }

                resetWatchTokenSource = null;
            }
        }

        #region INotifyPropertyChanged Support

        public event PropertyChangedEventHandler PropertyChanged;

        [MethodImpl(AggressiveInlining)]
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        [MethodImpl(AggressiveInlining)]
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