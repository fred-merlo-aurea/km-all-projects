CREATE PROCEDURE [dbo].[o_SubscriptionDetailKVP_Select_SubscriptionID]
@SubscriptionID int
AS
BEGIN
	
	SET NOCOUNT ON

	SELECT Upper(MastergroupName) as Name, 
		Upper(DisplayName) as DisplayName,
		STUFF((
			SELECT ',' + CAST([MasterValue] AS VARCHAR(MAX)) 
			FROM [dbo].[SubscriptionDetails] sd1 
				join Mastercodesheet mc1 on sd1.MasterID = mc1.MasterID  
			WHERE (sd1.SubscriptionID = @SubscriptionID and mc1.MasterGroupID = Results.MasterGroupID) 
			FOR XML PATH (''))
		,1,1,'') AS Value
	FROM 
	(
		SELECT distinct  mg.MasterGroupID, mg.Name MasterGroupName, mg.DisplayName
		FROM MasterGroups mg 
			join Mastercodesheet mc on mg.MasterGroupID = mc.MasterGroupID 
	) Results
	GROUP BY MasterGroupID, MastergroupName, DisplayName
	order by Name

End