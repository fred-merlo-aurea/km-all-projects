CREATE PROCEDURE [dbo].[e_MAControl_Select_MarketingAutomationID]
	@MarketingAutomationID int
AS
	SELECT *
	FROM MAControl ma with(nolock)
	where ma.MarketingAutomationID = @MarketingAutomationID
