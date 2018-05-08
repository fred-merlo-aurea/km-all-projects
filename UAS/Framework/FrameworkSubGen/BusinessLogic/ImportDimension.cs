using System.Collections.Generic;

namespace FrameworkSubGen.BusinessLogic
{
    public class ImportDimension
    {
        public List<Entity.ImportDimension> Select(int accountId, bool isMergedToUAD)
        {
            List<Entity.ImportDimension> retItem = null;
            retItem = DataAccess.ImportDimension.Select(accountId, isMergedToUAD);
            return retItem;
        }
        public bool SaveBulkXml(List<Entity.ImportDimension> list)
        {
            var coreImport = new CoreImport();
            return coreImport.CoreSaveBulkXml(list, (xml) => DataAccess.ImportDimension.SaveBulkXml(xml));
        }
    }
}
