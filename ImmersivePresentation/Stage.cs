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

        public Stage(string pStageId)
        {
            stageId = pStageId;
            timeOfCreation = DateTime.Now;
            canvas = new Canvas(stageId + "-c-1");
            scene = new Scene(stageId + "-s-1");
            handout = new Handout(stageId + "h-1");
        }
    }
}