using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Packet_Sender
{
    public class IpToStringConverter : IValueConverter
    {
        public Object Convert ( Object value, Type targetType, Object parameter, CultureInfo culture )
        {
            return value.ToString();
        }

        public Object ConvertBack ( Object value, Type targetType, Object parameter, CultureInfo culture )
        {
            System.Net.IPAddress addr   = null;
            Boolean hasParsed           = System.Net.IPAddress.TryParse( value.ToString( ), out addr );

            return addr;
        }
    }


}
