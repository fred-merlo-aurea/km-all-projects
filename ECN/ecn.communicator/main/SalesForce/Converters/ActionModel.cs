namespace ecn.communicator.main.Salesforce.SF_Pages.Converters
{
    public class ActionModel
    {
        public ActionModel(string name, int order)
        {
            Name = name;
            SortOrder = order;
        }
        public string Name { get; set; }
        public int SortOrder { get; set; }
    }
}