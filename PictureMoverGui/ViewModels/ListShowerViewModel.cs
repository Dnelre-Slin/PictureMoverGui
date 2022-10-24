using PictureMoverGui.Commands;
using PictureMoverGui.Models;
using PictureMoverGui.Store;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Input;

namespace PictureMoverGui.ViewModels
{
    public class ListShowerViewModel
    {
        private MasterStore _masterStore;
        public IEnumerable<FileData> FileDatas => _masterStore.FileDatas;

        public ICommand FalsifyList { get; }

        public ListShowerViewModel(MasterStore masterStore)
        {
            _masterStore = masterStore;

            FalsifyList = new CallbackCommand(OnFalsifyList);
        }

        protected void OnFalsifyList()
        {
            Debug.WriteLine("Falsifing list");
        }
    }
}
