using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace MyFrame
{
    public class MyStyleDialog
    {
        Control Control;
        FontDialog Fonts;
        ColorDialog Colors;
        public MyStyleDialog(Control Control)
        {
            this.Control = Control;
            Fonts = new FontDialog();
            Colors = new ColorDialog();
        }
        public Font Font { get { return Control.Font; } }
        public Color Color { get { return Control.ForeColor; } }
        public void ShowFonts()
        {
            Fonts.Font = Control.Font;//前设定
            Fonts.ShowDialog();//选择
            Control.Font = Fonts.Font;//应用
            Fonts.Font = Control.Font;//保存
        }
        public void ShowColors()
        {
            Colors.Color = Control.ForeColor;//前设定
            Colors.ShowDialog();//选择
            Control.ForeColor = Colors.Color;//应用
            Colors.Color = Control.ForeColor;//保存
        }
    }
}
