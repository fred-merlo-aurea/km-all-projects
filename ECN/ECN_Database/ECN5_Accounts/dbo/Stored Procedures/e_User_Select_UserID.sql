CREATE PROCEDURE [dbo].[e_User_Select_UserID]
@UserID int
AS
SELECT *
FROM 
	[Users] WITH(NOLOCK) 
WHERE 
	UserID = @UserID and
	IsDeleted = 0
