using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_BusinessLayer.Collector
{
    [Serializable]
    public class Survey
    {
        public static ECN_Framework_Common.Objects.Enums.Entity Entity = ECN_Framework_Common.Objects.Enums.Entity.Survey;

        public static List<ECN_Framework_Entities.Collector.Survey> GetByCustomerID(int customerID, KMPlatform.Entity.User user)
        {
            List<ECN_Framework_Entities.Collector.Survey> itemList = new List<ECN_Framework_Entities.Collector.Survey>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                itemList = ECN_Framework_DataLayer.Collector.Survey.GetByCustomerID(customerID, user.UserID);
                scope.Complete();
            }
            return itemList;
        }

        public static ECN_Framework_Entities.Collector.Survey GetBySurveyID(int surveyID, KMPlatform.Entity.User user)
        {
            ECN_Framework_Entities.Collector.Survey item = new ECN_Framework_Entities.Collector.Survey();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                item = ECN_Framework_DataLayer.Collector.Survey.GetBySurveyID(surveyID);
                scope.Complete();
            }
            return item;
        }

        public static void AddUserToSurveyGroup(int surveyID, string username, string IPAddress, KMPlatform.Entity.User user)
        {
            ECN_Framework_Entities.Collector.Survey s = GetBySurveyID(surveyID, user);
            StringBuilder xmlUDF = new StringBuilder("");
            xmlUDF.Append("<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><XML></XML>");
            StringBuilder xmlProfile = new StringBuilder("");
            xmlProfile.Append("<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><XML><Emails>");
            xmlProfile.Append("<emailaddress>" + username + "</emailaddress>");
            xmlProfile.Append("<notes>" + IPAddress + "</notes>");
            xmlProfile.Append("</Emails></XML>");
            ECN_Framework_BusinessLayer.Communicator.EmailGroup.ImportEmails_NoAccessCheck(user, s.CustomerID, s.GroupID, xmlProfile.ToString(), xmlUDF.ToString(), "HTML", "S", false, "", "ECN_Framework_BusinessLayer.collector.Survey.AddUserToSurveyGroup");
            
        }

        public static List<ECN_Framework_Entities.Collector.Survey> GetByGroupID(int GroupID, int CustomerID)
        {
            List<ECN_Framework_Entities.Collector.Survey> retList = new List<ECN_Framework_Entities.Collector.Survey>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                retList = ECN_Framework_DataLayer.Collector.Survey.GetByGroupID(GroupID, CustomerID);
                scope.Complete();
            }
            return retList;
        }

        public static bool HasSurvey(int groupID)
        {
            bool result = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                result = ECN_Framework_DataLayer.Collector.Survey.HasSurvey(groupID);
                scope.Complete();
            }
            return result;
        }

        public static bool HasResponses(int emailID, int surveyID)
        {
            bool result = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {

                scope.Complete();
            }
            return result;
        }

        public static void Delete(int surveyID, KMPlatform.Entity.User user)
        {
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                ECN_Framework_DataLayer.Collector.Survey.Delete(surveyID, user.UserID);
                scope.Complete();
            }
        }

        public static void Copy(int NewsurveyID, int OldsurveyID, KMPlatform.Entity.User user)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_DataLayer.Collector.Survey.Copy(NewsurveyID, OldsurveyID);
                scope.Complete();
            }
        }

        public static void DeleteResponses(int surveyID, KMPlatform.Entity.User user)
        {
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                ECN_Framework_DataLayer.Collector.Survey.DeleteResponses(surveyID, user.UserID);
                scope.Complete();
            }
        }

        public static void Validate(ECN_Framework_Entities.Collector.Survey survey)
        {
            ECN_Framework_Common.Objects.Enums.Method Method = ECN_Framework_Common.Objects.Enums.Method.Validate;
            List<ECNError> errorList = new List<ECNError>();

            if (survey.SurveyTitle.Trim() == string.Empty)
                errorList.Add(new ECNError(Entity, Method, "SurveyTitle cannot be empty"));
            else if (Exists(survey.SurveyID, survey.SurveyTitle, survey.CustomerID))
                errorList.Add(new ECNError(Entity, Method, "SurveyTitle already exists"));

            if (errorList.Count > 0)
            {
                throw new ECNException(errorList);
            }
        }

        public static bool Exists(int surveyID, string surveyTitle, int customerID)
        {

            bool exists = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.Collector.Survey.Exists(surveyID, surveyTitle, customerID);
                scope.Complete();
            }
            return exists;
        }

        public static void Save(ECN_Framework_Entities.Collector.Survey Survey, KMPlatform.Entity.User user)
        {
            Survey.SurveyTitle = Survey.SurveyTitle.Replace("'", "''");
            Validate(Survey);

            if (Survey.SurveyID < 1)
            {
                ECN_Framework_Entities.Communicator.Group group = new ECN_Framework_Entities.Communicator.Group();
                group.GroupName = Survey.SurveyTitle + "_Group";
                group.GroupDescription = Survey.SurveyTitle + "_Group";
                group.FolderID = 0;
                group.PublicFolder = 0;
                group.IsSeedList = false;
                group.AllowUDFHistory = "N";
                group.OwnerTypeCode = "customer";
                group.CreatedUserID = user.UserID;
                group.CustomerID = Survey.CustomerID;
                ECN_Framework_BusinessLayer.Communicator.Group.Save(group, user);
                Survey.GroupID = group.GroupID;
            }
            Survey.SurveyID= ECN_Framework_DataLayer.Collector.Survey.Save(Survey, user.UserID);
        }

        //public static bool Exists(int SkipSurveyID, string SurveyName, int customerID, KMPlatform.Entity.User user)
        //{
        //    bool exists = false;
        //    using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
        //    {
        //        exists = ECN_Framework_DataLayer.Collector.Survey.Exists(SkipSurveyID, SurveyName, customerID);
        //        scope.Complete();
        //    }
        //    return exists;
        //}

        public static bool HasResponses(int SurveyID, KMPlatform.Entity.User user)
        {
            return ECN_Framework_BusinessLayer.Communicator.EmailDataValues.HasResponses(SurveyID);
        }

        internal static bool IsSurveyGroup(int GroupID)
        {
            bool exists = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.Collector.Survey.IsSurveyGroup(GroupID);
                scope.Complete();
            }
            return exists;
        }       

        public static int GetIncompleteResponseCount(int SurveyID)
        {
            int IncompleteResponseCount = 0;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                IncompleteResponseCount = ECN_Framework_DataLayer.Collector.Survey.GetIncompleteResponseCount(SurveyID);
                scope.Complete();
            }
            return IncompleteResponseCount;
        }

        public static int GetCompletedResponseCount(int SurveyID, string Filters)
        {
            int CompletedResponseCount = 0;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                CompletedResponseCount = ECN_Framework_DataLayer.Collector.Survey.GetCompletedResponseCount(SurveyID, Filters);
                scope.Complete();
            }
            return CompletedResponseCount;
        }
    }
}
