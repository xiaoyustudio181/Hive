using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using MyFrame;
using System.Collections;
using System.IO;
namespace Notebook
{
    public partial class Groups : Form
    {
        MyWinForm FM;
        MyListBox LB;
        DataTable table;
        DataRow row;
        MyModel M;
        bool Operation;
        object SelectedGroupID;
        int MaxID;
        public Groups()
        {
            InitializeComponent();
            FM = new MyWinForm(this);
            FM.SetDefaultStyle("笔记本管理");
            M = MyModel.GetInstance(Global.con);
            LB = new MyListBox(listBox1, "select * from NoteGroups order by GroupOrder", "GroupName", "GroupID", Global.con, true);
            table = LB.DataTable;
            ToAdd();
        }
        void ToAdd()//准备新增分组
        {
            Operation = true;
            button1.Text = "确认新增";
            textBox1.Text = "";
            numericUpDown1.Value = 0;
            LB.ClearSelected();
            LB.Focus();
        }
        private void button2_Click(object sender, EventArgs e)//准备新增
        {
            ToAdd();
        }
        private void listBox1_Click(object sender, EventArgs e)//点击ListBox，准备更新
        {
            if (LB.SelectedValue == null) return;
            Operation = false;
            button1.Text = "确认修改";
            SelectedGroupID = LB.SelectedValue;
            row = table.Rows.Find(SelectedGroupID);
            textBox1.Text = row["GroupName"].ToString();
            numericUpDown1.Value = int.Parse(row["GroupOrder"].ToString());
        }
        private void button3_Click(object sender, EventArgs e)//删除分组
        {
            if (LB.SelectedValue == null)
            { MyDialog.Msg("没有选中笔记本。", 2); return; }
            if (MyDialog.Ask("操作将删除此笔记本的所有内容，确定执行？"))
            {
                M.DeleteNotesByGroupID(SelectedGroupID);
                table.Rows.Find(SelectedGroupID).Delete();
                LB.Update();
                ToAdd();
            }
        }
        bool Check()
        {
            MyError.NewRecord();
            if (textBox1.Text.Trim() == "")
                MyError.Set(textBox1, "名称不可为空。");
            else if (textBox1.Text.Trim().Length > 30)
                MyError.Set(textBox1, "名称字数太长。");
            else if (Operation && M.DuplicateNoteGroupName(textBox1.Text.Trim()))//新增时，检查是否重名
                MyError.Set(textBox1, "名称重复。");
            else if (!Operation
                && row["GroupName"].ToString() != textBox1.Text.Trim()
                && M.DuplicateNoteGroupName(textBox1.Text.Trim()))//修改时，名称有变，检查是否重名
                MyError.Set(textBox1, "名称重复。");
            else MyError.Clear(textBox1);

            return MyError.CheckAll();
        }
        private void button1_Click(object sender, EventArgs e)//保存
        {
            if (!Check()) return;
            if (Operation)
            {
                row = table.NewRow();
                try { MaxID = (int)table.Select("", "GroupID DESC")[0]["GroupID"] + 1; }
                catch { MaxID = 1; }
                row["GroupID"] = MaxID;
                row["GroupName"] = textBox1.Text.Trim();
                row["GroupOrder"] = numericUpDown1.Value;
                table.Rows.Add(row);
            }
            else
            {
                row["GroupName"] = textBox1.Text.Trim();
                row["GroupOrder"] = numericUpDown1.Value;
            }
            LB.Update();
            LB.Reload();
            ToAdd();
        }
        private void button4_Click(object sender, EventArgs e)//导出
        {
            if (LB.SelectedValue == null)
            { MyDialog.Msg("没有选中笔记本。", 2); return; }
            string GroupName = M.GetNoteGroupNameByID(SelectedGroupID);
            DataTable table0 = M.GetNotesByGroupID(SelectedGroupID);
            string DirPath = INI.Root + @"\Records\" + GroupName;
            if (Directory.Exists(DirPath))
            { MyDialog.Msg("Records目录下存在同名目录。", 2); return; }
            Directory.CreateDirectory(DirPath);
            foreach (DataRow row in table0.Rows)
                MyTextFile.Create(DirPath + "\\" + row["Title"] + ".txt", row["Content"].ToString());
            MyDialog.Msg("已导出到Records目录。", 1);
        }

    }
}
