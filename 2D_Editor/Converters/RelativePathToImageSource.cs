using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace _2D_Editor.Converters
{
    public class RelativePathToImageSource : IValueConverter
    {
        public string tempDirBase
        {
            get
            {
                return Path.GetTempPath().ToString();
            }
        }
        public const string tempSuffix = "ImPres\\presentation\\";
        public string tempPresDir
        {
            get
            {
                return tempDirBase + tempSuffix;
            }
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return errorImage();
            string relpath = value.ToString();
            string completePath = tempPresDir + relpath;

            if (!File.Exists(completePath)) return errorImage();
            BitmapImage newBitmap = new BitmapImage(new Uri(completePath));
            return newBitmap;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public BitmapImage errorImage()
        {
            return new BitmapImage(new Uri(@"pack://application:,,,/" + Assembly.GetExecutingAssembly().GetName().Name + ";component/" + "Images/picture.png", UriKind.Absolute));
        }
    }
}
