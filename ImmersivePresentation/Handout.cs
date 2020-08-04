using System;
using System.Collections.Generic;

namespace ImmersivePresentation
{
    public class Handout
    {
        public string handoutId { get; set; }
        public DateTime timeOfCreation { get; set; }
        public List<Element3D> elements { get; set; }

        public Handout(string pHandoutId)
        {
            handoutId = pHandoutId;
            timeOfCreation = DateTime.Now;
            elements = new List<Element3D>();
        }
    }
}