using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace MyFrame
{
    public class MyError
    {
        static ErrorProvider provider;
        static ArrayList record;
        static MyError()
        {
            provider = new ErrorProvider();
            provider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
            record = new ArrayList();
        }
        public static void Set(Control control, string Info)
        {
            provider.SetError(control, Info);
            provider.SetIconAlignment(control, ErrorIconAlignment.MiddleRight);
            provider.SetIconPadding(control, 4);
            record.Add(control);
        }
        public static void Clear(Control control)
        {
            provider.SetError(control, "");
            record.Remove(control);
        }
        public static string Get(Control control)
        {
            return provider.GetError(control);
        }
        public static bool Check(Control control)
        {
            return Get(control) == "";
        }
        public static bool CheckAll()
        {
            return record.Count == 0;
        }
        public static void NewRecord()//使用MyError进行新一轮的判断前执行
        {
            while (record.Count != 0)
                record.RemoveAt(0);
        }
    }
}
