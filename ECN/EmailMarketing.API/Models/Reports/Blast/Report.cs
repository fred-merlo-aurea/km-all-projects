using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace EmailMarketing.API.Models.Reports.Blast
{
    public class Report : Dictionary<ReportRowType,ReportRow>, IDisposable
    {
        public void Add(IEnumerable<ReportRow> rows)
        {
            foreach(var row in rows)
            {
                Add(row);
            }
        }
        public void Add(ReportRow row)
        {
            if(row.RowType == ReportRowType.Unknown)
            {
                return;
            }

            if (row.RowType == ReportRowType.Sends)
            {
                sendRow = row;
            }

            this.Add(row.RowType, row);
        }

        public void Dispose()
        {
            sendRow = null;
        }

        ReportRow sendRow;
        public ReportRow Sends { get { return sendRow; } }
        public bool HasSends { get { return sendRow != null; } }

        static public ReportRowType ActionTypeCodeToRowType(string actionTypeCode)
        {
            ReportRowType returnValue = ReportRowType.Unknown;

            switch (actionTypeCode.ToString().ToLower())
            {
                case "send":
                    return ReportRowType.Sends;
                case "open":
                    return ReportRowType.Opens;
                case "click":
                    return ReportRowType.Clicks;
                case "bounce":
                    return ReportRowType.Bounces;
                case "resend":
                    return ReportRowType.Resends;
                case "refer"://forward
                    return ReportRowType.Forwards;
                case "subscribe":
                    return ReportRowType.Unsubscribes;
                case "MASTSUP_UNSUB":
                    return ReportRowType.MasterSupressed;
                case "ABUSERPT_UNSUB":
                    return ReportRowType.AbuseComplaints;
                case "FEEDBACK_UNSUB":
                    return ReportRowType.ISPFeedbackLoops;
                default:
                    break;
            }

            return returnValue;
        }
    }
}