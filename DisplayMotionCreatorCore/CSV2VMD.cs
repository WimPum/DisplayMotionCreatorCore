using Microsoft.Win32;
using System.IO;
using System.Text;
using System.Windows;
using MW = ModernWpf;

namespace DisplayMotionCreatorCore
{

    public static class CSV2VMD
    {
        /// <summary>
        /// MainWindowで呼び出すための関数
        /// SaveBinaryが中で仕事します
        /// </summary>
        /// <param name="keyframes"></param>
        public static void WriteVmdFile(List<string[]> keyframes)
        {
            SaveFileDialog sfd = new()
            {
                FileName = "output.vmd",
                Filter = "VMDファイル (*.vmd)|*.vmd",
                Title = "名前を付けて保存"
            };
            if (sfd.ShowDialog() == true)
            {
                SaveBinary(sfd.FileName, keyframes);
                MW.MessageBox.Show($"{MainWindow.tempo}bpm、{MainWindow.timSig}拍子、{MainWindow.length}秒で生成しました。キー数{DMC.totalKeys}です。\n",
                    "出力完了", MessageBoxButton.OK, MessageBoxImage.Information);

                // 上限超えてたら注意
                if (DMC.totalKeys >= 20000)
                {
                    MW.MessageBox.Show("MMDのキー登録数の上限は20000ですので注意してください。",
                    "警告", MessageBoxButton.OK, MessageBoxImage.Warning);
                }

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
                    // まずファイルの先頭部分を書き込みます
                    writer.Write(encoding.GetBytes("Vocaloid Motion Data 0002\x00\x00\x00\x00\x00"));
                    writer.Write(encoding.GetBytes("Electone3DModel     "));
                    writer.Write(0);

                    int totalKeys = Convert.ToInt32(keyframes[3][0]);
                    writer.Write(totalKeys);

                    for (int i = 0; i < totalKeys; i++)
                    {
                        // キーフレームを書き込みます
                        // すべてstringなので変換して書き込み
                        // BitConverterだとLE/BEは環境依存だがWindows+x64だからリトルエンディアンという体で
                        byte[] keyName = encoding.GetBytes(keyframes[4 + i][0]);
                        ulong frame = Convert.ToUInt64(keyframes[4 + i][1]);
                        float value = Convert.ToSingle(keyframes[4 + i][2]);
                        byte[] byteFrames = BitConverter.GetBytes(frame);
                        byte[] byteValue = BitConverter.GetBytes(value);

                        Array.Resize(ref keyName, 15);          //15バイトで固定
                        Array.Resize(ref byteFrames, 4);        //4バイト
                        Array.Resize(ref byteValue, 4);         //4バイト

                        writer.Write(keyName);
                        writer.Write(byteFrames);
                        writer.Write(byteValue);
                    }
                }
                catch (IOException)
                {
                    MW.MessageBox.Show("書き込みができません", "エラー",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
        }
    }
}
