using ImmersivePresentation;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2D_Editor
{
    public class PresentationHandling
    {
        public Presentation openPresentation { get; set; }
        public string presentationSavingPath { get; set; }
        public string presentationName { get; set; }
        public string tempfolder { get; set; }
        public const string presentationJsonFilename = "presentation.json";
        public const string tempSub2D = "2DMedia\\";
        public const string tempSub3D = "3DMedia\\";

        public PresentationHandling()
        {
            openPresentation = null;
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
                tempfolder = Path.GetTempPath().ToString() + "ImPres\\";
                Console.WriteLine(tempfolder);
                createCleanDirectory(tempfolder);
                createCleanDirectory(tempfolder + tempSub2D);
                createCleanDirectory(tempfolder + tempSub3D);

                openPresentation.StoreAsJSON(tempfolder + presentationJsonFilename);
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
