using Budget;
using System.Data.SQLite;

namespace Joe_HomeBudget
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            HomeBudget budget= new HomeBudget("test.db", "test_categories.cats", true);

            Console.WriteLine(budget);
            budget.categories.Add("hat", Category.CategoryType.Expense);
            Console.WriteLine("did it");

        }
    }
}