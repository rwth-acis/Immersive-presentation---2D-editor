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

        //The Scale describes the scale of the object in a room
        public double xScale { get; set; }
        public double yScale { get; set; }
        public double zScale { get; set; }

        //The Scale describes the scale of the object in a room
        public double xRotation { get; set; }
        public double yRotation { get; set; }
        public double zRotation { get; set; }

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
            //Default Rotation
            xRotation = 0;
            yRotation = 0;
            zRotation = 0;
        }

        public Element3D(string pRelativePath) : base()
        {
            relativePath = pRelativePath;
            //Default Position
            xPosition = 0;
            yPosition = 0;
            zPosition = 0;
            //Default Scale
            xScale = 1;
            yScale = 1;
            zScale = 1;
            //Default Rotation
            xRotation = 0;
            yRotation = 0;
            zRotation = 0;
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
            //Default Rotation
            xRotation = 0;
            yRotation = 0;
            zRotation = 0;
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

            //Default Rotation
            xRotation = 0;
            yRotation = 0;
            zRotation = 0;
        }

    }
}
