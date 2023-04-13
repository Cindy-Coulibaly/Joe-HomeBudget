using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Budget;

namespace JoeWpfHomeBudget
{

    class Presenter
    {
        private readonly ViewInterface view;
        private HomeBudget model { get; set; }

        //need to provide file name for 
        public Presenter(ViewInterface v, string databaseFile, bool newDb)
        {
            model = new HomeBudget(databaseFile, newDb);
            view = v;
        }
        
        public void loadNewDatabase(string databaseFile)
        {
            //loading doesn't create a new database so bool is always false
            model = new HomeBudget(databaseFile, false);
        }
        public void AddCategory( string description,Category.CategoryType categoryType)
        {
            model.categories.Add(description, categoryType);
        }

        public List<Category> GetAllCategories()
        {
            return model.categories.List();
        }

        
        public void AddExpense(DateTime date, double amount, int categoryId, string description)
        {
            model.expenses.Add(date, categoryId, amount, description);
        }

        public void SaveBeforeClosing()
        {
            Boolean unsavedChanges = true;
            if(unsavedChanges)
            {
                if (MessageBox.Show("Would you like to save the following changes?", "Save Changes?", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                {
                    //Save changes and close Window
                }
            }
            else
            {
                //Discard changes and close Window
            }
        }
    }
}

