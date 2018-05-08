namespace FrameworkUAD.Object
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
