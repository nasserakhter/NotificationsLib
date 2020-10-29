using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Cookie
{
    /// <summary>
    /// Interaction logic for NotificationHost.xaml
    /// </summary>
    public partial class NotificationHost : Window
    {
        private double OffsetX = 25;
        private double OffsetY = 60;

        private double OffscreenNotificationX = 450;
        public double AnimationDurationMs = 550;
        public double AliveTime = 5;
        public int MaxNotificationsAtOnce = 3;

        public List<Notification> ActiveNotifications = new List<Notification>();
        public List<Notification> QueuedNotifications = new List<Notification>();

        public NotificationHost()
        {
            InitializeComponent();
            Point window = GetBottomRightPosition(Width, Height);
            Left = window.X + OffsetX;
            Top = window.Y + OffsetY;
        }
        private Point GetBottomRightPosition(double windowWidth, double windowHeight)
        {
            System.Drawing.Rectangle resolution = Screen.PrimaryScreen.Bounds;
            var pLeft = resolution.Width - windowWidth;
            var pTop = resolution.Height - windowHeight;
            return new Point(pLeft, pTop);
        }

        public void ShowNotification(string message, string title, Brush icon, Action<Notification> callback)
        {
            // Create coresponding notification object
            Notification n = new Notification()
            {
                Title = title,
                Content = message,
                Icon = icon
            };
            n.MouseUp += (x, y) => callback(n);

            // Check if current number of notifications is max
            if (ActiveNotifications.Count >= MaxNotificationsAtOnce)
            {
                // Add to notification queue instead.
                QueuedNotifications.Add(n);
            }
            else
            {
                // Good to go, show the notifcaton on next winpaint
                _ShowNotification(n);
            }
        }

        private void _ShowNotification(Notification n)
        {
            DispatcherTimer dt = new DispatcherTimer(TimeSpan.FromSeconds(AliveTime), DispatcherPriority.Normal, (x, y) =>
            {
                DismissNotification(n);
            }, Dispatcher);
            ActiveNotifications.Add(n);
            NotificationPresenter.Children.Add(n);
            AnimateIn();
            //AnimateOut(n);
            n.RenderTransform = new TranslateTransform(OffscreenNotificationX, 0);
            DoubleAnimation d = new DoubleAnimation()
            {
                To = 0,
                Duration = new Duration(TimeSpan.FromMilliseconds(AnimationDurationMs)),
                EasingFunction = new QuinticEase()
            };
            n.RenderTransform.BeginAnimation(TranslateTransform.XProperty, d);
        }

        private void AnimateIn()
        {
            if (ActiveNotifications.Count > 0)
            {
                foreach (Notification nt in ActiveNotifications)
                {
                    nt.RenderTransform = new TranslateTransform(0, (nt.Height + 35));
                    DoubleAnimation da = new DoubleAnimation()
                    {
                        To = 0,
                        Duration = new Duration(TimeSpan.FromMilliseconds(AnimationDurationMs)),
                        EasingFunction = new QuinticEase()
                    };
                    nt.RenderTransform.BeginAnimation(TranslateTransform.YProperty, da);

                }
            }
        }

        public void DismissNotification(Notification n)
        {
            DoubleAnimation d = new DoubleAnimation()
            {
                To = OffscreenNotificationX,
                Duration = new Duration(TimeSpan.FromMilliseconds(AnimationDurationMs)),
                EasingFunction = new QuinticEase()
            };
            d.Completed += (x, y) =>
            {
                ActiveNotifications.Remove(n);
                NotificationPresenter.Children.Remove(n);
            };
            n.RenderTransform.BeginAnimation(TranslateTransform.XProperty, d);
        }

        private void NotificationPresenter_LayoutUpdated(object sender, EventArgs e)
        {
            // Something changed

            // Update Host Window
            if (ActiveNotifications.Count < 1) Hide();
            if (ActiveNotifications.Count > 0) Show();

            // Check queue for pending notifications
            if (QueuedNotifications.Count > 0 && ActiveNotifications.Count < MaxNotificationsAtOnce)
            {
                Notification n = QueuedNotifications[0];
                if (n != null)
                {
                    QueuedNotifications.Remove(n);
                    _ShowNotification(n);
                }
            }

            // Recalculate Margins
            foreach (Notification i in NotificationPresenter.Children)
            {
                int index = NotificationPresenter.Children.IndexOf(i);
                // Make topmost notification not have a top margin
                if (index != 0)
                    i.Margin = new Thickness(0, 35, 0, 0);
                else
                    i.Margin = new Thickness(0);
            }
        }
    }
}
