using System;
using System.Collections.Generic;
using System.Text;

namespace ImmersivePresentation
{
    class Element3D
    {
        //The Position describes where the center of the element will be positioned. The value is in percentage (50 is in the middle)
        public double xPosition { get; set; }
        public double yPosition { get; set; }
        public double zPosition { get; set; }

        //The Scale describes how much of the Axis this element will cover in percentage (100 is the complete axis)
        public double xScale { get; set; }
        public double yScale { get; set; }
        public double zScale { get; set; }

    }
}
