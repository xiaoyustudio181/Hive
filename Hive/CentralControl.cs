using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using MyFrame;
using Reader;
using TouchTypist;
using ElectronicWatch;
using Labels;
using Notebook;
using Diary;
using Pack;
using System.IO;
namespace Hive
{
    public partial class CentralControl : Form
    {
        Form LoginForm;
        string About = "软件名称：WinHelper\n开发人：小宇\n开发语言：Visual C#\n开发工具：Visual Studio 2013\n框架版本：.NET Framework 2.0\n邮箱：studio181@sina.com";
        public CentralControl(Form LoginForm)
        {
            this.LoginForm = LoginForm;
            InitializeComponent();
            if (Global.con == null) TouristMode();//游客模式
            else OpenWindow(ref LabelsForm, new Labels.Labels());
            clearGarbage();
        }
        void TouristMode()//游客模式
        {
            toolStripStatusLabel1.Text = "游客模式";
            数据应用ToolStripMenuItem.Enabled = false;
            toolStripButton12.Enabled = false;
            toolStripButton13.Enabled = false;
            toolStripButton14.Enabled = false;
            toolStripButton17.Enabled = false;
        }
        #region 注销、退出
        bool Exit = true;
        private void CentralControl_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (Exit) LoginForm.Close();
            if (Global.con != null) Global.con.Dispose();
            Global.con = null;
        }
        public void Logout()
        {
            Exit = false;
            this.Close();
            LoginForm.Show();
        }
        private void 注销ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Logout();
        }
        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            Logout();
        }
        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void 注销ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Logout();
        }
        private void 退出ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion
        #region 其他操作
        private void 根目录ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            INI.OpenRoot();
        }
        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            INI.OpenRoot();
        }
        private void 配置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            INI.OpenConfig();
        }
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            INI.OpenConfig();
        }
        private void 关于ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MyDialog.Msg(About, 1);
        }
        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            MyDialog.Msg(About, 1);
        }
        #endregion
        #region 系统状态
        private void 关机ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            MyProcess.Shutdown();
        }
        private void 重启ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            MyProcess.Reboot();
        }
        private void 休眠ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            MyProcess.Sleep();
        }
        private void 锁定ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            MyProcess.Lock();
        }
        private void 关机ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MyProcess.Shutdown();
        }
        private void 重启ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MyProcess.Reboot();
        }
        private void 休眠ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MyProcess.Sleep();
        }
        private void 锁定ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MyProcess.Lock();
        }
        #endregion
        #region 窗口尺寸
        private void toolStripSplitButton1_ButtonClick(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }
        private void 最小化ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }
        private void 最大化ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Maximized;
        }
        private void 恢复ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Normal;
        }
        #endregion
        #region 系统管理
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            MyProcess.Computer();
        }

        private void 我的电脑ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MyProcess.Computer();
        }

        private void 计算机管理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MyProcess.Management();
        }

        private void 控制面板ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MyProcess.ControlPanel();
        }

        private void 添加或删除程序ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MyProcess.Applications();
        }

        private void 注册表ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MyProcess.Regedit();
        }

        private void 防火墙ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MyProcess.Firewall();
        }

        private void 网络连接ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MyProcess.Network();
        }

        private void 服务ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MyProcess.Services();
        }

        private void 任务管理器ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MyProcess.Tasks();
        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            MyProcess.Tasks();
        }

        private void 本地安全策略ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MyProcess.LocalSafty();
        }

        private void 本地组策略ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MyProcess.LocalGroup();
        }

        private void 系统信息ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MyProcess.SystemVersion();
        }
        #endregion
        #region 系统应用
        private void 记事本ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MyProcess.Notepad();
        }

        private void 画图ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MyProcess.MSPaint();
        }

        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            MyProcess.Notepad();
        }

        private void toolStripButton9_Click(object sender, EventArgs e)
        {
            MyProcess.MSPaint();
        }

        private void toolStripButton10_Click(object sender, EventArgs e)
        {
            MyProcess.Calculator();
        }

        private void 计算器ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MyProcess.Calculator();
        }

        private void 媒体播放器ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MyProcess.Player();
        }

        private void iE浏览器ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MyProcess.IE();
        }

        private void 远程控制ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MyProcess.RemoteDesk();
        }

        private void 命令提示符ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MyProcess.CMD();
        }

        private void toolStripButton11_Click(object sender, EventArgs e)
        {
            MyProcess.CMD();
        }

        #endregion
        #region 扩展应用
        Form ReaderForm, TypistForm, CmdForm,
            NowForm, AlarmForm, CountForm, SecondsForm,
            LabelsForm, NotesForm, DiariesForm, AnniversariesForm;
        private void toolStripButton16_Click(object sender, EventArgs e)//阅读器
        {
            OpenWindow(ref ReaderForm, new Reader.Reader());
        }
        private void 阅读器ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenWindow(ref ReaderForm, new Reader.Reader());
        }
        private void toolStripButton18_Click(object sender, EventArgs e)
        {
            OpenWindow(ref CmdForm, new FastCmd.Main());
        }
        private void 快速命令ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenWindow(ref CmdForm, new FastCmd.Main());
        }
        private void toolStripButton15_Click(object sender, EventArgs e)
        {
            OpenWindow(ref TypistForm, new Game());
        }
        private void 盲打机ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenWindow(ref TypistForm, new Game());
        }
        #region 电子表
        private void 时钟ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            OpenWindow(ref NowForm, new Now(null));
        }
        private void 时钟ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenWindow(ref NowForm, new Now(null));
        }
        private void 闹钟ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenWindow(ref AlarmForm, new Alarm(null));
        }
        private void 闹钟ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            OpenWindow(ref AlarmForm, new Alarm(null));
        }
        private void 秒表ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            OpenWindow(ref SecondsForm, new Seconds(null));
        }
        private void 秒表ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenWindow(ref SecondsForm, new Seconds(null));
        }
        private void 定时ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            OpenWindow(ref CountForm, new CountDown(null));
        }
        private void 定时ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenWindow(ref CountForm, new CountDown(null));
        }
        #endregion
        private void toolStripButton12_Click(object sender, EventArgs e)//标签簿
        {
            OpenWindow(ref LabelsForm, new Labels.Labels());
        }
        private void 标签簿ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenWindow(ref LabelsForm, new Labels.Labels());
        }
        private void toolStripButton13_Click(object sender, EventArgs e)//笔记本
        {
            OpenWindow(ref NotesForm, new Notes());
        }
        private void 笔记本ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenWindow(ref NotesForm, new Notes());
        }
        private void toolStripButton14_Click(object sender, EventArgs e)//日记本
        {
            OpenWindow(ref DiariesForm, new Diary.Records());
        }
        private void 日记本ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenWindow(ref DiariesForm, new Diary.Records());
        }
        private void 纪念簿ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenWindow(ref AnniversariesForm, new Anniversaries.Records());
        }
        private void toolStripButton17_Click(object sender, EventArgs e)
        {
            OpenWindow(ref AnniversariesForm, new Anniversaries.Records());
        }
        #endregion
        #region 右键菜单
        void MinimizeAll()//全部最小化
        {
            foreach (Form each in MdiChildren)
                if (each != null)
                    if (!each.IsDisposed) each.WindowState = FormWindowState.Minimized;//打开过，且未关闭
        }
        void Cascade()//层叠排列
        {
            foreach (Form each in MdiChildren)
                if (each != null)
                    if (!each.IsDisposed) each.WindowState = FormWindowState.Normal;//打开过，且未关闭
            LayoutMdi(MdiLayout.Cascade);
        }
        void CloseAll()//全部关闭
        {
            foreach (Form each in MdiChildren)
                if (each != null)
                    if (!each.IsDisposed) each.Close();//打开过，且未关闭
        }
        private void 层叠排列ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Cascade();
        }
        private void 全部最小化ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MinimizeAll();
        }
        private void 全部关闭ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CloseAll();
        }
        #endregion
        #region 辅助方法
        void OpenWindow(ref Form FormX, Form NewForm)//打开子窗体
        {
            if (FormX != null)//窗体打开过
            {
                if (!FormX.IsDisposed)//窗体未关闭
                {
                    FormX.Focus();//聚焦
                    return;
                }//else 窗体已关闭
            }//else 窗体未打开过
            FormX = NewForm;
            FormX.MdiParent = this;
            if (FormX.MainMenuStrip != null)
                FormX.MainMenuStrip.AllowMerge = false;//不允许子窗体的MenuStrip与父窗体的合并
            FormX.Show();
        }
        private void menuStrip1_ItemAdded(object sender, ToolStripItemEventArgs e)//使子窗体最大化后，其图标不会显示在父窗体的MenuStrip
        {
            if (e.Item.Text.Length == 0) e.Item.Visible = false;
        }
        bool go1 = true;//开关
        private void CentralControl_Resize(object sender, EventArgs e)//窗体尺寸改变时（未使用）
        {
            //if (!go1 || WindowState == FormWindowState.Minimized) return;
            //go1 = false;//防止下面代码再次触发此方法

            //Width = 1300;//最小化后尺寸变小，给尺寸属性赋值无效，恢复尺寸后会得出错误结果，因此设置最小化不执行下面运算
            //Height = 800;
            //Location = new Point(SystemInformation.WorkingArea.Width / 2 - Width / 2, SystemInformation.WorkingArea.Height / 2 - Height / 2);
            ////若用户手动改变了窗体位置，此改变坐标的代码无效

            //go1 = true;
        }
        #endregion
        Form MaintainForm;
        private void 软件维护ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenWindow(ref MaintainForm, new Maintenance(this));
        }
        private void 使用手册ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MyProcess.Run(INI.Root+@"\Backup\Manual.txt");
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            toolStripStatusLabel2.Text = MyTime.GetNowDateTimeInChinese();
        }
        void clearGarbage()//清理根目录下的垃圾
        {
            foreach (FileInfo file in new DirectoryInfo(INI.Root).GetFiles())
            {
                if (file.Name.StartsWith("hc_")||
                    file.Name == "debug.log")
                {
                    file.Delete();
                }
            }
        }
    }
}
