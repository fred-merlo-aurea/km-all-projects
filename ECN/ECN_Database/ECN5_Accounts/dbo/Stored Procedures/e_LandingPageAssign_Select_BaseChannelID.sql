CREATE PROCEDURE [dbo].[e_LandingPageAssign_Select_BaseChannelID] 
@BaseChannelID int
AS
	SELECT *
	FROM LandingPageAssign WITH (NOLOCK)
	WHERE BaseChannelID = @BaseChannelID