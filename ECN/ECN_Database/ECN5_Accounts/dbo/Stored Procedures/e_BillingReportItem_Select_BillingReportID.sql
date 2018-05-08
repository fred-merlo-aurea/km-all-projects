CREATE PROCEDURE [dbo].[e_BillingReportItem_Select_BillingReportID]
	@BillingReportID int
AS
	SELECT * 
	FROM BillingReportItem bri with(nolock)
	WHERE bri.BillingReportID = @BillingReportID and bri.IsDeleted = 0
