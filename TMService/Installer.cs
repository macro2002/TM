using System.ComponentModel;
using System.ServiceProcess;

namespace TMService
{
    [RunInstaller(true)]
    public partial class Installer : System.Configuration.Install.Installer
    {
        ServiceInstaller serviceInstaller;
        ServiceProcessInstaller processInstaller;
        public Installer()
        {
            InitializeComponent();

            serviceInstaller = new ServiceInstaller();
            processInstaller = new ServiceProcessInstaller();
            processInstaller.Account = ServiceAccount.LocalSystem;
            serviceInstaller.StartType = ServiceStartMode.Manual;
            serviceInstaller.ServiceName = "TMService";
            Installers.Add(processInstaller);
            Installers.Add(serviceInstaller);
        }
    }
}
