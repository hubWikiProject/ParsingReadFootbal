using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ParsingReadFootbal
{
    public class ParsingFootbalHtml
    {
        public static Dictionary<string, List<List<string>>> ParsingFootbal(HtmlDocument doc)
        {
            Dictionary<string, List<List<string>>> result = new Dictionary<string, List<List<string>>>();

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
}
