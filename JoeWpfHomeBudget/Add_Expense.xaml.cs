using Budget;
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
using System.Windows.Shapes;

namespace JoeWpfHomeBudget
{
    /// <summary>
    /// Interaction logic for Add_Expense.xaml
    /// </summary>
    public partial class Add_Expense : Window,ExpensesInterface
    {
        private readonly Presenter presenter;

        public Add_Expense()
        {
            InitializeComponent();

            presenter=new Presenter(this);

            SetDateDefault();
            PopulateCategoryInBox();
            

        }
        private void SetDateDefault()
        {
            
            date_expense.DisplayDate = DateTime.Now;
            date_expense.SelectedDate = DateTime.Now;

        }

        private void PopulateCategoryInBox()
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

            try
            {
                if (categoryList.SelectedIndex == -1)
                {
                    throw new Exception("you have not inputed for the category category");
                }
                else if(!double.TryParse(amount_expense.Text, out amount) || Double.IsNaN(amount) || Double.IsInfinity(amount))
                {
                    throw new Exception("the amount is not a valid value");
                }
                else if (double.TryParse(description.Text, out badDescription))
                {
                    throw new Exception("the description is a number");
                }
                else if (description.Text == "")
                {
                    throw new Exception("the description is empty");
                }
                else
                {
                    presenter.AddExpense(date, amount, categoryId, description.Text);
                    this.Hide();
                }
            }
            catch(Exception err)
            {
                MessageBox.Show(err.Message, "Invalid input", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
    }
}
