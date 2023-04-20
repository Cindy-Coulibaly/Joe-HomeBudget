using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        private readonly Presenter _presenter;
        private Boolean _submitted;

        public AddCategory(Presenter presenter)
        {
            InitializeComponent();           
            _presenter = presenter;
            PopulateCategoryInBox();
        }
        public void btn_Submit(object sender, RoutedEventArgs e){

        public void btn_Submit(object sender, RoutedEventArgs e)
        {           
            Presenter p1 = new Presenter();
            string textboxDescription = "Groceries";
            Category.CategoryType textboxCategoryType = Category.CategoryType.Expense;
                        
            
            p1.AddCategory(textboxDescription, textboxCategoryType );           
        }
        public void ShowCategoryTypesComboBox()
        {
            List<AddCategory> categoryTypes= new List<AddCategory>();
            
        }
    }
}
