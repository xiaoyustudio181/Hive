using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MyFrame
{
    public class MyGridView
    {
        protected DataGridView GV;
        public MyGridView(DataGridView GV)
        {
            this.GV = GV;
        }
        public string[] GetSelectedValues()
        {
            int Count = CountSelectedRows;
            SelectedValues = new string[Count];
            Console.WriteLine("{0}选中行数：{1}", GV.Name, Count);
            for (int i = 0; i < Count; i++)
            {
                SelectedValues[i] = GV.SelectedRows[i].Cells[0].Value.ToString();//获取每行第一列的值
                Console.WriteLine("{0}选中值：{1}", GV.Name, SelectedValues[i]);
            }
            Console.WriteLine();
            return SelectedValues;
        }
        public int[] GetSelectedIndexes()
        {
            int Count = CountSelectedRows;
            SelectedIndexes = new int[Count];
            Console.WriteLine("{0}选中行数：{1}", GV.Name, Count);
            for (int i = 0; i < Count; i++)
            {
                SelectedIndexes[i] = GV.SelectedRows[i].Index;//获取每行的索引
                Console.WriteLine("{0}选中索引：{1}", GV.Name, SelectedIndexes[i]);
            }
            //Console.WriteLine();
            return SelectedIndexes;
        }
        //====选中值
        public string[] SelectedValues { set; get; }//需先调用方法获取
        public int[] SelectedIndexes { set; get; }
        public int CountRows { get { return GV.Rows.Count; } }//行总数
        public int CountSelectedRows { get { return GV.SelectedRows.Count; } }//选中行总数
        //====样式
        protected bool AreColumnSortModesSet=false;
        public bool AlignCenter = false;
        public bool CanResizeColumns = true;
        public bool CanResizeRows = false;
        public bool CanEdit = false;
        protected DataGridViewColumnSortMode[] ColumnSortModes;//列排序功能
        public int[] ColumnWidths = new int[] { };//列宽
        public int RowHeight = 0;//行高
        public Color RowColor = Color.White;//行色
        public void SetStyle(string FontFamily = "微软雅黑", float Size = 11.5F, FontStyle Style = FontStyle.Regular)
        {
            GV.Font = new Font(FontFamily, Size, Style);
            //GV.RowTemplate.Height = 35;//行高（此句仅在绑定数据时有效）
            GV.SelectionMode = DataGridViewSelectionMode.FullRowSelect;//全行选择
            GV.AllowUserToAddRows = false;//是否允许手动添加新行
            GV.AllowUserToResizeColumns = CanResizeColumns;//是否允许调整列宽
            GV.AllowUserToResizeRows = CanResizeRows;//是否允许调整行高
            GV.ReadOnly = !CanEdit;//是否允许手动编辑
            //设置文字居中
            DataGridViewCellStyle CellStyle = new DataGridViewCellStyle();//格子样式
            CellStyle.Alignment = AlignCenter ? DataGridViewContentAlignment.MiddleCenter : DataGridViewContentAlignment.NotSet;//内容是否居中
            GV.DefaultCellStyle = CellStyle;//默认格子样式
            GV.ColumnHeadersDefaultCellStyle = CellStyle;//默认表头格子样式
            GV.RowHeadersDefaultCellStyle = CellStyle;//默认首行格子样式
        }
        public void SetColumnSortModes(bool[] Modes)//设置列宽
        {
            ColumnSortModes = new DataGridViewColumnSortMode[Modes.Length];
            for (int i = 0; i < Modes.Length; i++)
            {
                ColumnSortModes[i] = Modes[i] ? DataGridViewColumnSortMode.Automatic : DataGridViewColumnSortMode.NotSortable;
            }
            if (Modes.Length != 0) AreColumnSortModesSet = true;
        }
        protected void RenderRowColor()//渲染行色
        {
            for (int i = 0; i < GV.Rows.Count; i++)//绑定数据时无效
            {
                if (i % 2 == 0)
                    for (int j = 0; j < GV.ColumnCount; j++)
                        GV.Rows[i].Cells[j].Style.BackColor = RowColor;
            }
        }
        public void Focus()//获取焦点
        {
            GV.Focus();
        }
        public void ClearSelection()//移除选中
        {
            GV.ClearSelection();
        }
        //====分页
        public double PerPage { get; set; }//每页显示的条数
        public double TotalPages { get; set; }//页码总数，Math.Ceiling(总条数/每页条数)
        public double NowPage { get; set; }//当前页码
        public object DataAmount { set; get; }//数据总数，统计用
        public string KeyColumn { get; set; }//主键列(排除用)
        protected MyModel M;
        protected double TopN;//分页SQL用
    }
}
