CREATE PROCEDURE [dbo].[e_MAControl_Select_ControlID]
	@ControlID varchar(50),
	@MarketingAutomationID int
AS
	SELECT * FROM MAControl ma with(nolock)
	WHERE ma.ControlID = @ControlID and ma.MarketingAutomationID = @MarketingAutomationID
