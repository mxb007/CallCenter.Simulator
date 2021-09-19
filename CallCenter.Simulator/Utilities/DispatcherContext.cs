using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CallCenter.Simulator.Utilities
{
    public class DispatcherContext
    {
        public static void InvokeIfRequired(Action action)
        {
            var dispatcher = Application.Current.Dispatcher;

            if (!dispatcher.CheckAccess())
            {
                dispatcher.Invoke(action);
                return;
            }

            action();
        }
    }
}
