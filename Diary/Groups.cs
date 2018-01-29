using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using MyFrame;
using System.Data.OleDb;
using System.IO;
using System.Collections;
namespace Diary
{
    public partial class Groups : Form
    {
        MyWinForm FM;
        MyModel M;
        MyListBox LB;
        DataTable table;
        DataRow row;
        bool Operation;
        object SelectedGroupID;
        int MaxID;
        public Groups()
        {
            InitializeComponent();
            FM = new MyWinForm(this);
            FM.SetDefaultStyle("日记本");
            M = MyModel.GetInstance(Global.con);
            LB = new MyListBox(listBox1, "select * from DiaryGroups order by GroupOrder", "GroupName", "GroupID", Global.con, true);
            table = LB.DataTable;
            ToAdd();
        }
        void ToAdd()//准备新增
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
        private void listBox1_Click(object sender, EventArgs e)//点击日记本
        {
            if (LB.SelectedValue == null) return;
            Operation = false;
            button1.Text = "确认修改";
            SelectedGroupID = LB.SelectedValue;
            row = table.Rows.Find(SelectedGroupID);
            textBox1.Text = row["GroupName"].ToString();
            numericUpDown1.Value = int.Parse(row["GroupOrder"].ToString());
        }
        private void button3_Click(object sender, EventArgs e)//删除日记本
        {
            if (LB.SelectedValue == null)
            { MyDialog.Msg("没有选中日记本。", 2); return; }
            if (MyDialog.Ask("操作将删除此日记本的所有内容，确定执行？"))
            {
                M.DeleteRecordsByGroupID(SelectedGroupID);
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
            else if (Operation && M.DuplicateDiaryGroupName(textBox1.Text.Trim()))//新增时，检查是否重名
                MyError.Set(textBox1, "名称重复。");
            else if (!Operation
                && row["GroupName"].ToString() != textBox1.Text.Trim()
                && M.DuplicateDiaryGroupName(textBox1.Text.Trim()))//修改时，名称有变，检查是否重名
                MyError.Set(textBox1, "名称重复。");
            else MyError.Clear(textBox1);

            return MyError.CheckAll();
        }
        private void button1_Click(object sender, EventArgs e)//确认保存
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
        private void button4_Click(object sender, EventArgs e)//导出日记
        {
            if (LB.SelectedValue == null)
            { MyDialog.Msg("没有选中日记本。", 2); return; }
            string GroupName = M.GetDiaryGroupNameByID(SelectedGroupID);
            DataTable table0 = M.GetRecordsByGroupID(SelectedGroupID);
            string Path = INI.Root + @"\Records\" + GroupName + ".txt";
            if (File.Exists(Path))
            { MyDialog.Msg("Records目录下存在同名文件。", 2); return; }
            foreach (DataRow row in table0.Rows)
                MyTextFile.Append(Path, "【" + ((DateTime)row["DateOfRecord"]).ToShortDateString() + "】\n" + row["Content"]);
            MyDialog.Msg("已导出到Records目录。", 1);
        }
    }
}
