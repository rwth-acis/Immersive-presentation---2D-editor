using CoordinatorConnectorLibrary;
using Microsoft.Win32;
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
    /// Interaktionslogik für WelcomeWindow.xaml
    /// </summary>
    public partial class WelcomeWindow : Window
    {

        private bool _loggedIn;
        public bool loggedIn
        {
            get
            {
                return _loggedIn;
            }
            set
            {
                _loggedIn = value;
                if (value)
                {
                    loginForm.Visibility = Visibility.Collapsed;
                    selectPresentation.Visibility = Visibility.Visible;
                }
                else
                {
                    loginForm.Visibility = Visibility.Visible;
                    selectPresentation.Visibility = Visibility.Collapsed;
                }
            }
        }

        private CoordinatorConnection connection;
        public WelcomeWindow()
        {
            InitializeComponent();
            loggedIn = false;
        }

        private void OpenPres_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Open a Presentation";
            openFileDialog.Filter = "Presentation (*.pres)|*.pres|Zip (*.zip)|*.zip";
            if (openFileDialog.ShowDialog() == true)
            {
                MainWindow myMainWindow = new MainWindow(StartMode.Open, openFileDialog.FileName);
                this.Visibility = Visibility.Hidden;
                myMainWindow.Show();
            }
        }

        private void NewPres_Click(object sender, RoutedEventArgs e)
        {
            //Open a SaveFileDialog to get the path where the new presentation should be stored
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "Save New Presentation As";
            saveFileDialog.Filter = "Presentation (*.pres)|*.pres|Zip (*.zip)|*.zip";
            if (saveFileDialog.ShowDialog() == true)
            {
                MainWindow myMainWindow = new MainWindow(StartMode.New, saveFileDialog.FileName);
                this.Visibility = Visibility.Hidden;
                myMainWindow.Show();
            }
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            connection = new CoordinatorConnection(inputEmail.Text, inputPassword.Password);
            if (connection.login())
            {
                loggedIn = true;
            }
            else
            {
                loggedIn = false;

            }
        }

        private void ErrorBoxClose_LeftMouseDown(object sender, MouseButtonEventArgs e)
        {
            errorBox.Visibility = Visibility.Collapsed;
        }
    }
}
