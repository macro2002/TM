using System.ServiceProcess;

namespace TMService
{
    static class Program
    {

        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new TMService()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
