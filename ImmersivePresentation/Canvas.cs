using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ImmersivePresentation
{
    public class Canvas
    {
        public string canvasId { get; set; }
        public DateTime timeOfCreation { get; set; }
        public ObservableCollection<Element2D> elements { get; set; }

        public Canvas() { }
        public Canvas(string pCanvasId)
        {
            canvasId = pCanvasId;
            timeOfCreation = DateTime.Now;
            elements = new ObservableCollection<Element2D>();
        }
    }
}