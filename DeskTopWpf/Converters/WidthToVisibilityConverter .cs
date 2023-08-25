using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;
using DesktopWpfLib.Models;

namespace DeskTopWpf
{
    public class WidthToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double width && parameter is string thresholdValue && double.TryParse(thresholdValue, out double threshold))
            {
                if (App.Current.MainWindow.DataContext is MenuPanelModel menuPanelModel)
                {
                    if (width < threshold)
                    {
                        return menuPanelModel.ButtonFlag == 0 ? Visibility.Collapsed : Visibility.Visible;
                    }
                    return menuPanelModel.ButtonFlag == 1 ? Visibility.Collapsed : Visibility.Visible;
                }
            }
            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
