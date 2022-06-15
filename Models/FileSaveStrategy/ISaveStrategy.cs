namespace SimpleMarkdown.Models.FileSaveStrategy
{
    public interface ISaveStrategy
    {
        SaveResult Save(string content);
    }
}