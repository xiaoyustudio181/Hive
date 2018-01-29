using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using MyFrame;
using System.IO;
namespace Hive
{
    public partial class Maintenance : Form
    {
        CentralControl CentralForm;
        MyWinForm FM;
        MyModel M;//修改密码
        string Configuration = "[Basic]\nDBName=NAMEIT\nLocationID=1\n\n[Diary]\nPerPage=16\n\n[ReaderMode1]\nName=Style 1 -cn\nFontFamily=KaiTi\nBold=True\nFontSize=26.25\nFontColor=-16777216\n\n[ReaderMode2]\nName=Style 2 -cn\nFontFamily=STZhongsong\nBold=True\nFontSize=26.25\nFontColor=-16777216\n\n[ReaderMode3]\nName=Style 3 -en\nFontFamily=Comic Sans MS\nBold=True\nFontSize=26.25\nFontColor=-16777216\n\n[ReaderMode4]\nName=Style 4 -en\nFontFamily=Cambria\nBold=True\nFontSize=26.25\nFontColor=-16777216\n\n[ReaderMode5]\nName=Style 5 -Special\nFontFamily=Microsoft YaHei\nBold=True\nFontSize=26.25\nFontColor=-16777216\n\n[ReaderFinalState]\nFinalMode=1\nFinalWindowState=Normal\nFinalWindowWidth=1131\nFinalWindowHeight=735\n";
        string temp;
        public Maintenance(CentralControl CentralForm)
        {
            this.CentralForm = CentralForm;
            InitializeComponent();
            FM = new MyWinForm(this);
            FM.SetDefaultStyle("软件维护");
            if (Global.con == null) groupBox1.Enabled = false;
            else M = MyModel.GetInstance(Global.con);
        }
        string OldPWD, NewPWD;
        private void button1_Click(object sender, EventArgs e)//确认修改密码
        {
            temp = Global.con.ConnectionString;
            //data source=G:\NAMEIT2.mdb;provider=Microsoft.Jet.OLEDB.4.0;mode=12;Jet OleDb:DataBase Password=123
            OldPWD = temp.Substring(temp.IndexOf("Password") + "Password".Length + 1);//包括“=”占用的
            NewPWD = textBox2.Text;
            if (textBox1.Text == OldPWD)
                M.AlterDBPWD(OldPWD == "" ? "null" : OldPWD, NewPWD == "" ? "null" : NewPWD);//是空密码则切换为null
            else 
            { 
                MyDialog.Msg("验证失败。",3);
                textBox1.Text = "";
                textBox1.Focus();
                return; 
            }
            MyDialog.Msg("密码已更新！即将重新登录。",1);
            CentralForm.Logout();
        }
        string DBName;
        private void button2_Click(object sender, EventArgs e)//确认新建数据库
        {
            DBName = textBox3.Text.Trim();
            if (DBName == "") 
            { 
                MyDialog.Msg("请输入数据库名。", 2); 
                textBox3.Focus(); 
                return; 
            }
            foreach (FileInfo file in new DirectoryInfo(INI.Root).GetFiles())
                if (file.Name.ToLower() == DBName.ToLower() + ".mdb")
                { 
                    MyDialog.Msg("根目录存在同名数据库。", 2); 
                    textBox3.Focus(); 
                    return; 
                }
            File.Copy(INI.Root + @"\Backup\New.mdb", INI.Root + @"\" + DBName + ".mdb");
            MyModel.GetNewInstance(MyDB.GetAccessConnection(INI.Root + @"\" + DBName + ".mdb")).CreateTables();
            MyDialog.Msg("创建完成！", 1);
            textBox3.Clear();
            textBox3.Focus();
        }
        StreamWriter writer;
        private void button4_Click(object sender, EventArgs e)//恢复配置文件
        {
            writer = new StreamWriter(INI.Root + @"\Backup\Configuration.ini", false, Encoding.ASCII);//配置文件用UTF-8编码(默认编码)会无法识别。
            writer.Write(Configuration.Replace("\n", Environment.NewLine));
            writer.Close();
            MyDialog.Msg("配置文件已恢复！", 1);
        }
        private void button3_Click(object sender, EventArgs e)//开始选择
        {
            new SelectDB(CentralForm).ShowDialog();
        }
    }
}
