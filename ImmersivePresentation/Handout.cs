using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ImmersivePresentation
{
    public class Handout
    {
        public string handoutId { get; set; }
        public DateTime timeOfCreation { get; set; }
        public ObservableCollection<Element3D> elements { get; set; }

        public Handout() { }
        public Handout(string pHandoutId)
        {
            handoutId = pHandoutId;
            timeOfCreation = DateTime.Now;
            elements = new ObservableCollection<Element3D>();
        }
    }
}