using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.Drawing;
using System.Text.RegularExpressions;

namespace snap
{
    class Class1
    {

        string capFolder = @"data\";
        string newName;
        string coordFile;
        StringBuilder strOut = new StringBuilder();
        bool isReplay = false;
        Form1 form;

        public Class1(Form1 _form)
        {
            form = _form;

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

            newName = index.ToString().PadLeft(3, '0');
            Console.WriteLine(newName);

            coordFile = capFolder + newName + ".txt";
            if (File.Exists(coordFile))
            {
                isReplay = true;
                form.Visible = true;
                form.Show();

                // wait for mouseUp event to call startCap2
            }
            else
            {
                isReplay = false;
                startCap(newName + ".png");
            }

        }

        public void startCap2()
        {
            string args = "-c " + File.ReadAllText(coordFile) + " " + capFolder + newName + ".png";
            Console.WriteLine(args);
            runExe(args, "boxcutter.exe", waitForSnap, strOut);
        }

        void startCap(string name)
        {
            runExe(capFolder + name, "boxcutter.exe", waitForSnap, strOut);
        }

        void waitForSnap(object sender2, System.EventArgs e2)
        {
            string str = strOut.ToString();

            MatchCollection matches = Regex.Matches(str.Split('\n').First(), @"\d+");

            if (isReplay || !str.Contains("coords:") || matches.Count < 4)
            {
                Application.Exit();
                return;
            }

            string result = "";
            for (int i = 0; i < 4; i++)
            {
                result += matches[i].ToString() + ",";
            }

            File.WriteAllText(capFolder + newName + ".txt", result.TrimEnd(','));
            Console.WriteLine("box finished:" + str);
            Application.Exit();
        }

        static Process runExe(string arg, string exePath, Action<object, EventArgs> onExit, StringBuilder outputBuilder)
        {
            string path = Directory.GetCurrentDirectory();
            DirectoryInfo d = new DirectoryInfo(path);

            Process myProcess = new Process();

            ProcessStartInfo startInfo = new ProcessStartInfo();

            startInfo.CreateNoWindow = false;
            startInfo.UseShellExecute = true;
            startInfo.FileName = exePath;
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            // startInfo.WorkingDirectory = d.FullName;
            startInfo.Arguments = arg;
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.CreateNoWindow = true;

            if (outputBuilder != null)
            {
                startInfo.UseShellExecute = false;
                startInfo.RedirectStandardError = true;
                startInfo.RedirectStandardOutput = true;
                myProcess.OutputDataReceived += (sender, eventArgs) => outputBuilder.AppendLine(eventArgs.Data);
                myProcess.ErrorDataReceived += (sender, eventArgs) => outputBuilder.AppendLine(eventArgs.Data);
            }


            if (onExit != null)
            {
                myProcess.EnableRaisingEvents = true;
                myProcess.Exited += new EventHandler(onExit);
            }

            myProcess.StartInfo = startInfo;
            myProcess.Start();

            if (outputBuilder != null)
            {
                myProcess.BeginOutputReadLine();
                myProcess.BeginErrorReadLine();
            }

            return myProcess;
        }

    }
}
