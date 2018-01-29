namespace FastCmd
{
    partial class Main
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.命令记录ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.重载记录ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.清空窗口ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.执行命令ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(0, 67);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(527, 543);
            this.richTextBox1.TabIndex = 2;
            this.richTextBox1.Text = "";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(0, 32);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(525, 29);
            this.textBox1.TabIndex = 1;
            this.textBox1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox1_KeyPress);
            // 
            // listBox1
            // 
            this.listBox1.Dock = System.Windows.Forms.DockStyle.Right;
            this.listBox1.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 25;
            this.listBox1.Location = new System.Drawing.Point(531, 29);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(482, 581);
            this.listBox1.TabIndex = 3;
            this.listBox1.Click += new System.EventHandler(this.listBox1_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.命令记录ToolStripMenuItem,
            this.重载记录ToolStripMenuItem,
            this.清空窗口ToolStripMenuItem,
            this.执行命令ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1013, 29);
            this.menuStrip1.TabIndex = 4;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 命令记录ToolStripMenuItem
            // 
            this.命令记录ToolStripMenuItem.Name = "命令记录ToolStripMenuItem";
            this.命令记录ToolStripMenuItem.Size = new System.Drawing.Size(86, 25);
            this.命令记录ToolStripMenuItem.Text = "命令记录";
            this.命令记录ToolStripMenuItem.Click += new System.EventHandler(this.命令记录ToolStripMenuItem_Click);
            // 
            // 重载记录ToolStripMenuItem
            // 
            this.重载记录ToolStripMenuItem.Name = "重载记录ToolStripMenuItem";
            this.重载记录ToolStripMenuItem.Size = new System.Drawing.Size(86, 25);
            this.重载记录ToolStripMenuItem.Text = "重载列表";
            this.重载记录ToolStripMenuItem.Click += new System.EventHandler(this.重载记录ToolStripMenuItem_Click);
            // 
            // 清空窗口ToolStripMenuItem
            // 
            this.清空窗口ToolStripMenuItem.Name = "清空窗口ToolStripMenuItem";
            this.清空窗口ToolStripMenuItem.Size = new System.Drawing.Size(107, 25);
            this.清空窗口ToolStripMenuItem.Text = "清空窗口(&C)";
            this.清空窗口ToolStripMenuItem.Click += new System.EventHandler(this.清空窗口ToolStripMenuItem_Click);
            // 
            // 执行命令ToolStripMenuItem
            // 
            this.执行命令ToolStripMenuItem.Name = "执行命令ToolStripMenuItem";
            this.执行命令ToolStripMenuItem.Size = new System.Drawing.Size(106, 25);
            this.执行命令ToolStripMenuItem.Text = "执行命令(&X)";
            this.执行命令ToolStripMenuItem.Click += new System.EventHandler(this.执行命令ToolStripMenuItem_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1013, 610);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.menuStrip1);
            this.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "Main";
            this.Text = "Form1";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 清空窗口ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 命令记录ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 重载记录ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 执行命令ToolStripMenuItem;

    }
}

