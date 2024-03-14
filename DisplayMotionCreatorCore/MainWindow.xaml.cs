using System.Diagnostics;
using System.Windows;

namespace DisplayMotionCreatorCore
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// コントロールをいれる（関数の呼び出しのみ）+データの不正をチェック
    /// </summary>
    public partial class MainWindow : Window
    {
        // 入力の受け取り用
        internal static float tempo;
        internal static int timSig;
        internal static float length;
        internal int startFrame;
        internal int startBar;

        private string saveTempo
        {
            get { return Properties.Settings.Default.tempoSave; }
            set { Properties.Settings.Default.tempoSave = value; }
        }
        private string saveTimeSig
        {
            get { return Properties.Settings.Default.timSigSave; }
            set { Properties.Settings.Default.timSigSave = value; }
        }
        private string saveLength
        {
            get { return Properties.Settings.Default.lengthSave; }
            set { Properties.Settings.Default.lengthSave = value; }
        }
        private string saveStartFrame
        {
            get { return Properties.Settings.Default.startFrameSave; }
            set { Properties.Settings.Default.startFrameSave = value; }
        }
        private string saveStartBar
        {
            get { return Properties.Settings.Default.startBarSave; }
            set { Properties.Settings.Default.startBarSave = value; }
        }



        public MainWindow()
        {
            InitializeComponent();

            inputTempo.Text = saveTempo;
            inputTimeSig.Text = saveTimeSig;
            inputLength.Text = saveLength;
            inputStartFrame.Text = saveStartFrame;
            inputStartBar.Text = saveStartBar;

            // TextBoxの名前を配列に格納　参考：https://alfort.online/361
            var textBoxes = new[] { inputTempo, inputTimeSig, inputLength, inputStartFrame, inputStartBar };

            foreach (var textBox in textBoxes)
            {
                // TextBoxがクリックされた時に全選択する
                textBox.GotFocus += (s, e) =>
                {
                    textBox.SelectAll();
                };

                // マウスの左ボタンが押された時にフォーカスを移動する
                textBox.PreviewMouseLeftButtonDown += (s, e) =>
                {
                    if (!textBox.IsFocused)
                    {
                        e.Handled = true;
                        textBox.Focus();
                    }
                };
            }
        }
        private async void confirm_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                confirm.Content = "Wait";
                await Task.Delay(100); // UI更新まで待つ

                Debug.Write("GenSTART\n");

                tempo = Convert.ToSingle(inputTempo.Text);
                timSig = Convert.ToInt32(inputTimeSig.Text);
                length = Convert.ToSingle(inputLength.Text);
                if (inputStartFrame.Text == "開始フレーム位置を入力")
                {
                    startFrame = 0;
                }
                else
                {
                    startFrame = Convert.ToInt32(inputStartFrame.Text);
                }
                if (inputStartBar.Text == "開始小節番号を入力(1以上)")
                {
                    startBar = 1;
                }
                else
                {
                    startBar = Convert.ToInt32(inputStartBar.Text);
                }

                // 値がおかしくないかチェックする
                if (timSig < 1 || timSig > 9)
                {
                    MessageBox.Show("拍子は１〜９までの整数値で入力してください。", "エラー",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // 設定保存
                saveTempo = inputTempo.Text;
                saveTimeSig = inputTimeSig.Text;
                saveLength = inputLength.Text;
                saveStartFrame = inputStartFrame.Text;
                saveStartBar = inputStartBar.Text;
                Properties.Settings.Default.Save();

                //ここで呼び出し
                CSV2VMD.WriteVmdFile(DMC.VmdCalc(30, tempo, timSig, length, startFrame, startBar));
            }
            catch (FormatException)
            {
                MessageBox.Show("数値を入力してください。", "エラー",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                confirm.Content = "実行";
            }
        }
    }
}