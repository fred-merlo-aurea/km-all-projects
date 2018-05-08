CREATE PROCEDURE [dbo].[e_LandingPageOption_Select_LPID] 
@LPID int
AS
	SELECT *
	FROM LandingPageOption WITH (NOLOCK)
	WHERE LPID = @LPID and IsActive = 1