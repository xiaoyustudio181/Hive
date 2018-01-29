using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace MyFrame
{
    public class MyDialog
    {
        static OpenFileDialog open;
        static SaveFileDialog save;
        static StreamWriter writer;
        static MyDialog()
        {
            open = new OpenFileDialog();
            open.Filter = "所有文件 (*.*)|*.*|" +
                "文本文档 (*.txt)|*.txt|" +
                "Excel文件 (*.xls)|*.xls";
            open.FilterIndex = 1;
            //open.RestoreDirectory = true;

            save = new SaveFileDialog();
            save.Filter = "文本文档 (*.txt)|*.txt|" +
                "Excel文件 (*.xls)|*.xls|" +
                "所有文件 (*.*)|*.*";
            save.FilterIndex = 1;
            //save.RestoreDirectory = true;
        }
        /// <summary>
        /// 弹出消息框。
        /// </summary>
        /// <param name="Info">消息内容。</param>
        /// <param name="Type">消息类型。</param>
        public static void Msg(object Info, int Type = 0)
        {
            switch (Type)
            {
                case 1: MessageBox.Show(Info.ToString(), "消息", MessageBoxButtons.OK, MessageBoxIcon.Information); break;
                case 2: MessageBox.Show(Info.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning); break;
                case 3: MessageBox.Show(Info.ToString(), "意外", MessageBoxButtons.OK, MessageBoxIcon.Error); break;
                default: MessageBox.Show(Info.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.None); break;
            }
        }
        /// <summary>
        /// 弹出确认框。
        /// </summary>
        /// <param name="Question">提问内容。</param>
        /// <returns>选择结果。</returns>
        public static bool Ask(string Question)
        {
            return MessageBox.Show(Question, "选择", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes;
        }
        /// <summary>
        /// 弹出载入对话框。
        /// </summary>
        /// <param name="Extension">无点的扩展名。</param>
        /// <returns>MyFile对象。</returns>
        public static MyFile LoadingDialog(string Extension="")
        {
            if (Extension != "")//自定义扩展名
                open.Filter = Extension + "文件 (*." + Extension + ")|*." + Extension;

            if (open.ShowDialog() == DialogResult.OK)
                return new MyFile(open.FileName);
            else return null;
        }
        #region 保存操作
        /// <summary>
        /// 弹出保存对话框。
        /// </summary>
        /// <param name="Name">默认文件名（无需扩展名）。</param>
        /// <param name="Extension">无点的扩展名。</param>
        /// <returns>目标路径。</returns>
        public static string SavingDialog(string Name="Default", string Extension = "")
        {
            if (Extension != "")//自定义扩展名
                save.Filter = Extension + "文件 (*." + Extension + ")|*." + Extension;

            save.FileName = Name;
            if (save.ShowDialog() == DialogResult.OK) 
                return save.FileName;
            else return "";
        }
        static string FilePath;
        /// <summary>
        /// 保存到文本文件。
        /// </summary>
        /// <param name="Content">内容。</param>
        /// <param name="Name">文件名（无需扩展名）。</param>
        public static void SaveToTextFile(string Content, string Name = "Default")
        {
            FilePath = SavingDialog(Name, "txt");
            if (FilePath == "") return;
            else MyTextFile.Create(FilePath, Content);
        }
        #endregion
    }
}
