using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace _2D_Editor.CustomControls
{
    public delegate void TimeChangedEventHandler(object sender, TimeChangedEventArgs args);
    public class AnalogClock : Control
    {
        private Line hourHand;
        private Line minuteHand;
        private Line secondHand;

        //DependencyProperty
        public static DependencyProperty ShowSecondsProperty = DependencyProperty.Register("ShowSeconds", typeof(bool), typeof(AnalogClock), new PropertyMetadata(true));
        public bool ShowSeconds
        {
            get { return (bool)GetValue(ShowSecondsProperty); }
            set { SetValue(ShowSecondsProperty, value); }
        }

        //RoutedEvents
        //when not using own Delegate use RoutedEventHandler
        public static RoutedEvent TimeChangedEvent = EventManager.RegisterRoutedEvent("TimeChanged", RoutingStrategy.Bubble, typeof(TimeChangedEventHandler), typeof(AnalogClock));
        //when not using own Delegate use RoutedEventHandler
        public event TimeChangedEventHandler TimeChanged
        {
            add
            {
                AddHandler(TimeChangedEvent, value);
            }
            remove
            {
                RemoveHandler(TimeChangedEvent, value);
            }
        }

        static AnalogClock()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AnalogClock), new FrameworkPropertyMetadata(typeof(AnalogClock)));
        }

        public override void OnApplyTemplate()
        {
            //get all the Template Objects
            hourHand = Template.FindName("PART_HourHand", this) as Line;
            minuteHand = Template.FindName("PART_MinuteHand", this) as Line;
            secondHand = Template.FindName("PART_SecondHand", this) as Line;

            //Did the binding in the template
            //Binding showSecondHandBinding = new Binding
            //{
            //    Path = new PropertyPath(nameof(ShowSeconds)),
            //    Source = this,
            //    Converter = new BooleanToVisibilityConverter()
            //};
            //secondHand.SetBinding(VisibilityProperty, showSecondHandBinding);

            OnTimeChanged(DateTime.Now);
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Tick += (s, e) => OnTimeChanged(DateTime.Now);
            timer.Start();

            base.OnApplyTemplate();
        }

        protected virtual void OnTimeChanged(DateTime time)
        {
            UpdateHandAngles(time);
            //when not using own Delegate use RoutedEventArgs here
            //also possible and more easy: RoutedPropertyChangedEventArgs<DateTime> it allows for passing a old and a new value
            RaiseEvent(new TimeChangedEventArgs(TimeChangedEvent, this) { NewTime = time });
        }

        private void UpdateHandAngles(DateTime time)
        {
            hourHand.RenderTransform = new RotateTransform((time.Hour / 12.0) * 360, 0.5, 0.5);
            minuteHand.RenderTransform = new RotateTransform((time.Minute / 60.0) * 360, 0.5, 0.5);
            secondHand.RenderTransform = new RotateTransform((time.Second / 60.0) * 360, 0.5, 0.5);
        }
    }
}
