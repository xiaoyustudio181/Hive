using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using MyFrame;
namespace Labels
{
    public partial class Labels : Form
    {
        MyWinForm FM;
        Form LoginForm;
        MyModel M;
        string FirstGroupID;
        MyListBox LBGroups, LBLabels;
        public Labels(Form LoginForm=null)
        {
            this.LoginForm = LoginForm;
            M = MyModel.GetInstance(Global.con);
            InitializeComponent();
            FM = new MyWinForm(this);
            FM.SetDefaultStyle("标签簿 >> 地点："+M.GetLocationNameByID(INI.GetLocationID()));
            if (LoginForm == null) button4.Enabled = false;
            DefineView();
        }
        void DefineView()
        {
            LBGroups = new MyListBox(listBox1, "select GroupID,GroupName from LabelGroups where LocationID=" + INI.GetLocationID() + " order by GroupOrder", "GroupName", "GroupID", Global.con);
            try { FirstGroupID = ((DataRowView)LBGroups.Items[0])["GroupID"].ToString(); }
            catch { FirstGroupID = "0"; }
            LBLabels = new MyListBox(listBox2, "select LabelName,Path from Labels where GroupID=" + FirstGroupID + " order by LabelOrder", "LabelName", "Path", Global.con);
            try { LBGroups.SelectedIndex = 0; }
            catch { }
            LBLabels.Focus();
        }
        void ReloadData()
        {
            LBGroups.Reset("select GroupID,GroupName from LabelGroups where LocationID=" + INI.GetLocationID() + " order by GroupOrder", "GroupName", "GroupID");
            try { FirstGroupID = ((DataRowView)LBGroups.Items[0])["GroupID"].ToString(); }
            catch { FirstGroupID = "0"; }
            LBLabels.Reset("select LabelName,Path from Labels where GroupID=" + FirstGroupID + " order by LabelOrder", "LabelName", "Path");
            try { LBGroups.SelectedIndex = 0; }
            catch { }
            LBLabels.Focus();
        }
        bool Exit = true;
        private void Labels_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (LoginForm != null && Exit) LoginForm.Close();
        }
        private void button4_Click(object sender, EventArgs e)//注销
        {
            Exit = false;
            LoginForm.Show();
            this.Close();
        }
        private void button3_Click(object sender, EventArgs e)//标签管理
        {
            if (LBGroups.Items.Count == 0)
            { MyDialog.Msg("请新建标签分组。", 2); return; }
            new LabelsUpdate(LBGroups.SelectedValue).ShowDialog();
            ReloadData();
        }
        private void button1_Click(object sender, EventArgs e)//分组管理
        {
            if (Text.Contains("未知"))
            { MyDialog.Msg("请使用已知地点。", 2); return; }
            new Groups().ShowDialog();
            ReloadData();
        }
        private void button2_Click(object sender, EventArgs e)//地点管理
        {
            new Locations().ShowDialog();
            FM.SetDefaultStyle("标签簿 >> 地点：" + M.GetLocationNameByID(INI.GetLocationID()));
            ReloadData();
        }
        private void listBox1_Click(object sender, EventArgs e)//点击分组
        {
            if (LBGroups.SelectedValue != null)
                LBLabels.Reset("select LabelName,Path from Labels where GroupID=" + LBGroups.SelectedValue + " order by LabelOrder", "LabelName", "Path");
            LBLabels.Focus();
        }
        private void listBox2_Click(object sender, EventArgs e)//点击标签
        {
            if (LBLabels.SelectedValue != null)
                MyProcess.Run(LBLabels.SelectedValue.ToString());
            LBLabels.ClearSelected();
        }
    }
}
