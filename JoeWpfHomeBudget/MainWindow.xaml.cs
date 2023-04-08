﻿using Budget;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;

namespace JoeWpfHomeBudget
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, ViewInterface
    {
        private Presenter _presenter;
        private Boolean _unsavedChanges;
        public MainWindow()
        {
            InitializeComponent();
            _presenter= new Presenter();
            ShowCats();       
            _unsavedChanges
        }

        public void btn_AddNewCategory()
        {        
            AddCategory addCategory = new AddCategory();
            addCategory.Show();
        }

        public void ShowCats()
        {
            List<Category> categories = _presenter.GetAllCategories();

            foreach (Category category in categories)
            {
                categoryList.Items.Add(category.Description);

            }
        }

        //https://learn.microsoft.com/en-us/dotnet/api/system.windows.window.closing?view=windowsdesktop-7.0
        //How to check if user wants to save changes before closing the window
        
        void DataWindow_Closing(object sender, CancelEventArgs e)
        {                    
            // If user did not save changes, notify user and ask for a response
            if (_unsavedChanges)
            {
                string msg = "There are unsaved changes. Close without saving?";
                MessageBoxResult result =
                  MessageBox.Show(
                    msg,
                    "Data App",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);                
            }
        }
    }
}
