using PictureMoverGui.Store;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace PictureMoverGui.Commands
{
    public class CallbackCommand : CommandBase
    {
        private MasterStore _masterStore;
        private Action _callback;

        public CallbackCommand(MasterStore masterStore, Action callback)
        {
            _masterStore = masterStore;
            _callback = callback;
        }

        public override void Execute(object parameter)
        {
            _callback();
            Debug.WriteLine($"Command executed. Name : {_masterStore.Name} , Desc : {_masterStore.Description} , Active : {_masterStore.Active}");
        }
    }
}
