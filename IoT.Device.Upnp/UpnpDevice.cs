namespace IoT.Device.Upnp;

public class UpnpDevice
{
    public UpnpDevice(Uri descriptionUri, string usn)
    {
        ArgumentNullException.ThrowIfNull(descriptionUri);
        if (string.IsNullOrWhiteSpace(usn))
        {
            throw new ArgumentException("valid USN must be provided", nameof(usn));
        }

        DescriptionUri = descriptionUri;
        Usn = usn;
    }

    public string Usn { get; }

    public Uri DescriptionUri { get; }
}