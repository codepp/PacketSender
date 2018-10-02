using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.Net.Sockets;
using System.ComponentModel;

namespace Model
{
    public enum ConnectionType
    {
        Unused  = 0,
        UDP     = 1,
        TCP     = 2
    }

}
