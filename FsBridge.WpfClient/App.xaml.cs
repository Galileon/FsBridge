using Catel;
using Catel.IoC;
using Catel.Logging;
using Catel.Messaging;
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
            //MessageMediator.Default.
            ServiceLocator.Default.RegisterInstance<FsClient.FreeswitchClient>(new FsClient.FreeswitchClient(new FsClient.FreeswitchConfiguration() {  Context = ""}, null));
        }

        protected override void OnExit(ExitEventArgs e)
        {
            ServiceLocator.Default.ResolveType<FsClient.FreeswitchClient>()?.Close();
            base.OnExit(e); 
        }

    }

}
