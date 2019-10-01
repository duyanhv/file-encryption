using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace RivestCipher.View
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : MetroWindow
    {
        public LoginView()
        {
            InitializeComponent();

            buttonMoveToRegister.Click += ButtonMoveToRegister_Click;
            buttonBackToLogin.Click += ButtonBackToLogin_Click;
        }

        private void ButtonBackToLogin_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke((Action)(() => tabControlLogin.SelectedIndex = 0));
            buttonBackToLogin.Visibility = Visibility.Hidden;
        }

        private void ButtonMoveToRegister_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke((Action)(() => tabControlLogin.SelectedIndex = 1));
            buttonBackToLogin.Visibility = Visibility.Visible;
        }
    }
}
