CREATE PROCEDURE [dbo].[e_MarketingAutomation_Select_CustomerID]
	@CustomerID int
AS
	Select * 
	FROM MarketingAutomation ma with(nolock)
	WHERE ma.CustomerID = @CustomerID and ma.IsDeleted = 0
