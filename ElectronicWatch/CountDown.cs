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
    public partial class CountDown : Form
    {
        Main MainForm;
        //Thread ShowThread;
        System.Windows.Forms.Timer timer;
        public CountDown(Main MainForm)
        {
            InitializeComponent();
            this.MainForm = MainForm;
            timer = new System.Windows.Forms.Timer();
            timer.Interval = 1;
            timer.Tick += timer_Tick;
        }
        DateTime total, rest = new DateTime(), StartTime;
        int hour, min;
        long ticks;
        string h = "", m = "", s = "";
        double proportion;
        void timer_Tick(object sender, EventArgs e)
        {
            ticks = (DateTime.Now - StartTime).Ticks;
            try
            {
                rest = new DateTime(total.Ticks - ticks);
                if (rest.Ticks == 0) throw new Exception();
            }
            catch
            {
                timer.Stop();
                MessageBox.Show("时间到！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                //try { this.Invoke(new EventHandler(delegate { Reset(); })); }
                //catch { }//窗体关闭后再点“确定”引发的异常
                Reset();
                return;
            }
            h = format(rest.Hour.ToString());
            m = format(rest.Minute.ToString());
            s = format(rest.Second.ToString());

            label4.Text = String.Format("{0}:{1}:{2}", h, m, s);
            proportion = Convert.ToDouble(ticks) / Convert.ToDouble(total.Ticks);
            progressBar1.Value = 100 - Convert.ToInt32(proportion * 100);
        }
        void button1_Click(object sender, EventArgs e)//开始
        {
            button2.Enabled = true;
            //ShowThread = new Thread(new ThreadStart(ShowThreadMethod));
            //ShowThread.Start();
            button1.Enabled = false;
            numericUpDown1.Enabled = false;
            numericUpDown2.Enabled = false;
            progressBar1.Style = ProgressBarStyle.Continuous;
            hour = Convert.ToInt32(numericUpDown1.Value);
            min = Convert.ToInt32(numericUpDown2.Value);
            total = new DateTime(1, 1, 1, hour, min, 0);
            StartTime = DateTime.Now;
            timer.Start(); 
        }
        void ShowThreadMethod()
        {
            DateTime total, rest = new DateTime(), StartTime;
            this.Invoke(new EventHandler(delegate
            {
                button1.Enabled = false;
                numericUpDown1.Enabled = false;
                numericUpDown2.Enabled = false;
                progressBar1.Style = ProgressBarStyle.Continuous;
            }));
            int hour = Convert.ToInt32(numericUpDown1.Value);
            int min = Convert.ToInt32(numericUpDown2.Value);
            long ticks;
            total = new DateTime(1, 1, 1, hour, min, 0);
            StartTime = DateTime.Now;
            string h = "", m = "", s = "";
            double proportion;
            while (true)
            {
                ticks = (DateTime.Now - StartTime).Ticks;
                try
                {
                    rest = new DateTime(total.Ticks - ticks);
                    if (rest.Ticks == 0) throw new Exception();
                }
                catch
                {
                    MessageBox.Show("时间到！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                    try { this.Invoke(new EventHandler(delegate { Reset(); })); }
                    catch { }//窗体关闭后再点“确定”引发的异常
                }
                h = format(rest.Hour.ToString());
                m = format(rest.Minute.ToString());
                s = format(rest.Second.ToString());
                this.Invoke(new EventHandler(delegate
                {
                    label4.Text = String.Format("{0}:{1}:{2}", h, m, s);
                    proportion = Convert.ToDouble(ticks) / Convert.ToDouble(total.Ticks);
                    progressBar1.Value = 100 - Convert.ToInt32(proportion * 100);
                }));
            }
        }
        void button2_Click(object sender, EventArgs e)//重设
        {
            Reset();
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
        void CountDown_FormClosed(object sender, FormClosedEventArgs e)
        {
            //if (ShowThread != null) ShowThread.Abort();
            timer.Stop();
            if (MainForm != null) MainForm.Show();
        }
    }
}
