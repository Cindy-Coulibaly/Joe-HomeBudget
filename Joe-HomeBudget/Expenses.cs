using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;

public class UserInputErrors : Exception
{
    public string InputErrorString { get; } // extra info

    public UserInputErrors() { }         // standard constructor

    public UserInputErrors(string message) // calls 'Exception(message)'
        : base(message) { }

    public UserInputErrors(string message, string inputErrorString)
        : this(message)
    {
        InputErrorString = inputErrorString;
    }
}

// ============================================================================
// (c) Sandy Bultena 2018
// * Released under the GNU General Public License
// ============================================================================

namespace Budget
{
    // ====================================================================
    // CLASS: expenses
    /// <summary>
    /// A collection of expense items, it also read from a file where there is expenses and will
    /// populate it into a list, in addition it can output your list of expenses into a file.
    /// </summary>
    // ====================================================================
    public class Expenses
    {
        private static String DefaultFileName = "budget.txt";
        private List<Expense> _Expenses = new List<Expense>();
        private string _FileName;
        private string _DirName;

        // ====================================================================
        // Properties
        // ====================================================================
        /// <value>
        /// The name of the file, that your expenses are in.
        /// </value>
        public String FileName { get { return _FileName; } }
        /// <value>
        /// The directory name of where your file of expenses are.
        /// </value>
        public String DirName { get { return _DirName; } }

        // ====================================================================
        // populate categories from a file
        // if filepath is not specified, read/save in AppData file
        // Throws System.IO.FileNotFoundException if file does not exist
        // Throws System.Exception if cannot read the file correctly (parsing XML)
        /// <summary>
        /// Get all the expenses from your files and will put them in a list of expenses.
        /// </summary>
        /// <param name="filepath">The file path of the file you want to get your expenses from.</param>
        /// <example>
        /// <code>
        /// Expenses expenses = new Expenses();
        ///expenses.ReadFromFile("./File.path");
        /// </code>
        /// </example>
        // ====================================================================
        public void ReadFromFile(String filepath = null)
        {

            // ---------------------------------------------------------------
            // reading from file resets all the current expenses,
            // so clear out any old definitions
            // ---------------------------------------------------------------
            _Expenses.Clear();

            // ---------------------------------------------------------------
            // reset default dir/filename to null 
            // ... filepath may not be valid, 
            // ---------------------------------------------------------------
            _DirName = null;
            _FileName = null;

            // ---------------------------------------------------------------
            // get filepath name (throws exception if it doesn't exist)
            // ---------------------------------------------------------------
            filepath = BudgetFiles.VerifyReadFromFileName(filepath, DefaultFileName);

            // ---------------------------------------------------------------
            // read the expenses from the xml file
            // ---------------------------------------------------------------
            _ReadXMLFile(filepath);

            // ----------------------------------------------------------------
            // save filename info for later use?
            // ----------------------------------------------------------------
            _DirName = Path.GetDirectoryName(filepath);
            _FileName = Path.GetFileName(filepath);
        }


        // ====================================================================
        // save to a file
        // if filepath is not specified, read/save in AppData file
        /// <summary>
        /// Write all your expenses into a file and save it.
        /// </summary>
        /// <param name="filepath">The file path of the file where you want to save all your expenses informations.</param>
        /// <example>
        /// <code>
        /// Expenses expenses = new Expenses();
        ///expenses.Add( DateTime.Now, (int)Category.CategoryType.Expense, 
        ///23.45, "textbook" );
        ///expenses.Add(DateTime.Now,(int)Category.CategoryType.Credit,40.00,"school gym fee");
        ///expenses.SaveToFile("./filepath");
        /// </code>
        /// </example>
        // ====================================================================
        public void SaveToFile(String filepath = null)
        {
            // ---------------------------------------------------------------
            // if file path not specified, set to last read file
            // ---------------------------------------------------------------
            if (filepath == null && DirName != null && FileName != null)
            {
                filepath = DirName + "\\" + FileName;
            }

            // ---------------------------------------------------------------
            // just in case filepath doesn't exist, reset path info
            // ---------------------------------------------------------------
            _DirName = null;
            _FileName = null;

            // ---------------------------------------------------------------
            // get filepath name (throws exception if it doesn't exist)
            // ---------------------------------------------------------------
            filepath = BudgetFiles.VerifyWriteToFileName(filepath, DefaultFileName);

            // ---------------------------------------------------------------
            // save as XML
            // ---------------------------------------------------------------
            _WriteXMLFile(filepath);

            // ----------------------------------------------------------------
            // save filename info for later use
            // ----------------------------------------------------------------
            _DirName = Path.GetDirectoryName(filepath);
            _FileName = Path.GetFileName(filepath);
        }



        // ====================================================================
        // Add expense
        // ====================================================================
        private void Add(Expense exp)
        {
            _Expenses.Add(exp);
        }
        /// <summary>
        /// Add new expenses into your lists of expenses.
        /// </summary>
        /// <param name="date">The date of the expense.</param>
        /// <param name="category">What type of category the expense is it:Income, Expense, Credit, Saving .</param>
        /// <param name="amount">The amount of the expense.</param>
        /// <param name="description">The information of the expense.</param>
        /// <example>
        /// <code>
        /// Expenses expenses = new Expenses();
        ///expenses.Add( DateTime.Now, (int)Category.CategoryType.Expense, 
        ///23.45, "textbook" );
        /// </code>
        /// </example>
        public void Add(DateTime date, int category, Double amount, String description)
        {
            int new_id = 1;

            // if we already have expenses, set ID to max
            if (_Expenses.Count > 0)
            {
                new_id = (from e in _Expenses select e.Id).Max();
                new_id++;
            }

            _Expenses.Add(new Expense(new_id, date, category, amount, description));

        }
        public void UpdateExpense(int id, DateTime date, int category, Double amount, String description)
        {
            try
            {
                UpdateExpenseToDatabase(id,date,category,amount,description);
            }
            catch (UserInputErrors e)
            {
                Console.WriteLine(e.Message + ": " + e.InputErrorString);
            }
            catch (Exception e)
            {
                Console.WriteLine("Unknown error: " + e.Message);
            }
        }

        /// <summary>
        /// Updates the expense in the database according to its id.
        /// </summary>
        /// <param name="id"> Id of the expense. </param>
        /// <param name="date"> Date when the expense was done. </param>
        /// <param name="category"> Category of the expense. </param>
        /// <param name="amount"> Amount of the expense. </param>
        /// <param name="description"> Description of the expense. </param>
        public void UpdateExpenseToDatabase(int id, DateTime date, int category, Double amount, String description)
        {
            if (description != string.Empty)
            {
                string stringDate = date.ToString("yyyy-MM-dd");

                //create a command search for the given id
                using var cmdCheckId = new SQLiteCommand("SELECT Id from expenses WHERE Id=" + "@id", Database.dbConnection);
                cmdCheckId.Parameters.AddWithValue("@id", id);

                //take the first column of the select query
                //Parse object to int because ExecuteScalar() return an object
                object firstCollumId = cmdCheckId.ExecuteScalar();

                //if the id doesn't exist then insert to database
                if (firstCollumId != null)
                {

                    using var cmd = new SQLiteCommand(Database.dbConnection);
                    cmd.CommandText = $"UPDATE expenses Set Description = @description, Date = @date, Category = @category, Amount = @amount WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@description", description);
                    cmd.Parameters.AddWithValue("@date", stringDate);
                    cmd.Parameters.AddWithValue("@category", category);
                    cmd.Parameters.AddWithValue("@amount", amount);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
                else
                {
                    throw new UserInputErrors("Expense doesn't exist");
                }
            }
            else
            {
                throw new UserInputErrors("No description provided");
            }



        }

        public Expense GetExpenseFromId(int i)
        {
            List<Expense> newList = List();
            Expense c = newList.Find(x => x.Id == i);
            if (c == null)
            {
                throw new Exception("Cannot find category with id " + i.ToString());
            }
            return c;
        }

        // ====================================================================
        // Delete expense
        /// <summary>
        /// Delete an expense from your list of expenses.
        /// </summary>
        /// <param name="Id">The Id of the expense you want to delete from the file.</param>
        /// <example>
        /// <code>
        /// Expenses expenses = new Expenses();
        ///expenses.Add( DateTime.Now, (int)Category.CategoryType.Expense,23.45, "textbook" );
        ///expenses.Add(DateTime.Now,(int)Category.CategoryType.Credit,40.00,"school gym fee");
        ///expenses.Remove(1);
        /// </code>
        /// </example>
        // ====================================================================
        public void Delete(int Id)
        {
            int i = _Expenses.FindIndex(x => x.Id == Id);
            if (i != -1) { _Expenses.RemoveAt(i); } // will only delete if valid id

            try 
            {
                DeleteExpense(Id);
            }
            catch(UserInputErrors e)
            {
                Console.WriteLine(e.Message + ": " + e.InputErrorString);
            }
            catch(Exception e)
            {
                Console.WriteLine("Unknown error: " + e.Message);
            }

        }
        /// <summary>
        /// Deletes expense from the database.
        /// </summary>
        /// <param name="id"> Id of the expense to delete. </param>
        public void DeleteExpense(int id)
        {
            //create a command search for the given id
            using var cmdCheckId = new SQLiteCommand("SELECT Id from expenses WHERE Id=" + "@id", Database.dbConnection);
            cmdCheckId.Parameters.AddWithValue("@id", id);

            //take the first column of the select query
            //Parse object to int because ExecuteScalar() return an object
            object firstCollumId = cmdCheckId.ExecuteScalar();

            //if the id doesn't exist then insert to database
            if (firstCollumId != null)
            {

                using var cmd = new SQLiteCommand(Database.dbConnection);
                cmd.CommandText = "DELETE FROM expenses WHERE Id=" + "@id";
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();

                Console.WriteLine("Successfully deleted from Id=" + id);
            }
            else
            {
                throw new UserInputErrors("Expense doesn't exist");
            }

        }


        // ====================================================================
        // Return list of expenses
        // Note:  make new copy of list, so user cannot modify what is part of
        //        this instance
        /// <summary>
        /// Create a copy of the list of expenses, this copy will be unmodifyable.
        /// </summary>
        /// <returns>The copy of your expenses.</returns>
        /// <example>
        /// <code>
        /// Expenses expenses = new Expenses();
        ///expenses.Add( DateTime.Now, (int)Category.CategoryType.Expense, 
        ///23.45, "textbook" );
        ///List<Expense> list = expenses.List();
        ///foreach (Expense expense in list)
        ///Console.WriteLine(expense.Description);
        /// </code>
        /// </example>
        // ====================================================================
        public List<Expense> List()
        {
            List<Expense> newList = new List<Expense>();
            foreach (Expense expense in _Expenses)
            {
                newList.Add(new Expense(expense));
            }
            return newList;
        }


        // ====================================================================
        // read from an XML file and add categories to our categories list
        // ====================================================================
        private void _ReadXMLFile(String filepath)
        {


            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(filepath);

                // Loop over each Expense
                foreach (XmlNode expense in doc.DocumentElement.ChildNodes)
                {
                    // set default expense parameters
                    int id = int.Parse((((XmlElement)expense).GetAttributeNode("ID")).InnerText);
                    String description = "";
                    DateTime date = DateTime.Parse("2000-01-01");
                    int category = 0;
                    Double amount = 0.0;

                    // get expense parameters
                    foreach (XmlNode info in expense.ChildNodes)
                    {
                        switch (info.Name)
                        {
                            case "Date":
                                date = DateTime.Parse(info.InnerText);
                                break;
                            case "Amount":
                                amount = Double.Parse(info.InnerText);
                                break;
                            case "Description":
                                description = info.InnerText;
                                break;
                            case "Category":
                                category = int.Parse(info.InnerText);
                                break;
                        }
                    }

                    // have all info for expense, so create new one
                    this.Add(new Expense(id, date, category, amount, description));

                }

            }
            catch (Exception e)
            {
                throw new Exception("ReadFromFileException: Reading XML " + e.Message);
            }
        }


        // ====================================================================
        // write to an XML file
        // if filepath is not specified, read/save in AppData file
        // ====================================================================
        private void _WriteXMLFile(String filepath)
        {
            // ---------------------------------------------------------------
            // loop over all categories and write them out as XML
            // ---------------------------------------------------------------
            try
            {
                // create top level element of expenses
                XmlDocument doc = new XmlDocument();
                doc.LoadXml("<Expenses></Expenses>");

                // foreach Category, create an new xml element
                foreach (Expense exp in _Expenses)
                {
                    // main element 'Expense' with attribute ID
                    XmlElement ele = doc.CreateElement("Expense");
                    XmlAttribute attr = doc.CreateAttribute("ID");
                    attr.Value = exp.Id.ToString();
                    ele.SetAttributeNode(attr);
                    doc.DocumentElement.AppendChild(ele);

                    // child attributes (date, description, amount, category)
                    XmlElement d = doc.CreateElement("Date");
                    XmlText dText = doc.CreateTextNode(exp.Date.ToString("yyyy-MM-dd")); // changed hh:mm:ss tt/ M/dd/yyyy
                    ele.AppendChild(d);
                    d.AppendChild(dText);

                    XmlElement de = doc.CreateElement("Description");
                    XmlText deText = doc.CreateTextNode(exp.Description);
                    ele.AppendChild(de);
                    de.AppendChild(deText);

                    XmlElement a = doc.CreateElement("Amount");
                    XmlText aText = doc.CreateTextNode(exp.Amount.ToString());
                    ele.AppendChild(a);
                    a.AppendChild(aText);

                    XmlElement c = doc.CreateElement("Category");
                    XmlText cText = doc.CreateTextNode(exp.Category.ToString());
                    ele.AppendChild(c);
                    c.AppendChild(cText);

                }

                // write the xml to FilePath
                doc.Save(filepath);

            }
            catch (Exception e)
            {
                throw new Exception("SaveToFileException: Reading XML " + e.Message);
            }
        }

    }
}

