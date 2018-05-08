using System;
using System.ComponentModel;
using System.Text;
using System.Xml;
using System.Collections.Generic;
using System.Web.Services;
using ecn.webservice.Facades;
using ecn.webservice.Facades.Params;

namespace ecn.webservice
{
    /// <summary>
    /// Summary description for BlastManager
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.None)]
    [ToolboxItem(false)]
    public class BlastManager : WebServiceManagerBase
    {
        private string ScheduleType = "";
        private string WeekDay = "";
        private int HowOften = 0;
        
        private IBlastFacade _blastFacade;

        public IBlastFacade BlastFacade
        {
            get
            {
                if (_blastFacade == null)
                {
                    _blastFacade = new BlastFacade();
                }
                return _blastFacade;
            }
            set
            {
                _blastFacade = value;
            }
        }

        public BlastManager()
        {
        }

        public BlastManager(IWebMethodExecutionWrapper executionWrapper)
            :base(executionWrapper)
        {
        }

        #region Get Subscriber Count By Group ID - GetSubscriberCount()

        [WebMethod(Description = "Get the subscriber count from a List from ECN.")]
        public string GetSubscriberCount(string ecnAccessKey, int GroupID)
        {
            var executionWrapper = InitializeExecutionWrapper(
                string.Format(Consts.BlastManagerServiceMethodName, Consts.GetSubscriberCountMethodName),
                Consts.GetSubscriberCountMethodName,
                ecnAccessKey,
                string.Format(Consts.GetSubscriberCountLogInput, GroupID));

            var parameters = new Dictionary<string, object>
            {
                {Consts.GroupIdParameter, GroupID}
            };

            return executionWrapper.Execute(BlastFacade.GetSubscriberCount, parameters);
        }

        #endregion

        #region Add Blast - AddBlast()

        [WebMethod(Description = "Add a Blast to ECN. ")]
        public string AddBlast(string ecnAccessKey, int MessageID, int ListID, int DeptID, int FilterID, string Subject, string FromEmail, string FromName, string ReplyEmail, bool IsTest)
        {
            return AddBlastMain(ecnAccessKey, MessageID, ListID, DeptID, FilterID, Subject, FromEmail, FromName, ReplyEmail, IsTest, "", null, null);
        }

        [WebMethod(Description = "Add a Blast to ECN(Advanced). ")]
        public string AddAdvancedBlast(string ecnAccessKey, int MessageID, int ListID, int DeptID, int FilterID, string Subject, string FromEmail, string FromName, string ReplyEmail, bool IsTest, string refBlasts, int OverRideAmount, bool IsOverRideAmount)
        {
            return AddBlastMain(ecnAccessKey, MessageID, ListID, DeptID, FilterID, Subject, FromEmail, FromName, ReplyEmail, IsTest, refBlasts, OverRideAmount, IsOverRideAmount);
        }

        private string AddBlastMain(
            string ecnAccessKey,
            int MessageID,
            int ListID,
            int DeptID,
            int FilterID,
            string Subject,
            string FromEmail,
            string FromName,
            string ReplyEmail,
            bool IsTest,
            string refBlasts,
            int? OverRideAmount,
            bool? IsOverRideAmount)
        {
            var overrideAmount = OverRideAmount?.ToString() ?? string.Empty;
            var isOverrideAmount = IsOverRideAmount?.ToString() ?? string.Empty;
            var logInput = string.Format(
                    Consts.AddBlastLogInput,
                    MessageID,
                    ListID,
                    DeptID,
                    FilterID,
                    Subject,
                    FromEmail,
                    FromName,
                    ReplyEmail,
                    IsTest,
                    refBlasts,
                    overrideAmount,
                    isOverrideAmount);

            var executionWrapper = InitializeExecutionWrapper(
                string.Format(Consts.BlastManagerServiceMethodName, Consts.AddBlastMainMethodName),
                Consts.AddBlastMainMethodName,
                ecnAccessKey,
                logInput);

            var parameters = new AddBlastParams
            {
                MessageId = MessageID,
                ListId = ListID,
                FilterId = FilterID,
                Subject = Subject,
                FromEmail = FromEmail,
                FromName = FromName,
                ReplyEmail = ReplyEmail,
                IsTest = IsTest,
                RefBlasts = refBlasts
            };

            return executionWrapper.Execute(BlastFacade.AddBlast, parameters);
        }

        #endregion

        #region Update Blast - UpdateBlast()

        [WebMethod(Description = "Update a Blast in ECN. ")]
        public string UpdateBlast(
            string ecnAccessKey,
            int MessageID,
            int ListID,
            int BlastID,
            int FilterID,
            string Subject,
            string FromEmail,
            string FromName,
            string ReplyEmail)
        {
            var logInput = string.Format(
                Consts.UpdateBlastLogInput,
                MessageID,
                ListID,
                BlastID,
                FilterID,
                Subject,
                FromEmail,
                FromName,
                ReplyEmail);

            var executionWrapper = InitializeExecutionWrapper(
                string.Format(Consts.BlastManagerServiceMethodName, Consts.UpdateBlastMethodName),
                Consts.UpdateBlastMethodName,
                ecnAccessKey,
                logInput);

            var parameters = new UpdateBlastParams
            {
                MessageId = MessageID,
                ListId = ListID,
                BlastId = BlastID,
                FilterId = FilterID,
                Subject = Subject,
                FromEmail = FromEmail,
                FromName = FromName,
                ReplyEmail = ReplyEmail
            };

            return executionWrapper.Execute(BlastFacade.UpdateBlast, parameters);
        }

        #endregion

        #region Add Regular Scheduled Blast - AddScheduledBlast()

        [WebMethod(Description = "Add a Scheduled Blast to ECN. ")]
        public string AddScheduledBlast(string ecnAccessKey, int MessageID, int ListID, int DeptID, int FilterID, string Subject, string FromEmail, string FromName, string ReplyEmail, string XMLSchedule)
        {
            //this uses an old version of the xml before the scheduler so we need to convert the xml to the new format
            XMLSchedule = CreateNewXMLScheduleFromOld(XMLSchedule);
            return AddScheduledBlastMain(ecnAccessKey, MessageID, ListID, DeptID, FilterID, Subject, FromEmail, FromName, ReplyEmail, XMLSchedule, "");
        }

        [WebMethod(Description = "Add a Scheduled Blast to ECN(Advanced). ")]
        public string AddScheduledAdvancedBlast(string ecnAccessKey, int MessageID, int ListID, int DeptID, int FilterID, string Subject, string FromEmail, string FromName, string ReplyEmail, bool IsTest, string XMLSchedule, string refBlasts)
        {
            return AddScheduledBlastMain(ecnAccessKey, MessageID, ListID, DeptID, FilterID, Subject, FromEmail, FromName, ReplyEmail, XMLSchedule, refBlasts);
        }

        private string AddScheduledBlastMain(
            string ecnAccessKey,
            int MessageID,
            int ListID,
            int DeptID,
            int FilterID,
            string Subject,
            string FromEmail,
            string FromName,
            string ReplyEmail,
            string XMLSchedule,
            string refBlasts)
        {
            var logInput = string.Format(
                "<ROOT><MessageID>" + MessageID.ToString() + "</MessageID><ListID>" + ListID.ToString() + "</ListID><DeptID>" + DeptID.ToString() + "</DeptID><FilterID>" + FilterID.ToString() + "</FilterID><Subject>" + Subject + "</Subject><FromEmail>" +
                FromEmail + "</FromEmail><FromName>" + FromName + "</FromName><ReplyEmail>" + ReplyEmail + "</ReplyEmail><XMLSchedule><![CDATA[" + XMLSchedule + "]]></XMLSchedule><RefBlasts>" + refBlasts + "</RefBlasts></ROOT>",
                MessageID,
                ListID,
                //BlastID,
                FilterID,
                Subject,
                FromEmail,
                FromName,
                ReplyEmail);

            var executionWrapper = InitializeExecutionWrapper(
                string.Format(Consts.BlastManagerServiceMethodName, Consts.AddScheduledBlastMethodName),
                Consts.AddScheduledBlastMethodName,
                ecnAccessKey,
                logInput);

            var parameters = new Dictionary<string, object>
            {
                { Consts.MessageIdParameter, MessageID },
                { Consts.ListIdParameter, ListID },
                { Consts.FilterIdParameter, FilterID },
                { Consts.SubjectParameter, Subject },
                { Consts.FromEmailParameter, FromEmail },
                { Consts.FromNameParameter, FromName },
                { Consts.ReplyEmailParameter, ReplyEmail },
                { Consts.RefBlastsParameter, refBlasts },
                { Consts.XmlScheduleParameter, XMLSchedule }
            };

            return executionWrapper.Execute(BlastFacade.AddScheduledBlast, parameters);
        }

        #endregion

        #region Get Blast - GetBlast()
        
        [WebMethod(Description = "Get Blast from ECN. ")]
        public string GetBlast(string ecnAccessKey, int BlastID)
        {
            var executionWrapper = InitializeExecutionWrapper(
                string.Format(Consts.BlastManagerServiceMethodName, Consts.GetBlastMethodName),
                Consts.GetBlastMethodName,
                ecnAccessKey,
                string.Format(Consts.GetBlastLogInput, BlastID));

            var parameters = new Dictionary<string, object>
            {
                {Consts.BlastIdParameter, BlastID}
            };

            return executionWrapper.Execute(BlastFacade.GetBlast, parameters);
        }

        #endregion

        #region Search For Blasts - SearchForBlasts()

        [WebMethod(Description = "Search for Blasts in ECN. ")]
        public string SearchForBlasts(string ecnAccessKey, string XMLSearch)
        {
            var executionWrapper = InitializeExecutionWrapper(
                string.Format(Consts.BlastManagerServiceMethodName, Consts.SearchForBlastsMethodName),
                Consts.SearchForBlastsMethodName,
                ecnAccessKey,
                string.Format(Consts.SearchLogInput, XMLSearch));

            return executionWrapper.Execute(BlastFacade.SearchForBlasts, XMLSearch);
        }

        #endregion

        #region Delete Blast - DeleteBlast()

        [WebMethod(Description = "Delete Blast from ECN. ")]
        public string DeleteBlast(string ecnAccessKey, int BlastID)
        {
            var executionWrapper = InitializeExecutionWrapper(
                string.Format(Consts.BlastManagerServiceMethodName, Consts.DeleteBlastMethodName),
                Consts.DeleteBlastMethodName,
                ecnAccessKey,
                string.Format(Consts.DeleteBlastLogInput, BlastID));

            return executionWrapper.Execute(BlastFacade.DeleteBlast, BlastID);
        }

        #endregion

        #region Get Blast Report - GetBlastReport()

        [WebMethod(Description = "Get a Blast Report from ECN. ")]
        public string GetBlastReport(string ecnAccessKey, int blastID)
        {
            var logInput = string.Format(Consts.GetBlastReportLogInput, blastID);

            var executionWrapper = InitializeExecutionWrapper(
                string.Format(Consts.BlastManagerServiceMethodName, Consts.GetBlastReportMethodName),
                Consts.GetBlastReportMethodName,
                ecnAccessKey,
                logInput);

            return executionWrapper.Execute(BlastFacade.GetBlastReport, blastID);
        }

        #endregion

        #region Get Blast Report By ISP - GetBlastReportByISP()

        [WebMethod(Description = "Get a Blast Report by ISP from ECN. ")]
        public string GetBlastReportByISP(string ecnAccessKey, int blastID, string XMLSearch)
        {
            var logInput = string.Format(Consts.GetBlastReportByISPLogInput,blastID, XMLSearch);

            var executionWrapper = InitializeExecutionWrapper(
                string.Format(Consts.BlastManagerServiceMethodName, Consts.GetBlastReportByISPMethodName),
                Consts.GetBlastReportByISPMethodName,
                ecnAccessKey,
                logInput);

            var parameters = new GetBlastReportByISPParams()
            {
                BlastId = blastID,
                XmlSearch = XMLSearch
            };

            return executionWrapper.Execute(BlastFacade.GetBlastReportByISP, parameters);
        }

        #endregion

        #region Get Blast Opens Report - GetBlastOpensReport()

        [WebMethod(Description = "Get a Blast Opens Report from ECN. ")]
        public string GetBlastOpensReport(string ecnAccessKey, int blastID, string filterType, bool withDetail)
        {
            var logInput = string.Format(Consts.GetBlastOpensReportLogInput, blastID, filterType, withDetail);

            var executionWrapper = InitializeExecutionWrapper(
                string.Format(Consts.BlastManagerServiceMethodName, Consts.GetBlastOpensReportMethodName),
                Consts.GetBlastOpensReportMethodName,
                ecnAccessKey,
                logInput);

            var parameters = new GetBlastReportParams
            {
                BlastId = blastID,
                FilterType = filterType,
                WithDetail = withDetail
            };

            return executionWrapper.Execute(BlastFacade.GetBlastOpensReport, parameters);
        }

        #endregion

        #region Get Blast Clicks Report - GetBlastClicksReport()\

        [WebMethod(Description = "Get a Blast Clicks Report from ECN. ")]
        public string GetBlastClicksReport(string ecnAccessKey, int blastID, string filterType, bool withDetail)
        {
            var logInput = string.Format(Consts.GetBlastClicksReportLogInput, blastID, filterType, withDetail);

            var executionWrapper = InitializeExecutionWrapper(
                string.Format(Consts.BlastManagerServiceMethodName, Consts.GetBlastClicksReportMethodName),
                Consts.GetBlastClicksReportMethodName,
                ecnAccessKey,
                logInput);

            var parameters = new GetBlastReportParams
            {
                BlastId = blastID,
                FilterType = filterType,
                WithDetail = withDetail
            };

            return executionWrapper.Execute(BlastFacade.GetBlastClicksReport, parameters);
        }

        #endregion

        #region Get Blast Bounce Report - GetBlastBounceReport()

        [WebMethod(Description = "Get a Blast Bounce Report from ECN. ")]
        public string GetBlastBounceReport(string ecnAccessKey, int blastID, bool withDetail)
        {
            var logInput = string.Format(Consts.GetBlastBounceReportLogInput, blastID, withDetail);

            var executionWrapper = InitializeExecutionWrapper(
                string.Format(Consts.BlastManagerServiceMethodName, Consts.GetBlastBounceReportMethodName),
                Consts.GetBlastBounceReportMethodName,
                ecnAccessKey,
                logInput);

            var parameters = new GetBlastReportParams
            {
                BlastId = blastID,
                WithDetail = withDetail
            };

            return executionWrapper.Execute(BlastFacade.GetBlastBounceReport, parameters);
        }

        #endregion

        #region Get Blast Unsubscribe Report - GetBlastUnsubscribeReport()

        [WebMethod(Description = "Get a Blast Unsubscribe Report from ECN. ")]
        public string GetBlastUnsubscribeReport(string ecnAccessKey, int blastID, bool withDetail)
        {
            var logInput = string.Format(Consts.GetBlastUnsubscribeReportLogInput, blastID, withDetail);

            var executionWrapper = InitializeExecutionWrapper(
                string.Format(Consts.BlastManagerServiceMethodName, Consts.GetBlastUnsubscribeReportMethodName),
                Consts.GetBlastUnsubscribeReportMethodName,
                ecnAccessKey,
                logInput);

            var parameters = new GetBlastReportParams
            {
                BlastId = blastID,
                WithDetail = withDetail
            };

            return executionWrapper.Execute(BlastFacade.GetBlastUnsubscribeReport, parameters);
        }

        #endregion

        #region Get Blast Delivery Report - GetBlastDeliveryReport()

        [WebMethod(Description = "Get a Blast Delivery Report from ECN. ")]
        public string GetBlastDeliveryReport(string ecnAccessKey, DateTime fromDate, DateTime toDate)
        {
            var logInput = string.Format(Consts.GetBlastDeliveryReportLogInput, fromDate, toDate);

            var executionWrapper = InitializeExecutionWrapper(
                string.Format(Consts.BlastManagerServiceMethodName, Consts.GetBlastDeliveryReportMethodName),
                Consts.GetBlastDeliveryReportMethodName,
                ecnAccessKey,
                logInput);

            var parameters = new GetBlastDeliveryReportParams
            {
                DateFrom = fromDate,
                DateTo = toDate
            };

            return executionWrapper.Execute(BlastFacade.GetBlastDeliveryReport, parameters);
        }

        #endregion

        #region Private Methods
        
        private string CreateNewXMLScheduleFromOld(string oldXMLSchedule)
        {
            StringBuilder xmlDocument = new StringBuilder();

            string schedTime = string.Empty;
            string schedDate = string.Empty;
            string endDate = string.Empty;

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(oldXMLSchedule);

            bool found = false;
            XmlNode node = doc.SelectSingleNode("//Schedule/Type[@ID ='O']");
            if (node != null)
            {
                ScheduleType = "O";
                foreach (XmlNode childNode in node.ChildNodes)
                {
                    switch (childNode.Name)
                    {
                        case "StartTime":
                            schedTime = childNode.InnerText;
                            break;
                        case "StartDate":
                            schedDate = childNode.InnerText;
                            break;
                        default:
                            break;
                    }
                }
                xmlDocument.Append(String.Format("<Schedule><ScheduleType ID=\"OneTime\"><StartDate>{0}</StartDate><StartTime>{1}</StartTime><SplitType ID=\"SingleDay\"/></ScheduleType></Schedule>", schedDate, schedTime));
                found = true;
            }
            if (!found)
            {
                node = doc.SelectSingleNode("//Schedule/Type[@ID ='D']");
                if (node != null)
                {
                    ScheduleType = "D";
                    foreach (XmlNode childNode in node.ChildNodes)
                    {
                        switch (childNode.Name)
                        {
                            case "StartTime":
                                schedTime = childNode.InnerText;
                                break;
                            case "StartDate":
                                schedDate = childNode.InnerText;
                                endDate = Convert.ToDateTime(schedDate).AddYears(25).ToString("MM/dd/yyyy");
                                break;
                            default:
                                break;
                        }
                    }
                    xmlDocument.Append(String.Format("<Schedule><ScheduleType ID=\"Recurring\"><StartDate>{0}</StartDate><StartTime>{1}</StartTime><EndDate>{2}</EndDate>", schedDate, schedTime, endDate));
                    xmlDocument.Append(String.Format("<RecurrenceType ID=\"Daily\"/></ScheduleType></Schedule>"));
                    found = true;
                }
            }
            if (!found)
            {
                node = doc.SelectSingleNode("//Schedule/Type[@ID ='W']");
                if (node != null)
                {
                    ScheduleType = "W";
                    int dayOfWeek = -1;
                    foreach (XmlNode childNode in node.ChildNodes)
                    {
                        switch (childNode.Name)
                        {
                            case "StartTime":
                                schedTime = childNode.InnerText;
                                schedDate = DateTime.Now.AddDays(1).ToString("MM/dd/yyyy");
                                endDate = Convert.ToDateTime(schedDate).AddYears(25).ToString("MM/dd/yyyy");
                                break;
                            case "EveryNoWeeks":
                                HowOften = Convert.ToInt32(childNode.InnerText);
                                break;
                            case "DayOfWeek":
                                WeekDay = childNode.InnerText;
                                switch (WeekDay.ToUpper())
                                {
                                    case "SUNDAY":
                                        dayOfWeek = 0;
                                        break;
                                    case "MONDAY":
                                        dayOfWeek = 1;
                                        break;
                                    case "TUESDAY":
                                        dayOfWeek = 2;
                                        break;
                                    case "WEDNESDAY":
                                        dayOfWeek = 3;
                                        break;
                                    case "THURSDAY":
                                        dayOfWeek = 4;
                                        break;
                                    case "FRIDAY":
                                        dayOfWeek = 5;
                                        break;
                                    case "SATURDAY":
                                        dayOfWeek = 6;
                                        break;
                                    default:
                                        break;
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    xmlDocument.Append(String.Format("<Schedule><ScheduleType ID=\"Recurring\"><StartDate>{0}</StartDate><StartTime>{1}</StartTime><EndDate>{2}</EndDate>", schedDate, schedTime, endDate));
                    xmlDocument.Append(String.Format("<RecurrenceType ID=\"Weekly\"><HowManyWeeks>{0}</HowManyWeeks><SplitType ID=\"Evenly\"><Days><Day ID=\"{1}\"></Day></Days></SplitType>", HowOften, dayOfWeek));
                    xmlDocument.Append(String.Format("</RecurrenceType></ScheduleType></Schedule>"));
                    found = true;
                }
            }
            if (!found)
            {
                node = doc.SelectSingleNode("//Schedule/Type[@ID ='M']");
                if (node != null)
                {
                    ScheduleType = "M";
                    foreach (XmlNode childNode in node.ChildNodes)
                    {
                        switch (childNode.Name)
                        {
                            case "StartTime":
                                schedTime = childNode.InnerText;
                                schedDate = DateTime.Now.AddDays(1).ToString("MM/dd/yyyy");
                                endDate = Convert.ToDateTime(schedDate).AddYears(25).ToString("MM/dd/yyyy");
                                break;
                            case "DayOfMonth":
                                HowOften = Convert.ToInt32(childNode.InnerText);
                                break;
                            default:
                                break;
                        }
                    }
                    xmlDocument.Append(String.Format("<Schedule><ScheduleType ID=\"Recurring\"><StartDate>{0}</StartDate><StartTime>{1}</StartTime><EndDate>{2}</EndDate>", schedDate, schedTime, endDate));
                    xmlDocument.Append(String.Format("<RecurrenceType ID=\"Monthly\"><DayOfMonth>{0}</DayOfMonth></RecurrenceType></ScheduleType></Schedule>", HowOften));
                    found = true;
                }
            }
            return xmlDocument.ToString();
        }

        #endregion
    }
}
