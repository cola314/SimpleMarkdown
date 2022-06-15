using System.IO;
using Markdig;

namespace SimpleMarkdown.Models
{
    public class MarkdownService
    {
        private const string CSS_FILE = "Resources\\markdown.css";

        private readonly MarkdownPipeline _pipeline;
        private readonly string _cssStyleText;

        public MarkdownService()
        {
            _pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();
            _cssStyleText = File.ReadAllText(CSS_FILE);
        }

        public string GenerateHtml(string plainText)
        {
            string html = Markdown.ToHtml(plainText, _pipeline);
            return $"<style>{_cssStyleText}</style>{html}";
        }
    }
}