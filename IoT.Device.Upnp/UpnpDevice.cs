namespace IoT.Device.Upnp;

public class UpnpDevice
{
    public UpnpDevice(Uri descriptionUri, string usn)
    {
        DescriptionUri = descriptionUri ?? throw new ArgumentNullException(nameof(descriptionUri));

        if(string.IsNullOrWhiteSpace(usn)) throw new ArgumentException("valid USN must be provided", nameof(usn));

        Usn = usn;
    }

    public string Usn { get; }

    public Uri DescriptionUri { get; }
}