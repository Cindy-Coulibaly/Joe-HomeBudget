using System;
using System.Collections.Generic;
using System.Data.SQLite;
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
    /// An individual category for budget program
    /// Valid category types: Income, Expense, Credit, Saving.
    /// </summary>
    // ====================================================================
    public class Category
    {
        // ====================================================================
        // Properties
        // ====================================================================

        /// <value>
        /// A key to indetify the different categories.
        /// </value>
        public int Id { get; } //------------------------------GO BACK
        /// <value>
        /// The description of the category.
        /// </value>
        public String Description { get;  } //------------------------------GO BACK
        /// <value>
        /// The type of the category (Incore, expense, Credit, Savings).
        /// </value>
        public CategoryType Type { get;  }//------------------------------GO BACK
        /// <summary>
        /// The different type of Category of an item.
        /// </summary>
        public enum CategoryType
        {
            /// <summary>
            /// Adding money to your account.
            /// </summary>
            Income,
            /// <summary>
            /// Spending money.
            /// </summary>
            Expense,
            /// <summary>
            /// Pay the money later, aka in your credit card.
            /// </summary>
            Credit,
            /// <summary>
            /// Saving money for the future, money in your saving account.
            /// </summary>
            Savings
        };

        // ====================================================================
        // Constructor
        /// <summary>
        /// Create a new Category from scratch.
        /// </summary>
        /// <param name="id">The ID of the catagory you want to create.</param>
        /// <param name="description">The description of the catagory you want to create.</param>
        /// <param name="type">The type of category you want to create.</param>
        // ====================================================================
        public Category(int id, String description, CategoryType type = CategoryType.Expense)
        {
            
            this.Id = id;
            this.Description = description;
            this.Type = type;
        }

        // ====================================================================
        // Copy Constructor
        /// <summary>
        /// Makes a copy of a given category.
        /// </summary>
        /// <param name="category">The category the user wants to copy.</param>
        // ====================================================================
        public Category(Category category)
        {
            this.Id = category.Id;;
            this.Description = category.Description;
            this.Type = category.Type;
        }

        

        // ====================================================================
        /// <summary>
        /// Show the description of a given category.
        /// </summary>
        /// <example>
        /// <code>
        /// Category cat = new Category(1, "Food", Category.CategoryType.Expense);
        /// Console.WriteLine(cat.ToString());
        /// </code>
        /// </example>
        /// <returns>The description of the given category.</returns>
        // ====================================================================
        public override string ToString()
        {
            return Description;
        }

  

    }
}

