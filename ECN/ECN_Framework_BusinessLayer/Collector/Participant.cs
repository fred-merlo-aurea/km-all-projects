using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Transactions;

namespace ECN_Framework_BusinessLayer.Collector
{
    [Serializable]
    public class Participant
    {
        public static int GetByUserName(string UserName, int CustomerID)
        {
            ECN_Framework_Entities.Communicator.Email Email = ECN_Framework_BusinessLayer.Communicator.Email.GetByEmailAddress(UserName, CustomerID);
            if (Email != null && Email.EmailID > 0)
                return Email.EmailID;
            else
                return 0;
        }

        public static bool CompletedSurvey(int SurveyID, int ParticipantID, KMPlatform.Entity.User user)
        {
            ECN_Framework_Entities.Collector.Survey s = Survey.GetBySurveyID(SurveyID, user);
            ECN_Framework_Entities.Communicator.GroupDataFields gdf = ECN_Framework_BusinessLayer.Communicator.GroupDataFields.GetByShortName( SurveyID.ToString() + "_" + "completionDt", s.GroupID, user);
            if (gdf != null)
            {
                List<ECN_Framework_Entities.Communicator.EmailDataValues> edvList = ECN_Framework_BusinessLayer.Communicator.EmailDataValues.GetByGroupDataFieldsID(gdf.GroupDataFieldsID, ParticipantID, user);
                if (edvList != null && edvList.Count>0)
                    return true;
                else
                    return false;
            }
            else
                return false;
        }

        public static void InsertCompletionDt(int ParticipantID, int SurveyID, KMPlatform.Entity.User user)
        {
            string shortName = SurveyID.ToString() + "_completionDt";
            ECN_Framework_BusinessLayer.Collector.Response.Save(SurveyID, ParticipantID, DateTime.Now.ToString(), shortName, user);
        }

        public static void InsertBlastID(int ParticipantID, int SurveyID, int BlastID, KMPlatform.Entity.User user)
        {
            string shortName = SurveyID.ToString() + "_blastID";
            ECN_Framework_BusinessLayer.Collector.Response.Save(SurveyID, ParticipantID, BlastID.ToString(), shortName, user);       
        }

        public static List<ECN_Framework_Entities.Communicator.Email> GetParticipants(int SurveyID, string Filters)
        {
            List<ECN_Framework_Entities.Communicator.Email> emailList = new List<ECN_Framework_Entities.Communicator.Email>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                emailList = ECN_Framework_DataLayer.Collector.Survey.GetSurveyRespondents(SurveyID, Filters);
                scope.Complete();
            }
            return emailList;
        }

        public static DataTable ExportParticipants(int GroupID, int CustomerID)
        {
            DataTable dtParticipants = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dtParticipants = ECN_Framework_DataLayer.Collector.Survey.ExportParticipants(GroupID, CustomerID);
                scope.Complete();
            }
            return dtParticipants;
        }

        public static DataTable GetQuestionResponse(int ParticipantID, int QuestionID, string Filters, int count)
        {
            DataTable dtQuestionResponse = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dtQuestionResponse = ECN_Framework_DataLayer.Collector.Survey.GetQuestionResponse(ParticipantID, QuestionID, Filters, count);
                scope.Complete();
            }
            return dtQuestionResponse;
        }
    }
}
