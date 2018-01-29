using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using MyFrame;
namespace Diary
{
    public partial class Records : Form
    {
        Form LoginForm;
        MyWinForm FM;
        MyListBox LB;
        MyGridViewA GV;
        MyModel M;
        DataTable table;
        DataRow row;
        bool Operation;//true新增，false修改
        int Count;
        public Records(Form LoginForm=null)
        {
            this.LoginForm = LoginForm;
            M = MyModel.GetInstance(Global.con);
            InitializeComponent();
            FM = new MyWinForm(this);
            FM.SetDefaultStyle("日记本");
            if (LoginForm == null) button6.Enabled = false;
            DefineView();
        }
        void DefineView()
        {
            LB = new MyListBox(listBox1, "select * from DiaryGroups order by GroupOrder", "GroupName", "GroupID", Global.con);
            GV = new MyGridViewA(dataGridView1);
            try { LB.SelectedIndex = 0; }
            catch { LB.SelectedIndex = -1; }
            GV.SetDB(Global.con, "DiaryRecords");
            GV.ColumnWidths = new int[] { 0, 0, 115, 205 };
            GV.RowHeight = 30;
            GV.SetStyle("Cambria", 14, FontStyle.Bold);
            GV.QueryFields = "RecordID,GroupID,DateOfRecord as 记录日期,Content as 日记内容";
            GV.OrderBy("DateOfRecord desc,RecordID desc");
            GV.PerPage = INI.GetPerPage();
            GV.NowPage = 1;
            GV.KeyColumn = "RecordID";
            ShowData();
        }
        void ShowData()
        {
            GV.Where("GroupID=" + (LB.SelectedValue == null ? "0" : LB.SelectedValue));
            GV.ShowData(true);
            SetNumericMax(GV.TotalPages);
            //numericUpDown1.Maximum = (decimal)GV.TotalPages;//获取最大页码
            table = GV.DataTable;
            ToAdd();
            GV.Focus();
        }
        private void button1_Click(object sender, EventArgs e)//日记本管理
        {
            new Groups().ShowDialog();
            LB.Reload();
            try { LB.SelectedIndex = 0; }
            catch { LB.SelectedIndex = -1; }
            ShowData();
        }
        private void button4_Click(object sender, EventArgs e)//准备新增
        {
            ToAdd();
        }
        void ToAdd()//准备新增
        {
            Operation = true;
            button5.Text = "确认新增";
            richTextBox1.Text = "";
            dateTimePicker1.Value = DateTime.Now;
            GV.ClearSelection();
        }
        private void button6_Click(object sender, EventArgs e)//注销
        {
            Exit = false;
            LoginForm.Show();
            this.Close();
        }
        bool Exit = true;
        private void Records_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (LoginForm != null && Exit) LoginForm.Close();
        }
        bool Check()
        {
            MyError.NewRecord();
            if (listBox1.Items.Count == 0) 
                MyError.Set(button5, "请新建日记本。");
            else if (richTextBox1.Text.Trim() == "") 
                MyError.Set(button5, "日记内容不可为空。"); 
            else MyError.Clear(button5);

            return MyError.CheckAll();
        }
        private void button5_Click(object sender, EventArgs e)//确认保存
        {
            if (!Check()) return;
            if (Operation)
            {
                row = table.NewRow();
                row["RecordID"] = M.GetMaxRecordID();
                row["GroupID"] = LB.SelectedValue;
                row["记录日期"] = dateTimePicker1.Value.Date;
                row["日记内容"] = richTextBox1.Text;
                table.Rows.Add(row);
            }
            else
            {
                row = table.Rows.Find(GV.SelectedValues[0]);
                row["GroupID"] = LB.SelectedValue;
                row["记录日期"] = dateTimePicker1.Value.Date;
                row["日记内容"] = richTextBox1.Text;
            }
            GV.Update();
            GV.NowPage = 1;//方便查看
            ShowData();
            SetNumericValue(1);
        }
        private void listBox1_Click(object sender, EventArgs e)//点击日记本
        {
            GV.NowPage = 1;
            ShowData();
            SetNumericValue(1);
        }
        private void button2_Click(object sender, EventArgs e)//删除日记
        {
            if (GV.CountSelectedRows == 0)
            { MyDialog.Msg("没有选中日记。", 2); return; }
            if (MyDialog.Ask("你确定要删除选中的 " +
                GV.GetSelectedValues().Length + " 篇日记？"))
            {
                foreach (string each in GV.SelectedValues)
                    table.Rows.Find(each).Delete();
                GV.Update();
                GV.NowPage = 1;//防止总页码减少后当前页码大于总页码造成异常
                ShowData();
                SetNumericValue(1);
            }
        }
        private void button3_Click(object sender, EventArgs e)//准备新增
        {
            ToAdd();
        }
        private void 当前日记总数ToolStripMenuItem_Click(object sender, EventArgs e)//计算总数
        {
            MyDialog.Msg("这本日记共有 " + GV.DataAmount + " 篇记录。", 1);
        }
        #region 页码跳转
        private void button7_Click(object sender, EventArgs e)//第一页
        {
            GV.NowPage = 1;
            GV.ShowData();
            SetNumericValue(GV.NowPage);
        }
        private void button10_Click(object sender, EventArgs e)//最后一页
        {
            GV.NowPage = GV.TotalPages;
            GV.ShowData();
            SetNumericValue(GV.NowPage);
        }
        private void button8_Click(object sender, EventArgs e)//上一页
        {
            if (--GV.NowPage <1)//小于最小页码
            {
                GV.NowPage++;
                MyDialog.Msg("已在第一页。", 2);
                return;
            }
            GV.ShowData();
            SetNumericValue(GV.NowPage);
        }
        private void button9_Click(object sender, EventArgs e)//下一页
        {
            if (++GV.NowPage > GV.TotalPages)//超过最大页码
            {
                GV.NowPage--;
                MyDialog.Msg("已在最后一页。", 2);
                return;
            }
            GV.ShowData();
            SetNumericValue(GV.NowPage);
        }
        private void button11_Click(object sender, EventArgs e)//跳转
        {
            GV.NowPage = GetNumericValue();
            GV.ShowData();
        }
        #endregion
        private void 分页设置ToolStripMenuItem_Click(object sender, EventArgs e)//分页设置
        {
            new PageConfig().ShowDialog();
            GV.PerPage = INI.GetPerPage();
            GV.NowPage = 1;
            ShowData();
            SetNumericMax(GV.TotalPages);
            SetNumericValue(1);
        }
        void SetNumericValue(double Value)//设置Up-Down的值
        {
            try { numericUpDown1.Value = (decimal)Value; }
            catch(Exception e) { Console.WriteLine(e.Message); }
        }
        double GetNumericValue()//获取Up-Down的值
        {
            return (double)numericUpDown1.Value;
        }
        void SetNumericMax(double Value)//设置Up-Down最大值
        {
            numericUpDown1.Maximum = (decimal)Value;
        }
        private void Records_DragDrop(object sender, DragEventArgs e)//拖放文件
        {
            MyFile File = new MyFile(FM.DragInPath);
            richTextBox1.Text = File.Content;
        }

        private void dataGridView1_Click(object sender, EventArgs e)//点击数据
        {
            if (GV.CountSelectedRows == 0) return;
            Operation = false;
            button5.Text = "确认修改";
            dateTimePicker1.Value = (DateTime)table.Rows.Find(GV.GetSelectedValues()[0])["记录日期"]; 
            richTextBox1.Text = table.Rows.Find(GV.SelectedValues[0])["日记内容"].ToString();
        }
    }
}
