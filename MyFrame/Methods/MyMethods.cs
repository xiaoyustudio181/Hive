using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace MyFrame
{
    public class MyMethods
    {
        static string temp;
        public static void InsertAtSelectedPosition(RichTextBox Box, string Text)
        {
            int start = Box.SelectionStart;
            Box.Text = Box.Text.Insert(start, Text);
            Box.Focus();
            Box.Select(start + Text.Length, 0);
        }
        #region ASCII字符与ASCII码
        /// <summary>
        /// 根据ASCII码获取对应ASCII字符。
        /// </summary>
        /// <param name="n">ASCII码。</param>
        /// <returns>ASCII字符。</returns>
        public static string GetASCIICharacterByNumber(byte n)
        {
            return Encoding.ASCII.GetChars(new byte[] { n })[0].ToString();
        }
        /// <summary>
        /// 根据ASCII字符获取对应ASCII码。
        /// </summary>
        /// <param name="c">ASCII字符。</param>
        /// <returns>ASCII码。</returns>
        public static byte GetASCIINumberByCharacter(char c)
        {
            return Encoding.ASCII.GetBytes(new char[] { c })[0];
        }
        #endregion
        #region 十进制数转其他进制
        /// <summary>
        /// 根据十进制数获取二进制数。[10]->[2]
        /// </summary>
        /// <param name="n">十进制数。</param>
        /// <returns>二进制数字符串。</returns>
        public static string GetBinaryBy(int n)
        {
            return Convert.ToString(n, 2);
        }
        /// <summary>
        /// 根据十进制数获取八进制数。[10]->[8]
        /// </summary>
        /// <param name="n">十进制数。</param>
        /// <returns>八进制数字符串。</returns>
        public static string GetOctonaryBy(int n)
        {
            return Convert.ToString(n, 8);
        }
        /// <summary>
        /// 根据十进制数获取十六进制数。[10]->[16]
        /// </summary>
        /// <param name="n">十进制数。</param>
        /// <returns>十六进制数字符串</returns>
        public static string GetHexBy(int n)
        {
            return Convert.ToString(n, 16);
        }
        #endregion
        public static bool CreateDirectory(string DirectoryPath)
        {
            if (!Directory.Exists(DirectoryPath))
            {
                Directory.CreateDirectory(DirectoryPath);
                return true;//目录创建完成
            }
            else return false;//目录已存在
        }
        
    }
}
