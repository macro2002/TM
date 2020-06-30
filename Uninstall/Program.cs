using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Uninstall
{
    class Program
    {
        static void Main(string[] args)
        {
            var path = AppDomain.CurrentDomain.BaseDirectory;

            Process[] processes = Process.GetProcessesByName("TMAgent");
            foreach (Process process in processes)
            {
                process.Kill();
            }

            RegistryKey registry = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            registry.DeleteValue("TMAgent", false);

            using (var serviceController = new ServiceController("TMService"))
            {
                if (serviceController.Status == ServiceControllerStatus.Running)
                {
                    serviceController.Stop();
                    serviceController.WaitForStatus(ServiceControllerStatus.Stopped);
                }
            }

            try
            {
                var utilPath = "";
                FileInfo w64 = new FileInfo(@"C:\Windows\Microsoft.NET\Framework64\v4.0.30319\InstallUtil.exe");
                if (w64.Exists)
                {
                    utilPath = @"C:\Windows\Microsoft.NET\Framework64\v4.0.30319\InstallUtil.exe";

                }
                else
                {
                    FileInfo w32 = new FileInfo(@"C:\Windows\Microsoft.NET\Framework\v4.0.30319\InstallUtil.exe");
                    if (w32.Exists)
                    {
                        utilPath = @"C:\Windows\Microsoft.NET\Framework\v4.0.30319\InstallUtil.exe";
                    }
                    else
                    {
                        return;
                    }
                }
                Process.Start(utilPath, $@"/u {path}\TMService\TMService.exe");
            }
            catch
            {
                Console.WriteLine("Error: The service cannot be registered");
                Console.ReadKey();
            }

            DirectoryInfo dirInfo = new DirectoryInfo($@"{path}\TMAgent");
            if (dirInfo.Exists)
            {
                dirInfo.Delete(true);
            }
            DirectoryInfo dirSevice = new DirectoryInfo($@"{path}\TMService");
            if (dirSevice.Exists)
            {
                dirSevice.Delete(true);
            }
        }
    }
}
