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
            int categoryType = categoryList.SelectedIndex;
           bool good= _presenter.AddCategory(categoryName.Text, categoryType);

            if (good)
            {this.Close();}

            _submitted = true;
            
            
        }        
        public void PopulateCategoryInBox()
        {
            categoryList.Items.Add(Category.CategoryType.Expense);  
            categoryList.Items.Add(Category.CategoryType.Income);
            categoryList.Items.Add(Category.CategoryType.Credit);
            categoryList.Items.Add(Category.CategoryType.Savings);
        }
        //https://learn.microsoft.com/en-us/dotnet/api/system.windows.window.closing?view=windowsdesktop-7.0
        //How to check if user wants to quit before saving changes
        void SaveChangesValidationBeforeClosing(object sender, CancelEventArgs e)
        {
            // If user did not save changes, notify user and ask for a response            
            if (!_submitted)
            {
                if (categoryName.Text != string.Empty || (categoryList.SelectedIndex != -1 && categoryName.Text == string.Empty))
                {
                    if (_presenter.SaveBeforeClosing())
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
    }
}
