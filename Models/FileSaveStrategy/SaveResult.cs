namespace SimpleMarkdown.Models.FileSaveStrategy
{
    public class SaveResult
    {
        public bool IsCanceled { get; private set; }
        public string SavePath { get; private set; }

        public static SaveResult Success(string savePath)
        {
            return new SaveResult()
            {
                SavePath = savePath,
            };
        }

        public static SaveResult Canceled = new SaveResult() { IsCanceled = true };
    }
}