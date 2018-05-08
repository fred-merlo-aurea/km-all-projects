CREATE PROCEDURE [dbo].[e_LandingPageOption_Select_LPOID] 
@LPOID int
AS
	SELECT *
	FROM LandingPageOption WITH (NOLOCK)
	WHERE LPOID = @LPOID and IsActive = 1