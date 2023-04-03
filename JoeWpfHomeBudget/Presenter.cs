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
        HomeBudget homeBudget = new HomeBudget("dummy.db",true);
        public Presenter() { }


        public void AddCategory( string description,Category.CategoryType categoryType)
        {
            homeBudget.categories.Add(description, categoryType);
        }
    }
}
