namespace FrameworkUAD.Object
{
    public abstract class ExportFieldsBase
    {
        public string FieldName { get; set; }

        public string SQLText { get; set; }

        public bool isECNUDF { get; set; }

        public int GroupdatafieldsID { get; set; }
    }
}
