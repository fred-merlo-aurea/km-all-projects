using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Transactions;

namespace ECN_Framework_BusinessLayer.Collector
{
    [Serializable]
    public class Question
    {
        public static List<ECN_Framework_Entities.Collector.Question> GetBySurveyID(int SurveyID, KMPlatform.Entity.User user)
        {
            List<ECN_Framework_Entities.Collector.Question> itemList = new List<ECN_Framework_Entities.Collector.Question>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                itemList = ECN_Framework_DataLayer.Collector.Question.GetBySurveyID(SurveyID);
                scope.Complete();
            }
            return itemList;
        }

        public static void Reorder(int PageID, string Position, int QuestionID, int ToQuestionID, KMPlatform.Entity.User user)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_DataLayer.Collector.Question.Reorder(PageID, Position, QuestionID, ToQuestionID);
                scope.Complete();
            }
        }


        public static List<ECN_Framework_Entities.Collector.Question> GetByPageID(int PageID, KMPlatform.Entity.User user)
        {
            List<ECN_Framework_Entities.Collector.Question> itemList = new List<ECN_Framework_Entities.Collector.Question>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                itemList = ECN_Framework_DataLayer.Collector.Question.GetByPageID(PageID);
                scope.Complete();
            }
            return itemList;
        }

        public static ECN_Framework_Entities.Collector.Question GetByQuestionID(int QuestionID)
        {
            ECN_Framework_Entities.Collector.Question item = new ECN_Framework_Entities.Collector.Question();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                item = ECN_Framework_DataLayer.Collector.Question.GetByQuestionID(QuestionID);
                scope.Complete();
            }
            return item;
        }

        public static void Delete(int QuestionID, KMPlatform.Entity.User user)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                ECN_Framework_DataLayer.Collector.Question.Delete(QuestionID);
                scope.Complete();
            }
        }

        public static List<ECN_Framework_Entities.Collector.Question> GetBranchingQuestionsByPageID(int PageID, KMPlatform.Entity.User user)
        {
            List<ECN_Framework_Entities.Collector.Question> itemList = new List<ECN_Framework_Entities.Collector.Question>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                itemList = ECN_Framework_DataLayer.Collector.Question.GetBranchingQuestionsByPageID(PageID);
                scope.Complete();
            }
            return itemList;
        }

        public static void Save(ECN_Framework_Entities.Collector.Question q, string position,int targetID,string options,string gridrow, KMPlatform.Entity.User user)
        {
            q.QuestionID = ECN_Framework_DataLayer.Collector.Question.Save(q, position, targetID, options, gridrow);
            q = GetByQuestionID(q.QuestionID);
            ECN_Framework_Entities.Collector.Survey s = Survey.GetBySurveyID(q.SurveyID, user);
            string ShortName = q.SurveyID.ToString() + "_" + q.Number;
            ECN_Framework_Entities.Communicator.GroupDataFields gdf = ECN_Framework_BusinessLayer.Communicator.GroupDataFields.GetByShortName(ShortName, s.GroupID, user);
            if (gdf == null)
            {
                gdf = new ECN_Framework_Entities.Communicator.GroupDataFields();
                gdf.GroupID = s.GroupID;
                gdf.ShortName = ShortName;
                gdf.LongName = ShortName;
                gdf.CreatedUserID = user.UserID;
                gdf.CustomerID = s.CustomerID;
                gdf.IsPublic = "N";
                gdf.SurveyID = q.SurveyID;
                ECN_Framework_BusinessLayer.Communicator.GroupDataFields.Save(gdf, user);
            }
        }

        public static DataTable GetQuestionsGridByPageID(int PageID)
        {
            DataTable dtCampaignItems = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dtCampaignItems = ECN_Framework_DataLayer.Collector.Question.GetQuestionsGridByPageID(PageID);
                scope.Complete();
            }
            return dtCampaignItems;
        }

        public static void ResetAnswers(ECN_Framework_Entities.Collector.Question Question)
        {
            Question.ResponseList = new List<ECN_Framework_Entities.Collector.Response>();
        }

        public static void AddAnswer(ECN_Framework_Entities.Collector.Question Question, int id, string val)
        {
            ECN_Framework_Entities.Collector.Response r =new ECN_Framework_Entities.Collector.Response();
            r.ID=id;
            r.Value= val;
            Question.ResponseList.Add(r);
        }

        public static void AddAnswer(ECN_Framework_Entities.Collector.Question Question, string val)
        {
            ECN_Framework_Entities.Collector.Response r = new ECN_Framework_Entities.Collector.Response();
            r.ID = -1;
            r.Value = val;
            Question.ResponseList.Add(r);
        }

        public static void LoadResponses(ECN_Framework_Entities.Collector.Question question, List<ECN_Framework_Entities.Collector.Response> responseList)
        {
            if (responseList.Count == 0)
            {
                return;
            }
            string answer;
            switch (question.Format.ToLower())
            {
                case "checkbox":
                    foreach (ECN_Framework_Entities.Collector.Response response in responseList)
                    {
                        answer = response.Value;
                        if (answer.Trim() == string.Empty)
                        {
                            continue;
                        }
                        ECN_Framework_BusinessLayer.Collector.Question.AddAnswer(question,answer);
                    }
                    break;
                case "dropdown":
                case "radio":
                case "textbox":
                    answer = responseList[0].Value;
                    if (answer.Trim().Length > 0)
                    {
                        ECN_Framework_BusinessLayer.Collector.Question.AddAnswer(question, answer);
                    }
                    break;
                case "grid":
                    foreach (ECN_Framework_Entities.Collector.Response response in responseList)
                    {
                        answer = response.Value;
                        if (answer.Trim() == string.Empty)
                        {
                            continue;
                        }
                        ECN_Framework_BusinessLayer.Collector.Question.AddAnswer(question, response.ID, answer);
                    }
                    break;
                default:
                    return;
            }
        }

        public static DataTable GetFilterValues(string Filters)
        {
            DataTable dtFilterValues = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dtFilterValues = ECN_Framework_DataLayer.Collector.Question.GetFilterValues(Filters);
                scope.Complete();
            }
            return dtFilterValues;
        }

        public static DataTable GetQuestionsWithFilterCount(int SurveyID, string Filters)
        {
            DataTable dtQuestionsWithFilterCount = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dtQuestionsWithFilterCount = ECN_Framework_DataLayer.Collector.Question.GetQuestionsWithFilterCount(SurveyID, Filters);
                scope.Complete();
            }
            return dtQuestionsWithFilterCount;
        }

        public static DataTable GetTextResponses(int QuestionID, bool OtherText, string Filters)
        {
            DataTable dtQuestionsWithFilterCount = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Suppress))
            {
                dtQuestionsWithFilterCount = ECN_Framework_DataLayer.Collector.Question.GetTextResponses(QuestionID, OtherText, Filters);
                scope.Complete();
            }
            return dtQuestionsWithFilterCount;
        }
    }
}
