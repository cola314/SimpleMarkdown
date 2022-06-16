using System;
using System.IO;

namespace SimpleMarkdown.Models.FileSaveStrategy
{
    public class ExistFileSaveStrategy : ISaveStrategy
    {
        private readonly string _filePath;

        public ExistFileSaveStrategy(string filePath)
        {
            _filePath = filePath;
        }

        public SaveResult Save(string content)
        {
            try
            {
                File.WriteAllText(_filePath, content);
                return SaveResult.Success(_filePath);
            }
            catch (Exception ex)
            {
                return SaveResult.Error(ex);
            }
        }
    }
}