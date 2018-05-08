using FrameworkUAD.Object;

namespace KMPS.MD.Objects
{
    public class ExportFields : ExportFieldsBase
    {
        public ExportFields(string fieldName, string sqltext, bool isUDF, int gdfID)
        {
            FieldName = fieldName;
            SQLText = sqltext;
            isECNUDF = isUDF;
            GroupdatafieldsID = gdfID;
        }
    }
}