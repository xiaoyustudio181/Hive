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
    public partial class PageConfig : Form
    {
        MyWinForm FM;
        public PageConfig()
        {
            InitializeComponent();
            FM = new MyWinForm(this);
            FM.SetDefaultStyle("分页设置");
            numericUpDown1.Value = (decimal)INI.GetPerPage();
            MyTip.Set(label1, "设为 0 则不进行分页。");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            INI.SetPerPage(numericUpDown1.Value);
            this.Close();
        }
    }
}
