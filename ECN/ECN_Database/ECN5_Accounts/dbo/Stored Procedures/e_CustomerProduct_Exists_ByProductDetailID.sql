create  PROC [dbo].[e_CustomerProduct_Exists_ByProductDetailID] 
(
	@ProductDetailID int = NULL,
	@CustomerID int = NULL
)
AS 
BEGIN
	IF EXISTS (
		SELECT TOP 1 CustomerProductID
		from 
			CustomerProduct  with (nolock)
		where 
			ProductDetailID = @ProductDetailID AND IsDeleted = 0 and CustomerID = @CustomerID
	) SELECT 1 ELSE SELECT 0
END
