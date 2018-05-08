using System.Collections.Generic;

namespace ECN.Communicator.Tests.Main.Blasts
{
    public partial class ReportGraphicalTest
    {
        private const string ActionTypeCodeColumn = "ActionTypeCode";
        private const string GroupNameColumn = "GroupName";
        private const string GrpNavigateUrlColumn = "GrpNavigateURL";
        private const string FilterNameColumn = "FilterName";
        private const string FltrNavigateUrlColumn = "FltrNavigateURL";
        private const string LayoutNameColumn = "LayoutName";
        private const string LytNavigateUrlColumn = "LytNavigateURL";
        private const string EmailSubjectColumn = "EmailSubject";
        private const string EmailFromNameColumn = "EmailFromName";
        private const string EmailFromColumn = "EmailFrom";
        private const string SendTimeColumn = "SendTime";
        private const string FinishTimeColumn = "FinishTime";
        private const string SuccessTotalColumn = "SuccessTotal";
        private const string SendTotalColumn = "SendTotal";
        private const string SetupCostColumn = "SetupCost";
        private const string OutboundCostColumn = "OutboundCost";
        private const string InboundCostColumn = "InboundCost";
        private const string DesignCostColumn = "DesignCost";
        private const string OtherCostColumn = "OtherCost";
        private const string BounceCountColumn = "BounceCount";
        private const string BounceTypeColumn = "BounceType";
        private const string ActionValueColumn = "ActionValue";
        private const string ActionDateMmddyyyyColumn = "ActionDateMMDDYYYY";

        private const string GroupNameValue = "group_name";
        private const string GrpNavigateUrlValue = "test.com";
        private const string FilterNameValue = "filter-name";
        private const string FltrNavigateUrlValue = "test2.com";
        private const string LayoutNameValue = "layout-name";
        private const string LytNavigateUrlValue = "test3.com";
        private const string EmailSubjectValue = "email-subject";
        private const string EmailFromNameValue = "email-from-name";
        private const string EmailFromValue = "test@test.com";
        private const string SendTimeValue = "3/27/2018";
        private const string FinishTimeValue = "3/28/2018";
        private const string SuccessTotalValue = "5";
        private const string SendTotalValue = "10";
        private const string SetupCostValue = "1000";
        private const string OutboundCostValue = "2000";
        private const string InboundCostValue = "1500";
        private const string DesignCostValue = "1200";
        private const string OtherCostValue = "500";
        private const string ChartsTempDirectory = "ChartsTempDirectory";
        private const string TempDirectory = "C:\\temp";
        private const string ProcGetGraphicalBlastReportData = "spGetGraphicalBlastReportData";
        private const string ProcGetGraphicalBounceBlastReportData = "spGetGraphicalBlastBounceReportData";
        private const string LoadFormData = "LoadFormData";
        private const string ConnString = "activity";
        private const string TestConnectionString = "test-connection-string";
        private const int BounceCount = 5;
        private const int BlastId = 3;

        private const string BouncesPercentage = "BouncesPercentage";
        private const string BouncesUnique = "BouncesUnique";
        private const string Campaign = "Campaign";
        private const string ClicksPercentage = "ClicksPercentage";
        private const string ClicksUnique = "ClicksUnique";
        private const string DesignCostLbl = "DesignCostLbl";
        private const string EmailFrom = "EmailFrom";
        private const string EmailSubject = "EmailSubject";
        private const string EmailsSentLbl = "EmailsSentLbl";
        private const string Filter = "Filter";
        private const string ForwardsPercentage = "ForwardsPercentage";
        private const string ForwardsUnique = "ForwardsUnique";
        private const string GroupTo = "GroupTo";
        private const string HardBouncesPercentage = "HardBouncesPercentage";
        private const string HardBouncesUnique = "HardBouncesUnique";
        private const string InboundCostLbl = "InboundCostLbl";
        private const string OpensPercentage = "OpensPercentage";
        private const string OpensUnique = "OpensUnique";
        private const string OtherCostLbl = "OtherCostLbl";
        private const string OutboundCostLbl = "OutboundCostLbl";
        private const string PerClickLbl = "PerClickLbl";
        private const string PerEmailChargeLbl = "PerEmailChargeLbl";
        private const string PerResponseLbl = "PerResponseLbl";
        private const string RoiEmailsSentLbl = "ROI_EmailsSentLbl";
        private const string RoiPerClick = "ROI_PerClick";
        private const string RoiPerResponse = "ROI_PerResponse";
        private const string RoiSetupFees = "ROI_SetupFees";
        private const string RoiTotalConversion = "ROI_TotalConversion";
        private const string RoiTotalInvestment = "ROI_TotalInvestment";
        private const string RoiTotalResponse = "ROI_TotalResponse";
        private const string ResendsPercentage = "ResendsPercentage";
        private const string ResendsUnique = "ResendsUnique";
        private const string ResponsesLbl = "ResponsesLbl";
        private const string SendTime = "SendTime";
        private const string SetupSetupCostLbl = "SetupSetupCostLbl";
        private const string SoftBouncesPercentage = "SoftBouncesPercentage";
        private const string SoftBouncesUnique = "SoftBouncesUnique";
        private const string SubscribesPercentage = "SubscribesPercentage";
        private const string SubscribesUnique = "SubscribesUnique";
        private const string Successful = "Successful";
        private const string SuccessfulPercentage = "SuccessfulPercentage";
        private const string TotalSetupLbl = "TotalSetupLbl";
        private const string UnknownBouncesPercentage = "UnknownBouncesPercentage";
        private const string UnknownBouncesUnique = "UnknownBouncesUnique";

        private const string ConversionRate = "6";
        private const string EmailSentCount = "12";
        private const string HsuBoucesUniqueRatio = "2/12";
        private const string SuccessRatio = "8/12";
        private const string BouncePercentage = "33%";
        private const string CofrsBouncesPercentage = "75%";
        private const string PerEmailCharge = "2725";
        private const string TotalInboundCost = "6000";
        private const string TotalOutboundCost = "24000";
        private const string TotalCost = "32700";
        private const string PerClick = "5450";
        private const string BouncesUniqueRatio = "4/12";
        private const string CofrsBouncesUniqueRatio = "6/12";
        private const string HsuBouncesPercentage = "17%";
        private const string SuccessPercentage = "67%";

        private static readonly IList<string> ActionTypeCodes = new List<string>
        {
            "send",
            "testsend",
            "click",
            "open",
            "bounce",
            "subscribe",
            "resend",
            "refer",
            "conversion"
        };

        private static readonly IList<string> ActionValues = new List<string>
        {
            "resend",
            "U",
            "hard",
            "hardbounce",
            "soft",
            "softbounce"
        };
    }
}