CREATE PROCEDURE [dbo].[e_Role_Exists_ByID] 
	@RoleID int,
	@CustomerID int
AS     
BEGIN     		
	IF EXISTS (SELECT TOP 1 RoleID FROM [Role] WHERE CustomerID = @CustomerID AND RoleID = @RoleID AND IsDeleted = 0) SELECT 1 ELSE SELECT 0
END
