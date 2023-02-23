using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using System.Data.SQLite;
using System.Data.Common;
using static Budget.Category;
using System.Data.Entity;
using System.Data.SqlClient;
using static System.Net.Mime.MediaTypeNames;
using System.Reflection.PortableExecutable;


// ============================================================================
// (c) Sandy Bultena 2018
// * Released under the GNU General Public License
// ============================================================================

namespace Budget
{
    // ====================================================================
    /// <summary>
    /// Create a list of category,also it will take
    /// an input file that has category and will populate it in a list, it can also
    /// write the category list in a given file.
    /// </summary>
    // CLASS: categories
    //        - A collection of category items,
    //        - Read / write to file
    //        - etc
    // ====================================================================
    public class Categories
    {
        private static String DefaultFileName = "budgetCategories.txt";
        private List<Category> _Cats = new List<Category>();
        private string _FileName;
        private string _DirName;

        // ====================================================================
        // Properties
        // ====================================================================
        /// <value>
        /// The file of name. 
        /// </value>
         public String FileName { get { return _FileName; } }
        /// <value>
        /// The directory of the file.
        /// </value>
        public String DirName { get { return _DirName; } }

        // ====================================================================
        // Constructor
        // ====================================================================
        /// <summary>
        /// Create a new list of categories.
        /// </summary>
        public Categories()
        {
            SetCategoriesToDefaults();
            //DBCategoryType(Database.dbConnection);// try it
        }

        /// <summary>
        /// Gets a list of categories from previous database
        /// </summary>
        /// <param name="dbConnection"> new connection to database</param>
        /// <param name="newDb">If false, it will retrieve contents from databases</param>
        
        public Categories(SQLiteConnection dbConnection, bool newDb)
        {
            if (!newDb)
            {

               RetrieveCategoriesFromDatabase(dbConnection);
            }
            else
            {
                DBCategoryType(Database.dbConnection);
                //If there is new database then automatically set it to default
                SetCategoriesToDefaults();

            }
        }



        private void DBCategoryType(SQLiteConnection db)
        {

            using var cmd = new SQLiteCommand(db);

            cmd.CommandText = "INSERT INTO categoryTypes(Id,Description) VALUES(1, 'Income')";
            cmd.ExecuteNonQuery();

            cmd.CommandText = "INSERT INTO categoryTypes(Id,Description) VALUES(2, 'Expense')";
            cmd.ExecuteNonQuery();

            cmd.CommandText = "INSERT INTO categoryTypes(Id,Description) VALUES(3, 'Credit')";
            cmd.ExecuteNonQuery();

            cmd.CommandText = "INSERT INTO categoryTypes(Id,Description) VALUES(4, 'Savings')";
            cmd.ExecuteNonQuery();

        }








        /// <summary>
        /// Retrieve contents from the database
        /// </summary>
        /// <param name="dbConnection">Represents connection to database</param>

        public void RetrieveCategoriesFromDatabase(SQLiteConnection dbConnection)
        {
            //dbConnection.Open();

            //get the objects category data directly from the database and insert it into the list
            using var cmd = new SQLiteCommand(dbConnection);

            cmd.CommandText = "SELECT * FROM categories";
            using var newAddedId = new SQLiteCommand("SELECT * FROM categories", dbConnection);
            var rdr = newAddedId.ExecuteReader();
            while (rdr.Read())
            {              
                _Cats.Add(new Category((int)(long)rdr[0], (string)rdr[1], (CategoryType)(int)(long)rdr[2]));
            }

            //_Cats = (List<Category>)cmd.ExecuteScalar(); ------------------------CHANGED, CAN NOT PARSE A LIST
            //dbConnection.Close();
        }


        // ====================================================================
        // get a specific category from the list where the id is the one specified
        /// <summary>
        /// Get a specific category from the list where the id is the one specified.
        /// </summary>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// Categories listCat=new Categories();
        /// Category cat=listCat.GetCategoryFromId(1);
        /// ]]>
        /// </code>
        /// </example>
        /// <param name="i">The id of the category you want.</param>
        /// <returns>The information about the given category.</returns>
        /// <exception cref="Exception">If the category isn't in the list of categories.</exception>
        // ====================================================================
        public Category GetCategoryFromId(int i)
        {
            Category c = _Cats.Find(x => x.Id == i);
            if (c == null)
            {
                throw new Exception("Cannot find category with id " + i.ToString());
            }
            return c;
        }

        // ====================================================================
        // populate categories from a file
        // if filepath is not specified, read/save in AppData file
        // Throws System.IO.FileNotFoundException if file does not exist
        // Throws System.Exception if cannot read the file correctly (parsing XML)
        /// <summary>
        /// Read from a file, if it exists, and
        /// create a list a categories out of the category in the given file.
        /// </summary>
        /// <example>
        /// <code>
        /// Categories cat = new Categories();
        /// cat.ReadFromFile("./file.cats");
        /// </code>
        /// </example>
        /// <param name="filepath">The filepath of the file you want to read from.</param>
        // ====================================================================
        public void ReadFromFile(String filepath = null)
        {

            // ---------------------------------------------------------------
            // reading from file resets all the current categories,
            // ---------------------------------------------------------------
            _Cats.Clear();

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
            // If file exists, read it
            // ---------------------------------------------------------------
            _ReadXMLFile(filepath);
            _DirName = Path.GetDirectoryName(filepath);
            _FileName = Path.GetFileName(filepath);
        }

        // ====================================================================
        // save to a file
        // if filepath is not specified, read/save in AppData file
        /// <summary>
        /// Save the category list into a file, if the file exists.
        /// </summary>
        /// <example>
        /// <code>
        /// Categories cat = new Categories();
        /// cat.SaveToFile("./file.txt");
        /// </code>
        /// </example>
        /// <param name="filepath">The filepath of the file you want to write the category list too.</param>
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
        // set categories to default
        /// <summary>
        /// Sets the category list some default value.
        /// </summary>
        /// <example>
        /// Categories cat = new Categories();
        /// cat.SetCategoriesToDefaults();
        /// </example>
        // ====================================================================
        public void SetCategoriesToDefaults()
        {
            // ---------------------------------------------------------------
            // reset any current categories,
            // ---------------------------------------------------------------
            _Cats.Clear();

            // ---------------------------------------------------------------
            // Add Defaults
            // ---------------------------------------------------------------
            Add("Utilities", Category.CategoryType.Expense);
            Add("Rent", Category.CategoryType.Expense);
            Add("Food", Category.CategoryType.Expense);
            Add("Entertainment", Category.CategoryType.Expense);
            Add("Education", Category.CategoryType.Expense);
            Add("Miscellaneous", Category.CategoryType.Expense);
            Add("Medical Expenses", Category.CategoryType.Expense);
            Add("Vacation", Category.CategoryType.Expense);
            Add("Credit Card", Category.CategoryType.Credit);
            Add("Clothes", Category.CategoryType.Expense);
            Add("Gifts", Category.CategoryType.Expense);
            Add("Insurance", Category.CategoryType.Expense);
            Add("Transportation", Category.CategoryType.Expense);
            Add("Eating Out", Category.CategoryType.Expense);
            Add("Savings", Category.CategoryType.Savings);
            Add("Income", Category.CategoryType.Income);

        }
        /// <summary>
        /// Deletes a category from the database based on its id if a valid id is provided.
        /// </summary>
        /// <param name="id"> Id of the category. </param>
        /// <param name="db"> SQLite Database connection. </param>
        public void DeleteCategory(int id)
        {
            //create a command search for the given id
            using var cmdCheckId = new SQLiteCommand("SELECT Id from categories WHERE Id=" + id, Database.dbConnection);

            //take the first column of the select query
            //Parse object to int because ExecuteScalar() return an object
            object firstCollumId = cmdCheckId.ExecuteScalar();

            //if the id doesn't exist then insert to database
            if (firstCollumId != null)
            {
                using var beforeDeletedId = new SQLiteCommand("SELECT * FROM categories WHERE Id=" + id, Database.dbConnection);
                var rdr = beforeDeletedId.ExecuteReader();
                while (rdr.Read())
                {
                    Console.WriteLine("Category Id {0} before the delete: desc: {1}, type: {2}", rdr[0], rdr[1], rdr[2]);
                }


                using var cmd = new SQLiteCommand(Database.dbConnection);
                cmd.CommandText = "DELETE FROM categories WHERE Id=" + id;
                cmd.ExecuteNonQuery();

                Console.WriteLine("Successfully deleted from Id=" + id);
            }
            else
            {
                Console.WriteLine("Category doesn't exist");
            }

        }
        // ====================================================================
        // Add category
        // ====================================================================
        private void Add(Category cat)
        {
            _Cats.Add(cat);
        }


        #region Add to database

        /// add by creating category list
        public void AddCategoriesToDatabase1(Category cat)
        {
            int id = cat.Id;
            string description = cat.Description;
            CategoryType type = cat.Type;

            //Database.dbConnection.Open();

            //create a command search for the given id
            using var cmdCheckId = new SQLiteCommand("SELECT Id FROM categories WHERE Id=" + id , Database.dbConnection);
            

            //take the first column of the select query
            object firstCollumId = cmdCheckId.ExecuteScalar();

            //if the category doesn't exist in the database already, then insert it;
            if (firstCollumId == null)
            {               
                using var cmd = new SQLiteCommand(Database.dbConnection);
                cmd.CommandText = $"INSERT INTO categories(Id, Description, TypeId) VALUES({id}, '{description}', {(int)type})";
                cmd.ExecuteNonQuery();
                using var newAddedId = new SQLiteCommand("SELECT * FROM categories WHERE Id=" + id, Database.dbConnection);
                var rdr = newAddedId.ExecuteReader();
                while (rdr.Read())
                {
                    Console.WriteLine("Category added: id: {0}, desc: {1}, type: {2}", rdr[0], rdr[1], rdr[2]);

                }
                //Database.dbConnection.Close();
            }
            else
            {
                using var newAddedId = new SQLiteCommand("SELECT Id FROM categories WHERE Id=" + id, Database.dbConnection);
                var rdr = newAddedId.ExecuteReader();
                while (rdr.Read())
                {
                    Console.WriteLine("Category already exist, id: {0}", rdr[0]);
                }
            }           
        }

        //auto-increment a highest number Id
        public void AddCategoriesToDatabase2(String desc, Category.CategoryType type)
        {
            Int64 id;
            using var countCMD = new SQLiteCommand("SELECT COUNT(Id) FROM categories", Database.dbConnection);
            object idCount = countCMD.ExecuteScalar();
            id = (Int64)idCount;

            //Database.dbConnection.Open();

            //create a command search for the given id
            using var cmdCheckId = new SQLiteCommand("SELECT Id FROM categories WHERE Id=" + id, Database.dbConnection);

            //take the first column of the select query
            object firstCollumId = cmdCheckId.ExecuteScalar();

            //if the database is empty then automatically insert it, else find the highest id, and create a new one after the highest one
            if (firstCollumId == null)
            {
                id++;
                using var cmd = new SQLiteCommand(Database.dbConnection);
                cmd.CommandText = $"INSERT INTO categories(Id, Description, TypeId) VALUES({id}, '{desc}', {(int)type})";
                cmd.ExecuteNonQuery();
                using var newAddedId = new SQLiteCommand("SELECT * FROM categories WHERE Id=" + id, Database.dbConnection);
                var rdr = newAddedId.ExecuteReader();
                while (rdr.Read())
                {
                    Console.WriteLine("Category added: id: {0}, desc: {1}, type: {2}", rdr[0], rdr[1], rdr[2]);
                }
            }
            else
            {
                
                using var maxCMD = new SQLiteCommand("SELECT MAX(Id) from categories", Database.dbConnection);
                object highestId = maxCMD.ExecuteScalar();
                id = (Int64)highestId;
                id++;

                using var cmd = new SQLiteCommand(Database.dbConnection);
                cmd.CommandText = $"INSERT INTO categories(Id, Description, TypeId) VALUES({id}, '{desc}', {(int)type})";
                cmd.ExecuteNonQuery();
                using var newAddedId = new SQLiteCommand("SELECT * FROM categories WHERE Id=" + id, Database.dbConnection);
                var rdr = newAddedId.ExecuteReader();
                while (rdr.Read())
                {
                    Console.WriteLine("Category added: id: {0}, desc: {1}, type: {2}", rdr[0], rdr[1], rdr[2]);
                }
            }
        }

        //add without creating category list
        public void AddCategoriesToDatabase3(int id, String desc, Category.CategoryType type)
        {

            //Database.dbConnection.Open();

            //create a command search for the given id
            using var cmdCheckId = new SQLiteCommand("SELECT Id FROM categories WHERE Id=" + id, Database.dbConnection);


            //take the first column of the select query
            object firstCollumId = cmdCheckId.ExecuteScalar();

            //if the category doesn't exist in the database already, then insert it;
            if (firstCollumId == null)
            {
                using var cmd = new SQLiteCommand(Database.dbConnection);
                cmd.CommandText = $"INSERT INTO categories(Id, Description, TypeId) VALUES({id}, '{desc}', {(int)type + 1})";
                cmd.ExecuteNonQuery();
                using var newAddedId = new SQLiteCommand("SELECT * FROM categories WHERE Id=" + id, Database.dbConnection);
                var rdr = newAddedId.ExecuteReader();
                while (rdr.Read())
                {
                    Console.WriteLine("Category added: id: {0}, desc: {1}, type: {2}", rdr[0], rdr[1], rdr[2]);

                }
                //Database.dbConnection.Close();
            }
            else
            {
                using var newAddedId = new SQLiteCommand("SELECT Id FROM categories WHERE Id=" + id, Database.dbConnection);
                var rdr = newAddedId.ExecuteReader();
                while (rdr.Read())
                {
                    Console.WriteLine("Category already exist, id: {0}", rdr[0]);
                }
            }
        }

        #endregion


        /// <summary>
        /// Update a category to the Database.
        /// </summary>
        /// <example>
        /// <code>
        /// Categories listCats = new Categories();
        /// listCats.Add(1, "jam",Category.CategoryType.Expense);
        /// listCats.UpdateInDatabase(2, "ham",Category.CategoryType.Saving);
        /// </code>
        /// </example>
        /// <param name="Id">The Id of the category being updated to the database.</param>
        /// <param name="desc">The description of the category being updated database.</param>
        /// <param name="type">The type of the category updated to the database.</param>
        public void UpdateInDatabase(int id, string desc, CategoryType type) //----------------------Change to private once done
        {
            //Must provide desciprtion
            if (desc != string.Empty)
            {
                //create a command search for the given id
                using var cmdCheckId = new SQLiteCommand("SELECT Id from categories WHERE Id=" + id, Database.dbConnection);

                //take the first column of the selected query
                object firstCollumId = cmdCheckId.ExecuteScalar();

                //if the id doesn't exist then insert to database
                if (firstCollumId != null)
                {
                    //Output data before update
                    using var beforeUpdatedId = new SQLiteCommand("SELECT * FROM categories WHERE Id=" + id, Database.dbConnection);
                    var rdr = beforeUpdatedId.ExecuteReader();
                    while (rdr.Read())
                    {
                        Console.WriteLine("Category id {0} before the update: desc: {1}, type: {2}", rdr[0], rdr[1], rdr[2]);
                    }

                    //update
                    using var cmd = new SQLiteCommand(Database.dbConnection);
                    cmd.CommandText = $"UPDATE categories Set Description ='{desc}', TypeId = {(int)type + 1} WHERE Id = {id}";
                    cmd.ExecuteNonQuery();

                    //Output data after update
                    using var updatedId = new SQLiteCommand("SELECT * FROM categories WHERE Id=" + id, Database.dbConnection);
                    rdr = updatedId.ExecuteReader();
                    while (rdr.Read())
                    {
                        Console.WriteLine("Category id {0} after the update: desc: {1}, type: {2}", rdr[0], rdr[1], rdr[2]);
                    }
                }
                else
                {
                    Console.WriteLine("Category doesn't exist");
                }
            }
            else
            {
                Console.WriteLine("No description provided");
            }
            
        }


        /// <summary>
        /// Update a category in the category list.
        /// </summary>
        /// <example>
        /// <code>
        /// Categories listCats = new Categories();
        /// listCats.Add(1, "jam",Category.CategoryType.Expense);
        /// listCats.UpdateProperties(2, "ham",Category.CategoryType.Saving);
        /// </code>
        /// </example>
        /// <param name="Id">The Id of the category being updated.</param>
        /// <param name="desc">The description of the category being updated.</param>
        /// <param name="type">The type of the category updated to the list.</param>
        public void UpdateProperties(int Id, string desc, CategoryType type)
        {
            int i = _Cats.FindIndex(x => x.Id == Id);
            if (i != -1) 
            {
                _Cats.Insert(i, new Category(Id, desc, type)); 
            };

            UpdateInDatabase(Id, desc, type); //update to database
        }

        /// <summary>
        /// Add a category into the category list.
        /// </summary>
        /// <example>
        /// <code>
        /// Categories listCats=new Categories();
        /// listCats.Add("jam",Category.CategoryType.Expense);
        /// </code>
        /// </example>
        /// <param name="desc">The description of the category being added.</param>
        /// <param name="type">The type of the category added into the list.</param>
        public void Add(String desc, Category.CategoryType type)
        {
            int new_num = 1;
            if (_Cats.Count > 0)
            {
                new_num = (from c in _Cats select c.Id).Max();
                new_num++;
            }
            _Cats.Add(new Category(new_num, desc, type));

            AddCategoriesToDatabase3(new_num, desc, type);

        }

        // ====================================================================
        // Delete category
        /// <summary>
        /// Delete a category from  the list of categories.
        /// </summary>
        /// <example>
        /// <code>
        /// Categories listCats=new Categories();
        /// listCats.Delete(1);
        /// </code>
        /// </example>
        /// <param name="Id">The id of the category you want to delete.</param>
        // ====================================================================
        public void Delete(int Id)
        {
            int i = _Cats.FindIndex(x => x.Id == Id);
            if (i != -1) { _Cats.RemoveAt(i); }; // modified that too
        }

        // ====================================================================
        // Return list of categories
        // Note:  make new copy of list, so user cannot modify what is part of
        //        this instance
        /// <summary>
        /// Create a a copy of the category list. 
        /// </summary>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// Categories cat = new Categories();
        /// List<Category> copy = cat.List();
        /// ]]>
        /// </code>
        /// </example>
        /// <returns>The copy of the category list.</returns>
        // ====================================================================
        public List<Category> List()
        {
            List<Category> newList = new List<Category>();
            foreach (Category category in _Cats)
            {
                newList.Add(new Category(category));
            }
            return newList;
        }

        // ====================================================================
        // read from an XML file and add categories to our categories list
        // ====================================================================
        private void _ReadXMLFile(String filepath)
        {

            // ---------------------------------------------------------------
            // read the categories from the xml file, and add to this instance
            // ---------------------------------------------------------------
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(filepath);

                foreach (XmlNode category in doc.DocumentElement.ChildNodes)
                {
                    String id = (((XmlElement)category).GetAttributeNode("ID")).InnerText;
                    String typestring = (((XmlElement)category).GetAttributeNode("type")).InnerText;
                    String desc = ((XmlElement)category).InnerText;

                    Category.CategoryType type;
                    switch (typestring.ToLower())
                    {
                        case "income":
                            type = Category.CategoryType.Income;
                            break;
                        case "expense":
                            type = Category.CategoryType.Expense;
                            break;
                        case "credit":
                            type = Category.CategoryType.Credit;
                            break;
                        default:
                            type = Category.CategoryType.Savings; // Changed
                            break;
                    }
                    this.Add(new Category(int.Parse(id), desc, type));
                }

            }
            catch (Exception e)
            {
                throw new Exception("ReadXMLFile: Reading XML " + e.Message);
            }

        }


        // ====================================================================
        // write all categories in our list to XML file
        // ====================================================================
        private void _WriteXMLFile(String filepath)
        {
            try
            {
                // create top level element of categories
                XmlDocument doc = new XmlDocument();
                doc.LoadXml("<Categories></Categories>");

                // foreach Category, create an new xml element
                foreach (Category cat in _Cats)
                {
                    XmlElement ele = doc.CreateElement("Category");
                    XmlAttribute attr = doc.CreateAttribute("ID");
                    attr.Value = cat.Id.ToString();
                    ele.SetAttributeNode(attr);
                    XmlAttribute type = doc.CreateAttribute("type");
                    type.Value = cat.Type.ToString();
                    ele.SetAttributeNode(type);

                    XmlText text = doc.CreateTextNode(cat.Description);
                    doc.DocumentElement.AppendChild(ele);
                    doc.DocumentElement.LastChild.AppendChild(text);

                }

                // write the xml to FilePath
                doc.Save(filepath);

            }
            catch (Exception e)
            {
                throw new Exception("_WriteXMLFile: Reading XML " + e.Message);
            }

        }

    }
}

