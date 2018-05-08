CREATE PROCEDURE [dbo].[e_PubSubscriptions_Select_Responses]
@Filters TEXT = '<XML><ProductID>1</ProductID></XML>',
@AdHocFilters TEXT = '<XML></XML>',
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
	
	IF @IssueID = 0 --Query Current Issue
		BEGIN
			INSERT INTO #Subscriptions
			EXEC rpt_GetSubscriptionIDs_Copies_From_Filter_XML @Filters, @AdHocFilters
		
			SELECT rg.DisplayName, cs.Responsevalue + '. ' + cs.Responsedesc as 'Response', ps.SequenceID, rg.DisplayOrder as 'GroupOrder', cs.DisplayOrder as 'ResponseOrder'
			FROM PubSubscriptionDetail psd
				JOIN PubSubscriptions ps ON ps.PubSubscriptionID = psd.PubSubscriptionID
				JOIN #Subscriptions s ON s.PubSubscriptionID = ps.PubSubscriptionID
				JOIN CodeSheet cs ON cs.CodeSheetID = psd.CodesheetID
				JOIN ResponseGroups rg ON rg.ResponseGroupID = cs.ResponseGroupID
		END
	ELSE --Query Archive
		BEGIN
			INSERT INTO #Subscriptions
			EXEC rpt_GetSubscriptionIDs_Copies_From_Filter_XML @Filters, @AdHocFilters, 1, @IssueID
		
			SELECT rg.DisplayName, cs.Responsevalue + '. ' + cs.Responsedesc as 'Response', ps.SequenceID, rg.DisplayOrder as 'GroupOrder', cs.DisplayOrder as 'ResponseOrder'
			FROM IssueArchiveProductSubscriptionDetail psd
				JOIN IssueArchiveProductSubscription ps ON ps.PubSubscriptionID = psd.PubSubscriptionID
				JOIN #Subscriptions s ON s.PubSubscriptionID = ps.PubSubscriptionID
				JOIN CodeSheet cs ON cs.CodeSheetID = psd.CodesheetID
				JOIN ResponseGroups rg ON rg.ResponseGroupID = cs.ResponseGroupID
		END
	
	DROP TABLE #Subscriptions

END