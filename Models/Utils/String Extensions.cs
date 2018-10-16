using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.Utils
{
    public static class StringExtensions
    {
        public static string ToHex ( this string input )
        {
            StringBuilder sb = new StringBuilder();
            foreach ( char c in input )
                sb.AppendFormat( "0x{0:X2} ", ( int ) c );
            return sb.ToString( ).Trim( );
        }
    }
}
