using System.Collections.Generic;

namespace FrameworkSubGen.BusinessLogic
{
    public class ImportOrder
    {
        public bool SaveBulkXml(List<Entity.ImportOrder> list)
        {
            var coreImport = new CoreImport();
            return coreImport.CoreSaveBulkXml(list, (xml) => DataAccess.ImportOrder.SaveBulkXml(xml));
        }
    }
}
