CREATE PROCEDURE [dbo].[e_LandingPageAssign_Select_LPID] 
@LPID int
AS
	SELECT *
	FROM LandingPageAssign WITH (NOLOCK)
	WHERE LPID = @LPID