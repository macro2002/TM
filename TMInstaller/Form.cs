using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
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
            if (chkService.Checked == false && chkAgent.Checked == false)
            {
                MessageBox.Show("Error: Select the app to install");
            } else
            {
                if(chkService.Checked)
                {
                    TMAgentInstall();
                }
                if(chkAgent.Checked)
                {
                    TMServiceIntall();
                }
                File.WriteAllBytes($@"{txtPath.Text}\TM\TMUpdater.exe", Resources.TMUpdater);
                Close();
            }
        }

        private void TMAgentInstall()
        {
            Directory.CreateDirectory($@"{txtPath.Text}\TM\TMAgent");
            File.WriteAllBytes($@"{txtPath.Text}\TM\TMAgent\TMAgent.exe", Resources.TMAgent);
            File.WriteAllBytes($@"{txtPath.Text}\TM\TMAgent\TMProcess.dll", Resources.TMProcess);

            RegistryKey registry = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            registry.SetValue("TMAgent", $@"{txtPath.Text}\TM\TMAgent\TMAgent.exe");

            Process.Start($@"{txtPath.Text}\TM\TMAgent\TMAgent.exe");
        }

        private void TMServiceIntall()
        {
            Directory.CreateDirectory($@"{txtPath.Text}\TM\TMService");
            File.WriteAllBytes($@"{txtPath.Text}\TM\TMService\TMService.exe", Resources.TMService);
            File.WriteAllBytes($@"{txtPath.Text}\TM\TMService\TMProcess.dll", Resources.TMProcess);

            TestData();
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
                Process.Start(utilPath, $@"{txtPath.Text}\TM\TMService\TMService.exe");
            }
            catch
            {
                MessageBox.Show("Error: The service cannot be registered");
            }
        }


        private void TestData()
        {
            using (FileStream fstream = new FileStream($@"{txtPath.Text}\TM\TMService\cron.tab", FileMode.OpenOrCreate))
            {
                byte[] array = Encoding.Default.GetBytes(@"D:\Project\Consta\Consta\bin\Debug\Consta.exe param1 param2");
                fstream.Write(array, 0, array.Length);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
