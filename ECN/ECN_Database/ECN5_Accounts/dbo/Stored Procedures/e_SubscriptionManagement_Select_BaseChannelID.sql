CREATE PROCEDURE [dbo].[e_SubscriptionManagement_Select_BaseChannelID]
	@BaseChannelID int
AS
	SELECT *
	FROM SubscriptionManagement
	WHERE BaseChannelID = @BaseChannelID and IsDeleted = 0
