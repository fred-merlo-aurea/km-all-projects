CREATE PROCEDURE [dbo].[e_User_Select_UserName]
	@CustomerID int = null,
	@UserName varchar(100) = null		
AS
BEGIN	
	SET NOCOUNT ON;

    SELECT * from [Users] with(nolock) where CustomerID = @CustomerID and UserName = @UserName and IsDeleted = 0
END
