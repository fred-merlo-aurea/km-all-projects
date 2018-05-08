CREATE PROCEDURE [dbo].[e_LandingPage_Select_LPID]   
@LPID int
AS
	SELECT *
	FROM LandingPage WITH (NOLOCK)
	WHERE LPID = @LPID and IsActive = 1