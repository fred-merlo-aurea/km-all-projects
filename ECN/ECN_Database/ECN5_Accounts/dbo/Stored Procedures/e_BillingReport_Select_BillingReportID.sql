CREATE PROCEDURE [dbo].[e_BillingReport_Select_BillingReportID]
	@BillingReportID int
AS
	SELECT * 
	FROM BillingReport br with(nolock)
	WHERE br.BillingReportID = @BillingReportID and br.IsDeleted = 0
