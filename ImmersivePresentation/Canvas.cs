using System;
using System.Collections.Generic;

namespace ImmersivePresentation
{
    public class Canvas
    {
        public string canvasId { get; set; }
        public DateTime timeOfCreation { get; set; }
        public List<Element2D> elements { get; set; }

        public Canvas() { }
        public Canvas(string pCanvasId)
        {
            canvasId = pCanvasId;
            timeOfCreation = DateTime.Now;
            elements = new List<Element2D>();
        }
    }
}