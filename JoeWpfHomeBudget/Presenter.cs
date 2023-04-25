using System;
using System.Collections;
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
        private HomeBudget model { get; set; }

        public Presenter(ViewInterface v, string databaseFile, bool newDb)
        {
            model = new HomeBudget(databaseFile, newDb);
            view = v;
        }


        public List<Category> GetAllCategories()
        {
            try
            {
                var listAllCategories = model.categories.List();
            }
            catch (Exception err)
            {
                view.ShowError(err.Message);
            }
            return model.categories.List();
        }

        public void AddExpense(DateTime date, string amount, int categoryId, string description)
        {
            double amountTemp;
            double badDescription;
            try
            {
                if (categoryId == -1)
                {
                    throw new Exception("you have not inputed for the category category");
                }
                else if (!double.TryParse(amount, out amountTemp) || Double.IsNaN(amountTemp) || Double.IsInfinity(amountTemp))
                {
                    throw new Exception("the amount is not a valid value");
                }
                else if (double.TryParse(description, out badDescription))
                {
                    throw new Exception("the description is a number");
                }
                else if (description == "")
                {
                    throw new Exception("the description is empty");
                }
                else
                {
                    model.expenses.Add(date, categoryId, amountTemp, description);
                    view.ShowValid($"New expense just added named: {description}");
                    view.ClearExpense();
                }
            }
            catch (Exception err)
            {
                view.ShowError(err.Message);
            }
        }
        public List<Expense> GetAllExpenses()
        {
            try
            {
                var listOfExpenses = model.expenses.List();
            }
            catch(Exception err)
            {
                view.ShowError(err.Message);
            }
            return model.expenses.List();
        }

        public void loadNewDatabase(string databaseFile)
        {
            //loading doesn't create a new database so bool is always false
            model = new HomeBudget(databaseFile, false);
        }

        public bool AddCategory(string description, int categoryType)
        {
            Category.CategoryType type;
            try {
                int notNumeric;
                if (description == string.Empty)
                {
                    throw new Exception("Must provide Category Name");
                }
                else if (int.TryParse(description, out notNumeric))
                {
                    throw new Exception("Category Name cannot contain numbers");
                }
                else if (categoryType == -1)
                {
                    throw new Exception ("Must Select Category Type");
                }
                else
                {
                    type= (Category.CategoryType)categoryType;
                    model.categories.Add(description, type);
                    view.ShowValid("New Category Added");
                    return true;
                }
            }
            catch(Exception err) {
                view.ShowError(err.Message);
                return false;
            }
            
        }

        public List<BudgetItemsByMonth> GetExpensesByMonth(DateTime? Start, DateTime? End, bool FilterFlag, int CategoryID)
        {
            List<BudgetItemsByMonth> budgetItemsByMonths = new List<BudgetItemsByMonth>();
            try 
            {
                budgetItemsByMonths = model.GetBudgetItemsByMonth(Start,End,FilterFlag,CategoryID);
            }
            catch(Exception err)
            {
                view.ShowError(err.Message);
            }
            return budgetItemsByMonths;
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

