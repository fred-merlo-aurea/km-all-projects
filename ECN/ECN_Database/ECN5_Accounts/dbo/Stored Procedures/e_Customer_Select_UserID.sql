CREATE PROCEDURE [dbo].[e_Customer_Select_UserID]
@UserID int
AS

SELECT c.*
FROM Customer c WITH(NOLOCK)
JOIN [Users] u WITH(NOLOCK) ON u.CustomerID = c.CustomerID 
WHERE u.UserID = @UserID and c.IsDeleted=0
