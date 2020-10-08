﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml.Linq;

namespace ImmersivePresentation
{
    public class Presentation
    {
        public string presentationId { get; set; }
        public string ownerId { get; set; }
        public string name { get; set; }
        //the path where the resulting presentation file will be saved
        public string filepath { get; set; }
        //in tempFilePath the presentation will create a folder that will be exported as a ziped file to the filepath when it is saved
        //ToDo: maybe not needed because stored in the editor
        private string tempFilePath { get; set; }
        public DateTime timeOfCreation { get; set; }
        public ObservableCollection<Stage> stages { get; set; }

        public Presentation()
        {

        }
        public Presentation(string pPresentationId, string pPresentationName)
        {
            presentationId = pPresentationId;
            ownerId = "";

            //Initialize all parameters
            name = pPresentationName;
            timeOfCreation = DateTime.Now;

            //Create a new Stage
            stages = new ObservableCollection<Stage>();
            stages.Add(new Stage(presentationId + "-1"));

            //Initialize the folder structure in temp
        }
    }
}
