using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using System.Transactions;
using ECN_Framework_Common.Objects;

namespace ECN_Framework_BusinessLayer.Communicator
{
    [Serializable]
    public class EmailDataValues
    {
        public static List<ECN_Framework_Entities.Communicator.EmailDataValues> GetByGroupDataFieldsID(int groupDataFieldsID, int emailID, KMPlatform.Entity.User user)
        {
            List<ECN_Framework_Entities.Communicator.EmailDataValues> emailDataValuesList = new List<ECN_Framework_Entities.Communicator.EmailDataValues>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                emailDataValuesList = ECN_Framework_DataLayer.Communicator.EmailDataValues.GetByGroupDataFieldsID(groupDataFieldsID, emailID);
                scope.Complete();
            }

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(emailDataValuesList, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            return emailDataValuesList;
        }

        public static List<ECN_Framework_Entities.Communicator.EmailDataValues> GetByGroupDataFieldsID_NoAccessCheck(int groupDataFieldsID, int emailID)
        {
            List<ECN_Framework_Entities.Communicator.EmailDataValues> emailDataValuesList = new List<ECN_Framework_Entities.Communicator.EmailDataValues>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                emailDataValuesList = ECN_Framework_DataLayer.Communicator.EmailDataValues.GetByGroupDataFieldsID(groupDataFieldsID, emailID);
                scope.Complete();
            }

            return emailDataValuesList;
        }

        public static List<ECN_Framework_Entities.Communicator.EmailDataValues> GetByGroupDataFieldsID(int groupDataFieldsID, KMPlatform.Entity.User user)
        {
            List<ECN_Framework_Entities.Communicator.EmailDataValues> emailDataValuesList = new List<ECN_Framework_Entities.Communicator.EmailDataValues>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                emailDataValuesList = ECN_Framework_DataLayer.Communicator.EmailDataValues.GetByGroupDataFieldsID(groupDataFieldsID);
                scope.Complete();
            }

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(emailDataValuesList, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            return emailDataValuesList;
        }

        public static List<ECN_Framework_Entities.Communicator.EmailDataValues> GetByGroupDataFieldsID_NoAccessCheck(int groupDataFieldsID)
        {
            List<ECN_Framework_Entities.Communicator.EmailDataValues> emailDataValuesList = new List<ECN_Framework_Entities.Communicator.EmailDataValues>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                emailDataValuesList = ECN_Framework_DataLayer.Communicator.EmailDataValues.GetByGroupDataFieldsID(groupDataFieldsID);
                scope.Complete();
            }

            return emailDataValuesList;
        }

        public static List<ECN_Framework_Entities.Communicator.EmailDataValues> GetByGroupID(int groupID, KMPlatform.Entity.User user)
        {
            List<ECN_Framework_Entities.Communicator.EmailDataValues> emailDataValuesList = new List<ECN_Framework_Entities.Communicator.EmailDataValues>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                emailDataValuesList = ECN_Framework_DataLayer.Communicator.EmailDataValues.GetByGroupID(groupID);
                scope.Complete();
            }

            if (!ECN_Framework_BusinessLayer.Communicator.AccessCheck.CanAccessByCustomer(emailDataValuesList, user))
                throw new ECN_Framework_Common.Objects.SecurityException();

            return emailDataValuesList;
        }

        public static List<ECN_Framework_Entities.Communicator.EmailDataValues> GetByGroupID_NoAccessCheck(int groupID)
        {
            List<ECN_Framework_Entities.Communicator.EmailDataValues> emailDataValuesList = new List<ECN_Framework_Entities.Communicator.EmailDataValues>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                emailDataValuesList = ECN_Framework_DataLayer.Communicator.EmailDataValues.GetByGroupID(groupID);
                scope.Complete();
            }

            return emailDataValuesList;
        }

       

        public static int RecordTopicsValue(int blastID, int emailID, string values)
        {
            int success = 0;
            using (TransactionScope scope = new TransactionScope())
            {
                success = ECN_Framework_DataLayer.Communicator.EmailDataValues.RecordTopicsValue(blastID, emailID, values);
                scope.Complete();
            }
            return success;
        }

        public static void Delete(int groupID, int groupDataFieldsID, KMPlatform.Entity.User user)
        {
            //this does security check
            GetByGroupDataFieldsID(groupDataFieldsID, user);
            using (TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_DataLayer.Communicator.EmailDataValues.Delete(groupID, groupDataFieldsID, user.CustomerID, user.UserID);
                scope.Complete();
            }
        }

        public static void Delete(int groupID, KMPlatform.Entity.User user)
        {
            Group.GetByGroupID(groupID, user);
            using (TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_DataLayer.Communicator.EmailDataValues.Delete(groupID, user.CustomerID, user.UserID);
                scope.Complete();
            }
        }

        public static DataTable GetTransUDFDataValues(int customerID, int groupID, string emailID, int datafieldSetID, KMPlatform.Entity.User user)
        {
            DataTable dtTransUDFDataValues = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dtTransUDFDataValues = ECN_Framework_DataLayer.Communicator.EmailDataValues.GetTransUDFDataValues(customerID, groupID, emailID, datafieldSetID);
                scope.Complete();
            }
            return dtTransUDFDataValues;
        }

        internal static bool HasResponses(int SurveyID)
        {
            bool HasResponses = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                HasResponses = ECN_Framework_DataLayer.Communicator.EmailDataValues.HasResponses(SurveyID);
                scope.Complete();
            }
            return HasResponses;
        }

        public static DataTable GetStandaloneUDFDataValues(int groupID, int emailID, KMPlatform.Entity.User user)
        {
            DataTable dtStandaloneUDFDataValues = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dtStandaloneUDFDataValues = ECN_Framework_DataLayer.Communicator.EmailDataValues.GetStandaloneUDFDataValues(groupID, emailID);
                scope.Complete();
            }
            return dtStandaloneUDFDataValues;
        }

        internal static void UpdateGridStatementID(int ParticipantID, int GroupDataFieldsID, int GridStatementID)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_DataLayer.Communicator.EmailDataValues.UpdateGridStatementID(ParticipantID, GroupDataFieldsID, GridStatementID);
                scope.Complete();
            }
        }

        internal static void DeleteByEmailID(int GroupID, int EmailID, KMPlatform.Entity.User user)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_DataLayer.Communicator.EmailDataValues.DeleteByEmailID(GroupID, EmailID);
                scope.Complete();
            }
        }

        internal static void DeleteByEmailID(int GroupID, int EmailID, int GroupDataFieldsID, KMPlatform.Entity.User user)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_DataLayer.Communicator.EmailDataValues.DeleteByEmailID(GroupID, EmailID, GroupDataFieldsID);
                scope.Complete();
            }
        }

        internal static void SaveCheckBox(int SurveyID, int ParticipantID, string Value, string ShortName, KMPlatform.Entity.User user)
        {
            ECN_Framework_Entities.Collector.Survey s = ECN_Framework_BusinessLayer.Collector.Survey.GetBySurveyID(SurveyID, user);
            ECN_Framework_Entities.Communicator.GroupDataFields gdf = ECN_Framework_BusinessLayer.Communicator.GroupDataFields.GetByShortName(ShortName, s.GroupID, user);
            if (gdf == null)
            {
                gdf = new ECN_Framework_Entities.Communicator.GroupDataFields();
                gdf.GroupID = s.GroupID;
                gdf.ShortName = ShortName;
                gdf.LongName = ShortName;
                gdf.CreatedUserID = s.CreatedUserID;
                gdf.CustomerID = s.CustomerID;
                gdf.IsPublic = "N";
                gdf.SurveyID = SurveyID;
                ECN_Framework_BusinessLayer.Communicator.GroupDataFields.Save(gdf, user);


            }
            ECN_Framework_Entities.Communicator.Email e = ECN_Framework_BusinessLayer.Communicator.Email.GetByEmailID(ParticipantID, user);

            ECN_Framework_DataLayer.Communicator.EmailDataValues.SaveCheckBox(e.EmailID, gdf.GroupDataFieldsID, Value, user.UserID);
        }
      
        internal static void SaveCheckBox_NoAccessCheck(int SurveyID, int ParticipantID, string Value, string ShortName, KMPlatform.Entity.User user)
        {
            ECN_Framework_Entities.Collector.Survey s = ECN_Framework_BusinessLayer.Collector.Survey.GetBySurveyID(SurveyID, user);
            ECN_Framework_Entities.Communicator.GroupDataFields gdf = ECN_Framework_BusinessLayer.Communicator.GroupDataFields.GetByShortName(ShortName, s.GroupID, user);
            if (gdf == null)
            {
                gdf = new ECN_Framework_Entities.Communicator.GroupDataFields();
                gdf.GroupID = s.GroupID;
                gdf.ShortName = ShortName;
                gdf.LongName = ShortName;
                gdf.CreatedUserID = s.CreatedUserID;
                gdf.CustomerID = s.CustomerID;
                gdf.IsPublic = "N";
                gdf.SurveyID = SurveyID;
                ECN_Framework_BusinessLayer.Communicator.GroupDataFields.Save_NoAccessCheck(gdf, user);


            }
            ECN_Framework_Entities.Communicator.Email e = ECN_Framework_BusinessLayer.Communicator.Email.GetByEmailID_NoAccessCheck(ParticipantID);

            ECN_Framework_DataLayer.Communicator.EmailDataValues.SaveCheckBox(e.EmailID, gdf.GroupDataFieldsID, Value, user.UserID);
        }

        public static string WATT_GetNextTokenForSubscriber(string Token, int IssueID)
        {
            string retToken = "";
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                retToken = ECN_Framework_DataLayer.Communicator.EmailDataValues.WATT_GetNextTokenForSubscriber(Token, IssueID);
                scope.Complete();
            }
            return retToken;
        }

        public static bool WATT_SubscriberExists(string Token, int IssueID)
        {
            bool exists = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                exists = ECN_Framework_DataLayer.Communicator.EmailDataValues.WATT_SubscriberExists(Token, IssueID);
                scope.Complete();
            }
            return exists;
        }

        public static string WATT_GetTokenForSubscriber(string emailAddress,string shortName, int groupID,  int customerID)
        {
            string retToken = "";
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                retToken = ECN_Framework_DataLayer.Communicator.EmailDataValues.WATT_GetTokenForSubscriber(emailAddress, shortName, groupID, customerID);
                scope.Complete();
            }
            return retToken;

        }
    }
}
