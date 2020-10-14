using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FacebookGetLink.ControllerAction
{
    public delegate void Action(Control sender);
    public class CustomInvoker
    {
        public static void RunInvoker(Control control, Action action)
        {
            control.Invoke(new MethodInvoker(() =>
            {
                action(control);
            }));
        }
    }
}
