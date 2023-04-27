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

        int _id;


        public Update_Delete_Budget_Item(Presenter _presenter,int id, DateTime date, int categoryId, double amount, string description)
        {

            InitializeComponent();

            _id = id;
            presenter = _presenter;
            SetDateDefault(date, categoryId, amount, description);

            PopulateCategoryInBox();

        }

        /// <summary>
        /// sets the value default value of an expense to be today.
        /// </summary>
        public void SetDateDefault(DateTime _date, int _categoryId, double _amount, string _desc)
        {

            date_expense.DisplayDate = _date;
            date_expense.SelectedDate = _date;
            description.Text = _desc;
            categoryList.SelectedIndex = _categoryId;
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
           presenter.Delete_Expense(_id);
        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public void PopulateCategoryInBox()
        {
            List<Category> categories = presenter.GetAllCategories();

            foreach (Category category in categories)
            {
                categoryList.Items.Add(category.Description);
            }
        }     
    }
}
