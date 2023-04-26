﻿using Budget;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using Microsoft.Win32;
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
        private Add_Expense expense;

        public MainWindow()
        {
            InitializeComponent();
            initializeDatabase();
            //if the user hasn't choose or created a database then close the main window
            if (filePath != null)
            {
                presenter = new Presenter(this, filePath, newDb);
                ShowCats();
                unsavedChanges = false;
            }
            else { this.Close(); }
            rbt_allExpenses.IsChecked = true;



        }
        private void Delete_Click(object sender, RoutedEventArgs e)
        {

            var selected = listExpenses.SelectedItem as BudgetItem;
            if (selected != null)
            {
                presenter.Delete_Expense(selected.ExpenseID);
            }
        }

        private void Add_Expense_Click(object sender, RoutedEventArgs e)
        {

            Add_Expense _expense = new Add_Expense(presenter);
            expense = _expense;
            expense.Show();
        }

        private void Remove_Expense_Click(object sender, RoutedEventArgs e)
        {

        }

        public void btn_AddNewCategory(object sender, RoutedEventArgs e)
        {
            AddCategory addCategory = new AddCategory(presenter);
            addCategory.ShowDialog();
        }

        public void ShowCats()
        {
            List<Category> categories = presenter.GetAllCategories();

            foreach (Category category in categories)
            {
                categoryList.Items.Add(category.Description);
                cmbCategories.Items.Add(category.Description);

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
                MessageBox.Show(filePath + " file is now in uses", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
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


        public void ChooseDatabase_btn(object sender, RoutedEventArgs e)
        {

            ChooseDB();

        }

        private void close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public void ShowError(string err)
        {
            MessageBox.Show(err, "Invalid input", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public void ShowValid(string message)
        {
            MessageBox.Show(message, "Information", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public void ClearExpense()
        {
            expense.ClearExpense();
        }

        public void CancelExpense()
        {

            if (MessageBox.Show("Do you really want to cancel your Expense", "Cancel", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                ClearExpense();
            }

        }

        private void input_Changed(object sender, SelectionChangedEventArgs e)
        {
            if(rbt_allExpenses.IsChecked==true)
            {
                rbt_allExpenses_Checked(sender,e);
            }


        }

        private void rbt_allExpenses_Checked(object sender, RoutedEventArgs e)
        {
            Refresh();
        }

        private void rbt_byMonth_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void rbt_byCategory_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void rbt_byMonthAndCategory_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void Filter_Checked(object sender, RoutedEventArgs e)
        {
            if(rbt_allExpenses.IsChecked == true)
            {
                rbt_allExpenses_Checked(sender, e);
            }
        }

        public void GetBudgetItem(List<BudgetItem> items)
        {
            listExpenses.ItemsSource = items;
            listExpenses.Columns.Clear();


            //create the columns and bind them
            var date = new DataGridTextColumn();
            date.Header = "Date";
            date.Binding = new Binding("Date");
            listExpenses.Columns.Add(date);

            var category = new DataGridTextColumn();
            category.Header = "Category";
            category.Binding = new Binding("Category");
            listExpenses.Columns.Add(category);

            var description = new DataGridTextColumn();
            description.Header = "Description";
            description.Binding = new Binding("ShortDescription");
            listExpenses.Columns.Add(description);

            var amount = new DataGridTextColumn();
            amount.Header = "Amount";
            amount.Binding = new Binding("Amount");
            listExpenses.Columns.Add(amount);

            var balance = new DataGridTextColumn();
            balance.Header = "Balance";
            balance.Binding = new Binding("Balance");
            listExpenses.Columns.Add(balance);
        }

        public void Refresh()
        {
            // get all the specificities
            DateTime start = DateTime.MinValue;
            DateTime end = DateTime.MaxValue;

            if (StartDate.SelectedDate != null)
            {
                start = StartDate.SelectedDate.Value;
            }

            if (EndDate.SelectedDate != null)
            {
                end = EndDate.SelectedDate.Value;
            }


            bool filter = (bool)Filter.IsChecked;
            int categoryId = cmbCategories.SelectedIndex;

            //get the list of items
            presenter.GetAllBudgetItem(start, end, filter, categoryId);
        }
    }
}

