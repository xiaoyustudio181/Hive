using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Text;
using System.Windows.Forms;

namespace MyFrame
{
    public class MyListBox
    {
        ListBox LB;
        MyListBox(ListBox LB, string SQL, string DisplayMember, string ValueMember)
        {
            this.LB = LB;
            this.SQL = SQL;
            this.DisplayMember = DisplayMember;
            this.ValueMember = ValueMember;
            LB.SelectionMode = SelectionMode.One;
            LB.Click += new EventHandler(Click);
        }
        void Click(object sender, EventArgs e)
        {
            Console.WriteLine("{0}被选中值：{1}", LB.Name, SelectedValue);
        }
        string SQL, DisplayMember, ValueMember;
        DataSet dataset = new DataSet();
        DataTable table;
        public DataTable DataTable { get { return table; } }
        bool HasBound = false, useAccess, SetFirstColumnAsPrimaryKey;
        public object SelectedValue
        {
            set { LB.SelectedValue = value; }
            get { return LB.SelectedValue; }
        }
        public object SelectedItem
        {
            set { LB.SelectedItem = value; }
            get { return LB.SelectedItem; }
        }
        public ListBox.ObjectCollection Items
        {
            get { return LB.Items; }
        }
        public int SelectedIndex
        {
            set { LB.SelectedIndex = value; }
            get { return LB.SelectedIndex; }
        }
        //=============================
        OleDbConnection conA;
        OleDbCommand cmdA = new OleDbCommand();
        OleDbDataAdapter adapterA;
        public MyListBox(ListBox LB, string SQL, string DisplayMember, string ValueMember, OleDbConnection conA, bool SetFirstColumnAsPrimaryKey = false)
            : this(LB, SQL, DisplayMember, ValueMember)
        {
            this.conA = conA;
            cmdA.Connection = conA;
            adapterA = new OleDbDataAdapter(cmdA);
            useAccess = true;
            this.SetFirstColumnAsPrimaryKey = SetFirstColumnAsPrimaryKey;
            Reload();
        }
        //====Access
        SqlConnection conS;
        SqlCommand cmdS = new SqlCommand();
        SqlDataAdapter adapterS;
        public MyListBox(ListBox LB, string SQL, string DisplayMember, string ValueMember, SqlConnection conS, bool SetFirstColumnAsPrimaryKey = false)
            : this(LB, SQL, DisplayMember, ValueMember)
        {
            this.conS = conS;
            cmdS.Connection = conS;
            adapterS = new SqlDataAdapter(cmdS);
            useAccess = false;
            this.SetFirstColumnAsPrimaryKey = SetFirstColumnAsPrimaryKey;
            Reload();
        }
        //========
        public void Reset(string SQL, string DisplayMember, string ValueMember)
        {
            this.SQL = SQL;
            this.DisplayMember = DisplayMember;
            this.ValueMember = ValueMember;
            Reload();
        }
        public void Reload()
        {
            if (table != null) table.Clear();
            if (useAccess) LoadA();
            else LoadS();
            SetMember();
            LB.ClearSelected();
        }
        //====SqlServer
        void LoadA()
        {
            cmdA.CommandText = SQL;
            adapterA.Fill(dataset, "Tab");
            if (!HasBound) Bind();
        }
        void LoadS()
        {
            cmdS.CommandText = SQL;
            adapterS.Fill(dataset, "Tab");
            if (!HasBound) Bind();
        }
        void Bind()
        {
            table = dataset.Tables["Tab"];
            if (SetFirstColumnAsPrimaryKey)
                table.PrimaryKey = new DataColumn[] { table.Columns[0] };
            LB.DataSource = table;
            HasBound = true;
        }
        void SetMember()
        {
            LB.DisplayMember = DisplayMember;
            LB.ValueMember = ValueMember;
        }
        //====
        public int Update()
        {
            int ResultNum = 0;
            if (useAccess)
            {
                new OleDbCommandBuilder(adapterA);
                try { ResultNum = adapterA.Update(table); }
                catch (Exception ex) { MyDialog.Msg(ex.Message, 3); }
            }
            else
            {
                new SqlCommandBuilder(adapterS);
                try { ResultNum = adapterS.Update(table); }
                catch (Exception ex) { MyDialog.Msg(ex.Message, 3); }
            }
            return ResultNum;
        }
        public void ClearSelected()
        {
            LB.ClearSelected();
        }
        public void Focus()
        {
            LB.Focus();
        }
    }
}
