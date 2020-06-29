using System.Diagnostics;
using System.ServiceProcess;
using System.Threading;
using TMProcess;

namespace TMService
{
    public partial class TMService : ServiceBase
    {
        Manager manager = new Manager();

        public TMService()
        {
            InitializeComponent();
            this.CanStop = true;
            this.CanPauseAndContinue = true;
            this.AutoLog = true;
        }

        protected override void OnStart(string[] args)
        {
            manager.Start();
        }

        protected override void OnStop()
        {
            manager.Stop();
        }
    }
}
