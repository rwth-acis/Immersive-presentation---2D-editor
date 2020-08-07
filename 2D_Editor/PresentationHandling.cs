using ImmersivePresentation;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Compression;

namespace _2D_Editor
{
    public class PresentationHandling
    {
        public Presentation openPresentation { get; set; }
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

        public void createNewPresentation(string pJwt)
        {
            //Open a SaveFileDialog to get the path where the new presentation should be stored
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "Save New Presentation As";
            saveFileDialog.Filter = "Presentation (*.pres)|*.pres|Zip (*.zip)|*.zip";
            if (saveFileDialog.ShowDialog() == true)
            {
                //create a new presentation
                presentationSavingPath = saveFileDialog.FileName;
                presentationName = Path.GetFileNameWithoutExtension(saveFileDialog.FileName);

                openPresentation = new Presentation(pJwt, presentationName);

                //create a working directory in the users temp
                tempDirBase = Path.GetTempPath().ToString();
                Console.WriteLine(tempPresDir);
                createCleanDirectory(tempPresDir);
                createCleanDirectory(tempPresDir + tempSub2D);
                createCleanDirectory(tempPresDir + tempSub3D);

                saveOpenPresentation(); 
            }
        }

        public void saveOpenPresentation()
        {
            if(openPresentation != null)
            {
                //save the new presentation as json
                dataSerializer.SerializeAsJson(openPresentation, tempPresDir + presentationJsonFilename);

                //save the ziped new presentation where the user wanted it to be
                ZipFile.CreateFromDirectory(tempPresDir, presentationSavingPath);
            }
        }

        public void loadPresentation()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Open a Presentation";
            openFileDialog.Filter = "Presentation (*.pres)|*.pres|Zip (*.zip)|*.zip";
            if(openFileDialog.ShowDialog() == true)
            {
                //Load zip extracted in temp
                String filePath = openFileDialog.FileName.ToString();
                tempDirBase = Path.GetTempPath().ToString();
                createCleanDirectory(tempPresDir);
                ZipFile.ExtractToDirectory(filePath, tempPresDir);

                //Deserialize json
                openPresentation = dataSerializer.DeserializerJson(typeof(Presentation), tempPresDir + presentationJsonFilename) as Presentation;
            }
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
    }
}
