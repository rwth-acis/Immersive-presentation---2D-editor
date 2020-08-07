using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace _2D_Editor.CustomControls
{
    //We use this specialised EventArgs Class because we want to recieve Data from within the object that fires the Event
    public class TimeChangedEventArgs : RoutedEventArgs
    {
        public DateTime NewTime { get; set; }
        public TimeChangedEventArgs()
        {
        }

        public TimeChangedEventArgs(RoutedEvent routedEvent) : base(routedEvent)
        {
        }

        public TimeChangedEventArgs(RoutedEvent routedEvent, object source) : base(routedEvent, source)
        {
        }
    }
}
