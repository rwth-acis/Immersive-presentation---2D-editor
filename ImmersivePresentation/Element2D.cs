using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;

namespace ImmersivePresentation
{
    public class Element2D : Element
    {
        //The Position describes where the left top corner of the element will be positioned. The value is in percentage (50 is in the middle).
        public double xPosition { get; set; }
        public double yPosition { get; set; }

        public Element2D() : base()
        {
            xPosition = 50;
            yPosition = 50;
        }

        public Element2D(double pXPosition, double pYPosition) : base()
        {
            xPosition = pXPosition;
            yPosition = pYPosition;
        }
    }
}
