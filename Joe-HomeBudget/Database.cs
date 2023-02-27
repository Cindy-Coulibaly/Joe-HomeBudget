using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Budget
{
    public class Database
    {
        public static SQLiteConnection dbConnection { get { return _connection; } }
        private static SQLiteConnection _connection;

        /// <summary>
        /// Create a database if there is no database
        /// </summary>
        /// <param name="databaseFile">The file where you can create the database</param>
        public static void newDatabase(String databaseFile)
        {
            CloseDatabaseAndReleaseFile();

            _connection = new SQLiteConnection(@$"URI=file:{databaseFile};Foreign Keys=1;");
            dbConnection.Open();
            using var cmd = new SQLiteCommand(dbConnection);

            cmd.CommandText = "DROP TABLE IF EXISTS expenses;";
            cmd.ExecuteNonQuery();
            cmd.CommandText = "DROP TABLE IF EXISTS categories;";
            cmd.ExecuteNonQuery();
            cmd.CommandText = "DROP TABLE IF EXISTS categoryTypes;";
            cmd.ExecuteNonQuery();

            cmd.CommandText = @"CREATE TABLE categoryTypes(
                                Id INTEGER PRIMARY KEY,
                                Description TEXT);";
            cmd.ExecuteNonQuery();

            cmd.CommandText = @"CREATE TABLE categories(
                                Id INTEGER PRIMARY KEY,
                                Description TEXT,
                                TypeId INTEGER,
                                FOREIGN KEY(TypeId) REFERENCES categoryTypes(Id)
                                );";
            cmd.ExecuteNonQuery();

            cmd.CommandText = @"CREATE TABLE expenses(
                                Id INTEGER PRIMARY KEY,
                                Date TEXT,
                                Description TEXT,
                                Amount DOUBLE,
                                CategoryId INTEGER,
                                FOREIGN KEY(CategoryId) REFERENCES categories(Id)
                                );";
            cmd.ExecuteNonQuery();

        }
        /// <summary>
        /// Create a connection to an existing database
        /// </summary>
        /// <param name="filename"> The filename of the database</param>
        public static void existingDatabase(string filename)
        {
            CloseDatabaseAndReleaseFile();

            _connection = new SQLiteConnection(@$"URI=file:{filename};Foreign Keys=1;");
            dbConnection.Open();
        }

        private static void CloseDatabaseAndReleaseFile()
        {
            if (dbConnection != null)
            {
                dbConnection.Close();

                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }
    }
}
