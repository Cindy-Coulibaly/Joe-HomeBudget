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
using Budget;

namespace JoeWpfHomeBudget
{
    /// <summary>
    /// Interaction logic for Update_Delete_Budget_Item.xaml
    /// </summary>
    public partial class Update_Delete_Budget_Item : Window, Update_Delete_Interface
    {
        private readonly Presenter presenter;
        Expense _selectedExpense;
        private Boolean submitted;
        private Boolean cancelled;

        public Update_Delete_Budget_Item(Presenter _presenter,Expense selectedExpense)
        {

            InitializeComponent();

            presenter = _presenter;
            _selectedExpense = selectedExpense;
            submitted = false;
            cancelled = false;

            SetDateDefault();
            PopulateCategoryInBox();

        }

        private void update_Click(object sender, RoutedEventArgs e)
        {
            DateTime date = date_expense.SelectedDate.Value;
            int categoryId = categoryList.SelectedIndex;

            presenter.UpdateExpense(_selectedExpense.Id,date,categoryId, amount_expense.Text, description.Text);
            submitted = true;
        }

        private void delete_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        public void ShowError(String err)
        {
            MessageBox.Show(err, "Invalid input", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public void ShowValid(string message)
        {
            MessageBox.Show(message, "Information", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public void SetDateDefault()
        {
            date_expense.DisplayDate = DateTime.Now;
            date_expense.SelectedDate = DateTime.Now;
        }

        public void PopulateCategoryInBox()
        {
            List<Category> categories = presenter.GetAllCategories();

            foreach (Category category in categories)
            {
                categoryList.Items.Add(category.Description);
            }
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

        private void add_Category_Click(object sender, RoutedEventArgs e)
        {
                AddCategory addCategory = new AddCategory(presenter);
                addCategory.ShowDialog();
                PopulateCategoryInBox();
        }
    }
}
