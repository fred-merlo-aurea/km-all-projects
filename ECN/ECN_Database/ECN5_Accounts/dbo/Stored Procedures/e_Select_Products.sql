CREATE PROCEDURE [dbo].[e_Select_Products]  
AS
BEGIN	
	SET NOCOUNT ON;

    SELECT 
		ProductID, 
		ProductName        
	FROM 
		[Product] WITH (NOLOCK) 
END
