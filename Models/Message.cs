using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class PartialContent : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private String m_PartialContent;
        private UInt32 m_Index;

        public String Content
        {
            get                         => this.m_PartialContent;
            set
            {
                this.m_PartialContent   = value;
                this.NotifyPropertyChanged( "Content" );
            }
        }

        public UInt32 Index
        {
            get                         => this.m_Index;
            set
            {
                this.m_Index            = value;
                this.NotifyPropertyChanged( "Index" );
            }
        }

        private void NotifyPropertyChanged( String propertyName )
        {
            if ( this.PropertyChanged != null )
            {
                PropertyChangedEventArgs eA = new PropertyChangedEventArgs( propertyName );
                this.PropertyChanged( this, eA );
            }
        }

        public PartialContent ( String content, UInt32 index )
        {
            this.Content                    = content;
            this.Index                      = index;
        }

    }

    public class Message : INotifyPropertyChanged
    {
        private ObservableCollection<PartialContent>m_Content;
        private UInt64      m_RepeatInterval;
        private Boolean     m_IsRepeating;
        private UInt64      m_PartDelayTime;
        private Boolean     m_AppendCrLf;

        public ObservableCollection<PartialContent> Content
        {
            get                         => this.m_Content;
            set
            {
                this.m_Content          = value;
                this.NotifyPropertyChanged( "Content" );
            }
        }

        public UInt64 RepeatInterval
        {
            get                         => this.m_RepeatInterval;
            set
            {
                this.m_RepeatInterval   = value;
                this.NotifyPropertyChanged( "RepeatInterval" );
            }
        }

        public Boolean IsRepeating
        {
            get                         => this.m_IsRepeating;
            set
            {
                this.m_IsRepeating      = value;
                this.NotifyPropertyChanged( "IsRepeating" );
            }
        }

        public UInt64 PartDelayInterval
        {
            get                         => this.m_PartDelayTime;
            set 
            {
                this.m_PartDelayTime    = value;
                this.NotifyPropertyChanged( "PartDelayInterval" );
            }
        }

        public Boolean HasContent
        {
            get                         => ( this.Content.Count != 0 );
        }

        public Boolean AppendNewLine
        {
            get                         => this.m_AppendCrLf;
            set 
            {
                this.m_AppendCrLf       = value;
                this.NotifyPropertyChanged( "AppendNewLine" );
            }
        }

        public Message(UInt64 repeatInterval, Boolean isRepeating)
        {
            this.Content                    = new ObservableCollection<PartialContent> ( );
            this.Content.CollectionChanged  += this.UpdateContentProperties;
            
            this.IsRepeating                = isRepeating;
            this.RepeatInterval             = repeatInterval;
        }

        public Message () : this (0, false) { }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged( String propertyName )
        {
            if ( this.PropertyChanged != null )
            {
                PropertyChangedEventArgs eA         = new PropertyChangedEventArgs( propertyName );
                this.PropertyChanged( this, eA );
            }
        }

        private void UpdateContentProperties ( object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs args )
        {
            this.NotifyPropertyChanged( "Content" );
            this.NotifyPropertyChanged( "HasContent" );
        }
    }
}
