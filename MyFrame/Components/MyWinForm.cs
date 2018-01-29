using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace MyFrame
{
    public class MyWinForm
    {
        Form FM;
        public MyWinForm(Form FM)
        {
            this.FM = FM;
        }
        public void SetDefaultStyle(string Title)
        {
            FM.Text = Title;
            FM.MaximizeBox = false;
            FM.MinimizeBox = true;
            FM.StartPosition = FormStartPosition.CenterScreen;
            FM.FormBorderStyle = FormBorderStyle.FixedSingle;
            FM.WindowState = FormWindowState.Normal;
            FM.AllowDrop = true;
            //FM.Font = new Font("微软雅黑", 10.5f, FontStyle.Regular);
            //窗体的大小与字体最好手动设置
            FM.DragEnter += new System.Windows.Forms.DragEventHandler(FM_DragEnter);//计算拖入的文件路径
        }
        public string DragInPath { set; get; }//拖入的文件路径
        void FM_DragEnter(object sender, DragEventArgs e)//获取拖入的文件路径
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Link;
            else e.Effect = DragDropEffects.None;
            DragInPath = ((System.Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();
        }
        public void Minimize()//最小化
        {
            FM.WindowState = FormWindowState.Minimized;
        }
        public void Maximize()//最大化
        {
            FM.WindowState = FormWindowState.Maximized;
        }
        public void RecoverSize()//正常化
        {
            FM.WindowState = FormWindowState.Normal;
        }
    }
}
