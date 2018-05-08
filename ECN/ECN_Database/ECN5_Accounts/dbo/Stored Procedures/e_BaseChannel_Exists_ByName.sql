CREATE PROCEDURE [dbo].[e_BaseChannel_Exists_ByName]
	@BaseChannelName varchar(500),
	@BaseChannelID int
AS
	IF EXISTS(Select top 1 * from Basechannel bc with(nolock) where bc.IsDeleted = 0 and bc.BaseChannelName = @BaseChannelName and bc.BaseChannelID != @BaseChannelID)
		SELECT 1
	ELSE
		SELECT 0

