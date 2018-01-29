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
    public partial class Seconds : Form
    {
        Main MainForm;
        System.Windows.Forms.Timer timer;
        //Thread ShowThread;
        bool go_on = false;//是否为继续计时
        DateTime PastTime;
        public Seconds(Main MainForm)
        {
            InitializeComponent();
            this.MainForm = MainForm;
            timer = new System.Windows.Forms.Timer();
            timer.Interval = 1;
            timer.Tick += timer_Tick;
        }
        DateTime StartTime;
        long ticks;
        string hour = "", min = "", sec = "", millisec = "";
        void timer_Tick(object sender, EventArgs e)
        {
            if (go_on)
            {
                StartTime = new DateTime(StartTime.Ticks - PastTime.Ticks);//补上之前走过的时间
                go_on = false;
            }
            ticks = (DateTime.Now - StartTime).Ticks;
            PastTime = new DateTime(ticks);
            hour = format(PastTime.Hour.ToString());
            min = format(PastTime.Minute.ToString());
            sec = format(PastTime.Second.ToString());
            millisec = format(PastTime.Millisecond.ToString());
            label1.Text = String.Format("{0}:{1}:{2}.{3}", hour, min, sec, millisec);
        }
        void Seconds_FormClosed(object sender, FormClosedEventArgs e)
        {
            //if (ShowThread != null) ShowThread.Abort();
            timer.Stop();
            if (MainForm != null) MainForm.Show();
        }
        void button1_Click(object sender, EventArgs e)//开始
        {
            StartTime = DateTime.Now;
            button1.Enabled = false;
            button2.Enabled = true;
            button3.Enabled = false;
            //ShowThread = new Thread(new ThreadStart(ShowThreadMethod));
            //ShowThread.Start();
            timer.Start();
        }
        void ShowThreadMethod()
        {
            DateTime StartTime = DateTime.Now;
            long ticks;
            string hour = "", min = "", sec = "", millisec = "";
            while (true)
            {
                if (go_on)
                {
                    StartTime = new DateTime(StartTime.Ticks - PastTime.Ticks);//补上之前走过的时间
                    go_on = false;
                }
                ticks = (DateTime.Now - StartTime).Ticks;
                PastTime = new DateTime(ticks);
                hour = format(PastTime.Hour.ToString());
                min = format(PastTime.Minute.ToString());
                sec = format(PastTime.Second.ToString());
                millisec = format(PastTime.Millisecond.ToString());
                this.Invoke(new EventHandler(delegate
                {
                    label1.Text = String.Format("{0}:{1}:{2}.{3}", hour, min, sec, millisec);
                }));
            }
        }
        string format(string number)
        {
            switch (number.ToString().Length)
            {
                case 1: return number = "0" + number;
                case 2: return number;
                case 3: return number.Substring(0, 2);//针对毫秒
                default: return "00";//针对毫秒
            }
        }
        void button2_Click(object sender, EventArgs e)//暂停
        {
            button2.Enabled = false;
            button1.Text = "继续(&S)";
            //if (ShowThread != null) ShowThread.Abort();
            timer.Stop();
            go_on = true;
            button1.Enabled = true;
            button3.Enabled = true;
        }
        void button3_Click(object sender, EventArgs e)//重置
        {
            button3.Enabled = false;
            button1.Text = "开始(&S)";
            label1.Text = "00:00:00.00";
            go_on = false;
        }
    }
}
