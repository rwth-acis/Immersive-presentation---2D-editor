﻿using CoordinatorConnectorLibrary;
using ImmersivePresentation;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.Compression;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace _2D_Editor
{
    public class PresentationHandling
    {
        public MainWindow mainWindow;
        public PresentationWindow presentationWindow;
        public Presentation openPresentation { get; set; }
        private Stage _selectedStage;
        public Stage SelectedStage {
            get { return _selectedStage; } 
            set 
            {
                if(value == null)
                {
                    return;
                }
                _selectedStage = value;
                //upate the stage listbox
                if (WindowsStageListBox != null)
                {
                    int listboxIndex = WindowsStageListBox.Items.IndexOf(value);
                    if(listboxIndex >= 0 && listboxIndex < WindowsStageListBox.Items.Count)
                    {
                        WindowsStageListBox.SelectedIndex = listboxIndex;
                        WindowsStageListBox.ScrollIntoView(value);
                    }
                }
                //update the scene listbox
                if(WindowsSceneListBox != null)
                {
                    WindowsSceneListBox.ItemsSource = value.scene.elements;
                }
                //update the handout listbox
                if (WindowsHandoutListBox != null)
                {
                    WindowsHandoutListBox.ItemsSource = value.handout.elements;
                }
                //update the canvas ItemsControl
                if(WindowsCanvasPreview != null)
                {
                    WindowsCanvasPreview.ItemsSource = value.canvas.elements;
                    Console.WriteLine("Canvas Preview Itemsource updated.");
                }
            } 
        }
        public ObservableCollection<Element> selectedElements;
        public int indexOfSelectedStage { 
            get
            {
                return openPresentation.stages.IndexOf(SelectedStage);
            } 
            }
        public ListBox WindowsStageListBox { get; set; }
        public ListBox WindowsSceneListBox { get; set; }
        public ListBox WindowsHandoutListBox { get; set; }
        public ItemsControl WindowsCanvasPreview { get; set; }
        private ItemsControl _WindowsPropertyList;
        public ItemsControl WindowsPropertyList
        {
            get
            {
                return _WindowsPropertyList;
            }
            set
            {
                _WindowsPropertyList = value;
                _WindowsPropertyList.ItemsSource = selectedElements;
            }
        }
        public ItemsControl PresentationWindowCanvasPreview { get; set; }
        private int _indexPresentationStage;
        public int indexPresentationStage { 
            get
            {
                return _indexPresentationStage;
            }
            set
            {
                if (value < 0 || value >= openPresentation.stages.Count)
                {
                    
                }
                else
                {
                    _indexPresentationStage = value;
                    PresentationWindowCanvasPreview.ItemsSource = openPresentation.stages[value].canvas.elements;
                }
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
        public const string tempSubSubScene = "Scene\\";
        public const string tempSubSubHandout = "Handout\\";

        //private DataSerializer dataSerializer = new DataSerializer();
        private JsonSerializerSettings jsonSettings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto };

        private CoordinatorConnection connection;

        private PhotonClient photonClient;
        public PresentationHandling(CoordinatorConnection pConnection, MainWindow pMainWindow)
        {
            mainWindow = pMainWindow;
            connection = pConnection;
            openPresentation = null;
            openPresentation = new Presentation("fakeJWT", "DemoPresentation");
            selectedElements = new ObservableCollection<Element>();
        }

        public PresentationHandling(CoordinatorConnection pConnection, StartMode pMode, string pPath, MainWindow pMainWindow)
        {
            mainWindow = pMainWindow;
            connection = pConnection;
            switch (pMode)
            {
                case StartMode.New:
                    {
                        createNewPresentation(pPath);
                        break;
                    }
                case StartMode.Open:
                    {
                        loadPresentation(pPath);
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
            selectedElements = new ObservableCollection<Element>();
        }

        public void createAndGetLocationOfNewPresentation()
        {
            //Open a SaveFileDialog to get the path where the new presentation should be stored
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "Save New Presentation As";
            saveFileDialog.Filter = "Presentation (*.pres)|*.pres|Zip (*.zip)|*.zip";
            if (saveFileDialog.ShowDialog() == true)
            {
                createNewPresentation(saveFileDialog.FileName);
            }
        }

        private void createNewPresentation(string pPath)
        {
            //create a new presentation
            presentationSavingPath = pPath;
            presentationName = Path.GetFileNameWithoutExtension(pPath);

            //Get presentationId from Coordinator
            string presentationId = connection.newPresentation(presentationName);
            if(presentationId == "")
            {
                //Error
                mainWindow.StatsbarInfo.Text = "Unable to create a new file online.";
                MessageBox.Show("We were unable to upload the presentation to the cloud. Next time you load the presentation, we will try again.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            openPresentation = new Presentation(presentationId, presentationName);
            openPresentation.ownerId = connection.getIdUser().ToString();
            SelectedStage = openPresentation.stages[0];

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
            createCleanDirectory(tempPresDir + tempSub3D + tempSubSubScene);
            createCleanDirectory(tempPresDir + tempSub3D + tempSubSubHandout);
        }

        public  async void saveOpenPresentation()
        {
            if(openPresentation != null)
            {
                mainWindow.StatsbarInfo.Text = "Saving ...";
                mainWindow.Cursor = Cursors.Wait;
                //save the new presentation as json
                //*dataSerializer.SerializeAsJson(openPresentation, tempPresDir + presentationJsonFilename);
                File.WriteAllText(tempPresDir + presentationJsonFilename, JsonConvert.SerializeObject(openPresentation, jsonSettings));

                //save the ziped new presentation where the user wanted it to be
                if (File.Exists(presentationSavingPath))
                {
                    File.Delete(presentationSavingPath);
                }
                ZipFile.CreateFromDirectory(tempPresDir, presentationSavingPath);
                string msg = await connection.uploadPresentation(presentationSavingPath, openPresentation.presentationId);
                if (msg == "")
                {
                    //Successfully uploaded
                    mainWindow.StatsbarInfo.Text = "Successfully saved.";
                    mainWindow.Cursor = Cursors.Arrow;
                    await Task.Delay(2000);
                    mainWindow.StatsbarInfo.Text = "Ready";
                }
                else
                {
                    //Error by uploading
                    mainWindow.StatsbarInfo.Text = "Error by saving: " + msg;
                    mainWindow.Cursor = Cursors.Arrow;
                    await Task.Delay(2000);
                    mainWindow.StatsbarInfo.Text = "Ready";
                }
                mainWindow.Cursor = Cursors.Arrow;
            }
            else
            {
                //No Persentation open
                mainWindow.StatsbarInfo.Text = "Can not save - No presentation is open.";
                await Task.Delay(2000);
                mainWindow.StatsbarInfo.Text = "Ready";
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
            //*openPresentation = dataSerializer.DeserializerJson(typeof(Presentation), tempPresDir + presentationJsonFilename) as Presentation;
            openPresentation = JsonConvert.DeserializeObject<Presentation>(File.ReadAllText(tempPresDir + presentationJsonFilename), jsonSettings);
            presentationSavingPath = pPath;

            //check whether the presentation has been uploaded before
            if(openPresentation.presentationId == "")
            {
                string presentationId = connection.newPresentation(Path.GetFileNameWithoutExtension(pPath));
                if (presentationId != "") openPresentation.presentationId = presentationId;
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

        //Stages
        public void addNewStage()
        {
            Stage newStage = new Stage("DemoName");
            openPresentation.stages.Insert(indexOfSelectedStage + 1, newStage);
            SelectedStage = newStage;
        }
        public void deleteSelectedStage()
        {
            //check if there are stages remaining
            if(openPresentation.stages.Count > 1)
            {
                int saveIndexOfSelection = indexOfSelectedStage;
                openPresentation.stages.Remove(SelectedStage);
                if(saveIndexOfSelection <= openPresentation.stages.Count - 1)
                {
                    SelectedStage = openPresentation.stages[saveIndexOfSelection];
                }
                else
                {
                    SelectedStage = openPresentation.stages[openPresentation.stages.Count - 1];
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
                Stage saveStage = SelectedStage;
                openPresentation.stages.Remove(SelectedStage);
                openPresentation.stages.Insert(saveIndex - 1, saveStage);
                SelectedStage = saveStage;
                //ToDo: Maybe the selected Index in the List View must change as well?
            }
        }
        public void moveSelectedStageDown()
        {
            if (indexOfSelectedStage < openPresentation.stages.Count -1 && indexOfSelectedStage >= 0)
            {
                int saveIndex = indexOfSelectedStage;
                Stage saveStage = SelectedStage;
                openPresentation.stages.Remove(SelectedStage);
                openPresentation.stages.Insert(saveIndex + 1, saveStage);
                SelectedStage = saveStage;
                //ToDo: Maybe the selected Index in the List View must change as well?
            }
        }

        //Scene 3D Elements
        public void add3DElementToScene()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Select a 3D Model";
            openFileDialog.Filter = "Objects (*.obj)|*.obj|Others (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                string sourcePath = openFileDialog.FileName.ToString();
                string pathWithoutFilename = Path.GetDirectoryName(sourcePath);
                string nameOfFile = Path.GetFileNameWithoutExtension(sourcePath);
                string extension = Path.GetExtension(sourcePath);
                string targetTepFolder = tempPresDir + tempSub3D + tempSubSubScene;
                string relativeFolderPath = tempSub3D + tempSubSubScene;

                string appendix = "";
                int appendixCount = 0;
                while (File.Exists(targetTepFolder + nameOfFile + appendix + extension))
                {
                    appendixCount = appendixCount + 1;
                    appendix = "_" + appendixCount;
                }
                string availableNameOfFile = nameOfFile + appendix;


                try
                {
                    File.Copy(sourcePath, targetTepFolder + availableNameOfFile + extension);
                    Element3D newElement = new Element3D(relativeFolderPath + availableNameOfFile + extension);

                    //Check for a materialfile (.mtl)
                    string materialSourcePath = pathWithoutFilename + "\\" + nameOfFile + ".mtl";
                    if (File.Exists(materialSourcePath))
                    {
                        File.Copy(materialSourcePath, targetTepFolder + nameOfFile + appendix + ".mtl");
                        newElement.relativMaterialPath = relativeFolderPath + nameOfFile + appendix + ".mtl";
                    }
                    else
                    {
                        MessageBox.Show("We were unable to locate a material file (.mtl) automatically. Please add the material file by hand in the property section.");
                    }

                    SelectedStage.scene.elements.Add(newElement);
                }
                catch(Exception e)
                {
                    MessageBox.Show("Sorry - We were not able to add this model.");
                    Console.WriteLine(e);
                    //ToDo show error in status bar
                }
            }
        }
        public void delete3DElementFromScene()
        {
            if(WindowsSceneListBox.SelectedIndex >= 0)
            {
                //ToDo better error handling when it is not a Element3D
                Element3D elementToDelete = WindowsSceneListBox.Items[WindowsSceneListBox.SelectedIndex] as Element3D;

               
                //clean temp
                if(elementToDelete.relativePath != "" && File.Exists(tempPresDir + elementToDelete.relativePath))
                {
                    File.Delete(tempPresDir + elementToDelete.relativePath);
                }
                if (elementToDelete.relativMaterialPath != "" && File.Exists(tempPresDir + elementToDelete.relativMaterialPath))
                {
                    File.Delete(tempPresDir + elementToDelete.relativMaterialPath);
                }
                //remove element
                SelectedStage.scene.elements.Remove(elementToDelete);
            }
        }

        //Handout 3D Elements
        public void add3DElementToHandout()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Select a 3D Model";
            openFileDialog.Filter = "Objects (*.obj)|*.obj|Others (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                string sourcePath = openFileDialog.FileName.ToString();
                string pathWithoutFilename = Path.GetDirectoryName(sourcePath);
                string nameOfFile = Path.GetFileNameWithoutExtension(sourcePath);
                string extension = Path.GetExtension(sourcePath);
                string targetTepFolder = tempPresDir + tempSub3D + tempSubSubHandout;
                string relativeFolderPath = tempSub3D + tempSubSubHandout;

                string appendix = "";
                int appendixCount = 0;
                while (File.Exists(targetTepFolder + nameOfFile + appendix + extension))
                {
                    appendixCount = appendixCount + 1;
                    appendix = "_" + appendixCount;
                }
                string availableNameOfFile = nameOfFile + appendix;

                try
                {
                    File.Copy(sourcePath, targetTepFolder + availableNameOfFile + extension);
                    Element3D newElement = new Element3D(relativeFolderPath + availableNameOfFile + extension);

                    //Check for a materialfile (.mtl)
                    string materialSourcePath = pathWithoutFilename + "\\" + nameOfFile + ".mtl";
                    if (File.Exists(materialSourcePath))
                    {
                        File.Copy(materialSourcePath, targetTepFolder + nameOfFile + appendix + ".mtl");
                        newElement.relativMaterialPath = relativeFolderPath + nameOfFile + appendix + ".mtl";
                    }
                    else
                    {
                        MessageBox.Show("We were unable to locate a material file (.mtl) automatically. Please add the material file by hand in the property section.");
                    }

                    SelectedStage.handout.elements.Add(newElement);
                }
                catch
                {
                    MessageBox.Show("Sorry - We were not able to add this model.");
                    //ToDo show error in status bar
                }
            }
        }
        public void delete3DElementFromHandout()
        {
            if (WindowsHandoutListBox.SelectedIndex >= 0)
            {
                //ToDo better error handling when it is not a Element3D
                Element3D elementToDelete = WindowsHandoutListBox.Items[WindowsHandoutListBox.SelectedIndex] as Element3D;

                //clean temp
                if (elementToDelete.relativePath != "" && File.Exists(tempPresDir + elementToDelete.relativePath))
                {
                    File.Delete(tempPresDir + elementToDelete.relativePath);
                }
                if (elementToDelete.relativMaterialPath != "" && File.Exists(tempPresDir + elementToDelete.relativMaterialPath))
                {
                    File.Delete(tempPresDir + elementToDelete.relativMaterialPath);
                }
                //remove element
                SelectedStage.handout.elements.Remove(elementToDelete);
            }
        }

        //Canvas

        public void addNewText()
        {
            Text2D newText = new Text2D();
            SelectedStage.canvas.elements.Add(newText);
        }
        public void addNewImage()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Select an image.";
            openFileDialog.Filter = "PNG (*.png)|*.png|Bitmap (*.bmp)|*.bmp|JPEG (*.jpeg)|*.jpeg|JPG (*.jpg)|*.jpg|Others (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                string sourcePath = openFileDialog.FileName.ToString();
                string nameOfFile = Path.GetFileNameWithoutExtension(sourcePath);
                string extension = Path.GetExtension(sourcePath);
                string targetTepFolder = tempPresDir + tempSub2D;
                string relativeFolderPath = tempSub2D;

                string appendix = "";
                int appendixCount = 0;
                while (File.Exists(targetTepFolder + nameOfFile + appendix + extension))
                {
                    appendixCount = appendixCount + 1;
                    appendix = "_" + appendixCount;
                }
                string availableNameOfFile = nameOfFile + appendix;


                try
                {
                    File.Copy(sourcePath, targetTepFolder + availableNameOfFile + extension);
                    Image2D newImage = new Image2D(relativeFolderPath + availableNameOfFile + extension);
                    SelectedStage.canvas.elements.Add(newImage);
                }
                catch
                {
                    MessageBox.Show("Sorry - We were not able to add this image.");
                    //ToDo show error in status bar
                }
            }
        }
        public void canvas2DElementSelected(object element)
        {
            //select the concrete Type of Element
            if (typeof(Element2D).IsInstanceOfType(element))
            {
                Element2D selectedElement = (Element2D)element;
                setSelectedElement(selectedElement);
                return;
            }

            //No matching Type found
            //ToDo show error in Statusbar
            MessageBox.Show("The selected object was not detected.");
        }
        public void addSelectedElement(Element element)
        {
            element.highlighted = true;
            selectedElements.Add(element);
            WindowsPropertyList.ItemsSource = selectedElements;
        }
        public void setSelectedElement(Element element)
        {
            foreach(Element elem in selectedElements)
            {
                elem.highlighted = false;
            }
            selectedElements = new ObservableCollection<Element>();
            element.highlighted = true;
            selectedElements.Add(element);
            WindowsPropertyList.ItemsSource = selectedElements;
        }
        public void unselectAllElements()
        {
            foreach (Element elem in selectedElements)
            {
                elem.highlighted = false;
            }
            selectedElements = new ObservableCollection<Element>();
            WindowsPropertyList.ItemsSource = selectedElements;
        }
        public void canvasBackgroundClicked()
        {
            unselectAllElements();
        }
        public void changeImageSource(Image2D element)
        {
            //save old relativeImageSource to remove the source when the new one is added
            string oldRelativePath = element.relativeImageSource;
            //Add reference to new Image
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Select an image.";
            openFileDialog.Filter = "PNG (*.png)|*.png|Bitmap (*.bmp)|*.bmp|JPEG (*.jpeg)|*.jpeg|JPG (*.jpg)|*.jpg|Others (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                string sourcePath = openFileDialog.FileName.ToString();
                string nameOfFile = Path.GetFileNameWithoutExtension(sourcePath);
                string extension = Path.GetExtension(sourcePath);
                string targetTepFolder = tempPresDir + tempSub2D;
                string relativeFolderPath = tempSub2D;

                string appendix = "";
                int appendixCount = 0;
                while (File.Exists(targetTepFolder + nameOfFile + appendix + extension))
                {
                    appendixCount = appendixCount + 1;
                    appendix = "_" + appendixCount;
                }
                string availableNameOfFile = nameOfFile + appendix;


                try
                {
                    File.Copy(sourcePath, targetTepFolder + availableNameOfFile + extension);
                    element.relativeImageSource = relativeFolderPath + availableNameOfFile + extension;
                }
                catch
                {
                    MessageBox.Show("Sorry - We were not able to add this image.");
                    //ToDo show error in status bar
                }

                //Delete old File from temp
                if (File.Exists(tempPresDir + oldRelativePath))
                {
                    File.Delete(tempPresDir + oldRelativePath);
                }
            }
        }

        public void changeMaterialSource(Element3D elem)
        {
            //save old relativ material path to remove the source when the new one is added
            string oldRelativePath = elem.relativMaterialPath;
            //Add reference to new Image
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Select a Material.";
            openFileDialog.Filter = "Material (*.mtl)|*.mtl|Others (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                string sourcePath = openFileDialog.FileName.ToString();
                //The name of the material file needs to be the same as the one of the obj file
                string nameOfFile = Path.GetFileNameWithoutExtension(elem.relativePath);
                string extension = Path.GetExtension(sourcePath);
                string targetTepFolder = tempPresDir + tempSub3D;
                string relativeFolderPath = tempSub3D;

                //Delete old File from temp
                if (File.Exists(tempPresDir + oldRelativePath))
                {
                    File.Delete(tempPresDir + oldRelativePath);
                }

                try
                {
                    //delete here again because there could be another file extension than mtl in the future
                    if(File.Exists(targetTepFolder + nameOfFile + extension))
                    {
                        File.Delete(targetTepFolder + nameOfFile + extension);
                    }
                    File.Copy(sourcePath, targetTepFolder + nameOfFile + extension);
                    elem.relativMaterialPath = relativeFolderPath + nameOfFile + extension;
                    elem.originalMatName = Path.GetFileName(sourcePath);
                }
                catch
                {
                    MessageBox.Show("Sorry - We were not able to add this image.");
                    //ToDo show error in status bar
                }
            }
        }

        public void remove2DElementFromCanvas(Element2D elem)
        {
            unselectAllElements();
            removeRessources(elem);
            SelectedStage.canvas.elements.Remove(elem);
        }

        /// <summary>
        /// Removes the resources of the element from the temporary folder
        /// </summary>
        /// <param name="elem">The element that contains the ressources that need to be removed</param>
        private void removeRessources(Element elem)
        {
            if(elem.GetType() == typeof(Image2D))
            {
                Image2D img = (Image2D)elem;
                if(File.Exists(tempPresDir + img.relativeImageSource))
                {
                    File.Delete(tempPresDir + img.relativeImageSource);
                }
                return;
            }

            if(elem.GetType() == typeof(Element3D))
            {
                Element3D elem3d = (Element3D)elem;
                if (elem3d.relativePath != "" && File.Exists(tempPresDir + elem3d.relativePath))
                {
                    File.Delete(tempPresDir + elem3d.relativePath);
                }
                if (elem3d.relativMaterialPath != "" && File.Exists(tempPresDir + elem3d.relativMaterialPath))
                {
                    File.Delete(tempPresDir + elem3d.relativMaterialPath);
                }
            }
        }

        public void startPresentation(ItemsControl pPresentationWindowCanvasPreview, int pStageIndex)
        {
            //Save the presentation
            saveOpenPresentation();

            //Get the invitation Link
            PresentationStartResponse startRes = connection.startPresentation(openPresentation.presentationId);
            if (startRes == null)
            {
                stopPresentation();
                return;
            }
            //Show the shortCode
            presentationWindow.shortCode.Text = "Short Code: " + startRes.shortCode;

            //Join/Create the Photon Room
            photonClient = new PhotonClient(this);
            photonClient.Connect(startRes.photonRoomName);

            //Setup PhotonRoom

            PresentationWindowCanvasPreview = pPresentationWindowCanvasPreview;
            indexPresentationStage = pStageIndex;

            //
        }

        public void stopPresentation()
        {
            if(photonClient != null)
            {
                photonClient.Disconnect();
            }
            presentationWindow.callerWindow.Show();
            presentationWindow.Close();
        }
        public void nextPresentationStage()
        {
            if (indexPresentationStage + 1 >= 0 && indexPresentationStage + 1 < openPresentation.stages.Count)
            {
                //Send new index in the Photon Room
                photonClient.setStageIndex(indexPresentationStage + 1);
                //indexPresentationStage = indexPresentationStage + 1;
            }
        }
        public void previousPresentationStage()
        {
            if (indexPresentationStage - 1 >= 0 && indexPresentationStage - 1 < openPresentation.stages.Count)
            {
                //Send new index in the Photon Room
                photonClient.setStageIndex(indexPresentationStage - 1);
                //indexPresentationStage = indexPresentationStage - 1;
            }

        }
        public void updatePresStage(int stageIndex)
        {
            indexPresentationStage = stageIndex;
        }

        public void MakeCursorWait(bool value)
        {
            if(presentationWindow != null)
            {
                if (value)
                {
                    presentationWindow.Cursor = Cursors.Wait;
                }
                else
                {
                    presentationWindow.Cursor = Cursors.Arrow;
                }
                
            }
        }
    }
}
