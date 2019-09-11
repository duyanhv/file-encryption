using MahApps.Metro.Controls;
using RivestCipher.Helper;
using System;
using System.IO;

namespace RivestCipher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private static readonly string RIVEST_CIPHER_FOLDER_PATH = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "RivestCipher_DuyAnh");
        public MainWindow()
        {
            InitializeComponent();
            if (!Directory.Exists(RIVEST_CIPHER_FOLDER_PATH))
            {
                Directory.CreateDirectory(RIVEST_CIPHER_FOLDER_PATH);
            }

            //var rc4 = new RC4("cuocsong", File.ReadAllBytes(@"C:\Users\duyan\Downloads\69245215_2709803829245920_8674948049375592448_o.jpg"));
            //rc4.Encrypt();
            //var newFile = rc4.Decrypt();
            //File.WriteAllBytes(@"C:\\Users\\duyan\\Desktop\\good2", newFile);
        }
    }
}
