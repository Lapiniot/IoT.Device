using System.Runtime.Serialization;

namespace IoT.Device.Yeelight;

public sealed class YeelightException : Exception
{
    public YeelightException() { }

    public YeelightException(string message) : base(message) { }

    public YeelightException(int code, string message) : this(code, message, null) { }

    public YeelightException(int code, string message, Exception innerException) :
        base(message, innerException)
    {
        Code = code;
    }

    public YeelightException(string message, Exception innerException) : base(message, innerException) { }

    public YeelightException(SerializationInfo info, StreamingContext context) : base(info, context) { }

    public int Code { get; }
}