using Redux;
using RivestCipher.Model;
using RivestCipher.Reducer;
using RivestCipher.State;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using static RivestCipher.Action.UserProfileAction;

namespace RivestCipher
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        //https://github.com/GuillaumeSalles/redux.NET/blob/master/examples/todomvc/Redux.TodoMvc/Reducers.cs

        private static readonly string RIVEST_CIPHER_FOLDER_PATH = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "RivestCipher_DuyAnh");
        private static readonly string DOCUMENT_FOLDER_PATH = Path.Combine(RIVEST_CIPHER_FOLDER_PATH, "document");
        private static readonly string SETTING_FOLDER_PATH = Path.Combine(RIVEST_CIPHER_FOLDER_PATH, "setting");
        private static readonly string USER_FILE_PATH = Path.Combine(SETTING_FOLDER_PATH, "user.xml");
        private static readonly string DOCUMENT_FILE_PATH = Path.Combine(SETTING_FOLDER_PATH, "document.xml");
        public static IStore<ApplicationState> Store { get; private set; }
        private void Application_Startup(object sender, StartupEventArgs e)
        {

            if (!Directory.Exists(SETTING_FOLDER_PATH))
            {
                Directory.CreateDirectory(SETTING_FOLDER_PATH);
            }
            if (!Directory.Exists(RIVEST_CIPHER_FOLDER_PATH))
            {
                Directory.CreateDirectory(RIVEST_CIPHER_FOLDER_PATH);
            }
            if (!Directory.Exists(DOCUMENT_FOLDER_PATH))
            {
                Directory.CreateDirectory(DOCUMENT_FOLDER_PATH);
            }

            if (!File.Exists(DOCUMENT_FILE_PATH))
            {
                File.Create(DOCUMENT_FILE_PATH);
            }
            if (!File.Exists(USER_FILE_PATH))
            {
                File.Create(USER_FILE_PATH);
            }

            var initialState = new ApplicationState {
                UserProfile = null,
                UserConnectionString = USER_FILE_PATH,
                DocumentConnectionString = DOCUMENT_FILE_PATH,
                DocumentFolder = DOCUMENT_FOLDER_PATH,
                Documents = null
            };
            StartupUri = new Uri("./View/MainWindow.xaml", UriKind.Relative);
            Store = new Store<ApplicationState>(Reducers.ReduceApplication, initialState);
            App.Store.Dispatch(new GetLoggedInUserAction());

        }
    }
}
