using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Text;
using System.Windows.Forms;

namespace MyFrame
{
    public class MyCheckedListBox
    {
        CheckedListBox CLB;
        MyCheckedListBox(CheckedListBox CLB, string SQL, string DisplayMember, string ValueMember)
        {
            this.CLB = CLB;
            this.SQL = SQL;
            this.DisplayMember = DisplayMember;
            this.ValueMember = ValueMember;
            CLB.CheckOnClick = true;
            CLB.SelectedIndexChanged += new EventHandler(Changed);
        }
        string[] selectedValues;
        public string[] SelectedValues { get { return selectedValues; } }
        public CheckedListBox.ObjectCollection Items
        {
            get { return CLB.Items; }
        }
        void Changed(object sender, EventArgs e)
        {
            if (CLB.CheckedItems.Count != 0)
            {
                selectedValues = new string[CLB.CheckedItems.Count];
                int i = 0;
                foreach (DataRowView each in CLB.CheckedItems)//勾选中的
                {
                    selectedValues[i] = each.Row[ValueMember].ToString();
                    Console.WriteLine("{0}被选中的值：{1}", CLB.Name, selectedValues[i]);
                }
                Console.WriteLine();
            }
        }
        string SQL, DisplayMember, ValueMember;
        DataSet dataset = new DataSet();
        DataTable table;
        bool SetFirstColumnAsPrimaryKey, useAccess, HasBound = false;
        //====Access
        OleDbConnection conA;
        OleDbCommand cmdA = new OleDbCommand();
        OleDbDataAdapter adapterA;
        public MyCheckedListBox(CheckedListBox CLB, string SQL, string DisplayMember, string ValueMember, OleDbConnection conA, bool SetFirstColumnAsPrimaryKey=false)
            : this(CLB, SQL, DisplayMember, ValueMember)
        {
            this.conA = conA;
            this.SetFirstColumnAsPrimaryKey = SetFirstColumnAsPrimaryKey;
            cmdA.Connection = conA;
            adapterA = new OleDbDataAdapter(cmdA);
            useAccess = true;
            Reload();
        }
        //====SqlServer
        SqlConnection conS;
        SqlCommand cmdS = new SqlCommand();
        SqlDataAdapter adapterS;
        public MyCheckedListBox(CheckedListBox CLB, string SQL, string DisplayMember, string ValueMember, SqlConnection conS, bool SetFirstColumnAsPrimaryKey=false)
            : this(CLB, SQL, DisplayMember, ValueMember)
        {
            this.conS = conS;
            this.SetFirstColumnAsPrimaryKey = SetFirstColumnAsPrimaryKey;
            cmdS.Connection = conS;
            adapterS = new SqlDataAdapter(cmdS);
            useAccess = false;
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
            CLB.ClearSelected();
        }
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
            CLB.DataSource = table;
            if (SetFirstColumnAsPrimaryKey)
                table.PrimaryKey = new DataColumn[] { table.Columns[0] };
            HasBound = true;
        }
        void SetMember()
        {
            CLB.DisplayMember = DisplayMember;
            CLB.ValueMember = ValueMember;
        }
    }
}
