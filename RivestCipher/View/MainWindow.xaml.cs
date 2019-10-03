using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Win32;
using RivestCipher.Helper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using RivestCipher.View;
using static RivestCipher.Action.UserProfileAction;
using System.ComponentModel;

namespace RivestCipher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private static readonly string RIVEST_CIPHER_FOLDER_PATH = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "RivestCipher_DuyAnh");
        private static readonly string ENCRYPT_FOLDER_PATH = Path.Combine(RIVEST_CIPHER_FOLDER_PATH, "encrypt");
        private static readonly string DECRYPT_FOLDER_PATH = Path.Combine(RIVEST_CIPHER_FOLDER_PATH, "decrypt");
        private static readonly string SETTING_FOLDER_PATH = Path.Combine(RIVEST_CIPHER_FOLDER_PATH, "setting");
        private static readonly string USER_FILE_PATH = Path.Combine(SETTING_FOLDER_PATH, "user.xml");
        private static readonly string DOCUMENT_FILE_PATH = Path.Combine(SETTING_FOLDER_PATH, "document.xml");
        private static List<string> _listEncryptFilePath;
        private enum ErrorType
        {
            PasswordEmpty = 0
        }


        public MainWindow()
        {
            InitializeComponent();
            _listEncryptFilePath = new List<string>();
            btnOpenFile.Click += BtnOpenFile_Click;
            btnEncrypt.Click += BtnEncrypt_Click;
            btnDecrypt.Click += BtnDecrypt_Click;
            tbPassword.PreviewMouseDown += TbPassword_MouseLeftButtonUp;
            buttonLogin.Click += ButtonLogin_Click;
            buttonLogout.Click += ButtonLogout_Click;

            CheckUserHasLoggedIn();
        }

        private async void ButtonLogout_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            App.Store.Dispatch(new LogoutAction());
            CheckUserHasLoggedIn();
            await this.ShowMessageAsync("Success", "Logout Successfully");
        }

        private void CheckUserHasLoggedIn()
        {
            var isUserLoggedIn = App.Store.GetState().UserProfile != null;
            buttonLogin.Visibility = !isUserLoggedIn ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
            dockPanelLogout.Visibility = isUserLoggedIn ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
            labelUserName.Content = isUserLoggedIn ? App.Store.GetState().UserProfile.UserName : String.Empty;
        }

        private void ButtonLogin_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var loginView = new LoginView(USER_FILE_PATH);
            loginView.Show();
            loginView.Closing += LoginView_Closing;
        }

        private void LoginView_Closing(object sender, CancelEventArgs e)
        {
            CheckUserHasLoggedIn();
        }


        private void TbPassword_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            tbPassword.ClearValue(System.Windows.Controls.TextBox.BorderBrushProperty);
        }

        private void BtnDecrypt_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                if (!Validate())
                {
                    return;
                }
                ChangeControlsStatus(true);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                ChangeControlsStatus(false);
            }
        }

        private void BtnEncrypt_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                if (!Validate())
                {
                    return;
                }
                ChangeControlsStatus(true);
                foreach (var path in _listEncryptFilePath)
                {
                    if (!File.Exists(path) || !CanReadFile.Check(path))
                    {
                        continue;
                    }
                    var rc4 = new RC4(tbPassword.Text.Trim(), File.ReadAllBytes(path));
                    var encryptedFileRaw = rc4.Encrypt();
                    var encryptedFilePath = Path.Combine(ENCRYPT_FOLDER_PATH, string.Join("_", Path.GetFileNameWithoutExtension(path), string.Format("{0:yyyy-MM-dd_hh-mm-ss-fff}", DateTime.Now)));
                    File.WriteAllBytes(encryptedFilePath, encryptedFileRaw);

                    var decryptedFileRaw = rc4.Decrypt(encryptedFilePath);
                    var decryptedFilePath = Path.Combine(DECRYPT_FOLDER_PATH, string.Join("_", Path.GetFileNameWithoutExtension(path), string.Format("{0:yyyy-MM-dd_hh-mm-ss-fff}.{1}", DateTime.Now, Path.GetExtension(path))));
                    File.WriteAllBytes(decryptedFilePath, decryptedFileRaw);
                }
                Process.Start(RIVEST_CIPHER_FOLDER_PATH);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                ChangeControlsStatus(false);
            }
        }

        private void ChangeControlsStatus(bool isLoading)
        {
            //btnDecrypt.IsEnabled = !isLoading;
            btnEncrypt.IsEnabled = !isLoading;
            btnOpenFile.IsEnabled = !isLoading;
            tbPassword.IsEnabled = !isLoading;
        }

        private Boolean Validate()
        {
            if (_listEncryptFilePath.Count <= 0)
            {
                return false;
            }
            if (String.IsNullOrWhiteSpace(tbPassword.Text))
            {
                SetError(ErrorType.PasswordEmpty);
                return false;
            }
            return true;
        }

        private void SetError(ErrorType errorType)
        {
            switch (errorType)
            {
                case ErrorType.PasswordEmpty:
                    tbPassword.BorderBrush = System.Windows.Media.Brushes.Red;
                    break;
            }
        }

        private void BtnOpenFile_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;
            _listEncryptFilePath.Clear();
            tbSelectedFile.Text = String.Empty;
            if (openFileDialog.ShowDialog() == true)
            {
                if (openFileDialog.FileNames.Length > 0)
                {
                    tbPassword.Visibility = System.Windows.Visibility.Visible;
                    var listSelectedFiles = string.Empty;
                    foreach (var filepath in openFileDialog.FileNames)
                    {
                        _listEncryptFilePath.Add(filepath);
                        listSelectedFiles = string.IsNullOrWhiteSpace(listSelectedFiles) ?
                            Path.GetFileName(filepath) :
                            string.Join(", ", listSelectedFiles, Path.GetFileName(filepath));
                    }
                    tbSelectedFile.Text = listSelectedFiles;
                    EnableEncryption();
                }
            }
        }

        private void EnableEncryption()
        {
            btnEncrypt.IsEnabled = true;
            //btnDecrypt.IsEnabled = true;
        }
    }
}
