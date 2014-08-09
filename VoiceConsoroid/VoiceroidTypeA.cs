using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoiceConsoroid
{
    /// <summary>
    /// ゆかりさん、まきさんタイプのGUIを実装したVoiceroid用.
    /// </summary>
    public abstract class VoiceroidTypeA : Voiceroid
    {
        const string saveDiagTitle = "音声ファイルの保存";

        /// <summary>
        /// 保存ダイアログ,ファイルパス指定ウィンドウハンドルの位置.
        /// </summary>
        const int TextBoxIndex = 3;
        /// <summary>
        /// 保存ダイアログ,保存（S）ボタンウィンドウハンドルの位置.
        /// </summary>
        const int SaveBtnIndex = 19;

        protected VoiceroidTypeA(Process voiceroid)
            : base(voiceroid)
        {
        }

        protected abstract void ResetText();

        public override void CopyAndPaste(string text, int waitingTime = 100)
        {
            IntPtr textArea = FindTextHwnd();

            // 今あるテキストを削除.
            ResetText();

            // 別スレッドを起動してクリップボードにテキストを転送.
            System.Threading.Thread.Sleep(waitingTime);
            SystemHelper.Clip(text);
            System.Threading.Thread.Sleep(waitingTime);
            // クリップボードにコピーしたテキストを貼り付け.
            User32Util.SendMessageSafety(_process.MainWindowHandle, WM.COMMAND, ACTION.PASTE, 0);
        }

        public override void Play()
        {
            IntPtr playButton = findPlayButtoon();
            User32Util.PostMessageLocal(playButton, WM.NULL, 0, 0);
        }

        protected override bool SaveImpl(string path, int waitingTime = 1000)
        {
            IntPtr hwndPtr = FindSaveButton();

            // 保存ボタン押す。
            User32Util.PostMessageLocal(hwndPtr, WM.NULL, 0, 0);

            System.Threading.Thread.Sleep(waitingTime);

            IntPtr saveDiagHwnd = User32Util.FindLocalWindow(saveDiagTitle);
            if(saveDiagHwnd == IntPtr.Zero)
            {
                Console.WriteLine("Can't Find Save Dialog.");
                return false;
            }

            List<IntPtr> hwnds = User32Util.ListChildHwnds(saveDiagHwnd);

            IntPtr hFilenameTextBox = hwnds[TextBoxIndex];
            IntPtr hSaveButton = hwnds[SaveBtnIndex];

            // テキストボックスにテキスト挿入.
            User32Util.SendMessageSafety(hFilenameTextBox, WM.ADDSTRING, ACTION.NULL, path);

            // 下キーを送信しPathを表示
            User32Util.SendMessageSafety(hFilenameTextBox, WM.KEYDOWN, ACTION.VK_DOWN, 0);
            System.Threading.Thread.Sleep(100);

            // 保存ボタンクリック
            User32Util.PostMessageLocal(hSaveButton, WM.CLICK, 0, 0);

            return true;
        }

        /// <summary>
        /// 保存ボタンのウィンドウハンドルを取得する.
        /// </summary>
        /// <returns></returns>
        protected IntPtr FindSaveButton()
        {
            /*
             * ウィンドウハンドル位置.
             * 
             * mainHwnd
             *   + wndHwnd
             *     + wndHwnd
             *     + wndHwnd
             *       + wndHwnd
             *       + wndHwnd
             *       + <This one>wndHwnd
             */
            IntPtr hWndWorkPtr = _process.MainWindowHandle;
            hWndWorkPtr = User32Util.MoveWindow(hWndWorkPtr, GW_COMMAND.DOWN);
            hWndWorkPtr = User32Util.MoveWindow(hWndWorkPtr, GW_COMMAND.DOWN);
            hWndWorkPtr = User32Util.MoveWindow(hWndWorkPtr, GW_COMMAND.NEXT);
            hWndWorkPtr = User32Util.MoveWindow(hWndWorkPtr, GW_COMMAND.DOWN);
            hWndWorkPtr = User32Util.MoveWindow(hWndWorkPtr, GW_COMMAND.NEXT);
            return User32Util.MoveWindow(hWndWorkPtr, GW_COMMAND.NEXT);
        }

        /// <summary>
        /// 再生ボタンのウィンドウハンドルを取得する.
        /// </summary>
        /// <returns></returns>
        protected IntPtr findPlayButtoon()
        {
            /*
             * ウィンドウハンドル位置.
             * 
             * mainHwnd
             *   + wndHwnd
             *     + wndHwnd
             *     + wndHwnd
             *       + wndHwnd
             *       + wndHwnd
             *       + wndHwnd
             *       + wndHwnd
             *       + <This one>wndHwnd
             */
            IntPtr hWndWorkPtr = _process.MainWindowHandle;
            hWndWorkPtr = User32Util.MoveWindow(hWndWorkPtr, GW_COMMAND.DOWN);
            hWndWorkPtr = User32Util.MoveWindow(hWndWorkPtr, GW_COMMAND.DOWN);
            hWndWorkPtr = User32Util.MoveWindow(hWndWorkPtr, GW_COMMAND.NEXT);
            hWndWorkPtr = User32Util.MoveWindow(hWndWorkPtr, GW_COMMAND.DOWN);
            hWndWorkPtr = User32Util.MoveWindow(hWndWorkPtr, GW_COMMAND.NEXT);
            hWndWorkPtr = User32Util.MoveWindow(hWndWorkPtr, GW_COMMAND.NEXT);
            hWndWorkPtr = User32Util.MoveWindow(hWndWorkPtr, GW_COMMAND.NEXT);
            return User32Util.MoveWindow(hWndWorkPtr, GW_COMMAND.NEXT);
        }


        /// <summary>
        /// 読み上げテキストのウィンドウハンドルを取得する.
        /// </summary>
        /// <returns></returns> 
        protected IntPtr FindTextHwnd()
        {
            /*
             * ウィンドウハンドル位置.
             * 
             * mainHwnd
             *   + wndHwnd
             *     + wndHwnd
             *     + wndHwnd
             *     + wndHwnd
             *       + wndHwnd
             *       + <This one>wndHwnd
             */
            IntPtr hwndPtr = _process.MainWindowHandle;
            hwndPtr = User32Util.MoveWindow(hwndPtr, GW_COMMAND.DOWN);
            hwndPtr = User32Util.MoveWindow(hwndPtr, GW_COMMAND.DOWN);
            hwndPtr = User32Util.MoveWindow(hwndPtr, GW_COMMAND.NEXT);
            hwndPtr = User32Util.MoveWindow(hwndPtr, GW_COMMAND.NEXT);
            hwndPtr = User32Util.MoveWindow(hwndPtr, GW_COMMAND.DOWN);
            return User32Util.MoveWindow(hwndPtr, GW_COMMAND.NEXT);
        }
    }

    public class Yukaroid : VoiceroidTypeA
    {
        private static Yukaroid INSTANCE;

        public static Yukaroid getInstance()
        {
            if(INSTANCE != null)
            {
                return INSTANCE;
            }

            Process voiceroid = SystemHelper.FindProcess("VOICEROID＋ 結月ゆかり");
            if (voiceroid == null)
            {
                return null;
            }

            INSTANCE = new Yukaroid(voiceroid);
            return INSTANCE;
        }

        private Yukaroid(Process voiceroid)
            : base(voiceroid)
        {
        }

        protected override void ResetText()
        {
            User32Util.SendMessageSafety(_process.MainWindowHandle, WM.COMMAND, ACTION.SELECTALL, 0);
            User32Util.SendMessageSafety(_process.MainWindowHandle, WM.COMMAND, ACTION.CUT, 0);
        }
    }

    public class Makiroid : VoiceroidTypeA
    {
        private static Makiroid INSTANCE;

        public static Makiroid getInstance()
        {
            if (INSTANCE != null)
            {
                return INSTANCE;
            }

            Process voiceroid = SystemHelper.FindProcess("VOICEROID＋ 民安ともえ");
            if (voiceroid == null)
            {
                return null;
            }

            INSTANCE = new Makiroid(voiceroid);
            return INSTANCE;
        }

        private Makiroid(Process voiceroid)
            : base(voiceroid)
        {
        }

        protected override void ResetText()
        {
            // まさかの全選択が無いとはっ マジックナンバー46orz
            User32Util.SendMessageSafety(_process.MainWindowHandle, WM.COMMAND, 46, 0);
        }
    }
}
