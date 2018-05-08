CREATE PROCEDURE [dbo].[e_EmailDataValues_GetStandaloneUDFDataValues]
	(
		@GroupID int,
		@EmailID int
	)
AS

BEGIN
	set NOCOUNT ON
	
	SELECT gdf.GroupDatafieldsID, gdf.ShortName, edv.DataValue 
	from 
		GroupDatafields gdf  WITH (NOLOCK) 
		left outer join  [EMAILDATAVALUES] edv WITH (NOLOCK) on gdf.GroupDatafieldsID = edv.GroupDatafieldsID 
	WHERE gdf.IsDeleted = 0 AND isnull(gdf.DatafieldSetID,0) =0 and  gdf.GroupID= @GroupID  AND edv.EmailID=@EmailID  
	ORDER BY gdf.shortname asc

END