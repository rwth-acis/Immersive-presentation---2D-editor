using System;
using System.Collections.Generic;
using System.Text;

namespace ImmersivePresentation
{
    public class Text2D : Element2D
    {
        public String content { get; set; }
        public int  fontsize { get; set; }

        public Text2D()
        {
            xPosition = 20;
            yPosition = 20;
            content = "New Text";
            fontsize = 40;
        }
    }
}
