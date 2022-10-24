using PictureMoverGui.Store;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace PictureMoverGui.Commands
{
    public class CallbackCommand : CommandBase
    {
        private Action<object> _callback;

        public CallbackCommand(Action<object> callback)
        {
            _callback = callback;
        }

        public override void Execute(object parameter)
        {
            _callback(parameter);
        }
    }
}
