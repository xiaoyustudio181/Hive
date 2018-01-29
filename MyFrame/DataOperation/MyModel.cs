using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Text;

namespace MyFrame
{
    public class MyModel
    {
        static MyModel instance;
        public static MyModel GetInstance(OleDbConnection con)
        {
            if (instance == null || instance.con.ConnectionString == "")//重新登录后可能出现之前实例化的con被释放，因此要加上这种情况
                instance = new MyModel(con);
            return instance;
        }
        public static MyModel GetNewInstance(OleDbConnection con)
        {
            return new MyModel(con);
        }
        //*/
        //使用ACCESS数据库
        OleDbConnection con = new OleDbConnection();
        OleDbCommand cmd = new OleDbCommand();
        OleDbDataReader reader;
        OleDbCommand cmdRea = new OleDbCommand();
        OleDbDataAdapter adapter;
        OleDbCommand cmdAda = new OleDbCommand();
        MyModel(OleDbConnection con)
        {
            this.con = con;
            cmd.Connection = con;
            cmdRea.Connection = con;
            cmdAda.Connection = con;
            adapter = new OleDbDataAdapter(cmdAda);
        }
        /*/
        //使用SQL SERVER数据库
        SqlConnection con = new SqlConnection();
        SqlCommand cmd = new SqlCommand();
        SqlDataReader reader;
        SqlCommand cmdRea = new SqlCommand();
        SqlDataAdapter adapter;
        SqlCommand cmdAda = new SqlCommand();
        MyModel(SqlConnection con)
        {
            this.con = con;
            cmd.Connection = con;
            cmdRea.Connection = con;
            cmdAda.Connection = con;
            adapter = new SqlDataAdapter(cmdAda);
        }
        //*/
        DataSet dataset = new DataSet();
        DataTable table;
        DataRow row;
        int count;
        object result;
        string temp;
        bool doOpenClose = true;
        #region 数据库维护
        public void AlterDBPWD(string OldPWD, string NewPWD)//修改密码
        {
            Execute("alter database password " + NewPWD + " " + OldPWD);
        }
        public void CreateTables()//创建表结构
        {
            con.Open();
            doOpenClose = false;
            #region 标签簿结构
            Execute("create table LabelLocations(LocationID int constraint LBL_PK primary key,LocationName varchar(30) not null constraint LBLN_UQ unique,LocationOrder int not null)");//LabelLocations
            Execute("create table LabelGroups(GroupID int constraint LBG_PK primary key,LocationID int not null,GroupName varchar(30) not null constraint LBG_UQ unique,GroupOrder int not null)");//LabelGroups
            Execute("create table Labels(LabelID int constraint LB_PK primary key,GroupID int not null,LabelName varchar(50) not null constraint LB_UQ unique,Path varchar(200) not null,LabelOrder int not null)");//Labels
            Execute("alter table LabelGroups add constraint LBG_FK foreign key(LocationID) references LabelLocations(LocationID)");//LabelGroups-LabelLocations
            Execute("alter table Labels add constraint LB_FK foreign key(GroupID) references LabelGroups(GroupID)");//Labels-LabelGroups
            #endregion
            #region 笔记本结构
            Execute("create table NoteGroups(GroupID int constraint NTG_PK primary key,GroupName varchar(30) not null constraint NTGN_UQ unique,GroupOrder int not null)");//NoteGroups
            Execute("create table Notes(NoteID int constraint NT_PK primary key,GroupID int not null,Title varchar(30) not null constraint NTT_UQ unique,Content memo not null,TimeOfRevision datetime not null)");//Notes
            Execute("alter table Notes add constraint NT_FK foreign key(GroupID) references NoteGroups(GroupID)");//Notes-NoteGroups
            #endregion
            #region 日记本结构
            Execute("create table DiaryGroups(GroupID int constraint DG_PK primary key,GroupName varchar(30) not null constraint DGN_UQ unique,GroupOrder int not null)");//DiaryGroups
            Execute("create table DiaryRecords(RecordID int constraint DR_PK primary key,GroupID int not null,DateOfRecord datetime not null,Content memo not null)");//DiaryRecords
            Execute("alter table DiaryRecords add constraint DR_FK foreign key(GroupID) references DiaryGroups(GroupID)");//DiaryRecords-DiaryGroups
            #endregion
            #region 纪念簿结构
            Execute("create table Anniversaries(ID int constraint ANN_PK primary key,Title varchar(50) not null constraint ANN_UQ unique,YearN  smallint not null,MonthN smallint not null,DayN smallint not null,ValidYear bit not null)");//Anniversaries
            #endregion
            con.Close();
            doOpenClose = true;
        }
        #endregion
        #region 标签簿
        public string GetLocationNameByID(object LocationID)
        {
            result=FetchResult("select LocationName from LabelLocations where LocationID=" + LocationID);
            if (result != null)
                return result.ToString();
            else return "未知";
        }
        public void DeleteLabelsByGroupID(object GroupID)
        {
            count = Execute("delete from Labels where GroupID="+GroupID);
            Console.WriteLine("已删除分组({0})的 {1} 条标签。", GroupID, count);
        }
        public void DeleteAllByLocationID(object LocationID)
        {
            con.Open();
            doOpenClose = false;
            cmdRea.CommandText = "select GroupID from LabelGroups where LocationID=" + LocationID;
            reader = cmdRea.ExecuteReader();
            while (reader.Read())
            {
                count=Execute("delete from Labels where GroupID="+reader["GroupID"]);
                Console.WriteLine("已删除分组({0})的 {1} 条标签。", reader["GroupID"], count);
                count = Execute("delete from LabelGroups where GroupID=" + reader["GroupID"]);
                Console.WriteLine("已删除分组({0})。", reader["GroupID"]);
            }
            reader.Close();
            con.Close();
            doOpenClose = true;
        }
        public object GetMaxLabelGroupID()
        {
            result = FetchResult("select max(GroupID) from LabelGroups");
            return result == DBNull.Value ? 1 : (int)result + 1;
        }
        public object GetMaxLabelID()
        {
            result = FetchResult("select max(LabelID) from Labels");
            return result == DBNull.Value ? 1 : (int)result + 1;
        }
        public bool DuplicateLocationName(string LocationName)
        {
            result = FetchResult("select count(*) from LabelLocations where LocationName='" + LocationName + "'");
            return Convert.ToInt32(result) != 0;
        }
        public bool DuplicateLabelGroupName(string GroupName,string LocationID)
        {
            result = FetchResult("select count(*) from LabelGroups where GroupName='" + GroupName + "' and LocationID="+LocationID);//同一地点下是否重复
            return Convert.ToInt32(result) != 0;
        }
        public bool DuplicateLabelName(string LabelName,string LocationID)
        {
            result = FetchResult("select count(*) from Labels as L inner join LabelGroups as G on L.GroupID=G.GroupID where L.LabelName='" + LabelName + "' and G.LocationID=" + LocationID);//同一地点下是否重复
            return Convert.ToInt32(result) != 0;
        }
        #endregion
        #region 笔记本
        public void DeleteNotesByGroupID(object GroupID)
        {
            count = Execute("delete from Notes where GroupID="+GroupID);
            Console.WriteLine("已删除分组({0})的 {1} 篇笔记。", GroupID, count);
        }
        public DataTable GetNotesByGroupID(object GroupID)
        {
            return FetchDetails("select Title,Content from Notes where GroupID="+GroupID);
        }
        public string GetNoteGroupNameByID(object GroupID)
        {
            return FetchResult("select GroupName from NoteGroups where GroupID="+GroupID).ToString();
        }
        public object GetMaxNoteID()
        {
            result = FetchResult("select max(NoteID) from Notes");
            return result == DBNull.Value ? 1 : (int)result + 1;
        }
        public bool DuplicateNoteGroupName(string GroupName)
        {
            result = FetchResult("select count(*) from NoteGroups where GroupName='" + GroupName + "'");
            return Convert.ToInt32(result) != 0;
        }
        public bool DuplicateTitle(string Title)
        {
            result = FetchResult("select count(*) from Notes where Title='" + Title + "'");
            return Convert.ToInt32(result) != 0;
        }
        #endregion
        #region 日记本
        public void DeleteRecordsByGroupID(object GroupID)
        {
            count = Execute("delete from DiaryRecords where GroupID="+GroupID);
            Console.WriteLine("已删除分组({0})的 {1} 篇日记。", GroupID, count);
        }
        public DataTable GetRecordsByGroupID(object GroupID)
        {
            return FetchDetails("select DateOfRecord,Content from DiaryRecords where GroupID=" + GroupID + " order by DateOfRecord desc,RecordID desc");
        }
        public string GetDiaryGroupNameByID(object GroupID)
        {
            return FetchResult("select GroupName from DiaryGroups where GroupID="+GroupID).ToString();
        }
        public object GetMaxRecordID()
        {
            result = FetchResult("select max(RecordID) from DiaryRecords");
            return result == DBNull.Value ? 1 : (int)result + 1;
        }
        public bool DuplicateDiaryGroupName(string GroupName)
        {
            result = FetchResult("select count(*) from DiaryGroups where GroupName='" + GroupName + "'");
            return Convert.ToInt32(result) != 0;
        }
        #endregion
        #region 纪念簿
        public bool DuplicateAnniTitle(string Title)
        {
            result = FetchResult("select count(*) from Anniversaries where Title='" + Title + "'");
            return Convert.ToInt32(result) != 0;
        }
        DateTime now;
        int YearN, MonthN, DayN;
        bool ok = false;
        public object GetNearestDateID()
        {
            result = null;
            now = DateTime.Now;
            con.Open();
            cmdRea.CommandText = "select * from Anniversaries order by MonthN,DayN,ID";
            reader = cmdRea.ExecuteReader();
            while (reader.Read())
            {
                YearN=Convert.ToInt32(reader["YearN"]);
                MonthN=Convert.ToInt32(reader["MonthN"]);
                DayN=Convert.ToInt32(reader["DayN"]);
                if (now.Month == MonthN && now.Day <= DayN)
                {
                    result = reader["ID"];//第一个比今天大的日期的记录的ID
                    break;
                }
                if (now.Month < MonthN)
                {
                    result = reader["ID"];//第一个比今天大的日期的记录的ID
                    break;
                }
            }
            reader.Close();
            con.Close();
            if (result != null) return result;
            else return FetchResult("select  top 1 ID from Anniversaries order by MonthN,DayN,ID");//无结果，则返回最小的日期的记录的ID
        }
        public DataTable GetAnnInfoByID(object ID)
        {
            return FetchDetails("select * from Anniversaries where ID=" + ID);
        }
        public int CountAnniRecords()
        {
            return Convert.ToInt32(FetchResult("select count(*) from Anniversaries"));
        }
        #endregion
        #region 封装（数据源操作）
        public DataTable GetTable(string TableName)
        {
            return dataset.Tables[TableName];
        }
        public DataTable FillTable(string SQL, string TableName, bool SetKey = true)
        {
            cmdAda.CommandText = SQL;
            if (dataset.Tables[TableName] != null)
                dataset.Tables[TableName].Clear();
            adapter.Fill(dataset, TableName);
            table = dataset.Tables[TableName];
            if (SetKey) table.PrimaryKey = new DataColumn[] { table.Columns[0] };
            return table;
        }
        public int Update(string TableName)
        {
            //*/
            new OleDbCommandBuilder(adapter);
            /*/
            new SqlCommandBuilder(adapter);
            //*/
            return adapter.Update(dataset, TableName);
        }
        public int Update(DataTable Table)
        {
            //*/
            new OleDbCommandBuilder(adapter);
            /*/
            new SqlCommandBuilder(adapter);
            //*/
            return adapter.Update(Table);
        }
        /*/
        //导出DataTable数据到表格.xls
        public void SaveToExcel(string TableName, string Name = "Default")
        {
            SaveToExcel(dataset.Tables[TableName], Name);
        }
        string TargetPath;
        public void SaveToExcel(DataTable Table, string Name = "Default")
        {
            if (Name == "Default")
                Name = MyTime.GetNowTimeCode();
            TargetPath = MyDialog.SavingDialog(Name, "xls");
            if (TargetPath == "") return;
            else
            {
                //添加Excel引用，右击项目引用，在框架选项卡中搜索Microsoft Excel，添加一个引用。
                Microsoft.Office.Interop.Excel.Application App = new Microsoft.Office.Interop.Excel.Application();//旧写法 Excel.Application
                Microsoft.Office.Interop.Excel.Workbooks books = App.Workbooks;
                Microsoft.Office.Interop.Excel.Workbook book = books.Add(Microsoft.Office.Interop.Excel.XlWBATemplate.xlWBATWorksheet);
                Microsoft.Office.Interop.Excel.Sheets sheets = book.Worksheets;
                Microsoft.Office.Interop.Excel.Worksheet sheet = (Microsoft.Office.Interop.Excel.Worksheet)sheets.get_Item(1);
                //range = worksheet.get_Range(worksheet.Cells[1, 1], worksheet.Cells[dataGridView.RowCount + 1, dataGridView.ColumnCount]);//旧写法，会报异常
                Microsoft.Office.Interop.Excel.Range range = sheet.Range[sheet.Cells[1, 1], sheet.Cells[Table.Rows.Count + 1, Table.Columns.Count]];
                //range.NumberFormatLocal = "@";
                for (int i = 0; i < Table.Columns.Count; i++)
                    sheet.Cells[1, i + 1] = Table.Columns[i].ColumnName.ToString().Trim();//表头
                for (int row = 0; row < Table.Rows.Count; row++)
                    for (int column = 0; column < Table.Columns.Count; column++)
                        sheet.Cells[row + 2, column + 1] = Table.Rows[row][column].ToString().Trim();
                //range = worksheet.get_Range(worksheet.Cells[1, 1], worksheet.Cells[dataGridView.RowCount + 1, dataGridView.ColumnCount]);//旧写法，会报异常
                range = sheet.Range[sheet.Cells[1, 1], sheet.Cells[Table.Rows.Count + 1, Table.Columns.Count]];
                range.Columns.AutoFit();
                range.RowHeight = 18;
                range.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
                book.Saved = true;
                book.SaveCopyAs(TargetPath);
                //App.Visible = false;
                App.Quit();
                GC.Collect();
            }
        }
        /*/
        //*/
        #endregion
        #region 封装（基本操作）
        public object FetchResult(string SQL)
        {
            if (doOpenClose) con.Open();
            cmd.CommandText = SQL;
            result = cmd.ExecuteScalar();
            if (doOpenClose) con.Close();
            return result;
        }
        int affectedNum;
        public int Execute(string SQL)
        {
            if (doOpenClose) con.Open();
            cmd.CommandText = SQL;
            affectedNum = cmd.ExecuteNonQuery();
            if (doOpenClose) con.Close();
            return affectedNum;
        }
        public DataTable FetchDetails(string SQL)
        {
            cmdAda.CommandText = SQL;
            if (dataset.Tables["Temp"] != null)
                dataset.Tables["Temp"].Clear();
            adapter.Fill(dataset, "Temp");
            return dataset.Tables["Temp"];
        }
        #endregion
    }
}
