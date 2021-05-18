using System;
using System.Collections.Generic;
using System.Text;

namespace ImmersivePresentation
{
    public class Image2D : Element2D
    {
        private string _relativeImageSource;
        public string relativeImageSource
        {
            get
            {
                return _relativeImageSource;
            }
            set
            {
                _relativeImageSource = value;
                OnProperyChanged("relativeImageSource");
            }
        }
        private double _xScale;
        public double xScale
        {
            get
            {
                return _xScale;
            }
            set
            {
                _xScale = value;
                OnProperyChanged("xScale");
            }
        }
        private double _yScale;
        public double yScale
        {
            get
            {
                return _yScale;
            }
            set
            {
                _yScale = value;
                OnProperyChanged("yScale");
            }
        }

        public Image2D() : base()
        {
            //Default Scale
            xPosition = 50;
            yPosition = 50;
            xScale = 20;
            yScale = 20;
        }

        public Image2D(string pRelativeImageSource) : base()
        {
            //Default Scale
            xPosition = 50;
            yPosition = 50;
            xScale = 20;
            yScale = 20;
            relativeImageSource = pRelativeImageSource;
        }

        public Image2D(double pXPosition, double pYPosition, double pXScale, double pYScale) : base(pXPosition, pYPosition)
        {
            xScale = pXScale;
            yScale = pYScale;
        }

        public Image2D(string pRelativeImageSource, double pXPosition, double pYPosition, double pXScale, double pYScale) : base(pXPosition, pYPosition)
        {
            xScale = pXScale;
            yScale = pYScale;
            relativeImageSource = pRelativeImageSource;
        }

    }
}
