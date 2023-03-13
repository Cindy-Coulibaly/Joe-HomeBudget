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
    /// <summary>
    /// An item that you want to add in your budget, which will be an expense,
    /// this is where you can say what is your item and when did you get the item
    /// and finally what is the cost of this item.
    /// </summary>
    // ====================================================================
    public class Expense
    {
        // ====================================================================
        // Properties
        // ====================================================================
        /// <value>
        /// The key to identify the different expenses.
        /// </value>
        public int Id { get; }
        /// <value>
        /// The date of when expense accured.
        /// </value>
        public DateTime Date { get;  }
        /// <value>
        /// The value of the expense, how much was this expense.
        /// </value>
        public Double Amount { get; }
        /// <value>
        /// The description of this expense, what was the expense.
        /// </value>
        public String Description { get; }
        /// <value>
        /// The category id of the expense.
        /// </value>
        public int Category { get; }

        // ====================================================================
        // Constructor
        //    NB: there is no verification the expense category exists in the
        //        categories object
        /// <summary>
        /// Create a new expense from scratch.
        /// </summary>
        /// <param name="id">The Id of the expense.</param>
        /// <param name="date">The date of when the expense accure.</param>
        /// <param name="category">The category id of this expense.</param>
        /// <param name="amount">The value of the expense, how much it is.</param>
        /// <param name="description">The description of the expenses, what is it.</param>
        // ====================================================================
        public Expense(int id, DateTime date, int category, Double amount, String description)
        {
            this.Id = id;
            this.Date = date;
            this.Category = category;
            this.Amount = amount;
            this.Description = description;
        }

        // ====================================================================
        // Copy constructor - does a deep copy
        /// <summary>
        /// create a copy of an expense.
        /// </summary>
        /// <param name="obj">The expense that you want to copy.</param>
        // ====================================================================
        public Expense (Expense obj)
        {
            this.Id = obj.Id;
            this.Date = obj.Date;
            this.Category = obj.Category;
            this.Amount = obj.Amount;
            this.Description = obj.Description;
           
        }
    }
}
