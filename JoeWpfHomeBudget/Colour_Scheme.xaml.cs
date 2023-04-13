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
    /// Interaction logic for Colour_Scheme.xaml
    /// </summary>
    public partial class Colour_Scheme : Window
    {
        public Colour_Scheme()
        {
            InitializeComponent();
            SelectColour.SelectedIndex = 0;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SelectColour.Text == "Red")
            {
                Background = Brushes.Red;
            }
            if (SelectColour.Text == "Blue")
            {
                Background = Brushes.Blue;
            }
            if (SelectColour.Text == "Magenta")
            {
                Background = Brushes.Magenta;
            }
            if (SelectColour.Text == "Green")
            {
                Background = Brushes.Green;
            }
        }
    }
}
