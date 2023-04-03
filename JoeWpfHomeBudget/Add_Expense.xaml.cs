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
    public partial class Add_Expense : Window
    {
        public Add_Expense()
        {
            InitializeComponent();
            date_expense.DisplayDate=DateTime.Now;
            date_expense.SelectedDate=DateTime.Now;
        }
    }
}
