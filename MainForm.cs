using CefSharp.WinForms;
using CefSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CefSharp.Web;
using System.IO;
using System.Diagnostics;

namespace SimpleMarkdown
{
    public partial class MainForm : Form
    {
        private ChromiumWebBrowser browser;

        private string css = File.ReadAllText("markdown.css");

        public MainForm()
        {
            InitializeComponent();
            return;
            Cef.Initialize(new CefSettings());
            browser = new ChromiumWebBrowser(new HtmlString(""));
            splitContainer.Panel2.Controls.Add(browser);
            browser.Dock = DockStyle.Fill;
        }

        private void textBox_TextChanged(object sender, EventArgs e)
        {
            //browser.LoadHtml(CommonMark.CommonMarkConverter.Convert(textBox.Text));
            webBrowser.DocumentText = CreateHtmlWithCSS(CommonMark.CommonMarkConverter.Convert(textBox.Text));
        }

        private void webBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            HtmlElement head = webBrowser.Document.GetElementsByTagName("head")[0];
            HtmlElement styleEl = webBrowser.Document.CreateElement("style");
            styleEl.InnerHtml = @"body {background-color:transparent !important; margin: 0px auto; overflow: hidden; }.live__content{display:none;}.chat__footer{display:none;}.chat__header{display:none}#header{display:none}.in-chat-avatar{display:none}.message__username{font-style:italic;font-weight: 800!important;color:ff8f0f !important}.message__text{font-style:italic;font-weight: 400!important;color:ff8f0f !important}.chat__content{ background-color:transparent  !important}.chat{ background-color:transparent  !important}";
            head.AppendChild(styleEl);
        }

        private string CreateHtmlWithCSS(string html)
        {
            return $"<style>{css}</style>{html}";
        }
    }
}
