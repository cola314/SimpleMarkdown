using System.Windows.Forms;

namespace SimpleMarkdown.Utils
{
    /// <summary>
    /// 스크롤 초기화 방지를 위한 웹 브라우저 래퍼
    /// </summary>
    public class WebBrowserWrapper
    {
        private readonly WebBrowser _webBrowser;
        private int previousScrollPosition;

        public WebBrowserWrapper(WebBrowser webBrowser)
        {
            _webBrowser = webBrowser;
            _webBrowser.DocumentCompleted += WebBrowserOnDocumentCompleted;
        }

        private void WebBrowserOnDocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            _webBrowser.Document.Body.ScrollTop = previousScrollPosition;
        }

        public void SetContent(string content)
        {
            previousScrollPosition = _webBrowser.Document?.Body?.ScrollTop ?? 0;
            _webBrowser.DocumentText = content;
        }
    }
}