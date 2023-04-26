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
        void ShowBudgetItem(List<BudgetItem> items);
        void ShowBudgetItemByMonthAndCategory(List<Dictionary<string, object>> items);
        void ShowBudgetItemByMonth(List<BudgetItemsByMonth> items);
        void ShowBudgetItemByCategory(List<BudgetItemsByCategory> items);

    }
}
