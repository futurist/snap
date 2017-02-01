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
            Visible = false; // Hide form window.
            ShowInTaskbar = false; // Remove from taskbar.

            base.OnLoad(e);
            this.FormBorderStyle = FormBorderStyle.None;
        }

        Class1 player = null;

        private void Form1_Load(object sender, EventArgs e)
        {

            this.Hide();
            player = new Class1(this);

        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            Console.WriteLine(33333333333);
            this.Hide();
            player.startCap2();
        }
    }
}
