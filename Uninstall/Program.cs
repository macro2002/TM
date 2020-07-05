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

            Process.Start($@"{path}\TMService\TMService.exe", "-uninstall");

            //There is an error here
            return;
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
