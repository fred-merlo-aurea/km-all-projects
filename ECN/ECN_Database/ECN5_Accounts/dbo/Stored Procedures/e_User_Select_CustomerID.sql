CREATE PROCEDURE [dbo].[e_User_Select_CustomerID]
	@CustomerID int		
AS
BEGIN	
	SET NOCOUNT ON;

    SELECT * from [Users] with(nolock) where CustomerID = @CustomerID and IsDeleted = 0
END
