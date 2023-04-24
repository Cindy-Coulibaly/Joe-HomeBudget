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
    /// Interaction logic for TestDoubleClick.xaml
    /// </summary>
    public partial class TestDoubleClick : Window
    {
        private readonly Presenter _presenter;
        private Expense _selectedExpense;
        public TestDoubleClick(Presenter presenter)
        {
            InitializeComponent();
            _presenter = presenter;
            _selectedExpense = new Expense(1, new DateTime(2023, 4, 23), 1, 500, "movies");
            DataContext = _selectedExpense;
        }
        private void TextBlock_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (BudgetItems.SelectedItem != null)
            {
                Update_Delete_Budget_Item updateDeleteBudgetItem = new Update_Delete_Budget_Item(_presenter,_selectedExpense);
                updateDeleteBudgetItem.ShowDialog();
            }
        }


    }
}
