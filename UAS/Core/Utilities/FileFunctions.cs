using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Linq;
using System.IO.Compression;
using KM.Common.Import;
using CommonEnums = KM.Common.Enums;

namespace Core_AMS.Utilities
{
    public class FileFunctions
    {
        public FileInfo CreateZipFile(FileInfo sourceFile)
        {
            
            //string tempDirectory = sourceFile.Directory + "\\" + System.DateTime.Now.ToString("yyyyMMdd") + "\\";
            //if (Directory.Exists(tempDirectory) == false)
            //    Directory.CreateDirectory(tempDirectory);
            //File.Move(sourceFile.FullName, tempDirectory + sourceFile.Name);
            string archiveName = sourceFile.Directory + "\\" + sourceFile.Name.Replace(sourceFile.Extension, "") + ".zip";
            //using (ZipFile zip = new ZipFile())
            //{
            //    zip.AddFile(tempDirectory);
            //    zip.Save(archiveName);
            //}

            string zipPath = archiveName;

            using (ZipArchive archive = ZipFile.Open(zipPath, ZipArchiveMode.Create))
            {
                archive.CreateEntryFromFile(sourceFile.FullName, sourceFile.Name);
            } 

            FileInfo fiCompressed = new FileInfo(archiveName);
            File.Delete(sourceFile.FullName);
            //Directory.Delete(tempDirectory, true);
            return fiCompressed;
        }
        public FileInfo CreateZipFile(string directory, string zipFileName)
        {
            Ionic.Zip.ZipFile zip = new Ionic.Zip.ZipFile();
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
        public FileInfo CreateZipFile(FileInfo sourceFile, Ionic.Zip.EncryptionAlgorithm encrption)
        {
            string tempDirectory = sourceFile.Directory + System.DateTime.Now.ToString("yyyyMMdd") + "\\";
            if (Directory.Exists(tempDirectory) == false)
                Directory.CreateDirectory(tempDirectory);
            File.Move(sourceFile.FullName, tempDirectory + sourceFile.Name + sourceFile.Extension);
            string archiveName = sourceFile.Directory + sourceFile.Name.Replace(sourceFile.Extension, "") + ".zip";
            using (Ionic.Zip.ZipFile zip = new Ionic.Zip.ZipFile())
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
            using (Ionic.Zip.ZipFile zip = new Ionic.Zip.ZipFile())
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

                using (Ionic.Zip.ZipFile zip = Ionic.Zip.ZipFile.Read(sourceFile.FullName))
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

                using (Ionic.Zip.ZipFile zip = Ionic.Zip.ZipFile.Read(sourceFile.FullName))
                {
                    foreach (Ionic.Zip.ZipEntry e in zip)
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
            var delimiter = CommonEnums.GetDelimiterSymbol(fileConfig.FileColumnDelimiter.ToLower()).GetValueOrDefault(',');
            var appendString = string.Format("\"{0}\"", delimiter );
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
                        sbOrigFile.Append(drO.ColumnName + delimiter);
                }
                if (fileConfig.IsQuoteEncapsulated)
                    origList.Add(sbOrigFile.ToString().TrimEnd('"').TrimEnd(delimiter));
                else
                    origList.Add(sbOrigFile.ToString().TrimEnd(delimiter));

                
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
                    origList.Add(
                        string.Join(
                            delimiter.ToString(),
                            otl.ItemArray
                                .Select(p => p.ToString()
                                    .Replace('\r', ' ').Replace('\n', ' ')
                                    .Trim().TrimEnd('\r', '\n'))
                                .ToArray()
                            ));

                File.AppendAllLines(origFile, origList);
            }
            #endregion
        }
        public void CreateTSVFromDataTable(DataTable dataTable, string createFileName, bool deleteExisting = true)
        {
            // File Setup
            var origFile = createFileName;
            if (deleteExisting)
            {
                if (File.Exists(origFile))
                    File.Delete(origFile);
            }
            var file = new FileInfo(origFile);
            if (file.Directory == null)
            {
                throw new InvalidOperationException("Folder didn't found");
            }
            file.Directory.Create();

            var fileConfig = new FileConfiguration()
            {
                FileColumnDelimiter = "tab",
                IsQuoteEncapsulated = false
            };


            var delim = CommonEnums.GetDelimiterSymbol(fileConfig.FileColumnDelimiter).GetValueOrDefault(',');
            var appendString = "\"" + delim + "\"";
            var origList = new List<string>();
            var sbOrigFile = new StringBuilder();

            if (fileConfig.IsQuoteEncapsulated)
                sbOrigFile.Append('"');

            // Add Headers
            if (dataTable != null && !File.Exists(origFile))
            {
                foreach (DataColumn drO in dataTable.Columns)
                {
                    //create the headers
                    sbOrigFile.Append(fileConfig.IsQuoteEncapsulated 
                        ? drO.ColumnName + appendString 
                        : drO.ColumnName + delim);
                }
                origList.Add(fileConfig.IsQuoteEncapsulated
                    ? sbOrigFile.ToString().TrimEnd('"').TrimEnd(delim)
                    : sbOrigFile.ToString().TrimEnd(delim));


                File.WriteAllLines(origFile, origList);
            }

            // Add Data
            foreach (DataRow otl in dataTable.Rows)
            {
                foreach (DataColumn dc in dataTable.Columns)
                {
                    if (Type.GetTypeCode(dc.DataType) == TypeCode.String && dc.ReadOnly == false)
                    {
                        otl[dc.ColumnName] = otl[dc.ColumnName].ToString().Replace('\r', ' ').Replace('\n', ' ');
                    }
                }
            }

            foreach (DataRow otl in dataTable.Rows)
            {
                var tokens = otl.ItemArray
                    .Select(p => p.ToString().Replace('\r', ' ').Replace('\n', ' ').Trim().TrimEnd('\r', '\n'))
                    .ToArray();
                var line = fileConfig.IsQuoteEncapsulated
                    ? string.Join("\"", appendString, tokens, "\"")
                    : string.Join(delim.ToString(), tokens);

                File.AppendAllLines(origFile, new List<string> { line });
            }
        }
        public void CreateCSVFromList<T>(IList<T> list, string createFileName, bool deleteExisting = true)
        {
            DataTable dt = DataTableFunctions.ToDataTable(list);
            CreateCSVFromDataTable(dt, createFileName, deleteExisting);
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
                stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None);
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
        public static bool IsFileLocked(string filePath)
        {
            FileStream stream = null;
            FileInfo file = new FileInfo(filePath);
            try
            {
                stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None);
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

        public void WriteToFile(string text, StreamWriter streamWriter)
        {
            if (streamWriter == null)
            {
                throw new ArgumentNullException(nameof(streamWriter));
            }

            try
            {
                streamWriter.AutoFlush = true;
                streamWriter.WriteLine(text);
            }
            finally
            {
                streamWriter.Close();
            }
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
    }
}
