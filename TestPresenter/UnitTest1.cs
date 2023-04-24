using JoeWpfHomeBudget;

namespace TestPresenter
{

    public class TestView : ViewInterface
    {
        public bool calledShowError;
        public bool calledShowValid;
        public bool calledClearExpense;
        public bool calledCancelExpense;

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

    }
}