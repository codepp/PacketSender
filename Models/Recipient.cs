using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.Net.Sockets;
using System.ComponentModel;
using Model;

namespace ViewModels
{
    public class Recipient : INotifyPropertyChanged
    {
        private IPAddress       m_Address;
        private UInt16          m_PortNumber;
        public ConnectionType   m_ConnectionType;

        public event PropertyChangedEventHandler PropertyChanged;

        public IPAddress Address
        {
            get 
            {
                return this.m_Address;
            }
            set 
            {
                this.m_Address                  = value;
                this.NotifyPropertyChanged("Address");
            }
        }

        public UInt16 PortNumber
        {
            get 
            {
                return this.m_PortNumber;
            }
            set 
            {
                this.m_PortNumber               = value;
                this.NotifyPropertyChanged("PortNumber");
            }
        }

        public IPEndPoint GetEndpoint( )
        {
            IPEndPoint returnValue  = null;

            returnValue             = new IPEndPoint( this.Address, this.PortNumber );
            return returnValue;
        }

        public ConnectionType ConnectionType
        {
            get 
            {
                return this.m_ConnectionType;
            }
            set
            {
                this.m_ConnectionType           = value;
                this.NotifyPropertyChanged("ConnectionType");
            }
        }

        public Recipient(IPAddress ipAddress, UInt16 port)
        {
            this.Address                        = ipAddress;
            this.PortNumber                     = port;
        }

        private void NotifyPropertyChanged(String propertyName)
        {
            if (this.PropertyChanged != null)
            {
                PropertyChangedEventArgs eA     = new PropertyChangedEventArgs(propertyName);
                this.PropertyChanged(this, eA);
            }
        }
    }
}
