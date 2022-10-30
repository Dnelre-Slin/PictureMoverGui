using System;

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
