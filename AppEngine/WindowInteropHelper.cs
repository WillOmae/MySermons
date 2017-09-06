using System;
using System.Windows.Forms;

namespace AppEngine
{
    public class WindowInteropHelper
    {
        private Form form;

        public WindowInteropHelper(Form formIn)
        {
            form = formIn;
        }

        public IntPtr Handle { get; internal set; }
    }
}
