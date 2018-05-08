CREATE PROCEDURE [dbo].[e_MAConnector_Select_MarketingAutomationID]
	@MarketingAutomationID int
AS
	Select *
	FROM MAConnector ma with(nolock)
	where ma.MarketingAutomationID = @MarketingAutomationID
