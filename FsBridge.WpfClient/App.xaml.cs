using Catel;
using Catel.IoC;
using Catel.Logging;
using System.Configuration;
using System.Data;
using System.Windows;

namespace FsBridge.WpfClient
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            ServiceLocator.Default.RegisterInstance<FsClient.FreeswitchClient>(new FsClient.FreeswitchClient(new FsClient.FreeswitchConfiguration(), null));
        }

        protected override void OnExit(ExitEventArgs e)
        {
            ServiceLocator.Default.ResolveType<FsClient.FreeswitchClient>()?.Close();
            base.OnExit(e); 
        }

    }

}
