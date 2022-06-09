using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SimpleMarkdown.Utils
{
    /// <summary>
    /// TextBox 탭 사이즈 변경
    /// https://stackoverflow.com/questions/1298406/how-to-set-the-tab-width-in-a-windows-forms-textbox-control
    /// </summary>
    public static class TextBoxTabSizeSetter
    {
        private const int EM_SETTABSTOPS = 0x00CB;

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr h, int msg, int wParam, int[] lParam);

        public static void SetTabWidth(TextBox textbox, int tabWidth)
        {
            SendMessage(textbox.Handle, EM_SETTABSTOPS, 1,
                new int[] { tabWidth * 4 });
        }
    }
}