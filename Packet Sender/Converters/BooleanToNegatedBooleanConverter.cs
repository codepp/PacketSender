
using System;
using System.Globalization;
using System.Windows.Data;

namespace Packet_Sender
{
    public class BooleanToNegatedBooleanConverter : IValueConverter
    {
        public Object Convert ( Object value, Type targetType, Object parameter, CultureInfo culture )
        {
            if ( targetType != typeof( Boolean ) )
                throw new InvalidOperationException( "The target must be a boolean" );

            return !( Boolean ) value;

        }

        public Object ConvertBack ( Object value, Type targetType, Object parameter, CultureInfo culture )
        {
            throw new NotSupportedException( );
        }
    }
}
