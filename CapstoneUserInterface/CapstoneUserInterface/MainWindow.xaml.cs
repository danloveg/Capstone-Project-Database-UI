using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
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
        }

        private void LoadListViewAvailableFolders()
        {
            sqliteHelper = new Sqlite(databasePath);
            sqliteHelper.CreateDatabaseIfDoesNotExist();
            itemList = sqliteHelper.GetAllImages();
            ListViewAvailableFolders.ItemsSource = itemList;
        }

        private void ListViewAvailableFolders_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void ImportButton_Click(object sender, RoutedEventArgs e)
        {
        }

        private void OpenButton_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}
