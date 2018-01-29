using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace MyFrame
{
    public class MyTip
    {
        static ToolTip tip;
        static MyTip()
        {
            //tip = new ToolTip();//在此处实例化后使用容易失效（经过窗体反复打开关闭后，此对象可能被释放）
            //tip.ToolTipTitle = "提示";
            //tip.ToolTipIcon = ToolTipIcon.Info;
            //tip.IsBalloon = true;
        }
        public static void Set(Control control, string Info)
        {
            tip = new ToolTip();
            tip.IsBalloon = true;
            tip.SetToolTip(control, Info);
        }
    }
}
