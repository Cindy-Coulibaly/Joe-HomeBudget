using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
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
        private Presenter _presenter;

        public AddCategory(Presenter presenter)
        {
            InitializeComponent();           
            _presenter = presenter;
            PopulateCategoryInBox();
        }
        public void btn_Submit(object sender, RoutedEventArgs e){

            if (Validate())
            {
                _presenter.AddCategory(categoryName.Text,(Category.CategoryType)categoryList.SelectedItem);
                MessageBox.Show("New Category Added", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
        }        
        public void PopulateCategoryInBox()
        {
            categoryList.Items.Add(Category.CategoryType.Expense);  
            categoryList.Items.Add(Category.CategoryType.Income);
            categoryList.Items.Add(Category.CategoryType.Credit);
            categoryList.Items.Add(Category.CategoryType.Savings);
        }
        public Boolean Validate()
        {
            if(categoryName.Text == string.Empty)
            {
                MessageBox.Show("Must provide Category Name", "Input Missing", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            else if(categoryList.SelectedIndex == -1)
            {
                MessageBox.Show("Must Select Category Type", "Input Missing", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }
    }
}
