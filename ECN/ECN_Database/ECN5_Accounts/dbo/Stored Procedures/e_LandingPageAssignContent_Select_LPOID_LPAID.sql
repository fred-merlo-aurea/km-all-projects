CREATE PROCEDURE [dbo].[e_LandingPageAssignContent_Select_LPOID_LPAID]
	@LPOID int,
	@LPAID int
AS
	SELECT * 
	FROM LandingPageAssignContent lpac with(nolock)
	WHERE lpac.LPAID = @LPAID and lpac.LPOID = @LPOID and lpac.IsDeleted = 0
