using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace MyFrame
{
    public class MyProcess
    {
        static Process process;
        static MyProcess()
        {
            process = new Process();//若不在此处首次实例化，将无法在Run()之前发送命令
        }
        /// <summary>
        /// 运行路径。
        /// </summary>
        /// <param name="Path">目标路径。</param>
        public static void Run(string Path)
        {
            process = new Process();//若不在此处每次实例化，锁定后会出现无法访问文件的异常
            process.StartInfo.FileName = Path;
            try
            {
                process.Start();
            }
            catch (Exception ex)
            {
                MyDialog.Msg(ex.Message + "\n异常路径：" + Path, 3);
            }
        }
        /// <summary>
        /// 发送命令。
        /// </summary>
        /// <param name="Command">命令。</param>
        public static void SendCommand(string Command)
        {
            process.StartInfo.FileName = "cmd";
            process.StartInfo.UseShellExecute = false;//是否使用操作系统shell启动
            process.StartInfo.RedirectStandardInput = true;//接受来自调用程序的输入信息
            process.StartInfo.RedirectStandardOutput = false;//由调用程序获取输出信息
            process.StartInfo.RedirectStandardError = true;//重定向标准错误输出
            process.StartInfo.CreateNoWindow = true;//不显示程序窗口
            process.Start();
            process.StandardInput.WriteLine(Command);
        }
        //=================系统管理
        /// <summary>
        /// 添加或删除程序。
        /// </summary>
        public static void Applications()
        {
            Run("appwiz.cpl");
        }
        /// <summary>
        /// 网络连接。
        /// </summary>
        public static void Network()
        {
            Run("ncpa.cpl");
        }
        /// <summary>
        /// 服务。
        /// </summary>
        public static void Services()
        {
            Run("services.msc");
        }
        /// <summary>
        /// 控制面板。
        /// </summary>
        public static void ControlPanel()
        {
            Run("control");
        }
        /// <summary>
        /// 计算机管理。
        /// </summary>
        public static void Management()
        {
            Run("compmgmt.msc");
        }
        /// <summary>
        /// 设备管理器。
        /// </summary>
        public static void Devices()
        {
            Run("devmgmt.msc");
        }
        /// <summary>
        /// 资源管理器。
        /// </summary>
        public static void Explorer()
        {
            Run("explorer");
        }
        /// <summary>
        /// 我的电脑。
        /// </summary>
        public static void Computer()
        {
            SendCommand("explorer ,");
        }
        /// <summary>
        /// 任务管理器。
        /// </summary>
        public static void Tasks()
        {
            Run("taskmgr");
        }
        /// <summary>
        /// 本地安全策略。
        /// </summary>
        public static void LocalSafty()
        {
            Run("secpol.msc");
        }
        /// <summary>
        /// 本地组策略。
        /// </summary>
        public static void LocalGroup()
        {
            Run("gpedit.msc");
        }
        /// <summary>
        /// 防火墙。
        /// </summary>
        public static void Firewall()
        {
            Run("firewall.cpl");
        }
        /// <summary>
        /// 注册表。
        /// </summary>
        public static void Regedit()
        {
            Run("regedit");
        }
        /// <summary>
        /// 系统版本。
        /// </summary>
        public static void SystemVersion()
        {
            Run("winver");
        }
        /// <summary>
        /// 远程桌面。
        /// </summary>
        public static void RemoteDesk()
        {
            Run("mstsc");
        }
        //=================系统状态
        /// <summary>
        /// 关机。
        /// </summary>
        public static void Shutdown()
        {
            SendCommand("shutdown -p");
        }
        /// <summary>
        /// 重启。
        /// </summary>
        public static void Reboot()
        {
            SendCommand("shutdown -r -t 0");
        }
        /// <summary>
        /// 休眠。
        /// </summary>
        public static void Sleep()
        {
            SendCommand("rundll32.exe powrprof.dll,SetSuspendState 0,1,0");
        }
        /// <summary>
        /// 锁定。
        /// </summary>
        public static void Lock()
        {
            SendCommand("rundll32.exe user32.dll,LockWorkStation");
        }
        /// <summary>
        /// 开启休眠功能。
        /// </summary>
        public static void UseSleep()
        {//首先确认：控制面板->电源选项->更改计划设置->更改高级电源设置->睡眠允许混合睡眠->关闭
            SendCommand("powercfg -hibernate on");
        }
        /// <summary>
        /// 关闭休眠功能。
        /// </summary>
        public static void AbandonSleep()
        {
            SendCommand("powercfg -hibernate off");
        }
        //=================系统应用
        /// <summary>
        /// 命令提示符。
        /// </summary>
        public static void CMD()
        {
            Run("cmd");
        }
        /// <summary>
        /// IE浏览器。
        /// </summary>
        public static void IE()
        {
            Run("iexplore");
        }
        /// <summary>
        /// 媒体播放器。
        /// </summary>
        public static void Player()
        {
            Run("mplayer2");
        }
        /// <summary>
        /// 画图。
        /// </summary>
        public static void MSPaint()
        {
            Run("mspaint");
        }
        /// <summary>
        /// 计算器。
        /// </summary>
        public static void Calculator()
        {
            Run("calc");
        }
        /// <summary>
        /// 记事本。
        /// </summary>
        public static void Notepad()
        {
            Run("notepad");
        }
        /// <summary>
        /// 便笺。
        /// </summary>
        public static void SmallNote()
        {
            Run("StikyNot");
        }
    }
}
