using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using Budget;

namespace JoeWpfHomeBudget
{
    /// <summary>
    /// Interaction logic for TestDoubleClick.xaml
    /// </summary>
    public partial class TestDoubleClick : Window
    {
        private readonly Presenter _presenter;
        private List<TempExpense> _expenses;

        public TestDoubleClick(Presenter presenter)
        {
            InitializeComponent();
            _presenter = presenter;

            _expenses = new List<TempExpense>
            {
                new TempExpense { Id = 1, Date = new DateTime(2023, 4, 23), Category = 1, Amount = 500, Description = "movies" }
            };

            expenseClick.ItemsSource = _expenses;
        }

        private void expense_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectedTempExpense = expenseClick.SelectedItem as TempExpense;
            if (expenseClick.SelectedItem != null)
            {
                Update_Delete_Budget_Item updateDeleteBudgetItem = new Update_Delete_Budget_Item(_presenter, selectedTempExpense.Id);
                updateDeleteBudgetItem.ShowDialog();
            }
        }

        public class TempExpense
        {
            public int Id { get; set; }
            public DateTime Date { get; set; }
            public int Category { get; set; }
            public double Amount { get; set; }
            public string Description { get; set; }
        }
    }
}
