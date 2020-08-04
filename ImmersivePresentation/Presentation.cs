using System;
using System.Collections.Generic;

namespace ImmersivePresentation
{
    public class Presentation
    {
        public string presentationId { get; set; }
        public string ownerId { get; set; }
        public string name { get; set; }
        public string filepath { get; set; }
        public DateTime timeOfCreation { get; set; }

        public List<Stage> stages;
    }
}
