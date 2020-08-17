using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace _2D_Editor.Converters
{
    class StringToFontsize : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int intValue;
            if (int.TryParse(value.ToString(), out intValue))
            {
                return intValue;
            }
            //default fontsize
            return 40;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int intValue;
            if (int.TryParse(value.ToString(), out intValue))
            {
                return intValue;
            }
            //default fontsize
            return 40;
        }
    }
}
