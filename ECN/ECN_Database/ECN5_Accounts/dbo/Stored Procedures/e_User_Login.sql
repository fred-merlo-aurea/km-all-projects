CREATE PROCEDURE [dbo].[e_User_Login]
@UserName varchar(100),
@Password varchar(50)
AS
Begin

SELECT Top 1
	u.*
FROM 
	[Users] u WITH(NOLOCK) join 
	Customer c on u.CustomerID = c.CustomerID
WHERE 
	c.ActiveFlag = 'Y' and 
	UserName = @UserName and 
	Password = @Password and
	u.ActiveFlag = 'Y' and 
	u.IsDeleted = 0 and 
	c.IsDeleted = 0
	
End
