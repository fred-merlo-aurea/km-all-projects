CREATE PROCEDURE [dbo].[e_PubSubscriptions_Select_ResponsesMVC]
@Queries VARCHAR(MAX),
@IssueID int = 0
AS
BEGIN

	SET NOCOUNT ON

	--DECLARE @Filters varchar(max) = '<XML><Filters><ProductID>1</ProductID></Filters></XML>'
	--DECLARE @AdHocFilters varchar(max) = '<XML><FilterDetail><FilterField>SequenceID</FilterField><FilterObjectType>Standard</FilterObjectType><SearchCondition>Equal</SearchCondition><AdHocFieldValue>15219</AdHocFieldValue></FilterDetail></XML>'
	
	IF 1=0 BEGIN
    SET FMTONLY OFF
	END
	
	CREATE TABLE #Subscriptions (PubSubscriptionID int)
	INSERT INTO #Subscriptions
	EXEC (@Queries) 
	
	IF @IssueID = 0 --Query Current Issue
		BEGIN
			
			SELECT rg.DisplayName, cs.Responsevalue + '. ' + cs.Responsedesc as 'Response', ps.SequenceID, rg.DisplayOrder as 'GroupOrder', cs.DisplayOrder as 'ResponseOrder'
			FROM PubSubscriptionDetail psd
				JOIN PubSubscriptions ps ON ps.PubSubscriptionID = psd.PubSubscriptionID
				JOIN #Subscriptions s ON s.PubSubscriptionID = ps.PubSubscriptionID
				JOIN CodeSheet cs ON cs.CodeSheetID = psd.CodesheetID
				JOIN ResponseGroups rg ON rg.ResponseGroupID = cs.ResponseGroupID
		END
	ELSE --Query Archive
		BEGIN
			
			SELECT rg.DisplayName, cs.Responsevalue + '. ' + cs.Responsedesc as 'Response', ps.SequenceID, rg.DisplayOrder as 'GroupOrder', cs.DisplayOrder as 'ResponseOrder'
			FROM IssueArchiveProductSubscriptionDetail psd
				JOIN IssueArchiveProductSubscription ps ON ps.PubSubscriptionID = psd.PubSubscriptionID
				JOIN #Subscriptions s ON s.PubSubscriptionID = ps.PubSubscriptionID
				JOIN CodeSheet cs ON cs.CodeSheetID = psd.CodesheetID
				JOIN ResponseGroups rg ON rg.ResponseGroupID = cs.ResponseGroupID
		END
	
	DROP TABLE #Subscriptions

END