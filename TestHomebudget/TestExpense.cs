using System;
using Xunit;
using Budget;

namespace BudgetCodeTests
{
    [Collection("Sequential")]
    public class TestExpense
    {
        // ========================================================================

        [Fact]
        public void ExpenseObject_New()
        {

            // Arrange
            DateTime now = DateTime.Now;
            double amount = 24.55;
            string descr = "New Sweater";
            int category = 34;
            int id = 42;

            // Act
            Expense expense = new Expense(id, now, category, amount, descr);

            // Assert 
            Assert.IsType<Expense>(expense);

            Assert.Equal(id, expense.Id);
            Assert.Equal(amount, expense.Amount);
            Assert.Equal(descr, expense.Description);
            Assert.Equal(category, expense.Category);
            Assert.Equal(now, expense.Date);
        }

        // ========================================================================

        [Fact]
        public void ExpenseCopyConstructoryIsDeepCopy()
        {

            // Arrange
            DateTime now = DateTime.Now;
            double amount = 24.55;
            string descr = "New Sweater";
            int category = 34;
            int id = 42;
            Expense expense = new Expense(id, now, category, amount, descr);

            // Act
            double copyAmount = expense.Amount + 15;
            Expense copy = new Expense(id, now, category, copyAmount, descr);//---------------------REVIEW
            //Expense copyOld = new Expense(expense);

            // Assert 
            Assert.Equal(id, expense.Id);
            Assert.NotEqual(amount, copy.Amount);
            Assert.Equal(expense.Amount + 15, copy.Amount);
            Assert.Equal(descr, expense.Description);
            Assert.Equal(category, expense.Category);
            Assert.Equal(now, expense.Date);
        }


        // ========================================================================

        [Fact]
        public void ExpenseObjectGetSetProperties()
        {
            // question - why cannot I not change the date of an expense.  What if I got the date wrong?

            // Arrange
            DateTime now = DateTime.Now;
            double amount = 24.55;
            string descr = "New Sweater";
            int category = 34;
            int id = 42;
            double newAmount = 54.55;
            string newDescr = "Angora Sweater";
            int newCategory = 38;

            Expense expense = new Expense(id, now, category, amount, descr);


            // Act
            Expense newExpense = new Expense(id, now, newCategory, newAmount, newDescr);
            //expense.Amount = newAmount;
            //expense.Category = newCategory;
            //expense.Description = newDescr;

            // Assert 
            Assert.True(typeof(Expense).GetProperty("Date").CanWrite == false);//---------------------REVIEW
            Assert.True(typeof(Expense).GetProperty("Id").CanWrite == false);
            Assert.Equal(newAmount, newExpense.Amount);
            Assert.Equal(newDescr, newExpense.Description);
            Assert.Equal(newCategory, newExpense.Category);
            //Assert.Equal(newAmount, expense.Amount);
            //Assert.Equal(newDescr, expense.Description);
            //Assert.Equal(newCategory, expense.Category);
        }

        // ========================================================================

        [Fact]
        public void ExpenseObject_PropertiesAreReadOnly()
        {
            // Arrange
            DateTime now = DateTime.Now;
            double amount = 24.55;
            string descr = "New Sweater";
            int category = 34;
            int id = 42;

            // Act
            Expense expense = new Expense(id, now, category, amount, descr);

            // Assert 
            Assert.IsType<Expense>(expense);
            Assert.True(typeof(Expense).GetProperty("Id").CanWrite == false);
            Assert.True(typeof(Expense).GetProperty("Date").CanWrite == false);
            Assert.True(typeof(Expense).GetProperty("Amount").CanWrite == false);
            Assert.True(typeof(Expense).GetProperty("Description").CanWrite == false);
            Assert.True(typeof(Expense).GetProperty("Category").CanWrite == false);
        }
    }
}
