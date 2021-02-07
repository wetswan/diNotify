using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Notifications;
using Windows.UI.Notifications.Management;

namespace diNotify
{
    internal class ToastListener
    {
        internal static ToastListener Current;

        internal event EventHandler<ToastMessageEventArgs> ToastReceived;

        private ToastListener()
        {
            UserNotificationListener.Current.NotificationChanged += Listener_NotificationChanged;
        }

        public static async Task<bool> CreateInstanceAsync()
        {
            UserNotificationListener listener = UserNotificationListener.Current;                
            
            // And request access to the user's notifications (must be called from UI thread)
            UserNotificationListenerAccessStatus accessStatus = await listener.RequestAccessAsync();

            if (accessStatus == UserNotificationListenerAccessStatus.Allowed)
            {
                Current = new ToastListener();
                return true;
            }
            return false;
        }

        internal virtual void OnToastReceived(ToastMessageEventArgs args)
        {
            ToastReceived?.Invoke(this, args);
        }

        private void Listener_NotificationChanged(UserNotificationListener sender, UserNotificationChangedEventArgs args)
        {
            ToastMessage message = new ToastMessage(args.UserNotificationId, (ToastChangeType)args.ChangeKind);

            if(args.ChangeKind == UserNotificationChangedKind.Added)
            {
                var toast = UserNotificationListener.Current.GetNotification(args.UserNotificationId);

                NotificationBinding toastBinding = toast.Notification.Visual.GetBinding(KnownNotificationBindings.ToastGeneric);

                if (toastBinding == null)
                {
                    return;
                }

                IReadOnlyList<AdaptiveNotificationText> textElements = toastBinding.GetTextElements();

                message.SenderName = toast.AppInfo.DisplayInfo.DisplayName;
                message.ExpiresAt = toast.Notification.ExpirationTime;
                message.Content = textElements.Select(t => t.Text);
            }

            ToastMessageEventArgs toastArgs = new ToastMessageEventArgs(message);

            OnToastReceived(toastArgs);
        }

        private async Task<bool> HasAnyNotifcationsAsync()
        {
            IReadOnlyList<UserNotification> toasts = await UserNotificationListener.Current.GetNotificationsAsync(NotificationKinds.Toast);
            return toasts.Any();
        }

        internal bool HasAnyNotifcation()
        {
            var task = Task.Run(async () => await HasAnyNotifcationsAsync());
            return task.Result;

        }
    }
}
