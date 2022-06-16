using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using SimpleMarkdown.Models;

namespace SimpleMarkdown
{
    static class Program
    {
        /// <summary>
        /// 해당 애플리케이션의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            // 프로그램 베이스 경로를 exe 파일이 있는 경로로 설정
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
            
            string filePath = args?.FirstOrDefault();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm(filePath, new MarkdownService(), new ReadMeService()));
        }
    }
}
