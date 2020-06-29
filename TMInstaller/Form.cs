using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TMInstaller.Properties;

namespace TMInstaller
{
    public partial class Form : System.Windows.Forms.Form
    {
        public Form()
        {
            InitializeComponent();
        }

        private void btnSelectPath_Click(object sender, EventArgs e)
        {
            var dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                txtPath.Text = dialog.SelectedPath;
            }
        }

        private void btnInstall_Click(object sender, EventArgs e)
        {
            Directory.CreateDirectory($@"{txtPath.Text}\TM\TMService");
            File.WriteAllBytes($@"{txtPath.Text}\TM\TMService\TMService.exe", Resources.TMService);
            File.WriteAllBytes($@"{txtPath.Text}\TM\TMService\TMProcess.dll", Resources.TMProcess);

            Directory.CreateDirectory($@"{txtPath.Text}\TM\TMAgent");
            File.WriteAllBytes($@"{txtPath.Text}\TM\TMAgent\TMAgent.exe", Resources.TMAgent);
            File.WriteAllBytes($@"{txtPath.Text}\TM\TMAgent\TMProcess.dll", Resources.TMProcess);

            TestData();

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
            Process.Start(utilPath, $@"{txtPath.Text}\TM\TMService\TMService.exe");

            Close();
        }

        private void TestData()
        {
            using (FileStream fstream = new FileStream($@"{txtPath.Text}\TM\TMService\cron.tab", FileMode.OpenOrCreate))
            {
                byte[] array = Encoding.Default.GetBytes("D:/Project/Consta/Consta/bin/Debug/Consta.exe param1 param2");
                fstream.Write(array, 0, array.Length);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
