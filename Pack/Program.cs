using System;
using System.Collections.Generic;
using System.Text;
using MyFrame;
using System.IO;
namespace Pack
{
    class Program
    {
        static void Main(string[] args)
        {
            string NewDirPath = INI.Root + @"\WinHelper";
            string BackupPath=INI.Root+@"\Backup";
            string NewBackupPath = NewDirPath + @"\Backup";
            if (Directory.Exists(NewDirPath)) 
            { MyDialog.Msg("根目录下已存在 WinHelper 文件夹。打包已终止。", 2); return; }
            Directory.CreateDirectory(NewDirPath);
            Directory.CreateDirectory(NewBackupPath);
            //============================================
            foreach (FileInfo file in new DirectoryInfo(INI.Root).GetFiles())
            {
                if (file.Name == "Hive.exe" ||//主程序
                    file.Name == "Notebook.exe" ||//笔记本
                    file.Name == "Diary.exe" ||//日记本
                    file.Name == "ElectronicWatch.exe" ||//电子表
                    file.Name == "TouchTypist.exe" ||//盲打机
                    file.Name == "Reader.exe" ||//阅读器
                    file.Name == "Labels.exe" ||//标签簿
                    file.Name == "Anniversaries.exe" ||//纪念簿
                    file.Name == "FastCmd.exe" ||//快速命令

                    file.Name == "MyFrame.dll" ||//我的框架
                    file.Name == "NAMEIT.mdb" ||//新数据库
                    file.Name.StartsWith("v"))//版本文本
                    file.CopyTo(NewDirPath + @"\" + file.Name);//复制到新的目录
            }
            foreach (FileInfo file in new DirectoryInfo(BackupPath).GetFiles())
            {
                if(file.Name == "Commands.txt" ||//命令簿
                    file.Name == "New.mdb" ||
                    file.Name == "Configuration.ini" ||//配置文件
                    file.Name == "Manual.txt")
                    file.CopyTo(NewBackupPath + @"\" + file.Name);//复制到新的Backup目录
            }
            //======================================
            foreach (FileInfo file in new DirectoryInfo(INI.Root).GetFiles())//打包完成后，尽量清除垃圾
            {
                if (!file.Name.StartsWith("v")&&
                    file.Name != "NAMEIT.mdb"&&
                    file.Name != "Pack.exe" &&
                    file.Name != "Pack.pdb" &&
                    file.Name != "Pack.vshost.exe" &&
                    file.Name!="MyFrame.dll"&&
                    file.Name!="Hive.vshost.exe"&&
                    file.Name!="MyFrame.pdb")
                {
                    try
                    {
                        file.Delete();
                    }
                    catch(Exception ex)
                    {
                        MyTextFile.DoDefaultRecord(ex.Message);
                    }
                }
            }
        }
    }
}
