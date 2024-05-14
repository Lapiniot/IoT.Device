using System.Text.Json;

namespace Yeelight.Control;

internal sealed class ReportPropChangeObserver : IObserver<JsonElement>
{
    public void OnCompleted() { }
    public void OnError(Exception error) { }
    public void OnNext(JsonElement value) => Console.WriteLine(value);
}