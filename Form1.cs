using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace snap
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.FormBorderStyle = FormBorderStyle.None;
        }

        bool isReplay = false;

        private void Form1_Load(object sender, EventArgs e)
        {
            Process[] processes = Process.GetProcessesByName(@"ppp.exe");

            isReplay = processes.Length == 0;
            if (isReplay)
            {
                // it's in replay mode
            }
            else
            {
                this.Hide();
                new Class1(false);
            }
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            Console.WriteLine(33333333333);
            this.Hide();
            new Class1(true);
        }
    }
}
