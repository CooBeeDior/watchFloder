namespace MyWinFormsApp
{
    partial class WatchForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WatchForm));
            textBox1 = new TextBox();
            textBox2 = new TextBox();
            button1 = new Button();
            checkBox1 = new CheckBox();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            button2 = new Button();
            button3 = new Button();
            button4 = new Button();
            listBox1 = new ListBox();
            notifyIcon1 = new NotifyIcon(components);
            SuspendLayout();
            // 
            // textBox1
            // 
            textBox1.Cursor = Cursors.Hand;
            textBox1.Enabled = false;
            textBox1.Location = new Point(95, 48);
            textBox1.Name = "textBox1";
            textBox1.ReadOnly = true;
            textBox1.Size = new Size(316, 23);
            textBox1.TabIndex = 0;
            textBox1.Click += textBox1_Click;
            // 
            // textBox2
            // 
            textBox2.Enabled = false;
            textBox2.Location = new Point(95, 97);
            textBox2.Name = "textBox2";
            textBox2.ReadOnly = true;
            textBox2.Size = new Size(316, 23);
            textBox2.TabIndex = 1;
            textBox2.Click += textBox2_Click;
            // 
            // button1
            // 
            button1.BackColor = Color.Lime;
            button1.Font = new Font("Microsoft YaHei UI", 10.5F, FontStyle.Bold, GraphicsUnit.Point, 134);
            button1.Location = new Point(53, 298);
            button1.Name = "button1";
            button1.Size = new Size(76, 52);
            button1.TabIndex = 2;
            button1.Text = "开始监控";
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click;
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.Location = new Point(95, 144);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(111, 21);
            checkBox1.TabIndex = 3;
            checkBox1.Text = "是否同步文件夹";
            checkBox1.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 51);
            label1.Name = "label1";
            label1.Size = new Size(68, 17);
            label1.TabIndex = 4;
            label1.Text = "源文件夹：";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 100);
            label2.Name = "label2";
            label2.Size = new Size(80, 17);
            label2.TabIndex = 5;
            label2.Text = "目标文件夹：";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Microsoft YaHei UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 134);
            label3.ForeColor = Color.Red;
            label3.Location = new Point(53, 233);
            label3.Name = "label3";
            label3.Size = new Size(58, 21);
            label3.TabIndex = 6;
            label3.Text = "未监控";
            // 
            // button2
            // 
            button2.Location = new Point(417, 48);
            button2.Name = "button2";
            button2.Size = new Size(42, 23);
            button2.TabIndex = 7;
            button2.Text = "选择文件夹";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // button3
            // 
            button3.Location = new Point(417, 97);
            button3.Name = "button3";
            button3.Size = new Size(42, 23);
            button3.TabIndex = 8;
            button3.Text = "选择文件夹";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // button4
            // 
            button4.BackColor = Color.Red;
            button4.Font = new Font("Microsoft YaHei UI", 10.5F, FontStyle.Bold, GraphicsUnit.Point, 134);
            button4.Location = new Point(152, 298);
            button4.Name = "button4";
            button4.Size = new Size(86, 52);
            button4.TabIndex = 9;
            button4.Text = "停止监控";
            button4.UseVisualStyleBackColor = false;
            button4.Click += button4_Click;
            // 
            // listBox1
            // 
            listBox1.FormattingEnabled = true;
            listBox1.ItemHeight = 17;
            listBox1.Location = new Point(280, 145);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(542, 225);
            listBox1.TabIndex = 10;
            // 
            // notifyIcon1
            // 
            notifyIcon1.Icon = (Icon)resources.GetObject("notifyIcon1.Icon");
            notifyIcon1.Text = "同步文件夹监控";
            notifyIcon1.Visible = true;
            notifyIcon1.MouseDoubleClick += notifyIcon1_MouseDoubleClick;
            // 
            // WatchForm
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(838, 390);
            Controls.Add(listBox1);
            Controls.Add(button4);
            Controls.Add(button3);
            Controls.Add(button2);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(checkBox1);
            Controls.Add(button1);
            Controls.Add(textBox2);
            Controls.Add(textBox1);
            Name = "WatchForm";
            Text = "文件夹监控工具";
            FormClosing += WatchForm_FormClosing;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox textBox1;
        private TextBox textBox2;
        private Button button1;
        private CheckBox checkBox1;
        private Label label1;
        private Label label2;
        private Label label3;
        private Button button2;
        private Button button3;
        private Button button4;
        private ListBox listBox1;
        private NotifyIcon notifyIcon1;
    }
}
