using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace _2D_Editor.Converters
{
    public class PercentageToAbsolute : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double maxValue;
            double percentageValue; //percentage in %
            if(double.TryParse(value.ToString(), out percentageValue) && double.TryParse(parameter.ToString(), out maxValue))
            {
                return (int)(percentageValue / 100 * maxValue);
            }
            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double maxValue;
            double absoluteValue;
            if (double.TryParse(value.ToString(), out absoluteValue) && double.TryParse(parameter.ToString(), out maxValue))
            {
                return (int)(absoluteValue / maxValue * 100);
            }
            return 0;
        }
    }
}
