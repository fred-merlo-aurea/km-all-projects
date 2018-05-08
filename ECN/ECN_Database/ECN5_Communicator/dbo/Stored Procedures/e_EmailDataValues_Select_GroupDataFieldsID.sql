CREATE PROCEDURE [dbo].[e_EmailDataValues_Select_GroupDataFieldsID]   
@GroupDataFieldsID int = NULL
AS
	SELECT edv.*, g.CustomerID
	FROM [EMAILDATAVALUES] edv WITH (NOLOCK)
		JOIN GroupDatafields gdf WITH (NOLOCK) ON edv.GroupDatafieldsID = gdf.GroupDatafieldsID 
		JOIN [Groups] g WITH (NOLOCK) ON gdf.GroupID = g.GroupID
	WHERE edv.GroupDatafieldsID = @GroupDataFieldsID and gdf.IsDeleted = 0