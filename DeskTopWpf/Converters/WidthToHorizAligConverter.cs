using DesktopWpfLib.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;

namespace DeskTopWpf
{
    internal class WidthToHorizAligConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double width && parameter is string thresholdValue && double.TryParse(thresholdValue, out double threshold))
            {
                if (width < threshold)
                {
                    return HorizontalAlignment.Right;
                }
                return HorizontalAlignment.Left;
            }
            return HorizontalAlignment.Right;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
