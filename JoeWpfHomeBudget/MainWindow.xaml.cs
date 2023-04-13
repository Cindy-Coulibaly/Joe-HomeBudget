﻿using Budget;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
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
using System.Xml.Linq;
using System.ComponentModel;


namespace JoeWpfHomeBudget
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, ViewInterface
    {

        
        private readonly Presenter presenter;
        string filePath = string.Empty;
        bool newDb = false;
        private Boolean unsavedChanges;
        
        public MainWindow()
        {
            InitializeComponent();    
            initializeDatabase();
            //if the user hasn't choose or created a database then close the main window
            if (filePath != null) { presenter = new Presenter(this, filePath, newDb); }
            else { this.Close(); }           
            ShowCats();          
            unsavedChanges = false;           
        }

        public void btn_AddNewCategory(object sender, RoutedEventArgs e)
        {        
            AddCategory addCategory = new AddCategory(_presenter);
            addCategory.Show();
        }

        public void ShowCats()
        {
            List<Category> categories = presenter.GetAllCategories();

            foreach (Category category in categories)
            {
                categoryList.Items.Add(category.Description);

            }
        }
        
        
        public void ChooseDB()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "DB Files|*.db";
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == true)
            {
                //Get the path of specified file
                filePath = openFileDialog.FileName;
                presenter.loadNewDatabase(filePath);        
            }
        }

        //make the user choose whether they are working on a new or old database
        public void initializeDatabase()
        {
            loadDatabase selectDatabase = new loadDatabase();
            selectDatabase.DataContext = this;
            selectDatabase.ShowDialog();
            filePath = selectDatabase.filePath;
            newDb = selectDatabase.newDb;    
        }       

        //https://learn.microsoft.com/en-us/dotnet/api/system.windows.window.closing?view=windowsdesktop-7.0
        //How to check if user wants to save changes before closing the window
        
        void SaveChangesValidationBeforeClosing(object sender, CancelEventArgs e)
        {                    
            // If user did not save changes, notify user and ask for a response
            if (_unsavedChanges)
            {
                string msg = "Would you like to save your changes before exiting?";
                MessageBoxResult result =
                  MessageBox.Show(
                    msg,
                    "Unsaved Changes",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Information);                
            }
        }

        public void ChooseDatabase_btn(object sender, RoutedEventArgs e) {

            ChooseDB();
        }
    }
}
