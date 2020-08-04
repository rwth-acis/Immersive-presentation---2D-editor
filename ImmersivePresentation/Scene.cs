using System;
using System.Collections.Generic;

namespace ImmersivePresentation
{
    public class Scene
    {
        public string sceneId { get; set; }
        public DateTime timeOfCreation { get; set; }
        public List<Element3D> elements { get; set; }

        public Scene()
        {

        }
        public Scene(string pSceneId)
        {
            sceneId = pSceneId;
            timeOfCreation = DateTime.Now;
            elements = new List<Element3D>();
        }
    }
}