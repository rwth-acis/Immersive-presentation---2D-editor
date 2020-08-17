using System;
using System.Collections.Generic;
using System.Text;

namespace ImmersivePresentation
{
    public class Text2D : Element2D
    {
        private string _content;
        public string content
        {
            get
            {
                return _content;
            }
            set
            {
                _content = value;
                OnProperyChanged("content");
            }
        }
        private int _fontsize;
        public int fontsize
        {
            get
            {
                return _fontsize;
            }
            set
            {
                _fontsize = value;
                OnProperyChanged("fontsize");
            }
        }

        public Text2D()
        {
            xPosition = 20;
            yPosition = 20;
            content = "New Text";
            fontsize = 40;
        }
    }
}
