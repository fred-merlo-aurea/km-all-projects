CREATE PROCEDURE [dbo].[sp_GetEmailActivitySimpleUDFDataValues](
	@GroupID int,
	@EmailID int
)
AS
BEGIN
SET NOCOUNT ON
--Tristan McCormick 06/25/2014: This code was commented out for reasons that were not clear to me 
--and which broke the control that calls them emailProfile_UDF.ascx in ECNActivityEngines
--So I commented them back in! 
	----------------------------------------------------------------------
	SELECT DISTINCT g.GroupID, GroupName 
	FROM Groups g JOIN EmailGroups eg ON g.GroupID = eg.GroupID 
		JOIN GroupDataFields gdf on g.GroupID = gdf.GroupID 
	WHERE g.groupID = @groupID AND eg.SubscribeTypeCode = 'S' AND eg.emailID = @EmailID
	----------------------------------------------------------------------

SELECT 	gdf.GroupID, g.GroupName, gdf.ShortName, gdf.longName, isnull(e.DataValue,'') DataValue 
	FROM 	Groups g join EmailGroups eg on g.GroupID = eg.GroupID join
			GROUPDatafields gdf on g.GroupID = gdf.GroupID left outer join
			EmailDataValues e on gdf.GROUPDatafieldsID = e.GROUPDatafieldsID and e.emailID = eg.emailID
	WHERE 	eg.EmailID = @EmailID and g.GroupID = @GroupID  AND eg.SubscribeTypeCode = 'S' AND gdf.DataFieldSetID IS NULL AND gdf.surveyID IS NULL and gdf.IsDeleted = 0
end
