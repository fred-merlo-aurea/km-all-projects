CREATE   proc [dbo].[rpt_Geo_Breakdown_Domestic]  --new
(  
 @Filters varchar(max) ='<XML><Filters></Filters></XML>',  
 @AdHocFilters varchar(max) = '<XML></XML>',  
 @IncludeAddRemove bit = 0,  
 @IssueID int  
)  
as

--declare
--	@Filters varchar(max) ='<XML><Filters></Filters></XML>',
--	@AdHocFilters varchar(max) = '<XML></XML>',
--	@IncludeAddRemove bit = 0,
--	@IssueID int

--set @IssueID=0
--set @Filters=N'<XML><Filters><ProductID>3</ProductID><CategoryCode>101,102,103,130,115,116,117,118,119,120,121,122</CategoryCode><CategoryCodeType>1,3</CategoryCodeType><TransactionCode>101,140,141,111,112,113,114,115,116,102,117,103,137,145,146,138,139,104,136,118,119,120,121,122,123,124,125,142,143</TransactionCode><TransactionCodeType>1,3</TransactionCodeType></Filters></XML>'
--set @AdHocFilters=N'<XML></XML>'
--set @IncludeAddRemove=0


BEGIN 
	--Declare @Filters varchar(max) ='<XML><Filters><ProductID>3</ProductID><CategoryCode>101,102,103,115,116,117,118,119,120,121,122</CategoryCode><TransactionType>1,3</TransactionType></Filters></XML>'
	--DECLARE @IncludeAddRemove bit = 0
	--DECLARE @IssueID int = 2
	--DECLARE @AdHocFilters varchar(max) = '<XML></XML>'

	SET NOCOUNT ON
		
	IF 1=0 BEGIN
     SET FMTONLY OFF
	END
		
	DECLARE @DeliverID int = (SELECT CodeTypeID FROM UAD_Lookup..CodeType WHERE CodeTypeName = 'Deliver')
	DECLARE @USID int = (SELECT CountryID FROM UAD_Lookup..Country c WHERE c.ShortName = 'UNITED STATES')
	
	--SELECT DisplayName, CodeId
	--INTO #Demos
	--FROM UAD_Lookup..Code 
	--WHERE CodeTypeId = @DeliverID	

	CREATE TABLE #PubSubscriptionID (PubSubscriptionID int)  
	CREATE UNIQUE CLUSTERED INDEX IX_1 on #PubSubscriptionID (PubSubscriptionID)

	IF @IssueID = 0
	BEGIN
		INSERT INTO #PubSubscriptionID   
		EXEC rpt_GetSubscriptionIDs_Copies_From_Filter_XML 
		@Filters, @AdHocFilters, @IncludeAddRemove
		
		SELECT ISNULL(rg.RegionGroupName, 'No Region') as 'Region', ISNULL(NULLIF(r.ZipCodeRange + ' ', 'NULL'),'') + ISNULL(r.RegionName, 'No Region') as 'State & Zip Code', 
		cct.CategoryCodeTypeName, c.DisplayName as Demo7, ISNULL(c.CodeId, 0) as 'DemoID',
		(CASE WHEN cct.CategoryCodeTypeName LIKE 'Qualified%' THEN 'Qualified' ELSE 'Non Qualified' END) as 'Category Type', ISNULL(NULLIF(r.RegionCode, ''), 'ZZ') as 'RegionCode',
		ISNULL(r.RegionID, 0) as 'RegionID', sum(ps.Copies) as Copies, (case when r.ZipCodeRangeSortOrder = 0 then 1000 else isnull(r.ZipCodeRangeSortOrder, 1001)end ) as ZipCodeRangeSortOrder,
		ps.CountryID,'UNITED STATES' as 'CountryRecap',1 as 'RecapOrder'
		FROM PubSubscriptions ps with(NOLOCK)
		JOIN #PubSubscriptionID s ON s.PubSubscriptionID = ps.PubSubscriptionID
		LEFT JOIN UAD_Lookup..Region r ON r.RegionID = ps.RegionID
		LEFT JOIN UAD_Lookup..RegionGroup rg ON rg.RegionGroupID = r.RegionGroupID
		JOIN UAD_Lookup..CategoryCode cc ON cc.CategoryCodeID = ps.PubCategoryID
		JOIN UAD_Lookup..CategoryCodeType cct ON cct.CategoryCodeTypeID = cc.CategoryCodeTypeID
		JOIN UAD_Lookup..Code c ON c.CodeValue = ps.demo7
		WHERE c.CodeTypeId = @DeliverID AND ps.CountryID = @USID and rg.RegionGroupName != 'Territories'
		group by ISNULL(rg.RegionGroupName, 'No Region') , ISNULL(NULLIF(r.ZipCodeRange + ' ', 'NULL'),'') + ISNULL(r.RegionName, 'No Region') , 
		cct.CategoryCodeTypeName, c.DisplayName, ISNULL(c.CodeId, 0),
		(CASE WHEN cct.CategoryCodeTypeName LIKE 'Qualified%' THEN 'Qualified' ELSE 'Non Qualified' END) , ISNULL(NULLIF(r.RegionCode, ''), 'ZZ') ,
		ISNULL(r.RegionID, 0) , r.ZipCodeRangeSortOrder,ps.CountryID
		--order by r.ZipCodeRangeSortOrder
		union --Territories

		SELECT 'TERRITORIES' as 'Region', '' as 'State & Zip Code', 
		cct.CategoryCodeTypeName, c.DisplayName as Demo7, ISNULL(c.CodeId, 0) as 'DemoID',
		(CASE WHEN cct.CategoryCodeTypeName LIKE 'Qualified%' THEN 'Qualified' ELSE 'Non Qualified' END) as 'Category Type', 'TERRITORIES' as 'RegionCode',
		0 as 'RegionID', sum(ps.Copies) as Copies, 1000 as ZipCodeRangeSortOrder,ps.CountryID,'TERRITORIES' as 'CountryRecap',2 as 'RecapOrder'
		FROM PubSubscriptions ps with(NOLOCK)
		JOIN #PubSubscriptionID s ON s.PubSubscriptionID = ps.PubSubscriptionID
		LEFT JOIN UAD_Lookup..Region r ON r.RegionID = ps.RegionID
		LEFT JOIN UAD_Lookup..RegionGroup rg ON rg.RegionGroupID = r.RegionGroupID
		JOIN UAD_Lookup..CategoryCode cc ON cc.CategoryCodeID = ps.PubCategoryID
		JOIN UAD_Lookup..CategoryCodeType cct ON cct.CategoryCodeTypeID = cc.CategoryCodeTypeID
		JOIN UAD_Lookup..Code c ON c.CodeValue = ps.demo7
		WHERE c.CodeTypeId = @DeliverID AND ps.CountryID = @USID and rg.RegionGroupName = 'Territories'
		group by cct.CategoryCodeTypeName, c.DisplayName, ISNULL(c.CodeId, 0),
		(CASE WHEN cct.CategoryCodeTypeName LIKE 'Qualified%' THEN 'Qualified' ELSE 'Non Qualified' END) , r.ZipCodeRangeSortOrder,ps.CountryID

		union--Canada

		SELECT 'CANADA' as 'Region', '' as 'State & Zip Code', 
		cct.CategoryCodeTypeName, c.DisplayName as Demo7, ISNULL(c.CodeId, 0) as 'DemoID',
		(CASE WHEN cct.CategoryCodeTypeName LIKE 'Qualified%' THEN 'Qualified' ELSE 'Non Qualified' END) as 'Category Type', 'CANADA' as 'RegionCode',
		0 as 'RegionID', sum(ps.Copies) as Copies,1001 as 'ZipCodeRangeSortOrder',ps.CountryID,'CANADA' as 'CountryRecap',3 as 'RecapOrder'
		FROM PubSubscriptions ps with(NOLOCK)
		JOIN #PubSubscriptionID s ON s.PubSubscriptionID = ps.PubSubscriptionID
		JOIN UAD_Lookup..CategoryCode cc ON cc.CategoryCodeID = ps.PubCategoryID
		JOIN UAD_Lookup..CategoryCodeType cct ON cct.CategoryCodeTypeID = cc.CategoryCodeTypeID
		JOIN UAD_Lookup..Code c ON c.CodeValue = ps.demo7
		WHERE ps.CountryID=2 and
		c.CodeTypeId = @DeliverID
		group by cct.CategoryCodeTypeName, c.DisplayName, ISNULL(c.CodeId, 0),
		(CASE WHEN cct.CategoryCodeTypeName LIKE 'Qualified%' THEN 'Qualified' ELSE 'Non Qualified' END),ps.CountryID

		union--mexico

		SELECT 'MEXICO' as 'Region', '' as 'State & Zip Code', 
		cct.CategoryCodeTypeName, c.DisplayName as Demo7, ISNULL(c.CodeId, 0) as 'DemoID',
		(CASE WHEN cct.CategoryCodeTypeName LIKE 'Qualified%' THEN 'Qualified' ELSE 'Non Qualified' END) as 'Category Type', 'MEXICO' as 'RegionCode',
		0 as 'RegionID', sum(ps.Copies) as Copies,1002 as 'ZipCodeRangeSortOrder',ps.CountryID,'MEXICO' as 'CountryRecap',4 as 'RecapOrder'
		FROM PubSubscriptions ps with(NOLOCK)
		JOIN #PubSubscriptionID s ON s.PubSubscriptionID = ps.PubSubscriptionID
		JOIN UAD_Lookup..CategoryCode cc ON cc.CategoryCodeID = ps.PubCategoryID
		JOIN UAD_Lookup..CategoryCodeType cct ON cct.CategoryCodeTypeID = cc.CategoryCodeTypeID
		JOIN UAD_Lookup..Code c ON c.CodeValue = ps.demo7
		WHERE ps.CountryID=429 and
		c.CodeTypeId = @DeliverID
		group by cct.CategoryCodeTypeName, c.DisplayName, ISNULL(c.CodeId, 0),
		(CASE WHEN cct.CategoryCodeTypeName LIKE 'Qualified%' THEN 'Qualified' ELSE 'Non Qualified' END),ps.CountryID

		union--foreign

		SELECT 'FOREIGN' as 'Region', '' as 'State & Zip Code', 
		cct.CategoryCodeTypeName, c.DisplayName as Demo7, ISNULL(c.CodeId, 0) as 'DemoID',
		(CASE WHEN cct.CategoryCodeTypeName LIKE 'Qualified%' THEN 'Qualified' ELSE 'Non Qualified' END) as 'Category Type', 'FOREIGN' as 'RegionCode',
		0 as 'RegionID', sum(ps.Copies) as Copies,1003 as 'ZipCodeRangeSortOrder',9999,'FOREIGN' as 'CountryRecap',5 as 'RecapOrder'
		FROM PubSubscriptions ps with(NOLOCK)
		JOIN #PubSubscriptionID s ON s.PubSubscriptionID = ps.PubSubscriptionID
		JOIN UAD_Lookup..CategoryCode cc ON cc.CategoryCodeID = ps.PubCategoryID
		JOIN UAD_Lookup..CategoryCodeType cct ON cct.CategoryCodeTypeID = cc.CategoryCodeTypeID
		JOIN UAD_Lookup..Code c ON c.CodeValue = ps.demo7
		WHERE ps.RegionCode='FO' and 
		c.CodeTypeId = @DeliverID and 
		ps.CountryID not in (1,2,429)
		group by cct.CategoryCodeTypeName, c.DisplayName, ISNULL(c.CodeId, 0),
		(CASE WHEN cct.CategoryCodeTypeName LIKE 'Qualified%' THEN 'Qualified' ELSE 'Non Qualified' END)

		order by 'ZipCodeRangeSortOrder'

	END
	ELSE
	BEGIN
		INSERT INTO #PubSubscriptionID   
		EXEC rpt_GetSubscriptionIDs_Copies_From_Filter_XML 
		@Filters, @AdHocFilters, @IncludeAddRemove, 1, @IssueID
		
		SELECT ISNULL(rg.RegionGroupName, 'No Region') as 'Region', ISNULL(NULLIF(r.ZipCodeRange + ' ', 'NULL'),'') + ISNULL(r.RegionName, 'No Region') as 'State & Zip Code', 
		cct.CategoryCodeTypeName, c.DisplayName as Demo7, ISNULL(c.CodeId, 0) as 'DemoID',
		(CASE WHEN cct.CategoryCodeTypeName LIKE 'Qualified%' THEN 'Qualified' ELSE 'Non Qualified' END) as 'Category Type', ISNULL(NULLIF(r.RegionCode, ''), 'ZZ') as 'RegionCode',
		ISNULL(r.RegionID, 0) as 'RegionID', sum(ps.Copies) as Copies, (case when r.ZipCodeRangeSortOrder = 0 then 1000 else isnull(r.ZipCodeRangeSortOrder, 1001)end ) as ZipCodeRangeSortOrder,
		ps.CountryID,'UNITED STATES' as 'CountryRecap',1 as 'RecapOrder'
		FROM IssueArchiveProductSubscription ps with(NOLOCK)
		JOIN #PubSubscriptionID s ON s.PubSubscriptionID = ps.PubSubscriptionID
		LEFT JOIN UAD_Lookup..Region r ON r.RegionID = ps.RegionID
		LEFT JOIN UAD_Lookup..RegionGroup rg ON rg.RegionGroupID = r.RegionGroupID
		JOIN UAD_Lookup..CategoryCode cc ON cc.CategoryCodeID = ps.PubCategoryID
		JOIN UAD_Lookup..CategoryCodeType cct ON cct.CategoryCodeTypeID = cc.CategoryCodeTypeID
		JOIN UAD_Lookup..Code c ON c.CodeValue = ps.demo7
		WHERE c.CodeTypeId = @DeliverID AND ps.CountryID = @USID and rg.RegionGroupName != 'Territories'
		AND ps.IssueID = @IssueID
		group by ISNULL(rg.RegionGroupName, 'No Region') , ISNULL(NULLIF(r.ZipCodeRange + ' ', 'NULL'),'') + ISNULL(r.RegionName, 'No Region') , 
		cct.CategoryCodeTypeName, c.DisplayName, ISNULL(c.CodeId, 0),
		(CASE WHEN cct.CategoryCodeTypeName LIKE 'Qualified%' THEN 'Qualified' ELSE 'Non Qualified' END) , ISNULL(NULLIF(r.RegionCode, ''), 'ZZ') ,
		ISNULL(r.RegionID, 0) , r.ZipCodeRangeSortOrder,ps.CountryID--,'CountryRecap'
		--order by r.ZipCodeRangeSortOrder
		union --Territories

		SELECT 'TERRITORIES' as 'Region', '' as 'State & Zip Code', 
		cct.CategoryCodeTypeName, c.DisplayName as Demo7, ISNULL(c.CodeId, 0) as 'DemoID',
		(CASE WHEN cct.CategoryCodeTypeName LIKE 'Qualified%' THEN 'Qualified' ELSE 'Non Qualified' END) as 'Category Type', 'TERRITORIES' as 'RegionCode',
		0 as 'RegionID', sum(ps.Copies) as Copies, 1000 as ZipCodeRangeSortOrder,-1 as 'CountryID','TERRITORIES' as 'CountryRecap',2 as 'RecapOrder'
		FROM IssueArchiveProductSubscription ps with(NOLOCK)
		JOIN #PubSubscriptionID s ON s.PubSubscriptionID = ps.PubSubscriptionID
		LEFT JOIN UAD_Lookup..Region r ON r.RegionID = ps.RegionID
		LEFT JOIN UAD_Lookup..RegionGroup rg ON rg.RegionGroupID = r.RegionGroupID
		JOIN UAD_Lookup..CategoryCode cc ON cc.CategoryCodeID = ps.PubCategoryID
		JOIN UAD_Lookup..CategoryCodeType cct ON cct.CategoryCodeTypeID = cc.CategoryCodeTypeID
		JOIN UAD_Lookup..Code c ON c.CodeValue = ps.demo7
		WHERE c.CodeTypeId = @DeliverID AND ps.CountryID = @USID and rg.RegionGroupName = 'Territories'
		AND ps.IssueID = @IssueID
		group by cct.CategoryCodeTypeName, c.DisplayName, ISNULL(c.CodeId, 0),
		(CASE WHEN cct.CategoryCodeTypeName LIKE 'Qualified%' THEN 'Qualified' ELSE 'Non Qualified' END) , r.ZipCodeRangeSortOrder--,ps.CountryID

		union--Canada

		SELECT 'CANADA' as 'Region', '' as 'State & Zip Code', 
		cct.CategoryCodeTypeName, c.DisplayName as Demo7, ISNULL(c.CodeId, 0) as 'DemoID',
		(CASE WHEN cct.CategoryCodeTypeName LIKE 'Qualified%' THEN 'Qualified' ELSE 'Non Qualified' END) as 'Category Type', 'CANADA' as 'RegionCode',
		0 as 'RegionID', sum(ps.Copies) as Copies,1001 as 'ZipCodeRangeSortOrder',ps.CountryID,'CANADA' as 'CountryRecap',3 as 'RecapOrder'
		FROM IssueArchiveProductSubscription ps with(NOLOCK)
		JOIN #PubSubscriptionID s ON s.PubSubscriptionID = ps.PubSubscriptionID
		JOIN UAD_Lookup..CategoryCode cc ON cc.CategoryCodeID = ps.PubCategoryID
		JOIN UAD_Lookup..CategoryCodeType cct ON cct.CategoryCodeTypeID = cc.CategoryCodeTypeID
		JOIN UAD_Lookup..Code c ON c.CodeValue = ps.demo7
		WHERE ps.CountryID=2 and
		c.CodeTypeId = @DeliverID
		AND ps.IssueID = @IssueID
		group by cct.CategoryCodeTypeName, c.DisplayName, ISNULL(c.CodeId, 0),
		(CASE WHEN cct.CategoryCodeTypeName LIKE 'Qualified%' THEN 'Qualified' ELSE 'Non Qualified' END),ps.CountryID--,'CountryRecap'

		union--mexico

		SELECT 'MEXICO' as 'Region', '' as 'State & Zip Code', 
		cct.CategoryCodeTypeName, c.DisplayName as Demo7, ISNULL(c.CodeId, 0) as 'DemoID',
		(CASE WHEN cct.CategoryCodeTypeName LIKE 'Qualified%' THEN 'Qualified' ELSE 'Non Qualified' END) as 'Category Type', 'MEXICO' as 'RegionCode',
		0 as 'RegionID', sum(ps.Copies) as Copies,1002 as 'ZipCodeRangeSortOrder',ps.CountryID,'MEXICO' as 'CountryRecap',4 as 'RecapOrder'
		FROM IssueArchiveProductSubscription ps with(NOLOCK)
		JOIN #PubSubscriptionID s ON s.PubSubscriptionID = ps.PubSubscriptionID
		JOIN UAD_Lookup..CategoryCode cc ON cc.CategoryCodeID = ps.PubCategoryID
		JOIN UAD_Lookup..CategoryCodeType cct ON cct.CategoryCodeTypeID = cc.CategoryCodeTypeID
		JOIN UAD_Lookup..Code c ON c.CodeValue = ps.demo7
		WHERE ps.CountryID=429 and
		c.CodeTypeId = @DeliverID
		AND ps.IssueID = @IssueID
		group by cct.CategoryCodeTypeName, c.DisplayName, ISNULL(c.CodeId, 0),
		(CASE WHEN cct.CategoryCodeTypeName LIKE 'Qualified%' THEN 'Qualified' ELSE 'Non Qualified' END),ps.CountryID--,'CountryRecap'

		union--foreign

		SELECT 'FOREIGN' as 'Region', '' as 'State & Zip Code', 
		cct.CategoryCodeTypeName, c.DisplayName as Demo7, ISNULL(c.CodeId, 0) as 'DemoID',
		(CASE WHEN cct.CategoryCodeTypeName LIKE 'Qualified%' THEN 'Qualified' ELSE 'Non Qualified' END) as 'Category Type', 'FOREIGN' as 'RegionCode',
		0 as 'RegionID', sum(ps.Copies) as Copies,1003 as 'ZipCodeRangeSortOrder',9999,'FOREIGN' as 'CountryRecap',5 as 'RecapOrder'
		FROM IssueArchiveProductSubscription ps with(NOLOCK)
		JOIN #PubSubscriptionID s ON s.PubSubscriptionID = ps.PubSubscriptionID
		JOIN UAD_Lookup..CategoryCode cc ON cc.CategoryCodeID = ps.PubCategoryID
		JOIN UAD_Lookup..CategoryCodeType cct ON cct.CategoryCodeTypeID = cc.CategoryCodeTypeID
		JOIN UAD_Lookup..Code c ON c.CodeValue = ps.demo7
		WHERE ps.RegionCode='FO' and 
		c.CodeTypeId = @DeliverID
		AND ps.IssueID = @IssueID and 
		ps.CountryID not in (1,2,429)
		group by cct.CategoryCodeTypeName, c.DisplayName, ISNULL(c.CodeId, 0),
		(CASE WHEN cct.CategoryCodeTypeName LIKE 'Qualified%' THEN 'Qualified' ELSE 'Non Qualified' END)

		order by 'ZipCodeRangeSortOrder'

	
	END
	
	DROP TABLE #PubSubscriptionID

END
