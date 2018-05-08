using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Linq;
using Ionic.Zip;

namespace Core.Utilities
{
    public class FileFunctions
    {
        public FileInfo CreateZipFile(FileInfo sourceFile)
        {
            string tempDirectory = sourceFile.Directory + System.DateTime.Now.ToString("yyyyMMdd") + "\\";
            if (Directory.Exists(tempDirectory) == false)
                Directory.CreateDirectory(tempDirectory);
            File.Move(sourceFile.FullName, tempDirectory + sourceFile.Name + sourceFile.Extension);
            string archiveName = sourceFile.Directory + sourceFile.Name.Replace(sourceFile.Extension, "") + ".zip";
            using (ZipFile zip = new ZipFile())
            {
                zip.AddFile(tempDirectory);
                zip.Save(archiveName);
            }

            FileInfo fiCompressed = new FileInfo(archiveName);
            Directory.Delete(tempDirectory, true);
            return fiCompressed;
        }
        public FileInfo CreateZipFile(string directory, string zipFileName)
        {
            ZipFile zip = new ZipFile();
            string archiveName = directory + "\\" + zipFileName + ".zip";
            string[] files = Directory.GetFiles(directory);
            foreach (string filePath in files)
            {
                FileInfo fi = new FileInfo(filePath);
                zip.AddFile(fi.FullName);
            }

            zip.Save(archiveName);
            FileInfo fiCompressed = new FileInfo(archiveName);
             return fiCompressed;
        }
        public FileInfo CreateZipFile(FileInfo sourceFile, EncryptionAlgorithm encrption)
        {
            string tempDirectory = sourceFile.Directory + System.DateTime.Now.ToString("yyyyMMdd") + "\\";
            if (Directory.Exists(tempDirectory) == false)
                Directory.CreateDirectory(tempDirectory);
            File.Move(sourceFile.FullName, tempDirectory + sourceFile.Name + sourceFile.Extension);
            string archiveName = sourceFile.Directory + sourceFile.Name.Replace(sourceFile.Extension, "") + ".zip";
            using (ZipFile zip = new ZipFile())
            {
                zip.Encryption = encrption;
                zip.AddFile(tempDirectory);
                zip.Save(archiveName);
            }

            FileInfo fiCompressed = new FileInfo(archiveName);
            Directory.Delete(tempDirectory, true);
            return fiCompressed;
        }
        public FileInfo CreateZipFile(FileInfo sourceFile, string password)
        {
            string tempDirectory = sourceFile.Directory + System.DateTime.Now.ToString("yyyyMMdd") + "\\";
            if (Directory.Exists(tempDirectory) == false)
                Directory.CreateDirectory(tempDirectory);
            File.Move(sourceFile.FullName, tempDirectory + sourceFile.Name + sourceFile.Extension);
            string archiveName = sourceFile.Directory + sourceFile.Name.Replace(sourceFile.Extension, "") + ".zip";
            using (ZipFile zip = new ZipFile())
            {
                zip.Password = password;
                zip.AddFile(tempDirectory);
                zip.Save(archiveName);
            }

            FileInfo fiCompressed = new FileInfo(archiveName);
            Directory.Delete(tempDirectory, true);
            return fiCompressed;
        }
        public bool ExtractZipFile(FileInfo sourceFile, string destinationPath)
        {
            bool done = false;
            try
            {
                if (Directory.Exists(destinationPath) == false)
                    Directory.CreateDirectory(destinationPath);

                using (ZipFile zip = ZipFile.Read(sourceFile.FullName))
                {
                    zip.ExtractAll(destinationPath);
                    // here, we extract every entry, but we could extract conditionally
                    // based on entry name, size, date, checkbox status, etc.  
                    //foreach (ZipEntry e in zip1)
                    //{
                    //    e.Extract(destinationPath, ExtractExistingFileAction.OverwriteSilently);
                    //}
                }
                done = true;
            }
            catch
            {
                done = false;
            }
            return done;
        }
        public bool ExtractZipFile(FileInfo sourceFile, string destinationPath, string password)
        {
            bool done = false;
            try
            {
                if (Directory.Exists(destinationPath) == false)
                    Directory.CreateDirectory(destinationPath);

                using (ZipFile zip = ZipFile.Read(sourceFile.FullName))
                {
                    foreach (ZipEntry e in zip)
                    {
                        e.ExtractWithPassword(destinationPath, password);
                    }
                }
                done = true;
            }
            catch
            {
                done = false;
            }
            return done;
        }

        public bool CreateTextFile(string fileWithPath, FileMode mode, StreamWriter WriteFile)
        {
            bool fileCreated = true;
            try
            {
                string[] dirs = fileWithPath.Split('\\');
                string root = dirs[0] + @"\";

                for (int i = 1; i < dirs.Length - 1; i++)
                {
                    if (Directory.Exists(root + dirs[i]) == false)
                        Directory.CreateDirectory(root + dirs[i]);

                    root = root + dirs[i] + @"\";
                }

                WriteFile = new StreamWriter(new FileStream(fileWithPath, System.IO.FileMode.Append));
            }
            catch
            {
                fileCreated = false;
            }
            return fileCreated;
        }
        public bool CheckTextFileExists(string fileWithPath)
        {
            if (File.Exists(fileWithPath) == true)
                return true;
            else
                return false;
        }
        public void DeleteFile(string fileWithPath)
        {
            try
            {
                if (File.Exists(fileWithPath) == true)
                    File.Delete(fileWithPath);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void WriteToFile(string text, StreamWriter WriteFile)
        {
            try
            {
                WriteFile.AutoFlush = true;
                WriteFile.WriteLine(text);
                WriteFile.Flush();
                WriteFile.Close();
                System.GC.Collect();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void CreateCSVFromDataTable(DataTable dt, string createFileName, bool deleteExisting = true)
        {
            #region File Setup
            string origFile = createFileName;
            if (deleteExisting == true)
            {
                if (File.Exists(origFile))
                    File.Delete(origFile);
            }
            System.IO.FileInfo file = new System.IO.FileInfo(origFile);
            file.Directory.Create();

            FileConfiguration fileConfig = new FileConfiguration()
            {
                FileColumnDelimiter = "comma",
                IsQuoteEncapsulated = true
            };
            #endregion
            #region Variables
            char delim = ',';
            Core.Utilities.Enums.ColumnDelimiter delimiter = Core.Utilities.Enums.GetColumnDelimiter(fileConfig.FileColumnDelimiter.ToLower());
            if (delimiter == Core.Utilities.Enums.ColumnDelimiter.comma)
                delim = ',';
            else if (delimiter == Core.Utilities.Enums.ColumnDelimiter.semicolon)
                delim = ';';
            else if (delimiter == Core.Utilities.Enums.ColumnDelimiter.tab)
                delim = '\t';
            else if (delimiter == Core.Utilities.Enums.ColumnDelimiter.colon)
                delim = ':';
            else if (delimiter == Core.Utilities.Enums.ColumnDelimiter.tild)
                delim = '~';
            string appendString = "\"" + delim + "\"";
            List<string> origList = new List<string>();

            StringBuilder sbOrigFile = new StringBuilder();
            if (fileConfig.IsQuoteEncapsulated)
                sbOrigFile.Append('"');
            #endregion
            #region Add Headers
            if (dt != null && !File.Exists(origFile))
            {
                foreach (DataColumn drO in dt.Columns)
                {
                    //create the headers
                    if (fileConfig.IsQuoteEncapsulated)
                        sbOrigFile.Append(drO.ColumnName + appendString);
                    else
                        sbOrigFile.Append(drO.ColumnName + delim);
                }
                if (fileConfig.IsQuoteEncapsulated)
                    origList.Add(sbOrigFile.ToString().TrimEnd('"').TrimEnd(delim));
                else
                    origList.Add(sbOrigFile.ToString().TrimEnd(delim));

                
                File.WriteAllLines(origFile, origList);
                origList = new List<string>();
            }
            #endregion
            #region Add Data
            foreach (DataRow otl in dt.Rows)
            {
                foreach (DataColumn dc in dt.Columns)
                {
                    if (Type.GetTypeCode(dc.DataType) == TypeCode.String && dc.ReadOnly == false)
                        otl[dc.ColumnName] = otl[dc.ColumnName].ToString().Replace('\r', ' ').Replace('\n',' ');
                }
            }

            foreach (DataRow otl in dt.Rows)
            {
                origList = new List<string>();
                //create the orginal valid file
                if (fileConfig.IsQuoteEncapsulated)
                    origList.Add('"' + string.Join(appendString, otl.ItemArray.Select(p => p.ToString().Replace('\r', ' ').Replace('\n', ' ').Trim().TrimEnd('\r', '\n').Replace("\"", "")).ToArray()) + '"');//.TrimEnd('"'));//.TrimEnd(delim));
                else
                    origList.Add(string.Join(delim.ToString(), otl.ItemArray.Select(p => p.ToString().Replace('\r', ' ').Replace('\n', ' ').Trim().TrimEnd('\r', '\n')).ToArray()));

                File.AppendAllLines(origFile, origList);
            }
            #endregion
        }
        public void CreateFile(string filePath, string content)
        {
            File.WriteAllText(filePath, content.ToString());
        }
        public void CreateAppendFile(string filePath, string content)
        {
            File.AppendAllText(filePath, content.ToString());
        }
        public string ReadTextFile(string file)
        {
            string line = string.Empty;
            using (StreamReader sr = new StreamReader(file))
            {
                line = sr.ReadToEnd();
            }

            return line;
        }
        public string RemoveNonAsciiCharacters(string dirtyString)
        {
            string asAscii = Encoding.ASCII.GetString(
                Encoding.Convert(
                    Encoding.UTF8,
                    Encoding.GetEncoding(
                        Encoding.ASCII.EncodingName,
                        new EncoderReplacementFallback(string.Empty),
                        new DecoderExceptionFallback()
                        ),
                    Encoding.UTF8.GetBytes(dirtyString)
                )
            );

            return asAscii;
        }
        public bool HasUnicodeChars(string dirty)
        {
            const int MaxAnsiCode = 255;

            return dirty.Any(c => c > MaxAnsiCode);
        }
    
        public static bool IsFileLocked(FileInfo file)
        {
            FileStream stream = null;

            try
            {
                stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            }
            catch (IOException)
            {
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }

            return false;
        }
    }

    [Serializable]
    public class TextFile
    {
        public TextFile() { }
        #region Properties
        public static StreamWriter writeFile;
        public static StreamWriter WriteFile
        {
            get { return writeFile; }
            set { writeFile = value; }
        }
        #endregion

        #region Methods
        public bool CreateTextFile(string fileWithPath, FileMode mode)
        {
            bool fileCreated = true;
            try
            {
                string[] dirs = fileWithPath.Split('\\');
                string root = dirs[0] + @"\";

                for (int i = 1; i < dirs.Length - 1; i++)
                {
                    if (Directory.Exists(root + dirs[i]) == false)
                        Directory.CreateDirectory(root + dirs[i]);

                    root = root + dirs[i] + @"\";
                }

                WriteFile = new StreamWriter(new FileStream(fileWithPath, System.IO.FileMode.Append));
            }
            catch
            {
                fileCreated = false;
            }
            return fileCreated;
        }
        public bool CheckTextFileExists(string fileWithPath)
        {
            if (File.Exists(fileWithPath) == true)
                return true;
            else
                return false;
        }
        public void DeleteFile(string fileWithPath)
        {
            try
            {
                if (File.Exists(fileWithPath) == true)
                    File.Delete(fileWithPath);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void WriteToFile(string text)
        {
            try
            {
                WriteFile.AutoFlush = true;
                WriteFile.WriteLine(text);
                WriteFile.Flush();
                System.GC.Collect();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void CloseFile()
        {
            WriteFile.Close();
        }
        #endregion
    }
}
