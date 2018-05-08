CREATE PROCEDURE [dbo].[e_User_Select]
@UserID int
AS
SELECT *
FROM 
	[Users] WITH(NOLOCK)  
WHERE
	UserID = @UserID AND
	IsDeleted = 0
