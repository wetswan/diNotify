using System;
using System.Collections.Generic;

namespace diNotify
{
    internal class ToastMessage
    {
        internal uint Id;
        internal string SenderName;
        internal IEnumerable<string> Content;
        internal ToastChangeType ChangeType;
        internal DateTimeOffset? ExpiresAt;

        internal ToastMessage(uint id, ToastChangeType type)
        {
            Id = id;
            ChangeType = type;
        }
    }

    internal enum ToastChangeType
    {
        Added,
        Removed
    }
}
