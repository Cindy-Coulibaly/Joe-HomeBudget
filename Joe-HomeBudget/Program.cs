using Budget;
using System.Data.SQLite;

namespace Joe_HomeBudget
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            HomeBudget budget= new HomeBudget("test.db", true);

            //Console.WriteLine(budget);

            //budget.categories.Add("hat", Category.CategoryType.Expense);
            //budget.categories.AddCategoriesToDatabase2("z", Category.CategoryType.Expense);
            //budget.categories.AddCategoriesToDatabase1(new Category(2, "a", Category.CategoryType.Expense));
            //budget.categories.AddCategoriesToDatabase1(new Category(4, "b", Category.CategoryType.Expense));
            //budget.categories.AddCategoriesToDatabase1(new Category(3, "c", Category.CategoryType.Expense));
            //budget.categories.AddCategoriesToDatabase1(new Category(2, "d", Category.CategoryType.Expense));//check duplicate
            //budget.categories.AddCategoriesToDatabase2("e", Category.CategoryType.Expense); // should be 4 since 3 has been added
            //budget.categories.AddCategoriesToDatabase2("f", Category.CategoryType.Expense);
            //budget.categories.UpdateInDatabase(4, "g", Category.CategoryType.Savings);
            //budget.categories.UpdateInDatabase(10, "h", Category.CategoryType.Savings);
            Console.WriteLine("\nDid it!");

        }
    }
}