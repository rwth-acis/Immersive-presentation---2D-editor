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
using System.IO;
using System.Text.RegularExpressions;

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
            InitializeComponent();
            presHandling = pPresHandling;
        }

        private void ShowAdvanced(object sender, MouseButtonEventArgs e)
        {
            advancedPDFImportSettings.Visibility = Visibility.Visible;
        }

        private void SelectPdfFile(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Multiselect = false;
            openFileDialog.Filter = "PDF files (*.pdf)|*.pdf|All files (*.*)|*.*";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (openFileDialog.ShowDialog() == true)
            {
                if (openFileDialog.FileNames.Length >= 2)
                {
                    MessageBox.Show("Please only select one PDF.");
                }

                pdfpath = openFileDialog.FileName;
                showSelection.Text = System.IO.Path.GetFileNameWithoutExtension(pdfpath);
            }
        }

        private void StartImport(object sender, RoutedEventArgs e)
        {
            if(pdfpath == null || pdfpath == "" || !File.Exists(pdfpath))
            {
                MessageBox.Show("You need to select an existing pdf file first.");
                return;
            }

            //default dpi of 300
            int dpi = 300;
            if ((bool)checkBoxIndividualDPI.IsChecked)
            {
                try
                {
                    dpi = Convert.ToInt32(inputDPI.Text);
                }
                catch
                {
                    Console.WriteLine("The individual dpi value was not convertable to an integer.");
                }
            }

            if ((bool)checkBoxIndividualMapping.IsChecked)
            {
                try
                {
                    int pdfStart = Convert.ToInt32(inputFirstPdf.Text);
                    int stageStart = Convert.ToInt32(inputFirstStage.Text);
                    int numberOfPages = Convert.ToInt32(inputPdfCount.Text);

                    //Convert page and stage number to indexes
                    pdfStart--;
                    stageStart--;


                    presHandling.performImportPdf(pdfpath, dpi, pdfStart, stageStart, numberOfPages);
                    this.Close();

                }
                catch
                {
                    MessageBox.Show("The input strings for the advanced settings have not the correct format. Only integers are allowed.");
                }
            }
            else
            {
                presHandling.performImportPdf(pdfpath, dpi);
                this.Close();
            }
        }
    }
}
