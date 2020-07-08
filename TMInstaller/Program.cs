using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Windows.Forms;

namespace TMInstaller
{
    static class Program
    {
        private static Mutex _syncObject;
        private const string _syncObjectName = "{68e9dbf9-288e-444b-bc54-dac8041e653f}";

        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            bool createdNew;
            _syncObject = new Mutex(true, _syncObjectName, out createdNew);
            if (!createdNew) return;

            WindowsPrincipal pricipal = new WindowsPrincipal(WindowsIdentity.GetCurrent());
            bool hasAdministrativeRight = pricipal.IsInRole(WindowsBuiltInRole.Administrator);

            if (hasAdministrativeRight == false)
            {
                ProcessStartInfo processInfo = new ProcessStartInfo();
                processInfo.Verb = "runas";
                processInfo.FileName = Application.ExecutablePath;

                var arguments = "";
                for (int i = 0; i < args.Count(); i++)
                {
                    arguments = $"{arguments} {args[i]}";
                }
                processInfo.Arguments = arguments;
                try
                {
                    Process.Start(processInfo); 
                }
                catch (Win32Exception)
                {
                    
                }
                Application.Exit();
            }
            else 
            {
                if(args.Length == 0)
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new Form());
                } else
                {
                    Function.SilentIntall(args);
                }
            }
        }
    }
}
