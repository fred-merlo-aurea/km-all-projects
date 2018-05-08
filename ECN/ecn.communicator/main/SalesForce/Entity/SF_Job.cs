using System.Collections.Generic;
using ECN_Framework_Entities.Salesforce.Helpers;

namespace ecn.communicator.main.Salesforce.Entity
{
    public class SF_Job
    {
        public string JobID { get; set; }

        public string Operation { get; set; }

        public string ContentType { get; set; }

        public SF_Utilities.SFObject SF_Object { get; set; }

        public string State { get; set; }

        public static string Create(string accessToken, string operation, SF_Utilities.SFObject sfObject)
        {
            return JobUtility.Create(accessToken, operation, SF_Utilities.Fit(sfObject));
        }

        public static bool Close(string accessToken, string jobID)
        {
            return JobUtility.Close(accessToken, jobID);
        }

        public static string AddBatch(string accessToken, string jobID, string xmlString)
        {
            return JobUtility.AddBatch(accessToken, jobID, xmlString);
        }

        public static bool GetBatchState(string accessToken, string jobID, string batchID)
        {
            return JobUtility.GetBatchState(accessToken, jobID, batchID);
        }

        public static Dictionary<string, int> GetBatchResults(string accessToken, string jobID, string batchID)
        {
            return JobUtility.GetBatchResults(accessToken, jobID, batchID);
        }
    }
}