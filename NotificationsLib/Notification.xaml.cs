using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Cookie
{
    /// <summary>
    /// Interaction logic for Notification.xaml
    /// </summary>
    public partial class Notification : Border
    {
        [Category("Common")]
        public string Title
        {
            get => TitleControl.Text;
            set => TitleControl.Text = value;
        }

        [Category("Common")]
        public string Content
        {
            get => ContentControl.Text;
            set => ContentControl.Text = value;
        }

        [Category("Brush")]
        public Brush Icon
        {
            get => IconPresenter.Background;
            set => IconPresenter.Background = value;
        }

        public Notification()
        {
            InitializeComponent();
        }
    }
}
