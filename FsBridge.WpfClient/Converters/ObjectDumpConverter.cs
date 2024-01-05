using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;

namespace FsBridge.WpfClient.Converters
{
    public class ObjectDumpConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                var sb = new StringBuilder();   
                foreach (var prop in value.GetType ().GetProperties (BindingFlags.Public | BindingFlags.Instance))
                {
                    sb.AppendLine($"{prop.Name}: {prop.GetValue(value)}");
                }
                return sb.ToString();
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
