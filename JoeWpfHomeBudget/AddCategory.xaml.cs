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

        /// <summary>
        /// Create an Category window to let the user add a category
        /// </summary>
        /// <param name="_presenter">The presenter of this project</param>
        public AddCategory(Presenter presenter)
        {
            InitializeComponent();           
            _presenter = presenter;
            PopulateCategoryInBox();
        }

        private void btn_Submit(object sender, RoutedEventArgs e){
            int categoryType = categoryList.SelectedIndex;
           bool good= _presenter.AddCategory(categoryName.Text, categoryType);

            if (good)
            {this.Close();}

            _submitted = true;
            
            
        }           
        private void PopulateCategoryInBox()
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
                if (_presenter.changedCategory(categoryName.Text,categoryList.SelectedIndex))
                {
                   var result = MessageBox.Show("Are you sure you want to quit? All unsaved changes will be lost.", "Unsaved Changes", MessageBoxButton.YesNo, MessageBoxImage.Information);
                   e.Cancel= (result.Equals(MessageBoxResult.Yes)) ? false :true;
                }
            }
        }
    }
}
