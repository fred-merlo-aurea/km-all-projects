CREATE PROCEDURE [dbo].[e_LandingPage_Select_ALL] 
AS
	SELECT *
	FROM LandingPage WITH (NOLOCK)
	WHERE IsActive = 1