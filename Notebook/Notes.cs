using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using MyFrame;
namespace Notebook
{
    public partial class Notes : Form
    {
        MyWinForm FM;
        MyModel M;
        Form LoginForm;
        MyListBox LB;
        MyComboBox CBB;
        MyListViewA LV;
        DataTable table;
        DataRow row;
        bool Operation;//true新增，false修改
        int Count;
        public Notes(Form LoginForm=null)
        {
            this.LoginForm = LoginForm;
            M = MyModel.GetInstance(Global.con);
            InitializeComponent();
            FM = new MyWinForm(this);
            FM.SetDefaultStyle("笔记本");
            if (LoginForm == null) button6.Enabled = false;
            MyTip.Set(label1, "点击清空筛选");
            DefineView();
        }
        void DefineView()
        {
            LB = new MyListBox(listBox1, "select * from NoteGroups order by GroupOrder", "GroupName", "GroupID", Global.con);
            CBB = new MyComboBox(comboBox1, "select * from NoteGroups order by GroupOrder", "GroupName", "GroupID", Global.con);
            try { LB.SelectedIndex = 0; }
            catch { LB.SelectedIndex = -1; }
            //====
            LV = new MyListViewA(listView1);
            LV.SetDB(Global.con, "Notes");
            LV.ColumnWidths = new int[] { 0, 0, 0, 150, 220 };//列宽（默认80）
            LV.RowHeight = 30;
            //LV.AlignCenter = true;
            LV.SetStyle("Cambria", 14, FontStyle.Bold);
            LV.QueryFields = "NoteID,GroupID,Content,TimeOfRevision as 修改时间,Title as 笔记标题";
            LV.OrderBy("TimeOfRevision desc");
            ShowData();
        }
        void ShowData()
        {
            LV.Where("GroupID=" + (LB.SelectedValue == null ? "0" : LB.SelectedValue));
            LV.ShowData(true);
            table = LV.DataTable;
            ToAdd();
            LV.Focus();
        }
        bool Exit = true;
        private void Notes_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (LoginForm != null && Exit) LoginForm.Close();
        }
        private void button6_Click(object sender, EventArgs e)//注销
        {
            Exit = false;
            LoginForm.Show();
            this.Close();
        }
        private void button1_Click(object sender, EventArgs e)//笔记本管理
        {
            new Groups().ShowDialog();
            ReloadGroups();
            try { LB.SelectedIndex = 0; }
            catch { LB.SelectedIndex = -1; }
            ShowData();
        }
        void ReloadGroups()//重载笔记本列表
        {
            LB.Reload();
            CBB.Reload();
        }
        private void button4_Click(object sender, EventArgs e)//准备新增
        {
            ToAdd();
        }
        void ToAdd()//准备新增
        {
            Operation = true;
            button5.Text = "确认新增";
            textBox2.Text = "";
            richTextBox1.Text = "";
            CBB.SelectedIndex = LB.SelectedIndex;
        }
        bool Check()
        {
            MyError.NewRecord();
            if (textBox2.Text.Trim() == "")
                MyError.Set(textBox2, "标题不可为空。");
            else if (textBox2.Text.Trim().Length > 30)
                MyError.Set(textBox2, "标题字数太长。");
            else if (Operation && M.DuplicateTitle(textBox2.Text.Trim()))//新增时，检查是否重名
                MyError.Set(textBox2, "标题重复。");
            else if (!Operation
                && row["笔记标题"].ToString() != textBox2.Text.Trim()
                && M.DuplicateTitle(textBox2.Text.Trim()))//修改时，名称有变，检查是否重名
                MyError.Set(textBox2, "标题重复。");
            else MyError.Clear(textBox2);

            if (richTextBox1.Text.Trim() == "") 
                MyError.Set(button5, "内容不可为空。"); 
            else MyError.Clear(button5);

            if (comboBox1.Text == "") 
                MyError.Set(comboBox1, "请新建笔记本。");
            else MyError.Clear(comboBox1);

            return MyError.CheckAll();
        }
        private void button5_Click(object sender, EventArgs e)//确认保存
        {
            if (!Check()) return;
            if (Operation)
            {
                row = table.NewRow();
                row["NoteID"] = M.GetMaxNoteID();
                row["笔记标题"] = textBox2.Text.Trim();
                row["Content"] = richTextBox1.Text;
                row["GroupID"] = CBB.SelectedValue;
                row["修改时间"] = DateTime.Now;
                table.Rows.Add(row);
            }
            else
            {
                row["笔记标题"] = textBox2.Text.Trim();
                row["Content"] = richTextBox1.Text;
                row["GroupID"] = CBB.SelectedValue;
                row["修改时间"] = DateTime.Now;
            }
            LV.Update();
            ShowData();
        }
        private void button2_Click(object sender, EventArgs e)//删除笔记
        {
            if (LV.CountSelectedRows == 0)
            { MyDialog.Msg("没有选中笔记。", 2); return; }
            if (MyDialog.Ask("你确定要删除选中的 "+
                LV.GetSelectedValues().Length+" 篇笔记？"))
            {
                foreach (string each in LV.SelectedValues)
                    table.Rows.Find(each).Delete();
                LV.Update();
                ShowData();
            }
        }
        private void Notes_DragDrop(object sender, DragEventArgs e)//拖入文件
        {
            MyFile File = new MyFile(FM.DragInPath);
            textBox2.Text = File.Name;
            richTextBox1.Text = File.Content;
        }
        private void button3_Click(object sender, EventArgs e)//另存为
        {
            MyDialog.SaveToTextFile(richTextBox1.Text, textBox2.Text);
        }
        private void textBox1_TextChanged(object sender, EventArgs e)//搜索标题
        {
            if (textBox1.Text.Trim() != "")
            {
                LV.Where("Title like '%" + textBox1.Text + "%'");
                LV.ShowData();
                ToAdd();
            }
            else ShowData();
        }
        private void label1_Click(object sender, EventArgs e)//清空筛选
        {
            textBox1.Text = "";
            textBox1.Focus();
            //textBox1.Select();
        }
        private void 当前笔记总数ToolStripMenuItem_Click(object sender, EventArgs e)//右键菜单，计算总数
        {
            MyDialog.Msg("这本笔记共有 " + LV.CountRows + " 篇记录。", 1);
        }
        private void listBox1_Click(object sender, EventArgs e)////选择笔记本
        {
                ShowData();
        }

        private void listView1_MouseUp(object sender, MouseEventArgs e)//点击载入数据，状态修改（若用点击事件，则容易在点击滑动时未触发事件）
        {
            if (LV.CountSelectedRows == 0) return;
            Operation = false;
            button5.Text = "确认修改";
            row = table.Rows.Find(LV.GetSelectedValues()[0]);
            textBox2.Text = row["笔记标题"].ToString();
            CBB.SelectedValue = row["GroupID"].ToString();
            richTextBox1.Text = row["Content"].ToString();
        }
    }
}
