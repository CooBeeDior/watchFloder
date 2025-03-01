using Guru.DependencyInjection;
using Guru.Executable;
using Guru.ExtensionMethod;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Windows.Forms;

namespace MyWinFormsApp
{
    public partial class WatchForm : Form
    {
        private ContextMenuStrip contextMenuStrip;
        public WatchForm()
        {
            InitializeComponent();

            // 创建右键菜单
            contextMenuStrip = new ContextMenuStrip();
            // 添加菜单项
            ToolStripMenuItem showMenuItem = new ToolStripMenuItem("显示");
            showMenuItem.Name = "show";
            showMenuItem.Click += ShowMenuItem_Click;
            contextMenuStrip.Items.Add(showMenuItem);


            ToolStripMenuItem stateMenuItem = new ToolStripMenuItem("监控状态");
            stateMenuItem.Name = "state";
            stateMenuItem.Text = "监控状态：关闭";
            stateMenuItem.BackColor = Color.Red;
            stateMenuItem.ForeColor = Color.White;
            stateMenuItem.Enabled = false;
            contextMenuStrip.Items.Add(stateMenuItem);

            ToolStripMenuItem actionMenuItem = new ToolStripMenuItem("开始文件夹同步监控");
            actionMenuItem.Name = "action";
            actionMenuItem.Click += ActionItem_Click;
            contextMenuStrip.Items.Add(actionMenuItem);

            ToolStripMenuItem exitMenuItem = new ToolStripMenuItem("退出");
            exitMenuItem.Name = "exit";
            exitMenuItem.Click += ExitMenuItem_Click;
            contextMenuStrip.Items.Add(exitMenuItem);


            // 将右键菜单关联到通知图标
            notifyIcon1.ContextMenuStrip = contextMenuStrip;

            // 订阅窗体关闭事件
            this.FormClosing += WatchForm_FormClosing;

            init();
        }

        private void init()
        {
            string path = "app.json";
            if (File.Exists(path))
            {
                var json = File.ReadAllText(path);
                var app = JsonConvert.DeserializeObject<App>(json);
                if (app != null && !string.IsNullOrWhiteSpace(app.OriginFloder) && !string.IsNullOrWhiteSpace(app.TargetFloder))
                {
                    bool isExsitFloder = true;
                    if (Directory.Exists(textBox1.Text))
                    {
                        textBox1.Text = app.OriginFloder;
                    }
                    else
                    {
                        isExsitFloder = false;
                    }

                    if (Directory.Exists(textBox2.Text))
                    {
                        textBox2.Text = app.TargetFloder;
                    }
                    else
                    {
                        isExsitFloder = false;
                    }
                    if (isExsitFloder)
                    {
                        startWatch();
                    }

                }
            }


        }


        private void button2_Click(object sender, EventArgs e)
        {
            if (is_start)
            {
                MessageBox.Show($"监控服务已启动，请先关闭服务");
                return;
            }
            // 创建FolderBrowserDialog对象
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();

            // 设置对话框的描述信息
            folderBrowserDialog.Description = "请选择一个文件夹";

            // 显示对话框并获取用户的选择结果
            DialogResult result = folderBrowserDialog.ShowDialog();

            // 处理用户的选择结果
            if (result == DialogResult.OK)
            {
                // 获取用户选择的文件夹路径
                string selectedFolder = folderBrowserDialog.SelectedPath;
                // 显示用户选择的文件夹路径
                textBox1.Text = selectedFolder;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (is_start)
            {
                MessageBox.Show($"监控服务已启动，请先关闭服务");
                return;
            }
            // 创建FolderBrowserDialog对象
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();

            // 设置对话框的描述信息
            folderBrowserDialog.Description = "请选择一个文件夹";

            // 显示对话框并获取用户的选择结果
            DialogResult result = folderBrowserDialog.ShowDialog();

            // 处理用户的选择结果
            if (result == DialogResult.OK)
            {
                // 获取用户选择的文件夹路径
                string selectedFolder = folderBrowserDialog.SelectedPath;

                // 显示用户选择的文件夹路径
                textBox2.Text = selectedFolder;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

            startWatch();

        }
        private void startWatch()
        {
            if (is_start)
            {
                MessageBox.Show($"监控服务已启动");
                return;
            }
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show($"请选择源文件夹", "请选择源文件夹", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(textBox2.Text))
            {
                MessageBox.Show($"请选择目标文件夹", "请选择目标文件夹", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (checkBox1.Checked)
            {
                AddLog("同步文件夹开始");
                SyncFolders(textBox1.Text, textBox2.Text);
                AddLog("同步文件夹结束");
            }

            string[] args = new string[2] { textBox1.Text, textBox2.Text };


            label3.ForeColor = Color.Green;
            is_start = true;
            already_monitor();

            App app = new App()
            {
                OriginFloder = textBox1.Text,
                TargetFloder = textBox2.Text,
            };
            var json = JsonConvert.SerializeObject(app);
            string path = "app.json";
            File.WriteAllText(path, json);
            var context = DependencyContainer.Resolve<IContext>();
            context.Source = textBox1.Text;
            context.Target = textBox2.Text;
            _watchFolder = new WatchFolder("", null);
            _watchFolder.AddCommonAction(AddLog);
            Task.Run(() => ConsoleAppInstance.Default.Run(args, true));
            label3.Text = "已经监控";

            var actionToolStripMenuItem = contextMenuStrip.Items["action"];
            actionToolStripMenuItem.Text = "关闭文件夹同步监控";

            var stateToolStripMenuItem = contextMenuStrip.Items["state"];
            stateToolStripMenuItem.Text = "监控状态：开启";
            stateToolStripMenuItem.BackColor = Color.Green;
            stateToolStripMenuItem.ForeColor = Color.White;
        }

        private WatchFolder _watchFolder;
        private bool is_start = false;
        private void button4_Click(object sender, EventArgs e)
        {
            stopWatch();

        }

        private void stopWatch()
        {
            if (!is_start)
            {
                MessageBox.Show($"监控服务已关闭");
                return;
            }  // 显示确认对话框
            DialogResult result = MessageBox.Show("你确定要执行这个操作吗？", "确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            // 根据用户的选择进行相应的操作
            if (result == DialogResult.Yes)
            {
                _watchFolder.Dispose();
                is_start = false;
                no_monitor();

                var actionToolStripMenuItem = contextMenuStrip.Items["action"];
                actionToolStripMenuItem.Text = "开始文件夹同步监控";

                var stateToolStripMenuItem = contextMenuStrip.Items["state"];
                stateToolStripMenuItem.Text = "监控状态：关闭";
                stateToolStripMenuItem.BackColor = Color.Red;
                stateToolStripMenuItem.ForeColor = Color.White;

            }
            else
            {
                // 用户点击了“否”，不执行操作或执行其他逻辑

            }
        }
        private void already_monitor()
        {
            label3.Text = "已监控";
            label3.ForeColor = Color.Green;
        }

        private void no_monitor()
        {
            label3.Text = "未监控";
            label3.ForeColor = Color.Red;
        }

        private MessageBoxOptions options()
        {
            // 获取当前Form的屏幕坐标
            var formLocation = this.PointToScreen(Point.Empty);

            // 获取当前Form的大小
            var formSize = this.Size;

            // 计算消息框应该出现的位置
            int x = formLocation.X + (formSize.Width / 2) - (this.Width / 2);
            int y = formLocation.Y + (formSize.Height / 2) - (this.Height / 2);

            // 创建一个新的MessageBoxOptions对象，并设置消息框的位置
            MessageBoxOptions options = MessageBoxOptions.DefaultDesktopOnly | MessageBoxOptions.ServiceNotification;
            options |= (MessageBoxOptions)(x << 16 | y);
            return options;
        }

        private void SyncFolders(string source, string target)
        {

            // 创建目标文件夹（如果不存在）
            if (!Directory.Exists(target))
            {
                Directory.CreateDirectory(target);
            }

            // 同步文件
            string[] sourceFiles = Directory.GetFiles(source);
            foreach (string sourceFile in sourceFiles)
            {
                string fileName = Path.GetFileName(sourceFile);
                string targetFile = Path.Combine(target, fileName);

                if (!File.Exists(targetFile) || File.GetLastWriteTime(sourceFile) > File.GetLastWriteTime(targetFile))
                {
                    File.Copy(sourceFile, targetFile, true);
                    AddLog($"创建文件：{targetFile}");
                }
            }

            // 同步子文件夹
            string[] sourceSubFolders = Directory.GetDirectories(source);
            foreach (string sourceSubFolder in sourceSubFolders)
            {
                string folderName = Path.GetFileName(sourceSubFolder);
                string targetSubFolder = Path.Combine(target, folderName);

                SyncFolders(sourceSubFolder, targetSubFolder);
            }

            // 删除目标文件夹中多余的文件和文件夹
            string[] targetFiles = Directory.GetFiles(target);
            foreach (string targetFile in targetFiles)
            {
                string fileName = Path.GetFileName(targetFile);
                string sourceFile = Path.Combine(source, fileName);

                if (!File.Exists(sourceFile))
                {
                    File.Delete(targetFile);
                    AddLog($"删除文件：{targetFile}");
                }
            }

            string[] targetSubFolders = Directory.GetDirectories(target);
            foreach (string targetSubFolder in targetSubFolders)
            {
                string folderName = Path.GetFileName(targetSubFolder);
                string sourceSubFolder = Path.Combine(source, folderName);

                if (!Directory.Exists(sourceSubFolder))
                {
                    Directory.Delete(targetSubFolder, true);
                    AddLog($"删除文件夹：{targetSubFolder}");
                }
            }

        }

        private void AddLog(string logMessage)
        {
            if (listBox1.InvokeRequired)
            {
                // 如果需要封送调用
                listBox1.Invoke(new Action<string>(AddLog), logMessage);
            }
            else
            {
                // 在创建控件的线程上执行操作
                listBox1.Items.Add(logMessage);
                listBox1.SelectedIndex = listBox1.Items.Count - 1;
            }
        }

        private void WatchForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                if (!isExiting)
                {
                    e.Cancel = true;
                    this.Hide();
                    notifyIcon1.Visible = true;
                }
                else
                {
                    notifyIcon1.Visible = false; // 关闭通知图标
                }
            }
        }
        private bool isExiting { get; set; }
        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            // 显示窗体
            this.Show();
            // 激活窗体
            this.Activate();
            //// 隐藏通知图标
            //notifyIcon1.Visible = false;
        }
        // 添加一个方法用于设置真正退出标志并关闭窗体
        private void ExitApplication()
        {
            isExiting = true;
            this.Close();
        }
        private void ShowMenuItem_Click(object sender, EventArgs e)
        {
            this.Show();
            this.Activate();
            //notifyIcon1.Visible = false;
        }


        private void ActionItem_Click(object sender, EventArgs e)
        {
            if (is_start)
            {
                stopWatch();

            }
            else
            {
                startWatch();
            }
        }



        private void ExitMenuItem_Click(object sender, EventArgs e)
        {
            notifyIcon1.Visible = false;
            Application.Exit();
        }
        // 假设你有一个菜单项或其他方式来触发真正退出
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExitApplication();
        }

        private void textBox1_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(textBox1.Text))
            {
                if (Directory.Exists(textBox1.Text))
                {
                    try
                    {
                        Process.Start("explorer.exe", textBox1.Text);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"打开文件夹失败: {ex.Message}");
                    }
                }
            }
        }

        private void textBox2_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(textBox2.Text))
            {
                if (Directory.Exists(textBox2.Text))
                {
                    try
                    {
                        Process.Start("explorer.exe", textBox2.Text);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"打开文件夹失败: {ex.Message}");
                    }
                }
            }

        }
 
    }
}
