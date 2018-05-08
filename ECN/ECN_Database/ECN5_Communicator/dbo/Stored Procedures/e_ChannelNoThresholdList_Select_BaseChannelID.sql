CREATE PROCEDURE [dbo].[e_ChannelNoThresholdList_Select_BaseChannelID]   
@BaseChannelID int
AS
	SELECT * FROM ChannelNoThresholdList WITH (NOLOCK) WHERE BaseChannelID = @BaseChannelID and IsDeleted = 0