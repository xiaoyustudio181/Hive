using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Text;
using System.Windows.Forms;

namespace MyFrame
{
    public class MyDB
    {
        public static OleDbConnection GetAccessConnection(string AccessPath, string Password = "")
        {
            OleDbConnection con = new OleDbConnection(
                "data source=" + AccessPath + ";" +
                "provider=Microsoft.Jet.OLEDB.4.0;" +
                "mode=12;" +//独占方式
                "" +
                "Jet OleDb:DataBase Password=" + Password);
            try
            {
                con.Open();
                con.Close();
                return con;
            }
            catch (Exception ex)
            {
                MyDialog.Msg(ex.Message, 3);
                return null;
            }
        }
        public static SqlConnection GetSqlServerConnection(string ServerIP, string Password, string DBName, string UserID = "sa")
        {
            SqlConnection con = new SqlConnection(
                "data source=" + ServerIP + ";"+
                "initial catalog=" + DBName + ";"+
                "uid=" + UserID + ";"+
                "pwd=" + Password + ";"+
                "integrated security=no;"+
                "connection timeout=3;"+
                ""+
                "persist security info=false;");
            try
            {
                con.Open();//若IP地址有误，运行到这里时会等待很久，因此用connection timeout定时结束等待
                con.Close();
                return con;
            }
            catch (Exception ex)
            {
                MyDialog.Msg(ex.Message, 3);
                return null;
            }
        }
    }
}
