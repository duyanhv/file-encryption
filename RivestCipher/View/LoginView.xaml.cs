using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using RivestCipher.Service;
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
        string _connectionString;
        UserService _userSerivce;
        public LoginView(string connectionString)
        {
            InitializeComponent();
            _connectionString = connectionString;
            buttonMoveToRegister.Click += ButtonMoveToRegister_Click;
            buttonBackToLogin.Click += ButtonBackToLogin_Click;
            buttonLogin.Click += ButtonLogin_Click;
            buttonRegister.Click += ButtonRegister_Click;
            _userSerivce = new UserService(_connectionString);
        }

        private async void ButtonRegister_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var hey = _userSerivce.Create(textBoxRegisterUserName.Text, passwordBoxRegisterPassword.Password);

                if (hey)
                {
                    var hey1 = _userSerivce.Login(textBoxRegisterUserName.Text, passwordBoxRegisterPassword.Password);
                }
            }catch(Exception ex)
            {
                await this.ShowMessageAsync("Error", ex.Message);
            }
        }

        private void ButtonLogin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var hey = _userSerivce.Login(textBoxUserName.Text, textBoxPassword.Password);
            }
            catch(Exception ex)
            {
                var error = ex;
            }
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
