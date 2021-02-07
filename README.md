# diNotify
A small application which hooks to the Windows 10 notifications and pushes the content to the display of the Logitech diNovo mediapad which is a part of a keyboard package. Back in the days (2004, 2005..) it used to support messengers like ICQ and email clients. Nowadays it does not support a lot of applications which is why I tried to give it some modern use.

## Disclaimer
This repository uses a code sample found on winarkeo.net before it went offline. The MediaPad.cs is based on that and the original file is included at the bottom.

## Requirements
In order to run the application you need:
* Windows 10 1903 or later
* Logitech diNovo Keyboard / MediaPad

## etting to run
* You need to set the Package project as start-up as it wraps the WinForms application. This was done as receiving events from the UserNotificationListener does not seem to work without a UWP context
* After the app has started, click "Start Listener" and send a test notification with "Send toast"
* If the connection to the bluetooth MediaPad fails, check if the address in MediaPad.cs line 45 matches your device (there were multiple versions of the device)
