using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.ServiceProcess;
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
                    Function.TMAgentInstall(txtPath.Text);
                }
                if(chkAgent.Checked)
                {
                    Function.TMServiceIntall(txtPath.Text);
                }
                File.WriteAllBytes($@"{txtPath.Text}\TM\TMUpdater.exe", Resources.TMUpdater);
                File.WriteAllBytes($@"{txtPath.Text}\TM\Uninstall.exe", Resources.Uninstall);
                Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
