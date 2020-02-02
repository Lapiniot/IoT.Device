using System;
using System.Collections.Generic;
using IoT.Protocol.Upnp;

namespace IoT.Device.Upnp
{
    public class UpnpReplyComparer : IEqualityComparer<SsdpReply>
    {
        private const string Location = "LOCATION";
        private const string Usn = "USN";
        private static readonly StringComparer Comparer = StringComparer.Ordinal;

        #region Implementation of IEqualityComparer<in SsdpReply>

        public bool Equals(SsdpReply x, SsdpReply y)
        {
            return x != null && y != null &&
                   x.TryGetValue(Location, out var lx) && y.TryGetValue(Location, out var ly) &&
                   x.TryGetValue(Usn, out var ux) && y.TryGetValue(Usn, out var uy) &&
                   Comparer.Equals(lx, ly) &&
                   Comparer.Equals(ux, uy);
        }

        public int GetHashCode(SsdpReply obj)
        {
            obj.TryGetValue(Location, out var l);
            obj.TryGetValue(Usn, out var u);

            return (l != null ? Comparer.GetHashCode(l) : 0) ^
                   (u != null ? Comparer.GetHashCode(u) : 0);
        }

        #endregion
    }
}