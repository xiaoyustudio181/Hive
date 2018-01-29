using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using MyFrame;

namespace Notebook
{
    public partial class Login : Form
    {
        MyWinForm FM;
        string AccessPath, Password;
        public Login()
        {
            InitializeComponent();
            FM = new MyWinForm(this);
            FM.SetDefaultStyle("笔记本");
        }
        private void button1_Click(object sender, EventArgs e)//进入笔记本
        {
            doLogin();
        }
        void doLogin()
        {
            AccessPath = INI.Root+ @"\" + INI.GetDBName() + ".mdb";
            Password = textBox1.Text;
            Global.con = MyDB.GetAccessConnection(AccessPath, Password);
            if (Global.con != null)
            {
                new Notes(this).Show();
                this.Hide();
            }
            Recover();
        }
        /// <summary>
        /// 清空并聚焦文本框。
        /// </summary>
        void Recover()
        {
            textBox1.Text = "";
            textBox1.Focus();
            textBox1.Select();
        }
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar.Equals('\r')) doLogin();
        }

    }
}
