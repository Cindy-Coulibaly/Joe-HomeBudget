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
    public partial class loadDatabase : Window, loadDatabaseInterface
    {
        internal string filePath { get; set; }
        internal bool newDb { get; set; }

        public loadDatabase()
        {
            InitializeComponent();
        }

        private void newDatabase_Click(object sender, RoutedEventArgs e)
        {
            CreateNewDatabase();
        }

        private void loadDatabase_Click(object sender, RoutedEventArgs e)
        {
            ChooseOldDatabase();
        }

        public void ChooseOldDatabase()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            //make so that it only takes
            openFileDialog.Filter = "DB Files|*.db";
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == true)
            {
                filePath = openFileDialog.FileName;
                newDb = false;
                Close();
            }
        }

        public void CreateNewDatabase()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            //default directory will be at Document/Budgets folder
            saveFileDialog.InitialDirectory = $"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\\Budgets";
            saveFileDialog.Filter = "DB Files|*.db";

            if (saveFileDialog.ShowDialog() == true)
            {
                filePath = saveFileDialog.FileName;
                newDb = true;
                using (FileStream fs = File.Create(filePath)) ;
                Close();
            }
        }
    }
}
