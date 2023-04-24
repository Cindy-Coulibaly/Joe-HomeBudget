using Budget;
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
            if (filePath != null) { presenter = new Presenter(this, filePath, newDb);
                ShowCats();
                unsavedChanges = false;
                radio_ShowExpensesByCategory();
                //radio_ShowExpensesByMonth();
                //checkbox_FilterByCategory();
            }
            else { this.Close(); }
            

        }

        private void Add_Expense_Click(object sender, RoutedEventArgs e)
        {

            Add_Expense _expense = new Add_Expense(presenter);
            expense=_expense;
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

        public void radio_ShowExpensesByMonth()
        {
            List<Expense> expenses = presenter.GetAllExpenses();

            //If there's no content in the database
            if(expenses.Count <= 0 ) 
            {
                return;
            }
            
            myDataGrid.ItemsSource = expenses;

            // clear all the columns and create rows
            myDataGrid.Columns.Clear();

            //Date
            var date = new DataGridTextColumn();
            date.Header = "Date";
            date.Binding = new Binding("Date");

            //Balance
            var balance = new DataGridTextColumn();
            balance.Header = "Balance";
            balance.Binding = new Binding("Balance");

            double sum = 0;
            string prevMonth = expenses[0].Date.ToString("yyyy,MM");
            foreach (Expense expense in myDataGrid.ItemsSource)
            {
                //Sums the expenses for each month
                if (expense.Date.ToString("yyyy,MM") == prevMonth)
                {
                    sum += expense.Amount;                                   
                }
                else
                {
                    prevMonth = expense.Date.ToString("yyyy,MM");
                    sum += expense.Amount;
                    myDataGrid.Columns.Add(date);
                    myDataGrid.Columns.Add(balance);
                }
            }
        }

        public void radio_ShowExpensesByCategory()
        {
            List<Expense> expenses = presenter.GetAllExpenses();
            List<Category> category = presenter.GetAllCategories();

            //If there's no content in the database
            if (expenses.Count <= 0)
            {
                return;
            }
                           
            myDataGrid.ItemsSource = expenses;

            // clear all the columns and create rows
            myDataGrid.Columns.Clear();
                        
            //Date
            var date = new DataGridTextColumn();
            date.Header = "Date";
            date.Binding = new Binding("Date");

            //Balance
            var balance = new DataGridTextColumn();
            balance.Header = "Balance";
            balance.Binding = new Binding("Balance");

            double sum = 0;

            int prevCategory =expenses[0].Category;

            foreach (Expense expense in myDataGrid.ItemsSource)
            {
                string categoryName = category[expense.Category - 1].Description;

                //Sums the expenses for each category
                if (expense.Category == prevCategory)
                {
                    sum += expense.Amount;                                 
                }
                else
                {
                    prevCategory = category[expense.Category - 1].Id;
                    sum = 0;
                    sum += expense.Amount;
                    myDataGrid.Columns.Add(date);
                    myDataGrid.Columns.Add(balance);
                }
            }
        }

        public void checkbox_FilterByCategory()
        {
            List<Expense> expenses = presenter.GetAllExpenses();

            myDataGrid.ItemsSource = expenses;

            // clear all the columns.
            myDataGrid.Columns.Clear();


            //create the columns
            //Date
            var date = new DataGridTextColumn();
            date.Header = "Date";
            date.Binding = new Binding("Date");

            
            //Category
            var category = new DataGridTextColumn();
            category.Header = "Category";
            category.Binding = new Binding("Category");

            //Description
            var description = new DataGridTextColumn();
            description.Header = "Description";
            description.Binding = new Binding("Description");

            //Amount
            var amount = new DataGridTextColumn();
            amount.Header = "Amount";
            amount.Binding = new Binding("Amount");

            //Balance
            var balance = new DataGridTextColumn();
            balance.Header = "Balance";
            balance.Binding = new Binding("Balance");


            //Makes sure to show what the user wishes to see
            foreach (Expense expense in myDataGrid.ItemsSource)
            {
                if (expense.Category == categoryList.SelectedIndex)
                {
                    myDataGrid.Columns.Add(date);
                    myDataGrid.Columns.Add(category);
                    myDataGrid.Columns.Add(description);
                    myDataGrid.Columns.Add(amount);
                    myDataGrid.Columns.Add(balance);
                }
            }
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
            
            if ( MessageBox.Show("Do you really want to cancel your Expense", "Cancel", MessageBoxButton.YesNo,MessageBoxImage.Question)==MessageBoxResult.Yes)
            {
                ClearExpense();
            }

        }
    }
}

