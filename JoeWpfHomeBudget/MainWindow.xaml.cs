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
using System.Windows.Media.Media3D;

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
        private Update_Delete_Budget_Item updateExpense;
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
                //radio_ShowExpensesByCategory();
                //radio_ShowExpensesByMonth();
                checkbox_FilterByCategory();
            }
            else { this.Close(); }
        }
        private void Update_Click(object sender, RoutedEventArgs e)
        {

            var selected = listExpenses.SelectedItem as BudgetItem;
            if (selected != null)
            {
                Update_Delete_Budget_Item _expense = new Update_Delete_Budget_Item(presenter, selected.ExpenseID, selected.Date, selected.CategoryID, selected.Amount, selected.ShortDescription);
                updateExpense = _expense;
                updateExpense.Show();
            }
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

        public void radio_ShowExpensesByMonth()
        {
            List<Expense> expenses = presenter.GetAllExpenses();                       
            List<BudgetItemsByMonth> budgetItemsByMonths;           

            //If there's no content in the database
            if(expenses.Count <= 0 ) 
            {
                return;
            }

            budgetItemsByMonths = presenter.GetBudgetItemsByMonth(DateTime.Today, DateTime.Today.AddDays(200), false, expenses[0].Category);         
            
            //Put data in datagrid
            myDataGrid.ItemsSource = budgetItemsByMonths;

            // clear all the columns and create rows
            myDataGrid.Columns.Clear();

            //Date
            var date = new DataGridTextColumn();
            date.Header = "Date";
            date.Binding = new Binding("Date");
            myDataGrid.Columns.Add(date);

            //Balance
            var total = new DataGridTextColumn();
            total.Header = "Balance";
            total.Binding = new Binding("Total");            
            myDataGrid.Columns.Add(total);            
        }

        public void radio_ShowExpensesByCategory()
        {
            List<Expense> expenses = presenter.GetAllExpenses();           
            List<BudgetItemsByCategory> budgetItemsByCategory;

            //If there's no content in the database
            if (expenses.Count <= 0)
            {
                return;
            }

            budgetItemsByCategory = presenter.GetBudgetItemsByCategories(DateTime.Today, DateTime.Today.AddDays(200), false, expenses[0].Category);
            
            myDataGrid.ItemsSource = budgetItemsByCategory;

            // clear all the columns and create rows
            myDataGrid.Columns.Clear();
                        
            //Date
            var date = new DataGridTextColumn();
            date.Header = "Date";
            date.Binding = new Binding("Date");
            myDataGrid.Columns.Add(date);
            
            //Balance
            var balance = new DataGridTextColumn();
            balance.Header = "Balance";
            balance.Binding = new Binding("Balance");                    
            myDataGrid.Columns.Add(balance);          
        }

        public void checkbox_FilterByCategory()
        {
            List<Expense> expenses = presenter.GetAllExpenses();
            List<BudgetItem> budgetItems;

            budgetItems = presenter.GetBudgetItems(DateTime.Today,DateTime.Today.AddDays(200),false, expenses[0].Category);

            myDataGrid.ItemsSource = expenses;

            // clear all the columns.
            myDataGrid.Columns.Clear();

            //create the columns
            //Date
            var date = new DataGridTextColumn();
            date.Header = "Date";
            date.Binding = new Binding("Date");
            myDataGrid.Columns.Add(date);


            //Category
            var category = new DataGridTextColumn();
            category.Header = "Category";
            category.Binding = new Binding("Category");
            myDataGrid.Columns.Add(category);

            //Description
            var description = new DataGridTextColumn();
            description.Header = "Description";
            description.Binding = new Binding("Description");
            myDataGrid.Columns.Add(description);

            //Amount
            var amount = new DataGridTextColumn();
            amount.Header = "Amount";
            amount.Binding = new Binding("Amount");
            myDataGrid.Columns.Add(amount);

            //Balance
            var balance = new DataGridTextColumn();
            balance.Header = "Balance";
            balance.Binding = new Binding("Balance");
            myDataGrid.Columns.Add(balance);           
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
            listExpenses.IsEnabled = true;
            Refresh_allExpenses();
        }

        private void rbt_byMonth_Checked(object sender, RoutedEventArgs e)
        {
            listExpenses.IsEnabled = true;
            radio_ShowExpensesByMonth();
        }

        private void rbt_byCategory_Checked(object sender, RoutedEventArgs e)
        {
            listExpenses.IsEnabled = true;
            radio_ShowExpensesByCategory();
        }

        private void rbt_byMonthAndCategory_Checked(object sender, RoutedEventArgs e)
        {
            listExpenses.IsEnabled = true;
            
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

        public void Refresh_allExpenses()
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

        public void closingAfterUpdate()
        {
            updateExpense.Close();
        }
    }
}

