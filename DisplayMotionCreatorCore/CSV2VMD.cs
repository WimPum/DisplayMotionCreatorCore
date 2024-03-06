using System;
using System.Collections.Generic;
using System.IO.Enumeration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Win32;
using Microsoft.VisualBasic;
using System.Windows;

namespace DisplayMotionCreatorCore
{
    /// ここにwrite_vmd_file
    internal class CSV2VMD
    {
        //public void write_vmd_file(List<List<String>> keyframes) {

        public void write_vmd_file(byte[] keyframes) {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.FileName = "output.vmd";
            sfd.Filter = "VMDファイル (*.vmd)|*.vmd";
            sfd.Title = "名前を付けて保存";
            if (sfd.ShowDialog() == true){
                //File.WriteAllBytes(sfd.FileName, keyframes)
                save_binary(sfd.FileName, keyframes);
                MessageBox.Show("XXXbpm、X拍子、XXX秒で生成しました。キー数XXXXです。\n" +
                    "MMDのキー登録数の上限は20000ですので注意してください。", "出力完了", 
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }

            //using (BinaryWriter writer = new BinaryWriter(File.OpenWrite()));
        }

        public void save_binary(string filename, byte[] keyframes) {
            Encoding encoding = new UTF8Encoding();
            using (BinaryWriter writer = new BinaryWriter(File.OpenWrite(filename))){
                writer.Write(encoding.GetBytes("Vocaloid Motion Data 0002\x00\x00\x00\x00\x00"));
                writer.Write(encoding.GetBytes("Electone3DModel     "));
            }
        }
    }
}
