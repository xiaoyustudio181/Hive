using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Text;
using System.Windows.Forms;

namespace MyFrame
{
    public class MyComboBox
    {
        ComboBox CBB;
        DataSet dataset=new DataSet();
        DataTable table;
        MyComboBox(ComboBox CBB, string SQL, string DisplayMember, string ValueMember)
        {
            this.CBB = CBB;
            this.SQL = SQL;
            CBB.DropDownStyle = ComboBoxStyle.DropDownList;
            this.DisplayMember = DisplayMember;
            this.ValueMember = ValueMember;
            CBB.SelectedIndexChanged += new EventHandler(IndexChanged);
        }
        string SQL, DisplayMember, ValueMember;
        bool useAccess, SetFirstColumnAsPrimaryKey, HasBound;
        public string Text
        {
            set { CBB.Text = value; }
            get { return CBB.Text; }
        }
        public object SelectedValue
        {
            set { CBB.SelectedValue = value; }
            get { return CBB.SelectedValue; }
        }
        public object SelectedItem
        {
            set { CBB.SelectedItem = value; }
            get { return CBB.SelectedItem; }
        }
        public ComboBox.ObjectCollection Items
        {
            get { return CBB.Items; }
        }
        public int SelectedIndex
        {
            set { CBB.SelectedIndex = value; }
            get { return CBB.SelectedIndex; }
        }
        void IndexChanged(object sender, EventArgs e)
        {
            Console.WriteLine("{0}被选中值：{1}", CBB.Name, SelectedValue);
            Console.WriteLine("{0}被选中文本：{1}", CBB.Name,CBB.Text);
        }
        //====Access
        OleDbConnection conA;
        OleDbCommand cmdA = new OleDbCommand();
        OleDbDataAdapter adapterA;
        public MyComboBox(ComboBox CBB, string SQL, string DisplayMember, string ValueMember, OleDbConnection conA, bool SetFirstColumnAsPrimaryKey = false)
            : this(CBB, SQL, DisplayMember, ValueMember)
        {
            this.conA = conA;
            cmdA.Connection = conA;
            adapterA = new OleDbDataAdapter(cmdA);
            useAccess = true;
            this.SetFirstColumnAsPrimaryKey = SetFirstColumnAsPrimaryKey;
            Reload();
        }
        //====SqlServer
        SqlConnection conS;
        SqlCommand cmdS = new SqlCommand();
        SqlDataAdapter adapterS;
        public MyComboBox(ComboBox CBB, string SQL, string DisplayMember, string ValueMember, SqlConnection conS, bool SetFirstColumnAsPrimaryKey = false)
            : this(CBB, SQL, DisplayMember, ValueMember)
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
            if (useAccess) LoadA(); else LoadS();
            if (!HasBound) Bind();
            SetMember();
        }
        void LoadA()
        {
            cmdA.CommandText = SQL;
            adapterA.Fill(dataset, "Tab");
        }
        void LoadS()
        {
            cmdS.CommandText = SQL;
            adapterS.Fill(dataset, "Tab");
        }
        void Bind()
        {
            table = dataset.Tables["Tab"];
            if (SetFirstColumnAsPrimaryKey)
                table.PrimaryKey = new DataColumn[] { table.Columns[0] };
            CBB.DataSource = table;
            HasBound = true;
        }
        void SetMember()
        {
            CBB.DisplayMember = DisplayMember;
            CBB.ValueMember = ValueMember;
        }
        //====
    }
}
