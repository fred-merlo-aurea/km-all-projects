CREATE PROCEDURE [dbo].[e_Select_Users_CustomerID]
	@CustomerID int		
AS
BEGIN	
	SET NOCOUNT ON;

    SELECT * from Users with(nolock) where CustomerID = @CustomerID
END
