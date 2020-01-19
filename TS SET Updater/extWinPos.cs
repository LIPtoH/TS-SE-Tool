using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;

namespace TS_SET_Updater
{
    public class extWinPos
    {
        public struct Rect
        {
            public int Left { get; set; }
            public int Top { get; set; }
            public int Right { get; set; }
            public int Bottom { get; set; }
        }

        //public int ProcessID { get; }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr FindWindow(string strClassName, string strWindowName);

        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hwnd, ref Rect rectangle);

        public Rect GetExtWinPos(int _ProcessID)
        {
            Process processe = Process.GetProcessById(_ProcessID);
            IntPtr ptr = processe.MainWindowHandle;
            Rect WindowRect = new Rect();
            GetWindowRect(ptr, ref WindowRect);

            return WindowRect;
        }

        public Rectangle GetExtWinRectangle(int _ProcessID)
        {
            Process processe = Process.GetProcessById(_ProcessID);
            IntPtr ptr = processe.MainWindowHandle;
            Rect WindowRect = new Rect();
            GetWindowRect(ptr, ref WindowRect);

            Rectangle _out = new Rectangle(WindowRect.Left, WindowRect.Top, WindowRect.Right - WindowRect.Left, WindowRect.Bottom - WindowRect.Top);

            return _out;
        }
    }
}
