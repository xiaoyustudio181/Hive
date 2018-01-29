using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using MyFrame;
using System.IO;
using System.Threading;
using System.Diagnostics;
using System.Runtime.InteropServices;
namespace FastCmd
{
    public partial class Main : Form
    {
        MyWinForm FM;
        Thread thread;
        Process process;
        StreamReader reader;
        string CmdTxtPath;
        public Main()
        {
            InitializeComponent();
            FM = new MyWinForm(this);
            FM.SetDefaultStyle("快捷命令");
            process = new Process();
            CmdTxtPath = INI.Root + @"\Backup\Commands.txt";
            LoadCommands();
        }
        void LoadCommands()
        {
            listBox1.Items.Clear();
            reader = new StreamReader(CmdTxtPath);
            while (!reader.EndOfStream) 
                listBox1.Items.Add(reader.ReadLine());
            reader.Close();
        }
        private void listBox1_Click(object sender, EventArgs e)//选择命令
        {
            if (listBox1.SelectedItems.Count != 0)
            {
                textBox1.Text = listBox1.SelectedItem.ToString();
                textBox1.Focus();
                textBox1.Select(textBox1.Text.Length, 0);
            }
        }
        string cmd;
        void DoSend()
        {
            cmd = textBox1.Text;
            if (cmd.Contains("ping") && cmd.Contains("-t"))
            { MyDialog.Msg("请去掉 -t 再执行 ping 命令。",2); return; }
            Abort(true);
            thread = new Thread(new ParameterizedThreadStart(SendCmd));
            thread.Start(cmd);
            richTextBox1.Focus();
        }
        private void 执行命令ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DoSend();
        }
        int count;
        void SendCmd(object Cmd)
        {
            process.StartInfo.FileName = "cmd";//若不设置下面五项，则直接启动cmd
            process.StartInfo.UseShellExecute = false;//是否使用操作系统shell启动
            process.StartInfo.RedirectStandardInput = true;//接受来自调用程序的输入信息
            process.StartInfo.RedirectStandardOutput = true;//由调用程序获取输出信息
            process.StartInfo.RedirectStandardError = true;//重定向标准错误输出
            process.StartInfo.CreateNoWindow = true;//不显示程序窗口
            process.Start();
            process.StandardInput.WriteLine(Cmd + "&exit");
            process.StandardInput.AutoFlush = true;
            reader = process.StandardOutput;
            string line = "";
            count = 1;
            while (!reader.EndOfStream)
            {
                if ((line = reader.ReadLine()) != "")//跳过空行
                {
                    if (count <= 2) continue;//跳过开头无用的行
                    this.Invoke(new EventHandler(delegate
                    {
                        richTextBox1.AppendText(line + "\n");
                        richTextBox1.ScrollToCaret();
                    }));
                }
                count++;
            }
            this.Invoke(new EventHandler(delegate { StopLine(); }));
            process.WaitForExit();//等待程序执行完退出进程
            process.Close();
        }
        void StopLine()//终止线
        {
            richTextBox1.AppendText("———————————————————————\n");
        }
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)//关闭窗体
        {
            Abort();
        }
        void Abort(bool AppendLine=false)
        {
            if (thread != null && thread.IsAlive)//终止线程
            {
                process.Kill();
                process.Close();
                //process.Dispose();
                thread.Abort();
                if (AppendLine) StopLine();
            }
        }
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar.Equals('\r')) DoSend();
        }
        private void 清空窗口ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "";
        }
        private void 命令记录ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MyProcess.Run(CmdTxtPath);
        }
        private void 重载记录ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadCommands();
        }
    }
}
