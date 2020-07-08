using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TMProcess
{
    public class Manager
    {
        private bool isStop;
        public void Start()
        {
            Task.Factory.StartNew(() => {
                while (!isStop)
                {
                    var path = $@"{AppDomain.CurrentDomain.BaseDirectory}\cron.tab";
                    if (File.Exists(path))
                    {
                        using (StreamReader sr = new StreamReader(path, Encoding.Default))
                        {
                            string line;
                            while ((line = sr.ReadLine()) != null)
                            {
                                //Splitting the string into the path and launch parameters
                                string[] words = line.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                                string commands = "";

                                for (int i = 1; i < words.Count(); i++)
                                {
                                    commands = $"{commands} {words[i]}";
                                }

                                try
                                {
                                    using (var process = new Process())
                                    {
                                        process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                                        process.StartInfo.UseShellExecute = false;
                                        process.StartInfo.RedirectStandardOutput = true;
                                        process.StartInfo.FileName = words[0];
                                        process.StartInfo.Arguments = commands;
                                        process.Start();
                                    }
                                }
                                catch
                                {
                                    //Here you can add error logging
                                }
                            }
                        }
                    }   
                    //Five minutes
                    Thread.Sleep(300000);
                }
            });
        }

        public void Stop()
        {
            isStop = true;
        }
    }
}
