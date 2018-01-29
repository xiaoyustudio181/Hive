using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MyFrame
{
    public class MyFile
    {
        StreamReader reader;
        public MyFile(string FilePath)
        {
            this.FilePath = FilePath;
            FileName = Path.GetFileName(FilePath);
            if (Path.HasExtension(FilePath))//有扩展名的情况
            {
                Name = Path.GetFileNameWithoutExtension(FilePath);
                Extension = Path.GetExtension(FilePath);
            }
            else Name = FileName;//无扩展名的情况（Extension为null）
            if (!File.Exists(FilePath)) return;
            this.Length = new FileInfo(FilePath).Length;
            if (Extension == ".txt") ReadText();
        }
        void ReadText()
        {
            reader = new StreamReader(FilePath, Encoding.Default);
            Content = reader.ReadToEnd();
            reader.Close();
        }
        public string FilePath { set; get; }//完整路径
        public string FileName { set; get; }//文件名带扩展名
        public string Name { set; get; }//文件名不带扩展名
        public string Extension { set; get; }//带"."的扩展名
        public string Content { set; get; }
        public long Length { set; get; }
    }
}
