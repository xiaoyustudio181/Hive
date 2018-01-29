using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using MyFrame;
namespace Anniversaries
{
    public partial class Records : Form
    {
        Form LoginForm;
        MyWinForm FM;
        MyListBox LB;
        MyModel M;
        DataTable table;
        DataRow row;
        bool Operation;
        int MaxID;
        public Records(Form LoginForm=null)
        {
            this.LoginForm = LoginForm;
            InitializeComponent();
            FM = new MyWinForm(this);
            FM.SetDefaultStyle("纪念簿");
            if (LoginForm == null) button5.Enabled = false;
            M = MyModel.GetInstance(Global.con);
            LB = new MyListBox(listBox2, "select * from Anniversaries order by MonthN,DayN,ID", "Title", "ID", Global.con, true);
            table = LB.DataTable;
            ToAdd();
            if (M.CountAnniRecords() != 0)//有数据
            {
                row = M.GetAnnInfoByID(M.GetNearestDateID()).Rows[0];
                LB.SelectedValue = row["ID"];
                label2.Text = "最近的纪念日：";
                label4.Text = row["Title"] + "，" + row["MonthN"] + "月" + row["DayN"] + "日。";
            }
        }
        void ToAdd()
        {
            Operation = true;
            button2.Text = "确认新增";
            textBox1.Text = "";
            dateTimePicker1.Value = DateTime.Now;
            checkBox1.Checked = false;
            LB.ClearSelected();
            label2.Text = "";
            label4.Text = "";
            LB.Focus();
        }
        DateTime diff;
        private void button1_Click(object sender, EventArgs e)//计算距离今日年数
        {
            diff=new DateTime((DateTime.Now-dateTimePicker1.Value).Ticks);
            MyDialog.Msg("纪念日距现在有 " + (diff.Year -1) + " 年。", 1);//Ticks=0时，DateTime的年月日都是1
        }
        bool Check()
        {
            MyError.NewRecord();
            if (textBox1.Text.Trim() == "")
                MyError.Set(textBox1, "标题不可为空。");
            else if (Operation && M.DuplicateAnniTitle(textBox1.Text.Trim()))//新增时，检查是否重名
                MyError.Set(textBox1, "标题重复。");
            else if (!Operation
                && row["Title"].ToString() != textBox1.Text.Trim()
                && M.DuplicateAnniTitle(textBox1.Text.Trim()))//修改时，名称有变，检查是否重名
                MyError.Set(textBox1, "标题重复。");
            else MyError.Clear(textBox1);

            return MyError.CheckAll();
        }
        private void button2_Click(object sender, EventArgs e)//确认保存
        {
            if (!Check()) return;
            if (Operation)
            {
                row = table.NewRow();
                try { MaxID = (int)table.Select("", "ID DESC")[0]["ID"] + 1; }
                catch { MaxID = 1; }
                row["ID"] = MaxID;
                row["Title"] = textBox1.Text.Trim();
                row["YearN"] = dateTimePicker1.Value.Year;
                row["MonthN"] = dateTimePicker1.Value.Month;
                row["DayN"] = dateTimePicker1.Value.Day;
                row["ValidYear"] = checkBox1.Checked;
                table.Rows.Add(row);
            }
            else
            {
                row["Title"] = textBox1.Text.Trim();
                row["YearN"] = dateTimePicker1.Value.Year;
                row["MonthN"] = dateTimePicker1.Value.Month;
                row["DayN"] = dateTimePicker1.Value.Day;
                row["ValidYear"] = checkBox1.Checked;
            }
            LB.Update();
            LB.Reload();
            ToAdd();
        }
        private void button3_Click(object sender, EventArgs e)//准备新增
        {
            ToAdd();
        }
        private void button4_Click(object sender, EventArgs e)//删除
        {
            if (LB.SelectedValue == null)
            { MyDialog.Msg("没有选中记录。", 2); return; }
            if (MyDialog.Ask("确定要删除这条记录？"))
            {
                table.Rows.Find(LB.SelectedValue).Delete();
                LB.Update();
                ToAdd();
            }
        }
        bool Exit = true;
        private void button5_Click(object sender, EventArgs e)//注销登录
        {
            Exit = false;
            LoginForm.Show();
            this.Close();
        }
        private void Records_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (LoginForm != null && Exit) LoginForm.Close();
        }
        private void listBox2_Click(object sender, EventArgs e)//点击数据
        {
            Operation = false;
            button2.Text = "确认修改";
            row = table.Rows.Find(LB.SelectedValue);
            textBox1.Text = row["Title"].ToString();
            dateTimePicker1.Value = new DateTime(Convert.ToInt32(row["YearN"]), Convert.ToInt32(row["MonthN"]), Convert.ToInt32(row["DayN"]));
            checkBox1.Checked = (bool)row["ValidYear"];
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            button1.Enabled = checkBox1.Checked;
        }
    }
}
