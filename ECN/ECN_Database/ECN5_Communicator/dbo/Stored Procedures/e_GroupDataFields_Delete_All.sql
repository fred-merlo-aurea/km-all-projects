CREATE  PROC [dbo].[e_GroupDataFields_Delete_All] 
(
	@GroupID int = NULL,
    @CustomerID int = NULL,
    @UserID int = NULL
)
AS 
BEGIN
	UPDATE gdf SET gdf.IsDeleted = 1, gdf.UpdatedDate = GETDATE(), gdf.UpdatedUserID = @UserID
	FROM GroupDatafields gdf
		JOIN [Groups] g WITH (NOLOCK) ON g.GroupID = gdf.GroupID
	WHERE g.CustomerID = @CustomerID AND gdf.GroupID = @GroupID
END
