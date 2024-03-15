using System.IO;
using System.Text;

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

        internal static void CsvWrite(List<string[]> keyframes, string address)
        {
            using (StreamWriter SW = new(address, false, Encoding.UTF8))
            {
                foreach (string[] keys in keyframes)
                {
                    SW.WriteLine(string.Join(",", keys));
                }
                SW.Close();
            }
        }
    }
}
