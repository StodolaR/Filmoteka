using System.Globalization;
using System.IO;
using System.Windows.Data;

namespace Filmoteka.Framework
{
    class FullPathConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if ((value as string)[0] != ('/'))
                    return Path.GetFullPath(value as string);
            }
            catch
            {
                return "/Resources/bezobrazku.png";
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
