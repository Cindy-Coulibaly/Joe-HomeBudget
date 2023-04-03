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
using Budget;

namespace JoeWpfHomeBudget
{
    /// <summary>
    /// Interaction logic for AddCategory.xaml
    /// </summary>
    public partial class AddCategory : Window
    {
        public AddCategory()
        {
            InitializeComponent();
        }

        public void btn_Click(object sender, RoutedEventArgs e)
        {           
            Presenter p1 = new Presenter();

            int textBoxId = 1;
            string textboxDescription = "Groceries";
            Category.CategoryType textboxCategoryType = Category.CategoryType.Expense;


        }
    }
}
