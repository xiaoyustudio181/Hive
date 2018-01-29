using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace TouchTypist
{
    public partial class Game : Form
    {
        char[] chs1 = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', ' ' };
        char[] chs2 = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', '`', '~', '!', '@', '#', '$', '%', '^', '&', '*', '(', ')', '-', '=', '_', '+', '[', '{', ']', '}', ':', ';', '\'', '\"', '\\', '|', ',', '<', '.', '>', '/', '?', ' ' };
        List<Label> ChLabels;
        Hashtable Keyboard=new Hashtable();
        Random random = new Random();
        bool Mode = true;//默认天堂模式
        int score = 0, turn = 1;
        //Thread TimeThread;
        System.Windows.Forms.Timer timer;
        string previous_pressed,previous_answer;
        Color KeyboardColor;
        public Game()
        {
            InitializeComponent();
            ChLabels = new List<Label>() { label1, label2, label3, label4, label5, label6, label7, label8 };
            CollectKeys();
            timer = new System.Windows.Forms.Timer();
            timer.Interval = 1;
            timer.Tick += timer_Tick;
        }

        DateTime total = new DateTime(1, 1, 1, 0, 1, 0), StartTime;
        double proportion;
        long ticks;
        void timer_Tick(object sender, EventArgs e)
        {
            ticks = (DateTime.Now - StartTime).Ticks;
            proportion = Convert.ToDouble(ticks) / Convert.ToDouble(total.Ticks);
            try
            {
                progressBar1.Value = 100 - Convert.ToInt32(proportion * 100);
            }
            catch
            {
                progressBar1.Value = 0;
                GameOver();
            }
        }
        private void CollectKeys()
        {
            Keyboard.Add("1", label10);
            Keyboard.Add("!", label10);
            Keyboard.Add("2", label11);
            Keyboard.Add("@", label11);
            Keyboard.Add("3", label12);
            Keyboard.Add("#", label12);
            Keyboard.Add("4", label13);
            Keyboard.Add("$", label13);
            Keyboard.Add("5", label14);
            Keyboard.Add("%", label14);
            Keyboard.Add("6", label15);
            Keyboard.Add("^", label15);
            Keyboard.Add("7", label16);
            Keyboard.Add("&&", label16);
            Keyboard.Add("8", label17);
            Keyboard.Add("*", label17);
            Keyboard.Add("9", label18);
            Keyboard.Add("(", label18);
            Keyboard.Add("0", label19);
            Keyboard.Add(")", label19);

            Keyboard.Add("q", label20);
            Keyboard.Add("Q", label20);
            Keyboard.Add("w", label21);
            Keyboard.Add("W", label21);
            Keyboard.Add("e", label22);
            Keyboard.Add("E", label22);
            Keyboard.Add("r", label23);
            Keyboard.Add("R", label23);
            Keyboard.Add("t", label24);
            Keyboard.Add("T", label24);
            Keyboard.Add("y", label25);
            Keyboard.Add("Y", label25);
            Keyboard.Add("u", label26);
            Keyboard.Add("U", label26);
            Keyboard.Add("i", label27);
            Keyboard.Add("I", label27);
            Keyboard.Add("o", label28);
            Keyboard.Add("O", label28);
            Keyboard.Add("p", label29);
            Keyboard.Add("P", label29);
            Keyboard.Add("[", label30);
            Keyboard.Add("{", label30);
            Keyboard.Add("]", label31);
            Keyboard.Add("}", label31);


            Keyboard.Add("a", label32);
            Keyboard.Add("A", label32);
            Keyboard.Add("s", label33);
            Keyboard.Add("S", label33);
            Keyboard.Add("d", label34);
            Keyboard.Add("D", label34);
            Keyboard.Add("f", label35);
            Keyboard.Add("F", label35);
            Keyboard.Add("g", label36);
            Keyboard.Add("G", label36);
            Keyboard.Add("h", label37);
            Keyboard.Add("H", label37);
            Keyboard.Add("j", label38);
            Keyboard.Add("J", label38);
            Keyboard.Add("k", label39);
            Keyboard.Add("K", label39);
            Keyboard.Add("l", label40);
            Keyboard.Add("L", label40);
            Keyboard.Add(";", label41);
            Keyboard.Add(":", label41);
            Keyboard.Add("'", label42);
            Keyboard.Add("\"", label42);
            Keyboard.Add("\\", label43);
            Keyboard.Add("|", label43);

            Keyboard.Add("z", label44);
            Keyboard.Add("Z", label44);
            Keyboard.Add("x", label45);
            Keyboard.Add("X", label45);
            Keyboard.Add("c", label46);
            Keyboard.Add("C", label46);
            Keyboard.Add("v", label47);
            Keyboard.Add("V", label47);
            Keyboard.Add("b", label48);
            Keyboard.Add("B", label48);
            Keyboard.Add("n", label49);
            Keyboard.Add("N", label49);
            Keyboard.Add("m", label50);
            Keyboard.Add("M", label50);
            Keyboard.Add(",", label51);
            Keyboard.Add("<", label51);
            Keyboard.Add(".", label52);
            Keyboard.Add(">", label52);
            Keyboard.Add("/", label53);
            Keyboard.Add("?", label53);
            Keyboard.Add(" ", label54);

            Keyboard.Add("=", label55);
            Keyboard.Add("+", label55);
            Keyboard.Add("-", label56);
            Keyboard.Add("_", label56);
            Keyboard.Add("`", label57);
            Keyboard.Add("~", label57);
        }
        private void Game_Load(object sender, EventArgs e)
        {
            Start();
        }
        private void Start()
        {
            progressBar1.Value = 100;
            KeyboardColor = Mode ? Color.Snow : Color.LightGray;
            foreach (DictionaryEntry item in Keyboard)
            {//键盘颜色
                (item.Value as Label).BackColor = KeyboardColor;
            }
            if (Mode)
            {
                toolStripStatusLabel1.Text = "天堂模式";
                toolStripStatusLabel1.ForeColor = Color.BlueViolet;
                天堂模式HToolStripMenuItem.BackColor = Color.LightGray;
                地狱模式2ToolStripMenuItem.BackColor = Color.White;
            }
            else
            {
                toolStripStatusLabel1.Text = "地狱模式";
                toolStripStatusLabel1.ForeColor = Color.DarkRed;
                天堂模式HToolStripMenuItem.BackColor = Color.White;
                地狱模式2ToolStripMenuItem.BackColor = Color.LightGray;
            }
            score = 0;
            toolStripStatusLabel2.Text = "得分：" + score;
            turn = 1;
            LoadChs();
            //TimeThread = new Thread(new ThreadStart(TimeThreadMethod));
            //TimeThread.Start();
            StartTime = DateTime.Now;
            timer.Start();
            if (Mode) prompt();
        }
        private void TimeThreadMethod()
        {
            DateTime total = new DateTime(1, 1, 1, 0, 1, 0), StartTime = DateTime.Now;
            double proportion;
            long ticks;
            while (true)
            {
                ticks = (DateTime.Now - StartTime).Ticks;
                proportion = Convert.ToDouble(ticks) / Convert.ToDouble(total.Ticks);
                this.Invoke(new EventHandler(delegate
                {
                    try
                    {
                        progressBar1.Value = 100 - Convert.ToInt32(proportion * 100);
                    }
                    catch
                    {
                        progressBar1.Value = 0;
                        GameOver();
                    }
                }));
                Thread.Sleep(500);
            }
        }
        private void prompt()//提示色
        {
            string answer = ChLabels[turn - 1].Text;
            Label label = Keyboard[answer] as Label;
            label.ForeColor = Color.DeepPink;
            previous_answer = answer;
        }
        private void GameOver()
        {
            //TimeThread.Abort();
            timer.Stop();
            if (Mode || !Mode) MessageBox.Show("时间到！本轮得分：" + score + "。", "游戏结束", MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1);
            DialogResult result = MessageBox.Show("是否重新开始？", "选择", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
            if (result.Equals(DialogResult.Yes)) Start();
            else this.Close();
        }
        private void LoadChs()
        {
            char ch;
            foreach (Label each in ChLabels)
            {
                each.BackColor = Mode ? Color.AntiqueWhite : Color.PeachPuff;//原背景色
                ch = Mode ? chs1[random.Next(chs1.Length)] : chs2[random.Next(chs2.Length)];
                each.Text = ch == '&' ? "&&" : ch.ToString();//label标签不显示单个&字符，因此换掉
            }
        }
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            //==测试是否是异常按键
            string pressed = e.KeyChar.ToString().ToLower();
            if (pressed.Equals("&")) pressed = "&&";
            Label TestLabel = Keyboard[pressed] as Label;
            try { Console.WriteLine(TestLabel.Text); }
            catch { return; }
            //==恢复上一个按键的颜色
            if (previous_pressed != null) (Keyboard[previous_pressed] as Label).BackColor = KeyboardColor;
            previous_pressed = pressed;
            //==
            textBox1.Text = "";
            Label label = ChLabels[turn - 1];
            //==
            if (pressed.Equals(label.Text.ToLower()))
            {
                (Keyboard[previous_answer] as Label).ForeColor = Color.Black;//恢复上一提示键的颜色
                //==输入正确的按键色变化
                Label keylabel = Keyboard[pressed] as Label;
                keylabel.BackColor = Color.LightSkyBlue;
                //==卡牌颜色的变化
                label.Text = "";
                label.BackColor = Mode ? Color.LightSeaGreen : Color.LightPink;
                //==
                score++;
                turn++;
                toolStripStatusLabel2.Text = "得分：" + score;
                if (turn == 9)
                {
                    LoadChs();
                    turn = 1;
                }
            }
            else
            {
                //==输入错误的按键色变化
                Label keylabel = Keyboard[pressed] as Label;
                keylabel.BackColor = Color.PaleVioletRed;
                //==
                score--;
                toolStripStatusLabel2.Text = "得分：" + score;
            }
            if (Mode) prompt();
        }
        private void Game_FormClosed(object sender, FormClosedEventArgs e)
        {
            //if (TimeThread.IsAlive) TimeThread.Abort();
            timer.Stop();
        }
        private void 逃离噩梦EToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void 游戏规则RToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("每轮时间：1分钟。\n————————\n勿长时连续看电脑，\n注意适当休息眼睛。", "游戏说明", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        }
        private void 天堂模式HToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //if (TimeThread.IsAlive) TimeThread.Abort();
            timer.Stop();
            Mode = true;
            (Keyboard[previous_answer] as Label).ForeColor = Color.Black;//恢复上一提示键的颜色
            Start();
        }
        private void 地狱模式2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timer.Stop();
            Mode = false;
            (Keyboard[previous_answer] as Label).ForeColor = Color.Black;//恢复上一提示键的颜色
            Start();
        }
    }
}
