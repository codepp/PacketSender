using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ViewModels
{
    public class PartialMessageSentEventArgs : EventArgs
    {
        private readonly    Int32       m_NumBytesSent;
        private readonly    String      m_PartialMessage;

        private readonly    IPAddress   m_RecipientAddr;
        private readonly    UInt16      m_RecipientPort;

        //private readonly    DateTime    
        public Int32        NumBytesSent    => this.m_NumBytesSent;
        public String       PartialMessage  => this.m_PartialMessage;
        
        public IPAddress    IPAddress       => this.m_RecipientAddr;
        public UInt16       Port            => this.m_RecipientPort;

        //public DateTime     TimeSent => this.m_Time;
        public PartialMessageSentEventArgs ( String partialMessage, Int32 numBytesSent, UInt16 port, IPAddress addr )
        {
            this.m_NumBytesSent     = numBytesSent;
            this.m_PartialMessage   = partialMessage;

            this.m_RecipientAddr    = addr;
            this.m_RecipientPort    = port;
        }
    }
    public class PacketSendViewModel : INotifyPropertyChanged, IDisposable
    {
        private Socket      m_Socket;
        private Boolean     m_IsConnected;
        
        private Message     m_Message;
        private Recipient   m_Recipient;

        private Thread      m_SenderThread;
        private Boolean     m_IsSuspended;

        private Boolean     IsSuspended
        {
            get             => this.m_IsSuspended;
            set 
            {
                this.m_IsSuspended = value;
                this.NotifyPropertyChanged( "IsSending" );

            }
        }

        public delegate void PartialMessageSentEventHandler ( object sender, PartialMessageSentEventArgs eArgs );
        public event PartialMessageSentEventHandler PartialMessageSent;
        public Message Message
        {
            get                     => this.m_Message;
            set
            {
                this.m_Message      = value;
                this.NotifyPropertyChanged( "Message" );
            }
        }

        public Recipient Recipient
        {
            get                     => this.m_Recipient;
            set
            {
                this.m_Recipient    = value;
                this.NotifyPropertyChanged( "Recipient" );
            }
        }

        public Boolean IsSending
        {
            get 
            {
                return !this.IsSuspended;

            }
        }

        private void InitSenderThread ( )
        {
            this.m_SenderThread     = new Thread( ( ) =>
            {
                this.PerformSendMessage( );
            } );

            this.m_SenderThread.Start( );

            AddressFamily?   addrFamily     = null;
            SocketType?      sockType       = null;
            ProtocolType?    protocolType   = null;

            switch ( this.m_Recipient.ConnectionType )
            {
                case Model.ConnectionType.UDP:
                    addrFamily      = AddressFamily.InterNetwork;
                    protocolType    = ProtocolType.Udp;
                    sockType        = SocketType.Dgram;
                    break;
                default:
                    break;
            }


            this.m_Socket           = new Socket( addrFamily.Value, sockType.Value, protocolType.Value );
        }

        private void PerformSendMessage ( )
        {

            while(true)
            {

                if ( this.IsSuspended )
                    continue;

                IPEndPoint endpoint         = this.Recipient.GetEndpoint( );

                try
                {
                    foreach ( PartialContent pc in this.Message.Content )
                    {
                        Byte [] bytes       = Encoding.ASCII.GetBytes( pc.Content );
                        Int32 numBytesSent  = this.m_Socket.SendTo( bytes, endpoint );
                        System.Diagnostics.Debug.Assert( bytes.Length == numBytesSent );

                        if ( this.Message.PartDelayInterval != 0 )
                        {
                            Thread.Sleep( ( Int32 ) this.Message.PartDelayInterval );
                            System.Diagnostics.Trace.TraceInformation( "Waiting for delay of {0} milliseconds", this.Message.PartDelayInterval );
                        }

                        if (this.PartialMessageSent != null)
                        {
                            PartialMessageSentEventArgs eArgs = new PartialMessageSentEventArgs ( pc.Content, numBytesSent, ( UInt16 ) endpoint.Port, endpoint.Address );
                            this.PartialMessageSent( this, eArgs );

                        }
                    }
                }
                catch ( Exception e )
                {
                    System.Diagnostics.Trace.TraceError( e.Message );
                }

                Thread.Sleep( ( Int32 ) this.m_Message.RepeatInterval );
                if ( !this.Message.IsRepeating )
                    this.IsSuspended      = true;
            }
        }

        public PacketSendViewModel ( Message message, Recipient recipient )
        {
            this.Message        = message;
            this.Recipient      = recipient;

            this.InitSenderThread( );

        }

        public PacketSendViewModel ( )
        {
            IPAddress ipAddr                = new IPAddress(new Byte[] {10, 29, 50, 226});
            this.Recipient                  = new Recipient(ipAddr, 12301);
            this.Recipient.ConnectionType   = Model.ConnectionType.UDP;

            this.Message                    = new Message( );
            this.AddPartialContent( "Test message" );
            this.AddPartialContent( "Second test message" );

            this.InitSenderThread( );
        }

        public Boolean AddPartialContent( String partialContent )
        {
            Boolean retVal          = false;

            System.Diagnostics.Debug.Assert( partialContent != null );
            if ( partialContent == null )
                goto end;

            Int32 nextIndex         = ( this.Message.Content.Count + 1 );
            this.Message.Content.Add( new PartialContent( partialContent,  ( UInt32 ) nextIndex) );
            retVal                  = true;
            
            end:
            return retVal;
        }

        public Boolean RemovePartialContent( PartialContent partialContent )
        {
            Boolean retVal  = false;

            System.Diagnostics.Debug.Assert( partialContent != null );
            if ( partialContent == null )
                goto end;

            Int32 idx       = this.Message.Content.IndexOf( partialContent );
            System.Diagnostics.Debug.Assert( idx != -1 );
            if ( idx == -1 )
                goto end;

            this.Message.Content.RemoveAt( idx );
            IEnumerable<PartialContent> followingItems = this.Message.Content.Where( pc => pc.Index > partialContent.Index );
            foreach ( PartialContent pc in followingItems )
                pc.Index    -= 1;
            retVal          = true;

            end:
            return retVal;
        }

        public void Send()
        {
            this.IsSuspended  = false;
            this.NotifyPropertyChanged( "IsSending" );
        }

        public void StopSending()
        {
            this.IsSuspended  = true;
            this.NotifyPropertyChanged( "IsSending" );
        }

        public void Dispose()
        {
            this.m_SenderThread.Abort( );
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged( String propertyName )
        {
            if (this.PropertyChanged != null)
            {
                PropertyChangedEventArgs eA         = new PropertyChangedEventArgs( propertyName );
                this.PropertyChanged( this, eA );
            }
        }
    }
}
