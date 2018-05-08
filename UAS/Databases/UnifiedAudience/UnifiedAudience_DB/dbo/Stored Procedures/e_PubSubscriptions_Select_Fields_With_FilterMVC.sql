CREATE PROCEDURE [dbo].[e_PubSubscriptions_Select_Fields_With_FilterMVC]
(
	@Queries VARCHAR(MAX),
	@IssueID int = 0
)
AS
BEGIN

	SET NOCOUNT ON

	--DECLARE @Filters varchar(max) = '<XML><Filters><ProductID>1</ProductID></Filters></XML>'
	--DECLARE @AdHocFilters varchar(max) = '<XML><FilterDetail><FilterField>Qualificationdate</FilterField><SearchCondition>DateRange</SearchCondition><AdHocFromField>06/01/2015</AdHocFromField><AdHocToField>05/31/2016</AdHocToField><FilterObjectType>Standard</FilterObjectType></FilterDetail></XML>'
	
	CREATE TABLE #Subscriptions (PubSubscriptionID int)
	INSERT INTO #Subscriptions
	EXEC (@Queries)

	DECLARE @QSourceID int
	SET @QSourceID = (SELECT CodeTypeID FROM UAD_Lookup..CodeType WHERE CodeTypeName = 'Qualification Source')
	
	IF @IssueID = 0 --Query Current Issue
	BEGIN
		
		SELECT SequenceID, FirstName, LastName, cc.CategoryCodeValue as 'CategoryCode', tc.TransactionCodeValue as 'TransactionCode', c.CodeValue as 'QSource', demo7, Copies, Address1,
			   Address2, Address3, City, ZipCode, RegionCode, Country, Company, Title
		FROM PubSubscriptions ps
		JOIN #Subscriptions s ON s.PubSubscriptionID = ps.PubSubscriptionID
		LEFT JOIN UAD_Lookup..CategoryCode cc ON cc.CategoryCodeID = ps.PubCategoryID
		LEFT JOIN UAD_Lookup..TransactionCode tc ON tc.TransactionCodeID = ps.PubTransactionID
		LEFT JOIN UAD_Lookup..Code c ON c.CodeId = ps.PubQSourceID and c.CodeTypeId = @QSourceID
	END
	ELSE --Query Archive
	BEGIN
		
		SELECT SequenceID, FirstName, LastName, cc.CategoryCodeValue as 'CategoryCode', tc.TransactionCodeValue as 'TransactionCode', c.CodeValue as 'QSource', demo7, Copies, Address1,
			   Address2, Address3, City, ZipCode, RegionCode, Country, Company, Title
		FROM IssueArchiveProductSubscription ps
		JOIN #Subscriptions s ON s.PubSubscriptionID = ps.PubSubscriptionID
		LEFT JOIN UAD_Lookup..CategoryCode cc ON cc.CategoryCodeID = ps.PubCategoryID
		LEFT JOIN UAD_Lookup..TransactionCode tc ON tc.TransactionCodeID = ps.PubTransactionID
		LEFT JOIN UAD_Lookup..Code c ON c.CodeId = ps.PubQSourceID and c.CodeTypeId = @QSourceID
		WHERE IssueID = @IssueID
	END
	
	
	
	DROP TABLE #Subscriptions

END
