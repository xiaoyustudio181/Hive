using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ElectronicWatch
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }
        void button1_Click(object sender, EventArgs e)
        {
            new Now(this).Show();
            this.Hide();
        }
        void button2_Click(object sender, EventArgs e)
        {
            new Seconds(this).Show();
            this.Hide();
        }
        void button3_Click(object sender, EventArgs e)
        {
            new CountDown(this).Show();
            this.Hide();
        }
        void button4_Click(object sender, EventArgs e)
        {
            new Alarm(this).Show();
            this.Hide();
        }
    }
}
