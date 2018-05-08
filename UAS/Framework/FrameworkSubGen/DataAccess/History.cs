namespace FrameworkSubGen.DataAccess
{
    public static class History
    {
        private const string ClassName = "FrameworkSubGen.DataAccess.History";
        private const string CommandTextSaveBulkXml = "e_History_SaveBulkXml";

        public static bool SaveBulkXml(string xml)
        {
            return DataAccessBase.SaveBulkXml(xml, CommandTextSaveBulkXml, ClassName);
        }
    }
}
