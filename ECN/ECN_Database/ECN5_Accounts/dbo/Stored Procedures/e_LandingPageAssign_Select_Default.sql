CREATE PROCEDURE [dbo].[e_LandingPageAssign_Select_Default]
AS
	SELECT *
	FROM LandingPageAssign WITH (NOLOCK)
	WHERE IsNull(IsDefault, 0) = 1