// ============================================================================
// (c) Sandy Bultena 2018
// * Released under the GNU General Public License
// ============================================================================

using System.Data.SQLite;
using static Budget.Category;

namespace Budget
{
    // ====================================================================
    // CLASS: HomeBudget
    //        - Combines categories Class and expenses Class
    //        - One File defines Category and Budget File
    //        - etc
    // ====================================================================
    /// <summary>
    /// An app where you can manage your money. This help you create
    /// a report of all your different expenses and what type of category this expense are
    /// and will put all of these into a file,also it can take as input a files.
    /// <br/>
    /// <br/>
    /// Used Classes:
    /// <br/>
    /// <see cref="Categories"/>
    /// <see cref="Expenses"/>
    /// </summary>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// HomeBudget homeBudget = new HomeBudget("./test.budget");
    /// List<BudgetItem> list1=homeBudget.GetBudgetItems(null, null, false, 0);
    /// List<BudgetItemsByMonth> list2=homeBudget.GetBudgetItemsByMonth(null, null, false, 0);
    /// List<BudgetItemsByCategory> list3=homeBudget.GeBudgetItemsByCategory(null, null, false, 0);
    /// List<Dictionary<string, object>> list4=homeBudget.GetBudgetDictionaryByCategoryAndMonth(null, null, false, 0);
    /// ]]>
    /// </code>
    /// </example>
    public class HomeBudget
    {
        private string _FileName;
        private string _DirName;
        private Categories _categories;
        private Expenses _expenses;

        // ====================================================================
        // Properties
        // ===================================================================

        // Properties (location of files etc)
        /// <value>
        /// The file name where you want to read your past expenses.
        /// </value>
        public String FileName { get { return _FileName; } }
        /// <value>
        /// the directory name of the file you want to read your expenses from.
        /// </value>
        public String DirName { get { return _DirName; } }
        /// <value>
        /// the path of the file you want to read your expenses from.
        /// </value>
        public String PathName
        {
            get
            {
                if (_FileName != null && _DirName != null)
                {
                    return Path.GetFullPath(_DirName + "\\" + _FileName);
                }
                else
                {
                    return null;
                }
            }
        }

        // Properties (categories and expenses object)
        /// <value>
        /// A list of category that you have used in the budget.
        /// </value>
        public Categories categories { get { return _categories; } }
        /// <value>
        /// A list of your different expenses in the budget.
        /// </value>
        public Expenses expenses { get { return _expenses; } }

        // -------------------------------------------------------------------
        // Constructor (new... default categories, no expenses)
        // -------------------------------------------------------------------
        /// <summary>
        /// Create a new list of categories and a new list of expenses.
        /// </summary>
        public HomeBudget()
        {
            _categories = new Categories();
            _expenses = new Expenses();
        }

        // -------------------------------------------------------------------
        // Constructor (existing budget ... must specify file)
        // -------------------------------------------------------------------
        /// <summary>
        /// Create a new list of categories and a new list of expenses and will populate them 
        /// with the file you gave it.
        /// </summary>
        /// <param name="budgetFileName">The file with all your categories and expenses.</param>
        public HomeBudget(String budgetFileName)
        {
            _categories = new Categories();
            _expenses = new Expenses();
            ReadFromFile(budgetFileName);
        }


        public HomeBudget(String databaseFile, String expensesXMLFile, bool newDB = false)
        {
            // if database exists, and user doesn't want a new database, open existing DB
            if (!newDB && File.Exists(databaseFile))
            {
                Database.existingDatabase(databaseFile);
            }

            // file did not exist, or user wants a new database, so open NEW DB
            else
            {
                Database.newDatabase(databaseFile);
                newDB = true;
            }

            // create the category object
            _categories = new Categories(Database.dbConnection, newDB);

            // create the _expenses course

            DBCategoryType(Database.dbConnection);
            _categories = new Categories();
            _expenses = new Expenses();

            //_expenses.ReadFromFile(expensesXMLFile);
        }

        private void DBCategoryType(SQLiteConnection db)
        {

            using var cmd = new SQLiteCommand(db);

            cmd.CommandText = "INSERT INTO categoryTypes(Id, Description) VALUES(0, 'Income')";
            cmd.ExecuteNonQuery();

            cmd.CommandText = "INSERT INTO categoryTypes(Id, Description) VALUES(1, 'Expense')";
            cmd.ExecuteNonQuery();

            cmd.CommandText = "INSERT INTO categoryTypes(Id, Description) VALUES(2, 'Credit')";
            cmd.ExecuteNonQuery();

            cmd.CommandText = "INSERT INTO categoryTypes(Id, Description) VALUES(3, 'Savings')";
            cmd.ExecuteNonQuery();

        }
        #region OpenNewAndSave
        // ---------------------------------------------------------------
        // Read
        // Throws Exception if any problem reading this file
        /// <summary>
        /// Read all of the information from your file and put all you expenses and category into a list.
        /// </summary>
        /// <example>
        ///For exemple the file contains:
        ///<code>
        ///test_categories.cats
        ///test_expenses.exps
        ///</code>
        ///those two are the files of expenses and categories for this budget.
        ///
        /// <b>This will populate the categories and expenses lists.</b>
        /// <code>
        /// HomeBudget budget = new HomeBudget();
        /// budget.ReadFromFile("./test.budget");
        /// </code>
        /// </example>
        /// <param name="budgetFileName">The name of the file of your budget.</param>
        /// <exception cref="Exception">if the file given doesn't exist or that app can't read from it.</exception>
        // ---------------------------------------------------------------
        public void ReadFromFile(String budgetFileName)
        {
            // ---------------------------------------------------------------
            // read the budget file and process
            // ---------------------------------------------------------------
            try
            {
                // get filepath name (throws exception if it doesn't exist)
                budgetFileName = BudgetFiles.VerifyReadFromFileName(budgetFileName, "");

                // If file exists, read it
                string[] filenames = System.IO.File.ReadAllLines(budgetFileName);

                // ----------------------------------------------------------------
                // Save information about budget file
                // ----------------------------------------------------------------
                string folder = Path.GetDirectoryName(budgetFileName);
                _FileName = Path.GetFileName(budgetFileName);

                // read the expenses and categories from their respective files
                _categories.ReadFromFile(folder + "\\" + filenames[0]);
                _expenses.ReadFromFile(folder + "\\" + filenames[1]);
                                             

                // Save information about budget file
                _DirName = Path.GetDirectoryName(budgetFileName);
                _FileName = Path.GetFileName(budgetFileName);

            }

            // ----------------------------------------------------------------
            // throw new exception if we cannot get the info that we need
            // ----------------------------------------------------------------
            catch (Exception e)
            {
                throw new Exception("Could not read budget info: \n" + e.Message);
            }

        }

        // ====================================================================
        // save to a file
        // saves the following files:
        //  filepath_expenses.exps  # expenses file
        //  filepath_categories.cats # categories files
        //  filepath # a file containing the names of the expenses and categories files.
        //  Throws exception if we cannot write to that file (ex: invalid dir, wrong permissions)
        /// <summary>
        /// Save to a file all your expenses and categories, and will also save sub files of just expenses and just categories.
        /// </summary>
        /// <example>
        /// You need to give in a file name:
        /// <code>
        /// HomeBudget budget= new HomeBudget();
        /// budget.ReadFromFile("./test.budget");// this will populate the given lists
        /// budget.SaveToFile("./saveFile.budget"); //this actually saves it
        /// </code>
        /// <b>then the list of categories and expenses will go into 3 different files:</b>
        /// 
        /// the first file will be the name of the two files that this method outputed
        /// <code>
        /// output_categories.cats
        /// output_expenses.exps
        /// </code>
        /// the second file will the list of categories
        /// <code>
        /// <![CDATA[
        /// <Categories>
        ///<Category ID = "17" type="Expense">Non Standard</Category>
        ///<Category ID = "1" type= "Expense" > Utilities </ Category >
        ///< Category ID= "2" type= "Expense" > Rent </ Category >
        ///< Category ID= "3" type= "Expense" > Food </ Category >
        ///< Category ID= "4" type= "Expense" > Entertainment </ Category >
        ///< Category ID= "5" type= "Expense" > Education </ Category >
        ///< Category ID= "6" type= "Expense" > Miscellaneous </ Category >
        ///< Category ID= "7" type= "Expense" > Medical Expenses</Category>
        ///<Category ID = "8" type= "Expense" > Vacation </ Category >
        ///< Category ID= "9" type= "Credit" > Credit Card</Category>
        ///<Category ID = "10" type= "Expense" > Clothes </ Category >
        ///< Category ID= "11" type= "Expense" > Gifts </ Category >
        ///< Category ID= "12" type= "Expense" > Insurance </ Category >
        ///< Category ID= "13" type= "Expense" > Transportation </ Category >
        ///< Category ID= "14" type= "Expense" > Eating Out</Category>
        ///<Category ID = "15" type= "Expense" > Savings </ Category >
        ///< Category ID= "16" type= "Income" > Income </ Category >
        ///</ Categories >
        /// ]]>
        /// </code>
        /// 
        /// the third file will be the list of expenses:
        /// <code>
        /// <![CDATA[
        ///        <Description>Hat</Description>
        ///<Amount>-25</Amount>
        ///<Category>9</Category>
        ///  </Expense>
        ///  <Expense ID = "13" >
        ///    < Date > 2 / 10 / 2020 12:00:00 AM</Date>
        ///    <Description>mittens</Description>
        ///    <Amount>-15</Amount>
        ///    <Category>9</Category>
        ///  </Expense>
        ///  <Expense ID = "7" >
        ///    < Date > 1 / 12 / 2020 12:00:00 AM</Date>
        ///    <Description>Wendys</Description>
        ///    <Amount>25</Amount>
        ///    <Category>14</Category>
        ///  </Expense>
        ///    </Expenses>
        /// ]]>
        /// </code>
        /// </example>
        /// <param name="filepath">The file path of the file that you want the app to read from.</param>
        // ====================================================================
        public void SaveToFile(String filepath)
        {

            // ---------------------------------------------------------------
            // just in case filepath doesn't exist, reset path info
            // ---------------------------------------------------------------
            _DirName = null;
            _FileName = null;

            // ---------------------------------------------------------------
            // get filepath name (throws exception if we can't write to the file)
            // ---------------------------------------------------------------
            filepath = BudgetFiles.VerifyWriteToFileName(filepath, "");

            String path = Path.GetDirectoryName(Path.GetFullPath(filepath));
            String file = Path.GetFileNameWithoutExtension(filepath);
            String ext = Path.GetExtension(filepath);

            // ---------------------------------------------------------------
            // construct file names for expenses and categories
            // ---------------------------------------------------------------
            String expensepath = path + "\\" + file + "_expenses" + ".exps";
            String categorypath = path + "\\" + file + "_categories" + ".cats";

            // ---------------------------------------------------------------
            // save the expenses and categories into their own files
            // ---------------------------------------------------------------
            _expenses.SaveToFile(expensepath);
            _categories.SaveToFile(categorypath);

            // ---------------------------------------------------------------
            // save filenames of expenses and categories to budget file
            // ---------------------------------------------------------------
            string[] files = { Path.GetFileName(categorypath), Path.GetFileName(expensepath) };
            System.IO.File.WriteAllLines(filepath, files);

            // ----------------------------------------------------------------
            // save filename info for later use
            // ----------------------------------------------------------------
            _DirName = path;
            _FileName = Path.GetFileName(filepath);
        }
        #endregion OpenNewAndSave

        #region GetList



        // ============================================================================
        // Get all expenses list
        // NOTE: VERY IMPORTANT... budget amount is the negative of the expense amount
        // Reasoning: an expense of $15 is -$15 from your bank account.
        /// <summary>
        /// Get a list of all your Items, depending on your criteria.
        /// </summary>
        /// <param name="Start">The From what date do tou want to see.</param>
        /// <param name="End">The To what date do you want to see.</param>
        /// <param name="FilterFlag">This is tell that you want to have special filter on the categories you want to show.</param>
        /// <param name="CategoryID">The category you want appear in the output.</param>
        /// <returns>The list of budget items depending on your criteria. </returns>
        ///<example>
        ///The lists are being outputed are always sorted
        ///Also you can have the balance of your account: the balance is how much is in your account( if it's minus you own money)
        /// For all examples below, assume the budget file contains the
        /// following elements:
        /// 
        /// <code>
        /// Cat_ID  Expense_ID  Date                    Description                    Cost
        ///    10       1       1/10/2018 12:00:00 AM   Clothes hat (on credit)         10
        ///     9       2       1/11/2018 12:00:00 AM   Credit Card hat                -10
        ///    10       3       1/10/2019 12:00:00 AM   Clothes scarf(on credit)        15
        ///     9       4       1/10/2020 12:00:00 AM   Credit Card scarf              -15
        ///    14       5       1/11/2020 12:00:00 AM   Eating Out McDonalds            45
        ///    14       7       1/12/2020 12:00:00 AM   Eating Out Wendys               25
        ///    14      10       2/1/2020 12:00:00 AM    Eating Out Pizza                33.33
        ///     9      13       2/10/2020 12:00:00 AM   Credit Card mittens            -15
        ///     9      12       2/25/2020 12:00:00 AM   Credit Card Hat                -25
        ///    14      11       2/27/2020 12:00:00 AM   Eating Out Pizza                33.33
        ///    14       9       7/11/2020 12:00:00 AM   Eating Out Cafeteria            11.11
        /// </code>
        /// 
        /// <b>Getting a list of ALL budget items.</b>
        /// 
        /// <code>
        /// <![CDATA[
        ///  HomeBudget budget = new HomeBudget();
        ///  budget.ReadFromFile(filename);
        ///  
        ///  // Get a list of all budget items
        ///  var budgetItems = budget.GetBudgetItems(null, null, false, 0);
        ///            
        ///  // print important information
        ///  foreach (var bi in budgetItems)
        ///  {
        ///      Console.WriteLine ( 
        ///          String.Format("{0} {1,-20}  {2,8:C} {3,12:C}", 
        ///             bi.Date.ToString("yyyy/MMM/dd"),
        ///             bi.ShortDescription,
        ///             bi.Amount, bi.Balance)
        ///       );
        ///  }
        ///
        /// ]]>
        /// </code>
        /// 
        /// Sample output:
        /// <code>
        /// 2018/Jan/10 hat (on credit)       ($10.00)     ($10.00)
        /// 2018/Jan/11 hat                     $10.00        $0.00
        /// 2019/Jan/10 scarf(on credit)      ($15.00)     ($15.00)
        /// 2020/Jan/10 scarf                   $15.00        $0.00
        /// 2020/Jan/11 McDonalds             ($45.00)     ($45.00)
        /// 2020/Jan/12 Wendys                ($25.00)     ($70.00)
        /// 2020/Feb/01 Pizza                 ($33.33)    ($103.33)
        /// 2020/Feb/10 mittens                 $15.00     ($88.33)
        /// 2020/Feb/25 Hat                     $25.00     ($63.33)
        /// 2020/Feb/27 Pizza                 ($33.33)     ($96.66)
        /// 2020/Jul/11 Cafeteria             ($11.11)    ($107.77)
        /// </code>
        /// 
        /// <b>Getting a list of the budgets items that have a filter and that only want the category 14: </b>
        /// <code>
        /// <![CDATA[
        ///  HomeBudget budget = new HomeBudget();
        ///  budget.ReadFromFile(filename);
        ///  
        ///  // Get a list of all budget items
        ///  var budgetItems = budget.GetBudgetItems(null, null, true, 14);
        ///            
        ///  // print important information
        ///  foreach (var bi in budgetItems)
        ///  {
        ///      Console.WriteLine ( 
        ///          String.Format("{0} {1,-20}  {2,8:C} {3,12:C}", 
        ///             bi.Date.ToString("yyyy/MMM/dd"),
        ///             bi.ShortDescription,
        ///             bi.Amount, bi.Balance)
        ///       );
        ///  }
        /// ]]>
        /// </code>
        /// 
        /// Sample output:
        /// <code>
        /// 2020/Jan/11 McDonalds             ($45.00)     ($45.00)
        /// 2020/Jan/12 Wendys                ($25.00)     ($70.00)
        /// 2020/Feb/27 Pizza                 ($33.33)     ($96.66)
        /// 2020/Jul/11 Cafeteria             ($11.11)    ($107.77)
        /// </code>
        /// 
        /// <b>Getting a list of the budget items that from january 1st 2020 to  july 20th 2020, when we only want to see the category 9</b>
        /// <code>
        /// <![CDATA[
        ///  HomeBudget budget = new HomeBudget();
        ///  budget.ReadFromFile(filename);
        ///  DateTime start=DateTime(2020,1,1); //the date range is inclusive
        ///  DateTIme end=DateTime(2020, 7,20);
        ///  
        ///  // Get a list of all budget items
        ///  var budgetItems = budget.GetBudgetItems(start, end, true, 9);
        ///            
        ///  // print important information
        ///  foreach (var bi in budgetItems)
        ///  {
        ///      Console.WriteLine ( 
        ///          String.Format("{0} {1,-20}  {2,8:C} {3,12:C}", 
        ///             bi.Date.ToString("yyyy/MMM/dd"),
        ///             bi.ShortDescription,
        ///             bi.Amount, bi.Balance)
        ///       );
        ///  }
        /// ]]>
        /// </code>
        /// Sample output:
        /// <code>
        /// 2020/Jan/10 scarf                   $15.00        $0.00
        /// 2020/Feb/10 mittens                 $15.00     ($88.33)
        /// 2020/Feb/25 Hat                     $25.00     ($63.33)
        /// </code>
        /// 
        /// </example>

        // ============================================================================
        public List<BudgetItem> GetBudgetItems(DateTime? Start, DateTime? End, bool FilterFlag, int CategoryID)
        {
            // ------------------------------------------------------------------------
            // return joined list within time frame
            // ------------------------------------------------------------------------
            Start = Start ?? new DateTime(1900, 1, 1);
            End = End ?? new DateTime(2500, 1, 1);

            var query = from c in _categories.List()
                        join e in _expenses.List() on c.Id equals e.Category
                        where e.Date >= Start && e.Date <= End
                        select new { CatId = c.Id, ExpId = e.Id, e.Date, Category = c.Description, e.Description, e.Amount };

            // ------------------------------------------------------------------------
            // create a BudgetItem list with totals,
            // ------------------------------------------------------------------------
            List<BudgetItem> items = new List<BudgetItem>();
            Double total = 0;

            foreach (var queryResult in query.OrderBy(q => q.Date))
            {
                // filter out unwanted categories if filter flag is on
                if (FilterFlag && CategoryID != queryResult.CatId)
                {
                    continue;
                }

                // keep track of running totals
                total = total - queryResult.Amount;
                items.Add(new BudgetItem
                {
                    CategoryID = queryResult.CatId,
                    ExpenseID = queryResult.ExpId,
                    ShortDescription = queryResult.Description,
                    Date = queryResult.Date,
                    Amount = -queryResult.Amount,
                    Category = queryResult.Category,
                    Balance = total
                });
            }

            return items;
        }

        // ============================================================================
        // Group all expenses month by month (sorted by year/month)
        // returns a list of BudgetItemsByMonth which is 
        // "year/month", list of budget items, and total for that month
        /// <summary>
        /// Create a list of items depending on you criteria, by months, so for each month/year a list will be outputed.
        /// </summary>
        /// <param name="Start">The date start of the list</param>
        /// <param name="End">The date end of the list</param>
        /// <param name="FilterFlag">This is tell that you want to have special filter on the categories you want to show.</param>
        /// <param name="CategoryID">The category you  want appear in the list.</param>
        /// <returns>The list of of all the months and in each month the list of all the items.</returns>
        ///<example>
        ///The lists are being outputed are always sorted
        ///Also you can have the balance of your account: the balance is how much is in your account( if it's minus you own money)
        /// For all examples below, assume the budget file contains the
        /// following elements:
        /// 
        /// <code>
        /// Cat_ID  Expense_ID  Date                    Description                    Cost
        ///    10       1       1/10/2018 12:00:00 AM   Clothes hat (on credit)         10
        ///     9       2       1/11/2018 12:00:00 AM   Credit Card hat                -10
        ///    10       3       1/10/2019 12:00:00 AM   Clothes scarf(on credit)        15
        ///     9       4       1/10/2020 12:00:00 AM   Credit Card scarf              -15
        ///    14       5       1/11/2020 12:00:00 AM   Eating Out McDonalds            45
        ///    14       7       1/12/2020 12:00:00 AM   Eating Out Wendys               25
        ///    14      10       2/1/2020 12:00:00 AM    Eating Out Pizza                33.33
        ///     9      13       2/10/2020 12:00:00 AM   Credit Card mittens            -15
        ///     9      12       2/25/2020 12:00:00 AM   Credit Card Hat                -25
        ///    14      11       2/27/2020 12:00:00 AM   Eating Out Pizza                33.33
        ///    14       9       7/11/2020 12:00:00 AM   Eating Out Cafeteria            11.11
        /// </code>
        /// 
        /// <b>Getting a list of ALL budget items By Month.</b>
        /// 
        /// <code>
        /// <![CDATA[
        ///  HomeBudget budget = new HomeBudget();
        ///  budget.ReadFromFile(filename);
        ///  
        ///  // Get a list of all budget items By Month
        ///  var budgetItems = budget.GetBudgetItemsByMonth(null, null, false, 0);
        ///            
        ///  // print important information
        ///  foreach (BudgetItemsByMonth item in budgetItems)
        ///  {
        ///      Console.WriteLine ( 
        ///          String.Format("{0} {1,-20}", 
        ///             item.Month,
        ///             item.Total));
        ///     
        ///      foreach((BudgetItem temp in item.Details)
        ///      {
        ///         Console.Write(
        ///             String.Format("{2,8:C} {3,12:C} {4,25C} {5,35C}"),
        ///             temp.Category,temp.Description, temp.cost, temp.balance;
        ///      
        ///      }
        ///  }
        /// ]]>
        /// </code>
        /// 
        /// Sample output:
        /// <code>
        /// 2018/01 0 hat (on credit)       ($10.00)     ($10.00)
        /// 2018/01 -15 hat                     $10.00        $0.00
        /// 2019/01 -15 scarf(on credit)      ($15.00)     ($15.00)
        /// 2020/01 -55 scarf                   $15.00        $0.00
        /// 2020/01 -55 McDonalds             ($45.00)     ($45.00)
        /// 2020/01 -55 Wendys                ($25.00)     ($70.00)
        /// 2020/02 -26.659999999999997 Pizza                 ($33.33)    ($103.33)
        /// 2020/02 -26.659999999999997 mittens                 $15.00     ($88.33)
        /// 2020/02 -26.659999999999997 Hat                     $25.00     ($63.33)
        /// 2020/02 -26.659999999999997 Pizza                 ($33.33)     ($96.66)
        /// 2020/07 -11.11 Cafeteria             ($11.11)    ($107.77)
        /// </code>
        /// 
        /// <b>Getting a list of the budgets items by months that have a filter and that only want the category 14: </b>
        ///<code>
        /// <![CDATA[
        ///  HomeBudget budget = new HomeBudget();
        ///  budget.ReadFromFile(filename);
        ///  
        ///  // Get a list of all budget items By Month
        ///  var budgetItems = budget.GetBudgetItemsByMonth(null, null, true, 14);
        ///            
        ///  // print important information
        ///  foreach (BudgetItemsByMonth item in budgetItems)
        ///  {
        ///      Console.WriteLine ( 
        ///          String.Format("{0} {1,-20}", 
        ///             item.Month,
        ///             item.Total));
        ///     
        ///      foreach((BudgetItem temp in item.Details)
        ///      {
        ///         Console.Write(
        ///             String.Format("{2,8:C} {3,12:C} {4,25C} {5,35C}"),
        ///             temp.Category,temp.Description, temp.cost, temp.balance;
        ///      
        ///      }
        ///  }
        /// ]]>
        /// </code>
        /// 
        /// Sample output:
        /// <code>
        /// 2020/01 -55   McDonalds             ($45.00)     ($45.00)
        /// 2020/01 -55  Wendys                ($25.00)     ($70.00)
        /// 2020/02 -26.659999999999997  Pizza                 ($33.33)     ($96.66)
        /// 2020/07 -11.11  Cafeteria             ($11.11)    ($107.77)
        /// </code>
        /// 
        /// <b>Getting a list of the budget items by months that from january 1st 2020 to  july 20th 2020, when we only want to see the category 9</b>
        /// <code>
        /// <![CDATA[
        ///  DateTime start=DateTime(2020,1,1); //the date range is inclusive
        ///  DateTime end=DateTime(2020, 7,20);
        ///  HomeBudget budget = new HomeBudget();
        ///  budget.ReadFromFile(filename);
        ///  
        ///  // Get a list of all budget items By Month
        ///  var budgetItems = budget.GetBudgetItemsByMonth(start, end, true, 9);
        ///            
        ///  // print important information
        ///  foreach (BudgetItemsByMonth item in budgetItems)
        ///  {
        ///      Console.WriteLine ( 
        ///          String.Format("{0} {1,-20}", 
        ///             item.Month,
        ///             item.Total));
        ///     
        ///      foreach((BudgetItem temp in item.Details)
        ///      {
        ///         Console.Write(
        ///             String.Format("{2,8:C} {3,12:C} {4,25C} {5,35C}"),
        ///             temp.Category,temp.Description, temp.cost, temp.balance;
        ///      
        ///      }
        ///  }
        /// ]]>
        /// </code>
        /// Sample output:
        /// <code>
        /// 2020/01 -55 scarf                   $15.00        $0.00
        /// 2020/02 -26.659999999999997 mittens                 $15.00     ($88.33)
        /// 2020/02 -26.659999999999997 Hat                     $25.00     ($63.33)
        /// </code>
        /// 
        /// </example>
        // ============================================================================
        public List<BudgetItemsByMonth> GetBudgetItemsByMonth(DateTime? Start, DateTime? End, bool FilterFlag, int CategoryID)
        {
            // -----------------------------------------------------------------------
            // get all items first
            // -----------------------------------------------------------------------
            List<BudgetItem> items = GetBudgetItems(Start, End, FilterFlag, CategoryID);

            // -----------------------------------------------------------------------
            // Group by year/month
            // -----------------------------------------------------------------------
            var GroupedByMonth = items.GroupBy(c => c.Date.Year.ToString("D4") + "/" + c.Date.Month.ToString("D2"));

            // -----------------------------------------------------------------------
            // create new list
            // -----------------------------------------------------------------------
            var summary = new List<BudgetItemsByMonth>();
            foreach (var MonthGroup in GroupedByMonth)
            {
                // calculate total for this month, and create list of details
                double total = 0;
                var details = new List<BudgetItem>();
                foreach (var item in MonthGroup)
                {
                    total = total + item.Amount;
                    details.Add(item);
                }

                // Add new BudgetItemsByMonth to our list
                summary.Add(new BudgetItemsByMonth
                {
                    Month = MonthGroup.Key,
                    Details = details,
                    Total = total
                });
            }

            return summary;
        }

        // ============================================================================
        // Group all expenses by category (ordered by category name)
        /// <summary>
        /// Create a list of all the items group by category
        /// </summary>
        /// <param name="Start">The date start of the list.</param>
        /// <param name="End">The date end of the list</param>
        /// <param name="FilterFlag">This is tell that you want to have special filter on the categories you want to show.</param>
        /// <param name="CategoryID">The category you want appear in the list.</param>
        /// <returns>The list of of all the categories and in each category the list of all the items.</returns>
        /// <example>
        ///The lists are being outputed are always sorted
        ///Also you can have the balance of your account: the balance is how much is in your account( if it's minus you own money)
        /// For all examples below, assume the budget file contains the
        /// following elements:
        /// 
        /// <code>
        /// Cat_ID  Expense_ID  Date                    Description                    Cost
        ///    10       1       1/10/2018 12:00:00 AM   Clothes hat (on credit)         10
        ///     9       2       1/11/2018 12:00:00 AM   Credit Card hat                -10
        ///    10       3       1/10/2019 12:00:00 AM   Clothes scarf(on credit)        15
        ///     9       4       1/10/2020 12:00:00 AM   Credit Card scarf              -15
        ///    14       5       1/11/2020 12:00:00 AM   Eating Out McDonalds            45
        ///    14       7       1/12/2020 12:00:00 AM   Eating Out Wendys               25
        ///    14      10       2/1/2020 12:00:00 AM    Eating Out Pizza                33.33
        ///     9      13       2/10/2020 12:00:00 AM   Credit Card mittens            -15
        ///     9      12       2/25/2020 12:00:00 AM   Credit Card Hat                -25
        ///    14      11       2/27/2020 12:00:00 AM   Eating Out Pizza                33.33
        ///    14       9       7/11/2020 12:00:00 AM   Eating Out Cafeteria            11.11
        /// </code>
        /// 
        /// <b>Getting a list of ALL budget items By Category.</b>
        /// 
        /// <code>
        /// <![CDATA[
        ///  HomeBudget budget = new HomeBudget();
        ///  budget.ReadFromFile(filename);
        ///  
        ///  // Get a list of all budget items By Category
        ///  var budgetItems = budget.GeBudgetItemsByCategory(null, null, false, 0);
        ///            
        ///  // print important information
        ///  foreach (BudgetItemsByCategory item in budgetItems)
        ///  {
        ///      Console.WriteLine ( 
        ///          String.Format("{0} {1,-20}", 
        ///             item.Category,
        ///             item.Total));
        ///     
        ///      foreach((BudgetItem temp in item.Details)
        ///      {
        ///         Console.Write(
        ///             String.Format("{2,8:C} {3,12:C} {4,25C} {5,35C}"),
        ///             temp.Category,temp.Description, temp.cost, temp.balance;
        ///      
        ///      }
        ///  }
        /// ]]>
        /// </code>
        /// 
        /// Sample output:
        /// <code>
        /// Clothe -25 hat (on credit)       ($10.00)     ($10.00)
        /// Clothe -25 scarf(on credit)      ($15.00)     ($15.00)
        /// Credit Card 65 hat                     $10.00        $0.00
        /// Credit Card 65 scarf                   $15.00        $0.00
        /// Credit Card 65 mittens                 $15.00     ($88.33)
        /// Credit Card 65 Hat                     $25.00     ($63.33)
        /// Eating Out  -147.76999999999998 McDonalds             ($45.00)     ($45.00)
        /// Eating Out  -147.76999999999998 Wendys                ($25.00)     ($70.00)
        /// Eating Out  -147.76999999999998 Pizza                 ($33.33)    ($103.33)
        /// Eating Out  -147.76999999999998 Pizza                 ($33.33)     ($96.66)
        /// Eating Out  -147.76999999999998 Cafeteria             ($11.11)    ($107.77)
        /// </code>
        /// 
        /// <b>Getting a list of the budgets items by Category that have a filter and that only want the category 14: </b>
        ///<code>
        /// <![CDATA[
        ///  HomeBudget budget = new HomeBudget();
        ///  budget.ReadFromFile(filename);
        ///  
        ///  // Get a list of all budget items By Category
        ///  var budgetItems = budget.GeBudgetItemsByCategory(null, null, true, 14);
        ///            
        ///  // print important information
        ///  foreach (BudgetItemsByCategory item in budgetItems)
        ///  {
        ///      Console.WriteLine ( 
        ///          String.Format("{0} {1,-20}", 
        ///             item.Category,
        ///             item.Total));
        ///     
        ///      foreach((BudgetItem temp in item.Details)
        ///      {
        ///         Console.Write(
        ///             String.Format("{2,8:C} {3,12:C} {4,25C} {5,35C}"),
        ///             temp.Category,temp.Description, temp.cost, temp.balance;
        ///      
        ///      }
        ///  }
        /// ]]>
        /// </code>
        /// 
        /// Sample output:
        /// <code>
        /// Eating Out  -147.76999999999998 McDonalds             ($45.00)     ($45.00)
        /// Eating Out  -147.76999999999998 Wendys                ($25.00)     ($70.00)
        /// Eating Out  -147.76999999999998 Pizza                 ($33.33)    ($103.33)
        /// Eating Out  -147.76999999999998 Pizza                 ($33.33)     ($96.66)
        /// Eating Out  -147.76999999999998 Cafeteria             ($11.11)    ($107.77)
        /// </code>
        /// 
        /// <b>Getting a list of the budget items by Category that from january 1st 2020 to  july 20th 2020, when we only want to see the category 9</b>
        /// <code>
        /// <![CDATA[
        ///  DateTime start=DateTime(2020,1,1); //the date range is inclusive
        ///  DateTime end=DateTime(2020, 7,20);
        ///  HomeBudget budget = new HomeBudget();
        ///  budget.ReadFromFile(filename);
        ///  
        ///  // Get a list of all budget items By Category
        ///  var budgetItems = budget.GeBudgetItemsByCategory(start, end, true, 9);
        ///            
        ///  // print important information
        ///  foreach (BudgetItemsByCategory item in budgetItems)
        ///  {
        ///      Console.WriteLine ( 
        ///          String.Format("{0} {1,-20}", 
        ///             item.Month,
        ///             item.Total));
        ///     
        ///      foreach((BudgetItem temp in item.Details)
        ///      {
        ///         Console.Write(
        ///             String.Format("{2,8:C} {3,12:C} {4,25C} {5,35C}"),
        ///             temp.Category,temp.Description, temp.cost, temp.balance;
        ///      
        ///      }
        ///  }
        /// ]]>
        /// </code>
        /// Sample output:
        /// <code>
        /// Credit Card 65 scarf                   $15.00        $0.00
        /// Credit Card 65 mittens                 $15.00     ($88.33)
        /// Credit Card 65 Hat                     $25.00     ($63.33)
        /// </code>
        /// 
        /// </example>
        // ============================================================================
        public List<BudgetItemsByCategory> GeBudgetItemsByCategory(DateTime? Start, DateTime? End, bool FilterFlag, int CategoryID)
        {

            // -----------------------------------------------------------------------
            // get all items first
            // -----------------------------------------------------------------------
            List<BudgetItem> items = GetBudgetItems(Start, End, FilterFlag, CategoryID);

            // -----------------------------------------------------------------------
            // Group by Category
            // -----------------------------------------------------------------------
            var GroupedByCategory = items.GroupBy(c => c.Category);

            // -----------------------------------------------------------------------
            // create new list
            // -----------------------------------------------------------------------
            var summary = new List<BudgetItemsByCategory>();
            foreach (var CategoryGroup in GroupedByCategory.OrderBy(g => g.Key))
            {
                // calculate total for this category, and create list of details
                double total = 0;
                var details = new List<BudgetItem>();
                foreach (var item in CategoryGroup)
                {
                    total = total + item.Amount;
                    details.Add(item);
                }

                // Add new BudgetItemsByCategory to our list
                summary.Add(new BudgetItemsByCategory
                {
                    Category = CategoryGroup.Key,
                    Details = details,
                    Total = total
                });
            }

            return summary;
        }


        // ============================================================================
        // Group all expenses by category and Month
        // creates a list of Dictionary objects (which are objects that contain key value pairs).
        // The list of Dictionary objects includes:
        //          one dictionary object per month with expenses,
        //          and one dictionary object for the category totals
        // 
        // Each per month dictionary object has the following key value pairs:
        //           "Month", <the year/month for that month as a string>
        //           "Total", <the total amount for that month as a double>
        //            and for each category for which there is an expense in the month:
        //             "items:category", a List<BudgetItem> of all items in that category for the month
        //             "category", the total amount for that category for this month
        //
        // The one dictionary for the category totals has the following key value pairs:
        //             "Month", the string "TOTALS"
        //             for each category for which there is an expense in ANY month:
        //             "category", the total for that category for all the months
        /// <summary>
        /// Create a dictionary that will hold a list categories and each categories will be sorted by month.
        /// </summary>
        /// <param name="Start">The date start of the list</param>
        /// <param name="End">The date end of the list</param>
        /// <param name="FilterFlag">This is tell that you want to have special filter on the categories you want to show</param>
        /// <param name="CategoryID">The category you want appear in the list</param>
        /// <returns>A dictionary that holds a list categories and each categories will be sorted by month.</returns>
        /// <example>
        ///The lists are being outputed are always sorted
        ///Also you can have the balance of your account: the balance is how much is in your account( if it's minus you own money)
        /// For all examples below, assume the budget file contains the
        /// following elements:
        /// 
        /// <code>
        /// Cat_ID  Expense_ID  Date                    Description                    Cost
        ///    10       1       1/10/2018 12:00:00 AM   Clothes hat (on credit)         10
        ///     9       2       1/11/2018 12:00:00 AM   Credit Card hat                -10
        ///    10       3       1/10/2019 12:00:00 AM   Clothes scarf(on credit)        15
        ///     9       4       1/10/2020 12:00:00 AM   Credit Card scarf              -15
        ///    14       5       1/11/2020 12:00:00 AM   Eating Out McDonalds            45
        ///    14       7       1/12/2020 12:00:00 AM   Eating Out Wendys               25
        ///    14      10       2/1/2020 12:00:00 AM    Eating Out Pizza                33.33
        ///     9      13       2/10/2020 12:00:00 AM   Credit Card mittens            -15
        ///     9      12       2/25/2020 12:00:00 AM   Credit Card Hat                -25
        ///    14      11       2/27/2020 12:00:00 AM   Eating Out Pizza                33.33
        ///    14       9       7/11/2020 12:00:00 AM   Eating Out Cafeteria            11.11
        /// </code>
        /// 
        /// <b>Getting a list of ALL budget items By Category by month.</b>
        /// 
        /// <code>
        /// <![CDATA[
        ///  HomeBudget budget = new HomeBudget();
        ///  budget.ReadFromFile(filename);
        ///  
        ///  // Get a list of all budget items By Category by month
        ///  var budgetItems = budget.GetBudgetDictionaryByCategoryAndMonth(null, null, false, 0);
        ///            
        ///  // print important information
        ///foreach (Dictionary<string, object> dic in list)
        ///     {
        ///         foreach(KeyValuePair<string, object> item in dic)
        ///             {
        ///             
        ///                 if(item.Key == "Month")
        ///                 {
        ///                        Console.WriteLine ( 
        ///                        String.Format("{0} {1,-20}", 
        ///                         item.Key,
        ///                         item.Value));
        ///                 }
        ///                else if (item.Key=="Total")
        ///                {
        ///                        Write(String.Format("{2,30} {3,40}", 
        ///                         item.Key,
        ///                         item.Value));
        ///                }
        ///                else if (regex.IsMatch(item.Key))
        ///                {
        ///                     WriteLine($"{item.Key}\n");
        ///                    foreach (var detail in (List<BudgetItem>)item.Value)
        ///                    {
        ///                         Console.Write(
        ///                     String.Format("{4,50:C} {5,60:C} {6,70C}"),
        ///                     detail.Description, detail.cost, detail.balance;
        ///                     }
        ///                 }
        ///             }
        ///     }
        /// ]]>
        /// </code>
        /// 
        /// Sample output:
        /// <code>
        /// Month 2018/01 Total:0 Clothes hat (on credit)       ($10.00)     ($10.00)
        /// Month 2018/01 Total:0 Credit Card hat                     $10.00        $0.00
        /// Month 2019/01 Total:-15 Clothes scarf(on credit)      ($15.00)     ($15.00)
        /// Month 2020/01 Total:-55 Credit Card scarf                   $15.00        $0.00
        /// Month 2020/01 Total:-55 Eating Out McDonalds             ($45.00)     ($45.00)
        /// Month 2020/01 Total:-55 Eating Out McDonalds Wendys                ($25.00)     ($70.00)
        /// Month 2020/02 Total:-26.9997 Credit Card mittens                 $15.00     ($88.33)
        /// Month 2020/02 Total:-26.9997 Credit Card  Hat                     $25.00     ($63.33)
        /// Month 2020/02 Total:-26.9997  Eating Out Pizza                 ($33.33)    ($103.33)
        /// Month 2020/02 Total:-26.9997 Eating Out Pizza                 ($33.33)     ($96.66)
        /// Month 2020/07 Total:-11.11 Eating Out Cafeteria             ($11.11)    ($107.77)
        /// </code>
        /// 
        /// <b>Getting a list of the budgets items by Category by month that have a filter and that only want the category 14: </b>
        ///<code>
        /// <![CDATA[
        ///  HomeBudget budget = new HomeBudget();
        ///  budget.ReadFromFile(filename);
        ///  
        ///  // Get a list of all budget items By Category by month
        ///  var budgetItems = budget.GetBudgetDictionaryByCategoryAndMonth(null, null, true, 14);
        ///            
        /// //print important information
        ///foreach (Dictionary<string, object> dic in list)
        ///     {
        ///         foreach(KeyValuePair<string, object> item in dic)
        ///             {
        ///             
        ///                 if(item.Key == "Month")
        ///                 {
        ///                        Console.WriteLine ( 
        ///                        String.Format("{0} {1,-20}", 
        ///                         item.Key,
        ///                         item.Value));
        ///                 }
        ///                else if (item.Key=="Total")
        ///                {
        ///                        Write(String.Format("{2,30} {3,40}", 
        ///                         item.Key,
        ///                         item.Value));
        ///                }
        ///                else if (regex.IsMatch(item.Key))
        ///                {
        ///                     WriteLine($"{item.Key}\n");
        ///                    foreach (var detail in (List<BudgetItem>)item.Value)
        ///                    {
        ///                         Console.Write(
        ///                     String.Format("{4,50:C} {5,60:C} {6,70C} {7,80C}"),
        ///                     detail.Category,detail.Description, detail.cost, detail.balance;
        ///                     }
        ///                 }
        ///             }
        ///     }
        /// ]]>
        /// </code>
        /// 
        /// Sample output:
        /// <code>
        /// Month 2020/01 Total:-55 Eating Out McDonalds             ($45.00)     ($45.00)
        /// Month 2020/01 Total:-55 Eating Out McDonalds Wendys                ($25.00)     ($70.00)
        /// Month 2020/02 Total:-26.9997  Eating Out Pizza                 ($33.33)    ($103.33)
        /// Month 2020/02 Total:-26.9997 Eating Out Pizza                 ($33.33)     ($96.66)
        /// Month 2020/07 Total:-11.11 Eating Out Cafeteria             ($11.11)    ($107.77)
        /// </code>
        /// 
        /// <b>Getting a list of the budget items by Category by month that from january 1st 2020 to  july 20th 2020, when we only want to see the category 9</b>
        /// <code>
        /// <![CDATA[
        ///  DateTime start=DateTime(2020,1,1); //the date range is inclusive
        ///  DateTime end=DateTime(2020, 7,20);
        ///  HomeBudget budget = new HomeBudget();
        ///  budget.ReadFromFile(filename);
        ///  
        ///  // Get a list of all budget items By Category by month
        ///  var budgetItems = budget.GetBudgetDictionaryByCategoryAndMonth(start, end, true, 9);
        ///            
        ///  // print important information
        ///foreach (Dictionary<string, object> dic in list)
        ///     {
        ///         foreach(KeyValuePair<string, object> item in dic)
        ///             {
        ///             
        ///                 if(item.Key == "Month")
        ///                 {
        ///                        Console.WriteLine ( 
        ///                        String.Format("{0} {1,-20}", 
        ///                         item.Key,
        ///                         item.Value));
        ///                 }
        ///                else if (item.Key=="Total")
        ///                {
        ///                        Write(String.Format("{2,30} {3,40}", 
        ///                         item.Key,
        ///                         item.Value));
        ///                }
        ///                else if (regex.IsMatch(item.Key))
        ///                {
        ///                     WriteLine($"{item.Key}\n");
        ///                    foreach (var detail in (List<BudgetItem>)item.Value)
        ///                    {
        ///                         Console.Write(
        ///                     String.Format("{4,50:C} {5,60:C} {6,70C} {7,80C}"),
        ///                     detail.Category,detail.Description, detail.cost, detail.balance;
        ///                     }
        ///                 }
        ///             }
        ///     }
        /// ]]>
        /// </code>
        /// Sample output:
        /// <code>
        /// Month 2020/01 Total:-55 Credit Card scarf                   $15.00        $0.00
        /// Month 2020/02 Total:-26.9997 Credit Card mittens                 $15.00     ($88.33)
        /// Month 2020/02 Total:-26.9997 Credit Card  Hat                     $25.00     ($63.33)
        /// </code>
        /// 
        /// </example>
        // ============================================================================
        public List<Dictionary<string, object>> GetBudgetDictionaryByCategoryAndMonth(DateTime? Start, DateTime? End, bool FilterFlag, int CategoryID)
        {
            // -----------------------------------------------------------------------
            // get all items by month 
            // -----------------------------------------------------------------------
            List<BudgetItemsByMonth> GroupedByMonth = GetBudgetItemsByMonth(Start, End, FilterFlag, CategoryID);

            // -----------------------------------------------------------------------
            // loop over each month
            // -----------------------------------------------------------------------
            var summary = new List<Dictionary<string, object>>(); // give u a list of Dico and each Dico ( string is the key and object is a the value)
            var totalsPerCategory = new Dictionary<String, Double>();// is a dictionary with a string has the key of each value that are double

            foreach (var MonthGroup in GroupedByMonth) // foreach month in a groupedByMonth
            {
                // create record object for this month
                Dictionary<string, object> record = new Dictionary<string, object>(); //for each month create a dictionary
                record["Month"] = MonthGroup.Month; //the month key (month is the name of key) = the name of the month (which is an object)
                record["Total"] = MonthGroup.Total; // the total key (total is the name of the key) = the total of that given month (which is a object)

                // break up the month details into categories
                // Cindy: Details is a list of budget items
                // this is saying for the given month get the list of budget items and group it by Category(Income, Expense, Credit, Saving) by budget item
                var GroupedByCategory = MonthGroup.Details.GroupBy(c => c.Category);// so GroupedByCategory is a list budget items grouped by categories (key=category) (value=list of budject items)

                // -----------------------------------------------------------------------
                // loop over each category
                // -----------------------------------------------------------------------
                foreach (var CategoryGroup in GroupedByCategory.OrderBy(g => g.Key)) //order Alphabetically each key of the dictionary GroupedByCategory (so basically the categories)
                {

                    // calculate totals for the cat/month, and create list of details
                    double total = 0;
                    var details = new List<BudgetItem>();

                    foreach (var item in CategoryGroup) // for the item in the list budget items of the given category(alphabitically)
                    {
                        total = total + item.Amount; // get the total of the given month on that given category
                        details.Add(item); // add the given item to the list of details
                    }

                    // add new properties and values to our record object
                    record["details:" + CategoryGroup.Key] = details; // for the "details cateoryGroup(name)"=key the value will be the list of items in this given category
                    record[CategoryGroup.Key] = total; // and for category name=key the value will be the total of that month in the given category

                    // keep track of totals for each category
                    if (totalsPerCategory.TryGetValue(CategoryGroup.Key, out Double CurrentCatTotal)) // if the total is a cat/month is a double
                    {
                        totalsPerCategory[CategoryGroup.Key] = CurrentCatTotal + total; // the total of that given month will be current category value+ total
                    }
                    else
                    {
                        totalsPerCategory[CategoryGroup.Key] = total;
                    }
                }

                // add record to collection
                summary.Add(record); // in the summary list of dictionnary each dictionnary represent the month -> that has different categories -> that has total price and details of each items
            }
            // ---------------------------------------------------------------------------
            // add final record which is the totals for each category
            // ---------------------------------------------------------------------------
            Dictionary<string, object> totalsRecord = new Dictionary<string, object>();
            totalsRecord["Month"] = "TOTALS";

            foreach (var cat in categories.List())
            {
                try
                {
                    totalsRecord.Add(cat.Description, totalsPerCategory[cat.Description]); // foreach category find the description to put it has a key then get the total of that category 
                }
                catch { }
            }
            summary.Add(totalsRecord); // the dictionary of given categories description=key total=total


            return summary;
        }




        #endregion GetList

    }
}
