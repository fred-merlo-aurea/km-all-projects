CREATE PROCEDURE [dbo].[e_LinkAlias_Exists_ByContentID] 
	@CustomerID int = NULL,
	@ContentID int = NULL
AS     
BEGIN     		
	IF EXISTS	(
					SELECT TOP 1 la.AliasID 
					FROM LinkAlias la with (nolock) join Content c WITH (NOLOCK) on la.ContentID = c.ContentID 
					WHERE la.ContentID = @ContentID and c.CustomerID = @CustomerID and la.IsDeleted = 0 and c.IsDeleted = 0
				) 
	SELECT 1 ELSE SELECT 0
END