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
    /// コントロールをいれる（関数の呼び出しのみ）+データの不正をチェック
    public partial class MainWindow : Window
    {
        // 入力の受け取り用
        internal static float tempo;
        internal static int timSig;
        internal static float length;
        internal int startFrame;
        internal int startBar;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void confirm_Click(object sender, RoutedEventArgs e)
        {
            try{
                Debug.Write("GenSTART\n");
                tempo = Convert.ToSingle(inputTempo.Text);
                timSig = Convert.ToInt32(inputTimeSig.Text);
                length = Convert.ToSingle(inputLength.Text);
                if (inputStartFrame.Text == "開始フレーム位置を入力"){
                    startFrame = 0;
                }
                else{
                    startFrame = Convert.ToInt32(inputStartFrame.Text);
                }
                if (inputStartBar.Text == "開始小節番号を入力(1以上)"){
                    startBar = 1;
                }
                else{
                    startBar = Convert.ToInt32(inputStartBar.Text);
                }
                //ここで呼び出しましょう
                CSV2VMD.WriteVmdFile(DMC.VmdCalc(30, tempo, timSig, length, startFrame, startBar));
                //Debug.Print($"Tempo: {tempo}bpm, {DisplayMotionCreator.totalKeys}");
            }
            catch(FormatException){
                MessageBox.Show("えらーでーす ちゃんとにゅうりょくしてね！", "エラー",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void debug_Click(object sender, RoutedEventArgs e)
        {
            //List<string> list = new List<string>();
            Debug.Write("Debugging/Opening\n");
            // 30, 60, 4, 20, 0, 1で出力
            // CSV2VMD.WriteVmdFile(data);
        }

        private void OnClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }
    }
}