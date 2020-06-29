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
                while (true)
                {
                    using (StreamReader sr = new StreamReader($@"{AppDomain.CurrentDomain.BaseDirectory}\cron.tab", Encoding.Default))
                    {
                        Test("reader");
                        string line;
                        while ((line = sr.ReadLine()) != null)
                        {
                            //Splitting the string into the path and launch parameters
                            string[] words = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                            string commands = "";

                            for (int i = 1; i < words.Count(); i++)
                            {
                                commands = $"{commands} {words[i]}";
                            }

                            try
                            {
                                ProcessStartInfo pro = new ProcessStartInfo();
                                pro.FileName = words[0];
                                pro.Arguments = commands;
                                Process.Start(pro);

                                //Process x = Process.Start(pro);
                                //x.WaitForExit();
                            }
                            catch
                            {
                                Test("error open");
                                //Here you can add error logging
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

        private void Test(string action)
        {
            using (FileStream fstream = new FileStream($@"D:\TM\TMService\test.txt", FileMode.OpenOrCreate))
            {
                byte[] array = Encoding.Default.GetBytes($"{DateTime.Now}-{action}");
                fstream.Write(array, 0, array.Length);
            }
        }
    }
}
