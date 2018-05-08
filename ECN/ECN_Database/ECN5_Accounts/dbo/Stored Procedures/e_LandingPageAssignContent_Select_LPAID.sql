CREATE PROCEDURE [dbo].[e_LandingPageAssignContent_Select_LPAID] 
@LPAID int
AS
	SELECT *
	FROM LandingPageAssignContent WITH (NOLOCK)
	WHERE LPAID = @LPAID and IsDeleted = 0
	
	
--exec e_LandingPageAssignContent_Select_LPAID 1