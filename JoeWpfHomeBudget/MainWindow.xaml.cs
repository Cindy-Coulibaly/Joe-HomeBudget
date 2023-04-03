using Budget;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace JoeWpfHomeBudget
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, ViewInterface
    {

        private readonly Presenter presenter;
        string filePath = string.Empty;

        public MainWindow()
        {
            //loadDatabase test = new loadDatabase();
            //test.Show();
            InitializeComponent();
            CreateDb();
            presenter = new Presenter(this, filePath);
        }


        private void CreateDb()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            //default directory will be at Document/Budgets folder
            saveFileDialog.InitialDirectory = $"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\\Budgets";
            saveFileDialog.Filter = "DB Files|*.db";

            if (saveFileDialog.ShowDialog() == true)
            {
                filePath = saveFileDialog.FileName;
                using (FileStream fs = File.Create(filePath));
            }
                

        }

        
        public void ChooseDB()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "DB Files|*.db";
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == true)
            {
                //Get the path of specified file
                filePath = openFileDialog.FileName;
            }
        }

        public void CreateDb_btn(object sender, RoutedEventArgs e)
        {
            CreateDb();
        }

        public void ChooseDB_btn(object sender, RoutedEventArgs e) {

            ChooseDB();
        }

    }
}
