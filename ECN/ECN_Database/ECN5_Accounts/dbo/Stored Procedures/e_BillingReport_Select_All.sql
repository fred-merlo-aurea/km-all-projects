CREATE PROCEDURE [dbo].[e_BillingReport_Select_All]
	
AS
	SELECT *
	FROM BillingReport br with(nolock)
	WHERE br.IsDeleted = 0
