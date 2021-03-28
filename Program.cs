using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

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
            try
            {
                string filePath = args?.FirstOrDefault();

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainForm(filePath));
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
    }
}
