CREATE PROCEDURE [dbo].[e_ProductDetail_Select_All]      
AS    
BEGIN     
	SET NOCOUNT ON;    
    
    SELECT	*  
	FROM     
			ProductDetail WITH (NOLOCK)
END
