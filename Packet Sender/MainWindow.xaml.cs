using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Model;
using ViewModels;

using ViewModel = ViewModels.PacketSendViewModel;

namespace Packet_Sender
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ViewModel ViewModel
        {
            get
            {
                ViewModel vm        = null;
                
                vm                  = this.DataContext as PacketSendViewModel;
                System.Diagnostics.Debug.Assert( vm != null );
                
                return vm;
            }
        }

        public MainWindow()
        {
            this.InitializeComponent();

            ViewModel vm            = this.ViewModel;
            vm.PartialMessageSent   += this.Vm_PartialMessageSent;
        }

        private void Vm_PartialMessageSent ( Object sender, PartialMessageSentEventArgs eArgs )
        {
            String message          = String.Format( "[{4}]Sent {0} bytes of message {1} at {2}:{3}", eArgs.NumBytesSent, eArgs.PartialMessage, eArgs.IPAddress, eArgs.Port, DateTime.Now );
            this.txtLog.Dispatcher.Invoke( ( ) => 
            {
                this.txtLog.Text += message += "\r\n";
            } );

            Console.WriteLine( message );
        }

        private void BtnAddMessageContent_MouseUp ( Object sender, MouseButtonEventArgs e )
        {
            Image senderControl         = sender as Image;
            System.Diagnostics.Debug.Assert( senderControl != null );

            if ( senderControl == null )
                goto end;

            ViewModel vm      = senderControl.DataContext as ViewModel;
            System.Diagnostics.Debug.Assert( vm != null );
            if ( vm == null )
                goto end;

            vm.AddPartialContent( "" );

            end:
            return;
        }

        private void BtnRemovePartialContent_Click ( Object sender, RoutedEventArgs e )
        {
            TextBlock btn               = sender as TextBlock;
            System.Diagnostics.Debug.Assert( btn != null );
            if ( btn == null )
                goto end;

            PartialContent content      = btn.DataContext as PartialContent;
            System.Diagnostics.Debug.Assert( content != null );
            if ( content == null )
                goto end;

            ViewModel vm                = this.ViewModel;
            if ( vm == null )
                goto end;
            
            Boolean didRemoveContent    = vm.RemovePartialContent( content );
            if ( !didRemoveContent )
                MessageBox.Show( "Failed to delete message" );

            end:
            return;
        }

        private void BtnSend_Click ( Object sender, RoutedEventArgs e )
        {
            Button btn      = sender as Button;
            System.Diagnostics.Debug.Assert( btn != null );
            if ( btn == null )
                goto end;

            ViewModel vm    = this.ViewModel;
            if ( vm == null )
                goto end;

            vm.Send( );

            end:
            return;
        }

        private void BtnStopSend_Click ( Object sender, RoutedEventArgs e )
        {
            this.ViewModel.StopSending( );
        }

        private void Window_Closing ( Object sender, System.ComponentModel.CancelEventArgs e )
        {
            this.ViewModel.Dispose( );
        }

        private void RadioProtocolChecked ( Object sender, RoutedEventArgs e )
        {
            ConnectionType selectedType = ConnectionType.Unused;

            if ( sender == this.rbUdp )
                selectedType            = ConnectionType.UDP;
            else if ( sender == this.rbTcp )
                selectedType            = ConnectionType.TCP;

            System.Diagnostics.Debug.Assert( selectedType != ConnectionType.Unused );
            if ( selectedType == ConnectionType.Unused )
                goto end;

            ViewModel vm                = this.ViewModel;
            System.Diagnostics.Debug.Assert( vm != null );

            vm.SwitchConnectionType( selectedType );

            end:
            return;
        }
    }
}
