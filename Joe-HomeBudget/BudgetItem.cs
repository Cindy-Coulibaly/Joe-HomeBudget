using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// ============================================================================
// (c) Sandy Bultena 2018
// * Released under the GNU General Public License
// ============================================================================

namespace Budget
{
    // ====================================================================
    // CLASS: BudgetItem
    //        A single budget item, includes Category and Expense
    // ====================================================================

    /// <summary>
    /// All the different item in your budget which their given description: it's category, expense , when
    /// it happened, and how much was it.
    /// </summary>
    /// <example>
    /// <code>
    /// BugdgetItem item = new BudgetItem();
    /// item.CategoryID=15;
    /// item.ExpenseID=3;
    /// item.Date=Date.Now();
    /// item.Category="Food";
    /// item.ShortDescription="takis";
    /// item.Amount="3.99";
    /// </code>
    /// </example>
    public class BudgetItem
    {
        /// <value>
        /// The key to indentify the category of the item.
        /// </value>
        public int CategoryID { get; set; }
        /// <value>
        /// the key to indentify the expense.
        /// </value>
        public int ExpenseID { get; set; }
        /// <value>
        /// the date of which this expense accured.
        /// </value>
        public DateTime Date { get; set; }
        /// <value>
        /// the description of the category of this item.
        /// </value>
        public String Category { get; set; }
        /// <value>
        /// the description of the given item.
        /// </value>
        public String ShortDescription { get; set; }
        /// <value>
        /// how much was this item.
        /// </value>
        public Double Amount { get; set; }
        /// <value>
        /// the balance of your account after this item.
        /// </value>
        public Double Balance { get; set; }

    }

    /// <summary>
    /// A list of all the items in the budget sorted by Year/Month.
    /// </summary>
    /// <example>
    /// <code>
    /// <![CDATA[
    ///BudgetItemsByMonth items=New BudgetItemsByMonth();
    ///items.Details=List<BusgetItem()>;
    /// ]]>
    /// </code>
    /// </example>
    public class BudgetItemsByMonth
    {
        /// <value>
        /// The month number and the year number.
        /// </value>
        public String Month { get; set; }
        /// <value>
        /// The list of all the items of this given month.
        /// </value>
        public List<BudgetItem> Details { get; set; }
        /// <value>
        /// The total of all the items of this given month.
        /// </value>
        public Double Total { get; set; }
    }

    /// <summary>
    /// A list of all the items in the budget sorted by Category.
    /// </summary>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// double total=0;
    /// BudgetItem one = new BudgetItem();
    /// one.CategoryID=15;
    /// one.ExpenseID=1;
    /// one.Date=Date.Now();
    /// one.Category="Food";
    /// one.ShortDescription="takis";
    /// one.Amount="3.99";
    /// 
    /// BudgetItem two = new BudgetItem();
    /// two.CategoryID=2;
    /// two.ExpenseID=2;
    /// two.Date=Date.Now();
    /// two.Category="Food";
    /// two.ShortDescription="Pad Thai";
    /// two.Amount="19.99";
    /// 
    /// List<BudgetItem> items=new List<BudgetItem>();
    /// items.Add(one);
    /// items.Add(two);
    /// 
    /// BudgetItemsByCategory itemsByCategory=new BudgetItemsByCategory();
    /// itemsByCategory.Category=one.Category;
    /// itemsByCategory.Details=items;
    /// 
    /// foreach(BudgetItem money in itemsByCategory.Details){
    /// total+=money.Amount;
    /// }
    /// itemsByCategory.Total=total;
    /// 
    /// ]]>
    /// </code>
    /// </example>
    public class BudgetItemsByCategory
    {
        /// <value>
        /// The name of the given category.
        /// </value>
        public String Category { get; set; }
        /// <value>
        /// The list of all the item in this given category.
        /// </value>
        public List<BudgetItem> Details { get; set; }
        /// <value>
        /// The total of all the items of this given cateroy.
        /// </value>
        public Double Total { get; set; }

    }


}
