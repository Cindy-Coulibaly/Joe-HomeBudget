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
    /// Interaction logic for DuplicateCheckTest.xaml
    /// </summary>
    public partial class DuplicateCheckTest : Window, ViewInterface
    {
        private readonly Presenter presenter;
        public DuplicateCheckTest()
        {
            InitializeComponent();
            presenter = new Presenter(this);
        }

        public void ShowInvalidDuplicateExpense()
        {
            throw new NotImplementedException();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            presenter.CheckIfExpenseExists(int.Parse(id.Text));
        }
    }
}
