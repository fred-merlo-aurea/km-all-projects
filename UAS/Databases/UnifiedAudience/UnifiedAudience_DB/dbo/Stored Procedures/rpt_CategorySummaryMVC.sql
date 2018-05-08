CREATE PROCEDURE [dbo].[rpt_CategorySummaryMVC]
	 @Queries VARCHAR(MAX),
	 @IssueID int = 0 
AS
BEGIN
	
	SET NOCOUNT ON

	--DECLARE @Filters varchar(max) = '<XML><Filters><ProductID>1</ProductID></Filters></XML>'
	--DECLARE @AdHocFilters varchar(max) = '<XML></XML>'
	--DECLARE @IssueID int = 0
	
	IF 1=0 
		BEGIN
			SET FMTONLY OFF
		END
	
	DECLARE @DeliverID int = (SELECT CodeTypeID FROM UAD_Lookup..CodeType WHERE CodeTypeName = 'Deliver')
	
	SELECT DisplayName
	INTO #Demos
	FROM UAD_Lookup..Code 
	WHERE CodeTypeId = @DeliverID	
	
	 CREATE TABLE #SubscriptionID (SubscriptionID int)    
	 CREATE UNIQUE CLUSTERED INDEX IX_1 on #SubscriptionID (SubscriptionID)  
  
	 INSERT INTO #SubscriptionID
	 EXEC (@Queries)
	
	IF @IssueID = 0
		BEGIN
			
		
			SELECT ISNULL(cct.CategoryCodeTypeName, 'Invalid Category') as 'CategoryType', ISNULL(convert(varchar(10),cc.CategoryCodeValue) + ' - ' + cc.CategoryCodeName, 'Invalid') as 'Category', sum(ps.Copies) Copies, c.DisplayName as 'Demo7',
			ISNULL(cc.CategoryCodeID,0) as 'CategoryCodeID', c.CodeId as 'DemoID', count(distinct ps.pubsubscriptionID) as 'RecordCount', cc.CategoryCodeValue as 'CodeValue', cc.CategoryCodeTypeID as 'CodeTypeID', 
			CASE when cc.CategoryCodeTypeID = 1 THEN '1'
				when cc.CategoryCodeTypeID = 2 THEN '4'
				when cc.CategoryCodeTypeID = 3 THEN '2'
				when cc.CategoryCodeTypeID = 4 THEN '3' END AS 'Order'
			FROM PubSubscriptions ps
				JOIN #SubscriptionID s ON ps.PubSubscriptionID = s.SubscriptionID
				LEFT JOIN UAD_Lookup..CategoryCode cc ON cc.CategoryCodeID = ps.PubCategoryID
				LEFT JOIN UAD_Lookup..CategoryCodeType cct ON cct.CategoryCodeTypeID = cc.CategoryCodeTypeID
				JOIN UAD_Lookup..Code c ON c.CodeValue = ps.demo7
			WHERE c.CodeTypeId = @DeliverID
			group by ISNULL(cct.CategoryCodeTypeName, 'Invalid Category') , ISNULL(convert(varchar(10),cc.CategoryCodeValue) + ' - ' + cc.CategoryCodeName, 'Invalid'), c.DisplayName,
			ISNULL(cc.CategoryCodeID,0) , c.CodeId, cc.CategoryCodeValue, cc.CategoryCodeTypeID
		END
	ELSE
		BEGIN
			
		
			SELECT ISNULL(cct.CategoryCodeTypeName, 'Invalid Category') as 'CategoryType', ISNULL(convert(varchar(10),cc.CategoryCodeValue) + ' - ' + cc.CategoryCodeName, 'Invalid') as 'Category', sum(ps.Copies) Copies, c.DisplayName as 'Demo7',
			ISNULL(cc.CategoryCodeID,0) as 'CategoryCodeID', c.CodeId as 'DemoID', count(distinct ps.pubsubscriptionID) as 'RecordCount', cc.CategoryCodeValue as 'CodeValue', cc.CategoryCodeTypeID as 'CodeTypeID', 
			CASE when cc.CategoryCodeTypeID = 1 THEN '1'
				when cc.CategoryCodeTypeID = 2 THEN '4'
				when cc.CategoryCodeTypeID = 3 THEN '2'
				when cc.CategoryCodeTypeID = 4 THEN '3' END AS 'Order'
			FROM IssueArchiveProductSubscription ps
				JOIN #SubscriptionID s ON ps.PubSubscriptionID = s.SubscriptionID
				LEFT JOIN UAD_Lookup..CategoryCode cc ON cc.CategoryCodeID = ps.PubCategoryID
				LEFT JOIN UAD_Lookup..CategoryCodeType cct ON cct.CategoryCodeTypeID = cc.CategoryCodeTypeID
				JOIN UAD_Lookup..Code c ON c.CodeValue = ps.demo7
			WHERE c.CodeTypeId = @DeliverID AND ps.IssueID = @IssueID
			group by
				ISNULL(cct.CategoryCodeTypeName, 'Invalid Category') , ISNULL(convert(varchar(10),cc.CategoryCodeValue) + ' - ' + cc.CategoryCodeName, 'Invalid') , c.DisplayName,
			ISNULL(cc.CategoryCodeID,0) , c.CodeId, cc.CategoryCodeValue, cc.CategoryCodeTypeID
		END
	
	DROP TABLE #Demos
	DROP TABLE #SubscriptionID

END

go