using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using MyFrame;
using System.IO;
namespace Hive
{
    public partial class SelectDB : Form
    {
        MyWinForm FM;
        CentralControl CentralForm;
        string temp;
        public SelectDB(CentralControl CentralForm)
        {
            this.CentralForm = CentralForm;
            InitializeComponent();
            FM = new MyWinForm(this);
            FM.SetDefaultStyle("选择数据库");

            listBox1.Items.Clear();
            foreach (FileInfo file in new DirectoryInfo(INI.Root).GetFiles("*.mdb"))
                listBox1.Items.Add(file.Name);
        }

        private void button1_Click(object sender, EventArgs e)//使用选中的数据库
        {
            if (listBox1.SelectedItems.Count == 0) 
            { MyDialog.Msg("没有选中数据库。", 2); return; }
            temp = listBox1.SelectedItem.ToString();
            temp = temp.Substring(0, temp.IndexOf('.'));
            INI.SetDBName(temp);
            MyDialog.Msg("设置完成！即将重新登录。", 1);
            CentralForm.Logout();
        }
    }
}
