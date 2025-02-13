using System.ComponentModel;
using System.ServiceProcess;

namespace ComandasServerService
{
    [RunInstaller(true)]
    public partial class ServerServiceInstaller : System.Configuration.Install.Installer
    {
        private ServiceProcessInstaller _processInstaller;
        private ServiceInstaller _serviceInstaller;

        public ServerServiceInstaller()
        {
            //InitializeComponent();

            _processInstaller = new ServiceProcessInstaller();
            _serviceInstaller = new ServiceInstaller();

            _processInstaller.Account = ServiceAccount.LocalSystem;

            _serviceInstaller.ServiceName = "ComandasServerService";
            _serviceInstaller.Description = "Serviço do servidor de comandas";
            _serviceInstaller.StartType = ServiceStartMode.Automatic;

            Installers.Add(_processInstaller);
            Installers.Add(_serviceInstaller);
        }
    }
}
// %WINDIR%\Microsoft.NET\Framework\v4.0.30319\InstallUtil.exe