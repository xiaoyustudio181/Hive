using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MyFrame
{
    public class MyGridViewA : MyGridView
    {
        public MyGridViewA(DataGridView GV) : base(GV) { }
        //====数据
        OleDbConnection con;
        OleDbCommand cmd = new OleDbCommand();
        OleDbDataAdapter adapter;
        public void SetDB(OleDbConnection con, string TableName)
        {
            this.con = con;
            cmd.Connection = con;
            adapter = new OleDbDataAdapter(cmd);
            this.TableName = TableName;
            M = MyModel.GetInstance(con);
        }
        protected DataSet dataset = new DataSet();
        protected DataTable table;
        protected string orderBy, where, Sql, SpecialSql;
        public string TableName { get; set; }
        public string QueryFields { get; set; }
        public DataTable DataTable { get { return table; } }
        public void UseSpecialSQL(string SQL)
        {
            SpecialSql = SQL;
        }
        public void AbandonSpecialSQL()
        {
            SpecialSql = null;
        }
        public void Where(string Condition)
        {
            where = " where " + Condition;
        }
        public void OrderBy(string Condition)
        {
            orderBy = " order by " + Condition;
        }
        public void ClearWhere()
        {
            where = null;
        }
        public void ClearOrderBy()
        {
            orderBy = null;
        }
        void MakeCommandText()
        {
            if (SpecialSql == null)//不使用自编SQL语句
            {
                if (PerPage == 0)//不分页
                {
                    Sql = "select " + QueryFields + " from " + TableName;
                    if (where != null) Sql += where;
                    if (orderBy != null) Sql += orderBy;
                    cmd.CommandText = Sql;
                }
                else//要分页
                {
                    //例句：select top PerPage * from Categories where CategoryID not in(select top ((NowPage-1)*PerPage) CategoryID from Categories order by CategoryID) order by CategoryID--第一页数据
                    Sql = "select count(*) from " + TableName;
                    if (where != null) Sql += where;
                    DataAmount = M.FetchResult(Sql);//获取数据总数
                    TotalPages = Math.Ceiling(Convert.ToDouble(DataAmount) / PerPage);//获取总页数
                    if (NowPage > TotalPages) NowPage = TotalPages; //大于最大页码，包括没有数据的情况(1>0)，TotalPages=0
                    if (NowPage < 1) NowPage=1; //小于最小页码
                    Sql = "select top " + PerPage + " " + QueryFields + " from " + TableName;
                    TopN = (NowPage - 1) * PerPage;
                    if (where != null)//有where条件
                    {
                        Sql += where + " and " + KeyColumn + " not in";
                        if (TopN != 0)//非第一页
                            Sql += "(select top " + TopN + " " + KeyColumn + " from " + TableName;
                        else Sql += "(0";//因为Access查询不识别top 0，所以这种情况直接赋0

                        if (orderBy != null)//有order by排序
                        {
                            if (TopN != 0)//非第一页
                                Sql += where+" "+orderBy + ")" + orderBy;
                            else Sql += ") " + orderBy;
                        }
                        else Sql += ")";
                    }
                    else//无where条件
                    {
                        Sql += " where " + KeyColumn + " not in";
                        if (TopN != 0)//非第一页
                            Sql += "(select top " + TopN + " " + KeyColumn + " from " + TableName;
                        else Sql += "(0";//因为Access查询不识别top 0，所以这种情况直接赋0

                        if (orderBy != null)//有order by排序
                        {
                            if (TopN != 0)//非第一页
                                Sql += orderBy + ")" + orderBy;
                            else Sql += ") " + orderBy;
                        }
                        else Sql += ")";
                    }
                    cmd.CommandText = Sql;
                }
            }
            else cmd.CommandText = SpecialSql;//使用自编SQL语句
        }
        //====装载：代码生成
        protected void RemoveColumns()
        {
            while (GV.Columns.Count != 0)
                GV.Columns.RemoveAt(0);
        }
        bool CheckCondition()
        {
            if (con == null)
            {
                MyDialog.Msg(GV.Name + "尚未绑定数据库！",3);
                Environment.Exit(0);
                return false;
            }
            if (QueryFields == null)
            {
                MyDialog.Msg(GV.Name + "尚未设置查询字段！", 3);
                Environment.Exit(0);
                return false;
            }
            return true;
        }
        protected void GenerateHeader()
        {
            DefaultStyles();
            DataGridViewTextBoxColumn Column;
            for (int i = 0; i < table.Columns.Count; i++)
            {
                Column = new DataGridViewTextBoxColumn();
                Column.HeaderText = table.Columns[i].ColumnName;
                Column.Width = ColumnWidths[i];
                Column.SortMode = ColumnSortModes[i];
                GV.Columns.Add(Column);
            }
        }
        void DefaultStyles()
        {
            #region 若未设置行高（默认自动适应）
            if (RowHeight == 0)
                GV.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            #endregion
            #region 若未设置列宽（默认自动适应）
            if (ColumnWidths.Length == 0)
            {
                GV.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                ColumnWidths = new int[table.Columns.Count];
                for (int i = 0; i < table.Columns.Count; i++)
                    ColumnWidths[i] = 0;//填充用，实际将自动适应
            }
            #endregion
            #region 若未设置表头排序功能（禁用表头排序功能）
            if (!AreColumnSortModesSet)
            {
                ColumnSortModes = new DataGridViewColumnSortMode[table.Columns.Count];
                for (int i = 0; i < table.Columns.Count; i++)
                    ColumnSortModes[i] = DataGridViewColumnSortMode.NotSortable;
            }
            #endregion
        }
        protected void LoadData()
        {
            DataGridViewRow NewRow;
            DataGridViewTextBoxCell NewCell;
            GV.Rows.Clear();
            for (int i = 0; i < table.Rows.Count; i++)
            {
                NewRow = new DataGridViewRow();
                NewRow.Height = RowHeight;
                for (int j = 0; j < table.Columns.Count; j++)
                {
                    NewCell = new DataGridViewTextBoxCell();
                    if (table.Rows[i][j].GetType() == typeof(DateTime))
                        NewCell.Value = table.Rows[i][j].ToString().Split(' ')[0].Replace('/', '-');
                    else NewCell.Value = table.Rows[i][j];
                    NewRow.Cells.Add(NewCell);
                }
                GV.Rows.Add(NewRow);
            }
        }
        public void ShowData(bool SetFirstColumnAsPrimaryKey = false)
        {
            RemoveColumns();
            if (!CheckCondition()) return;
            MakeCommandText();
            if (table != null) table.Clear();
            adapter.Fill(dataset, "Tab");
            table = dataset.Tables["Tab"];
            if (SetFirstColumnAsPrimaryKey)//设置主键后，主键列不允许出现重复值
                table.PrimaryKey = new DataColumn[] { table.Columns[0] };
            GenerateHeader();
            LoadData();
            RenderRowColor();
        }
        //====装载：数据绑定
        public void BindData(bool SetFirstColumnAsPrimaryKey = false)
        {
            if (!CheckCondition()) return;
            MakeCommandText();
            if (table != null) table.Clear();
            adapter.Fill(dataset, "Tab");
            table = dataset.Tables["Tab"];
            GV.Columns.Clear();
            while (GV.Columns.Count < table.Columns.Count)//自动生成列
                GV.Columns.Add(new DataGridViewTextBoxColumn());
            if (SetFirstColumnAsPrimaryKey)
                table.PrimaryKey = new DataColumn[] { table.Columns[0] };
            DefaultStylesB();
            GV.AutoGenerateColumns = false;
            GV.DataSource = table;
            for (int i = 0; i < GV.Columns.Count; i++)
            {
                GV.Columns[i].HeaderText = table.Columns[i].ColumnName;
                GV.Columns[i].Width = ColumnWidths[i];
                GV.Columns[i].DataPropertyName = table.Columns[i].ColumnName;
                GV.Columns[i].SortMode = ColumnSortModes[i];
            }
            GV.RowTemplate.Height = RowHeight;
            //RenderRowColor();//无效
        }
        protected void DefaultStylesB()
        {
            #region 若未设置行高（默认自动适应）
            if (RowHeight == 0)
                GV.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            #endregion
            #region 若未设置列宽（默认自动适应）
            if (ColumnWidths.Length == 0)
            {
                GV.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                ColumnWidths = new int[table.Columns.Count];
                for (int i = 0; i < table.Columns.Count; i++)
                    ColumnWidths[i] = 0;//填充用，实际将自动适应
            }
            #endregion
            #region 若未设置表头排序功能（禁用表头排序功能）
            if (!AreColumnSortModesSet)
            {
                ColumnSortModes = new DataGridViewColumnSortMode[table.Columns.Count];
                for (int i = 0; i < table.Columns.Count; i++)
                    ColumnSortModes[i] = DataGridViewColumnSortMode.NotSortable;
            }
            #endregion
        }
        //====操作
        public virtual int Update()
        {
            try
            {
                new OleDbCommandBuilder(adapter);
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