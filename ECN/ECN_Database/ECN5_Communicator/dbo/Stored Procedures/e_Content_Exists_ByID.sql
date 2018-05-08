CREATE PROCEDURE [dbo].[e_Content_Exists_ByID] 
	@ContentID int,
	@CustomerID int
AS     
BEGIN     		
	IF EXISTS (SELECT TOP 1 ContentID FROM Content WITH (NOLOCK) WHERE CustomerID = @CustomerID AND ContentID = @ContentID AND IsDeleted = 0) SELECT 1 ELSE SELECT 0
END