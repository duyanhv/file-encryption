using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using RivestCipher.Helper;
using RivestCipher.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using static RivestCipher.Action.DocumentAction;

namespace RivestCipher.View
{
    /// <summary>
    /// Interaction logic for InputBox.xaml
    /// </summary>
    public partial class InputBox : MetroWindow
    {
        private enum ErrorType
        {
            PasswordEmpty = 0
        }
        private bool _isEncrypted;
        private DocumentModel _document;
        public InputBox(DocumentModel document, bool isEncrypted)
        {
            InitializeComponent();
            if (document != null)
            {
                textBoxInputBox.Text = document.Password;
            }
            _document = document;
            _isEncrypted = isEncrypted;
            buttonAccept.Click += ButtonAccept_Click;
            textBoxInputBox.PreviewMouseDown += TextBoxInputBox_PreviewMouseDown;
            buttonCancel.Click += ButtonCancel_Click;
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void TextBoxInputBox_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            textBoxInputBox.ClearValue(System.Windows.Controls.TextBox.BorderBrushProperty);
        }

        private Boolean Validate()
        {
            if (String.IsNullOrWhiteSpace(textBoxInputBox.Text))
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
                    textBoxInputBox.BorderBrush = System.Windows.Media.Brushes.Red;
                    break;
            }
        }
        private async void ButtonAccept_Click(object sender, RoutedEventArgs e)
        {
            if (!Validate())
            {
                return;
            }
            if (!File.Exists(_document.Path) || !CanReadFile.Check(_document.Path))
            {
                return;
            }
            _document.Password = textBoxInputBox.Text.Trim();
            if (_isEncrypted)
            {
                App.Store.Dispatch(new EncryptAction
                {
                    createDocumentParams = new List<DocumentModel>
                        {
                          _document
                        }
                });
            }else
            {
                App.Store.Dispatch(new DecryptAction
                {
                    createDocumentParams = new List<DocumentModel>
                        {
                          _document
                        }
                });
            }
            await this.ShowMessageAsync("Success", $"{(_isEncrypted ? "Encrypt" : "Decrypt")} Successfully");
            this.Close();
        }
    }
}
