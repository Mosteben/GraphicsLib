using System.Windows.Forms;

namespace System
{
    internal class MouseEventHandler
    {
        private Action<object, MouseEventArgs> p1_MouseClick;

        public MouseEventHandler(Action<object, MouseEventArgs> p1_MouseClick)
        {
            this.p1_MouseClick = p1_MouseClick;
        }
    }
}