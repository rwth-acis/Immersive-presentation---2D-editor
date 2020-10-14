using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace _2D_Editor
{
    /// <summary>
    /// Interaktionslogik für PresentationWindow.xaml
    /// </summary>
    public partial class PresentationWindow : Window
    {
        public MainWindow callerWindow;
        PresentationHandling presentationHandler;
        public PresentationWindow(MainWindow pCaller, PresentationHandling pPresentationHandler, int pStageCount)
        {
            InitializeComponent();
            callerWindow = pCaller;
            presentationHandler = pPresentationHandler;
            presentationHandler.presentationWindow = this;

            PreviewKeyDown += (s, e) => { 
                if (e.Key == Key.Escape) presentationHandler.stopPresentation();
                if (e.Key == Key.Left) previousStage();
                if (e.Key == Key.Right) nextStage();
            };

            presentationHandler.startPresentation(canvasPreview, pStageCount);
        }

        
        private void nextStage()
        {
            presentationHandler.nextPresentationStage();
        }
        private void previousStage()
        {
            presentationHandler.previousPresentationStage();
        }
    }
}
