using PictureMoverGui.Commands;
using PictureMoverGui.Models;
using PictureMoverGui.Store;
using PictureMoverGui.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace PictureMoverGui.SubViewModels
{
    public class EventEditViewModel : ViewModelBase
    {
        private MasterStore _masterStore;

        private string _eventName;
        public string EventName
        {
            get => _eventName;
            set
            {
                if (_eventName != value)
                {
                    _eventName = value;
                    OnPropertyChanged(nameof(EventName));
                }
            }
        }

        private DateTime _startDateTime;
        public DateTime StartDateTime => _startDateTime;
        public string EventStartDateTime => _startDateTime.ToString();
        public string EventStartDate
        {
            get => _startDateTime.Date.ToString();
            set
            {
                if (_startDateTime.Date.ToString() != value)
                {
                    string[] dateValues = value.Split('.');
                    _startDateTime = new DateTime(
                        int.Parse(dateValues[2]), 
                        int.Parse(dateValues[1]), 
                        int.Parse(dateValues[0]), 
                        _startDateTime.Hour, 
                        _startDateTime.Minute, 
                        _startDateTime.Second);
                    OnPropertyChanged(nameof(EventStartDate));
                    OnPropertyChanged(nameof(EventStartDateTime));
                    OnPropertyChanged(nameof(ErrorVisibility));
                }
            }
        }
        public string EventStartHour
        {
            get => _startDateTime.Hour.ToString("00");
            set
            {
                if (_startDateTime.Hour.ToString() != value)
                {
                    _startDateTime = new DateTime(
                        _startDateTime.Year,
                        _startDateTime.Month,
                        _startDateTime.Day,
                        int.Parse(value),
                        _startDateTime.Minute,
                        _startDateTime.Second);
                    OnPropertyChanged(nameof(EventStartHour));
                    OnPropertyChanged(nameof(EventStartDateTime));
                    OnPropertyChanged(nameof(ErrorVisibility));
                }
            }
        }
        public string EventStartMinute
        {
            get => _startDateTime.Minute.ToString("00");
            set
            {
                if (_startDateTime.Minute.ToString() != value)
                {
                    _startDateTime = new DateTime(
                        _startDateTime.Year,
                        _startDateTime.Month,
                        _startDateTime.Day,
                        _startDateTime.Hour,
                        int.Parse(value),
                        _startDateTime.Second);
                    OnPropertyChanged(nameof(EventStartMinute));
                    OnPropertyChanged(nameof(EventStartDateTime));
                    OnPropertyChanged(nameof(ErrorVisibility));
                }
            }
        }
        public string EventStartSecond
        {
            get => _startDateTime.Second.ToString("00");
            set
            {
                if (_startDateTime.Minute.ToString() != value)
                {
                    _startDateTime = new DateTime(
                        _startDateTime.Year,
                        _startDateTime.Month,
                        _startDateTime.Day,
                        _startDateTime.Hour,
                        _startDateTime.Minute,
                        int.Parse(value));
                    OnPropertyChanged(nameof(EventStartSecond));
                    OnPropertyChanged(nameof(EventStartDateTime));
                    OnPropertyChanged(nameof(ErrorVisibility));
                }
            }
        }

        private DateTime _endDateTime;
        public DateTime EndDateTime => _endDateTime;
        public string EventEndDateTime => _endDateTime.ToString();
        public string EventEndDate
        {
            get => _endDateTime.Date.ToString();
            set
            {
                if (_endDateTime.Date.ToString() != value)
                {
                    string[] dateValues = value.Split('.');
                    _endDateTime = new DateTime(
                        int.Parse(dateValues[2]),
                        int.Parse(dateValues[1]),
                        int.Parse(dateValues[0]),
                        _endDateTime.Hour,
                        _endDateTime.Minute,
                        _endDateTime.Second);
                    OnPropertyChanged(nameof(EventEndDate));
                    OnPropertyChanged(nameof(EventEndDateTime));
                    OnPropertyChanged(nameof(ErrorVisibility));
                }
            }
        }
        public string EventEndHour
        {
            get => _endDateTime.Hour.ToString("00");
            set
            {
                if (_endDateTime.Hour.ToString() != value)
                {
                    _endDateTime = new DateTime(
                        _endDateTime.Year,
                        _endDateTime.Month,
                        _endDateTime.Day,
                        int.Parse(value),
                        _endDateTime.Minute,
                        _endDateTime.Second);
                    OnPropertyChanged(nameof(EventEndHour));
                    OnPropertyChanged(nameof(EventEndDateTime));
                    OnPropertyChanged(nameof(ErrorVisibility));
                }
            }
        }
        public string EventEndMinute
        {
            get => _endDateTime.Minute.ToString("00");
            set
            {
                if (_endDateTime.Minute.ToString() != value)
                {
                    _endDateTime = new DateTime(
                        _endDateTime.Year,
                        _endDateTime.Month,
                        _endDateTime.Day,
                        _endDateTime.Hour,
                        int.Parse(value),
                        _endDateTime.Second);
                    OnPropertyChanged(nameof(EventEndMinute));
                    OnPropertyChanged(nameof(EventEndDateTime));
                    OnPropertyChanged(nameof(ErrorVisibility));
                }
            }
        }
        public string EventEndSecond
        {
            get => _endDateTime.Second.ToString("00");
            set
            {
                if (_endDateTime.Minute.ToString() != value)
                {
                    _endDateTime = new DateTime(
                        _endDateTime.Year,
                        _endDateTime.Month,
                        _endDateTime.Day,
                        _endDateTime.Hour,
                        _endDateTime.Minute,
                        int.Parse(value));
                    OnPropertyChanged(nameof(EventEndSecond));
                    OnPropertyChanged(nameof(EventEndDateTime));
                    OnPropertyChanged(nameof(ErrorVisibility));
                }
            }
        }

        public IEnumerable<string> ValidHours => Enumerable.Range(0, 24).Select(i => i.ToString("00"));
        public IEnumerable<string> ValidMinutesAndSeconds => Enumerable.Range(0, 60).Select(i => i.ToString("00"));

        public Visibility ErrorVisibility => _startDateTime <= _endDateTime ? Visibility.Hidden : Visibility.Visible;

        public ICommand SelectStartDate { get; }
        public ICommand SelectEndDate { get; }

        public EventEditViewModel(MasterStore masterStore)
        {
            _masterStore = masterStore;

            SelectStartDate = new CallbackCommand(OnSelectStartDate);
            SelectEndDate = new CallbackCommand(OnSelectEndDate);

            _eventName = "";
            _startDateTime = DateTime.Now;
            _endDateTime = DateTime.Now;
        }

        public void SetEventData(string name, DateTime startTime, DateTime endTime)
        {
            _eventName = name;
            _startDateTime = startTime;
            _endDateTime = endTime;
            OnPropertyChanged(nameof(EventName));
            OnPropertyChanged(nameof(EventStartDate));
            OnPropertyChanged(nameof(EventStartDateTime));
            OnPropertyChanged(nameof(EventStartHour));
            OnPropertyChanged(nameof(EventStartMinute));
            OnPropertyChanged(nameof(EventStartSecond));
            OnPropertyChanged(nameof(EventEndDate));
            OnPropertyChanged(nameof(EventEndDateTime));
            OnPropertyChanged(nameof(EventEndHour));
            OnPropertyChanged(nameof(EventEndMinute));
            OnPropertyChanged(nameof(EventEndSecond));
            OnPropertyChanged(nameof(ErrorVisibility));
        }

        protected void OnSelectStartDate(object parameter)
        {
            System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();
            var result = openFileDialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrEmpty(openFileDialog.FileName))
            {
                System.IO.FileInfo fileInfo = new System.IO.FileInfo(openFileDialog.FileName);
                SetEventData(EventName, fileInfo.LastWriteTime, EndDateTime);
            }
        }
        protected void OnSelectEndDate(object parameter)
        {
            System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();
            var result = openFileDialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrEmpty(openFileDialog.FileName))
            {
                System.IO.FileInfo fileInfo = new System.IO.FileInfo(openFileDialog.FileName);
                SetEventData(EventName, StartDateTime, fileInfo.LastWriteTime);
            }
        }
    }
}
