using System.IO;

namespace DisplayMotionCreatorCore {
    internal class Tools
    {
        internal static List<string[]> CsvReader(string fileName)
        {
            string filePath = @fileName;
            var csvData = new List<string[]>();

            // csvファイルをリストに格納
            using (StreamReader reader = new(filePath))
            {
                while (!reader.EndOfStream)
                {
                    string? line = reader.ReadLine();
                    string[] fields = line?.Split(',') ?? [];
                    csvData.Add(fields);
                }
            }
            return csvData;
        }
    }
}
