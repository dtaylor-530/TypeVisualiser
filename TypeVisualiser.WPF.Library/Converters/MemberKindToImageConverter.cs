namespace TypeVisualiser.UI.Converters
{
    using System;
    using System.Globalization;
    using System.Windows.Data;
    using Model.Persistence;

    public class MemberKindToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }

            var memberKind = (MemberKind)value;
            switch (memberKind)
            {
                case MemberKind.Field:
                    return "/TypeVisualiser.WPF.Library;component/Assets/Field.png";
                case MemberKind.Method:
                    return "/TypeVisualiser.WPF.Library;component/Assets/Method.png";
                default:
                    return "/TypeVisualiser.WPF.Library;component/Assets/Property.png";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}