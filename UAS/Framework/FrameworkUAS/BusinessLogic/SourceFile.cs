using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace FrameworkUAS.BusinessLogic
{
    public class SourceFile
    {
        public  List<Entity.SourceFile> Select(bool includeCustomProperties = false)
        {
            List<Entity.SourceFile> x = null;
            x = DataAccess.SourceFile.Select().ToList();

            if (includeCustomProperties)
            {
                SetCustomProperties(x);
            }
            return x;
        }
        public List<Entity.SourceFile> Select(List<int> sourceFileIds, bool includeCustomProperties = false)
        {
            List<Entity.SourceFile> x = null;
            string sfIds = string.Join(",", sourceFileIds);
            x = DataAccess.SourceFile.Select(sfIds).ToList();

            if (includeCustomProperties)
            {
                SetCustomProperties(x);
            }
            return x;
        }
        public List<Entity.SourceFile> SelectByFileType(int clientId, FrameworkUAD_Lookup.Enums.FileTypes fileType, bool includeCustomProperties = false)
        {
            List<Entity.SourceFile> x = null;

            x = DataAccess.SourceFile.SelectByFileType(clientId, fileType.ToString().Replace("_", " ")).ToList();

            if (includeCustomProperties)
            {
                SetCustomProperties(x);
            }
            return x;
            
        }
        public  List<Entity.SourceFile> Select(bool includeCustomProperties = false, bool isDeleted = false)
        {
            List<Entity.SourceFile> x = null;
            x = DataAccess.SourceFile.Select(isDeleted).ToList();

            if (includeCustomProperties)
            {
                SetCustomProperties(x);
            }
            return x;
        }

        //public  List<Entity.SourceFile> Select(string clientName, bool includeCustomProperties = false, bool isDeleted = false)
        //{
        //    List<Entity.SourceFile> x = null;
        //    x = DataAccess.SourceFile.Select(clientName).Where(z => z.IsDeleted == isDeleted).ToList();

        //    if (includeCustomProperties)
        //    {
        //        foreach (var c in x)
        //        {
        //            FieldMapping fm = new FieldMapping();
        //            if (c != null)
        //                c.FieldMappings = fm.Select(c.SourceFileID, true).ToList();
        //        }

        //    }
        //    return x;
        //}

        public  List<Entity.SourceFile> Select(int clientID, bool includeCustomProperties = false, bool isDeleted = false)
        {
            List<Entity.SourceFile> x = null;
            x = DataAccess.SourceFile.Select(clientID,isDeleted).ToList();

            if (includeCustomProperties)
            {
                SetCustomProperties(x);
            }
            return x;
        }
        public  List<Entity.SourceFile> Select(int clientID, bool includeCustomProperties = false)
        {
            List<Entity.SourceFile> x = null;
            x = DataAccess.SourceFile.Select(clientID).ToList();

            if (includeCustomProperties)
            {
                SetCustomProperties(x);
            }
            return x;
        }
        public List<Entity.SourceFile> SelectSpecialFiles(bool includeCustomProperties = false)
        {
            List<Entity.SourceFile> x = null;
            x = DataAccess.SourceFile.Select().Where(y => y.IsSpecialFile == true).ToList();

            if (includeCustomProperties)
            {
                SetCustomProperties(x);
            }
            return x;
        }
        public List<Entity.SourceFile> SelectSpecialFiles(int clientID, bool includeCustomProperties = false)
        {
            List<Entity.SourceFile> x = null;
            x = DataAccess.SourceFile.Select(clientID).Where(y => y.IsSpecialFile == true).ToList();

            if (includeCustomProperties)
            {
                SetCustomProperties(x);
            }
            return x;
        }

        public  Entity.SourceFile Select(int clientId, string fileName, bool includeCustomProperties = false)
        {
            Entity.SourceFile x = null;
            x = DataAccess.SourceFile.Select(clientId, fileName);

            if (includeCustomProperties)
            {
                x = GetCustomProperties(x);
            }

            return x;
        }
        public Entity.SourceFile SelectSourceFileID(int sourceFileID, bool includeCustomProperties = false)
        {
            Entity.SourceFile x = null;
            x = DataAccess.SourceFile.SelectSourceFileID(sourceFileID);

            if (includeCustomProperties)
            {
                x = GetCustomProperties(x);
            }

            return x;
        }
   
        public int Save(Entity.SourceFile x, bool defaultRules = true)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                x.SourceFileID = DataAccess.SourceFile.Save(x, defaultRules);
                scope.Complete();
            }

            return x.SourceFileID;
        }
        public  int Delete(int SourceFileID, int ClientID)
        {
            int res = 0;
            using (TransactionScope scope = new TransactionScope())
            {
                res = DataAccess.SourceFile.Delete(SourceFileID, ClientID);
                scope.Complete();
            }

            return res;
        }
        public int Delete(int SourceFileID, bool IsDeleted, int UserID)
        {
            int res = 0;
            using (TransactionScope scope = new TransactionScope())
            {
                res = DataAccess.SourceFile.Delete(SourceFileID, IsDeleted, UserID);
                scope.Complete();
            }

            return res;
        }
        public bool IsFileNameUnique(int clientId, string fileName)
        {
            int lastIndex = fileName.LastIndexOf('.');
            string fName = fileName;
            if (lastIndex > 0)
            {
                int length = fileName.Length - 1;
                int take = length - lastIndex;
                fName = fileName.Substring(0, length - take);
            }
            return DataAccess.SourceFile.IsFileNameUnique(clientId, fName);
        }
        private void SetCustomProperties(List<Entity.SourceFile> sfList)
        {
            RuleSet rs = new RuleSet();
            FrameworkUAD_Lookup.BusinessLogic.Code cWrk = new FrameworkUAD_Lookup.BusinessLogic.Code();
            var codes = cWrk.Select(FrameworkUAD_Lookup.Enums.CodeType.Database_File);
            sfList.ForEach(sf =>
            {
                try
                {
                    FieldMapping fm = new FieldMapping();
                    if (sf != null)
                    {
                        if (codes.Exists(e => e.CodeId == sf.DatabaseFileTypeId))
                        {
                            var code = codes.Single(s => s.CodeId == sf.DatabaseFileTypeId);
                            var fileTypeEnum = FrameworkUAD_Lookup.Enums.GetDatabaseFileType(code.CodeName);
                            sf.FileTypeEnum = fileTypeEnum;
                        }
                        sf.FieldMappings = fm.HashSetSelect(sf.SourceFileID, true);
                        if (sf.RuleSets == null) sf.RuleSets = new HashSet<Object.RuleSet>();

                        var fileRS = rs.GetRuleSetsForSourceFile(sf.SourceFileID, true);
                        foreach (var r in fileRS)
                            sf.RuleSets.Add(r);
                    }
                }
                catch(Exception ex)
                {
                    string msg = Core_AMS.Utilities.StringFunctions.FormatException(ex);
                }
            });
        }
        private FrameworkUAS.Entity.SourceFile GetCustomProperties(FrameworkUAS.Entity.SourceFile sourceFile)
        {
            //FIXME: Select should go by ClientId
            FieldMapping fm = new FieldMapping();
            RuleSet rs = new RuleSet();
            if (sourceFile != null)
            {
                FrameworkUAD_Lookup.BusinessLogic.Code cWrk = new FrameworkUAD_Lookup.BusinessLogic.Code();
                var codes = cWrk.Select(FrameworkUAD_Lookup.Enums.CodeType.Database_File);
                if (codes.Exists(e => e.CodeId == sourceFile.DatabaseFileTypeId))
                {
                    var code = codes.Single(s => s.CodeId == sourceFile.DatabaseFileTypeId);
                    var fileTypeEnum = FrameworkUAD_Lookup.Enums.GetDatabaseFileType(code.CodeName);
                    sourceFile.FileTypeEnum = fileTypeEnum;
                }
                sourceFile.FieldMappings = fm.HashSetSelect(sourceFile.SourceFileID, true);
                var fileRS = rs.GetRuleSetsForSourceFile(sourceFile.SourceFileID, true);
                if (sourceFile.RuleSets == null) sourceFile.RuleSets = new HashSet<Object.RuleSet>();
                foreach (var r in fileRS)
                    sourceFile.RuleSets.Add(r);
            }

            return sourceFile;
        }

        public List<Entity.SourceFile> SelectPaging(int clientID, int currentPage, int pageSize, int serviceID, string type, int pubID, int fileType, string filename, string sortField, string sortDirection)
        {
            List<Entity.SourceFile> y = null;

            y = DataAccess.SourceFile.SelectPaging(clientID, currentPage, pageSize, serviceID, type, pubID, fileType, filename, sortField, sortDirection).ToList();
            return y;
        }
        public int SelectPagingCount(int clientID, int serviceID, string type, int pubID, int fileType, string filename)
        {
            int res = 0;            

            using (TransactionScope scope = new TransactionScope())
            {
                res = DataAccess.SourceFile.SelectPagingCount(clientID, serviceID, type, pubID, fileType, filename);
                scope.Complete();
            }

            return res;
        }
    }
}
