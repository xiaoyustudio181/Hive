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
    public partial class Groups : Form
    {
        MyWinForm FM;
        MyListBox LB;
        DataTable table;
        DataRow row;
        MyModel M;
        bool Operation;
        string LocationID = INI.GetLocationID();
        object result, SelectedGroupID;
        public Groups()
        {
            InitializeComponent();
            M = MyModel.GetInstance(Global.con);
            FM = new MyWinForm(this);
            LB = new MyListBox(listBox1, "select * from LabelGroups where LocationID=" + LocationID + " order by GroupOrder", "GroupName", "GroupID", Global.con, true);
            table = LB.DataTable;
            FM.SetDefaultStyle("标签分组 >> 地点：" + M.GetLocationNameByID(LocationID));
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
        private void button2_Click(object sender, EventArgs e)//准备新增分组
        {
            ToAdd();
        }
        private void listBox1_Click(object sender, EventArgs e)//点击分组列表
        {
            if (LB.SelectedValue == null) return;
            Operation = false;
            button1.Text = "确认修改";
            SelectedGroupID = LB.SelectedValue;
            row = table.Rows.Find(SelectedGroupID);
            textBox1.Text = row["GroupName"].ToString();
            numericUpDown1.Value = int.Parse(row["GroupOrder"].ToString());
        }
        private void button3_Click(object sender, EventArgs e)//删除选中分组
        {
            if (LB.SelectedValue == null)
            { MyDialog.Msg("没有选中分组。", 2); return; }
            if (MyDialog.Ask("操作将删除此分组下的所有标签，确定执行？"))
            {
                M.DeleteLabelsByGroupID(SelectedGroupID);
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
            else if (textBox1.Text.Trim().Length>30)
                MyError.Set(textBox1, "名称字数太长。");
            else if (Operation && M.DuplicateLabelGroupName(textBox1.Text.Trim(), LocationID))//新增时，检查是否重名
                MyError.Set(textBox1, "名称重复。");
            else if (!Operation
                && row["GroupName"].ToString() != textBox1.Text.Trim()
                && M.DuplicateLabelGroupName(textBox1.Text.Trim(), LocationID))//修改时，名称有变，检查是否重名
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
                row["GroupID"] = M.GetMaxLabelGroupID();
                row["GroupName"] = textBox1.Text.Trim();
                row["GroupOrder"] = numericUpDown1.Value;
                row["LocationID"] = LocationID;
                table.Rows.Add(row);
            }
            else
            {
                row["GroupName"] = textBox1.Text.Trim();
                row["GroupOrder"] = numericUpDown1.Value;
                row["LocationID"] = LocationID;
            }
            LB.Update();
            LB.Reload();
            ToAdd();
        }
    }
}
