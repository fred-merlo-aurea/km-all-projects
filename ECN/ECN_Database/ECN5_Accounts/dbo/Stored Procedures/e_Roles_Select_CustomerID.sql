CREATE PROCEDURE [dbo].[e_Roles_Select_CustomerID]
@CustomerID int
AS
SELECT 
	RoleID,
	CustomerID,
	RoleName
FROM 
	[Role] WITH(NOLOCK)
WHERE 
	CustomerID = @CustomerID
