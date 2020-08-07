using ImmersivePresentation;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Compression;
using System.Windows;

namespace _2D_Editor
{
    public class PresentationHandling
    {
        public Presentation openPresentation { get; set; }
        public Stage selectedStage { get; set; }
        public int indexOfSelectedStage { 
            get
            {
                return openPresentation.stages.IndexOf(selectedStage);
            } 
            }
        public string presentationSavingPath { get; set; }
        public string presentationName { get; set; }
        public string tempDirBase { get; set; } //Path to the start of the temporary folder of the actual windows user 
        public const string tempSuffix = "ImPres\\presentation\\";
        public string tempPresDir { get {
                return tempDirBase + tempSuffix;
            } } //Path where the presentation content is stored and the json of the presentation.
        public const string presentationJsonFilename = "presentation.json";
        public const string tempSub2D = "2DMedia\\";
        public const string tempSub3D = "3DMedia\\";

        private DataSerializer dataSerializer = new DataSerializer();

        public PresentationHandling()
        {
            openPresentation = null;
            openPresentation = new Presentation("fakeJWT", "DemoPresentation");
        }

        public PresentationHandling(StartMode pMode, string pPath)
        {
            switch (pMode)
            {
                case StartMode.New:
                    {
                        createNewPresentation("fakeJWT", pPath);
                        break;
                    }
                case StartMode.Open:
                    {
                        loadPresentation(pPath);
                        break;
                    }
                default:
                    {
                        openPresentation = new Presentation("fakeJWT", "DemoPresentation");
                        break;
                    }
            }
        }

        public void createAndGetLocationOfNewPresentation(string pJwt)
        {
            //Open a SaveFileDialog to get the path where the new presentation should be stored
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "Save New Presentation As";
            saveFileDialog.Filter = "Presentation (*.pres)|*.pres|Zip (*.zip)|*.zip";
            if (saveFileDialog.ShowDialog() == true)
            {
                createNewPresentation(pJwt, saveFileDialog.FileName);
            }
        }

        private void createNewPresentation(string pJwt, string pPath)
        {
            //create a new presentation
            presentationSavingPath = pPath;
            presentationName = Path.GetFileNameWithoutExtension(pPath);

            openPresentation = new Presentation(pJwt, presentationName);
            //ToDo: check more robust whether there exist a stage[0]
            selectedStage = openPresentation.stages[0];

            //create a working directory in the users temp
            createWorkingDir();

            saveOpenPresentation();
        }

        private void createWorkingDir()
        {
            tempDirBase = Path.GetTempPath().ToString();
            Console.WriteLine(tempPresDir);
            createCleanDirectory(tempPresDir);
            createCleanDirectory(tempPresDir + tempSub2D);
            createCleanDirectory(tempPresDir + tempSub3D);
        }

        public void saveOpenPresentation()
        {
            if(openPresentation != null)
            {
                //save the new presentation as json
                dataSerializer.SerializeAsJson(openPresentation, tempPresDir + presentationJsonFilename);

                //save the ziped new presentation where the user wanted it to be
                if (File.Exists(presentationSavingPath))
                {
                    //ToDo: make a backup
                    File.Delete(presentationSavingPath);
                }
                ZipFile.CreateFromDirectory(tempPresDir, presentationSavingPath);
            }
        }

        public void loadPresentationAndGetLocation()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Open a Presentation";
            openFileDialog.Filter = "Presentation (*.pres)|*.pres|Zip (*.zip)|*.zip";
            if(openFileDialog.ShowDialog() == true)
            {
                loadPresentation(openFileDialog.FileName.ToString());
            }
        }

        private void loadPresentation(string pPath)
        {
            //Load zip extracted in temp
            String filePath = pPath;
            tempDirBase = Path.GetTempPath().ToString();
            createCleanDirectory(tempPresDir);
            ZipFile.ExtractToDirectory(filePath, tempPresDir);

            //Deserialize json
            openPresentation = dataSerializer.DeserializerJson(typeof(Presentation), tempPresDir + presentationJsonFilename) as Presentation;
        }

        public void createCleanDirectory(string pDirectoryPath)
        {
            try
            {
                // Determine whether the directory exists.
                if (Directory.Exists(pDirectoryPath))
                {
                    //delete all the content of the directory
                    DirectoryInfo dirInf = new DirectoryInfo(pDirectoryPath);

                    foreach (FileInfo file in dirInf.GetFiles())
                    {
                        file.Delete();
                    }
                    foreach (DirectoryInfo dir in dirInf.GetDirectories())
                    {
                        dir.Delete(true);
                    }
                }
                else
                {
                    // Try to create the directory.
                    DirectoryInfo di = Directory.CreateDirectory(pDirectoryPath);
                    Console.WriteLine("The directory was created successfully at {0}.", Directory.GetCreationTime(pDirectoryPath));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The process failed: {0}", e.ToString());
            }
            finally { }
        }

        //Stages
        public void addNewStage()
        {
            Stage newStage = new Stage("DemoName");
            openPresentation.stages.Insert(indexOfSelectedStage + 1, newStage);
        }
        public void deleteSelectedStage()
        {
            //check if there are stages remaining
            if(openPresentation.stages.Count > 1)
            {
                int saveIndexOfSelection = indexOfSelectedStage;
                openPresentation.stages.Remove(selectedStage);
                if(saveIndexOfSelection <= openPresentation.stages.Count - 1)
                {
                    selectedStage = openPresentation.stages[saveIndexOfSelection];
                }
                else
                {
                    selectedStage = openPresentation.stages[openPresentation.stages.Count - 1];
                }
            }
            else
            {
                MessageBox.Show("Sorry - You are not allowed to delete the last Stage.");
            }
            

        }
        public void moveSelectedStageUp()
        {
            if(indexOfSelectedStage > 0)
            {
                int saveIndex = indexOfSelectedStage;
                Stage saveStage = selectedStage;
                openPresentation.stages.Remove(selectedStage);
                openPresentation.stages.Insert(saveIndex - 1, saveStage);
                selectedStage = saveStage;
                //ToDo: Maybe the selected Index in the List View must change as well?
            }
        }
        public void moveSelectedStageDown()
        {
            if (indexOfSelectedStage < openPresentation.stages.Count -1 && indexOfSelectedStage >= 0)
            {
                int saveIndex = indexOfSelectedStage;
                Stage saveStage = selectedStage;
                openPresentation.stages.Remove(selectedStage);
                openPresentation.stages.Insert(saveIndex + 1, saveStage);
                selectedStage = saveStage;
                //ToDo: Maybe the selected Index in the List View must change as well?
            }
        }
    }
}
