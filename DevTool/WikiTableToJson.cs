namespace DevTool
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Newtonsoft.Json;

    public static class WikiTableToJson
    {
        public static void Convert(string filePath, string outFilePath, Dictionary<int, string> columns)
        {
            var wikiRows = File.ReadAllLines(filePath);

            var jsonObjects = new List<Dictionary<string, object>>();

            for (var rowIndex = 0; rowIndex < wikiRows.Length; rowIndex++)
            {
                var wikiRow = wikiRows[rowIndex];
                var wikiCells = wikiRow
                    .Split('\t')
                    .Where(c => !string.IsNullOrWhiteSpace(c))
                    .ToArray();
                var jsonObject = new Dictionary<string, object>();

                foreach (var column in columns)
                {
                    var wikiColumnIndex = column.Key;

                    if (wikiColumnIndex >= wikiCells.Length)
                    {
                        break;
                    }

                    var wikiCell = wikiCells[wikiColumnIndex];

                    if (double.TryParse(wikiCell, out var doubleValue))
                    {
                        jsonObject[column.Value] = doubleValue;
                        continue;
                    }

                    jsonObject[column.Value] = wikiCell;
                    jsonObjects.Add(jsonObject);
                }
            }
            
            var jsonRoot = new Dictionary<string, object> { { "Rows", jsonObjects } };

            File.WriteAllText(outFilePath, JsonConvert.SerializeObject(jsonRoot, Formatting.Indented));
        }
    }
}
