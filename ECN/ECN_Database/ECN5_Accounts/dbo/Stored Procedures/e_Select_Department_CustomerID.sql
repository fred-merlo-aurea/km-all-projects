CREATE PROCEDURE [dbo].[e_Select_Department_CustomerID]
	@CustomerID int		
AS
BEGIN	
	SET NOCOUNT ON;

    SELECT * from CustomerDepartments with(nolock) where CustomerID = @CustomerID
END
