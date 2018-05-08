using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NuGet;
using System.IO;

namespace NuGetUpdater
{
    class Program
    {
        static void Main(string[] args)
        {
            var p = new Program();
            p.GetNuGetPackages();
        }
        public void GetNuGetPackages()
        {
            try
            {
                string path = System.Configuration.ConfigurationManager.AppSettings["NugetOriginal"].ToString();
                string fileName = System.Configuration.ConfigurationManager.AppSettings["NuGetPackageConfig"].ToString();
                string refPath = System.Configuration.ConfigurationManager.AppSettings["ReferenceFolder"].ToString();
                string conPath = System.Configuration.ConfigurationManager.AppSettings["ContentFolder"].ToString();

                #region Download latest NuGet packages
                //Connect to the official package repository
                IPackageRepository repo = PackageRepositoryFactory.Default.CreateRepository("https://packages.nuget.org/api/v2");
                //Initialize the package manager
                var packageManager = new PackageManager(repo, path);
                var file = new PackageReferenceFile(fileName);
                foreach (PackageReference packageReference in file.GetPackageReferences())
                {
                    if (packageReference != null && !string.IsNullOrEmpty(packageReference.Id) && packageReference.Version != null)
                        try
                        {
                            Console.WriteLine("Id={0}, Version={1}", packageReference.Id, packageReference.Version);
                            //Download and unzip the package
                            packageManager.InstallPackage(packageReference.Id, packageReference.Version, true, false);
                        }
                        catch { }
                }
                #endregion
                #region Copy dlls
                //copy just the 45 or 40 dll to folder for referencing in projects
                //-ThirdPartyDlls -- NugetUpdater -- NugetOriginal -- Dlls
                List<string> allfiles = System.IO.Directory.GetFiles(path, "*.dll", System.IO.SearchOption.AllDirectories).ToList();
                List<string> namesToCheck = System.Configuration.ConfigurationManager.AppSettings["CheckName"].ToString().Split(',').ToList();
                allfiles.RemoveAll(x => namesToCheck.Any(y => x.ToLower().Contains(@"\" + y)));//delete the SilverLight dlls
                allfiles.Sort();

                foreach (var dll in allfiles.Where(x => !x.Contains(@"\tools")))
                {
                    var info = new FileInfo(dll);
                    

                        Console.WriteLine("MOVING:{0} - VERSION:{1}", dll, info.Directory.Name);
                        if (info.Directory.Name.Contains("462"))
                            File.Copy(info.FullName, refPath + info.Name, true);
                        else if (info.Directory.Name.Contains("461"))
                            File.Copy(info.FullName, refPath + info.Name, true);
                        else if (info.Directory.Name.Contains("46"))
                            File.Copy(info.FullName, refPath + info.Name, true);
                        else if (info.Directory.Name.Contains("452"))
                            File.Copy(info.FullName, refPath + info.Name, true);
                        else if (info.Directory.Name.Contains("451"))
                            File.Copy(info.FullName, refPath + info.Name, true);
                        else if (info.Directory.Name.Contains("45"))
                            File.Copy(info.FullName, refPath + info.Name, true);
                        else if (info.Directory.Name.Contains("40"))
                            File.Copy(info.FullName, refPath + info.Name, true);
                        else if (info.Directory.Name.Contains("35"))
                            File.Copy(info.FullName, refPath + info.Name, true);
                        else
                            File.Copy(info.FullName, refPath + info.Name, true);

                    
                }
                #endregion
                #region Copy Content
                var di = new DirectoryInfo(path);
                List<DirectoryInfo> allFolders = di.GetDirectories().ToList();
                List<DirectoryInfo> foldersToCopy = new List<DirectoryInfo>();
                foreach (var dir in allFolders)
                    foldersToCopy.AddRange(dir.GetDirectories("Content").ToList());
                int folderCount = foldersToCopy.Count;
                //copy to ContentFolder
                foreach (var dir in foldersToCopy)
                {
                    string dllFolder = dir.Parent.ToString().Replace(".", "").Replace("0", "").Replace("1", "").Replace("2", "").Replace("3", "").Replace("4", "").Replace("5", "").Replace("6", "").Replace("7", "").Replace("8", "").Replace("9", "");
                    DirectoryCopy(dir.FullName, conPath + dllFolder + "\\Content", true);
                }
                #endregion
                #region Copy Tools
                DirectoryInfo toolsDI = new DirectoryInfo(path);
                List<DirectoryInfo> toolsFolders = toolsDI.GetDirectories().ToList();
                List<DirectoryInfo> toolsFoldersToCopy = new List<DirectoryInfo>();
                foreach (var dir in toolsFolders)
                    toolsFoldersToCopy.AddRange(dir.GetDirectories("Tools", SearchOption.AllDirectories).ToList());
                int toolCount = toolsFoldersToCopy.Count;
                //copy to ContentFolder
                foreach (var dir in toolsFoldersToCopy)
                {
                    string dllFolder = dir.Parent.ToString().Replace(".", "").Replace("0", "").Replace("1", "").Replace("2", "").Replace("3", "").Replace("4", "").Replace("5", "").Replace("6", "").Replace("7", "").Replace("8", "").Replace("9", "");
                    DirectoryCopy(dir.FullName, conPath + dllFolder + "\\Tools", true);
                }
                #endregion
            }
            catch (Exception ex) { string msg = ex.StackTrace.ToString(); }
        }


        private void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            // Get the subdirectories for the specified directory.
            var dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();
            // If the destination directory doesn't exist, create it.
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, true);
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                }
            }
        }
    }
}
