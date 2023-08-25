using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopWpfLib.Models
{
    public class MenuPanelModel : INotifyPropertyChanged
    {
        private int _buttonFlag;
        public int ButtonFlag
        {
            get { return _buttonFlag; }
            set
            {
                if (_buttonFlag != value)
                {
                    _buttonFlag = value;
                    OnPropertyChanged(nameof(ButtonFlag));
                }
            }
        }

        //private double _windowWidth;
        //public double WindowWidth
        //{
        //    get { return _windowWidth; }
        //    set
        //    {
        //        if (_windowWidth != value)
        //        {
        //            _windowWidth = value;
        //            OnPropertyChanged(nameof(WindowWidth));
        //        }
        //    }
        //}

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
