using System.Collections.Generic;
using System.IO;
using System.Net;
using ECN_Framework_Entities.Salesforce.Convertors;

namespace ECN_Framework_Entities.Salesforce.Interfaces
{
    public interface ISFUtilities
    {
        WebRequest CreateQueryRequest(string accessToken, string query, SF_Utilities.Method method, SF_Utilities.ResponseType rt = SF_Utilities.ResponseType.JSON);
        WebRequest CreateUpdateRequest(string accessToken, string json, SF_Utilities.SFObject sfObject, string objectID, SF_Utilities.ResponseType rt = SF_Utilities.ResponseType.JSON);
        StreamReader GetNextURL(string url);
        void LogWebException(WebException ex, string requestURL);
        bool ProceedRequest(WebRequest request);
        string ProceedJobRequest(WebRequest request);
        bool Update(string accessToken, SalesForceBase obj, SF_Utilities.SFObject objectType, string id);
        WebRequest CreateWebRequest(string url);
        T ReadToken<T>(WebRequest request);
        void WriteToLog(string text);
        WebRequest GetBatchResults(string accessToken, string jobId, string batchId);
        WebRequest CreateNewJob(string accessToken, SF_Utilities.SFObject sfObject, string operation);
        WebRequest GetBatchState(string accessToken, string jobId, string batchId);
        WebRequest AddBatchToJob(string accessToken, string jobId, string xmlString);
        WebRequest CloseJob(string accessToken, string jobId);
        string GetXmlForOutJob(Dictionary<string, string> dicLeads);
        T GetSingle<T>(string accessToken, string where, EntityConverterBase converter) where T : new();
        bool Insert(string accessToken, object obj, SF_Utilities.SFObject objectType);
    }
}
