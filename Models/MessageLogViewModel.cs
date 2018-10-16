using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ViewModels.Utils;

namespace ViewModels
{
    public class MessageLogViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private DateTime    m_TimeSent;

        private IPAddress   m_SenderAddr;
        private UInt16      m_SenderPort;

        private IPAddress   m_RecipientAddr;
        private UInt16      m_RecipientPort;

        private String      m_Method;
        private String      m_Error;
        private String      m_Content;

        public  DateTime     Time
        {
            get                     => this.m_TimeSent;
            set 
            {
                this.m_TimeSent     = value;
                this.NotifyPropertyChanged( this, "Time" );
            }
        }

        public IPAddress Sender
        {
            get                     => this.m_SenderAddr;
            set 
            {
                this.m_SenderAddr   = value;
                this.NotifyPropertyChanged( this, "Sender" );
            }
        }

        public IPAddress Recipient
        {
            get                     => this.m_RecipientAddr;
            set {
                this.m_RecipientAddr = value;
                this.NotifyPropertyChanged( this, "Recipient" );
            }
        }

        public UInt16 SenderPort
        {
            get                     => this.m_SenderPort;
            set 
            {
                this.m_SenderPort   = value;
                this.NotifyPropertyChanged( this, "SenderPort" );
            }
        }

        public UInt16 RecipientPort
        {
            get                     => this.m_RecipientPort;
            set 
            {
                this.m_RecipientPort = value;
                this.NotifyPropertyChanged( this, "RecipientPort" );
            }
        }

        public String Method
        {
            get                     => this.m_Method;
            set 
            {
                this.m_Method       = value;
                this.NotifyPropertyChanged( this, "Method" );
            }
        }

        public String Error
        {
            get                     => this.m_Error;
            set 
            {
                this.m_Error        = value;
                this.NotifyPropertyChanged( this, "Error" );
            }
        }

        public String Ascii
        {
            get                     => this.m_Content;
        }

        public String Hex
        {
            get                     => this.m_Content.ToHex();
        }

        public String Content
        {
            set 
            {
                this.m_Content      = value;
                this.NotifyPropertyChanged( this, "Ascii" );
                this.NotifyPropertyChanged( this, "Hex" );

            }
        }

        public void UpdateAll ( )
        {
            this.NotifyPropertyChanged( this, "" );
        }

        public void NotifyPropertyChanged(object sender, String propertyName)
        {
            if ( this.PropertyChanged != null )
            {
                PropertyChangedEventArgs args   = new PropertyChangedEventArgs(propertyName);
                this.PropertyChanged( this, args );
            }
        }
    }
}
