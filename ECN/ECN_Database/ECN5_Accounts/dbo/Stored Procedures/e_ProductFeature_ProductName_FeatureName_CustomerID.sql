CREATE PROCEDURE [dbo].[e_ProductFeature_ProductName_FeatureName_CustomerID]
	@ProductName varchar(250),
	@FeatureName varchar(250), 
	@customerID int
AS
BEGIN	
	SET NOCOUNT ON;

    SELECT cp.Active
    FROM [CustomerProduct] cp JOIN [Productdetail] pd ON cp.ProductDetailID = pd.ProductDetailID 
    AND ProductDetailName = @FeatureName
    JOIN [Product] p ON pd.ProductID = p.ProductID AND p.ProductName = @ProductName
    WHERE CustomerID = @customerID
END
