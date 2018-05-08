

CREATE VIEW [dbo].[vw_RecentConsensus]
AS

WITH CTE AS (
SELECT 
	psd.SubscriptionID,
	vw.MasterGroupID,
	MAX(psd.DateCreated) AS DateCreated
FROM
	dbo.PubSubscriptionDetail AS psd 
	INNER JOIN dbo.vw_Mapping AS vw ON vw.CodeSheetID = psd.CodesheetID
GROUP BY
	psd.SubscriptionID,
	vw.MasterGroupID
)
SELECT DISTINCT
	cte.SubscriptionID
	,mc.MasterID
	,cte.MasterGroupID
--	,cte.DateCreated
FROM 
	dbo.PubSubscriptionDetail AS psd 

	INNER JOIN dbo.CodeSheet_Mastercodesheet_Bridge mcb ON psd.CodeSheetID = mcb.CodeSheetID 
	INNER JOIN dbo.Mastercodesheet mc ON mcb.MasterID = mc.MasterID 
	INNER JOIN dbo.MasterGroups mg ON mc.MasterGroupID = mg.MasterGroupID
	JOIN CTE ON psd.SubscriptionID = cte.SubscriptionID AND psd.DateCreated = cte.dateCreated and mg.mastergroupid = cte.mastergroupid