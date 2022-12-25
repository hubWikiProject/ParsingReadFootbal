using System;
using System.Windows.Forms;
using HtmlAgilityPack;
using ParsingReadFootbal;

namespace ParsingWithWebBrowserForm
{
    public partial class WebBrowserMainForm : Form
    {
        public WebBrowserMainForm()
        {
            InitializeComponent();
        }

        private void webBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            try
            {
                var html = webBrowser.DocumentText;
                HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                doc.LoadHtml(html);

                var result = ParsingFootbalHtml.ParsingFootbal(doc);

                if (result != null)
                {
                    foreach (var item in result)
                    {
                        Console.WriteLine("-----------------------------------------");
                        Console.WriteLine(item.Key);
                        Console.WriteLine("-----------------------------------------");
                        item.Value.ForEach(r => Console.WriteLine(string.Join("\t", r)));
                        Console.WriteLine("-----------------------------------------\n");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void WebBrowserMainForm_Load(object sender, EventArgs e)
        {
            webBrowser.Navigate("https://www.readfootball.com/tables.html");
        }
    }
}
