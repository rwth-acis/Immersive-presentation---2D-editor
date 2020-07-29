﻿using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _2D_Editor
{
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

        public MainWindow()
        {
            InitializeComponent();
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
			MessageBox.Show("Custom New Command");
		}
		//open Command
		private void OpenCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = true;
		}

		private void OpenCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			MessageBox.Show("Custom Open Command");
		}
		//save Command
		private void SaveCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = true;
		}

		private void SaveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			MessageBox.Show("Custom Save Command");
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
			MessageBox.Show("Custom Present Command");
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
	}
}
