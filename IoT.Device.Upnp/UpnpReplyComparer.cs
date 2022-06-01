namespace IoT.Device.Upnp;

#nullable enable

public class UpnpReplyComparer : IEqualityComparer<SsdpReply>
{
    private const string Location = "LOCATION";
    private const string Usn = "USN";
    private static readonly StringComparer Comparer = StringComparer.OrdinalIgnoreCase;

    #region Implementation of IEqualityComparer<in SsdpReply>

    public bool Equals(SsdpReply? x, SsdpReply? y) =>
        x is not null && y is not null &&
        x.TryGetValue(Location, out var lx) && y.TryGetValue(Location, out var ly) &&
        x.TryGetValue(Usn, out var ux) && y.TryGetValue(Usn, out var uy) &&
        Comparer.Equals(lx, ly) &&
        Comparer.Equals(ux, uy);

    public int GetHashCode(SsdpReply obj)
    {
        obj.TryGetValue(Location, out var location);
        obj.TryGetValue(Usn, out var usn);
        return HashCode.Combine(location, usn);
    }

    #endregion
}