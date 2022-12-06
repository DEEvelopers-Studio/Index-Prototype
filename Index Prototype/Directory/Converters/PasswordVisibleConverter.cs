using MahApps.Metro.IconPacks;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Index_Prototype.Directory.Converters
{
    [ValueConversion(typeof(string), typeof(PackIconControl))]
    internal class PasswordVisibleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? PackIconFontAwesomeKind.EyeSlashSolid : PackIconFontAwesomeKind.EyeRegular;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {

            return (PackIconFontAwesomeKind)value == PackIconFontAwesomeKind.EyeSlashSolid;
        }
    }
}
