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
        private readonly ExpensesInterface expensesView; 

        public Presenter(ViewInterface v)
        {
            view = v;

            model=new HomeBudget("messy.db"); // the person in charge of the file thingy is the one choosing putting the file
            model.categories.List();


        }
        public Presenter(ExpensesInterface v)
        {
            model = new HomeBudget("./messy.db");
            expensesView = v;
        }

        public List<Category> GetAllCategories()
        {
            return model.categories.List();
        }

        public void AddExpense(DateTime date,double amount,int categoryId,string description)
        {
            model.expenses.Add(date,categoryId,amount,description);
        }
        public List<Expense> GetAllExpenses()
        {
            return model.expenses.List();
        }
        
    }
}
