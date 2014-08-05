using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace VoiceConsoroid
{
    /// <summary>
    /// Subset of Windows Messages.
    /// </summary>
    /// <see cref="http://wiki.winehq.org/List_Of_Windows_Messages"/> for all of them.
    static class WM
    {
        public const uint NULL    = 0X0000;
        public const uint KEYDOWN = 0X0100;
        public const uint COMMAND = 0X0111;
        //might be WM_LBUTTONDOWN, WM_LBUTTONUP, WM_LBUTTONDBLCLK, and WM_LBUTTONUP
        public const uint CLICK = 0X0F5;
        public const uint ADDSTRING = 0X0143;
    }

    /// <summary>
    /// Subset of actions.
    /// </summary>
    static class ACTION
    {
        public const uint NULL = 0X00;
        public const uint VK_DOWN = 0X28;
        public const uint CUT = 0X34; //52
        public const uint PASTE = 0X38; // 56
        public const uint SELECTALL = 0X3C; //60
    }

    /// <summary>
    /// GetWindow Command.
    /// Currently, Voiceroid Talker only need next and down.
    /// </summary>
    public enum GW_COMMAND : uint
    {
        NEXT = 2,
        DOWN = 5
    }

    /// <summary>
    /// user32.dllの関数をラップするユーティリティクラス。
    /// </summary>
    static class User32Util
   { 
        /// <summary>
        /// EnumChildWindows実行時のコールバック関数。
        /// </summary>
        delegate void EnumWndCallback(IntPtr hwnd, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern IntPtr SetFocus(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern int EnumChildWindows(IntPtr hWnd, EnumWndCallback lpEnumFunc, IntPtr lParam);

        [DllImport("user32.dll")]
        private static extern IntPtr GetWindow(IntPtr hWnd, GW_COMMAND uCmd);

        /// <summary>
        /// 指定された文字列と一致するクラス名とウィンドウ名を持つトップレベルウィンドウ（ 親を持たないウィンドウ）のハンドルを返します。
        /// この関数は、子ウィンドウは探しません。検索では、大文字小文字は区別されません。
        /// </summary>
        /// <see cref="http://msdn.microsoft.com/ja-jp/library/cc364634.aspx"/>
        /// <param name="lpszClass"></param>
        /// <param name="lpszWindow"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        private static extern IntPtr FindWindow(String lpszClass, String lpszWindow);

        [DllImport("user32.dll")]
        private static extern int PostMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, uint Msg, int wParam, string lParam);

        /// <summary>
        /// user32.dll SendMessageTimeout関数。
        /// </summary>
        /// <remarks>
        /// SetLastError = trueによりMarshal.GetLastWin32Error();で発生したエラーを取得できる（はず）
        /// CharSet = CharSet.Auto...文字セットの指定, 実行時に ANSI 形式または Unicode 形式を選択。
        /// </remarks>
        /// <see cref="http://msdn.microsoft.com/ja-jp/library/7b93s42f(v=vs.110).aspx"/>
        /// <param name="windowHandle"></param>
        /// <param name="Msg"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <param name="flags"></param>
        /// <param name="timeout"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessageTimeout(
            IntPtr windowHandle,
            uint Msg,
            uint wParam,
            string lParam,
            SmtoFlag flags,
            uint timeout,
            out IntPtr result);

        /// <summary>
        /// user32.dll SendMessageTimeout関数。
        /// </summary>
        /// <see cref="http://msdn.microsoft.com/ja-jp/library/cc411010.aspx"/>
        /// <param name="windowHandle"></param>
        /// <param name="Msg"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <param name="flags"></param>
        /// <param name="timeout"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessageTimeout(
            IntPtr windowHandle,
            uint Msg,
            uint wParam,
            uint lParam,
            SmtoFlag flags,
            uint timeout,
            out IntPtr result);

        /// <summary>
        /// Send message timeout(Smto) flags.
        /// </summary>
        private enum SmtoFlag : uint
        {
            NORMAL = 0x0,
            BLOCK = 0x1,
            ABORTIFHUNG = 0x2,
            NOTIMEOUTIFNOTHUNG = 0x8,
            ERRORONEXIT = 0x0020
        }

        /**
         * Command identifier of GetWindow.
         */
        public const uint GW_HWNDNEXT = 2;
        public const uint GW_CHILD = 5;

        private const int TIME_WAIT_SENDMSG = 20000;

        /// <summary>
        /// 指定した名前を持つウィンドウを探す。
        /// 内部的にはuser32.dll FindWindow関数をコールしています。
        /// 見つからない場合はIntPtr.Zeroを返します。
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public static IntPtr FindLocalWindow(string title)
        {
            IntPtr hWnd = FindWindow(null, title);
            if (hWnd != null)
            {
                return hWnd;
            }
            return IntPtr.Zero;
        }

        /// <summary>
        /// 指定したウィンドウを起点にウィンドウを移動する。
        /// 今のところ隣接要素(GW_COMMAND.NEXT)、または子要素(GW_COMMAND.DOWN)への移動を想定しています。
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="command"></param>
        /// <returns>移動後のウィンドウ</returns>
        public static IntPtr MoveWindow(IntPtr hwnd, GW_COMMAND command)
        {
            return GetWindow(hwnd, command);
        }

        /// <summary>
        /// 指定したウィンドウの子要素を全て取得する。
        /// 注意：この関数は階層構造を考慮しません。単純に全ての子要素を列挙します。
        /// </summary>
        /// <param name="hwndRoot"></param>
        /// <returns></returns>
        public static List<IntPtr> ListChildHwnds(IntPtr hwndRoot)
        {
            List<IntPtr> hwndList = new List<IntPtr>();
            EnumWndCallback callback = (IntPtr handle, IntPtr pointer) =>
            {
                hwndList.Add(handle);
            };
            EnumChildWindows(hwndRoot, callback, new IntPtr(0));
            return hwndList;
        }

        /// <summary>
        /// 指定したウィンドウにメッセージを送る。
        /// 20000ms後に応答が無い場合はタイムアウトし、IntPtr.Zeroを返しますはずです。
        /// </summary>
        /// <param name="wndHandle"></param>
        /// <param name="wm"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        public static IntPtr SendMessageSafety(IntPtr wndHandle, uint wm, uint wParam, uint lParam)
        {
            IntPtr useless;
            return SendMessageTimeout(wndHandle, wm, wParam, lParam,
                SmtoFlag.ABORTIFHUNG, TIME_WAIT_SENDMSG, out useless);
        }

        /// <summary>
        /// 指定したウィンドウにメッセージを送る。
        /// lParam(メッセージの 2 番目のパラメータ)に文字列を指定できる版。
        /// 20000ms後に応答が無い場合はタイムアウトし、IntPtr.Zeroを返しますはずです。
        /// </summary>
        /// <param name="wndHandle"></param>
        /// <param name="wm"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        public static IntPtr SendMessageSafety(IntPtr wndHandle, uint wm, uint wParam, string lParam)
        {
            IntPtr useless;
            return SendMessageTimeout(wndHandle, wm, wParam, lParam,
                SmtoFlag.ABORTIFHUNG, TIME_WAIT_SENDMSG, out useless);
        }

        /// <summary>
        /// 指定したウィンドウにメッセージを送信する。
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="msg"></param>
        /// <param name="wparam"></param>
        /// <param name="lparam"></param>
        /// <returns></returns>
        public static int PostMessageLocal(IntPtr hwnd, uint msg, int wparam, int lparam)
        {
            return PostMessage(hwnd, msg, wparam, lparam);
        }
    }
}
