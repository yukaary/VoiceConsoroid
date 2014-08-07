using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoiceConsoroid
{
    /// <summary>
    /// ずんちゃんタイプのボイスロイド用.
    /// </summary>
    public class VoiceroidTypeB : Voiceroid
    {
        private const uint BM_CLICK = 0X00F5;

        private const uint WM_SELALL = 0X00B1;
        private const uint WM_CUT = 0X0300;
        private const uint WM_COPY = 0X0301;
        private const uint WM_SETTEXT = 0X000C;

        const string saveDiagTitle = "音声ファイルの保存";

        private static VoiceroidTypeB INSTANCE;

        public static VoiceroidTypeB getInstance()
        {
            if (INSTANCE != null)
            {
                return INSTANCE;
            }

            Process voiceroid = SystemHelper.FindProcess("VOICEROID＋ 東北ずん子");

            if (voiceroid == null)
            {
                voiceroid = SystemHelper.FindProcess("VOICEROID＋ 東北ずん子*");
                if(voiceroid == null)
                {
                    return null;
                }
            }

            INSTANCE = new VoiceroidTypeB(voiceroid);
            return INSTANCE;            
        }

        private VoiceroidTypeB(Process voiceroid)
            : base(voiceroid)
        {
        }

        public override void CopyAndPaste(string text, int waitingTime = 100)
        {
            IntPtr textArea = FindTextHwnd();
            User32Util.SendMessageSafety(textArea, WM_SETTEXT, 0, "");
            User32Util.SendMessageSafety(textArea, WM_SETTEXT, 0, text);
        }

        public override void Play()
        {
            IntPtr playButton = findPlayButtoon();
            User32Util.SendMessageSafety(playButton, BM_CLICK, 0, 0);
        }

        protected override bool DoSave(string path, int waitingTime = 1000)
        {
            IntPtr hwndPtr = FindSaveButton();
            User32Util.SendMessageSafety(hwndPtr, BM_CLICK, 0, 0, 100);

            System.Threading.Thread.Sleep(waitingTime);

            IntPtr saveDiagHwnd = User32Util.FindLocalWindow(saveDiagTitle);
            if (saveDiagHwnd == IntPtr.Zero)
            {
                Console.WriteLine("Can't Find Save Dialog.");
                return false;
            }

            User32Util.SendMessageSafety(FindFilePathField(saveDiagHwnd), WM_SETTEXT, ACTION.NULL, path);
            User32Util.SendMessageSafety(FindSaveWavButton(saveDiagHwnd), BM_CLICK, 0, 0, 100);

            return true;
        }

        private static IntPtr FindFilePathField(IntPtr rootPtr)
        {
            IntPtr hwndPtr = rootPtr;
            hwndPtr = User32Util.MoveWindow(hwndPtr, GW_COMMAND.DOWN);
            hwndPtr = User32Util.MoveWindow(hwndPtr, GW_COMMAND.DOWN);
            hwndPtr = User32Util.MoveWindow(hwndPtr, GW_COMMAND.DOWN);
            hwndPtr = User32Util.MoveWindow(hwndPtr, GW_COMMAND.DOWN);
            return User32Util.MoveWindow(hwndPtr, GW_COMMAND.DOWN);
        }

        private static IntPtr FindSaveWavButton(IntPtr rootPtr)
        {
            IntPtr hwndPtr = rootPtr;
            hwndPtr = User32Util.MoveWindow(hwndPtr, GW_COMMAND.DOWN);
            hwndPtr = User32Util.MoveWindow(hwndPtr, GW_COMMAND.NEXT);
            return User32Util.MoveWindow(hwndPtr, GW_COMMAND.NEXT);
        }

        /// <summary>
        /// 読み上げテキストのウィンドウハンドルを取得する.
        /// </summary>
        /// <returns></returns> 
        protected IntPtr FindTextHwnd()
        {
            /*
             * ウィンドウハンドル位置.
             */
            IntPtr hwndPtr = _process.MainWindowHandle;
            hwndPtr = User32Util.MoveWindow(hwndPtr, GW_COMMAND.DOWN);
            hwndPtr = User32Util.MoveWindow(hwndPtr, GW_COMMAND.DOWN);
            hwndPtr = User32Util.MoveWindow(hwndPtr, GW_COMMAND.NEXT);
            hwndPtr = User32Util.MoveWindow(hwndPtr, GW_COMMAND.DOWN);
            hwndPtr = User32Util.MoveWindow(hwndPtr, GW_COMMAND.DOWN);
            hwndPtr = User32Util.MoveWindow(hwndPtr, GW_COMMAND.DOWN);
            hwndPtr = User32Util.MoveWindow(hwndPtr, GW_COMMAND.DOWN);
            return hwndPtr = User32Util.MoveWindow(hwndPtr, GW_COMMAND.DOWN);
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
             */
            IntPtr hWndWorkPtr = _process.MainWindowHandle;
            hWndWorkPtr = User32Util.MoveWindow(hWndWorkPtr, GW_COMMAND.DOWN);
            hWndWorkPtr = User32Util.MoveWindow(hWndWorkPtr, GW_COMMAND.DOWN);
            hWndWorkPtr = User32Util.MoveWindow(hWndWorkPtr, GW_COMMAND.NEXT);
            hWndWorkPtr = User32Util.MoveWindow(hWndWorkPtr, GW_COMMAND.DOWN);
            hWndWorkPtr = User32Util.MoveWindow(hWndWorkPtr, GW_COMMAND.DOWN);
            hWndWorkPtr = User32Util.MoveWindow(hWndWorkPtr, GW_COMMAND.DOWN);
            hWndWorkPtr = User32Util.MoveWindow(hWndWorkPtr, GW_COMMAND.DOWN);
            hWndWorkPtr = User32Util.MoveWindow(hWndWorkPtr, GW_COMMAND.DOWN);
            hWndWorkPtr = User32Util.MoveWindow(hWndWorkPtr, GW_COMMAND.NEXT);
            return User32Util.MoveWindow(hWndWorkPtr, GW_COMMAND.DOWN);
        }

        /// <summary>
        /// 保存ボタンのウィンドウハンドルを取得する.
        /// </summary>
        /// <returns></returns>
        protected IntPtr FindSaveButton()
        {
            /*
             * ウィンドウハンドル位置.
             */
            IntPtr hWndWorkPtr = _process.MainWindowHandle;
            hWndWorkPtr = User32Util.MoveWindow(hWndWorkPtr, GW_COMMAND.DOWN);
            hWndWorkPtr = User32Util.MoveWindow(hWndWorkPtr, GW_COMMAND.DOWN);
            hWndWorkPtr = User32Util.MoveWindow(hWndWorkPtr, GW_COMMAND.NEXT);
            hWndWorkPtr = User32Util.MoveWindow(hWndWorkPtr, GW_COMMAND.DOWN);
            hWndWorkPtr = User32Util.MoveWindow(hWndWorkPtr, GW_COMMAND.DOWN);
            hWndWorkPtr = User32Util.MoveWindow(hWndWorkPtr, GW_COMMAND.DOWN);
            hWndWorkPtr = User32Util.MoveWindow(hWndWorkPtr, GW_COMMAND.DOWN);
            hWndWorkPtr = User32Util.MoveWindow(hWndWorkPtr, GW_COMMAND.DOWN);
            hWndWorkPtr = User32Util.MoveWindow(hWndWorkPtr, GW_COMMAND.NEXT);
            hWndWorkPtr = User32Util.MoveWindow(hWndWorkPtr, GW_COMMAND.DOWN);
            hWndWorkPtr = User32Util.MoveWindow(hWndWorkPtr, GW_COMMAND.NEXT);            
            return User32Util.MoveWindow(hWndWorkPtr, GW_COMMAND.NEXT);
        }
    }
}
