using System;
using System.ComponentModel;

namespace DesktopWpfLib.Models
{
    public class ApplicationView: INotifyPropertyChanged
    {
        public int Id { get; set; }
        public DateTime Datetime { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
        private string _status;
        public string Status { 
            get { return _status; }
            set
            {
                if (_status != value)
                {
                    _status = value;
                    OnPropertyChanged(nameof(Status));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
