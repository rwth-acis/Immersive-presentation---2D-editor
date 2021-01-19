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
using System.IO;
using IdentityModel.OidcClient;

namespace _2D_Editor
{
    /// <summary>
    /// Interaktionslogik für WelcomeWindow.xaml
    /// </summary>
    public partial class WelcomeWindow : Window
    {
        private OidcClient _oidcClient = null;
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
                    //start loading list
                    ListResponse listres = connection.loadPresentationList();
                    downloadPresList.ItemsSource = listres.presentations;
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
                MainWindow myMainWindow = new MainWindow(this, connection, StartMode.Open, openFileDialog.FileName);
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
                MainWindow myMainWindow = new MainWindow(this, connection, StartMode.New, saveFileDialog.FileName);
                this.Visibility = Visibility.Hidden;
                myMainWindow.Show();
            }
        }

        private void DownloadButton_Click(object sender, RoutedEventArgs e)
        {
            //Determin position where to store the download
            string downloadFolder = System.IO.Path.GetTempPath().ToString() + "ImPres\\download";
            Directory.CreateDirectory(downloadFolder);
            string downloadPath = downloadFolder + "\\presentationDownload.pres";
            //Determin which presentation was selected
            if (downloadPresList.SelectedIndex == -1) return;
            //Download the presentation to the destination
            if(connection.downloadPresentation(downloadPath, ((PresentationElement)downloadPresList.SelectedValue).idpresentation))
            {
                //Open the main window for editing the presentation
                MainWindow myMainWindow = new MainWindow(this, connection, StartMode.Open, downloadPath);
                this.Visibility = Visibility.Hidden;
                myMainWindow.Show();
            }
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            //Indicate activity
            loadingSpinner.Visibility = Visibility.Visible;
            Cursor = Cursors.Wait;
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
            Cursor = Cursors.Arrow;
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

        private void RegisterButton_Clicked(object sender, RoutedEventArgs e)
        {
            //Indicate Activity
            Cursor = Cursors.Wait;

            string email = inputRegisterEmail.Text;
            string password = inputRegisterPassword.Password;
            string passwordCheck = inputRegisterPasswordCheck.Password;

            //PasswordCheck guard
            if(password != passwordCheck)
            {
                registerSuccessBox.Visibility = Visibility.Collapsed;
                registerErrorMessage.Text = "The two passwords do not match.";
                registerErrorBox.Visibility = Visibility.Visible;
                Cursor = Cursors.Arrow;
                return;
            }
            string msg = connection.register(email, password);
            if (msg == "")
            {
                //Success
                registerErrorBox.Visibility = Visibility.Collapsed;
                registerSuccessMessage.Text = "Sucessfully registered. Please check your emails.";
                registerSuccessBox.Visibility = Visibility.Visible;
            }
            else
            {
                //Success
                registerSuccessBox.Visibility = Visibility.Collapsed;
                registerErrorMessage.Text = msg;
                registerErrorBox.Visibility = Visibility.Visible;
            }

            Cursor = Cursors.Arrow;
        }

        private async void LoginLearningLayers(object sender, RoutedEventArgs e)
        {
            //var options = new OidcClientOptions()
            //{
            //    Authority = "https://demo.identityserver.io/",
            //    ClientId = "interactive.public",
            //    Scope = "openid profile email",
            //    RedirectUri = "http://127.0.0.1/sample-wpf-app",
            //    Browser = new WpfEmbeddedBrowser()
            //};
            string envClientID = Properties.Settings.Default.OIDCclientID;
            string envClientSecret = Properties.Settings.Default.OIDCclientSecret;
            var options = new OidcClientOptions()
            {
                Authority = "https://api.learning-layers.eu/o/oauth2/",
                ClientId = envClientID,
                ClientSecret = envClientSecret,
                Scope = "openid profile email",
                RedirectUri = "http://127.0.0.1/sample-wpf-app",
                Browser = new WpfEmbeddedBrowser()
            };

            _oidcClient = new OidcClient(options);

            LoginResult result;
            try
            {
                result = await _oidcClient.LoginAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected Error: {ex.Message}");
                return;
            }

            if (result.IsError)
            {
                Console.WriteLine(result.Error == "UserCancel" ? "The sign-in window was closed before authorization was completed." : result.Error);
            }
            else
            {
                var name = result.User.Identity.Name;
                var accessToken = result.AccessToken;
                string email = "";
                var claimList = result.User.Claims.ToList();
                foreach(System.Security.Claims.Claim claim in claimList)
                {
                    if(claim.Type == "email")
                    {
                        email = claim.Value;
                        break;
                    }
                }
                //Console.WriteLine($"Hello {name}");
                //MessageBox.Show($"Sucessfully Logged in as {name}. With Email: {email}  - But this is just a demo.");
                //Register/Login the authenthicated user
                if (connection.loginLearningLayers(email, accessToken))
                {
                    loggedIn = true;
                }
                else
                {
                    loggedIn = false;
                    errorMessage.Text = "Invalid Login Data.";
                    errorBox.Visibility = Visibility.Visible;
                }
            }
            Cursor = Cursors.Arrow;
            loadingSpinner.Visibility = Visibility.Hidden;
        }
    }
}
