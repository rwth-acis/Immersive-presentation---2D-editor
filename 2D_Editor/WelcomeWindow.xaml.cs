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
                if (value == true)
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
            if(Properties.Settings.Default.userEmail != string.Empty && Properties.Settings.Default.userPassword != string.Empty)
            {
                inputEmail.Text = Properties.Settings.Default.userEmail;
                inputPassword.Password = Properties.Settings.Default.userPassword;
                inputRemember.IsChecked = Properties.Settings.Default.userRemember;
            }
            loggedIn = false;
            connection = new CoordinatorConnection();
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

        private void DownloadButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Not available yet");
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            //Indicate activity
            loadingSpinner.Visibility = Visibility.Visible;
            //Save input
            string inpEmail = inputEmail.Text;
            string inpPassword = inputPassword.Password;
            bool inpRemember = (inputRemember.IsChecked == true);
            if (connection.login(inpEmail, inpPassword))
            {
                loggedIn = true;
                //Save Logindata when user wants to
                if(inpRemember == true)
                {
                    Properties.Settings.Default.userEmail = inpEmail;
                    Properties.Settings.Default.userPassword = inpPassword;
                    Properties.Settings.Default.userRemember = inpRemember;
                    Properties.Settings.Default.Save();
                }
                else
                {
                    Properties.Settings.Default.userEmail = "";
                    Properties.Settings.Default.userPassword = "";
                    Properties.Settings.Default.userRemember = false;
                    Properties.Settings.Default.Save();
                }
            }
            else
            {
                loggedIn = false;
                errorMessage.Text = "Invalid Login Data.";
                errorBox.Visibility = Visibility.Visible;
            }
            loadingSpinner.Visibility = Visibility.Hidden;
        }

        private void ErrorBoxClose_LeftMouseDown(object sender, MouseButtonEventArgs e)
        {
            errorBox.Visibility = Visibility.Collapsed;
        }

        private void Register_Clicked(object sender, MouseButtonEventArgs e)
        {
            loginForm.Visibility = Visibility.Collapsed;
            registerForm.Visibility = Visibility.Visible;
        }

        private void Login_Clicked(object sender, MouseButtonEventArgs e)
        {
            loginForm.Visibility = Visibility.Visible;
            registerForm.Visibility = Visibility.Collapsed;
        }

        private void RegisterErrorClose_Clicked(object sender, MouseButtonEventArgs e)
        {
            registerErrorBox.Visibility = Visibility.Collapsed;
        }

        private void RegisterSuccess_Clicked(object sender, MouseButtonEventArgs e)
        {
            registerSuccessMessage.Visibility = Visibility.Collapsed;
        }
    }
}
