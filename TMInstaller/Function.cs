using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using TMInstaller.Properties;

namespace TMInstaller
{
    public class Function
    {
        public static void SilentIntall(string[] args)
        {
            if (args[0] == "/s")
            {
                var path = ProgramFiles();

                var p = args.FirstOrDefault(s => s.Contains("/p="));
                if (!String.IsNullOrEmpty(p))
                {
                    path = p.Substring(3);
                }

                if (!args.Contains("/-agent"))
                {
                    TMAgentInstall(path);
                }

                if (!args.Contains("/-service"))
                {
                    TMServiceIntall(path);
                }

                File.WriteAllBytes($@"{path}\TM\TMUpdater.exe", Resources.TMUpdater);
                File.WriteAllBytes($@"{path}\TM\Uninstall.exe", Resources.Uninstall);
            }
        }

        public static void TMAgentInstall(string path)
        {
            Directory.CreateDirectory($@"{path}\TM\TMAgent");
            File.WriteAllBytes($@"{path}\TM\TMAgent\TMAgent.exe", Resources.TMAgent);
            File.WriteAllBytes($@"{path}\TM\TMAgent\TMProcess.dll", Resources.TMProcess);
            File.Create($@"{path}\TM\TMAgent\cron.tab");

            RegistryKey registry = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            registry.SetValue("TMAgent", $@"{path}\TM\TMAgent\TMAgent.exe");

            Process.Start($@"{path}\TM\TMAgent\TMAgent.exe");
        }

        public static void TMServiceIntall(string path)
        {
            Directory.CreateDirectory($@"{path}\TM\TMService");
            File.WriteAllBytes($@"{path}\TM\TMService\TMService.exe", Resources.TMService);
            File.WriteAllBytes($@"{path}\TM\TMService\TMProcess.dll", Resources.TMProcess);
            File.Create($@"{path}\TM\TMService\cron.tab");

            Process.Start($@"{path}\TM\TMService\TMService.exe", "-install");
        }

        private static string ProgramFiles()
        {
            if (8 == IntPtr.Size
                || (!String.IsNullOrEmpty(Environment.GetEnvironmentVariable("PROCESSOR_ARCHITEW6432"))))
            {
                return Environment.GetEnvironmentVariable("ProgramFiles(x86)");
            }

            return Environment.GetEnvironmentVariable("ProgramFiles");
        }
    }
}
