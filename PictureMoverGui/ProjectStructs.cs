using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace PictureMoverGui
{
    [Serializable]
    public class EventStruct: INotifyPropertyChanged
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

        public EventStruct(string Name, EventDateTime StartDateTime, EventDateTime EndDateTime)
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
    }
}
