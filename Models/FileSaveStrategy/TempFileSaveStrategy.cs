using System.IO;
using System.Windows.Forms;

namespace SimpleMarkdown.Models.FileSaveStrategy
{
    public class TempFileSaveStrategy : ISaveStrategy
    {
        public SaveResult Save(string content)
        {
            var dialog = new SaveFileDialog()
            {
                FileName = "새 문서",
                Filter = "마크다운 문서|*.md|텍스트 파일|*.txt|파일|*.*"
            };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(dialog.FileName, content);
                return SaveResult.Success(dialog.FileName);
            }

            return SaveResult.Canceled;
        }
    }
}