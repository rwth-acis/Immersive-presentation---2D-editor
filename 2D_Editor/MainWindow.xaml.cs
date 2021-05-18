using _2D_Editor.CustomControls;
using CoordinatorConnectorLibrary;
using ImmersivePresentation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _2D_Editor
{
	public enum StartMode
	{
		Open,
		New
	}

	/// <summary>
	/// Interaktionslogik für MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{

		private Boolean propertiesVisible = true;
		public Boolean PropertiesVisible{
			get { return propertiesVisible; }
			set
            {
				if(value == true)
                {
					//propertiesView.Width = 300;
					propertieViewHiddenName.Visibility = Visibility.Collapsed;
					propertiesViewContent.Visibility = Visibility.Visible;
					propertiesVisible = value;
                }
                else
                {
					//propertiesView.Width = 30;
					propertiesViewContent.Visibility = Visibility.Collapsed;
					propertieViewHiddenName.Visibility = Visibility.Visible;
					propertiesVisible = value;
				}
            }
		}

		public PresentationHandling presentationHandler;

		public WelcomeWindow callerWindow;

		public bool moveImage = false;
		public bool moveLabel = false;
		public bool scaleImageCorner = false;
		public Image2D movingImage;
		public Image movingImageElement;
		public Text2D movingLabel;
		public Label movingLabelElement;
		public bool firstClick = true;
		public double startImagePositionX;
		public double startImagePositionY;
		public double startImageScaleX;
		public double startImageScaleY;
		public double startMousePositionX;
		public double startMousePositionY;

		public MainWindow(WelcomeWindow pCallerWindow)
        {
            InitializeComponent();
			callerWindow = pCallerWindow;

			//Initialize the Presentation handler
			presentationHandler = new PresentationHandling(new CoordinatorConnection(), this);

			//connect Presentation in Presentation handler to UI
			stageList.ItemsSource = presentationHandler.openPresentation.stages;
			presentationHandler.openPresentation.stages.Add(new Stage("Test Stage"));
		}

		public MainWindow(WelcomeWindow pCallerWindow, CoordinatorConnection pConnection, StartMode pMode, string pPath)
        {
			InitializeComponent();
			callerWindow = pCallerWindow;

			switch (pMode)
            {
				case StartMode.New:
                    {
						presentationHandler = new PresentationHandling(pConnection, pMode, pPath, this);
						connectPresentationWithUI();
						break;
                    }
				case StartMode.Open:
                    {
						presentationHandler = new PresentationHandling(pConnection, pMode, pPath, this);
						connectPresentationWithUI();
						break;
                    }
				default:
                    {
						presentationHandler = new PresentationHandling(pConnection, this);
						connectPresentationWithUI();
						break;
                    }
            }
			//set initial stage selection to first stage
			if (presentationHandler.openPresentation.stages.Count > 0)
			{
				presentationHandler.SelectedStage = presentationHandler.openPresentation.stages[0];
			}
		}

		private void connectPresentationWithUI()
        {
			stageList.ItemsSource = presentationHandler.openPresentation.stages;
			presentationHandler.WindowsStageListBox = stageList;
			presentationHandler.WindowsSceneListBox = sceneElemetsListbox;
			presentationHandler.WindowsHandoutListBox = handoutElemetsListbox;
			presentationHandler.WindowsCanvasPreview = canvasPreview;
			presentationHandler.WindowsPropertyList = propertiesEditingList;
		}

		private void ExitCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = true;
		}

		private void ExitCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			Application.Current.Shutdown();
		}

		//customize predifined commands
		//new Command
		private void NewCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = true;
		}

		private void NewCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			//Create a new Presentation
			presentationHandler.createAndGetLocationOfNewPresentation();
		}
		//open Command
		private void OpenCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = true;
		}

		private void OpenCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			presentationHandler.loadPresentationAndGetLocation();
		}
		//save Command
		private void SaveCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = true;
		}

		private void SaveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			presentationHandler.saveOpenPresentation();
		}
		//save as Command
		private void SaveAsCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = true;
		}

		private void SaveAsCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			MessageBox.Show("Custom Save As Command");
		}
		//copy Command
		private void CopyCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = true;
		}

		private void CopyCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			MessageBox.Show("Custom Copy Command");
		}
		//paste Command
		private void PasteCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = true;
		}

		private void PasteCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			MessageBox.Show("Custom Paste Command");
		}
		//present Command
		private void PresentCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = true;
		}

		private void PresentCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			PresentationWindow presentationWindow = new PresentationWindow(this, presentationHandler, 0);
			presentationWindow.Show();
			this.Hide();
		}
		//Import PDF Command
		private void ImportPDFCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = true;
		}

		private void ImportPDFCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			presentationHandler.importPDF();
		}
		//presentProperties Command
		private void PresentPropertiesCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = true;
		}

		private void PresentPropertiesCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			MessageBox.Show("Custom Present Properties Command");
		}
		//presentProperties Command
		private void HelpCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = true;
		}

		private void HelpCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			MessageBox.Show("Custom Help Command");
		}

        private void Toggle_Toolbox_LeftMouseDown(object sender, MouseButtonEventArgs e)
        {
			PropertiesVisible = !PropertiesVisible;
        }

        private void Toggle_Toolbox_MouseEnter(object sender, MouseEventArgs e)
        {
			rec_toogle_button.Visibility = Visibility.Visible;
		}

        private void Toggle_Toolbox_MouseLeave(object sender, MouseEventArgs e)
        {
			rec_toogle_button.Visibility = Visibility.Hidden;
		}

        private void AnalogClock_TimeChanged(object sender, TimeChangedEventArgs e)
        {
			//Console.WriteLine(e.NewTime);
        }

        private void stageList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
			//ToDo: robust error handling in case it is not a Stage
			Stage selectedStage = stageList.SelectedItem as Stage;
			presentationHandler.SelectedStage = selectedStage;
        }

        private void addStageButton_LeftMouseDown(object sender, MouseButtonEventArgs e)
        {
			presentationHandler.addNewStage();
        }

        private void deleteSelectedStage_LeftMouseDown(object sender, MouseButtonEventArgs e)
        {
			presentationHandler.deleteSelectedStage();
        }

        private void moveSelectedStageUp_LeftMouseDown(object sender, MouseButtonEventArgs e)
        {
			presentationHandler.moveSelectedStageUp();
        }

        private void moveSelectedStageDown_LeftMouseDown(object sender, MouseButtonEventArgs e)
        {
			presentationHandler.moveSelectedStageDown();
        }

        private void sceneAdd3DElement_LeftMouseDown(object sender, MouseButtonEventArgs e)
        {
			presentationHandler.add3DElementToScene();
        }

        private void sceneDelete3DElement_LeftMouseDown(object sender, MouseButtonEventArgs e)
        {
			presentationHandler.delete3DElementFromScene();
        }

        private void handoutAdd3DElement_LeftMouseDown(object sender, MouseButtonEventArgs e)
        {
			presentationHandler.add3DElementToHandout();
        }

        private void handoutDelete3DElement_LeftMouseDown(object sender, MouseButtonEventArgs e)
        {
			presentationHandler.delete3DElementFromHandout();
        }

        private void canvasAddText_LeftMouseDown(object sender, MouseButtonEventArgs e)
        {
			presentationHandler.addNewText();
        }

        private void canvasAddImage_LeftMouseDown(object sender, MouseButtonEventArgs e)
        {
			presentationHandler.addNewImage();
        }

        private void canvasImage_LeftMouseDown(object sender, MouseButtonEventArgs e)
        {
			presentationHandler.canvas2DElementSelected(((Image)sender).Tag);
			e.Handled = true;

			if (firstClick)
			{
				Cursor = Cursors.Hand;
				moveImage = true;
				firstClick = false;
				movingImageElement = (Image)sender;
				movingImage = (Image2D)movingImageElement.Tag;
				GeneralTransform transform = movingImageElement.TransformToAncestor(movingImageElement.Parent as Visual);
				Point StartPoint = transform.Transform(new Point(0, 0));
				startImagePositionX = StartPoint.X;
				startImagePositionX = StartPoint.Y;

				Point RelativeMousePoint = Mouse.GetPosition(movingImageElement);
				startMousePositionX = RelativeMousePoint.X;
				startMousePositionY = RelativeMousePoint.Y;
				movingImageElement.RenderTransform = new TranslateTransform();
			}
		}

        private void PreviewCanvas_LeftMouseDown(object sender, MouseButtonEventArgs e)
        {
			presentationHandler.canvasBackgroundClicked();
        }

        private void canvasLabel_LeftMouseDown(object sender, MouseButtonEventArgs e)
        {
			presentationHandler.canvas2DElementSelected(((Label)sender).Tag);
			e.Handled = true;

			if (firstClick)
			{
				Cursor = Cursors.Hand;
				moveLabel = true;
				firstClick = false;
				movingLabelElement = (Label)sender;
				movingLabel = (Text2D)movingLabelElement.Tag;
				GeneralTransform transform = movingLabelElement.TransformToAncestor(movingLabelElement.Parent as Visual);
				Point StartPoint = transform.Transform(new Point(0, 0));
				startImagePositionX = StartPoint.X;
				startImagePositionX = StartPoint.Y;

				Point RelativeMousePoint = Mouse.GetPosition(movingLabelElement);
				startMousePositionX = RelativeMousePoint.X;
				startMousePositionY = RelativeMousePoint.Y;
				movingLabelElement.RenderTransform = new TranslateTransform();
			}
		}

        private void PropertyEditorImageSource_Clicked(object sender, RoutedEventArgs e)
        {
			presentationHandler.changeImageSource((Image2D)((Button)sender).Tag);
        }

		private void PropertyEditorPDFUpdate_complete_Clicked(object sender, RoutedEventArgs e)
		{
			presentationHandler.PDFUpdate_complete((PDFElement)((Button)sender).Tag);
		}

		private void PropertyEditorPDFRemove_after_Clicked(object sender, RoutedEventArgs e)
		{
			presentationHandler.PDFRemove_after((PDFElement)((Button)sender).Tag);
		}

		private void PropertyEditorPDFRemove_before_Clicked(object sender, RoutedEventArgs e)
		{
			presentationHandler.PDFRemove_before((PDFElement)((Button)sender).Tag);
		}

		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
			callerWindow.Close();
        }

        private void PropertyEditorMaterialSource_Clicked(object sender, RoutedEventArgs e)
        {
			presentationHandler.changeMaterialSource((Element3D)((Button)sender).Tag);
        }

        private void Scene3DElementSelected(object sender, MouseButtonEventArgs e)
        {
			presentationHandler.setSelectedElement((Element3D)((StackPanel)sender).Tag);
        }

        private void Remove2DElementClicked(object sender, RoutedEventArgs e)
        {
			presentationHandler.remove2DElementFromCanvas((Element2D)((Button)sender).Tag);

		}

		public static readonly List<string> ImageExtensions = new List<string> { ".JPG", ".JPE", ".BMP", ".GIF", ".PNG" };
		private void canvasPreview_Drop(object sender, DragEventArgs e)
        {
			if (e.Data.GetDataPresent(DataFormats.FileDrop))
			{
				string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

				foreach(string file in files)
                {
					if (ImageExtensions.Contains(System.IO.Path.GetExtension(file).ToUpperInvariant()))
					{
						presentationHandler.addNewImage(file);
					}
                }
			}
		}

		public static readonly List<string> ObjektExtensions = new List<string> { ".OBJ" };
		private void sceneElemetsListbox_Drop(object sender, DragEventArgs e)
        {
			if (e.Data.GetDataPresent(DataFormats.FileDrop))
			{
				string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

				foreach (string file in files)
				{
					if (ObjektExtensions.Contains(System.IO.Path.GetExtension(file).ToUpperInvariant()))
					{
						presentationHandler.add3DElementToScene(file);
					}
				}
			}
		}

        private void handoutElemetsListbox_Drop(object sender, DragEventArgs e)
        {
			if (e.Data.GetDataPresent(DataFormats.FileDrop))
			{
				string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

				foreach (string file in files)
				{
					if (ObjektExtensions.Contains(System.IO.Path.GetExtension(file).ToUpperInvariant()))
					{
						presentationHandler.add3DElementToHandout(file);
					}
				}
			}
		}

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (moveImage)
            {
				Point MousePoint = Mouse.GetPosition(canvasPreview);
				double DistanceFromStartX = MousePoint.X - startImagePositionX - startMousePositionX;
				double DistanceFromStartY = MousePoint.Y - startImagePositionY - startMousePositionY;

				double absolutePositionX = startImagePositionX + DistanceFromStartX;
				double absolutePositionY = startImagePositionY + DistanceFromStartY;
				movingImage.xPosition = absolutePositionX / 1920 * 100;
				movingImage.yPosition = absolutePositionY / 1080 * 100;
			} else if (scaleImageCorner)
            {
				Point MousePoint = Mouse.GetPosition(canvasPreview);
				if (MousePoint.X > 1920) MousePoint.X = 1920;
				if (MousePoint.X < 0) MousePoint.X = 0;
				if (MousePoint.Y > 1080) MousePoint.Y = 1080;
				if (MousePoint.Y < 0) MousePoint.Y = 0;

				double DistanceFromStartX = MousePoint.X - startMousePositionX;
				if ((DistanceFromStartX / 1920 * 100) < (-1 * startImageScaleX)) DistanceFromStartX = 0;
				double DistanceFromStartY = MousePoint.Y - startMousePositionY;
				if ((DistanceFromStartY / 1080 * 100) < (-1 * startImageScaleY)) DistanceFromStartY = 0;

				movingImage.xScale = startImageScaleX + ((DistanceFromStartX / 1920 * 100));
				movingImage.yScale = startImageScaleY + ((DistanceFromStartY / 1080 * 100));
			} else if (moveLabel)
            {
				Point MousePoint = Mouse.GetPosition(canvasPreview);
				double DistanceFromStartX = MousePoint.X - startImagePositionX - startMousePositionX;
				double DistanceFromStartY = MousePoint.Y - startImagePositionY - startMousePositionY;

				double absolutePositionX = startImagePositionX + DistanceFromStartX;
				double absolutePositionY = startImagePositionY + DistanceFromStartY;
				movingLabel.xPosition = absolutePositionX / 1920 * 100;
				movingLabel.yPosition = absolutePositionY / 1080 * 100;
			}
		}

        private void Canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (moveImage || scaleImageCorner || moveLabel)
            {
				Cursor = Cursors.Arrow;
				firstClick = true;
				moveImage = false;
				moveLabel = false;
				scaleImageCorner = false;
			}
        }

        private void Rectangle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
			e.Handled = true;

            if (firstClick)
            {
				Cursor = Cursors.SizeNWSE;
				firstClick = false;
				scaleImageCorner = true;
				Rectangle helpRec = (Rectangle)sender;
				movingImage = (Image2D)helpRec.Tag;
				GeneralTransform transform = movingImageElement.TransformToAncestor(movingImageElement.Parent as Visual);
				Point StartPoint = transform.Transform(new Point(0, 0));
				startImagePositionX = StartPoint.X;
				startImagePositionX = StartPoint.Y;
				startImageScaleX = movingImage.xScale;
				startImageScaleY = movingImage.yScale;

				Point RelativeMousePoint = Mouse.GetPosition(canvasPreview);
				startMousePositionX = RelativeMousePoint.X;
				startMousePositionY = RelativeMousePoint.Y;
				movingImageElement.RenderTransform = new TranslateTransform();
			}
        }
    }


    public static class CustomCommands
	{
		public static readonly RoutedUICommand Exit = new RoutedUICommand
			(
				"Exit",
				"Exit",
				typeof(CustomCommands),
				new InputGestureCollection()
				{
					new KeyGesture(Key.F4, ModifierKeys.Alt)
				}
			);

		public static readonly RoutedUICommand Present = new RoutedUICommand
			(
				"Present",
				"Start Presentation",
				typeof(CustomCommands),
				new InputGestureCollection()
				{
					new KeyGesture(Key.P, ModifierKeys.Control)
				}
			);
		public static readonly RoutedUICommand PresentProperties = new RoutedUICommand
			(
				"PresentProperties",
				"Presentation Properties",
				typeof(CustomCommands),
				new InputGestureCollection()
				{
					new KeyGesture(Key.P, ModifierKeys.Control | ModifierKeys.Shift)
				}
			);

		public static readonly RoutedUICommand ImportPDF = new RoutedUICommand
			(
				"Import PDF",
				"Import a PDF",
				typeof(CustomCommands),
				new InputGestureCollection()
				{
					new KeyGesture(Key.I, ModifierKeys.Control)
				}
			);
	}
}
