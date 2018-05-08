CREATE PROCEDURE [dbo].[e_BillingReportItem_Select_BillingReportItemID]
	@BillingReportItemID int
AS
	SELECT * 
	FROM BillingReportItem bri with(nolock)
	WHERE bri.BillingItemID = @BillingReportItemID and bri.IsDeleted = 0
