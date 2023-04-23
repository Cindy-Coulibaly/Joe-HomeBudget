using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Budget;

namespace JoeWpfHomeBudget
{
    public interface ViewInterface
    {
        void ShowError(string message);
        void ShowValid(string message);
        void ClearExpense();
        void CancelExpense();
        void GetCatergoryInBox(List<Category> categories);

    }
}
