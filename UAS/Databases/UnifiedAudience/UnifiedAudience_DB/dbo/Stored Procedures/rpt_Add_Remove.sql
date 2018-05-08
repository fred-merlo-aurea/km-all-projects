CREATE proc [dbo].[rpt_Add_Remove]
(  
	@IssueID int,    
	@ProductID int, 
	@CategoryIDs varchar(800),
	@CategoryCodes varchar(800),
	@TransactionIDs varchar(800),
	@TransactionCodes varchar(800),
	@QsourceIDs varchar(800),
	@StateIDs varchar(800),
	@CountryIDs varchar(1500),
	@Email varchar(10),
	@Phone varchar(10),
	@Mobile varchar(10),
	@Fax varchar(10),
	@ResponseIDs varchar(1000),
	@Demo7 varchar(10),		
	@Year varchar(20),
	@startDate varchar(10),		
	@endDate varchar(10),
	@AdHocXML varchar(8000)      
)      
as    
BEGIN
	
	SET NOCOUNT ON

	DECLARE @magazineCode varchar(20)
	SELECT @magazineCode  = PubCode from Pubs where PubID = @ProductID

	CREATE TABLE #SubscriptionID (SubscriptionID int, copies int) 
	CREATE TABLE #ArchivedSubs (SubscriptionID int, IsPaid bit, FullName varchar(50))
	CREATE TABLE #CurrentSubs (SubscriptionID int, IsPaid bit, FullName varchar(50))  
    
	INSERT INTO #SubscriptionID   
	EXEC rpt_GetSubscriptionIDs_Copies_From_Filter 
		@ProductID, 
		@CategoryIDs,
		@CategoryCodes,
		@TransactionIDs,
		@TransactionCodes,
		@QsourceIDs,
		@StateIDs,
		@CountryIDs,
		@Email,
		@Phone,
		@Mobile,
		@Fax,
		@ResponseIDs,
		@Demo7,		
		@Year,
		@startDate,		
		@endDate,
		@AdHocXML 

	CREATE UNIQUE CLUSTERED INDEX IX_1 on #SubscriptionID (SubscriptionID)
	
	INSERT INTO #CurrentSubs
	Select DISTINCT s.SubscriptionID, IsPaid, (FirstName + ' ' + LastName) as 'FullName'
	FROM PubSubscriptions s
		JOIN #SubscriptionID sub ON s.SubscriptionID = sub.SubscriptionID
		JOIN UAD_Lookup..CategoryCode c on c.CategoryCodeValue = s.PubCategoryID
		JOIN UAD_Lookup..TransactionCode t on t.TransactionCodeValue = s.PubTransactionID
		JOIN UAD_Lookup..CategoryCodeType cg on cg.CategoryCodeTypeID = c.CategoryCodeTypeID
		JOIN UAD_Lookup..TransactionCodeType tg on tg.TransactionCodeTypeID = t.TransactionCodeTypeID
	WHERE s.PubID = @ProductID
		AND cg.CategoryCodeTypeID in (1,2,3,4) AND c.CategoryCodeValue not in (70,71)
		AND tg.TransactionCodeTypeID in (1,3)
	
	INSERT INTO #ArchivedSubs
	Select DISTINCT s.SubscriptionID, IsPaid, (FirstName + ' ' + LastName) as 'FullName'
	FROM IssueArchiveProductSubscription s
		JOIN #SubscriptionID sub ON s.SubscriptionID = sub.SubscriptionID
		JOIN UAD_Lookup..CategoryCode c on c.CategoryCodeValue = s.PubCategoryID
		JOIN UAD_Lookup..TransactionCode t on t.TransactionCodeValue = s.PubTransactionID
		JOIN UAD_Lookup..CategoryCodeType cg on cg.CategoryCodeTypeID = c.CategoryCodeTypeID
		JOIN UAD_Lookup..TransactionCodeType tg on tg.TransactionCodeTypeID = t.TransactionCodeTypeID
	WHERE s.PubID = @ProductID AND s.IssueID = @IssueID
		AND cg.CategoryCodeTypeID in (1,2,3,4) AND c.CategoryCodeValue not in (70,71)
		AND tg.TransactionCodeTypeID in (1,3)
	
	Select * 
	FROM (SELECT COUNT(*) as 'Adds' 
		FROM #CurrentSubs c
			LEFT OUTER JOIN (SELECT SubscriptionID, IsPaid, FullName FROM #ArchivedSubs a) inn ON inn.SubscriptionID = c.SubscriptionID
		WHERE inn.SubscriptionID is null or inn.IsPaid <> c.IsPaid or c.FullName <> inn.FullName
		) adds
		CROSS JOIN (SELECT COUNT(*) as 'Removes' 
			FROM #CurrentSubs c
				RIGHT OUTER JOIN (SELECT SubscriptionID, IsPaid, FullName FROM #ArchivedSubs a) inn ON inn.SubscriptionID = c.SubscriptionID
			WHERE c.SubscriptionID is null or inn.IsPaid <> c.IsPaid or c.FullName <> inn.FullName
		) removes
	
	DROP TABLE #ArchivedSubs
	DROP TABLE #CurrentSubs
	DROP TABLE #SubscriptionID
		
END