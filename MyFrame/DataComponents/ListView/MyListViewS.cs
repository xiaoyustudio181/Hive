using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Windows.Forms;

namespace MyFrame
{
    public class MyListViewS : MyListViewA
    {
        public MyListViewS(ListView LV) : base(LV) { }
        //==========================数据
        SqlConnection con;
        SqlCommand cmd = new SqlCommand();//查询用
        SqlDataAdapter adapter;
        public void SetDB(SqlConnection con, string TableName)
        {
            this.con = con;
            cmd.Connection = con;
            adapter = new SqlDataAdapter(cmd);
            this.TableName = TableName;
        }
        void MakeCommandText()
        {
            if (SpecialSql == null)
            {
                Sql = "select " + QueryFields + " from " + TableName;
                if (where != null) Sql += where;
                if (orderBy != null) Sql += orderBy;
                cmd.CommandText = Sql;
            }
            else cmd.CommandText = SpecialSql;
        }
        //====装载
        bool CheckCondition()
        {
            if (con == null)
            {
                MyDialog.Msg(LV.Name + "尚未绑定数据库！", 3);
                Environment.Exit(0);
                return false;
            }
            if (QueryFields == null)
            {
                MyDialog.Msg(LV.Name + "尚未设置查询字段！", 3);
                Environment.Exit(0);
                return false;
            }
            return true;
        }
        new public void ShowData(bool SetFirstColumnAsPrimaryKey = false)
        {
            LV.Clear();
            if (!CheckCondition()) return;
            MakeCommandText();
            if (table != null) table.Clear();
            adapter.Fill(dataset, "Tab");
            table = dataset.Tables["Tab"];
            if (SetFirstColumnAsPrimaryKey)
                table.PrimaryKey = new DataColumn[] { table.Columns[0] };
            GenerateHeader();
            LoadData();
            RenderRowColor();
        }
        //====操作
        public override int Update()
        {
            try
            {
                new SqlCommandBuilder(adapter);
                return adapter.Update(table);
            }
            catch (Exception ex)
            {
                MyDialog.Msg(ex.Message, 3);
                return 0;
            }
        }
    }
}
