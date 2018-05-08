using FrameworkUAD.Object;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Transactions;

namespace FrameworkUAD.BusinessLogic
{
    public class CodeSheet
    {
        public FrameworkUAD.Object.Enums.Entity entity = FrameworkUAD.Object.Enums.Entity.CodeSheet;
        List<UADError> errorList = new List<UADError>();

        public bool ExistsByResponseGroupIDValue(int codesheetID, int responseGroupID, string responseValue, KMPlatform.Object.ClientConnections client)
        {
            bool exists = false;
            exists = DataAccess.CodeSheet.ExistsByResponseGroupIDValue(codesheetID, responseGroupID, responseValue, client);
            return exists;
        }
        public List<Entity.CodeSheet> Select(KMPlatform.Object.ClientConnections client)
        {
            List<Entity.CodeSheet> x = null;
            x = DataAccess.CodeSheet.Select(client).ToList();
            return x;
        }
        public List<Entity.CodeSheet> Select(int pubID, KMPlatform.Object.ClientConnections client)
        {
            List<Entity.CodeSheet> x = null;
            x = DataAccess.CodeSheet.Select(pubID,client).ToList();
            return x;
        }
        public Entity.CodeSheet SelectByID(int codeSheetID, KMPlatform.Object.ClientConnections client)
        {
            Entity.CodeSheet x = DataAccess.CodeSheet.SelectByID(codeSheetID, client);
            return x;
        }

        public List<Entity.CodeSheet> SelectByResponseGroupID(int responseGroupID, KMPlatform.Object.ClientConnections client)
        {
            List<Entity.CodeSheet> x = null;
            x = DataAccess.CodeSheet.SelectByResponseGroupID(responseGroupID, client).ToList();
            return x;
        }

        public DataSet SelectBySearch(int responseGroupID, string name, string searchCriteria, int currentPage, int pageSize, string sortDirection, string sortColumn, KMPlatform.Object.ClientConnections client)
        {
            DataSet x = null;
            x = DataAccess.CodeSheet.SelectBySearch(responseGroupID, name, searchCriteria, currentPage, pageSize, sortDirection, sortColumn, client);
            return x;
        }

        public int Save(Entity.CodeSheet x, KMPlatform.Object.ClientConnections client)
        {
            FrameworkUAD.Object.Enums.Method method = FrameworkUAD.Object.Enums.Method.Save;

            if (ExistsByResponseGroupIDValue(x.CodeSheetID, x.ResponseGroupID, x.ResponseValue, client))
            {
                errorList.Add(new UADError(entity, method, "Value already exists."));
            }

            if (errorList.Count > 0)
            {
                throw new UADException(errorList);
            }

            using (TransactionScope scope = new TransactionScope())
            {
                x.CodeSheetID = DataAccess.CodeSheet.Save(x, client);
                scope.Complete();
            }

            return x.CodeSheetID;
        }

        public bool Delete(KMPlatform.Object.ClientConnections client, int codeSheetID)
        {
            bool delete = false;
            using (TransactionScope scope = new TransactionScope())
            {
                delete = DataAccess.CodeSheet.Delete(client, codeSheetID);
                scope.Complete();
            }

            return delete;
        }

        public bool CodeSheetValidation(int sourceFileID, string processCode, KMPlatform.Object.ClientConnections client)
        {
            return DataAccess.CodeSheet.CodeSheetValidation(sourceFileID, processCode, client);
        }
        public void CreateNoValueRespones(KMPlatform.Object.ClientConnections client)
        {
            DataAccess.CodeSheet.CreateNoValueRespones(client);
        }
        public bool CodeSheetValidation_Delete(int sourceFileID, string processCode, KMPlatform.Object.ClientConnections client)
        {
            return DataAccess.CodeSheet.CodeSheetValidation_Delete(sourceFileID, processCode, client);
        }
        #region File Validator methods
        public bool FileValidator_CodeSheetValidation(int sourceFileID, string processCode, KMPlatform.Object.ClientConnections client)
        {
            return DataAccess.CodeSheet.FileValidator_CodeSheetValidation(sourceFileID, processCode, client);
        }
        #endregion
    }
}
