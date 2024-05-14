using IoT.Protocol.Upnp;

internal sealed class ReportSsdpReplyObserver : IObserver<SsdpReply>
{
    public void OnCompleted() { }
    public void OnError(Exception error) { }

    public void OnNext(SsdpReply value)
    {
        var uri = new Uri(value.Location);
        Console.WriteLine($"Address:  {uri.Host}");
        Console.WriteLine($"Model:    {value["model"]}");
        Console.WriteLine($"Supports: [{string.Join(", ", value["support"].Split(' ').Select(s => $"\"{s}\""))}]");
        Console.WriteLine();
    }
}