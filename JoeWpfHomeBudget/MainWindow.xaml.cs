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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace JoeWpfHomeBudget
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, ViewInterface
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public void btn_AddNewCategory()
        {
            //Once the buttons to create a new category are defined in WPF, please make
            //modifications here.
            int textBoxId = 1;
            string textboxDescription = "Groceries";            
            Category.CategoryType textboxCategoryType = Category.CategoryType.Expense;                        
         
            AddCategory addCategory = new AddCategory();
        }
    }
}
