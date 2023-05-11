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
    public partial class Update_Delete_Budget_Item : Window
    {
        private readonly Presenter presenter;

        int _id;

        /// <summary>
        /// Update an Expense window to let the user update an expense
        /// </summary>
        /// <param name="_presenter"> The presenter of this project.</param>
        /// <param name="id"> the expenses's Id</param>
        /// <param name="date"> the expense's date</param>
        /// <param name="categoryId"> the expense's category Id</param>
        /// <param name="amount">the expense's amount</param>
        /// <param name="description"> the expense's description</param>
        public Update_Delete_Budget_Item(Presenter _presenter, BudgetItem item)
        {
            InitializeComponent();
            presenter = _presenter;

            presenter.ValidUpdate_Delete(item);
            _id = item.ExpenseID;
            

            SetDateDefault(item.Date, item.CategoryID, item.Amount, item.ShortDescription);
            PopulateCategoryInBox();
        }

        private void SetDateDefault(DateTime _date, int _categoryId, double _amount, string _desc)
        {

            date_expense.DisplayDate = _date;
            date_expense.SelectedDate = _date;
            description.Text = _desc;
            categoryList.SelectedIndex = _categoryId -1;
            amount_expense.Text = _amount.ToString();
        }

        private void update_Click(object sender, RoutedEventArgs e)
        {
            DateTime date = date_expense.SelectedDate.Value;
            int categoryId = categoryList.SelectedIndex;

            presenter.UpdateExpense(_id,date,categoryId, amount_expense.Text, description.Text);
        }

        private void delete_Click(object sender, RoutedEventArgs e)
        {
           presenter.Delete_Expense_InUpdate(_id);
        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void PopulateCategoryInBox()
        {
            List<Category> categories = presenter.GetAllCategories();

            foreach (Category category in categories)
            {
                categoryList.Items.Add(category.Description);
            }
        }     
    }
}
