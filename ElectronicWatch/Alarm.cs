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
    public partial class Alarm : Form
    {
        Main MainForm;
        System.Windows.Forms.Timer timer;
        //Thread ShowThread;
        public Alarm(Main MainForm)
        {
            InitializeComponent();
            this.MainForm = MainForm;
            numericUpDown1.Value = DateTime.Now.Hour;
            numericUpDown2.Value = DateTime.Now.Minute;
            timer = new System.Windows.Forms.Timer();
            timer.Interval = 1;
            timer.Tick += timer_Tick;
        }
        DateTime StartTime, EndTime, total, rest;
        int hour, min;
        long ticks;
        double proportion;
        void timer_Tick(object sender, EventArgs e)
        {
            ticks = (DateTime.Now - StartTime).Ticks;
            try { rest = new DateTime(total.Ticks - ticks); }
            catch
            {
                timer.Stop();
                MessageBox.Show("时间到！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                //try { this.Invoke(new EventHandler(delegate { Reset(); })); }
                //catch { }//窗体关闭后再点“确定”引发的异常
                Reset();
                return;
            }
            string h = format(rest.Hour.ToString());
            string m = format(rest.Minute.ToString());
            string s = format(rest.Second.ToString());
            label4.Text = String.Format("{0}:{1}:{2}", h, m, s);
            proportion = Convert.ToDouble(ticks) / Convert.ToDouble(total.Ticks);
            progressBar1.Value = Convert.ToInt32(proportion * 100);
        }
        void button1_Click(object sender, EventArgs e)//开始
        {
            button2.Enabled = true;
            //ShowThread = new Thread(new ThreadStart(ShowThreadMethod));
            //ShowThread.Start();
            StartTime = DateTime.Now;
            hour = Convert.ToInt32(numericUpDown1.Value);
            min = Convert.ToInt32(numericUpDown2.Value);
            EndTime = new DateTime(StartTime.Year, StartTime.Month, StartTime.Day, hour, min, 0);
            ticks = 0; 
            try
            {
                ticks = (EndTime - StartTime).Ticks;
                total = new DateTime(ticks);
            }
            catch
            {
                MessageBox.Show("请将时间设置在今日剩下的时段。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                return;
            }
            button1.Enabled = false;
            numericUpDown1.Enabled = false;
            numericUpDown2.Enabled = false;
            progressBar1.Style = ProgressBarStyle.Continuous;
            rest = new DateTime();
            timer.Start();
        }
        void ShowThreadMethod()
        {
            DateTime StartTime, EndTime, total;
            StartTime = DateTime.Now;
            int hour = Convert.ToInt32(numericUpDown1.Value);
            int min = Convert.ToInt32(numericUpDown2.Value);
            EndTime = new DateTime(StartTime.Year, StartTime.Month, StartTime.Day, hour, min, 0);
            long ticks = 0;
            try
            {
                ticks = (EndTime - StartTime).Ticks;
                total = new DateTime(ticks);
            }
            catch 
            { 
                MessageBox.Show("请将闹钟时间设置在今日剩下的时段内。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1); 
                return; 
            }
            this.Invoke(new EventHandler(delegate
            {
                button1.Enabled = false;
                numericUpDown1.Enabled = false;
                numericUpDown2.Enabled = false;
                progressBar1.Style = ProgressBarStyle.Continuous;
            }));
            DateTime rest = new DateTime();
            double proportion;
            while (true)
            {
                ticks = (DateTime.Now - StartTime).Ticks;
                try { rest = new DateTime(total.Ticks - ticks); }
                catch
                {
                    MessageBox.Show("闹钟时间到！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                    try { this.Invoke(new EventHandler(delegate { Reset(); })); }
                    catch { }//窗体关闭后再点“确定”引发的异常
                }
                string h = format(rest.Hour.ToString());
                string m = format(rest.Minute.ToString());
                string s = format(rest.Second.ToString());
                this.Invoke(new EventHandler(delegate
                {
                    label4.Text = String.Format("{0}:{1}:{2}", h, m, s);
                    proportion = Convert.ToDouble(ticks) / Convert.ToDouble(total.Ticks);
                    progressBar1.Value = Convert.ToInt32(proportion * 100);
                }));
            }
        }
        void button2_Click(object sender, EventArgs e)//重设
        {
            Reset();
        }
        void Alarm_FormClosed(object sender, FormClosedEventArgs e)
        {
            //if (ShowThread != null) ShowThread.Abort();
            timer.Stop();//如果不在关闭窗体时停止，且主进程未关闭，绑定方法将在窗体关闭后继续运行
            if (MainForm != null) MainForm.Show();
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
        void Reset()
        {
            timer.Stop();
            //if (ShowThread != null) ShowThread.Abort();
            button2.Enabled = false;
            button1.Enabled = true;
            //this.Invoke(new EventHandler(delegate { 
                label4.Text = "00:00:00";
            //}));//有必要用Invoke
            numericUpDown1.Enabled = true;
            numericUpDown2.Enabled = true;
            progressBar1.Style = ProgressBarStyle.Marquee;
        }
    }
}
