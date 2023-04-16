using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Budget;

namespace JoeWpfHomeBudget
{

    public partial class Presenter
    {
        private readonly ViewInterface view;
        private readonly ExpensesInterface expensesView;
        private HomeBudget model { get; set; }

        public Presenter(ViewInterface v, string databaseFile, bool newDb)
        {
            model = new HomeBudget(databaseFile, newDb);
            view = v;
        }

        public Presenter(ExpensesInterface v)
        {

            expensesView = v;
        }

        public List<Category> GetAllCategories()
        {
            return model.categories.List();
        }

        public void AddExpense(DateTime date, double amount, int categoryId, string description)
        {
            model.expenses.Add(date, categoryId, amount, description);
        }
        public List<Expense> GetAllExpenses()
        {
            return model.expenses.List();
        }

        public void loadNewDatabase(string databaseFile)
        {
            //loading doesn't create a new database so bool is always false
            model = new HomeBudget(databaseFile, false);
        }

        public void AddCategory(string description, Category.CategoryType categoryType)
        {
            model.categories.Add(description, categoryType);
        }

        public Boolean SaveBeforeClosing()
        {
            if (MessageBox.Show("Are you sure you want to quit? All unsaved changes will be lost.", "Unsaved Changes", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
            {
                return true;
            }
            return false;
        }
    }
}

