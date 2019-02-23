using System.Collections.Generic;
using System.IO;
using System.Management.Automation;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Ookii.Dialogs.Wpf;
using SQLiteDatabase;

namespace CapstoneUserInterface
{
    public partial class MainWindow : Window
    {
        private readonly string databasePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"ENG4600_Capstone.db");
        private readonly string scriptPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Scripts\openSlicer.ps1");

        private List<SQLiteDatabase.Image> itemList;
        private Sqlite sqliteHelper;


        public MainWindow()
        {
            InitializeComponent();
            LoadListViewAvailableFolders();
            SetRemoteSignedExecutionPolicy();
            OpenButton.IsEnabled = false;
        }

        private void LoadListViewAvailableFolders()
        {
            sqliteHelper = new Sqlite(databasePath);
            sqliteHelper.CreateDatabaseIfDoesNotExist();
            itemList = sqliteHelper.GetAllImages();
            ListViewAvailableFolders.ItemsSource = itemList;
        }

        // Set the execution policy to remote signed for this process asynchronously
        public void SetRemoteSignedExecutionPolicy()
        {
            new Task(() =>
            {
                using (var ps = PowerShell.Create())
                {
                    ps.AddScript("Set-ExecutionPolicy -Scope Process -ExecutionPolicy RemoteSigned");
                    var output = ps.Invoke();
                }
            }).Start();
        }

        private void ListViewAvailableFolders_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            OpenButton.IsEnabled = true;
        }

        private void ImportButton_Click(object sender, RoutedEventArgs e)
        {
            // Create a folder picker
            var dialog = new VistaFolderBrowserDialog
            {
                Description = "Choose a folder containing DICOM data",
                UseDescriptionForTitle = true
            };

            // Show it and get the chosen folder
            string selectedFolder;
            if ((bool)dialog.ShowDialog(this))
            {
                selectedFolder = dialog.SelectedPath;
                MessageBox.Show(this, "The selected folder was: " + dialog.SelectedPath, "Sample folder browser dialog");

                // Add Image to database.
                //sqliteHelper.AddImage(<Some Image>)

                // Reload list view

                // If ListView has no selected item now, disable the open button
            }
            // If user did not choose a folder, do nothing
            else
            {
                return;
            }

        }

        private void OpenButton_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}
