using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;

namespace ImmersivePresentation
{
    public class Element2D : Element
    {
        //The Position describes where the left top corner of the element will be positioned. The value is in percentage (50 is in the middle).
        private double _xPosition;
        public double xPosition
        {
            get
            {
                return _xPosition;
            }
            set
            {
                _xPosition = value;
                OnProperyChanged("xPosition");
            }
        }
        public double _yPosition;
        public double yPosition
        {
            get
            {
                return _yPosition;
            }
            set
            {
                _yPosition = value;
                OnProperyChanged("yPosition");
            }
        }
        

        public Element2D() : base()
        {
            highlighted = false;
            xPosition = 50;
            yPosition = 50;
        }

        public Element2D(double pXPosition, double pYPosition) : base()
        {
            highlighted = false;
            xPosition = pXPosition;
            yPosition = pYPosition;
        }
    }
}
