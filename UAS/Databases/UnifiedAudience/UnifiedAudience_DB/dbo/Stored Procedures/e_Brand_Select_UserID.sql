CREATE PROCEDURE [dbo].[e_Brand_Select_UserID]
	@UserID int = 0
AS
BEGIN
	SELECT b.* 
	FROM Brand b 
	WITH (NOLOCK) 
	JOIN userbrand ub WITH (NOLOCK) ON b.BrandID = ub.brandID 
	WHERE b.IsDeleted = 0 and userID = @UserID
END
