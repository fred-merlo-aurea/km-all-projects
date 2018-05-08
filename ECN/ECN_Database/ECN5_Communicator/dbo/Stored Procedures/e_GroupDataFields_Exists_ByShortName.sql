CREATE PROCEDURE [dbo].[e_GroupDataFields_Exists_ByShortName] 
	@GroupID int = NULL,
	@GroupDatafieldsID int = NULL,
	@CustomerID int = NULL,
	@ShortName varchar(50) = NULL
AS     
BEGIN     		
	IF EXISTS (
				SELECT TOP 1 gdf.GroupDatafieldsID
				FROM GroupDatafields gdf WITH (NOLOCK) 
					join [Groups] g WITH (NOLOCK) on gdf.GroupID = g.GroupID
				WHERE 
					g.CustomerID = @CustomerID AND 
					gdf.GroupDatafieldsID != ISNULL(@GroupDatafieldsID, -1) AND 
					gdf.GroupID = @GroupID AND 
					gdf.ShortName = @ShortName AND 
					gdf.IsDeleted = 0
				) SELECT 1 ELSE SELECT 0
END

