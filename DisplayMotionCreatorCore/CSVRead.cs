using System.IO;

namespace DisplayMotionCreatorCore
{
    internal static class Tools
    {
        /// <summary>
        /// CSVファイルを読み、リストにする
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
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
                    string[] fields = line?.Split(',') ?? []; // なかったら空の配列を入れる
                    csvData.Add(fields);
                }
            }
            return csvData;
        }
    }
}
