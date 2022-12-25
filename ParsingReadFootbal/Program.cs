using HtmlAgilityPack;
using ParsingReadFootbal;

internal class Program
{
    private static void Main(string[] args)
    {
        var result = Parsing(url: "https://www.readfootball.com/tables.html");

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

    private static Dictionary<string, List<List<string>>>? Parsing(string url)
    {
        try
        {
            Dictionary<string, List<List<string>>> result = new Dictionary<string, List<List<string>>>();
            using HttpClientHandler clientHandler = new HttpClientHandler { AllowAutoRedirect = false, AutomaticDecompression = System.Net.DecompressionMethods.Deflate | System.Net.DecompressionMethods.GZip | System.Net.DecompressionMethods.None };
            using var client = new HttpClient(clientHandler);
            using HttpResponseMessage response = client.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                var html = response.Content.ReadAsStringAsync().Result;
                if (string.IsNullOrEmpty(html))
                {
                    throw new Exception("Пришел пустой html для парсинга.");
                }

                HtmlDocument doc = new();
                doc.LoadHtml(html);

                return ParsingFootbalHtml.ParsingFootbal(doc);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        return null;
    }
}