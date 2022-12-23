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

                HtmlAgilityPack.HtmlDocument doc = new();
                doc.LoadHtml(html);
                var tables = doc.DocumentNode.SelectNodes(".//div[@id='block-system-main']//div[@class='block_content']//div[@class='two-table-row']//div[@class]");

                if (tables == null || tables.Count <= 0)
                {
                    throw new Exception("Не найден блок div[@class='two-table']");
                }

                foreach (var table in tables)
                {
                    var titleNode = table.SelectSingleNode(".//div[@class='head_tb']");

                    if (titleNode == null)
                    {
                        continue;
                    }

                    var tbl = table.SelectSingleNode(".//div[@class='tab_champ']//table");

                    if (tbl == null)
                    {
                        throw new Exception("Не найден блок .//div[@class='tab_champ']//table");
                    }

                    var rows = tbl.SelectNodes(".//tr");

                    if (rows == null || rows.Count <= 0)
                    {
                        throw new Exception("Не найден блок .//div[@class='tab_champ']//table");
                    }

                    var res = new List<List<string>>();

                    foreach (var row in rows)
                    {
                        var cells = row.SelectNodes(".//td");
                        if (cells != null && cells.Count > 0)
                        {
                            res.Add(new List<string>(cells.Select(c => c.InnerText)));
                        }
                    }

                    result[titleNode.InnerText] = res;
                }

                return result;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        return null;
    }
}