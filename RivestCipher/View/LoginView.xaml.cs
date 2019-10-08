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
using static RivestCipher.Action.UserProfileAction;

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
            buttonLogin.Click += ButtonLogin_Click;
            buttonRegister.Click += ButtonRegister_Click;
        }

        private async void ButtonRegister_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var userParams = new Model.LoginUserParams
                {
                    UserName = textBoxRegisterUserName.Text,
                    Password = passwordBoxRegisterPassword.Password
                };
                App.Store.Dispatch(new RegisterAction
                {
                    registerUserParams = userParams
                });
                App.Store.Dispatch(new LoginAction
                {
                    loginUserParams = userParams
                });
                await this.ShowMessageAsync("Success", "Register & Login Successfully");
                this.Close();
            }
            catch(Exception ex)
            {
                await this.ShowMessageAsync("Error", ex.Message);
            }
        }

        private async void ButtonLogin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                App.Store.Dispatch(new LoginAction
                {
                    loginUserParams = new Model.LoginUserParams
                    {
                        UserName = textBoxUserName.Text,
                        Password = textBoxPassword.Password
                    }
                });
                await this.ShowMessageAsync("Success", "Login successfully");
                this.Close();
            }
            catch(Exception ex)
            {
                var error = ex;
                await this.ShowMessageAsync("Error", ex.Message);
            }
        }

        private void ButtonBackToLogin_Click(object sender, RoutedEventArgs e)
        {
            
            Dispatcher.BeginInvoke((System.Action)(() => tabControlLogin.SelectedIndex = 0));
            buttonBackToLogin.Visibility = Visibility.Hidden;
        }

        private void ButtonMoveToRegister_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke((System.Action)(() => tabControlLogin.SelectedIndex = 1));
            buttonBackToLogin.Visibility = Visibility.Visible;
        }
    }
}
