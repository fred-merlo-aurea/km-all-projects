using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

namespace ECN_Framework_BusinessLayer.Collector
{
    [Serializable]
    public class Response
    {
        

        public static void Save(ECN_Framework_Entities.Collector.Question Question, int ParticipantID, int SurveyID, KMPlatform.Entity.User user)
        {
            Delete(SurveyID, ParticipantID, Question.Number, user);
            switch (Question.Format)
            {
                case "dropdown":
                case "radio":

                    if (Question.ShowTextControl)
                        Delete(SurveyID, ParticipantID, Question.Number, user, true);

                    foreach (ECN_Framework_Entities.Collector.Response answer in Question.ResponseList)
                    {
                        if (answer.ID == -1 && answer.Value.Trim() != string.Empty)
                            Save(SurveyID, ParticipantID, answer.Value, SurveyID + "_" + Question.Number.ToString(), user);
                        else if (answer.ID == -2 && answer.Value.Trim() != string.Empty)
                            Save(SurveyID, ParticipantID, answer.Value, SurveyID + "_" + Question.Number + "_TEXT", user);
                    }

                    break;
                case "textbox":
                    ECN_Framework_Entities.Collector.Response ans = Question.ResponseList[0];
                    if (ans.Value.Trim() == string.Empty)
                    {
                        break;
                    }
                    Save(SurveyID, ParticipantID, ans.Value, SurveyID + "_" + Question.Number.ToString(), user);
                    break;
                case "checkbox":
                    foreach (ECN_Framework_Entities.Collector.Response answer in Question.ResponseList)
                    {
                        if (answer.ID == -1 && answer.Value.Trim() != string.Empty)
                        {
                            ECN_Framework_BusinessLayer.Communicator.EmailDataValues.SaveCheckBox(SurveyID, ParticipantID, answer.Value, SurveyID + "_" + Question.Number.ToString(), user);
                        }
                        else if (answer.ID == -2 && answer.Value.Trim() != string.Empty)
                            ECN_Framework_BusinessLayer.Communicator.EmailDataValues.SaveCheckBox(SurveyID, ParticipantID, answer.Value, SurveyID + "_" + Question.Number + "_TEXT", user);
                    }
                    break;
                case "grid":
                    foreach (ECN_Framework_Entities.Collector.Response answer in Question.ResponseList)
                    {
                        if (answer.Value.Trim() == "")
                        {
                            continue;
                        }
                        Save(SurveyID, ParticipantID, answer.Value, SurveyID + "_" + Question.Number.ToString(), user, answer.ID);
                    }
                    break;
                default:
                    return;
            }
        }

        public static void Save_NoAccessCheck(ECN_Framework_Entities.Collector.Question Question, int ParticipantID, int SurveyID, KMPlatform.Entity.User user)
        {
            Delete(SurveyID, ParticipantID, Question.Number, user);
            switch (Question.Format)
            {
                case "dropdown":
                case "radio":

                    if (Question.ShowTextControl)
                        Delete(SurveyID, ParticipantID, Question.Number, user, true);

                    foreach (ECN_Framework_Entities.Collector.Response answer in Question.ResponseList)
                    {
                        
                        if (answer.ID == -1 && answer.Value.Trim() != string.Empty)
                            Save(SurveyID, ParticipantID, answer.Value, SurveyID + "_" + Question.Number.ToString(), user);
                        else if (answer.ID == -2 && answer.Value.Trim() != string.Empty)
                            Save(SurveyID, ParticipantID, answer.Value, SurveyID + "_" + Question.Number + "_TEXT", user);
                    }

                    break;
                case "textbox":
                    ECN_Framework_Entities.Collector.Response ans = Question.ResponseList[0];
                    
                    if (ans.Value.Trim() == string.Empty)
                    {
                        break;
                    }
                    Save(SurveyID, ParticipantID, ans.Value, SurveyID + "_" + Question.Number.ToString(), user);
                    break;
                case "checkbox":
                    foreach (ECN_Framework_Entities.Collector.Response answer in Question.ResponseList)
                    {
                       
                        if (answer.ID == -1 && answer.Value.Trim() != string.Empty)
                        {
                            ECN_Framework_BusinessLayer.Communicator.EmailDataValues.SaveCheckBox_NoAccessCheck(SurveyID, ParticipantID, answer.Value, SurveyID + "_" + Question.Number.ToString(), user);
                        }
                        else if (answer.ID == -2 && answer.Value.Trim() != string.Empty)
                            ECN_Framework_BusinessLayer.Communicator.EmailDataValues.SaveCheckBox_NoAccessCheck(SurveyID, ParticipantID, answer.Value, SurveyID + "_" + Question.Number + "_TEXT", user);
                    }
                    break;
                case "grid":
                    foreach (ECN_Framework_Entities.Collector.Response answer in Question.ResponseList)
                    {
                        if (answer.Value.Trim() == "")
                        {
                            continue;
                        }
                        Save(SurveyID, ParticipantID, answer.Value, SurveyID + "_" + Question.Number.ToString(), user, answer.ID);
                    }
                    break;
                default:
                    return;
            }
        }

        

        internal static void Save(int SurveyID, int ParticipantID, string Value, string ShortName,KMPlatform.Entity.User user, int GridStatementID = -1)
        {
            ECN_Framework_Entities.Collector.Survey s = Survey.GetBySurveyID(SurveyID, user);
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
                //ECN_Framework_BusinessLayer.Communicator.GroupDataFields.Save_NoAccessCheck(gdf, user);
                ECN_Framework_BusinessLayer.Communicator.GroupDataFields.Save_NoAccessCheck(gdf, user);

            }
            ECN_Framework_Entities.Communicator.Email e = ECN_Framework_BusinessLayer.Communicator.Email.GetByEmailID_NoAccessCheck(ParticipantID);
            StringBuilder xmlUDF = new StringBuilder("");
            xmlUDF.Append("<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><XML>");
            xmlUDF.Append("<row>");
            xmlUDF.Append("<ea>" + e.EmailAddress + "</ea>");
            xmlUDF.Append("<udf id=\"" + gdf.GroupDataFieldsID.ToString() + "\">");
            xmlUDF.Append("<v><![CDATA[" + Value + "]]></v>");
            if(GridStatementID != -1)
                xmlUDF.Append("<sgid>" + GridStatementID + "</sgid>");
            xmlUDF.Append("</udf>");
            xmlUDF.Append("</row>");
            xmlUDF.Append("</XML>");
            StringBuilder xmlProfile = new StringBuilder("");
            xmlProfile.Append("<?xml version=\"1.0\" encoding=\"iso-8859-1\"?><XML><Emails>");
            xmlProfile.Append("<emailaddress>" + e.EmailAddress + "</emailaddress>");
            xmlProfile.Append("</Emails></XML>");
            ECN_Framework_BusinessLayer.Communicator.EmailGroup.ImportEmails_NoAccessCheck(user, s.CustomerID, s.GroupID, xmlProfile.ToString(), xmlUDF.ToString(), "HTML", "S", false, "", "ECN_Framework_BusinessLayer.collector.Response.Save");
            //if (GridStatementID != -1)
            //{
            //    ECN_Framework_BusinessLayer.Communicator.EmailDataValues.UpdateGridStatementID(ParticipantID, gdf.GroupDataFieldsID, GridStatementID);
            //}
        }

        public static List<ECN_Framework_Entities.Collector.Response> GetByQuestion(int SurveyID, int QuestionNumber, int ParticipantID, KMPlatform.Entity.User user)
        {
            ECN_Framework_Entities.Collector.Survey s= ECN_Framework_BusinessLayer.Collector.Survey.GetBySurveyID(SurveyID, user);
            List<ECN_Framework_Entities.Collector.Response> itemList = new List<ECN_Framework_Entities.Collector.Response>();
            ECN_Framework_Entities.Communicator.GroupDataFields gdf= ECN_Framework_BusinessLayer.Communicator.GroupDataFields.GetByShortName(SurveyID.ToString() + "_" + QuestionNumber.ToString(), s.GroupID, user);
            if (gdf != null)
            {
                List<ECN_Framework_Entities.Communicator.EmailDataValues> edvList = ECN_Framework_BusinessLayer.Communicator.EmailDataValues.GetByGroupDataFieldsID_NoAccessCheck(gdf.GroupDataFieldsID, ParticipantID);
                foreach (ECN_Framework_Entities.Communicator.EmailDataValues edv in edvList)
                {
                    ECN_Framework_Entities.Collector.Response r = new ECN_Framework_Entities.Collector.Response();
                    if(edv.SurveyGridID!=null)
                        r.ID = edv.SurveyGridID.Value;
                    r.Value = edv.DataValue;
                    itemList.Add(r);
                }
            }
            return itemList;
        }

        public static string GetTEXTResponses(int SurveyID, int QuestionNumber, int ParticipantID, KMPlatform.Entity.User user)
        {
            string r = string.Empty;
            ECN_Framework_Entities.Collector.Survey s = ECN_Framework_BusinessLayer.Collector.Survey.GetBySurveyID(SurveyID, user);
            
            ECN_Framework_Entities.Communicator.GroupDataFields gdf= ECN_Framework_BusinessLayer.Communicator.GroupDataFields.GetByShortName(SurveyID.ToString() + "_" + QuestionNumber.ToString() + "_TEXT", s.GroupID, user);
            if (gdf != null)
            {
                List<ECN_Framework_Entities.Communicator.EmailDataValues> edvList = ECN_Framework_BusinessLayer.Communicator.EmailDataValues.GetByGroupDataFieldsID_NoAccessCheck(gdf.GroupDataFieldsID, ParticipantID);
                r = edvList[0].DataValue;
            }
            return r;

        }

        public static void Delete(int SurveyID, int ParticipantID, int QuestionNumber, KMPlatform.Entity.User user, bool textResponse=false)
        {
            ECN_Framework_Entities.Collector.Survey s = ECN_Framework_BusinessLayer.Collector.Survey.GetBySurveyID(SurveyID, user);
            string shortName= string.Empty;
            if(!textResponse)
                shortName= SurveyID.ToString() + "_" + QuestionNumber.ToString();
            else
                shortName = SurveyID.ToString() + "_" + QuestionNumber.ToString()+"_TEXT";
            ECN_Framework_Entities.Communicator.GroupDataFields gdf = ECN_Framework_BusinessLayer.Communicator.GroupDataFields.GetByShortName(shortName, s.GroupID, user);
            if (gdf != null)
            {
                ECN_Framework_BusinessLayer.Communicator.EmailDataValues.DeleteByEmailID(s.GroupID, ParticipantID, gdf.GroupDataFieldsID, user);
            }
        }

        public static void Delete(int SurveyID, int EmailID, KMPlatform.Entity.User user)
        {
            ECN_Framework_Entities.Collector.Survey s = ECN_Framework_BusinessLayer.Collector.Survey.GetBySurveyID(SurveyID, user);
            ECN_Framework_BusinessLayer.Communicator.EmailDataValues.DeleteByEmailID(s.GroupID, EmailID, user);
            ECN_Framework_BusinessLayer.Communicator.EmailGroup.Delete_NoAccessCheck(s.GroupID, EmailID, s.CustomerID,user);
        }
    }
}
 