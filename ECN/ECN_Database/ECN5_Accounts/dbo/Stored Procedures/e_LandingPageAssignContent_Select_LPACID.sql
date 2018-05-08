CREATE PROCEDURE [dbo].[e_LandingPageAssignContent_Select_LPACID] 
@LPACID int
AS
	SELECT *
	FROM LandingPageAssignContent WITH (NOLOCK)
	WHERE LPACID = @LPACID and IsDeleted = 0