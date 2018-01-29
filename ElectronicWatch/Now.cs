using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace ElectronicWatch
{
    public partial class Now : Form
    {
        Main MainForm;
        System.Windows.Forms.Timer timer;
        public Now(Main MainForm)
        {
            InitializeComponent();
            this.MainForm = MainForm;
            timer = new System.Windows.Forms.Timer();
            timer.Interval = 1;
            timer.Tick += timer_Tick;
            timer.Enabled = true;
        }

        DateTime now;
        void timer_Tick(object sender, EventArgs e)
        {
            now = DateTime.Now;
            label1.Text = String.Format("{0}-{1}-{2}  {3}", now.Year, now.Month, now.Day, CN(now.DayOfWeek));
            label2.Text = now.ToLongTimeString();
        }
        string CN(DayOfWeek day)
        {
            switch (day)
            {
                case DayOfWeek.Monday: return "星期一";
                case DayOfWeek.Tuesday: return "星期二";
                case DayOfWeek.Wednesday: return "星期三";
                case DayOfWeek.Thursday: return "星期四";
                case DayOfWeek.Friday: return "星期五";
                case DayOfWeek.Saturday: return "星期六";
                case DayOfWeek.Sunday: return "星期天";
                default: return "";
            }
        }
        void Now_FormClosed(object sender, FormClosedEventArgs e)
        {
            timer.Stop();
            if (MainForm != null) MainForm.Show();
        }
    }
}
