using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;

namespace PictureMoverGui
{
    [Serializable]
    public class EventData : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string arg)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(arg));
            }
        }

        public EventData(string Name, EventDateTime StartDateTime, EventDateTime EndDateTime)
        {
            this._name = Name;
            this._startDateTime = StartDateTime;
            this._endDateTime = EndDateTime;
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set 
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }

        private EventDateTime _startDateTime;
        public EventDateTime StartDateTime
        {
            get { return _startDateTime; }
            set
            {
                _startDateTime = value;
                OnPropertyChanged("StartDateTime");
            }
        }

        private EventDateTime _endDateTime;
        public EventDateTime EndDateTime
        {
            get { return _endDateTime; }
            set
            {
                _endDateTime = value;
                OnPropertyChanged("EndDateTime");
            }
        }

        [NonSerialized] private bool _edit;
        public bool Edit
        {
            get { return _edit; }
            set
            {
                _edit = value;
                OnPropertyChanged("Edit");
                OnPropertyChanged("ShowEditableView");
                OnPropertyChanged("ShowNormalView");
            }
        }

        public Visibility ShowEditableView => Edit ? Visibility.Visible : Visibility.Collapsed;
        public Visibility ShowNormalView => !Edit ? Visibility.Visible : Visibility.Collapsed;
    }

    [Serializable]
    public class EventDateTime: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string arg)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(arg));
            }
        }

        public static readonly List<string> StaticListOfValidHours = new List<string>() { "00", "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23" };
        public static readonly List<string> StaticListOfValidMinutesAndSeconds = new List<string>() { "00", "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "30", "31", "32", "33", "34", "35", "36", "37", "38", "39", "40", "41", "42", "43", "44", "45", "46", "47", "48", "49", "50", "51", "52", "53", "54", "55", "56", "57", "58", "59" };

        public EventDateTime(DateTime dateTime) : 
            this(dateTime.Date.ToString("dd.MM.yyyy"), dateTime.Hour.ToString("D2"), dateTime.Minute.ToString("D2"), dateTime.Second.ToString("D2"))
        {}
        public EventDateTime(string date, string hour, string minute, string second)
        {
            this._date = date;
            this._hour = hour;
            this._minute = minute;
            this._second = second;
        }

        private string _date;
        public string Date
        {
            get { return _date; }
            set 
            {
                _date = value;
                OnPropertyChanged("Date");
                OnPropertyChanged("DateTimePrettyString");
            }
        }

        private string _hour;
        public string Hour
        {
            get { return _hour; }
            set
            {
                _hour = value;
                OnPropertyChanged("Hour");
                OnPropertyChanged("DateTimePrettyString");
            }
        }

        private string _minute;
        public string Minute
        {
            get { return _minute; }
            set
            {
                _minute = value;
                OnPropertyChanged("Minute");
                OnPropertyChanged("DateTimePrettyString");
            }
        }

        private string _second;
        public string Second
        {
            get { return _second; }
            set
            {
                _second = value;
                OnPropertyChanged("Second");
                OnPropertyChanged("DateTimePrettyString");
            }
        }

        public string DateTimePrettyString
        {
            get { return $"{Date} {Hour}:{Minute}:{Second}"; }
        }

        public List<string> ListOfValidHours => StaticListOfValidHours;
        public List<string> ListOfValidMinutesAndSeconds => StaticListOfValidMinutesAndSeconds;
    }
}
