using PictureMoverGui.Store;
using System.Collections.Generic;

namespace PictureMoverGui.ViewModels
{
    public class StatusInfoViewModel : ViewModelBase
    {
        private MasterStore _masterStore;

        public IEnumerable<string> StatusMessageList => _masterStore.RunningStore.StatusMessageLogList;

        public StatusInfoViewModel(MasterStore masterStore)
        {
            _masterStore = masterStore;

            _masterStore.RunningStore.StatusMessageLogAdded += RunningStore_StatusMessageLogAdded;
        }

        public override void Dispose()
        {
            base.Dispose();

            _masterStore.RunningStore.StatusMessageLogAdded -= RunningStore_StatusMessageLogAdded;
        }

        protected void RunningStore_StatusMessageLogAdded(string statusLog)
        {
            OnPropertyChanged(nameof(StatusMessageList));
        }
    }
}
