CREATE PROCEDURE [dbo].[e_Filter_Exists_ByID] 
	@FilterID int,
	@CustomerID int
AS     
BEGIN     		
	IF EXISTS (SELECT TOP 1 FilterID FROM Filter WITH (NOLOCK) WHERE CustomerID = @CustomerID AND FilterID = @FilterID AND IsDeleted = 0) SELECT 1 ELSE SELECT 0
END