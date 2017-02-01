using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace snap
{
    class Class1
    {

        string capFolder = @"data\";

        public Class1()
        {

            if (!Directory.Exists(Path.GetDirectoryName(capFolder)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(capFolder));
            }

            IEnumerable<string> localFiles = new System.IO.DirectoryInfo("data\\").GetFiles("*.png").Select(fi => fi.Name).OrderByDescending(v => v);

            int index = 0;

            foreach (var name in localFiles)
            {
                try
                {
                    index = int.Parse(name.Substring(0, name.Length - 4));
                    break;
                }
                catch (Exception) { }
            }

            index = index + 1;

            string newName = index.ToString().PadLeft(3, '0') + ".png";
            Console.WriteLine(newName);

            startCap(newName);

        }

        void startCap(string name)
        {
            runExe(capFolder + name, "boxcutter.exe", waitForSnap);
        }

        void waitForSnap(object sender2, System.EventArgs e2)
        {
            Console.WriteLine("box finished");
            Application.Exit();
        }


        Process runExe(string arg, string exePath, Action<object, EventArgs> onExit)
        {
            string path = Directory.GetCurrentDirectory();
            DirectoryInfo d = new DirectoryInfo(path);

            Process myProcess = new Process();

            ProcessStartInfo startInfo = new ProcessStartInfo();

            startInfo.CreateNoWindow = false;
            startInfo.UseShellExecute = true;
            startInfo.FileName = exePath;
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.WorkingDirectory = d.FullName;
            startInfo.Arguments = arg;

            myProcess.StartInfo = startInfo;
            if (onExit != null)
            {
                myProcess.EnableRaisingEvents = true;
                myProcess.Exited += new EventHandler(onExit);
            }
            myProcess.Start();

            return myProcess;
        }
    }
}
