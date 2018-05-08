﻿CREATE  PROC [dbo].[e_EmailDataValues_Delete_Single] 
(
	@GroupID int = NULL,
	@GroupDataFieldsID int = NULL,
    @CustomerID int = NULL,
    @UserID int = NULL
)
AS 
BEGIN

	DELETE From edv
	FROM [EMAILDATAVALUES] edv
		JOIN GroupDatafields gdf WITH (NOLOCK) ON edv.GroupDatafieldsID = gdf.GroupDatafieldsID
		JOIN [Groups] g WITH (NOLOCK) ON g.GroupID = gdf.GroupID
	WHERE g.CustomerID = @CustomerID AND gdf.GroupID = @GroupID and edv.GroupDatafieldsID = @GroupDataFieldsID
	
	--UPDATE edv SET edv.IsDeleted = 1, edv.UpdatedDate = GETDATE(), edv.UpdatedUserID = @UserID
	--FROM [EMAILDATAVALUES] edv
	--	JOIN GroupDatafields gdf WITH (NOLOCK) ON edv.GroupDatafieldsID = gdf.GroupDatafieldsID
	--	JOIN [Groups] g WITH (NOLOCK) ON g.GroupID = gdf.GroupID
	--WHERE g.CustomerID = @CustomerID AND edv.GroupDatafieldsID = @GroupDataFieldsID
END
