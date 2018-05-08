CREATE PROCEDURE [dbo].[e_MarketingAutomation_Select_MAID]
	@MarketingAutomationID int
AS
	Select * 
	FROM MarketingAutomation ma with(nolock)
	where ma.MarketingAutomationID = @MarketingAutomationID and ma.IsDeleted = 0
