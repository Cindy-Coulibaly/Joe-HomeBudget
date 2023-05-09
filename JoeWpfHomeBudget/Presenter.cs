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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="v">The view Interface.</param>
        /// <param name="databaseFile">The database file.</param>
        /// <param name="newDb">if true then it's a new database, false otherwise</param>
        public Presenter(ViewInterface v, string databaseFile, bool newDb)
        {
            model = new HomeBudget(databaseFile, newDb);
            view = v;
        }


        /// <summary>
        /// Get all the categories in the database from the model
        /// </summary>
        /// <returns> the list of categories</returns>
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

        /// <summary>
        /// Add a new expenses to the database
        /// </summary>
        /// <param name="date"> the expense's date</param>
        /// <param name="amount"> the expense's amount</param>
        /// <param name="categoryId">the expense's category's id</param>
        /// <param name="description">the expense's description</param>
        public void AddExpense(DateTime date, string amount, int categoryId, string description)
        {
            double amountTemp;
            double badDescription;
            try
            {
                if (categoryId == -1)
                {
                    throw new Exception("No category to add has been provided.");
                }
                else if (!double.TryParse(amount, out amountTemp) || Double.IsNaN(amountTemp) || Double.IsInfinity(amountTemp))
                {
                    throw new Exception("The amount to add is not a valid value.");
                }
                else if (double.TryParse(description, out badDescription))
                {
                    throw new Exception("The description to add is a number");
                }
                else if (description == "")
                {
                    throw new Exception("The description is empty.");
                }
                else
                {
                    categoryId = categoryId + 1;
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

        /// <summary>
        /// Update a new expenses to the database
        /// </summary>
        /// <param name="date"> the expense's date</param>
        /// <param name="amount"> the expense's amount</param>
        /// <param name="categoryId">the expense's category's id</param>
        /// <param name="description">the expense's description</param>
        public void UpdateExpense(int id, DateTime date, int category, string amount, string description)
        {
            double amountTemp;
            double badDescription;

            try
            {
                if (category == -1)
                {
                    throw new Exception("No category to update has been provided");
                }
                else if (!double.TryParse(amount, out amountTemp) || Double.IsNaN(amountTemp) || Double.IsInfinity(amountTemp))
                {
                    throw new Exception("The amount to update is not a valid value.");
                }
                else if (double.TryParse(description, out badDescription))
                {
                    throw new Exception("The description to update is a number");
                }
                else if (description == "")
                {
                    throw new Exception("The description is empty.");
                }
                else
                {
                    //category = category + 1;
                    model.expenses.UpdateProperties(id, date, category +1, amountTemp, description);
                    view.ShowValid($"New expense just updated with properties:: Id: {id} Category: {category} Amount: {amount} Description: {description}");
                    view.closingAfterUpdate();
                    view.Refresh_allExpenses();
                }
            }
            catch (Exception err)
            {
                view.ShowError(err.Message);
            }
        }

        /// <summary>
        /// Get all the Expenses in the database from the model
        /// </summary>
        /// <returns> the list of Expenses</returns>
        public List<Expense> GetAllExpenses()
        {
            try
            {
                var listOfExpenses = model.expenses.List();
            }
            catch (Exception err)
            {
                view.ShowError(err.Message);
            }
            return model.expenses.List();
        }

        /// <summary>
        /// Load a new database file
        /// </summary>
        /// <param name="databaseFile">the database file to load</param>
        public void loadNewDatabase(string databaseFile)
        {
            //loading doesn't create a new database so bool is always false
            model = new HomeBudget(databaseFile, false);
        }

        /// <summary>
        /// Add a new category to the database
        /// </summary>
        /// <param name="description"> the category's description</param>
        /// <param name="categoryType">the category's type</param>
        /// <returns></returns>
        public bool AddCategory(string description, int categoryType)
        {
            Category.CategoryType type;
            try
            {
                if (description == string.Empty)
                {
                    throw new Exception("Must provide Category Name");
                }
                else if (description.Any(c => char.IsDigit(c)))
                {
                    throw new Exception("Category Name cannot contain numbers");
                }
                else if (categoryType == -1)
                {
                    throw new Exception("Must Select Category Type");
                }
                else
                {
                    type = (Category.CategoryType)categoryType;
                    model.categories.Add(description, type);
                    view.ShowValid("New Category Added");
                    return true;
                }
            }
            catch (Exception err)
            {
                view.ShowError(err.Message);
                return false;
            }

        }
        /// <summary>
        /// Get a list of budget item depending on the Date and if there is a filter
        /// </summary>
        /// <param name="start">the start date</param>
        /// <param name="end"></param>
        /// <param name="flag"></param>
        /// <param name="CategoryId"></param>
        /// <returns></returns>
        public void GetAllBudgetItem(DateTime start, DateTime end, bool filter, int categoryId)
        {
            try
            {
                categoryId = categoryId + 1;
                List<BudgetItem> expenses = model.GetBudgetItems(start, end, filter, categoryId);
                view.ShowBudgetItem(expenses);
            }
            catch (Exception err)
            {
                view.ShowError(err.Message);
            }

        }

        /// <summary>
        /// Get a list of all the budget item by category and by month
        /// </summary>
        /// <param name="start">The start of date</param>
        /// <param name="end">the end of date</param>
        /// <param name="filter"> if true, the the filter is on, false otherwise</param>
        /// <param name="categoryId">the category'id to filter</param>
        public void GetAllBudgetItemByCategoryAndByMonth(DateTime start, DateTime end, bool filter, int categoryId)
        {
            try
            {
                categoryId = categoryId + 1;
                List<Dictionary<string, object>> expenses = model.GetBudgetDictionaryByCategoryAndMonth(start, end, filter, categoryId);
                view.ShowBudgetItemByMonthAndCategory(expenses);

            }
            catch (Exception err)
            {
                view.ShowError(err.Message);
            }
        }

        /// <summary>
        /// Get a list of all the budget item  by month
        /// </summary>
        /// <param name="start">The start of date</param>
        /// <param name="end">the end of date</param>
        /// <param name="filter"> if true, the the filter is on, false otherwise</param>
        /// <param name="categoryId">the category'id to filter</param>
        public void GetAllBudgetItemByMonth(DateTime start, DateTime end, bool filter, int categoryId)
        {
            try
            {
                categoryId = categoryId + 1;
                List<BudgetItemsByMonth> expenses = model.GetBudgetItemsByMonth(start, end, filter, categoryId);
                view.ShowBudgetItemByMonth(expenses);

            }
            catch (Exception err)
            {
                view.ShowError(err.Message);
            }
        }

        /// <summary>
        /// Get a list of all the budget item  by category
        /// </summary>
        /// <param name="start">The start of date</param>
        /// <param name="end">the end of date</param>
        /// <param name="filter"> if true, the the filter is on, false otherwise</param>
        /// <param name="categoryId">the category'id to filter</param>
        public void GetAllBudgetItemByCategory(DateTime start, DateTime end, bool filter, int categoryId)
        {
            try
            {
                categoryId = categoryId + 1;
                List<BudgetItemsByCategory> expenses = model.GetBudgetItemsByCategory(start, end, filter, categoryId);
                view.ShowBudgetItemByCategory(expenses);


            }
            catch (Exception err)
            {
                view.ShowError(err.Message);
            }
        }

        /// <summary>
        /// Delete an expense in the database
        /// </summary>
        /// <param name="id">the expense's id</param>
        public void Delete_Expense(int id)
        {
            try
            {
                if (id < 0)
                {
                    throw new Exception("There's no number");
                }

                model.expenses.Delete(id);
               // view.closingAfterUpdate();
                view.Refresh_allExpenses();
            }
            catch (Exception err)
            {
                view.ShowError(err.Message);
            }
        }

        /// <summary>
        /// delete expense after update and refresh it
        /// </summary>
        /// <param name="id">the expenses id</param>
        public void Delete_Expense_InUpdate(int id)
        {
            Delete_Expense(id);
            view.closingAfterUpdate();
        }

        /// <summary>
        /// refresh the display table with new expense
        /// </summary>
        public void RefreshWithNewExpense()
        {
            view.CalledRefresh();
        }

        public void ValidUpdate_Delete(BudgetItem item)
        {
            if (item == null)
            {
                view.ShowError("The item you've been trying to update/ delete is null");
            }
        }

        public bool changedExpenses(string amount,string description,int categoryId)
        {
            return (amount != string.Empty || description != string.Empty) ||
                    (categoryId != -1 && (amount == string.Empty || description== string.Empty));
        }
    }

}

