# NotificationsLib
Notifications Lib is a beautifully crafted library that seamlessly creates desktop notifications that are stunning to look at and have adequate functionality.

# Screenshot
![Screenshot](https://github.com/Absence209/NotificationsLib/blob/main/Screenshot%202020-10-29%20142304.png?raw=true)

# Getting Started
Download the latest release of the `NotificationsLib.dll` file from the releases page.

Add it to your project.

Add the namespace to the C# header.
```cs
using Cookie;
```
Next declare  a static Notification Host to use throughout your application.
```cs
using Cookie;
namespace YourApp
{
  class Program
  {
    public static NotificationHost notificationHost = new NotificationHost();
  }
}
```
Now whenever needed (like in a button click event) you can invoke the notification queue system to either immediatly show or until another notification gets dismissed.
```cs
private void ButtonClick(RoutedEventArgs a)
{
  notificationHost.ShowNotification("Text", "Title",  new ImageBrush(image), (notification) =>
    {
      // You can either wait for the notification to dismiss after 'x' seconds or dismiss immediatly when the notification is clicked.
      notificationHost.DismissNotification(notification);
      // Handle Notification Clicks Here
      Debug.WriteLine("Notification was clicked.");
    });
}
```
