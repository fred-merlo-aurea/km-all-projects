CREATE PROCEDURE [dbo].[e_GroupDataFields_Select_DataFieldSetID]   
@GroupID int = NULL,
@DatafieldSetID int = NULL
AS
	SELECT gdf.*, g.CustomerID 
	FROM GroupDataFields gdf with (nolock) join [Groups] g with (nolock) on gdf.GroupID = g.GroupID
	WHERE gdf.GroupID = @GroupID and gdf.DatafieldSetID = @DatafieldSetID and gdf.IsDeleted = 0
