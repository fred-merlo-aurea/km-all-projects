CREATE PROCEDURE [dbo].[e_MarketingAutomationHistory_Insert]
	@MAID int,
	@UserID int,
	@Action varchar(MAX)
AS
	INSERT INTO MarketingAutomationHistory(MarketingAutomationID, UserID, Action,HistoryDate)
	VALUES(@MAID, @UserID, @Action , GetDATE())
