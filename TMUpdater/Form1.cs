using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TMUpdater
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            ClosedProcess();
            Download();
        }

        private void ClosedProcess()
        {
            Process[] processes = Process.GetProcessesByName("TMAgent");
            foreach (Process process in processes)
            {
                process.Kill();
            }

            using (var serviceController = new ServiceController("TMService"))
            {
                if(serviceController.Status == ServiceControllerStatus.Running)
                {
                    serviceController.Stop();
                    serviceController.WaitForStatus(ServiceControllerStatus.Stopped);
                }
            }
        }

        private void Download()
        {
            WebClient client = new WebClient();
            client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(client_DownloadProgressChanged);
            client.DownloadFileCompleted += new AsyncCompletedEventHandler(client_DownloadFileCompleted);

            //Here you can add a download file with paths for downloading updates.
            client.DownloadFileAsync(new Uri("https://moscow.sprashivai.ru/46,02297b47bf93736c.jpg"), "Test.jpg");

            StartProcess();
        }

        private void client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            StartProcess();
        }
        private void client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            progressBar.Maximum = (int)e.TotalBytesToReceive / 100;
            progressBar.Value = (int)e.BytesReceived / 100;
        }


        private void StartProcess()
        {
            Process.Start($@"{AppDomain.CurrentDomain.BaseDirectory}\TMAgent\TMAgent.exe");

            using (var serviceController = new ServiceController("TMService"))
            {
                serviceController.Start();
                serviceController.WaitForStatus(ServiceControllerStatus.Running);
            }
        }
    }
}
