CREATE PROCEDURE [dbo].[e_PubSubscriptions_Select_All_Fields_With_Filter]
(
	@Filters TEXT = '<XML><Filters></Filters></XML>',
	@AdHocFilters TEXT = '<XML></XML>',
	@IssueID int = 0
)
AS
BEGIN

	SET NOCOUNT ON

	--DECLARE @Filters varchar(max) = '<XML><Filters><ProductID>1</ProductID></Filters></XML>'
	--DECLARE @AdHocFilters varchar(max) = '<XML><FilterDetail><FilterField>SequenceID</FilterField><FilterObjectType>Standard</FilterObjectType><SearchCondition>Equal</SearchCondition><AdHocFieldValue>15219</AdHocFieldValue></FilterDetail></XML>'
	
	IF 1=0 BEGIN
    SET FMTONLY OFF
	END
	
	CREATE TABLE #Subscriptions (PubSubscriptionID int)
	DECLARE @QSourceID int
	SET @QSourceID = (SELECT CodeTypeID FROM UAD_Lookup..CodeType WHERE CodeTypeName = 'Qualification Source')
	
	IF @IssueID = 0
		BEGIN
			INSERT INTO #Subscriptions
			EXEC rpt_GetSubscriptionIDs_Copies_From_Filter_XML @Filters, @AdHocFilters
		
			SELECT ps.PubSubscriptionID, SequenceID, Qualificationdate, FirstName, LastName, Email, cc.CategoryCodeValue as 'CategoryCode', tc.TransactionCodeValue as 'TransactionCode', c.CodeValue as 'QSource', 
				demo7, Copies, Company, Title, Address1, Address2, Address3, City, ZipCode, Plus4, Phone, Mobile, Fax, Website, RegionCode, Country, SubscriberSourceCode, OrigsSrc, Count(*) as 'ResponseCount'
			FROM PubSubscriptions ps
				JOIN #Subscriptions s ON s.PubSubscriptionID = ps.PubSubscriptionID
				LEFT JOIN PubSubscriptionDetail psd ON psd.PubSubscriptionID = ps.PubSubscriptionID
				LEFT JOIN UAD_Lookup..CategoryCode cc ON cc.CategoryCodeID = ps.PubCategoryID
				LEFT JOIN UAD_Lookup..TransactionCode tc ON tc.TransactionCodeID = ps.PubTransactionID
				LEFT JOIN UAD_Lookup..Code c ON c.CodeId = ps.PubQSourceID and c.CodeTypeId = @QSourceID
			GROUP BY ps.PubSubscriptionID, SequenceID, Qualificationdate, FirstName, LastName, Email, cc.CategoryCodeValue, tc.TransactionCodeValue, c.CodeValue, 
				demo7, Copies, Company, Title, Address1, Address2, Address3, City, ZipCode, Plus4, Phone, Mobile, Fax, Website, RegionCode, Country, SubscriberSourceCode, OrigsSrc
		END
	ELSE
		BEGIN
			INSERT INTO #Subscriptions
			EXEC rpt_GetSubscriptionIDs_Copies_From_Filter_XML @Filters, @AdHocFilters, 1, @IssueID
		
			SELECT ps.PubSubscriptionID, SequenceID, Qualificationdate, FirstName, LastName, Email, cc.CategoryCodeValue as 'CategoryCode', tc.TransactionCodeValue as 'TransactionCode', c.CodeValue as 'QSource', 
				demo7, Copies, Company, Title, Address1, Address2, Address3, City, ZipCode, Plus4, Phone, Mobile, Fax, Website, RegionCode, Country, SubscriberSourceCode, OrigsSrc, Count(*) as 'ResponseCount'
			FROM IssueArchiveProductSubscription ps
				JOIN #Subscriptions s ON s.PubSubscriptionID = ps.PubSubscriptionID
				LEFT JOIN IssueArchiveProductSubscriptionDetail psd ON psd.PubSubscriptionID = ps.PubSubscriptionID
				LEFT JOIN UAD_Lookup..CategoryCode cc ON cc.CategoryCodeID = ps.PubCategoryID
				LEFT JOIN UAD_Lookup..TransactionCode tc ON tc.TransactionCodeID = ps.PubTransactionID
				LEFT JOIN UAD_Lookup..Code c ON c.CodeId = ps.PubQSourceID and c.CodeTypeId = @QSourceID
			GROUP BY ps.PubSubscriptionID, SequenceID, Qualificationdate, FirstName, LastName, Email, cc.CategoryCodeValue, tc.TransactionCodeValue, c.CodeValue, 
				demo7, Copies, Company, Title, Address1, Address2, Address3, City, ZipCode, Plus4, Phone, Mobile, Fax, Website, RegionCode, Country, SubscriberSourceCode, OrigsSrc	
		END
	DROP TABLE #Subscriptions
END