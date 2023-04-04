using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Budget;

namespace JoeWpfHomeBudget
{
    class Presenter
    {
        HomeBudget model = new HomeBudget("dummy.db",true);
        public Presenter() { }


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
    }
}
