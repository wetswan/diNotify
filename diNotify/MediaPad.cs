/*
 * Based on code found on winarkeo.net (offline)
 * (c) Val @ winarkeo.net 2018 https://grav.winarkeo.net/fr/articles/logitech-dinovo-mediapadlib-hack-library
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using HidLibrary;
using Microsoft.Win32;

namespace diNotify
{
    /// <summary>
    /// C# class to interface the Logitech diNovo Mediapad
    /// Special thanks to https://github.com/thentenaar/bluez-dinovo/blob/dinovo/input/logitech_mediapad.c for reference
    /// </summary>

    public class MediaPad
	{

		/* Display modes */
		const byte LCD_DISP_MODE_INIT = 0x01; /* Initialize the line */
		const byte LCD_DISP_MODE_BUF1 = 0x10; /* Display the first buffer on the line */
		const byte LCD_DISP_MODE_BUF2 = 0x11; /* ... 2nd buffer */
		const byte LCD_DISP_MODE_BUF3 = 0x12; /* ... 3rd buffer */
		const byte LCD_DISP_MODE_SCROLL = 0x20; /* Scroll by one buffer */
		const byte LCD_DISP_MODE_SCROLL2 = 0x02; /* ... by 2 buffers */
		const byte LCD_DISP_MODE_SCROLL3 = 0x03; /* ... by 3 buffers */

		readonly byte[] screen_start = { 0x10, 0x00, 0x81, 0x10, 0x00, 0x00, 0x00 };
		readonly byte[] screen_finish = { 0x10, 0x00, 0x83, 0x11, 0x00, 0x00, 0x00 };

		HidDevice[] sockets;

		public static MediaPad Current { get; private set; }

		private MediaPad(HidDevice[] sockets)
		{
			this.sockets = sockets;
		}

		public static bool CreateInstance()
        {
			var sockets = HidDevices.Enumerate(0x046D, 0xB3E3).Where(d => d.Capabilities.UsagePage == -256).ToArray();

			if (sockets.Length == 0)
			{
				return false;
			}

			Current = new MediaPad(sockets);
			return true;

		}

        public void DisplayText(string text)
        {
			if (sockets.Length == 0)
			{
				return;
			}

            sockets[0].Write(screen_start, 500);
            SetDisplayMode(LCD_DISP_MODE_INIT, LCD_DISP_MODE_INIT, LCD_DISP_MODE_INIT);
            ClockMode(false);

            string[] lines = (text + "\n\n").Split('\n').Take(3).ToArray();

			for (int i = 0; i < lines.Length; i++)
			{
				BufferText(lines[i], i * 3);
			}

            SetDisplayMode(LCD_DISP_MODE_BUF1, LCD_DISP_MODE_BUF1, LCD_DISP_MODE_BUF1);
            sockets[0].Write(screen_finish, 500);
        }

        //put the text into buffer
        void BufferText(string text, int bufno = 0)
        {
			if (bufno > 9)
			{
				return;
			}

            List<byte> payload = new List<byte>();
            payload.AddRange(new byte[] { 0x11, 0x00, 0x82 });
            payload.Add((byte)(0x20 + (byte)bufno));
            payload.AddRange(Encoding.Unicode.GetBytes(text).Where(b => b != 0).Take(16));

			for (int i = 0; i < 16 - text.Length; i++)
			{
				payload.Add(0x20);
			}

            sockets[1].Write(payload.ToArray());
        }

		//got mail?
		bool hasUnreadMail
		{
			get
			{
				int mailsCount = 0;
				RegistryKey key = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\UnreadMail");
				foreach (var v in key.GetSubKeyNames())
				{
					RegistryKey accountKey = key.OpenSubKey(v);
					if (accountKey.GetValue("MessageCount") != null) mailsCount += (int)accountKey.GetValue("MessageCount");
				}
				return mailsCount > 0;
			}
		}

		//sync date/time
		public void SetClock()
		{
			if (sockets.Length == 0)
			{
				return;
			}

			DateTime time = DateTime.Now;
			sockets[0].Write(new byte[] { 0x10, 0x00, 0x80, 0x31, (byte)time.Second, (byte)time.Minute, (byte)time.Hour });
			sockets[0].Write(new byte[] { 0x10, 0x00, 0x80, 0x32, 0x02, (byte)time.Day, (byte)(time.Month - 1) });
			sockets[0].Write(new byte[] { 0x10, 0x00, 0x80, 0x33, (byte)(time.Year % 100), 0x00, 0x00 }); //two digits year
		}
		//display mode
		public void SetDisplayMode(byte mode1, byte mode2, byte mode3)
		{
			if (sockets.Length == 0)
			{
				return;
			}

			sockets[0].Write(new byte[] { 0x10, 0x00, 0x80, 0x12, mode1, mode2, mode3 });
		}
		//icons
		//basic set icons
		public void SetIcons(bool mail = false, bool aim = false, bool mute = false, bool alert = false)
		{
			if (sockets.Length == 0)
			{
				return;
			}

			//set 0x01 from [4] to set icon, add 0x01 to the byte to make it blink
			sockets[1].Write(new byte[]{ 0x11, 0x00, 0x82, 0x11, (byte)(mail ? 0x02 : 0x00), Convert.ToByte(aim), Convert.ToByte(mute), Convert.ToByte(alert),
	  			0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 });
		}
		//text or clock mode
		public void ClockMode(bool enabled = true)
		{
			if (sockets.Length == 0)
			{
				return;
			}

			sockets[0].Write(new byte[] { 0x10, 0x00, 0x80, 0x10, 0x00, Convert.ToByte(enabled), 0x00 }, 500);
		}
		//led toggle
		public void Led(bool enabled = true)
		{
			if (sockets.Length == 0)
			{
				return;
			}

			sockets[0].Write(new byte[] { 0x10, 0x00, 0x80, 0x50, 0x00, Convert.ToByte(enabled), 0x00 }, 500);
		}
		//Beep
		public void Beep()
		{
			if (sockets.Length == 0)
			{
				return;
			}

			sockets[0].Write(new byte[] { 0x10, 0x00, 0x80, 0x50, 0x02, 0x00, 0x00 });
		}

		//reset screen
		public void Dispose()
		{
			if (sockets.Length == 0)
            {
                return;
            }

            SetIcons();
			DisplayText("");
			ClockMode();
		}
	}
}
//copy of original file

///*
// * (c) Val @ winarkeo.net 2018
// */
//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Linq;
//using HidLibrary;
//using Microsoft.Win32;

//namespace Devices
//{
//	/// <summary>
//	/// C# class to interface the Logitech diNovo Mediapad
//	/// Special thanks to https://github.com/thentenaar/bluez-dinovo/blob/dinovo/input/logitech_mediapad.c for reference
//	/// </summary>

//	public class MediaPad
//	{

//		/* Display modes */
//		const byte LCD_DISP_MODE_INIT = 0x01; /* Initialize the line */
//		const byte LCD_DISP_MODE_BUF1 = 0x10; /* Display the first buffer on the line */
//		const byte LCD_DISP_MODE_BUF2 = 0x11; /* ... 2nd buffer */
//		const byte LCD_DISP_MODE_BUF3 = 0x12; /* ... 3rd buffer */
//		const byte LCD_DISP_MODE_SCROLL = 0x20; /* Scroll by one buffer */
//		const byte LCD_DISP_MODE_SCROLL2 = 0x02; /* ... by 2 buffers */
//		const byte LCD_DISP_MODE_SCROLL3 = 0x03; /* ... by 3 buffers */

//		readonly byte[] screen_start = { 0x10, 0x00, 0x81, 0x10, 0x00, 0x00, 0x00 };
//		readonly byte[] screen_finish = { 0x10, 0x00, 0x83, 0x11, 0x00, 0x00, 0x00 };

//		HidDevice[] sockets;

//		//instanciate, look for the device
//		public MediaPad()
//		{
//			sockets = HidDevices.Enumerate(0x046D, 0xB3E1).Where(d => d.Capabilities.UsagePage == -256).ToArray();
//			/*foreach(HidDevice device in HidDevices.Enumerate(0x046D, 0xB3E1)){
//				System.Windows.Forms.MessageBox.Show(device.Description+"\n"+device.Capabilities.Usage+"");
//			}*/

//			if (sockets.Length == 0) return;

//			SetClock();
//		}

//		//put the text into buffer
//		void BufferText(string text, int bufno = 0)
//		{
//			if (bufno > 9) return;
//			List<byte> payload = new List<byte>();
//			payload.AddRange(new byte[] { 0x11, 0x00, 0x82 });
//			payload.Add((byte)(0x20 + (byte)bufno));
//			payload.AddRange(Encoding.Default.GetBytes(text).Take(16));
//			for (int i = 0; i < 16 - text.Length; i++) payload.Add(0x20);
//			sockets[1].Write(payload.ToArray());
//		}
//		//got mail?
//		bool hasUnreadMail
//		{
//			get
//			{
//				int mailsCount = 0;
//				RegistryKey key = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\UnreadMail");
//				foreach (var v in key.GetSubKeyNames())
//				{
//					RegistryKey accountKey = key.OpenSubKey(v);
//					if (accountKey.GetValue("MessageCount") != null) mailsCount += (int)accountKey.GetValue("MessageCount");
//				}
//				return mailsCount > 0;
//			}
//		}

//		//sync date/time
//		public void SetClock()
//		{
//			if (sockets.Length == 0) return;

//			DateTime time = DateTime.Now;
//			sockets[0].Write(new byte[] { 0x10, 0x00, 0x80, 0x31, (byte)time.Second, (byte)time.Minute, (byte)time.Hour });
//			sockets[0].Write(new byte[] { 0x10, 0x00, 0x80, 0x32, 0x02, (byte)time.Day, (byte)(time.Month - 1) });
//			sockets[0].Write(new byte[] { 0x10, 0x00, 0x80, 0x33, (byte)(time.Year % 100), 0x00, 0x00 }); //two digits year
//		}
//		//display mode
//		public void SetDisplayMode(byte mode1, byte mode2, byte mode3)
//		{
//			if (sockets.Length == 0) return;

//			sockets[0].Write(new byte[] { 0x10, 0x00, 0x80, 0x12, mode1, mode2, mode3 });
//		}
//		//icons
//		//basic set icons
//		public void SetIcons(bool mail = false, bool aim = false, bool mute = false, bool alert = false)
//		{
//			if (sockets.Length == 0) return;

//			//set 0x01 from [4] to set icon, add 0x01 to the byte to make it blink
//			sockets[1].Write(new byte[]{ 0x11, 0x00, 0x82, 0x11, (byte)(mail && hasUnreadMail ? 0x02 : 0x00), Convert.ToByte(aim), Convert.ToByte(mute), Convert.ToByte(alert),
//	  			0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 });
//		}
//		//text or clock mode
//		public void ClockMode(bool enabled = true)
//		{
//			if (sockets.Length == 0) return;

//			sockets[0].Write(new byte[] { 0x10, 0x00, 0x80, 0x10, 0x00, Convert.ToByte(enabled), 0x00 }, 500);
//		}
//		//led toggle
//		public void Led(bool enabled = true)
//		{
//			if (sockets.Length == 0) return;

//			sockets[0].Write(new byte[] { 0x10, 0x00, 0x80, 0x50, 0x00, Convert.ToByte(enabled), 0x00 }, 500);
//		}
//		//Beep
//		public void Beep()
//		{
//			if (sockets.Length == 0) return;

//			sockets[0].Write(new byte[] { 0x10, 0x00, 0x80, 0x50, 0x02, 0x00, 0x00 });
//		}
//		//looking for coyote
//		public void DisplayText(string text)
//		{
//			if (sockets.Length == 0) return;

//			sockets[0].Write(screen_start, 500);
//			SetDisplayMode(LCD_DISP_MODE_INIT, LCD_DISP_MODE_INIT, LCD_DISP_MODE_INIT);
//			ClockMode(false);
//			string[] lines = (text + "\n\n").Split('\n').Take(3).ToArray();
//			for (int i = 0; i < lines.Length; i++) BufferText(lines[i], i * 3);
//			SetDisplayMode(LCD_DISP_MODE_BUF1, LCD_DISP_MODE_BUF1, LCD_DISP_MODE_BUF1);
//			sockets[0].Write(screen_finish, 500);
//		}
//		//reset screen
//		public void Dispose()
//		{
//			if (sockets.Length == 0) return;

//			SetIcons();
//			DisplayText("");
//			ClockMode();
//		}
//	}
//}