using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using EmailMarketing.API.Models.Reports.Blast;

namespace EmailMarketing.API.ExtentionMethods
{
    public static class DataTableExtentionMethods
    {
        public static IEnumerable<Dictionary<string, string>> ToSimpleDictionary(this DataTable dt, IEnumerable<string> fieldNames = null, IEnumerable<string> excludeFieldNames = null)
        {
            if (fieldNames == null)
            {
                fieldNames = (from DataColumn c in dt.Columns select c.ColumnName).ToArray();
            }
            if( null != excludeFieldNames)
            {
                fieldNames = from f in fieldNames where false == excludeFieldNames.Contains(f) select f;
            }
            foreach (DataRow dataRow in dt.Rows)
            {
                Dictionary<string, string> dictionary = new Dictionary<string, string>();
                foreach (string column in fieldNames)
                {
                    dictionary[column] = (dataRow.Field<object>(column) ?? "").ToString();
                }
                yield return dictionary;
            }
        }

        public static IEnumerable<BounceReport> ToListOfBounceReports(this DataTable dt)
        {
            foreach(DataRow dataRow in dt.Rows)
            {
                BounceReport br = new BounceReport();
                br.BounceSignature = dataRow["BounceSignature"].ToString();
                br.BounceType = dataRow["BounceType"].ToString();
                br.BounceTime = dataRow["BounceTime"].ToString();
                br.EmailAddress = dataRow["EmailAddress"].ToString();
                yield return br;
            }
        }

        public static IEnumerable<ClickReport> ToListOfClickReports(this DataTable dt)
        {
            foreach(DataRow dr in dt.Rows)
            {
                ClickReport cr = new ClickReport();
                cr.EmailAddress = dr["EmailAddress"].ToString();
                cr.Link = dr["Link"].ToString();
                cr.ClickTime = dr["ClickTime"].ToString();
                yield return cr;
            }
        }

        public static IEnumerable<DeliveryReport> ToListOfDeliveryReports(this DataTable dt)
        {
            foreach(DataRow row in dt.Rows)
            {
                DeliveryReport dr = new DeliveryReport();
                dr.blastID = (int)row["blastId"];
                dr.sendTime = row["sendTime"].ToString();
                dr.emailSubject = row["emailsubject"].ToString();
                dr.groupName = row["groupName"].ToString();
                dr.filterName = row["filterName"].ToString();
                dr.campaignName = row["campaignName"].ToString();
                dr.sendTotal = (int)row["sendTotal"];
                dr.delivered = (int)row["delivered"];
                dr.softBounceTotal = (int)row["softBounceTotal"];
                dr.hardBounceTotal = (int)row["hardBounceTotal"];
                dr.otherBounceTotal = (int)row["otherBounceTotal"];
                dr.bounceTotal = (int)row["bounceTotal"];
                dr.uniqueOpens = (int)row["uniqueOpens"];
                dr.totalOpens = (int)row["totalOpens"];
                dr.uniqueClicks = (int)row["uniqueClicks"];
                dr.totalClicks = (int)row["totalClicks"];
                dr.unsubscribeTotal = (int)row["unsubscribeTotal"];
                dr.suppressedTotal = (int)row["suppressedTotal"];
                dr.mobileOpens = (int)row["mobileOpens"];
                dr.fromEmail = row["fromEmail"].ToString();
                dr.campaignItemName = row["campaignItemName"].ToString();
                dr.customerName = row["customerName"].ToString();
                dr.field1 = row["field1"].ToString();
                dr.field2 = row["field2"].ToString();
                dr.field3 = row["field3"].ToString();
                dr.field4 = row["field4"].ToString();
                dr.field5 = row["field5"].ToString();
                try
                {
                    dr.clickThrough = row["clickthrough"] == string.Empty ? 0 : Convert.ToInt32(row["clickthrough"].ToString());
                }
                catch {
                    dr.clickThrough = 0;
                }
                dr.abuse = (int)row["abuse"];
                dr.feedback = (int)row["feedback"];
                dr.spamCount = (int)row["spamcount"];
                yield return dr;
            }
        }

        public static IEnumerable<OpensReport> ToListOfOpensReports(this DataTable dt)
        {
            foreach(DataRow row in dt.Rows)
            {
                OpensReport or = new OpensReport();
                or.EmailAddress = row["EmailAddress"].ToString();
                or.OpenTime = row["OpenTime"].ToString();
                or.Info = row["Info"].ToString();
                yield return or;
            }
        }

        public static IEnumerable<ISPReport> ToListOfISPReports(this DataTable dt)
        {
            foreach(DataRow row in dt.Rows)
            {
                ISPReport ir = new ISPReport();
                ir.ISPs = row["ISPs"].ToString();
                ir.Sends = row["Sends"].ToString();
                ir.Opens = row["Opens"].ToString();
                ir.Clicks = row["Clicks"].ToString();
                ir.Bounces = row["Bounces"].ToString();
                ir.Unsubscribes = row["Unsubscribes"].ToString();
                ir.Resends = row["Resends"].ToString();
                ir.Forwards = row["Forwards"].ToString();
                ir.FeedbackUnsubs = row["FeedbackUnsubs"].ToString();
                yield return ir;
            }
        }

        public static IEnumerable<UnsubscribeReport> ToListOfUnsubscribeReports(this DataTable dt)
        {
            foreach(DataRow row in dt.Rows)
            {
                UnsubscribeReport ur = new UnsubscribeReport();
                ur.EmailAddress = row["EmailAddress"].ToString();
                ur.UnsubscribeTime = row["UnsubscribeTime"].ToString();
                ur.SubscriptionChange = row["SubscriptionChange"].ToString();
                ur.Reason = row["Reason"].ToString();
                ur.Title = row["Title"].ToString();
                ur.FirstName = row["FirstName"].ToString();
                ur.LastName = row["LastName"].ToString();
                ur.FullName = row["FullName"].ToString();
                ur.Company = row["Company"].ToString();
                ur.Occupation = row["Occupation"].ToString();
                ur.Address = row["Address"].ToString();
                ur.Address2 = row["Address2"].ToString();
                ur.City = row["City"].ToString();
                ur.State = row["State"].ToString();
                ur.Zip = row["Zip"].ToString();
                ur.Country = row["Country"].ToString();
                ur.Voice = row["Voice"].ToString();
                ur.Mobile = row["Mobile"].ToString();
                ur.Fax = row["Fax"].ToString();
                ur.Website = row["Website"].ToString();
                ur.Age = row["Age"].ToString() == string.Empty ? 0 : Convert.ToInt32(row["Age"].ToString());
                ur.Income = row["Income"].ToString() == string.Empty ? 0 : float.Parse(row["Income"].ToString());
                ur.Gender = row["Gender"].ToString();
                ur.User1 = row["User1"].ToString();
                ur.User2 = row["User2"].ToString();
                ur.User3 = row["User3"].ToString();
                ur.User4 = row["User4"].ToString();
                ur.User5 = row["User5"].ToString();
                ur.User6 = row["User6"].ToString();
                ur.Birthdate = row["Birthdate"].ToString();
                ur.UserEvent1 = row["UserEvent1"].ToString();
                ur.UserEvent1Date = row["UserEvent1Date"].ToString();
                ur.UserEvent2 = row["UserEvent2"].ToString();
                ur.UserEvent2Date = row["UserEvent2Date"].ToString();
                ur.CreatedOn = row["CreatedOn"].ToString();
                ur.LastChanged = row["LastChanged"].ToString();
                ur.FormatTypeCode = row["FormatTypeCode"].ToString();
                ur.SubscribeTypeCode = row["SubscribeTypeCode"].ToString();
                ur.tmp_EmailID = row["tmp_EmailID"].ToString();
                //ur.NickName = row["nickname"].ToString();
                yield return ur;
            }
        }
    }
}