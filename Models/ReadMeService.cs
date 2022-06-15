using System.IO;

namespace SimpleMarkdown.Models
{
    public class ReadMeService
    {
        private const string README_FILE = "Resources\\readme.md";

        public string GetReadMeContent()
        {
            if (!File.Exists(README_FILE))
                throw new FileNotFoundException(README_FILE);

            return File.ReadAllText(README_FILE);
        }
    }
}