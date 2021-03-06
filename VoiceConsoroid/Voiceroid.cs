﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.IO;

namespace VoiceConsoroid
{
    public abstract class Voiceroid
    {
        protected Process _process { get; private set; }

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="name"></param>
        protected Voiceroid(Process process)
        {
            _process = process;
        }

        /// <summary>
        /// 指定したテキストをコピー＆ペーストでVoiceroidのテキストエリアに貼り付ける。
        /// </summary>
        /// <param name="text"></param>
        /// <param name="waitingTime"></param>
        public abstract void CopyAndPaste(string text, int waitingTime = 100);

        /// <summary>
        /// 再生ボタンを押してしゃべってもらう。
        /// </summary>
        public abstract void Play();

        /// <summary>
        /// 指定したファイルに音声（wav）を保存し、再生用オブジェクトを返す。
        /// 同名のファイルを上書きする点に注意してください。
        /// 保存に成功した場合、デフォルトで3秒間待機します。待機する必要がない場合は
        /// 明示的にsleepを0にしてください。
        /// </summary>
        /// <param name="path">保存パス</param>
        public async Task Save(String path)
        {
            string fullpath = DoSave(path);
            if(fullpath == null)
            {
                return;
            }
            await Task.Run(() => monitorVoiceGeneration(fullpath));
        }

        public async Task Record(String path)
        {
            string fullpath = DoSave(path);
            if (fullpath == null)
            {
                return;
            }
            await Task.Run(() => monitorVoiceGeneration(fullpath));
            new WavPlayer(fullpath).PlaySync();
        }

        private string DoSave(string path)
        {
            String fullpath = Path.GetFullPath(path);

            if (!CheckDirExistence(fullpath))
            {
                Console.WriteLine("{0} と同名のディレクトリが存在するため保存処理を中断します。");
                return null;
            }
            if (!SaveImpl(fullpath))
            {
                return null;
            }

            System.Threading.Thread.Sleep(500);
            return fullpath;
        }

        private void monitorVoiceGeneration(string filepath)
        {
            // 0.1sec間隔でwavファイル生成を待つ.
            while (!File.Exists(filepath))
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        /// <summary>
        /// 保存処理の本体。
        /// Voiceroidの種類別に異なりそう（具体的にはずんちゃん）なので抽象メソッド化してあります。
        /// </summary>
        /// <param name="path"></param>
        /// <param name="waitingTime">保存ボタンを押してから待機する時間(msec).</param>
        protected abstract bool SaveImpl(String path, int waitingTime = 1000);

        /// <summary>
        /// 指定した名前に一致するディレクトリの存在有無を確認する。
        /// 同名のファイルがある場合は保存処理の妨げになるので削除します。
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private static bool CheckDirExistence(String path)
        {
            if (Directory.Exists(path))
            {
                return false;
            }
            if (File.Exists(path))
            {
                Console.WriteLine("既存ファイルを削除。{0}", path);
                File.Delete(path);
            }
            return true;
        }
    }
}
