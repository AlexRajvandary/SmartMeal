using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Windows.Data;

namespace WpfSmsTestClient.Converters
{
    public class PropertyDescriptionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string propertyName && parameter is Type modelType)
            {
                var prop = modelType.GetProperty(propertyName);
                if (prop != null)
                {
                    var attr = prop.GetCustomAttribute<DescriptionAttribute>();
                    if (attr != null && !string.IsNullOrEmpty(attr.Description))
                    {
                        return attr.Description;
                    }
                }
                return propertyName;
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
