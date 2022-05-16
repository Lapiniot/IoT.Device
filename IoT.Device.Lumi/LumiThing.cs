using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json;
using IoT.Device.Lumi.Interfaces;
using static System.Runtime.CompilerServices.MethodImplOptions;
using static System.Threading.Tasks.TaskContinuationOptions;

namespace IoT.Device.Lumi;

public abstract class LumiThing : INotifyPropertyChanged, IProvideOnlineInfo, IAsyncDisposable
{
    private bool isOnline;
    private CancellationTokenSource resetWatchTokenSource;

    protected LumiThing(string sid) => (Sid, isOnline) = (sid, true);

    public abstract string ModelName { get; }

    protected abstract TimeSpan HeartbeatTimeout { get; }

    public string Sid { get; }

    #region Implementation of IAsyncDisposable

    public virtual ValueTask DisposeAsync()
    {
        GC.SuppressFinalize(this);

        var source = resetWatchTokenSource;
        if (source != null)
        {
            source.Cancel();
            source.Dispose();
        }

        resetWatchTokenSource = null;

        return new ValueTask();
    }

    #endregion

    public bool IsOnline
    {
        get => isOnline;
        private set => Set(ref isOnline, value);
    }

    protected internal virtual void OnHeartbeat(JsonElement state)
    {
        IsOnline = true;

        ResetOnlineWatch();

        OnStateChanged(state);
    }

    protected internal abstract void OnStateChanged(JsonElement state);

    private void ResetOnlineWatch()
    {
        var newCts = new CancellationTokenSource();

        _ = Task.Delay(HeartbeatTimeout, newCts.Token).ContinueWith(t => IsOnline = t.IsCanceled,
            default, ExecuteSynchronously, TaskScheduler.Default);

        using var oldCts = Interlocked.Exchange(ref resetWatchTokenSource, newCts);

        oldCts?.Cancel();
    }

    #region INotifyPropertyChanged Support

    public event PropertyChangedEventHandler PropertyChanged;

    [MethodImpl(AggressiveInlining)]
    protected void OnPropertyChanged([CallerMemberName] string name = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

    [MethodImpl(AggressiveInlining)]
    protected void Set<T>(ref T field, T value, [CallerMemberName] string name = null)
    {
        if (Equals(field, value)) return;
        field = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    #endregion
}