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
        private readonly HomeBudget model;
        private readonly ViewInterface view;

        public Presenter(ViewInterface v)
        {
            view = v;

            model=new HomeBudget("./messy.db",false); // the person in charge of the file thingy is the one choosing putting the file
            model.categories.List();


        }

        public List<Category> GetAllCategories()
        {
            return model.categories.List();
        }

        public void AddExpense(DateTime date,double amount,int categoryId,string description)
        {
            model.expenses.Add(date,categoryId,amount,description);
        }
        
    }
}
