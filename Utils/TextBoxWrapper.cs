using System;
using System.Windows.Forms;

namespace SimpleMarkdown.Utils
{
    public class TextBoxWrapper
    {
        private readonly TextBox _textBox;
        private string _previousText;

        public TextBoxWrapper(TextBox textBox)
        {
            _textBox = textBox;
            _textBox.TextChanged += TextBoxOnTextChanged;
        }

        private void TextBoxOnTextChanged(object sender, EventArgs e)
        {
            var text = _textBox.Text.Replace("\r\n", "\n").Replace("\n", "\r\n");

            if (_previousText != text)
            {
                _previousText = text;
                _textBox.Text = text;

                OnTextChanged?.Invoke(text);
            }
        }

        public event Action<string> OnTextChanged;
    }
}