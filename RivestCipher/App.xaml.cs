using Redux;
using RivestCipher.Model;
using RivestCipher.Reducer;
using RivestCipher.State;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace RivestCipher
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        //https://github.com/GuillaumeSalles/redux.NET/blob/master/examples/todomvc/Redux.TodoMvc/Reducers.cs
        public static IStore<ApplicationState> Store { get; private set; }
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            StartupUri = new Uri("./View/MainWindow.xaml", UriKind.Relative);
            UserProfileStore = new Store<ApplicationState>(new ApplicationState { UserProfile = null }, Reducers);
        }
    }
}
