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
        private HomeBudget model { get; set; }

        public Presenter(ViewInterface v, string databaseFile, bool newDb)
        {
            model = new HomeBudget(databaseFile, newDb);
            view = v;
        }


        public List<Category> GetAllCategories()
        {
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
                    categoryId = categoryList.SelectedIndex;
                    presenter.AddExpense(date, amount, categoryId, description.Text);
                    MessageBox.Show("New Expense Added", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    submitted = true;

                    this.Close();
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, "Invalid input", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            model.expenses.Add(date, categoryId, amountTemp, description);
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

