using System;
using System.Collections.Generic;
using System.Net;
using System.Xml;
using ECN_Framework_Entities.Salesforce.Interfaces;

namespace ECN_Framework_Entities.Salesforce.Helpers
{
    public class JobUtility
    {
        private const string IdTag = "id";
        private const string StateTag = "state";
        private const string SuccessTag = "success";

        private const string ClosedState = "closed";
        private const string CompletedState = "completed";
        private const string FailedState = "failed";

        private const string True = "true";
        private const string Success = "success";
        private const string Fail = "fail";


        private static ISFUtilities _sFUtilities;

        static JobUtility()
        {
            _sFUtilities = new SFUtilitiesAdapter();
        }

        public static void InitUtilities(ISFUtilities utilities)
        {
            _sFUtilities = utilities;
        }

        public static string Create(string accessToken, string operation, SF_Utilities.SFObject sfObject)
        {
            var req = _sFUtilities.CreateNewJob(accessToken, sfObject, operation);

            var jobId = SafeWebExecute(
                req,
                () =>
                {
                    var xDoc = ProccedAsXml(req, true);
                    var idNode = GetNodeByTagName(xDoc, IdTag);

                    return idNode.InnerText;
                });

            return jobId ?? string.Empty;
        }

        public static bool Close(string accessToken, string jobId)
        {
            var req = _sFUtilities.CloseJob(accessToken, jobId);

            return SafeWebExecute(req,
                () =>
                {
                    var xDoc = ProccedAsXml(req, false);
                    var stateNode = GetNodeByTagName(xDoc, StateTag);

                    return stateNode.InnerText.ToLower().Equals(ClosedState);
                });
        }

        public static string AddBatch(string accessToken, string jobId, string xmlString)
        {
            var req = _sFUtilities.AddBatchToJob(accessToken, jobId, xmlString);

            var batchId = SafeWebExecute(
                req,
                () =>
                {
                    var xDoc = ProccedAsXml(req, true);
                    var idNode = GetNodeByTagName(xDoc, IdTag);

                    return idNode.InnerText;
                });

            return batchId ?? string.Empty;
        }

        public static bool GetBatchState(string accessToken, string jobId, string batchId)
        {
            var req = _sFUtilities.GetBatchState(accessToken, jobId, batchId);

            return SafeWebExecute(
                req,
                () =>
                {
                    var xDoc = ProccedAsXml(req, false);
                    var stateNode = GetNodeByTagName(xDoc, StateTag);

                    return stateNode.InnerText.ToLower().Equals(CompletedState) ||
                           stateNode.InnerText.ToLower().Contains(FailedState);
                });
        }

        public static Dictionary<string, int> GetBatchResults(string accessToken, string jobId, string batchId)
        {
            var req = _sFUtilities.GetBatchResults(accessToken, jobId, batchId);

            return SafeWebExecute(
                req,
                () =>
                {
                    var xDoc = ProccedAsXml(req, false);

                    var resultDict = new Dictionary<string, int>();
                    var success = 0;
                    var fail = 0;

                    foreach (XmlNode node in xDoc.DocumentElement.GetElementsByTagName(SuccessTag))
                    {
                        if (node.InnerText.ToLower().Equals(True))
                        {
                            success++;
                        }
                        else
                        {
                            fail++;
                        }
                    }
                    resultDict.Add(Success, success);
                    resultDict.Add(Fail, fail);

                    return resultDict;
                });
        }

        private static TResult SafeWebExecute<TResult>(WebRequest request, Func<TResult> func)
        {
            return SFUtilitiesAdapter.SafeExecute<WebException, TResult>(
                func,
                e => _sFUtilities.LogWebException(e, request.RequestUri.ToString()));
        }

        private static XmlNode GetNodeByTagName(XmlDocument document, string tagName)
        {
            return document.DocumentElement.GetElementsByTagName(tagName)[0];
        }

        private static XmlDocument ProccedAsXml(WebRequest request, bool writeLog)
        {
            var result = _sFUtilities.ProceedJobRequest(request);
            if (writeLog)
            {
                _sFUtilities.WriteToLog(result);
            }
            var xDoc = new XmlDocument();
            xDoc.LoadXml(result);

            return xDoc;
        }
    }
}
