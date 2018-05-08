CREATE PROCEDURE [dbo].[e_LinkAlias_Exists_ByAlias] 
	@AliasID int = NULL,
	@CustomerID int = NULL,
	@ContentID int = NULL,
	@Alias varchar (200) = NULL
AS     
BEGIN  
		IF EXISTS	(	
								
								SELECT TOP 1 la.AliasID 
								FROM 
									LinkAlias la with (nolock) 
									join Content c WITH (NOLOCK) on la.ContentID = c.ContentID 
								WHERE 
									la.AliasID != ISNULL(@AliasID, -1) and 
									c.CustomerID = @CustomerID and 
									la.ContentID = @ContentID and 
									la.Alias = @Alias and 
									la.IsDeleted = 0 and 
									c.IsDeleted = 0
							) 
				SELECT 1 ELSE SELECT 0
END