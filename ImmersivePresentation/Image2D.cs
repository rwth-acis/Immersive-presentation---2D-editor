using System;
using System.Collections.Generic;
using System.Text;

namespace ImmersivePresentation
{
    public class Image2D : Element2D
    {
        public string relativeImageSource { get; set; }
        public double xScale { get; set; }
        public double yScale { get; set; }

        public Image2D() : base()
        {
            //Default Scale
            xScale = 20;
            yScale = 20;
        }

        public Image2D(double pXPosition, double pYPosition, double pXScale, double pYScale) : base(pXPosition, pYPosition)
        {
            xScale = pXScale;
            yScale = pYScale;
        }

    }
}
