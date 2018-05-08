CREATE PROCEDURE [dbo].[e_RoleAction_Select_RoleID] 
@RoleID int
AS
SELECT 
	ra.RoleActionID,
	ra.RoleID,
	ra.ActionID,
	ra.Active,
	r.CustomerID
FROM 
	RoleAction ra WITH(NOLOCK)
	JOIN [Role] r WITH(NOLOCK) on ra.RoleID = r.RoleID
WHERE 
	ra.RoleID = @RoleID
