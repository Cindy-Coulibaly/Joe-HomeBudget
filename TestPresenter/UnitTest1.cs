using Budget;
using JoeWpfHomeBudget;

namespace TestPresenter
{

    public class TestView : ViewInterface
    {
        public bool calledShowError;
        public bool calledShowValid;
        public bool calledClearExpense;
        public bool calledCancelExpense;
        public bool calledGetBudgetItem;
        public bool calledRefresh_allExpenses;
        public bool calledclosingAfterUpdate;
        public bool calledShowBudgetItemByMonthAndCategory;
        public bool calledShowBudgetItemByMonth;
        public bool calledShowBudgetItemByCategory;
        public bool calledcalledRefresh;
        public bool calleddeleteAdterUpdate;

        public void ShowError(string msg)
        {
            calledShowError = true;
        }

        public void ShowValid(string message)
        {
            calledShowValid = true;
        }

        public void ClearExpense()
        {
            calledClearExpense = true;
        }

        public void CancelExpense()
        {
            calledCancelExpense = true;
        }

        public void ShowBudgetItem(List<BudgetItem> items)
        {
            calledGetBudgetItem = true;
        }

        public void Refresh_allExpenses()
        {
            calledRefresh_allExpenses = true;
        }

        public void closingAfterUpdate()
        {
            calledclosingAfterUpdate = true;
        }

        public void ShowBudgetItemByMonthAndCategory(List<Dictionary<string, object>> items)
        {
            calledShowBudgetItemByMonthAndCategory = true;
        }

        public void ShowBudgetItemByMonth(List<BudgetItemsByMonth> items)
        {
            calledShowBudgetItemByMonth = true;
        }

        public void ShowBudgetItemByCategory(List<BudgetItemsByCategory> items)
        {
            calledShowBudgetItemByCategory = true;
        }

        public void CalledRefresh()
        {
            calledcalledRefresh = true;
        }

        public void Delete_Expense_InUpdate()
        {
            calleddeleteAdterUpdate = true;
        }

        public void Refresh_MonthCategoryExpenses()
        {
            throw new NotImplementedException();
        }

        public void Refresh_MonthExpenses()
        {
            throw new NotImplementedException();
        }

        public void Refresh_CategoryExpenses()
        {
            throw new NotImplementedException();
        }
    }

    public class UnitTest1
    {

        [Fact]
        public void TestConstructor()
        {
            //Arrange
            string dummyFile = "./dummyFile.db";
            bool newDb = false;
            TestView view = new TestView();

            //Act
            Presenter p = new Presenter(view, dummyFile, newDb);

            //Assert
            Assert.IsType<Presenter>(p);
        }

        [Fact]
        public void Test_AddingExpense_Success()
        {
            //Arrange
            string dummyFile = "./dummyFile.db";
            bool newDb = false;
            TestView view = new TestView();
            Presenter p = new Presenter(view, dummyFile, newDb);        
            DateTime dateNow = DateTime.Now;
            string amount = "50";
            int categoryId = 1;
            string desc = "a Hat";

            //Act
            p.AddExpense(dateNow, amount, categoryId, desc);

            //Assert
            Assert.True(view.calledShowValid);
            Assert.True(view.calledClearExpense);
        }

        [Fact]
        public void Test_AddingExpense_Success_DescriptionWithNumber()
        {
            //Arrange
            string dummyFile = "./dummyFile.db";
            bool newDb = false;
            TestView view = new TestView();
            Presenter p = new Presenter(view, dummyFile, newDb);
            DateTime dateNow = DateTime.Now;
            string amount = "50";
            int categoryId = 1;
            string desc = "a Hat2";

            //Act
            p.AddExpense(dateNow, amount, categoryId, desc);

            //Assert
            Assert.True(view.calledShowValid);
            Assert.True(view.calledClearExpense);
        }

        [Fact]
        public void Test_AddingExpense_noSelectedCategory()
        {
            //Arrange
            string dummyFile = "./dummyFile.db";
            bool newDb = false;
            TestView view = new TestView();
            Presenter p = new Presenter(view, dummyFile, newDb);
            DateTime dateNow = DateTime.Now;
            string amount = "50";
            int categoryId = -1; //---------------------------------------------------------------------------ask
            string desc = "a Hat";

            //Act
            p.AddExpense(dateNow, amount, categoryId, desc);

            //Assert
            Assert.True(view.calledShowError);
        }

        [Fact]
        public void Test_AddingExpense_InvalidAmount_notNumber()
        {
            //Arrange
            string dummyFile = "./dummyFile.db";
            bool newDb = false;
            TestView view = new TestView();
            Presenter p = new Presenter(view, dummyFile, newDb);
            DateTime dateNow = DateTime.Now;
            string amount = "notNumber";
            int categoryId = 1;
            string desc = "a Hat";

            //Act
            p.AddExpense(dateNow, amount, categoryId, desc);

            //Assert
            Assert.True(view.calledShowError);
        }

        [Fact]
        public void Test_AddingExpense_InvalidAmount_isInfinity()
        {
            //Arrange
            string dummyFile = "./dummyFile.db";
            bool newDb = false;
            TestView view = new TestView();
            Presenter p = new Presenter(view, dummyFile, newDb);
            DateTime dateNow = DateTime.Now;
            string amount = "10 / 0.0 ";
            int categoryId = 1;
            string desc = "a Hat";

            //Act
            p.AddExpense(dateNow, amount, categoryId, desc);

            //Assert
            Assert.True(view.calledShowError);
        }

        [Fact]
        public void Test_AddingExpense_InvalidAmount_isNaN()
        {
            //Arrange
            string dummyFile = "./dummyFile.db";
            bool newDb = false;
            TestView view = new TestView();
            Presenter p = new Presenter(view, dummyFile, newDb);
            DateTime dateNow = DateTime.Now;
            string amount = "0.0 / 0.0";
            int categoryId = 1;
            string desc = "a Hat";

            //Act
            p.AddExpense(dateNow, amount, categoryId, desc);

            //Assert
            Assert.True(view.calledShowError);
        }

        [Fact]
        public void Test_AddingExpense_InvalidDescription_IsEmpty()
        {
            //Arrange
            string dummyFile = "./dummyFile.db";
            bool newDb = false;
            TestView view = new TestView();
            Presenter p = new Presenter(view, dummyFile, newDb);
            DateTime dateNow = DateTime.Now;
            string amount = "1";
            int categoryId = 1;
            string desc = "";

            //Act
            p.AddExpense(dateNow, amount, categoryId, desc);

            //Assert
            Assert.True(view.calledShowError);
        }

        [Fact]
        public void Test_AddingExpense_InvalidDescription_IsNumberOnly()
        {
            //Arrange
            string dummyFile = "./dummyFile.db";
            bool newDb = false;
            TestView view = new TestView();
            Presenter p = new Presenter(view, dummyFile, newDb);
            DateTime dateNow = DateTime.Now;
            string amount = "1";
            int categoryId = 1;
            string desc = "1";

            //Act
            p.AddExpense(dateNow, amount, categoryId, desc);

            //Assert
            Assert.True(view.calledShowError);
        }

        [Fact]
        public void Test_AddingCategory_Success()
        {
            //Arrange
            string dummyFile = "./dummyFile.db";
            bool newDb = false;
            TestView view = new TestView();
            Presenter p = new Presenter(view, dummyFile, newDb);
            int categoryType = 1;
            string desc = "a Hat";

            //Act
            p.AddCategory(desc, categoryType);

            //Assert
            Assert.True(view.calledShowValid);
        }

        [Fact]
        public void Test_AddingCategory_InvalidDescription_IsEmpty()
        {
            //Arrange
            string dummyFile = "./dummyFile.db";
            bool newDb = false;
            TestView view = new TestView();
            Presenter p = new Presenter(view, dummyFile, newDb);
            int categoryType = 1;
            string desc = "";

            //Act
            p.AddCategory(desc, categoryType);

            //Assert
            Assert.True(view.calledShowError);
        }

        [Fact]
        public void Test_AddingCategory_InvalidDescription_HaveNumbers()
        {
            //Arrange
            string dummyFile = "./dummyFile.db";
            bool newDb = false;
            TestView view = new TestView();
            Presenter p = new Presenter(view, dummyFile, newDb);
            int categoryType = 1;
            string desc = "A1";

            //Act
            p.AddCategory(desc, categoryType);

            //Assert
            Assert.True(view.calledShowError);
        }

        [Fact]
        public void Test_AddingCategory_InvalidDescription_isNumbers()
        {
            //Arrange
            string dummyFile = "./dummyFile.db";
            bool newDb = false;
            TestView view = new TestView();
            Presenter p = new Presenter(view, dummyFile, newDb);
            int categoryType = 1;
            string desc = "1";

            //Act
            p.AddCategory(desc, categoryType);

            //Assert
            Assert.True(view.calledShowError);
        }

        [Fact]
        public void Test_AddingCategory_InvalidDescription_NotSelectedCategory()
        {
            //Arrange
            string dummyFile = "./dummyFile.db";
            bool newDb = false;
            TestView view = new TestView();
            Presenter p = new Presenter(view, dummyFile, newDb);
            int categoryType = -1;
            string desc = "Hat";

            //Act
            p.AddCategory(desc, categoryType);

            //Assert
            Assert.True(view.calledShowError);
        }

        [Fact]
        public void Test_UpdateExpense_Success()
        {
            //Arrange
            string dummyFile = "./dummyFile.db";
            bool newDb = false;
            TestView view = new TestView();
            Presenter p = new Presenter(view, dummyFile, newDb);
            int id = 1;
            DateTime dateNow = DateTime.Now;
            string amount = "50";
            int categoryId = 1;
            string desc = "a Hat";

            //Act
            p.UpdateExpense(id, dateNow, categoryId, amount, desc);

            //Assert
            Assert.True(view.calledShowValid);
            Assert.True(view.calledclosingAfterUpdate);
            Assert.True(view.calledRefresh_allExpenses);
        }

        [Fact]
        public void Test_UpdateExpense_Success_DescriptionWithNumber()
        {
            //Arrange
            string dummyFile = "./dummyFile.db";
            bool newDb = false;
            TestView view = new TestView();
            Presenter p = new Presenter(view, dummyFile, newDb);
            int id = 1;
            DateTime dateNow = DateTime.Now;
            string amount = "50";
            int categoryId = 1;
            string desc = "a Hat2";

            //Act
            p.UpdateExpense(id, dateNow, categoryId, amount, desc);

            //Assert
            Assert.True(view.calledShowValid);
            Assert.True(view.calledclosingAfterUpdate);
            Assert.True(view.calledRefresh_allExpenses);
        }

        [Fact]
        public void Test_UpdateExpense_InvalidAmount_notNumber()
        {
            //Arrange
            string dummyFile = "./dummyFile.db";
            bool newDb = false;
            TestView view = new TestView();
            Presenter p = new Presenter(view, dummyFile, newDb);
            int id = 1;
            DateTime dateNow = DateTime.Now;
            string amount = "Not a Number";
            int categoryId = 1;
            string desc = "a Hat2";

            //Act
            p.UpdateExpense(id, dateNow, categoryId, amount, desc);

            //Assert
            Assert.True(view.calledShowError);
        }

        [Fact]
        public void Test_UpdateExpense_InvalidAmount_isInfinity()
        {
            //Arrange
            string dummyFile = "./dummyFile.db";
            bool newDb = false;
            TestView view = new TestView();
            Presenter p = new Presenter(view, dummyFile, newDb);
            DateTime dateNow = DateTime.Now;
            string amount = "10 / 0.0 ";
            int categoryId = 1;
            string desc = "a Hat";
            int id = 1;

            //Act
            p.UpdateExpense(id, dateNow, categoryId, amount, desc);

            //Assert
            Assert.True(view.calledShowError);
        }

        [Fact]
        public void Test_UpdateExpense_InvalidAmount_isNaN()
        {
            //Arrange
            string dummyFile = "./dummyFile.db";
            bool newDb = false;
            TestView view = new TestView();
            Presenter p = new Presenter(view, dummyFile, newDb);
            DateTime dateNow = DateTime.Now;
            string amount = "0.0 / 0.0";
            int categoryId = 1;
            string desc = "a Hat";
            int id = 1;

            //Act
            p.UpdateExpense(id, dateNow, categoryId, amount, desc);

            //Assert
            Assert.True(view.calledShowError);
        }

        [Fact]
        public void Test_UpdategExpense_InvalidDescription_IsEmpty()
        {
            //Arrange
            string dummyFile = "./dummyFile.db";
            bool newDb = false;
            TestView view = new TestView();
            Presenter p = new Presenter(view, dummyFile, newDb);
            DateTime dateNow = DateTime.Now;
            string amount = "1";
            int categoryId = 1;
            string desc = "";
            int id = 1;

            //Act
            p.UpdateExpense(id, dateNow, categoryId, amount, desc);

            //Assert
            Assert.True(view.calledShowError);
        }

        [Fact]
        public void Test_UpdateExpense_InvalidDescription_IsNumberOnly()
        {
            //Arrange
            string dummyFile = "./dummyFile.db";
            bool newDb = false;
            TestView view = new TestView();
            Presenter p = new Presenter(view, dummyFile, newDb);
            DateTime dateNow = DateTime.Now;
            string amount = "1";
            int categoryId = 1;
            string desc = "1";
            int id = 1;

            //Act
            p.UpdateExpense(id, dateNow, categoryId, amount, desc);

            //Assert
            Assert.True(view.calledShowError);
        }

        [Fact]
        public void Test_GetAllBudgetItem_Success()
        {
            //Arrange
            string dummyFile = "./dummyFile.db";
            bool newDb = false;
            TestView view = new TestView();
            Presenter p = new Presenter(view, dummyFile, newDb);
            DateTime dateNow = DateTime.Now;
            DateTime dateThen = DateTime.Now;
            bool filter = true;
            int categoryId = 1;
            //Act
            p.GetAllBudgetItem(dateNow, dateThen, filter, categoryId);

            //Assert
            Assert.True(view.calledGetBudgetItem);
        }

        [Fact]
        public void Test_GetAllBudgetItemByCategoryAndByMonth_Success()
        {
            //Arrange
            string dummyFile = "./dummyFile.db";
            bool newDb = false;
            TestView view = new TestView();
            Presenter p = new Presenter(view, dummyFile, newDb);
            DateTime dateNow = DateTime.Now;
            DateTime dateThen = DateTime.Now;
            bool filter = true;
            int categoryId = 1;
            //Act
            p.GetAllBudgetItemByCategoryAndByMonth(dateNow, dateThen, filter, categoryId);

            //Assert
            Assert.True(view.calledShowBudgetItemByMonthAndCategory);
        }

        [Fact]
        public void Test_GetAllBudgetItemByMonth_Success()
        {
            //Arrange
            string dummyFile = "./dummyFile.db";
            bool newDb = false;
            TestView view = new TestView();
            Presenter p = new Presenter(view, dummyFile, newDb);
            DateTime dateNow = DateTime.Now;
            DateTime dateThen = DateTime.Now;
            bool filter = true;
            int categoryId = 1;
            //Act
            p.GetAllBudgetItemByMonth(dateNow, dateThen, filter, categoryId);

            //Assert
            Assert.True(view.calledShowBudgetItemByMonth);
        }

        [Fact]
        public void Test_GetAllBudgetItemByCategory_Success()
        {
            //Arrange
            string dummyFile = "./dummyFile.db";
            bool newDb = false;
            TestView view = new TestView();
            Presenter p = new Presenter(view, dummyFile, newDb);
            DateTime dateNow = DateTime.Now;
            DateTime dateThen = DateTime.Now;
            bool filter = true;
            int categoryId = 1;
            //Act
            p.GetAllBudgetItemByCategory(dateNow, dateThen, filter, categoryId);

            //Assert
            Assert.True(view.calledShowBudgetItemByCategory);
        }

        [Fact]
        public void Test_DeleteExpense_Success()
        {
            //Arrange
            string dummyFile = "./dummyFile.db";
            bool newDb = false;
            TestView view = new TestView();
            Presenter p = new Presenter(view, dummyFile, newDb);
            int id = 1;

            //Act
            p.Delete_Expense(id);

            //Assert
           
            Assert.True(view.calledRefresh_allExpenses);
        }

        [Fact]
        public void Test_DeleteExpenseAfterUpdate_Success()
        {
            //Arrange
            string dummyFile = "./dummyFile.db";
            bool newDb = false;
            TestView view = new TestView();
            Presenter p = new Presenter(view, dummyFile, newDb);
            int id = 1;

            //Act
            p.Delete_Expense_InUpdate(id);

            //Assert

            Assert.True(view.calledclosingAfterUpdate);
            Assert.True(view.calledRefresh_allExpenses);
        }

        [Fact]
        public void Test_DeleteExpense_InvalidId()
        {
            //Arrange
            string dummyFile = "./dummyFile.db";
            bool newDb = false;
            TestView view = new TestView();
            Presenter p = new Presenter(view, dummyFile, newDb);
            int id = -1;

            //Act
            p.Delete_Expense(id);

            //Assert
            Assert.True(view.calledShowError);
        }

        public void Test_RefreshWithNewExpense_Success()
        {
            //Arrange
            string dummyFile = "./dummyFile.db";
            bool newDb = false;
            TestView view = new TestView();
            Presenter p = new Presenter(view, dummyFile, newDb);

            //Act
            p.RefreshWithNewExpense();

            //Assert
            Assert.True(view.calledcalledRefresh);
        }


    }
}