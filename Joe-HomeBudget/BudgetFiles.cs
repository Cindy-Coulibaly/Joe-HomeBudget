using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

// ============================================================================
// (c) Sandy Bultena 2018
// * Released under the GNU General Public License
// ============================================================================

namespace Budget
{

    /// <summary>
    /// manage the files used in the Budget project and check if they exist.
    /// </summary>
    public class BudgetFiles
    {
        private static String DefaultSavePath = @"Budget\";
        private static String DefaultAppData = @"%USERPROFILE%\AppData\Local\";

        // ====================================================================
        /// <summary>
        /// verify that the name of the file exists, or set the default file, and check if
        /// is it readable.
        /// </summary>
        /// <param name="FilePath">The file path of the budget file.</param>
        /// <param name="DefaultFileName">The file name of the budget file.</param>
        /// <returns>The file path of the budget file if it exits.</returns>
        /// <exception cref="FileNotFoundException">throw when the file path doesn't exist.</exception>
        /// <example>
        /// <code>
        /// string filepath="./test.budget";
        /// string defaultFileName="test";
        /// string validPath=VerifyReadFromFileName(filepath,defaultFileName);
        /// </code>
        /// </example>
        // throws System.IO.FileNotFoundException if file does not exist
        // ====================================================================
        public static String VerifyReadFromFileName(String FilePath, String DefaultFileName)
        {

            // ---------------------------------------------------------------
            // if file path is not defined, use the default one in AppData
            // ---------------------------------------------------------------
            if (FilePath == null)
            {
                FilePath = Environment.ExpandEnvironmentVariables(DefaultAppData + DefaultSavePath + DefaultFileName);
            }

            // ---------------------------------------------------------------
            // does FilePath exist?
            // ---------------------------------------------------------------
            if (!File.Exists(FilePath))
            {
                throw new FileNotFoundException("ReadFromFileException: FilePath (" + FilePath + ") does not exist");
            }

            // ----------------------------------------------------------------
            // valid path
            // ----------------------------------------------------------------
            return FilePath;

        }

        // ====================================================================
        /// <summary>
        /// verify that the name of the file exists, or set the default file, and check
        /// if you can edit the file.
        /// </summary>
        /// <param name="FilePath">The file path of the expenses file.</param>
        /// <param name="DefaultFileName">The file name of the expenses file.</param>
        /// <returns>The file path if the file is writable specified by the user.</returns>
        /// <exception cref="Exception">if the filepath doesn't exist or if the file is read-only</exception>
        /// <example>
        /// <code>
        /// string filepath="./test.budget";
        /// string defaultFileName="test";
        /// string validWritePath=VerifyWriteToFileName(filepath,defaultFileName);
        /// </code>
        /// </example>
        // ====================================================================

        public static String VerifyWriteToFileName(String FilePath, String DefaultFileName)
        {
            // ---------------------------------------------------------------
            // if the directory for the path was not specified, then use standard application data
            // directory
            // ---------------------------------------------------------------
            if (FilePath == null)
            {
                // create the default appdata directory if it does not already exist
                String tmp = Environment.ExpandEnvironmentVariables(DefaultAppData);
                if (!Directory.Exists(tmp))
                {
                    Directory.CreateDirectory(tmp);
                }

                // create the default Budget directory in the appdirectory if it does not already exist
                tmp = Environment.ExpandEnvironmentVariables(DefaultAppData + DefaultSavePath);
                if (!Directory.Exists(tmp))
                {
                    Directory.CreateDirectory(tmp);
                }

                FilePath = Environment.ExpandEnvironmentVariables(DefaultAppData + DefaultSavePath + DefaultFileName);
            }

            // ---------------------------------------------------------------
            // does directory where you want to save the file exist?
            // ... this is possible if the user is specifying the file path
            // ---------------------------------------------------------------
            String folder = Path.GetDirectoryName(FilePath);
            String delme = Path.GetFullPath(FilePath);
            if (!Directory.Exists(folder))
            {
                throw new Exception("SaveToFileException: FilePath (" + FilePath + ") does not exist");
            }

            // ---------------------------------------------------------------
            // can we write to it?
            // ---------------------------------------------------------------
            if (File.Exists(FilePath))
            {
                FileAttributes fileAttr = File.GetAttributes(FilePath);
                if ((fileAttr & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                {
                    throw new Exception("SaveToFileException:  FilePath(" + FilePath + ") is read only");
                }
            }

            // ---------------------------------------------------------------
            // valid file path
            // ---------------------------------------------------------------
            return FilePath;

        }



    }
}
