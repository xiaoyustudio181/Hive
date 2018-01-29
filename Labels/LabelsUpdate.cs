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
    public partial class LabelsUpdate : Form
    {
        MyWinForm FM;
        MyComboBox CBB;
        MyListViewA LV;
        DataTable table;
        DataRow row;
        bool Operation;//true新增，false修改
        MyModel M;
        object result;
        string LocationID;
        int Count;
        object GroupID;
        public LabelsUpdate(object GroupID)
        {
            this.GroupID = GroupID;
            InitializeComponent();
            M = MyModel.GetInstance(Global.con);
            FM = new MyWinForm(this);
            LocationID = INI.GetLocationID();
            FM.SetDefaultStyle("标签 >> 地点：" + M.GetLocationNameByID(LocationID));
            LoadList();
            DefineView();
        }
        void LoadList()
        {
            CBB = new MyComboBox(comboBox1, "select * from LabelGroups where LocationID=" + LocationID + " order by GroupOrder", "GroupName", "GroupID", Global.con);
            CBB.SelectedValue = GroupID;
        }
        void DefineView()
        {
            LV = new MyListViewA(listView1);
            LV.SetDB(Global.con, "Labels");
            LV.ColumnWidths = new int[] { 0, 0, 0, 230, 65 };//列宽（默认80）
            LV.RowHeight = 30;
            LV.RowColor = Color.LightGray;
            LV.SetStyle("Cambria", 16, FontStyle.Bold);
            LV.QueryFields = "LabelID,GroupID,Path,LabelName as 标签名,LabelOrder as 序号";
            LV.Where("GroupID=" + GroupID);
            LV.OrderBy("LabelOrder");
            ShowData();
        }
        void ShowData()
        {
            LV.ShowData(true);
            if (table == null) table = LV.DataTable;
            ToAdd();
            LV.Focus();
        }
        void ToAdd()//准备新增
        {
            Operation = true;
            button1.Text = "确认新增";
            textBox1.Text = "";
            textBox2.Text = "";
            numericUpDown1.Value = 0;
            CBB.SelectedValue = GroupID;
        }
        private void button4_Click(object sender, EventArgs e)//准备新增
        {
            ToAdd();
        }
        bool Check()
        {
            MyError.NewRecord();
            if (textBox1.Text.Trim() == "")
                MyError.Set(textBox1, "名称不可为空。");
            else if (textBox1.Text.Trim().Length > 50)
                MyError.Set(textBox1, "名称字数太长。");
            else if (Operation && M.DuplicateLabelName(textBox1.Text.Trim(), LocationID))//新增时，检查是否重名
                MyError.Set(textBox1, "名称重复。");
            else if (!Operation
                && row["标签名"].ToString() != textBox1.Text.Trim()
                && M.DuplicateLabelName(textBox1.Text.Trim(), LocationID))//修改时，名称有变，检查是否重名
                MyError.Set(textBox1, "名称重复。");
            else MyError.Clear(textBox1);

            if (textBox2.Text.Trim() == "") 
                MyError.Set(textBox2, "路径不可为空。");
            else if (textBox2.Text.Trim().Length > 200)
                MyError.Set(textBox2, "路径字数太长。");
            else MyError.Clear(textBox2);

            return MyError.CheckAll();
        }
        private void button1_Click(object sender, EventArgs e)//确认
        {
            if (!Check()) return;
            if (Operation)
            {
                row = table.NewRow();
                row["LabelID"] = M.GetMaxLabelID();
                row["标签名"] = textBox1.Text.Trim();
                row["Path"] = textBox2.Text.Trim();
                row["GroupID"] = CBB.SelectedValue;
                row["序号"] = numericUpDown1.Value;
                table.Rows.Add(row);
            }
            else
            {
                row["标签名"] = textBox1.Text.Trim();
                row["Path"] = textBox2.Text.Trim();
                row["GroupID"] = CBB.SelectedValue;
                row["序号"] = numericUpDown1.Value;
            }
            LV.Update();
            ShowData();
        }
        private void button2_Click(object sender, EventArgs e)//删除标签
        {
            if (LV.CountSelectedRows == 0) { MyDialog.Msg("没有选中标签。", 2); return; }
            if (MyDialog.Ask("确定要删除选中的 "+LV.GetSelectedValues().Length+" 个标签？"))
            {
                foreach (string each in LV.SelectedValues)
                    table.Rows.Find(each).Delete();
                LV.Update();
                ShowData();
            }
        }
        private void button3_Click(object sender, EventArgs e)//测试路径
        {
            MyProcess.Run(textBox2.Text);
        }
        private void LabelsUpdate_DragDrop(object sender, DragEventArgs e)//拖入文件
        {
            textBox2.Text = FM.DragInPath;
        }

        private void listView1_MouseUp(object sender, MouseEventArgs e)
        {
            if (LV.CountSelectedRows == 0) return;
            Operation = false;
            button1.Text = "确认修改";
            row = table.Rows.Find(LV.GetSelectedValues()[0]);
            textBox1.Text = row["标签名"].ToString();
            textBox2.Text = row["Path"].ToString();
            numericUpDown1.Value = Convert.ToInt32(row["序号"]);
        }


    }
}
