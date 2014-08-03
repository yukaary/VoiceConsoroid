using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoiceroidTalker
{
    /// <summary>
    /// C#が提供するWindows基本機能をまとめたクラス。
    /// VoiceroidTalkerが使用するものだけをまとめています。
    /// </summary>
    static class SystemHelper
    {
        /// <summary>
        /// 指定した名前を持つプロセスを探す。
        /// 見つからない場合はnullを返します。
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public static Process FindProcess(string title)
        {
            Process[] ps = Process.GetProcesses();
            foreach (Process pitem in ps)
            {
                if ((pitem.MainWindowHandle != IntPtr.Zero) && pitem.MainWindowTitle.Equals(title))
                {
                    return pitem;
                }
            }
            return null;
        }

        private const int CB_RETRY = 10;
        private const int CB_DELAY = 100;
        private const bool CB_KEEP = true;

        /// <summary>
        /// 指定したテキストのクリップボードにコピーする。
        /// 専用のスレッドを立ち上げています。安全のため実行の前後に100msくらい待機させることをお勧めします。
        /// </summary>
        /// <param name="text"></param>
        public static void Clip(String text)
        {
            System.Threading.Thread t = new System.Threading.Thread(SetClipboard);
            t.SetApartmentState(System.Threading.ApartmentState.STA);
            t.Start(text);
            t.Join();
        }

        /// <summary>
        /// クリップボードに指定したテキストを保存する.
        /// スレッドで実行されることを前提に実装しています。
        /// </summary>
        /// <param name="o"></param>
        static void SetClipboard(Object o)
        {
            System.Windows.Forms.Clipboard.SetDataObject((string)o, CB_KEEP, CB_RETRY, CB_DELAY);
        }

    }
}
