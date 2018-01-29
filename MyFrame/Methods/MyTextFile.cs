using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MyFrame
{
    public class MyTextFile
    {
        static StreamWriter writer;
        static string RecordsDirPath;
        static string DefaultRecordPath;
        static MyTextFile()
        {
            RecordsDirPath = INI.Root + @"\Records";
            DefaultRecordPath = RecordsDirPath + @"\DefaultRecord.txt";
            MyMethods.CreateDirectory(RecordsDirPath);
        }
        public static void Prepare()
        {
            //无任何操作的空方法，只为执行静态构造函数。
        }
        #region 文档操作
        public static void Create(string Path, string Text)
        {
            writer = new StreamWriter(Path, false);
            writer.Write(Text.Replace("\n", Environment.NewLine));
            writer.Close();
        }
        public static void Append(string Path, string Text)
        {
            writer = new StreamWriter(Path, true);
            writer.WriteLine(Text.Replace("\n", Environment.NewLine));
            writer.Close();
        }
        public static void Clear(string Path)
        {
            writer = new StreamWriter(Path, false);
            writer.Close();
        }
        #endregion
        #region 记录操作
        public static void CreateUniqueRecordFile(string Text)
        {
            writer = new StreamWriter(RecordsDirPath + "\\" + MyTime.GetNowTimeCode() + ".txt", false);
            writer.Write(Text.Replace("\n", Environment.NewLine));
            writer.Close();
        }
        public static void DoDefaultRecord(string Text)//追加式
        {
            writer = new StreamWriter(DefaultRecordPath, true);
            writer.WriteLine((MyTime.GetNowTimeCode(1)+" >>> "+Text).Replace("\n", Environment.NewLine));
            writer.Close();
        }
        public static void ClearDefaultRecrod()
        {
            writer = new StreamWriter(DefaultRecordPath, false);
            writer.Close();
        }
        #endregion
    }
}
