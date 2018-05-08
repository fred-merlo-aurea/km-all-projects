CREATE PROCEDURE [dbo].[e_LandingPageAssign_Select_BaseChannelID_LPID] 
@BaseChannelID int,
@LPID int
AS
	SELECT *
	FROM LandingPageAssign lpa WITH (NOLOCK)
	join LandingPage lp
	on lpa.LPID= lp.LPID
	WHERE BaseChannelID = @BaseChannelID and lp.LPID=@LPID