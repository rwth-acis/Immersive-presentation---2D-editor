using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ImmersivePresentation
{
    public class Element3D
    {
        //The Position describes where the center of the element will be positioned. The value is in percentage (0 is in the middle)
        public double xPosition { get; set; }
        public double yPosition { get; set; }
        public double zPosition { get; set; }

        //The Scale describes how much of the Axis this element will cover in percentage (100 is the complete axis)
        public double xScale { get; set; }
        public double yScale { get; set; }
        public double zScale { get; set; }

        public string relativePath { get; set; }
        public string filename { 
            get
            {
                try
                {
                    if(relativePath != "")
                    {
                        return Path.GetFileNameWithoutExtension(relativePath);
                    }
                    else
                    {
                        return "3D Element";
                    }
                }
                catch
                {
                    return "3D Element";
                }
            }
            set
            {

            }
        }
        public string relativMaterialPath { get; set; }

        public Element3D() : base()
        {
            //Default Position
            xPosition = 0;
            yPosition = 0;
            zPosition = 20;
            //Default Scale
            xScale = 20;
            yScale = 20;
            zScale = 20;
        }

        public Element3D(string pRelativePath) : base()
        {
            relativePath = pRelativePath;
            //Default Position
            xPosition = 0;
            yPosition = 0;
            zPosition = 20;
            //Default Scale
            xScale = 20;
            yScale = 20;
            zScale = 20;
        }

        public Element3D(string pRelativePath, double pXPosition, double pYPosition, double pZPosition) : base()
        {
            relativePath = pRelativePath;

            xPosition = pXPosition;
            yPosition = pYPosition;
            zPosition = pZPosition;
            //Default Scale
            xScale = 20;
            yScale = 20;
            zScale = 20;
        }

        public Element3D(string pRelativePath, double pXPosition, double pYPosition, double pZPosition, double pXScale, double pYScale, double pZScale) : base()
        {
            relativePath = pRelativePath;

            xPosition = pXPosition;
            yPosition = pYPosition;
            zPosition = pZPosition;
            
            xScale = pXScale;
            yScale = pYScale;
            zScale = pZScale;
        }

    }
}
