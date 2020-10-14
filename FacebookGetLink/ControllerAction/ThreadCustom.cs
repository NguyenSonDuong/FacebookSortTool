using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FacebookGetLink.ControllerAction
{
    public delegate void RunThread();
    public class ThreadCustom
    {
        public static void StartThread(RunThread run)
        {
            Thread th = new Thread(()=> {
                run();
            });
            th.IsBackground = true;
            th.Start();
        }
    }
}
