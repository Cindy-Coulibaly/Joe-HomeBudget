using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Interaction logic for loadDatabase.xaml
    /// </summary>
    public partial class loadDatabase : Window
    {
        internal string filePath { get; set; }

        private readonly Presenter presenter;
        public loadDatabase()
        {
            InitializeComponent();
            Window test = new Window();

        }

        private void a_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            //default directory will be at Document/Budgets folder
            saveFileDialog.InitialDirectory = $"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\\Budgets";
            saveFileDialog.Filter = "DB Files|*.db";

            if (saveFileDialog.ShowDialog() == true)
            {
                filePath = saveFileDialog.FileName;
                using (FileStream fs = File.Create(filePath));
                MessageBox.Show("worked", "test", MessageBoxButton.OK,
                MessageBoxImage.Asterisk);
                Close();
            }
        }

        private void b_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "DB Files|*.db";
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == true)
            {
                filePath = openFileDialog.FileName;
                MessageBox.Show("worked", "test", MessageBoxButton.OK,
                MessageBoxImage.Asterisk);
                Close();

            }
        }
    }
}
