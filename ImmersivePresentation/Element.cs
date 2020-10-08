using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ImmersivePresentation
{
    public class Element : INotifyPropertyChanged
    {
        public string elementId { get; set; }

        public Element()
        {
            elementId = "";
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnProperyChanged(string name)
        {
            if(PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        private bool _highlighted;
        public bool highlighted
        {
            get
            {
                return _highlighted;
            }
            set
            {
                _highlighted = value;
                OnProperyChanged("highlighted");
            }
        }
    }
}
