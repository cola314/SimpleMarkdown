using System;
using System.IO;

namespace SimpleMarkdown.Models
{
    public class ProgramState
    {
        public event Action OnStateChanged;

        public string CurrentFileName { get; private set; }
        public bool IsSaved { get; private set; }

        private void NotifyStateChanged()
        {
            OnStateChanged?.Invoke();
        }

        public void ExistFileOpened(string filePath)
        {
            CurrentFileName = Path.GetFileName(filePath);
            IsSaved = true;
            NotifyStateChanged();
        }

        public void TextChanged()
        {
            IsSaved = false;
            NotifyStateChanged();
        }

        public void NewFileOpened()
        {
            CurrentFileName = "새 문서";
            IsSaved = true;
            NotifyStateChanged();
        }

        public void FileSaved(string filePath)
        {
            CurrentFileName = Path.GetFileName(filePath);
            IsSaved = true;
            NotifyStateChanged();
        }
    }
}