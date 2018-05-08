using System.Xml.Serialization;

namespace ecn.communicator.SalesForcePartner.Interfaces
{
    public interface ISforceService
    {
        SaveResult[] create([XmlElementAttribute("sObjects")] sObject[] sObjects);
        QueryResult query(string queryString);
    }
}
