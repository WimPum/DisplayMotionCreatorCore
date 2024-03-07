using System.Text;
using System.IO;
using Microsoft.Win32;
using System.Windows;
using System.Diagnostics;

namespace DisplayMotionCreatorCore
{
    /// ここにwrite_vmd_file
    internal class CSV2VMD
    {
        internal static void WriteVmdFile(List<string[]> keyframes)
        {
            SaveFileDialog sfd = new()
            {
                FileName = "output.vmd",
                Filter = "VMDファイル (*.vmd)|*.vmd",
                Title = "名前を付けて保存"
            };
            if (sfd.ShowDialog() == true){
                SaveBinary(sfd.FileName, keyframes);
                MessageBox.Show($"{MainWindow.tempo}bpm、{MainWindow.timSig}拍子、{MainWindow.length}秒で生成しました。キー数{DMC.totalKeys}です。\n" +
                    "MMDのキー登録数の上限は20000ですので注意してください。", "出力完了", 
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private static void SaveBinary(string fileName, List<string[]> keyframes)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance); //SJIS使えるようにする
            Encoding encoding = Encoding.GetEncoding("shift_jis");
            using (BinaryWriter writer = new(new FileStream(fileName, FileMode.Create)))
            {
                try
                {
                    writer.Write(encoding.GetBytes("Vocaloid Motion Data 0002\x00\x00\x00\x00\x00"));
                    writer.Write(encoding.GetBytes("Electone3DModel     "));
                    writer.Write(0);

                    int totalKeys = Convert.ToInt32(keyframes[3][0]);
                    writer.Write(totalKeys);

                    for (int i = 0; i < totalKeys; i++)
                    {
                        byte[] keyName = encoding.GetBytes(keyframes[4 + i][0]);
                        Array.Resize(ref keyName, 15);//15バイトで確定
                        ulong frame = Convert.ToUInt64(keyframes[4 + i][1]);
                        float value = Convert.ToSingle(keyframes[4 + i][2]);
                        byte[] byteFrames = BitConverter.GetBytes(frame);
                        byte[] byteValue = BitConverter.GetBytes(value);
                        // BitConverterだとLE/BEは環境依存だがWindows+x64だからリトルエンディアンという体で
                        //Debug.Print($"frame: {frame}, value: {value}, original: {keyframes[4 + i][2]}");
                        Array.Resize(ref byteFrames, 4);//4バイトで確定
                        Array.Resize(ref byteValue, 4);//4バイトで確定
                        writer.Write(keyName);
                        writer.Write(byteFrames);
                        writer.Write(byteValue);
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }
        }
    }
}
