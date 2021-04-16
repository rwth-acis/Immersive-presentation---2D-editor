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
    /// Interaktionslogik für PDFImportDialog.xaml
    /// </summary>
    public partial class PDFImportDialog : Window
    {
        PresentationHandling presHandling;
        string pdfpath;

        public PDFImportDialog()
        {
            InitializeComponent();
        }

        public PDFImportDialog(PresentationHandling pPresHandling)
        {
            presHandling = pPresHandling;
        }

        private void ShowAdvanced(object sender, MouseButtonEventArgs e)
        {
            advancedPDFImportSettings.Visibility = Visibility.Visible;
        }

        private void SelectPdfFile(object sender, RoutedEventArgs e)
        {


            showSelection.Text = "";
        }
    }
}
