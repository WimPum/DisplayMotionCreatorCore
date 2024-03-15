using System.Diagnostics;
using System.Text;
using System.IO;

namespace DisplayMotionCreatorCore
{
    /// <summary>
    /// MMDElectone用　キーフレームを生成するツール（モーフのみ）
    /// </summary>
    internal class DMC
    {

        internal static int totalKeys; // 外から参照できるようにする

        private static List<string[]> AddKeyName(string name, int frameNum, string value, List<string[]> lists)
        {
            // stringのvalueは1.0とか0.0がfloat変換ではできなかったから
            string strFrameNum = Convert.ToString(frameNum);
            string[] strings = [name, strFrameNum, value];
            lists.Add(strings);
            totalKeys += 1; //キー追加したから増やす
            return lists;
        }

        private static List<string[]> AddKey(int num, int digit, int frameNum, List<string[]> lists)
        {
            List<string[]> keyframesLocal = [];
            string strFrameNum = Convert.ToString(frameNum);
            switch (num)
            {
                case 1:
                    keyframesLocal.AddRange(
                    [   [ $"ShiftV1_{digit}", strFrameNum, "0.0" ],
                        [ $"ShiftV2_{digit}", strFrameNum, "0.0" ],
                        [ $"Shift>1_{digit}", strFrameNum, "0.0" ],
                        [ $"Shift>2_{digit}", strFrameNum, "0.0" ],
                        [ $"Shift>3_{digit}", strFrameNum, "0.0" ]
                    ]);
                    break;
                case 2:
                    keyframesLocal.AddRange(
                    [   [ $"ShiftV1_{digit}", strFrameNum, "0.0" ],
                        [ $"ShiftV2_{digit}", strFrameNum, "0.0" ],
                        [ $"Shift>1_{digit}", strFrameNum, "1.0" ],
                        [ $"Shift>2_{digit}", strFrameNum, "0.0" ],
                        [ $"Shift>3_{digit}", strFrameNum, "0.0" ]
                    ]);
                    break;
                case 3:
                    keyframesLocal.AddRange(
                    [   [ $"ShiftV1_{digit}", strFrameNum, "0.0" ],
                        [ $"ShiftV2_{digit}", strFrameNum, "0.0" ],
                        [ $"Shift>1_{digit}", strFrameNum, "1.0" ],
                        [ $"Shift>2_{digit}", strFrameNum, "1.0" ],
                        [ $"Shift>3_{digit}", strFrameNum, "0.0" ]
                    ]);
                    break;
                case 4:
                    keyframesLocal.AddRange(
                    [   [ $"ShiftV1_{digit}", strFrameNum, "0.0" ],
                        [ $"ShiftV2_{digit}", strFrameNum, "0.0" ],
                        [ $"Shift>1_{digit}", strFrameNum, "1.0" ],
                        [ $"Shift>2_{digit}", strFrameNum, "1.0" ],
                        [ $"Shift>3_{digit}", strFrameNum, "1.0" ]
                    ]);
                    break;
                case 5:
                    keyframesLocal.AddRange(
                    [   [ $"ShiftV1_{digit}", strFrameNum, "1.0" ],
                        [ $"ShiftV2_{digit}", strFrameNum, "0.0" ],
                        [ $"Shift>1_{digit}", strFrameNum, "0.0" ],
                        [ $"Shift>2_{digit}", strFrameNum, "0.0" ],
                        [ $"Shift>3_{digit}", strFrameNum, "0.0" ]
                    ]);
                    break;
                case 6:
                    keyframesLocal.AddRange(
                    [   [ $"ShiftV1_{digit}", strFrameNum, "1.0" ],
                        [ $"ShiftV2_{digit}", strFrameNum, "0.0" ],
                        [ $"Shift>1_{digit}", strFrameNum, "1.0" ],
                        [ $"Shift>2_{digit}", strFrameNum, "0.0" ],
                        [ $"Shift>3_{digit}", strFrameNum, "0.0" ]
                    ]);
                    break;
                case 7:
                    keyframesLocal.AddRange(
                    [   [ $"ShiftV1_{digit}", strFrameNum, "1.0" ],
                        [ $"ShiftV2_{digit}", strFrameNum, "0.0" ],
                        [ $"Shift>1_{digit}", strFrameNum, "1.0" ],
                        [ $"Shift>2_{digit}", strFrameNum, "1.0" ],
                        [ $"Shift>3_{digit}", strFrameNum, "0.0" ]
                    ]);
                    break;
                case 8:
                    keyframesLocal.AddRange(
                    [   [ $"ShiftV1_{digit}", strFrameNum, "1.0" ],
                        [ $"ShiftV2_{digit}", strFrameNum, "0.0" ],
                        [ $"Shift>1_{digit}", strFrameNum, "1.0" ],
                        [ $"Shift>2_{digit}", strFrameNum, "1.0" ],
                        [ $"Shift>3_{digit}", strFrameNum, "1.0" ]
                    ]);
                    break;
                case 9:
                    keyframesLocal.AddRange(
                    [   [ $"ShiftV1_{digit}", strFrameNum, "1.0" ],
                        [ $"ShiftV2_{digit}", strFrameNum, "1.0" ],
                        [ $"Shift>1_{digit}", strFrameNum, "0.0" ],
                        [ $"Shift>2_{digit}", strFrameNum, "0.0" ],
                        [ $"Shift>3_{digit}", strFrameNum, "0.0" ]
                    ]);
                    break;
                case 0:
                    keyframesLocal.AddRange(
                    [   [ $"ShiftV1_{digit}", strFrameNum, "1.0" ],
                        [ $"ShiftV2_{digit}", strFrameNum, "1.0" ],
                        [ $"Shift>1_{digit}", strFrameNum, "1.0" ],
                        [ $"Shift>2_{digit}", strFrameNum, "0.0" ],
                        [ $"Shift>3_{digit}", strFrameNum, "0.0" ]
                    ]);
                    break;
            }
            lists.AddRange((IEnumerable<string[]>)keyframesLocal);
            totalKeys += 5;
            return lists;
        }

        private static List<string[]> VmdCleaner(List<string[]> keyframes)
        {
            int maxKeyNums = Convert.ToInt32(keyframes[3][0]);
            int delKeyNums = 0; // 消されたキー数
            int readIndex = 0; // リストを読み取る
            //List<string[]> keyframesReturn = keyframes;
            string[] localList = []; // 1フレーム分のキーが入る
            string[] localNextList = new string[3]; // 次のキー
            LinkedList<string[]> linkedKeyframes = new(keyframes); // 中身を消すときに強い
            while (readIndex < maxKeyNums) // 最高でもこれだけループ
            {
                try
                {
                    localList = (string[])keyframes[readIndex + 4].Clone(); // キーは3つの要素をもつ
                    localNextList = (string[])keyframes[readIndex + 4].Clone(); // 連続するキー

                    int nextFrame = Convert.ToInt32(localList[1]) + 1;
                    localNextList[1] = nextFrame.ToString(); // フレーム番号＋１になっているだけの同じキーを探す。

                    if (linkedKeyframes.Any(x => x.SequenceEqual(localNextList))) // 連続するキーがあったら
                    {
                        Debug.Write("Deleted keys\n");
                        //linkedKeyframes.Remove(localNextList); //オリジナルもその次のキーも消す
                        //linkedKeyframes.Remove(localList); //消えてない
                        linkedKeyframes.RemoveAll(x => x.SequenceEqual(localNextList) || x.SequenceEqual(localList));

                        delKeyNums += 2;
                    }
                    else
                    {
                        Debug.Write("Didn't delete keys\n");
                    }

                    readIndex++;
                }
                catch (ArgumentOutOfRangeException)
                {
                    Debug.WriteLine("Exiting Loop now");
                    break;
                }
            }
            Debug.Print($"deleted {delKeyNums} keys");
            keyframes = linkedKeyframes.ToList();
            keyframes[3][0] = (maxKeyNums - delKeyNums).ToString();
            return keyframes;
        }

        /// <summary>
        /// どこにキー打つか求める
        /// </summary>
        /// <param name="mmdFps">MMDキーフレームのFPS</param>
        /// <param name="bpm">テンポ</param>
        /// <param name="timSig">拍子</param>
        /// <param name="length">長さ(秒)</param>
        /// <param name="startFrame">開始フレーム</param>
        /// <param name="startBar">開始拍子番号</param>
        /// <returns></returns>
        internal static List<string[]> VmdCalc(
            int mmdFps,
            float bpm, //(bpm / 60)はbps
            int timSig,
            float length,
            int startFrame,
            int startBar)
        {
            // HeaderはCSV2VMDでは無視されます
            List<string[]> keyframesHeader =
            [
                ["Vocaloid Motion Data 0002", "0"],
                ["Electone3DModel"],
                ["0"] //ボーンの数は０
            ];

            List<string[]> keyframes = []; //ここにキー追加
            float noteDuration = mmdFps / (bpm / 60); // 切り替えまでの長さ(秒)
            int totalNotes = (int)(length * (bpm / 60)); // 何回画面が切り替わるか
            int remain = totalNotes % timSig; // 下の繰り上げに使う
            float currentFrames = startFrame; // 今のフレーム
            int roundedFrames = startFrame; // 実際に書き込むフレーム
            bool canAddKey1 = false; // X00.0 100の位に連続で同じキーを入れないようにする
            bool canAddKey2 = false; // 0X0.0

            totalKeys = 0; // 何回も実行できるようにリセット

            // 繰り上げ 区切りよく(ちょっと長くなる)
            if (remain != 0)
            {
                totalNotes = totalNotes - remain + timSig;
            }

            for (int i = 0; i < totalNotes; i++)
            {
                int sigValue = (i / timSig) + startBar; // 小節番号
                float sigDisp = sigValue + (float)((i % timSig) + 1) / 10; // X.x表示
                float currentTime = currentFrames / mmdFps;
                Debug.Print($"{roundedFrames}frame, {sigDisp}, {currentTime}s");

                if (sigDisp == (startBar + 0.1f)) // 開始位置に関する部分
                {
                    List<string[]> keyframesSort = [];
                    if (startFrame > 1) // roundedFrames - 1 で開始フレームの前に初期化用キー
                    {   //startFrame > 1 roundedFramesは0が最小
                        AddKey(1, 4, roundedFrames - 1, keyframes); // 桁を有効にする前
                        AddKey(1, 3, roundedFrames - 1, keyframes); // 1はすべてのモーフが０
                        if (sigDisp >= 10.1f)
                        {
                            AddKey(1, 2, roundedFrames - 1, keyframes);
                            canAddKey2 = true;
                        }
                        if (sigDisp >= 100.1f)
                        {
                            AddKey(1, 1, roundedFrames - 1, keyframes);
                            canAddKey1 = true;
                        }
                        AddKeyName("Ready", roundedFrames - 1, "0.0", keyframesSort);
                    }

                    AddKeyName("Ready", roundedFrames, "1.0", keyframesSort);

                    if (sigDisp >= 1.1f)
                    {
                        if (startFrame > 1) // 上にまとめず個別に条件分岐します
                        {
                            AddKeyName("One_3", roundedFrames - 1, "0.0", keyframesSort);
                            AddKeyName("One_4", roundedFrames - 1, "0.0", keyframesSort);
                        }
                        AddKeyName("One_3", roundedFrames, "1.0", keyframesSort);
                        AddKeyName("One_4", roundedFrames, "1.0", keyframesSort);
                    }
                    if (sigDisp >= 10.1f)
                    {
                        if (startFrame > 1) // 上にまとめず個別に条件分岐します
                        {
                            AddKeyName("One_2", roundedFrames - 1, "0.0", keyframesSort);
                        }
                        AddKeyName("One_2", roundedFrames, "1.0", keyframesSort);
                    }
                    if (sigDisp >= 100.1f)
                    {
                        if (startFrame > 1) // 上にまとめず個別に条件分岐します
                        {
                            AddKeyName("One_1", roundedFrames - 1, "0.0", keyframesSort);
                        }
                        AddKeyName("One_1", roundedFrames, "1.0", keyframesSort);
                    }
                    keyframesSort.Sort(
                        (a, b) => Convert.ToInt32(a[1]) - Convert.ToInt32(b[1]));
                    keyframes.AddRange(keyframesSort);
                }
                else if (sigDisp == 10.1f)
                {
                    AddKeyName("One_2", roundedFrames - 1, "0.0", keyframes);
                    AddKeyName("One_2", roundedFrames, "1.0", keyframes);
                }
                else if (sigDisp == 100.1f)
                {
                    AddKeyName("One_1", roundedFrames - 1, "0.0", keyframes);
                    AddKeyName("One_1", roundedFrames, "1.0", keyframes);
                }

                // 以下はメインのキー追加
                AddKey(i % timSig + 1, 4, roundedFrames, keyframes); // キー追加 000.Xの部分
                AddKey(sigValue % 10, 3, roundedFrames, keyframes); // 00X.0
                if (sigDisp >= 10 & canAddKey2 == true)
                {
                    AddKey(sigValue / 10 % 10, 2, roundedFrames, keyframes); // 0X0.0
                    canAddKey2 = false;
                }
                if (sigDisp >= 100 & canAddKey1 == true)
                {
                    AddKey(sigValue / 100 % 10, 1, roundedFrames, keyframes); // X00.0
                    canAddKey1 = false;
                }

                currentFrames += noteDuration;
                roundedFrames = (int)Math.Round(currentFrames); //整数にする
                int nextSigValue = ((i + 1) / timSig) + startBar;

                // 以下はメインに対応する終了キー
                AddKey(i % timSig + 1, 4, roundedFrames - 1, keyframes); // キー追加 000.Xの部分
                AddKey(sigValue % 10, 3, roundedFrames - 1, keyframes); // 00X.0
                if (sigDisp >= 10 & sigValue / 10 % 10 != nextSigValue / 10 % 10)
                { // 「次も同じ値」でなければキー追加
                    AddKey(sigValue / 10 % 10, 2, roundedFrames - 1, keyframes); // 0X0.0
                    canAddKey2 = true;
                }
                if (sigDisp >= 100 & sigValue / 100 % 10 != nextSigValue / 100 % 10)
                {
                    AddKey(sigValue / 100 % 10, 1, roundedFrames - 1, keyframes); // X00.0
                    canAddKey1 = true;
                }

            }

            keyframesHeader.Add([Convert.ToString(totalKeys)]);
            keyframesHeader.AddRange(keyframes);
            Debug.Print($"TotalNotes:{totalNotes}, rawTotalKeys:{totalKeys}");
            List<string[]> keyframesCleaned = VmdCleaner(keyframesHeader);
            totalKeys = Convert.ToInt32(keyframesCleaned[3][0]);
            Debug.Print($"TotalKeys:{totalKeys}");
            return keyframesCleaned;
        }
    }

    // StackOverFlowより
    public static class LinkedListExtensions
    {
        public static void RemoveAll<T>(this LinkedList<T> linkedList,
                                        Func<T, bool> predicate)
        {
            for (LinkedListNode<T> node = linkedList.First; node != null;)
            {
                LinkedListNode<T> next = node.Next;
                if (predicate(node.Value))
                    linkedList.Remove(node);
                node = next;
            }
        }
    }
}
