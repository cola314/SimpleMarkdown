﻿using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using SimpleMarkdown.Properties;
using SimpleMarkdown.Models;
using SimpleMarkdown.Models.FileSaveStrategy;
using SimpleMarkdown.Utils;

namespace SimpleMarkdown
{
    public partial class MainForm : Form
    {
        private readonly MarkdownService _markdownService;
        private readonly WebBrowserWrapper _browser;
        private readonly TextBoxWrapper _editorTextBox;
        private readonly ReadMeService _readMeService;

        private ISaveStrategy _saveStrategy;

        private readonly ProgramState _programState;

        public MainForm(string filePath, MarkdownService markdownService, ReadMeService readMeService)
        {
            _markdownService = markdownService;
            _readMeService = readMeService;

            Icon = Resources.icon;
            InitializeComponent();

            _browser = new WebBrowserWrapper(webBrowser);
            _editorTextBox = new TextBoxWrapper(textBox);
            _editorTextBox.OnTextChanged += EditorTextBoxOnOnTextChanged;

            _programState = new ProgramState();
            _programState.StateChanged += ProgramStateOnStateChanged;

            TextBoxTabSizeSetter.SetTabWidth(textBox, 4);

            MouseWheel += MainForm_MouseWheel;

            if (filePath != null)
            {
                InitOpenFile(filePath);
            }
            else
            {
                InitNewFile();
            }
        }

        private void ProgramStateOnStateChanged()
        {
            var saveMark = _programState.IsSaved ? "" : "*";
            Text = string.Format(Resources.PROGRAM_TITLE_FORMAT, saveMark, _programState.CurrentFileName);
        }

        private void EditorTextBoxOnOnTextChanged(string newText)
        {
            var html = _markdownService.GenerateHtml(newText);
            _browser.SetContent(html);
            _programState.TextChanged();
        }

        private void MainForm_MouseWheel(object sender, MouseEventArgs e)
        {
            if (Control.ModifierKeys == Keys.Control)
            {
                textBox.Font = new Font(textBox.Font.FontFamily, textBox.Font.Size + e.Delta * 0.005f);
            }
        }

        private void InitOpenFile(string filePath)
        {
            try
            {
                textBox.Text = File.ReadAllText(filePath);

                _saveStrategy = new ExistFileSaveStrategy(filePath);
                _programState.FileOpened(filePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Resources.FAIL_TO_OPEN_FILE);
                Close();
            }
        }

        private void InitNewFile()
        {
            try
            {
                textBox.Text = _readMeService.GetReadMeContent();
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.Message);
            }
            _saveStrategy = new TempFileSaveStrategy();
            _programState.NewFileOpened();
        }

        private void 저장ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFile();
        }

        private SaveResult SaveFile()
        {
            var result = _saveStrategy.Save(textBox.Text);

            if (result.IsSuccess)
            {
                _programState.FileSaved(result.SavePath);

                if (_saveStrategy is TempFileSaveStrategy)
                    _saveStrategy = new ExistFileSaveStrategy(result.SavePath);
            }
            else if (result.IsFailed)
            {
                MessageBox.Show(result.Exception.Message, Resources.FAIL_TO_SAVE_FILE);
            }

            return result;
        }

        private void 열기ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                var result = CloseCurrentDocument();
                if (!result)
                    return;

                InitOpenFile(dialog.FileName);
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            var result = CloseCurrentDocument();
            if (!result)
                e.Cancel = true;
        }

        private bool CloseCurrentDocument()
        {
            bool success = true;
            if (!_programState.IsSaved)
            {
                var result = MessageBox.Show(Resources.SAVE_FILE_MESSAGE, Resources.SIMPLE_MARKDOWN, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);
                if (result == DialogResult.Yes)
                {
                    success = SaveFile().IsSuccess;
                }
                else if (result == DialogResult.Cancel)
                {
                    success = false;
                }
            }
            return success;
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
