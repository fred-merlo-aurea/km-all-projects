create PROCEDURE [dbo].[e_DomainTracker_Select_BaseChannelID]
@BaseChannelID int
AS
	SELECT * FROM DomainTracker cd WITH (NOLOCK) 
	WHERE cd.BaseChannelID=@BaseChannelID and cd.IsDeleted=0