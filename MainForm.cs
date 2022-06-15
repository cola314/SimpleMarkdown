using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using SimpleMarkdown.Properties;
using SimpleMarkdown.Models;
using SimpleMarkdown.Utils;

namespace SimpleMarkdown
{
    public partial class MainForm : Form
    {
        private readonly MarkdownService _markdownService;

        static MainForm()
        {
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
        }

        private const string README_FILE = "Resources\\readme.md";
        
        private bool isTempFile;

        private string currentFilePath_;
        private string CurrentFilePath
        {
            get { return currentFilePath_; }
            set
            {
                currentFilePath_ = value;
                RefreshTitle();
            }
        }

        private bool isSaved_;
        private bool IsSaved
        {
            get { return isSaved_; }
            set
            {  
                isSaved_ = value;
                RefreshTitle();
            }
        }

        private string BodyText { get; set; }

        private void RefreshTitle()
        {
            Text = $"{(!IsSaved ? "*" : "")}{Path.GetFileName(CurrentFilePath)} - SimpleMarkdown";
        }

        public MainForm(string filePath, MarkdownService markdownService)
        {
            _markdownService = markdownService;

            Icon = Resources.icon;
            InitializeComponent();

            TextBoxTabSizeSetter.SetTabWidth(textBox, 4);

            MouseWheel += MainForm_MouseWheel;
            webBrowser.DocumentCompleted += (s, e) =>
            {
                webBrowser.Document.Body.ScrollTop = preScroll;
            };
            if(filePath != null)
            {
                InitOpenFile(filePath);
            }
            else
            {
                try
                {
                    if(File.Exists(README_FILE))
                    {
                        textBox.Text = File.ReadAllText(README_FILE);
                    }   
                }
                catch(Exception e)
                {
                    Trace.WriteLine(e.Message);
                }
                CurrentFilePath = "새 문서";
                isTempFile = true;
                IsSaved = true;
            }
        }

        private void MainForm_MouseWheel(object sender, MouseEventArgs e)
        {
            if(Control.ModifierKeys == Keys.Control)
            {
                textBox.Font = new Font(textBox.Font.FontFamily, textBox.Font.Size + e.Delta * 0.005f);
            }
        }

        private void InitOpenFile(string filePath)
        {
            CurrentFilePath = filePath;
            try
            {
                textBox.Text = File.ReadAllText(filePath);
                isTempFile = false;
                IsSaved = true;
            }
            catch(Exception e)
            {
                Trace.WriteLine(e);
            }
        }

        private static int preScroll;

        private void textBox_TextChanged(object sender, EventArgs e)
        {
            var text = textBox.Text.Replace("\r\n", "\n").Replace("\n", "\r\n");
            if (BodyText != text)
            {
                BodyText = text;
                textBox.Text = BodyText;
            }
            else
            {
                return;
            }
            IsSaved = false;

            preScroll = webBrowser.Document?.Body?.ScrollTop ?? 0;
            webBrowser.DocumentText = _markdownService.GenerateHtml(textBox.Text);
        }

        private void 열기ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog();
            if(dialog.ShowDialog() == DialogResult.OK)
            {
                if (!IsSaved)
                {
                    var result = MessageBox.Show("파일을 저장하시겠습니까?", "정보", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);
                    if (result == DialogResult.Yes)
                    {
                        SaveFile();
                    }
                    else if (result == DialogResult.Cancel)
                    {
                        return;
                    }
                }
                InitOpenFile(dialog.FileName);
            }
        }

        private void 저장ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFile();
        }

        private void SaveFile()
        {
            if(isTempFile)
            {
                var dialog = new SaveFileDialog()
                {
                    FileName = "새 문서",
                    Filter = "마크다운 문서|*.md|텍스트 파일|*.txt|파일|*.*"
                };
                if(dialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        File.WriteAllText(dialog.FileName, textBox.Text);
                        CurrentFilePath = dialog.FileName;
                        IsSaved = true;
                        isTempFile = false;
                    }
                    catch(Exception e)
                    {
                        MessageBox.Show(e.Message);
                    }
                }
            }
            else
            {
                try
                {
                    File.WriteAllText(CurrentFilePath, textBox.Text);
                    IsSaved = true;
                }
                catch(Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(!IsSaved)
            {
                var result = MessageBox.Show("파일을 저장하시겠습니까?", "정보", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);
                if (result == DialogResult.Yes)
                {
                    SaveFile();
                    if(!IsSaved)
                    {
                        e.Cancel = true;
                    }
                }
                else if(result == DialogResult.Cancel)
                {
                    e.Cancel = true;
                }
            }
        }

        private void 새로만들기ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start(Process.GetCurrentProcess().ProcessName);
        }

        private void 기본ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            splitContainer.Panel1Collapsed = false;
            splitContainer.Panel2Collapsed = false;
        }

        private void 편집기ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            splitContainer.Panel1Collapsed = false;
            splitContainer.Panel2Collapsed = true;
        }

        private void 뷰어ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            splitContainer.Panel1Collapsed = true;
            splitContainer.Panel2Collapsed = false;
        }
    }
}
