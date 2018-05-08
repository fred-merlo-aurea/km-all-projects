using System.Collections.Generic;

namespace ECN.Communicator.Tests.Main.Blasts
{
    public partial class ReportsTest
    {
        private const string AolFeedbackUnsubscribePercentage = "AOLFeedbackUnsubscribePercentage";
        private const string AolFeedbackUnsubscribeTotal = "AOLFeedbackUnsubscribeTotal";
        private const string AolFeedbackUnsubscribeTotalPercentage = "AOLFeedbackUnsubscribeTotalPercentage";
        private const string AolFeedbackUnsubscribeUnique = "AOLFeedbackUnsubscribeUnique";
        private const string AbusePercentage = "AbusePercentage";
        private const string AbuseTotal = "AbuseTotal";
        private const string AbuseTotalPercentage = "AbuseTotalPercentage";
        private const string AbuseUnique = "AbuseUnique";
        private const string BouncesPercentage = "BouncesPercentage";
        private const string BouncesTotal = "BouncesTotal";
        private const string BouncesTotalPercentage = "BouncesTotalPercentage";
        private const string BouncesUnique = "BouncesUnique";
        private const string ClickThrough = "ClickThrough";
        private const string ClickThroughPercentage = "ClickThroughPercentage";
        private const string ClicksPercentage = "ClicksPercentage";
        private const string ClicksTotal = "ClicksTotal";
        private const string ClicksTotalPercentage = "ClicksTotalPercentage";
        private const string ClicksUnique = "ClicksUnique";
        private const string ForwardsPercentage = "ForwardsPercentage";
        private const string ForwardsTotal = "ForwardsTotal";
        private const string ForwardsTotalPercentage = "ForwardsTotalPercentage";
        private const string ForwardsUnique = "ForwardsUnique";
        private const string MasterSuppressPercentage = "MasterSuppressPercentage";
        private const string MasterSuppressTotal = "MasterSuppressTotal";
        private const string MasterSuppressTotalPercentage = "MasterSuppressTotalPercentage";
        private const string MasterSuppressUnique = "MasterSuppressUnique";
        private const string NoClickPercentage = "NoClickPercentage";
        private const string NoClickTotal = "NoClickTotal";
        private const string NoOpenPercentage = "NoOpenPercentage";
        private const string NoOpenTotal = "NoOpenTotal";
        private const string OpensPercentage = "OpensPercentage";
        private const string OpensTotal = "OpensTotal";
        private const string OpensTotalPercentage = "OpensTotalPercentage";
        private const string OpensUnique = "OpensUnique";
        private const string ResendsPercentage = "ResendsPercentage";
        private const string ResendsTotal = "ResendsTotal";
        private const string ResendsTotalPercentage = "ResendsTotalPercentage";
        private const string ResendsUnique = "ResendsUnique";
        private const string SendsTotal = "SendsTotal";
        private const string SendsUnique = "SendsUnique";
        private const string SubscribesPercentage = "SubscribesPercentage";
        private const string SubscribesTotal = "SubscribesTotal";
        private const string SubscribesTotalPercentage = "SubscribesTotalPercentage";
        private const string SubscribesUnique = "SubscribesUnique";
        private const string Successful = "Successful";
        private const string SuccessfulDownload = "SuccessfulDownload";
        private const string SuccessfulPercentage = "SuccessfulPercentage";
        private const string SuppressedPercentage = "SuppressedPercentage";
        private const string SuppressedTotal = "SuppressedTotal";
        private const string SuppressedTotalPercentage = "SuppressedTotalPercentage";
        private const string SuppressedUnique = "SuppressedUnique";
        private const string LoadReportsData = "loadReportsData";
        private const int SuppressedCodeID = 1;
        private const string SupressedCode = "supressed-code";
        private const string ZeroPercent = "0%";
        private const string Zero = "0";

        private const string ActionTypeCodeColumn = "ActionTypeCode";
        private static readonly IList<string> ActionTypeCodes = new List<string>
        {
            "send",
            "testsend",
            "click",
            "clickthrough",
            "open",
            "bounce",
            "subscribe",
            "FEEDBACK_UNSUB",
            "ABUSERPT_UNSUB",
            "MASTSUP_UNSUB",
            "UNIQCLIQ",
            "resend",
            "refer",
            "conversion"
        };
    }
}