CREATE PROCEDURE [dbo].[e_MarketingAutomationHistory_Select_MAID]
	@MAID int
AS
	Select * from MarketingAutomationHistory mah with(nolock)
	where mah.MarketingAutomationID = @MAID
