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
                rbt_allExpenses.IsChecked = true;
            }
            else { this.Close(); }



        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {

            listExpenses.SelectedItem = null;
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
        /// <summary>
        /// Show all the categories and put in the comboBox
        /// </summary>
        public void ShowCats()
        {
            List<Category> categories = presenter.GetAllCategories();

            foreach (Category category in categories)
            {
                categoryList.Items.Add(category.Description);
                cmbCategories.Items.Add(category.Description);

            }
        }
        /// <summary>
        /// Let the user choose a file where they want to use the database on
        /// </summary>
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
        /// <summary>
        /// make the user choose whether they are working on a new or old database
        /// </summary>
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

        /// <summary>
        /// How to check if user wants to save changes before closing the window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ChooseDatabase_btn(object sender, RoutedEventArgs e)
        {

            ChooseDB();

        }

        private void close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Show a message if there is a error with the operation
        /// </summary>
        /// <param name="err">the error message</param>
        public void ShowError(string err)
        {
            MessageBox.Show(err, "Invalid input", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        /// <summary>
        /// Show a message if the operation was successful
        /// </summary>
        /// <param name="message">The success message</param>
        public void ShowValid(string message)
        {
            MessageBox.Show(message, "Information", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /// <summary>
        /// Calls expense.clear
        /// </summary>
        public void ClearExpense()
        {
            expense.ClearExpense();
        }

        /// <summary>
        /// Show message to make sure that the user really want to cancel an expense
        /// </summary>
        public void CancelExpense()
        {

            if (MessageBox.Show("Do you really want to cancel your Expense", "Cancel", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                ClearExpense();
            }

        }

        private void input_Changed(object sender, SelectionChangedEventArgs e)
        {
            if (rbt_allExpenses.IsChecked == true)
            {
                rbt_allExpenses_Checked(sender, e);
            }
            else if (rbt_byMonthAndCategory.IsChecked == true)
            {
                rbt_byMonthAndCategory_Checked(sender, e);
            }
            else if (rbt_byMonth.IsChecked == true)
            {
                rbt_byMonth_Checked(sender, e);
            }
            else if (rbt_byCategory.IsChecked == true)
            {
                rbt_byCategory_Checked(sender, e);
            }


        }

        private void rbt_allExpenses_Checked(object sender, RoutedEventArgs e)
        {
            menuItem_Update.IsEnabled = true;
            menuItem_Delete.IsEnabled = true;
            Refresh_allExpenses();

        }

        private void rbt_byMonth_Checked(object sender, RoutedEventArgs e)
        {
            menuItem_Update.IsEnabled = false;
            menuItem_Delete.IsEnabled = false;

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
            presenter.GetAllBudgetItemByMonth(start, end, filter, categoryId);
        }

        private void rbt_byCategory_Checked(object sender, RoutedEventArgs e)
        {
            menuItem_Update.IsEnabled = false;
            menuItem_Delete.IsEnabled = false;

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
            presenter.GetAllBudgetItemByCategory(start, end, filter, categoryId);

        }

        private void rbt_byMonthAndCategory_Checked(object sender, RoutedEventArgs e)
        {
            menuItem_Update.IsEnabled = false;
            menuItem_Delete.IsEnabled = false;

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

            presenter.GetAllBudgetItemByCategoryAndByMonth(start, end, filter, categoryId);

        }

        private void Filter_Checked(object sender, RoutedEventArgs e)
        {
            if (rbt_allExpenses.IsChecked == true)
            {
                rbt_allExpenses_Checked(sender, e);
            }
            else if (rbt_byMonthAndCategory.IsChecked == true)
            {
                rbt_byMonthAndCategory_Checked(sender, e);
            }
            else if (rbt_byMonth.IsChecked == true)
            {
                rbt_byMonth_Checked(sender, e);
            }
            else if (rbt_byCategory.IsChecked == true)
            {
                rbt_byCategory_Checked(sender, e);
            }
        }

        /// <summary>
        /// Show all the expenses every in this file, depending on the user input
        /// </summary>
        /// <param name="items">The list of expenses of needed with the balance </param>
        public void ShowBudgetItem(List<BudgetItem> items)
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


        /// <summary>
        /// Show all the items grouped by category and by month,depending on the user input
        /// </summary>
        /// <param name="items">The list of all the expense grouped by month and cateogry</param>
        public void ShowBudgetItemByMonthAndCategory(List<Dictionary<string, object>> items)
        {
            listExpenses.ItemsSource = items;
            listExpenses.Columns.Clear();

            // get the list of category 
            List<Category> categories = presenter.GetAllCategories();

            var month = new DataGridTextColumn();
            month.Header = "Month";
            month.Binding = new Binding("[Month]");
            listExpenses.Columns.Add(month);

            var total = new DataGridTextColumn();
            total.Header = "Total";
            total.Binding = new Binding("[Total]");
            listExpenses.Columns.Add(total);

            foreach (var category in categories)
            {
                var column = new DataGridTextColumn();
                column.Header = category;
                column.Binding = new Binding($"[{category}]"); // Notice the square brackets!
                listExpenses.Columns.Add(column);
            }


        }

        /// <summary>
        /// Show all month and the total expenses of that given month,depending on the user input
        /// </summary>
        /// <param name="items">The list of months with their total amount of expenses</param>
        public void ShowBudgetItemByMonth(List<BudgetItemsByMonth> items)
        {
            listExpenses.ItemsSource = items;
            listExpenses.Columns.Clear();

            var date = new DataGridTextColumn();
            date.Header = "Month";
            date.Binding = new Binding("Month");
            listExpenses.Columns.Add(date);

            var total = new DataGridTextColumn();
            total.Header = "Total";
            total.Binding = new Binding("Total");
            listExpenses.Columns.Add(total);
        }

        /// <summary>
        /// Show all categories and the total expenses of that given cateogry, depending on the user input
        /// </summary>
        /// <param name="items">The list of categories with their total amount of expenses</param>
        public void ShowBudgetItemByCategory(List<BudgetItemsByCategory> items)
        {
            listExpenses.ItemsSource = items;
            listExpenses.Columns.Clear();

            var date = new DataGridTextColumn();
            date.Header = "Category";
            date.Binding = new Binding("Category");
            listExpenses.Columns.Add(date);

            var total = new DataGridTextColumn();
            total.Header = "Total";
            total.Binding = new Binding("Total");
            listExpenses.Columns.Add(total);
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

