CREATE PROCEDURE [o_SubscriberProductDemographic_Select_SubscriptionID_PubCode]
@SubscriptionID int,
@PubCode varchar(50)
AS
BEGIN

	SET NOCOUNT ON

	SELECT Upper(Results.ResponseGroupName) as Name, 
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
			FROM ResponseGroups rg With(NoLock)
				JOIN Pubs p With(NoLock) ON rg.PubID = p.PubID
			WHERE P.PubCode = @PubCode
		)
	Results
	GROUP BY results.ResponseGroupID, results.ResponseGroupName
	ORDER BY Name
END