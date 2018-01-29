using MyFrame;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Labels
{
    public partial class Locations : Form
    {
        MyWinForm FM;
        MyModel M;
        MyListBox LB;
        DataTable table;
        DataRow row;
        bool Operation = true;//新增true/修改false
        object SelectedLocationID;
        int MaxID;
        public Locations()
        {
            InitializeComponent();
            M = MyModel.GetInstance(Global.con);
            FM = new MyWinForm(this);
            FM.SetDefaultStyle("当前地点：" + M.GetLocationNameByID(INI.GetLocationID()));
            LB = new MyListBox(listBox1, "select * from LabelLocations order by LocationOrder", "LocationName", "LocationID", Global.con, true);
            table = LB.DataTable;
            ToAdd();
        }
        private void button2_Click(object sender, EventArgs e)//准备新增地点
        {
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
        private void listBox1_Click(object sender, EventArgs e)//点击地点列表
        {
            if (LB.SelectedValue == null) return;
            Operation = false;
            button1.Text = "确认修改";
            SelectedLocationID = LB.SelectedValue;
            row = table.Rows.Find(SelectedLocationID);
            textBox1.Text = row["LocationName"].ToString();
            numericUpDown1.Value = int.Parse(row["LocationOrder"].ToString());
        }
        bool Check()
        {
            MyError.NewRecord();
            if (textBox1.Text.Trim() == "")
                MyError.Set(textBox1, "名称不可为空。");
            else if (textBox1.Text.Trim().Contains("未知"))
                MyError.Set(textBox1, "名称不可包含“未知”。");
            else if (textBox1.Text.Trim().Length > 30)
                MyError.Set(textBox1, "名称字数太长。");
            else if (Operation && M.DuplicateLocationName(textBox1.Text.Trim()))//新增时，检查是否重名
                MyError.Set(textBox1, "名称重复。");
            else if (!Operation
                && row["LocationName"].ToString() != textBox1.Text.Trim()
                && M.DuplicateLocationName(textBox1.Text.Trim()))//修改时，名称有变，检查是否重名
                MyError.Set(textBox1, "名称重复。");
            else MyError.Clear(textBox1);

            return MyError.CheckAll();
        }
        private void button1_Click(object sender, EventArgs e)//确认
        {
            if (!Check()) return;
            if (Operation)
            {
                row = table.NewRow();
                //用如下这种办法新增主键值时，表主键不要设自动增长，否则新增后立马删除会报错。因为如果是自动增长，存入的表的主键值实际是历史最大值，而DataTable存入的是本次最大值，这样就会发生找不到对应数据的情况。
                try { MaxID = (int)table.Select("", "LocationID DESC")[0]["LocationID"] + 1; }//只有地点才能用这种方式取最大ID，因为它不会分组
                catch { MaxID = 1; }
                row["LocationID"] = MaxID;
                row["LocationName"] = textBox1.Text.Trim();
                row["LocationOrder"] = numericUpDown1.Value;
                table.Rows.Add(row);
            }
            else
            {
                row["LocationName"] = textBox1.Text.Trim();
                row["LocationOrder"] = numericUpDown1.Value;
            }
            LB.Update();
            LB.Reload();
            ToAdd();
        }
        private void button3_Click(object sender, EventArgs e)//删除选中地点
        {
            if (LB.SelectedValue == null)
            { MyDialog.Msg("没有选中地点。", 2); return; }
            if (MyDialog.Ask("操作将删除此地点下的所有标签，确定执行？"))
            {
                M.DeleteAllByLocationID(SelectedLocationID);
                table.Rows.Find(SelectedLocationID).Delete();
                LB.Update();
                ToAdd();
            }
        }
        private void button4_Click(object sender, EventArgs e)//使用选中地点
        {
            if (LB.SelectedValue == null)
            { MyDialog.Msg("没有选中地点。", 2); return; }
            INI.SetLocationID(LB.SelectedValue);
            MyDialog.Msg("设置完成！", 1);
            this.Text = "当前地点：" + ((DataRowView)LB.SelectedItem)["LocationName"];
        }
    }
}
