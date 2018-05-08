CREATE PROCEDURE [dbo].[Usp_MergeSubscriberMasterValues]
@GroupName VARCHAR(100)
AS 
BEGIN
	
	SET NOCOUNT ON

	CREATE TABLE #MasterGroupList (MasterGroupId INT)

	INSERT INTO #MasterGroupList
	SELECT DISTINCT(mg.MasterGroupID) 
	FROM MasterGroups mg WITH (NOLOCK)
		JOIN Mastercodesheet mc WITH (NOLOCK)ON mg.MasterGroupID = mc.MasterGroupID
		LEFT JOIN CodeSheet_Mastercodesheet_Bridge  cmb WITH(NOLOCK) ON cmb.MasterID = mc.MasterID   
		LEFT JOIN CodeSheet cs WITH (NOLOCK) ON cmb.CodeSheetID= cs.CodeSheetID
		LEFT JOIN ResponseGroups rg WITH (NOLOCK) ON cs.PubID = rg.pubID AND cs.ResponseGroupID  = rg.ResponseGroupID 
	WHERE rg.ResponseGroupName = @GroupName
		OR mg.Name = @GroupName


	/* Build Temp Table of MasteValues Pivoted into a Comma delimited String for Merge WITH Existing Table */


	SELECT MasterGroupID, 
		SubscriptionID, 
		LEFT(STUFF((
			SELECT ',' + IsNull(CAST([MasterValue] AS VARCHAR(MAX)),'')			-- added IsNull by chuck,  null values from Pennwell caused problems
			FROM SubscriptionDetails sd1 
				JOIN Mastercodesheet mc1 on sd1.MasterID = mc1.MasterID  
			WHERE (sd1.SubscriptionID = Results.SubscriptionID AND mc1.MasterGroupID = Results.MasterGroupID) 	
			FOR XML PATH ('')),1,1,''),8000) AS CombinedValues
			INTO #tmpCombined
		FROM 
			(
			SELECT 	DISTINCT sd.SubscriptionID, 
				mg.MasterGroupID
			FROM SubscriptionDetails sd 
				JOIN Mastercodesheet mc on sd.MasterID = mc.MasterID 
				JOIN MasterGroups mg on mg.MasterGroupID = mc.MasterGroupID
				JOIN #MasterGroupList mgl ON mg.MasterGroupID = mgl.MasterGroupId
				where mc.MasterValue is null									-- added by chuck,  null values from Pennwell caused problems
			) Results
		GROUP BY SubscriptionID, 
			MasterGroupID
		ORDER BY SubscriptionID

	CREATE INDEX IDX_#tmpCombined ON #tmpCombined (SubscriptionId)

	/*Merge values into SubscriberMasterValues.  Insert if not exists, othewise Update*/

	MERGE SubscriberMasterValues AS tgt
	USING #tmpCombined AS src
	ON (tgt.MasterGroupId = src.MasterGroupID AND tgt.SubscriptionId = src.SubscriptionId) 

	WHEN NOT MATCHED BY TARGET 
	THEN INSERT(MasterGroupID, SubscriptionID, MastercodesheetValues) VALUES(src.MasterGroupID, src.SubscriptionID, src.CombinedValues)

	WHEN MATCHED 
	THEN UPDATE SET tgt.MastercodesheetValues = src.CombinedValues;

	--WHEN NOT MATCHED BY SOURCE THEN DELETE ;
	--OUTPUT $action, inserted.*, deleted.*;

	DELETE S
	FROM SubscriberMasterValues s
		LEFT JOIN #tmpCombined t ON t.MasterGroupId = s.MasterGroupID AND t.SubscriptionId = s.SubscriptionId
	WHERE t.subscriptionId IS NULL
		AND s.MasterGroupId IN (SELECT MasterGroupID FROM #MasterGroupList)


	DROP TABLE #tmpCombined
	DROP TABLE #MasterGroupList

END