﻿using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace JoeWpfHomeBudget
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, ViewInterface
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Add_Expense_Click(object sender, RoutedEventArgs e)
        {
            Add_Expense expense=new Add_Expense();
            expense.Show();
        }

        private void Remove_Expense_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
