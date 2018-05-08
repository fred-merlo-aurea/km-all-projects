CREATE PROCEDURE [dbo].[e_GroupDataFields_Select_GroupDataFieldsID]   
@GroupDataFieldsID int,--this is a required parameter = NULL,
@GroupID int = NULL--this is no longer used in the query
AS
	SELECT gdf.*, g.CustomerID 
	FROM GroupDataFields gdf with (nolock) join [Groups] g with (nolock) on gdf.GroupID = g.GroupID
	WHERE gdf.GroupDataFieldsID = @GroupDataFieldsID AND gdf.IsDeleted = 0
