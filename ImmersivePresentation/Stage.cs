using System;

namespace ImmersivePresentation
{
    public class Stage
    {
        public string stageId { get; set; }
        public DateTime timeOfCreation { get; set; }
        public Canvas canvas { get; set; }
        public Scene scene { get; set; }
        public Handout handout { get; set; }
    }
}