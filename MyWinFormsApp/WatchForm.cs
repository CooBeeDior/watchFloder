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

            // �����Ҽ��˵�
            contextMenuStrip = new ContextMenuStrip();
            // ��Ӳ˵���
            ToolStripMenuItem showMenuItem = new ToolStripMenuItem("��ʾ");
            showMenuItem.Name = "show";
            showMenuItem.Click += ShowMenuItem_Click;
            contextMenuStrip.Items.Add(showMenuItem);


            ToolStripMenuItem stateMenuItem = new ToolStripMenuItem("���״̬");
            stateMenuItem.Name = "state";
            stateMenuItem.Text = "���״̬���ر�";
            stateMenuItem.BackColor = Color.Red;
            stateMenuItem.ForeColor = Color.White;
            stateMenuItem.Enabled = false;
            contextMenuStrip.Items.Add(stateMenuItem);

            ToolStripMenuItem actionMenuItem = new ToolStripMenuItem("��ʼ�ļ���ͬ�����");
            actionMenuItem.Name = "action";
            actionMenuItem.Click += ActionItem_Click;
            contextMenuStrip.Items.Add(actionMenuItem);

            ToolStripMenuItem exitMenuItem = new ToolStripMenuItem("�˳�");
            exitMenuItem.Name = "exit";
            exitMenuItem.Click += ExitMenuItem_Click;
            contextMenuStrip.Items.Add(exitMenuItem);


            // ���Ҽ��˵�������֪ͨͼ��
            notifyIcon1.ContextMenuStrip = contextMenuStrip;

            // ���Ĵ���ر��¼�
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
                MessageBox.Show($"��ط��������������ȹرշ���");
                return;
            }
            // ����FolderBrowserDialog����
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();

            // ���öԻ����������Ϣ
            folderBrowserDialog.Description = "��ѡ��һ���ļ���";

            // ��ʾ�Ի��򲢻�ȡ�û���ѡ����
            DialogResult result = folderBrowserDialog.ShowDialog();

            // �����û���ѡ����
            if (result == DialogResult.OK)
            {
                // ��ȡ�û�ѡ����ļ���·��
                string selectedFolder = folderBrowserDialog.SelectedPath;
                // ��ʾ�û�ѡ����ļ���·��
                textBox1.Text = selectedFolder;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (is_start)
            {
                MessageBox.Show($"��ط��������������ȹرշ���");
                return;
            }
            // ����FolderBrowserDialog����
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();

            // ���öԻ����������Ϣ
            folderBrowserDialog.Description = "��ѡ��һ���ļ���";

            // ��ʾ�Ի��򲢻�ȡ�û���ѡ����
            DialogResult result = folderBrowserDialog.ShowDialog();

            // �����û���ѡ����
            if (result == DialogResult.OK)
            {
                // ��ȡ�û�ѡ����ļ���·��
                string selectedFolder = folderBrowserDialog.SelectedPath;

                // ��ʾ�û�ѡ����ļ���·��
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
                MessageBox.Show($"��ط���������");
                return;
            }
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show($"��ѡ��Դ�ļ���", "��ѡ��Դ�ļ���", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(textBox2.Text))
            {
                MessageBox.Show($"��ѡ��Ŀ���ļ���", "��ѡ��Ŀ���ļ���", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (checkBox1.Checked)
            {
                AddLog("ͬ���ļ��п�ʼ");
                SyncFolders(textBox1.Text, textBox2.Text);
                AddLog("ͬ���ļ��н���");
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
            label3.Text = "�Ѿ����";

            var actionToolStripMenuItem = contextMenuStrip.Items["action"];
            actionToolStripMenuItem.Text = "�ر��ļ���ͬ�����";

            var stateToolStripMenuItem = contextMenuStrip.Items["state"];
            stateToolStripMenuItem.Text = "���״̬������";
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
                MessageBox.Show($"��ط����ѹر�");
                return;
            }  // ��ʾȷ�϶Ի���
            DialogResult result = MessageBox.Show("��ȷ��Ҫִ�����������", "ȷ��", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            // �����û���ѡ�������Ӧ�Ĳ���
            if (result == DialogResult.Yes)
            {
                _watchFolder.Dispose();
                is_start = false;
                no_monitor();

                var actionToolStripMenuItem = contextMenuStrip.Items["action"];
                actionToolStripMenuItem.Text = "��ʼ�ļ���ͬ�����";

                var stateToolStripMenuItem = contextMenuStrip.Items["state"];
                stateToolStripMenuItem.Text = "���״̬���ر�";
                stateToolStripMenuItem.BackColor = Color.Red;
                stateToolStripMenuItem.ForeColor = Color.White;

            }
            else
            {
                // �û�����ˡ��񡱣���ִ�в�����ִ�������߼�

            }
        }
        private void already_monitor()
        {
            label3.Text = "�Ѽ��";
            label3.ForeColor = Color.Green;
        }

        private void no_monitor()
        {
            label3.Text = "δ���";
            label3.ForeColor = Color.Red;
        }

        private MessageBoxOptions options()
        {
            // ��ȡ��ǰForm����Ļ����
            var formLocation = this.PointToScreen(Point.Empty);

            // ��ȡ��ǰForm�Ĵ�С
            var formSize = this.Size;

            // ������Ϣ��Ӧ�ó��ֵ�λ��
            int x = formLocation.X + (formSize.Width / 2) - (this.Width / 2);
            int y = formLocation.Y + (formSize.Height / 2) - (this.Height / 2);

            // ����һ���µ�MessageBoxOptions���󣬲�������Ϣ���λ��
            MessageBoxOptions options = MessageBoxOptions.DefaultDesktopOnly | MessageBoxOptions.ServiceNotification;
            options |= (MessageBoxOptions)(x << 16 | y);
            return options;
        }

        private void SyncFolders(string source, string target)
        {

            // ����Ŀ���ļ��У���������ڣ�
            if (!Directory.Exists(target))
            {
                Directory.CreateDirectory(target);
            }

            // ͬ���ļ�
            string[] sourceFiles = Directory.GetFiles(source);
            foreach (string sourceFile in sourceFiles)
            {
                string fileName = Path.GetFileName(sourceFile);
                string targetFile = Path.Combine(target, fileName);

                if (!File.Exists(targetFile) || File.GetLastWriteTime(sourceFile) > File.GetLastWriteTime(targetFile))
                {
                    File.Copy(sourceFile, targetFile, true);
                    AddLog($"�����ļ���{targetFile}");
                }
            }

            // ͬ�����ļ���
            string[] sourceSubFolders = Directory.GetDirectories(source);
            foreach (string sourceSubFolder in sourceSubFolders)
            {
                string folderName = Path.GetFileName(sourceSubFolder);
                string targetSubFolder = Path.Combine(target, folderName);

                SyncFolders(sourceSubFolder, targetSubFolder);
            }

            // ɾ��Ŀ���ļ����ж�����ļ����ļ���
            string[] targetFiles = Directory.GetFiles(target);
            foreach (string targetFile in targetFiles)
            {
                string fileName = Path.GetFileName(targetFile);
                string sourceFile = Path.Combine(source, fileName);

                if (!File.Exists(sourceFile))
                {
                    File.Delete(targetFile);
                    AddLog($"ɾ���ļ���{targetFile}");
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
                    AddLog($"ɾ���ļ��У�{targetSubFolder}");
                }
            }

        }

        private void AddLog(string logMessage)
        {
            if (listBox1.InvokeRequired)
            {
                // �����Ҫ���͵���
                listBox1.Invoke(new Action<string>(AddLog), logMessage);
            }
            else
            {
                // �ڴ����ؼ����߳���ִ�в���
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
                    notifyIcon1.Visible = false; // �ر�֪ͨͼ��
                }
            }
        }
        private bool isExiting { get; set; }
        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            // ��ʾ����
            this.Show();
            // �����
            this.Activate();
            //// ����֪ͨͼ��
            //notifyIcon1.Visible = false;
        }
        // ���һ�������������������˳���־���رմ���
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
        // ��������һ���˵����������ʽ�����������˳�
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
                        MessageBox.Show($"���ļ���ʧ��: {ex.Message}");
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
                        MessageBox.Show($"���ļ���ʧ��: {ex.Message}");
                    }
                }
            }

        }
 
    }
}
