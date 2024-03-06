using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

namespace DisplayMotionCreatorCore
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// こっちにWindowコントロールをいれる（つまりほとんど関数の呼び出しのみ）
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void confirm_Click(object sender, RoutedEventArgs e)
        {
            Debug.Write("GenSTART\n");
        }

        private void debug_Click(object sender, RoutedEventArgs e)
        {
            //List<string> list = new List<string>();
            Debug.Write("Debugging/Opening\n");
            byte[] data = File.ReadAllBytes("PATHTO/output.vmd"); //すぐ消すからね!
            CSV2VMD cSV2VMD = new CSV2VMD();
            cSV2VMD.write_vmd_file(data);
        }
    }
}