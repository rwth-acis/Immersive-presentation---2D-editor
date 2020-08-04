using System;
using System.Collections.Generic;
using System.Text;

namespace ImmersivePresentation
{
    class Element2D : Element
    {
        //The Position describes where the left top corner of the element will be positioned. The value is in percentage (50 is in the middle).
        public double xPosition { get; set; }
        public double yPosition { get; set; }
    }
}
