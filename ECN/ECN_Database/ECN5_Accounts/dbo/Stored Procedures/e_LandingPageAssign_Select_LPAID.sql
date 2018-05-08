CREATE PROCEDURE [dbo].[e_LandingPageAssign_Select_LPAID] 
@LPAID int
AS
	SELECT *
	FROM LandingPageAssign WITH (NOLOCK)
	WHERE LPAID = @LPAID