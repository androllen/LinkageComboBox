using LinkageComboBox.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using System.Windows.Markup;

namespace LinkageComboBox.Extensions
{
    [MarkupExtensionReturnType(typeof(IValueConverter))]
    public class EnvToNameConverterExtension : MarkupExtension, IValueConverter
    {
        private static EnvToNameConverterExtension _converter;
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return Binding.DoNothing;
            }
            return GetArray(value);
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        private Array GetArray(object value)
        {
            Type actualEnumType = typeof(PocName);
            Array enumValues = Enum.GetValues(actualEnumType);
            List<PocName> pocNames = ((PocName[])enumValues).ToList();
            var temp = pocNames.Where(p => p.GetDescription() == EnumHelper.GetDescription((PocEnv)value)).ToArray();

            Array tempArray = Array.CreateInstance(actualEnumType, temp.Length);
            temp.CopyTo(tempArray, 0);
            return tempArray;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (_converter == null)
            {
                _converter = new EnvToNameConverterExtension();
            }
            return _converter;
        }

    }
}
