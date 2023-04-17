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
            int categoryId=0;
            double amount;
            double badDescription;


             categoryId = categoryList.SelectedIndex;
             presenter.AddExpense(date,amount_expense.Text, categoryId, description.Text);
            MessageBox.Show("New Expense Added", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            submitted = true;
            this.Close();
                
 

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
            cancelled = true;
            this.Close();
        }

        public void ShowError(Exception err)
        {
            MessageBox.Show(err.Message, "Invalid input", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
