CREATE PROCEDURE [dbo].[o_PubSubscriptionDetailKVP_Select_SubscriptionID_PubCode]
@SubscriptionID int,
@PubCode varchar(50)
AS
BEGIN

	SET NOCOUNT ON

		SELECT 
			Upper(Results.ResponseGroupName) as Name, 
			STUFF((
				SELECT ',' + CAST(c.[Responsevalue] AS VARCHAR(MAX)) 
				FROM PubSubscriptionDetail d With(NoLock) 
					JOIN CodeSheet c With(NoLock) ON d.CodesheetID = c.CodeSheetID 
				WHERE (d.SubscriptionID = @SubscriptionID and c.ResponseGroupID = Results.ResponseGroupID) 
				FOR XML PATH (''))
				,1,1,'') AS Value
		FROM 
			(
				SELECT distinct  rg.ResponseGroupID, rg.ResponseGroupName
				FROM ResponseGroups rg 
					JOIN Pubs p With(NoLock) ON rg.PubID = p.PubID
				where P.PubCode = @PubCode
			)
		Results
		GROUP BY results.ResponseGroupID, results.ResponseGroupName
		order by Name	

	--SELECT rg.ResponseGroupName as 'Name',c.Responsevalue as 'Value'
	--FROM 
	--		PubSubscriptions ps With(NoLock) 	
	--		JOIN Pubs p With(NoLock) ON ps.PubID = p.PubID
	--		JOIN PubSubscriptionDetail d With(NoLock) ON d.PubSubscriptionID = ps.PubSubscriptionID 
	--		JOIN CodeSheet c With(NoLock) ON d.CodesheetID = c.CodeSheetID 
	--		JOIN ResponseGroups rg  With(NoLock) on rg.ResponseGroupID = c.ResponseGroupID
	--WHERE d.SubscriptionID = @SubscriptionID AND p.PubCode = @PubCode

End