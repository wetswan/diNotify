using System;

namespace diNotify
{
    internal class ToastMessageEventArgs : EventArgs
    {
        internal readonly ToastMessage Message;

        internal ToastMessageEventArgs(ToastMessage message) : base()
        {
            Message = message;
        }
    }
}
