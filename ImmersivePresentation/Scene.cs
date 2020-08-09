using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ImmersivePresentation
{
    public class Scene
    {
        public string sceneId { get; set; }
        public DateTime timeOfCreation { get; set; }
        public ObservableCollection<Element3D> elements { get; set; }

        public Scene()
        {

        }
        public Scene(string pSceneId)
        {
            sceneId = pSceneId;
            timeOfCreation = DateTime.Now;
            elements = new ObservableCollection<Element3D>();
        }
    }
}