using Budget;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace JoeWpfHomeBudget
{
    /// <summary>
    /// Interaction logic for Add_Expense.xaml
    /// </summary>
    public partial class Add_Expense : Window,ExpensesInterface
    {
        private readonly Presenter presenter;
        private Boolean submitted;
        private Boolean cancelled;


        public Add_Expense(Presenter _presenter)
        {
            InitializeComponent();

            presenter = _presenter;
            submitted = false;
            cancelled = false;

            SetDateDefault();
            PopulateCategoryInBox();
            

        }
        /// <summary>
        /// sets the value default value of an expense to be today.
        /// </summary>
        public void SetDateDefault()
        {
            
            date_expense.DisplayDate = DateTime.Now;
            date_expense.SelectedDate = DateTime.Now;

        }
        /// <summary>
        /// Populate the Category that we have in the dropdown.
        /// </summary>
        public void PopulateCategoryInBox()
        {
            List<Category> categories = presenter.GetAllCategories();

            foreach (Category category in categories)
            {
                categoryList.Items.Add(category.Description);
               

            }
        }

        private void add_Click(object sender, RoutedEventArgs e)
        {

            DateTime date = date_expense.SelectedDate.Value;
            int categoryId = categoryList.SelectedIndex;

             presenter.AddExpense(date,amount_expense.Text, categoryId, description.Text);
            submitted = true;
                
 

        }

        private void add_Category_Click(object sender, RoutedEventArgs e)
        {
            AddCategory addCategory = new AddCategory(presenter);
            addCategory.ShowDialog();
            PopulateCategoryInBox();

            

        }

        //https://learn.microsoft.com/en-us/dotnet/api/system.windows.window.closing?view=windowsdesktop-7.0
        //How to check if user wants to quit before saving changes
        void SaveChangesValidationBeforeClosing(object sender, CancelEventArgs e)
        {
            // If user did not save changes, notify user and ask for a response            
            if (!submitted && !cancelled)
            {
                if ((amount_expense.Text != string.Empty || description.Text != string.Empty) || 
                    (categoryList.SelectedIndex != -1 && (amount_expense.Text == string.Empty || description.Text == string.Empty)))
                {
                    if (presenter.SaveBeforeClosing())
                    {
                        e.Cancel = false;
                    }
                    else
                    {
                        e.Cancel = true;
                    }
                }
            }
        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            CancelExpense();
        }

        public void ShowError(String err)
        {
            MessageBox.Show(err, "Invalid input", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        public void ShowValid(string message)
        {
            MessageBox.Show(message, "Information", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        public void ClearExpense()
        {
            SetDateDefault();
            amount_expense.Clear();
            categoryList.SelectedIndex= -1;
            description.Clear();
        }
        public void CancelExpense()
        {

            if (MessageBox.Show("Do you really want to cancel your Expense", "Cancel", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                cancelled = true;
                ClearExpense();
            }

        }

        private void close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
