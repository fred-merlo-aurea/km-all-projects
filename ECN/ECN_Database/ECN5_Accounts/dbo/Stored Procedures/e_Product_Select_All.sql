CREATE PROCEDURE [dbo].[e_Product_Select_All]  
AS
BEGIN	
	SET NOCOUNT ON;

    SELECT 
		*
	FROM 
		Product WITH (NOLOCK)
END
