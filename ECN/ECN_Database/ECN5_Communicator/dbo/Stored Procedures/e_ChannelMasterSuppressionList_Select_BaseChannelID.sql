CREATE PROCEDURE [dbo].[e_ChannelMasterSuppressionList_Select_BaseChannelID]   
@BaseChannelID int
AS
	SELECT * FROM ChannelMasterSuppressionList WITH (NOLOCK) WHERE BaseChannelID = @BaseChannelID and IsDeleted = 0