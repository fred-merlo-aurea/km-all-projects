CREATE  PROC [dbo].[e_Customer_Exists_ByID] 
(
	@CustomerID int = NULL
)
AS 
BEGIN
	IF EXISTS (SELECT TOP 1 CustomerID from Customer  with (nolock) where CustomerID = @CustomerID AND IsDeleted = 0) SELECT 1 ELSE SELECT 0
END
