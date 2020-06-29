using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using TMAgent.Properties;
using TMProcess;

namespace TMAgent
{
    static class Program
    {
        private static Mutex _syncObject;
        private const string _syncObjectName = "{0f9443d6-af89-4ddf-bab6-9c515d7ab4a9}";

        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            bool createdNew;
            _syncObject = new Mutex(true, _syncObjectName, out createdNew);
            if (!createdNew) return;

            NotifyIcon i = new NotifyIcon();
            i.Visible = true;
            i.Icon = Resources.logo;
            i.ContextMenuStrip = new ContextMenuStrip();
            i.ContextMenuStrip.Items.Add("Update", null, delegate { Update(); });
            i.ContextMenuStrip.Items.Add("Install", null, delegate { Install(); });
            i.ContextMenuStrip.Items.Add("Exit", null, delegate { Application.Exit(); });

            Manager manager = new Manager();
            manager.Start();

            Application.Run();
        }


        private static void Update()
        {
            //Here you can first contact the server to get information about the update
            var version = "1";
            if (Assembly.GetExecutingAssembly().GetName().Version.ToString() == version)
            {
                var path = AppDomain.CurrentDomain.BaseDirectory;
                path = path.Substring(0, path.IndexOf(@"\TM\"));

                using (var process = new Process())
                {
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.FileName = $@"{path}\TM\Update.exe";
                    process.Start();
                }
            } else
            {
                MessageBox.Show("You have the latest version installed");
            }
        }

        private static void Install()
        {
            var fileName = "";
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
            WebClient client = new WebClient();
            client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(client_DownloadProgressChanged);
            client.DownloadFileCompleted += new AsyncCompletedEventHandler(client_DownloadFileCompleted);
            client.DownloadFileAsync(new Uri("https://moscow.sprashivai.ru/46,02297b47bf93736c.jpg"), "Test.jpg");
        }

        private static void client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            using (var process = new Process())
            {
                process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.FileName = "path";
                process.Start();
            }
        }
        private static void client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            //progressBar1.Maximum = (int)e.TotalBytesToReceive / 100;
            //progressBar1.Value = (int)e.BytesReceived / 100;
        }

    }
}
