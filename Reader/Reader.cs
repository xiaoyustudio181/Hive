using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using MyFrame;
namespace Reader
{
    public partial class Reader : Form
    {
        MyStyleDialog style;
        int currentMode = INI.GetFinalMode();//获取默认模式的编号
        public Reader()
        {
            InitializeComponent();
            style = new MyStyleDialog(richTextBox1);
            defaultWindowState();
            defaultMode();
        }
        private void defaultWindowState()//读取上次的窗体样式，并应用
        {
            switch (INI.GetFinalWindowState().ToString())
            {
                case "Normal":
                    this.WindowState = FormWindowState.Normal;
                    int[] size = INI.GetFinalWindowSize();
                    this.Width = size[0];
                    this.Height = size[1];
                    break;
                case "Maximized":
                    this.WindowState = FormWindowState.Maximized;
                    break;
                default: break;//Minimized不会出现
            }
        }
        private void defaultMode()//读取菜单项配置，并应用上次关闭前的模式样式
        {
            string[] ModeNames = INI.GetModeNames();
            mode1ToolStripMenuItem.Text = ModeNames[0];
            mode2ToolStripMenuItem.Text = ModeNames[1];
            mode3ToolStripMenuItem.Text = ModeNames[2];
            mode4ToolStripMenuItem.Text = ModeNames[3];
            mode5ToolStripMenuItem.Text = ModeNames[4];
            apply();
        }
        private void 字体FToolStripMenuItem_Click(object sender, EventArgs e)//设置字体并保存到配置
        {
            style.ShowFonts();
            string bold = style.Font.Bold.ToString();
            string fontfamily = style.Font.FontFamily.Name.ToString();
            string fontsize = style.Font.Size.ToString();
            INI.SetModeFont(currentMode, new string[] { bold, fontfamily, fontsize });
        }
        private void 颜色CToolStripMenuItem_Click(object sender, EventArgs e)//设置颜色并保存到配置
        {
            style.ShowColors();
            INI.SetModeColor(currentMode, style.Color.ToArgb().ToString());
        }
        private void 帮助HToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MyDialog.Msg("每种模式的字体样式会自动保存。\n模式名称可以在配置文件中修改。\n将文本文档拖到窗体顶部可载入。", 1);
        }
        private void Reader_FormClosed(object sender, FormClosedEventArgs e)//保存关闭前的模式与窗体样式
        {
            INI.SetFinalMode(currentMode.ToString());
            string state=this.WindowState.ToString();
            if (state != "Minimized")
            {
                INI.SetFinalWindowState(state);
                INI.SetFinalWindowSize(this.Width, this.Height);
            }
        }
        #region 应用模式样式
        void apply()
        {
            applyModePattern();
            applyModeFont();
            applyModeColor();
        }
        private void applyModePattern()//根据选项改变菜单项颜色
        {
            switch (currentMode)
            {
                case 1:
                    mode1ToolStripMenuItem.BackColor = Color.LightGray;
                    mode2ToolStripMenuItem.BackColor = Color.White;
                    mode3ToolStripMenuItem.BackColor = Color.White;
                    mode4ToolStripMenuItem.BackColor = Color.White;
                    mode5ToolStripMenuItem.BackColor = Color.White;
                    break;
                case 2:
                    mode2ToolStripMenuItem.BackColor = Color.LightGray;
                    mode1ToolStripMenuItem.BackColor = Color.White;
                    mode3ToolStripMenuItem.BackColor = Color.White;
                    mode4ToolStripMenuItem.BackColor = Color.White;
                    mode5ToolStripMenuItem.BackColor = Color.White;
                    break;
                case 3:
                    mode3ToolStripMenuItem.BackColor = Color.LightGray;
                    mode1ToolStripMenuItem.BackColor = Color.White;
                    mode2ToolStripMenuItem.BackColor = Color.White;
                    mode4ToolStripMenuItem.BackColor = Color.White;
                    mode5ToolStripMenuItem.BackColor = Color.White;
                    break;
                case 4:
                    mode4ToolStripMenuItem.BackColor = Color.LightGray;
                    mode1ToolStripMenuItem.BackColor = Color.White;
                    mode2ToolStripMenuItem.BackColor = Color.White;
                    mode3ToolStripMenuItem.BackColor = Color.White;
                    mode5ToolStripMenuItem.BackColor = Color.White;
                    break;
                case 5:
                    mode5ToolStripMenuItem.BackColor = Color.LightGray;
                    mode1ToolStripMenuItem.BackColor = Color.White;
                    mode2ToolStripMenuItem.BackColor = Color.White;
                    mode3ToolStripMenuItem.BackColor = Color.White;
                    mode4ToolStripMenuItem.BackColor = Color.White;
                    break;
                default:
                    MyDialog.Msg("读取配置失败。\n模式代码：" + currentMode + "。\n请检查配置文件是否存在于程序根目录，\n或当中是否缺少相应的键或值。", 3);
                    Environment.Exit(0);
                    break;
            }
        }
        private void applyModeFont()//读取配置并应用：字体
        {
            string[] values = INI.GetModeFont(currentMode);
            richTextBox1.Font = new Font(values[0], (float)(Convert.ToDouble(values[1])), values[2] == "True" ? FontStyle.Bold : FontStyle.Regular);
        }
        private void applyModeColor()//读取配置并应用：颜色
        {
            richTextBox1.ForeColor = Color.FromArgb(int.Parse(INI.GetModeColor(currentMode)));
        }
        #endregion
        #region 点击菜单项：获取选中模式样式并应用
        private void mode1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            currentMode = 1;
            apply();
        }
        private void mode2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            currentMode = 2;
            apply();
        }
        private void mode3ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            currentMode = 3;
            apply();
        }
        private void mode4ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            currentMode = 4;
            apply();
        }
        private void mode5ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            currentMode = 5;
            apply();
        }
        #endregion
        private void 清空LToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "";
        }
        private void 保存SToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MyDialog.SaveToTextFile(richTextBox1.Text, MyTime.GetNowTimeCode(3));
        }
        string DragInPath;
        private void Reader_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Link;
            else e.Effect = DragDropEffects.None;
            DragInPath = ((System.Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();
        }
        private void Reader_DragDrop(object sender, DragEventArgs e)
        {
            MyFile file = new MyFile(DragInPath);
            richTextBox1.Text = file.Content;
        }
        #region 窗体尺寸
        private void 小ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Normal;
            Width = 800;
            Height = 500;
        }
        private void 中ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Normal;
            Width = 1100;
            Height = 700;
        }
        private void 大ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Normal;
            Width = 1300;
            Height = 800;
        }
        #endregion

    }
}
