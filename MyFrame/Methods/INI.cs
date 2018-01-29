using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace MyFrame
{
    public class INI
    {
        static string INIPath;
        static StringBuilder result;
        static string temp;
        static INI()
        {
            INIPath = Root + @"\Backup\Configuration.ini";
            result = new StringBuilder();
        }
        #region 其他封装
        public static string Root
        {
            get { return Environment.CurrentDirectory; }
        }
        public static void OpenRoot()
        {
            MyProcess.Run(Root);
        }
        public static void OpenConfig()
        {
            MyProcess.Run(INIPath);
        }
        #endregion
        public static string GetLocationID()
        {
            return Get("Basic", "LocationID");
        }
        public static void SetLocationID(object value)
        {
            Set("Basic", "LocationID", value);
        }
        public static void SetDBName(object Value)
        {
            Set("Basic", "DBName", Value);
        }
        public static string GetDBName()
        {
            return Get("Basic", "DBName");
        }
        public static double GetPerPage()
        {
            temp = Get("Diary", "PerPage");
            if (temp == "ERROR") return 0;
            else return double.Parse(temp);
        }
        public static void SetPerPage(object value)
        {
            Set("Diary", "PerPage", value);
        }
        #region 阅读器
        public static string[] GetModeNames()
        {
            string[] names = { "", "", "", "", "" };
            names.SetValue(Get("ReaderMode1", "Name"), 0);
            names.SetValue(Get("ReaderMode2", "Name"), 1);
            names.SetValue(Get("ReaderMode3", "Name"), 2);
            names.SetValue(Get("ReaderMode4", "Name"), 3);
            names.SetValue(Get("ReaderMode5", "Name"), 4);
            return names;
        }
        public static int GetFinalMode()
        {
            temp = Get("ReaderFinalState", "FinalMode");
            if (temp == "Default") return 0;
            else return Convert.ToInt32(temp);
        }
        public static string GetFinalWindowState()
        {
            temp = Get("ReaderFinalState", "FinalWindowState");
            if (temp == "Default") temp = "Normal";
            return temp;
        }
        public static int[] GetFinalWindowSize()
        {
            int[] size = { 0, 0 };
            temp = Get("ReaderFinalState", "FinalWindowWidth");
            if (temp == "Default") temp = "500";
            size.SetValue(Convert.ToInt32(temp), 0);
            temp = Get("ReaderFinalState", "FinalWindowHeight");
            if (temp == "Default") temp = "500";
            size.SetValue(Convert.ToInt32(temp), 1);
            return size;
        }
        public static string[] GetModeFont(int modeNum)
        {
            string[] values = { "", "", "" };
            temp = Get("ReaderMode" + modeNum, "FontFamily");
            if (temp == "Default") temp = "微软雅黑";
            values.SetValue(temp, 0);
            temp = Get("ReaderMode" + modeNum, "FontSize");
            if (temp == "Default") temp = "10";
            values.SetValue(temp, 1);
            temp = Get("ReaderMode" + modeNum, "Bold");
            if (temp == "Default") temp = "False";
            values.SetValue(temp, 2);
            return values;
        }
        public static string GetModeColor(int modeNum)
        {
            temp = Get("ReaderMode" + modeNum, "FontColor");
            if (temp == "Default") temp = "-16777216";
            return temp;
        }
        public static void SetFinalMode(string value)
        {
            Set("ReaderFinalState", "FinalMode", value);
        }
        public static void SetFinalWindowState(string value)
        {
            Set("ReaderFinalState", "FinalWindowState", value);
        }
        public static void SetFinalWindowSize(int width, int height)
        {
            Set("ReaderFinalState", "FinalWindowWidth", width.ToString());
            Set("ReaderFinalState", "FinalWindowHeight", height.ToString());
        }
        public static void SetModeFont(int modeNum, string[] values)
        {
            Set("ReaderMode" + modeNum, "Bold", values[0]);
            Set("ReaderMode" + modeNum, "FontFamily", values[1]);
            Set("ReaderMode" + modeNum, "FontSize", values[2]);
        }
        public static void SetModeColor(int modeNum, string color)
        {
            Set("ReaderMode" + modeNum, "FontColor", color);
        }
        #endregion
        public static void SetXXX(object Value)
        {
            Set("Basic", "XXX", Value);
        }
        public static string GetXXX()
        {
            return Get("Basic", "XXX");
        }
        #region 二次封装
        public static void Set(string section, string key, object value)
        {
            WritePrivateProfileString(section, key, value.ToString(), INIPath);
        }
        public static string Get(string section, string key)
        {
            GetPrivateProfileString(section, key, "Not Found", result, 34, INIPath);//最大只能读34位字符
            temp = result.ToString();
            if (temp.Equals("") || temp.Equals("Not Found"))
            {
                if (temp == "Not Found")
                    Console.WriteLine("意外：请检查配置文件是否存在，或配置中是否存在指定的键。[{0}]-{1}", section, key);
                if (temp == "")
                    Console.WriteLine("意外：配置文件中的指定键值为空。[{0}]-{1}", section, key);
                temp = "ERROR";//两种情况的默认返回值
            }
            return temp;
        }
        #endregion
        #region 原封装引用
        /// <summary>
        /// 向指定配置文件的指定块写入指定键的值。若配置文件不存在，则创建文件并添加指定的键。
        /// </summary>
        /// <param name="section">块</param>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="INIPath">路径</param>
        /// <returns>非零表示成功，零表示失败（类型可以是任意整数类型，除了sbyte）</returns>
        [DllImport("kernel32")]
        static extern byte WritePrivateProfileString(string section, string key, string value, string INIPath);
        /// <summary>
        /// 从指定配置文件的指定块读取指定键的值。
        /// </summary>
        /// <param name="section">块</param>
        /// <param name="key">键</param>
        /// <param name="default_value">默认值</param>
        /// <param name="result">结果（引用类型）</param>
        /// <param name="size">目的缓存器的大小</param>
        /// <param name="INIPath">路径</param>
        /// <returns>string的长度（类型可以是任意整数类型）</returns>
        [DllImport("kernel32")]
        static extern sbyte GetPrivateProfileString(string section, string key, string default_value, StringBuilder result, int size, string INIPath);
        #endregion
    }

}
