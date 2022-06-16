using System;

namespace SimpleMarkdown.Models.FileSaveStrategy
{
    public class SaveResult
    {

        public bool IsSuccess => SavePath != null;
        public string SavePath { get; private set; }

        public bool IsFailed => Exception != null;
        public Exception Exception { get; private set; }

        public bool IsCanceled { get; private set; }

        public static SaveResult Success(string savePath)
        {
            return new SaveResult()
            {
                SavePath = savePath,
            };
        }

        public static SaveResult Error(Exception exception)
        {
            return new SaveResult()
            {
                Exception = exception
            };
        }

        public static SaveResult Canceled = new SaveResult() { IsCanceled = true };
    }
}