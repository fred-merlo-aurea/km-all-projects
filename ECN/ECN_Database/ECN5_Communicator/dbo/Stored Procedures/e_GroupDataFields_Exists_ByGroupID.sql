CREATE PROCEDURE [dbo].[e_GroupDataFields_Exists_ByGroupID] 
	@GroupID int = null,
	@CustomerID int = null
AS     
BEGIN     		
	IF EXISTS	(SELECT TOP 1 gdf.GroupDataFieldsID 
				FROM GroupDataFields gdf WITH (NOLOCK) join [Groups] g WITH (NOLOCK) on gdf.GroupID = g.GroupID
				WHERE gdf.GroupID = @GroupID AND g.CustomerID = @CustomerID AND gdf.IsDeleted = 0 
				) 
	SELECT 1 ELSE SELECT 0
END