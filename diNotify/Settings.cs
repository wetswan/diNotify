using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Windows.UI.Notifications;
using Microsoft.Toolkit.Uwp.Notifications;

namespace diNotify
{
    public partial class Settings : Form
    {
        public Settings()
        {
            InitializeComponent();
            bool success = MediaPad.CreateInstance();
            LogNotification($"Connection to MediaPad: {success}");
        }

        private async void button1_ClickAsync(object sender, EventArgs e)
        {
            if(await ToastListener.CreateInstanceAsync())
            {
                ToastListener.Current.ToastReceived += HandleToastMessage;
                LogNotification("Listener started ...");
                btnReset.Enabled = true;
                btnSendTestToast.Enabled = true;
                btnStartListener.Enabled = false;
            }
            else
            {
                LogNotification("Error: Listener could not be started started.");
            }
        }

        private void HandleToastMessage(object sender, ToastMessageEventArgs e)
        {
            if(e.Message.ChangeType == ToastChangeType.Removed)
            {
                MediaPad.Current.SetIcons(alert: ToastListener.Current.HasAnyNotifcation());
                return;
            }

            if(e.Message.ExpiresAt < DateTime.Now)
            {
                return;
            }

            List<string> toastText = e.Message.Content.ToList();

            //sometimes the first 2 elements are identical, maybe add an option for that
            if (toastText[0].Equals(toastText[1]))
            {
                toastText.RemoveAt(0);
            }

            string bodyText = string.Join("\n", toastText);
            if (cbShowAppName.Checked && !e.Message.SenderName.Equals("Ihr Smartphone")) //todo config
            {
                bodyText = e.Message.SenderName + "\n" + bodyText;
            }

            MediaPad.Current.SetIcons(false, false, false, true);
            MediaPad.Current.DisplayText(bodyText);
            if (cbBeep.Checked)
            {
                MediaPad.Current.Beep();
            }

            if (cbLed.Checked)
            {
                MediaPad.Current.Led();
            }

            Timer timer = new Timer
            {
                Interval = (int)seconds.Value * 1000
            };
            timer.Tick += Timer_Tick;
            timer.Start();
            LogNotification($"Notification received from {e.Message.SenderName}");
        }    

        private void LogNotification(string message)
        {
            MethodInvoker methodInvokerDelegate = delegate ()
            { richTextBox1.Text += $"{DateTime.Now} {message} \n"; };

            if (InvokeRequired)
            {
                Invoke(methodInvokerDelegate);
            }
            else
            {
                methodInvokerDelegate();
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            MediaPad.Current.ClockMode(true);
            MediaPad.Current.Led(false);
            (sender as Timer).Stop();
        }

        private void SendTestToastMessage(object sender, EventArgs e)
        {
            // Construct the content
            var content = new ToastContentBuilder()
                .AddText($"Der Inhalt der ")
                .AddText($"Benachrichtigung")
                .GetToastContent();

            // Create the notification
            var notif = new ToastNotification(content.GetXml());

            // And show it!
            ToastNotificationManager.CreateToastNotifier().Show(notif);

            LogNotification("Sent a toast");
        }

        private void ResetDisplay(object sender, EventArgs e)
        {
            MediaPad.Current.ClockMode(true);
            MediaPad.Current.SetIcons();
        }

        private void ResizeHandler(object sender, EventArgs e)
        {
            if(WindowState == FormWindowState.Minimized)
            {
                Hide();                
            }
        }

        private void trayIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Show();
            WindowState = FormWindowState.Normal;
        }
    }
}
