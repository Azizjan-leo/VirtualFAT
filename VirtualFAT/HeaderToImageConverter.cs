using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace VirtualFAT
{
    /// <summary>
    /// Converts a full path to a specific image type of a drive, folder or file
    /// </summary>
    [ValueConversion(typeof(string), typeof(BitmapImage))]
    public class HeaderToImageConverter : IValueConverter
    {
        public static HeaderToImageConverter Instance = new HeaderToImageConverter();
        /// <summary>
        /// To convert value id to appropriate image
        /// </summary>
        /// <param name="value">The id</param>
        /// <param name="targetType">BitmapImage</param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string stringId = value.ToString();
            int id = Int32.Parse(stringId.Substring(stringId.Length-1));
            var theItem = FakeOS.Volume.GetTreeItem(id);
            return new BitmapImage(new Uri($"pack://application:,,,/Images/{theItem.Type}.png"));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
