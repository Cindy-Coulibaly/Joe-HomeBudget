﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using System.Data.SQLite;

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
        public void DeleteCategory(int id, SQLiteConnection db)
        {
            using var cmd = new SQLiteCommand(db);

            if(id > 0)
            {
                cmd.CommandText = $"DELETE FROM categories WHERE Id={id}";
                cmd.ExecuteNonQuery();
            }
        }

        // ====================================================================
        // Add category
        // ====================================================================
        private void Add(Category cat)
        {
            _Cats.Add(cat);
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
            _Cats.RemoveAt(i);
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
                            type = Category.CategoryType.Expense;
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

