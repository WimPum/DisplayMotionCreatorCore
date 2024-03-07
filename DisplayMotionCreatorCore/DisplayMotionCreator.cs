using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisplayMotionCreatorCore
{
    /// 実際の処理をこっちに
    internal class DMC
    {

        internal static int totalKeys = 0; // 外でも使いたい

        private static List<string[]> AddKeyName(string name, int frameNum, float value, List<string[]> lists)
        {
            string strFrameNum = Convert.ToString(frameNum);
            string strValue = Convert.ToString(value);
            string[] strings = [name, strFrameNum, strValue];
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

        internal static List<string[]> VmdCalc( //どこにキー打つか求める
            int mmdFps, // MMDキーフレームのFPS
            float bpm, // テンポ //(bpm / 60)はbps
            int timSig, // 拍子
            float length, // 長さ(秒)
            int startFrame, // 開始フレーム
            int startBar) // 開始拍子番号
        {
            List<string[]> keyframesHeader =
            [
                ["Vocaloid Motion Data 0002", "0"],
                ["Electone3dModel"],
                ["0"] //ボーンの数は０
            ];

            List<string[]> keyframes = []; //ここにキー追加
            float noteDuration = mmdFps / (bpm / 60); // 音符間の長さ(秒)
            int totalNotes = (int)(length * (bpm / 60)); // 音符の合計
            int remain = totalNotes % timSig;
            float currentFrames = startFrame;
            int roundedFrames = startFrame;
            bool canAddKey1 = false;
            bool canAddKey2 = false;

            totalKeys = 0; // リセット
            if (remain != 0) // 繰り上げ 区切りよく(ちょっと長くなる)
            {
                totalNotes = totalNotes - remain + timSig;
            }

            for( int i = 0; i < totalNotes; i++ )
            {
                int sigValue = (i / timSig) + startBar; // 小節番号
                float sigDisp = sigValue + (float)((i % timSig) + 1) / 10; // X.x表示
                Debug.Print($"{sigValue}, {sigDisp}");
                float currentTime = currentFrames / mmdFps;
                Debug.Print($"{roundedFrames}f, {sigDisp}, {currentTime}s");

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
                        AddKeyName("Ready", roundedFrames - 1, 0.0f, keyframesSort);
                    }

                    AddKeyName("Ready", roundedFrames, 1.0f, keyframesSort);

                    if (sigDisp >= 1.1f)
                    {
                        if (startFrame > 1) // 上にまとめず個別に条件分岐します
                        {
                            AddKeyName("One_3", roundedFrames - 1, 0.0f, keyframesSort);
                            AddKeyName("One_4", roundedFrames - 1, 0.0f, keyframesSort);
                        }
                        AddKeyName("One_3", roundedFrames, 1.0f, keyframesSort);
                        AddKeyName("One_4", roundedFrames, 1.0f, keyframesSort);
                    }
                    if (sigDisp >= 10.1f)
                    {
                        if (startFrame > 1) // 上にまとめず個別に条件分岐します
                        {
                            AddKeyName("One_2", roundedFrames - 1, 0.0f, keyframesSort);
                        }
                        AddKeyName("One_2", roundedFrames, 1.0f, keyframesSort);
                    }
                    if (sigDisp >= 100.1f)
                    {
                        if (startFrame > 1) // 上にまとめず個別に条件分岐します
                        {
                            AddKeyName("One_1", roundedFrames - 1, 0.0f, keyframesSort);
                        }
                        AddKeyName("One_1", roundedFrames, 1.0f, keyframesSort);
                    }
                    keyframesSort.Sort(
                        (a, b) => Convert.ToInt32(a[1]) - Convert.ToInt32(b[1]));
                    keyframes.AddRange(keyframesSort);
                }
                else if (sigDisp == 10.1f)
                {
                    AddKeyName("One_2", roundedFrames - 1, 0.0f, keyframes);
                    AddKeyName("One_2", roundedFrames, 1.0f, keyframes);
                }
                else if (sigDisp == 100.1f)
                {
                    AddKeyName("One_1", roundedFrames - 1, 0.0f, keyframes);
                    AddKeyName("One_1", roundedFrames, 1.0f, keyframes);
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
            //クリーンアップ後で追加
            return keyframesHeader;
        }
    }
}
