CREATE proc [dbo].[rpt_Geo_BreakDown_International]
(
	@Filters varchar(max) ='<XML><Filters></Filters></XML>',
	@AdHocFilters varchar(max) = '<XML></XML>',
	@IssueID int
)
as
BEGIN 

	SET NOCOUNT ON
	--Declare @FilterString varchar(max) ='<XML><Filters><ProductID>1</ProductID><TransactionCodes>2</TransactionCodes><Demo7>1899</Demo7></Filters></XML>'
	--DECLARE @IncludeAddRemove bit = 0
	
	IF 1=0 
		BEGIN
			SET FMTONLY OFF
		END
		
	DECLARE @DeliverID int = (SELECT CodeTypeID FROM UAD_Lookup..CodeType WHERE CodeTypeName = 'Deliver')
	
	SELECT DisplayName, CodeId
	INTO #Demos
	FROM UAD_Lookup..Code 
	WHERE CodeTypeId = @DeliverID	
	
	CREATE TABLE #PubSubscriptionID (PubSubscriptionID int)  
	CREATE UNIQUE CLUSTERED INDEX IX_1 on #PubSubscriptionID (PubSubscriptionID)
		
	IF @IssueID = 0
		BEGIN
			INSERT INTO #PubSubscriptionID   
			EXEC rpt_GetSubscriptionIDs_Copies_From_Filter_XML 
			@Filters, @AdHocFilters
		
			SELECT 
			co.ShortName as 'Country',
			ps.CountryID,
			co.BpaArea,
			co.BpaAreaOrder,
			'UNITED STATES' as 'CountryRecap',
			1 as 'RecapOrder',
			cct.CategoryCodeTypeName, 
			sum(ps.Copies) as 'Copies',
			c.DisplayName as 'Demo7', 
			ISNULL(c.CodeId, 0) as 'DemoID',
			(CASE WHEN cct.CategoryCodeTypeName LIKE 'Qualified%' THEN 'Qualified' ELSE 'Non Qualified' END) as 'Category Type'

			FROM PubSubscriptions ps with(NOLOCK)
			JOIN #PubSubscriptionID s ON s.PubSubscriptionID = ps.PubSubscriptionID
			LEFT JOIN UAD_Lookup..Country co ON co.CountryID = ps.CountryID
			LEFT JOIN UAD_Lookup..Region r ON r.RegionID = ps.RegionID
			LEFT JOIN UAD_Lookup..RegionGroup rg ON rg.RegionGroupID = r.RegionGroupID
			JOIN UAD_Lookup..CategoryCode cc ON cc.CategoryCodeID = ps.PubCategoryID
			JOIN UAD_Lookup..CategoryCodeType cct ON cct.CategoryCodeTypeID = cc.CategoryCodeTypeID
			JOIN UAD_Lookup..Code c ON c.CodeValue = ps.demo7
			WHERE c.CodeTypeId = @DeliverID AND ps.CountryID = 1 and rg.RegionGroupName != 'Territories'
			group by co.ShortName,ps.CountryID,co.BpaArea,co.BpaAreaOrder,cct.CategoryCodeTypeName,c.DisplayName,c.CodeId
			
			union --Territories

			SELECT 
			co.ShortName as 'Country',
			ps.CountryID,
			co.BpaArea,
			co.BpaAreaOrder,
			'TERRITORIES' as 'CountryRecap',
			2 as 'RecapOrder',
			cct.CategoryCodeTypeName, 
			sum(ps.Copies) as 'Copies',
			c.DisplayName as 'Demo7', 
			ISNULL(c.CodeId, 0) as 'DemoID',
			(CASE WHEN cct.CategoryCodeTypeName LIKE 'Qualified%' THEN 'Qualified' ELSE 'Non Qualified' END) as 'Category Type'

			FROM PubSubscriptions ps with(NOLOCK)
			JOIN #PubSubscriptionID s ON s.PubSubscriptionID = ps.PubSubscriptionID
			LEFT JOIN UAD_Lookup..Country co ON co.CountryID = ps.CountryID
			LEFT JOIN UAD_Lookup..Region r ON r.RegionID = ps.RegionID
			LEFT JOIN UAD_Lookup..RegionGroup rg ON rg.RegionGroupID = r.RegionGroupID
			JOIN UAD_Lookup..CategoryCode cc ON cc.CategoryCodeID = ps.PubCategoryID
			JOIN UAD_Lookup..CategoryCodeType cct ON cct.CategoryCodeTypeID = cc.CategoryCodeTypeID
			JOIN UAD_Lookup..Code c ON c.CodeValue = ps.demo7
			WHERE c.CodeTypeId = @DeliverID AND ps.CountryID = 1 and rg.RegionGroupName = 'Territories'
			group by co.ShortName,ps.CountryID,co.BpaArea,co.BpaAreaOrder,cct.CategoryCodeTypeName,c.DisplayName,c.CodeId

			union--Canada

			SELECT 
			co.ShortName as 'Country',
			ps.CountryID,
			co.BpaArea,
			co.BpaAreaOrder,
			'CANADA' as 'CountryRecap',
			3 as 'RecapOrder',
			cct.CategoryCodeTypeName, 
			sum(ps.Copies) as 'Copies',
			c.DisplayName as 'Demo7', 
			ISNULL(c.CodeId, 0) as 'DemoID',
			(CASE WHEN cct.CategoryCodeTypeName LIKE 'Qualified%' THEN 'Qualified' ELSE 'Non Qualified' END) as 'Category Type'

			FROM PubSubscriptions ps with(NOLOCK)
			JOIN #PubSubscriptionID s ON s.PubSubscriptionID = ps.PubSubscriptionID
			LEFT JOIN UAD_Lookup..Country co ON co.CountryID = ps.CountryID
			--LEFT JOIN UAD_Lookup..Region r ON r.RegionID = ps.RegionID
			--LEFT JOIN UAD_Lookup..RegionGroup rg ON rg.RegionGroupID = r.RegionGroupID
			JOIN UAD_Lookup..CategoryCode cc ON cc.CategoryCodeID = ps.PubCategoryID
			JOIN UAD_Lookup..CategoryCodeType cct ON cct.CategoryCodeTypeID = cc.CategoryCodeTypeID
			JOIN UAD_Lookup..Code c ON c.CodeValue = ps.demo7
			WHERE c.CodeTypeId = @DeliverID AND ps.CountryID = 2 
			group by co.ShortName,ps.CountryID,co.BpaArea,co.BpaAreaOrder,cct.CategoryCodeTypeName,c.DisplayName,c.CodeId

			union--mexico

			SELECT 
			co.ShortName as 'Country',
			ps.CountryID,
			co.BpaArea,
			co.BpaAreaOrder,
			'MEXICO' as 'CountryRecap',
			4 as 'RecapOrder',
			cct.CategoryCodeTypeName, 
			sum(ps.Copies) as 'Copies',
			c.DisplayName as 'Demo7', 
			ISNULL(c.CodeId, 0) as 'DemoID',
			(CASE WHEN cct.CategoryCodeTypeName LIKE 'Qualified%' THEN 'Qualified' ELSE 'Non Qualified' END) as 'Category Type'

			FROM PubSubscriptions ps with(NOLOCK)
			JOIN #PubSubscriptionID s ON s.PubSubscriptionID = ps.PubSubscriptionID
			LEFT JOIN UAD_Lookup..Country co ON co.CountryID = ps.CountryID
			--LEFT JOIN UAD_Lookup..Region r ON r.RegionID = ps.RegionID
			--LEFT JOIN UAD_Lookup..RegionGroup rg ON rg.RegionGroupID = r.RegionGroupID
			JOIN UAD_Lookup..CategoryCode cc ON cc.CategoryCodeID = ps.PubCategoryID
			JOIN UAD_Lookup..CategoryCodeType cct ON cct.CategoryCodeTypeID = cc.CategoryCodeTypeID
			JOIN UAD_Lookup..Code c ON c.CodeValue = ps.demo7
			WHERE c.CodeTypeId = @DeliverID AND ps.CountryID = 429
			group by co.ShortName,ps.CountryID,co.BpaArea,co.BpaAreaOrder,cct.CategoryCodeTypeName,c.DisplayName,c.CodeId

			union--foreign

			SELECT 
			co.ShortName as 'Country',
			ps.CountryID,
			co.BpaArea,
			co.BpaAreaOrder,
			'FOREIGN' as 'CountryRecap',
			5 as 'RecapOrder',
			cct.CategoryCodeTypeName, 
			sum(ps.Copies) as 'Copies',
			c.DisplayName as 'Demo7', 
			ISNULL(c.CodeId, 0) as 'DemoID',
			(CASE WHEN cct.CategoryCodeTypeName LIKE 'Qualified%' THEN 'Qualified' ELSE 'Non Qualified' END) as 'Category Type'

			FROM PubSubscriptions ps with(NOLOCK)
			JOIN #PubSubscriptionID s ON s.PubSubscriptionID = ps.PubSubscriptionID
			LEFT JOIN UAD_Lookup..Country co ON co.CountryID = ps.CountryID
			--LEFT JOIN UAD_Lookup..Region r ON r.RegionID = ps.RegionID
			--LEFT JOIN UAD_Lookup..RegionGroup rg ON rg.RegionGroupID = r.RegionGroupID
			JOIN UAD_Lookup..CategoryCode cc ON cc.CategoryCodeID = ps.PubCategoryID
			JOIN UAD_Lookup..CategoryCodeType cct ON cct.CategoryCodeTypeID = cc.CategoryCodeTypeID
			JOIN UAD_Lookup..Code c ON c.CodeValue = ps.demo7
			WHERE c.CodeTypeId = @DeliverID AND ps.RegionCode='FO' and ps.CountryID not in (1,2,429)
			group by co.ShortName,ps.CountryID,co.BpaArea,co.BpaAreaOrder,cct.CategoryCodeTypeName,c.DisplayName,c.CodeId

			--UNION ALL
			--	SELECT '', 0, '','',0, '', 0, DisplayName, CodeId, '' 
			--	FROM #Demos

			order by co.BpaAreaOrder

		END
	ELSE
		BEGIN
			INSERT INTO #PubSubscriptionID   
			EXEC rpt_GetSubscriptionIDs_Copies_From_Filter_XML 
			@Filters, @AdHocFilters, 0, 1, @IssueID
		
			
			SELECT 
			co.ShortName as 'Country',
			ps.CountryID,
			co.BpaArea,
			co.BpaAreaOrder,
			'UNITED STATES' as 'CountryRecap',
			1 as 'RecapOrder',
			cct.CategoryCodeTypeName, 
			sum(ps.Copies) as 'Copies',
			c.DisplayName as 'Demo7', 
			ISNULL(c.CodeId, 0) as 'DemoID',
			(CASE WHEN cct.CategoryCodeTypeName LIKE 'Qualified%' THEN 'Qualified' ELSE 'Non Qualified' END) as 'Category Type'

			FROM IssueArchiveProductSubscription ps with(NOLOCK)
			JOIN #PubSubscriptionID s ON s.PubSubscriptionID = ps.PubSubscriptionID
			LEFT JOIN UAD_Lookup..Country co ON co.CountryID = ps.CountryID
			LEFT JOIN UAD_Lookup..Region r ON r.RegionID = ps.RegionID
			LEFT JOIN UAD_Lookup..RegionGroup rg ON rg.RegionGroupID = r.RegionGroupID
			JOIN UAD_Lookup..CategoryCode cc ON cc.CategoryCodeID = ps.PubCategoryID
			JOIN UAD_Lookup..CategoryCodeType cct ON cct.CategoryCodeTypeID = cc.CategoryCodeTypeID
			JOIN UAD_Lookup..Code c ON c.CodeValue = ps.demo7
			WHERE c.CodeTypeId = @DeliverID AND ps.CountryID = 1 and rg.RegionGroupName != 'Territories' AND ps.IssueID = @IssueID
			group by co.ShortName,ps.CountryID,co.BpaArea,co.BpaAreaOrder,cct.CategoryCodeTypeName,c.DisplayName,c.CodeId
			
			union --Territories

			SELECT 
			co.ShortName as 'Country',
			ps.CountryID,
			co.BpaArea,
			co.BpaAreaOrder,
			'TERRITORIES' as 'CountryRecap',
			2 as 'RecapOrder',
			cct.CategoryCodeTypeName, 
			sum(ps.Copies) as 'Copies',
			c.DisplayName as 'Demo7', 
			ISNULL(c.CodeId, 0) as 'DemoID',
			(CASE WHEN cct.CategoryCodeTypeName LIKE 'Qualified%' THEN 'Qualified' ELSE 'Non Qualified' END) as 'Category Type'

			FROM IssueArchiveProductSubscription ps with(NOLOCK)
			JOIN #PubSubscriptionID s ON s.PubSubscriptionID = ps.PubSubscriptionID
			LEFT JOIN UAD_Lookup..Country co ON co.CountryID = ps.CountryID
			LEFT JOIN UAD_Lookup..Region r ON r.RegionID = ps.RegionID
			LEFT JOIN UAD_Lookup..RegionGroup rg ON rg.RegionGroupID = r.RegionGroupID
			JOIN UAD_Lookup..CategoryCode cc ON cc.CategoryCodeID = ps.PubCategoryID
			JOIN UAD_Lookup..CategoryCodeType cct ON cct.CategoryCodeTypeID = cc.CategoryCodeTypeID
			JOIN UAD_Lookup..Code c ON c.CodeValue = ps.demo7
			WHERE c.CodeTypeId = @DeliverID AND ps.CountryID = 1 and rg.RegionGroupName = 'Territories' AND ps.IssueID = @IssueID
			group by co.ShortName,ps.CountryID,co.BpaArea,co.BpaAreaOrder,cct.CategoryCodeTypeName,c.DisplayName,c.CodeId

			union--Canada

			SELECT 
			co.ShortName as 'Country',
			ps.CountryID,
			co.BpaArea,
			co.BpaAreaOrder,
			'CANADA' as 'CountryRecap',
			3 as 'RecapOrder',
			cct.CategoryCodeTypeName, 
			sum(ps.Copies) as 'Copies',
			c.DisplayName as 'Demo7', 
			ISNULL(c.CodeId, 0) as 'DemoID',
			(CASE WHEN cct.CategoryCodeTypeName LIKE 'Qualified%' THEN 'Qualified' ELSE 'Non Qualified' END) as 'Category Type'

			FROM IssueArchiveProductSubscription ps with(NOLOCK)
			JOIN #PubSubscriptionID s ON s.PubSubscriptionID = ps.PubSubscriptionID
			LEFT JOIN UAD_Lookup..Country co ON co.CountryID = ps.CountryID
			--LEFT JOIN UAD_Lookup..Region r ON r.RegionID = ps.RegionID
			--LEFT JOIN UAD_Lookup..RegionGroup rg ON rg.RegionGroupID = r.RegionGroupID
			JOIN UAD_Lookup..CategoryCode cc ON cc.CategoryCodeID = ps.PubCategoryID
			JOIN UAD_Lookup..CategoryCodeType cct ON cct.CategoryCodeTypeID = cc.CategoryCodeTypeID
			JOIN UAD_Lookup..Code c ON c.CodeValue = ps.demo7
			WHERE c.CodeTypeId = @DeliverID AND ps.CountryID = 2 AND ps.IssueID = @IssueID
			group by co.ShortName,ps.CountryID,co.BpaArea,co.BpaAreaOrder,cct.CategoryCodeTypeName,c.DisplayName,c.CodeId

			union--mexico

			SELECT 
			co.ShortName as 'Country',
			ps.CountryID,
			co.BpaArea,
			co.BpaAreaOrder,
			'MEXICO' as 'CountryRecap',
			4 as 'RecapOrder',
			cct.CategoryCodeTypeName, 
			sum(ps.Copies) as 'Copies',
			c.DisplayName as 'Demo7', 
			ISNULL(c.CodeId, 0) as 'DemoID',
			(CASE WHEN cct.CategoryCodeTypeName LIKE 'Qualified%' THEN 'Qualified' ELSE 'Non Qualified' END) as 'Category Type'

			FROM IssueArchiveProductSubscription ps with(NOLOCK)
			JOIN #PubSubscriptionID s ON s.PubSubscriptionID = ps.PubSubscriptionID
			LEFT JOIN UAD_Lookup..Country co ON co.CountryID = ps.CountryID
			--LEFT JOIN UAD_Lookup..Region r ON r.RegionID = ps.RegionID
			--LEFT JOIN UAD_Lookup..RegionGroup rg ON rg.RegionGroupID = r.RegionGroupID
			JOIN UAD_Lookup..CategoryCode cc ON cc.CategoryCodeID = ps.PubCategoryID
			JOIN UAD_Lookup..CategoryCodeType cct ON cct.CategoryCodeTypeID = cc.CategoryCodeTypeID
			JOIN UAD_Lookup..Code c ON c.CodeValue = ps.demo7
			WHERE c.CodeTypeId = @DeliverID AND ps.CountryID = 429 AND ps.IssueID = @IssueID
			group by co.ShortName,ps.CountryID,co.BpaArea,co.BpaAreaOrder,cct.CategoryCodeTypeName,c.DisplayName,c.CodeId

			union--foreign

			SELECT 
			co.ShortName as 'Country',
			ps.CountryID,
			co.BpaArea,
			co.BpaAreaOrder,
			'FOREIGN' as 'CountryRecap',
			5 as 'RecapOrder',
			cct.CategoryCodeTypeName, 
			sum(ps.Copies) as 'Copies',
			c.DisplayName as 'Demo7', 
			ISNULL(c.CodeId, 0) as 'DemoID',
			(CASE WHEN cct.CategoryCodeTypeName LIKE 'Qualified%' THEN 'Qualified' ELSE 'Non Qualified' END) as 'Category Type'

			FROM IssueArchiveProductSubscription ps with(NOLOCK)
			JOIN #PubSubscriptionID s ON s.PubSubscriptionID = ps.PubSubscriptionID
			LEFT JOIN UAD_Lookup..Country co ON co.CountryID = ps.CountryID
			--LEFT JOIN UAD_Lookup..Region r ON r.RegionID = ps.RegionID
			--LEFT JOIN UAD_Lookup..RegionGroup rg ON rg.RegionGroupID = r.RegionGroupID
			JOIN UAD_Lookup..CategoryCode cc ON cc.CategoryCodeID = ps.PubCategoryID
			JOIN UAD_Lookup..CategoryCodeType cct ON cct.CategoryCodeTypeID = cc.CategoryCodeTypeID
			JOIN UAD_Lookup..Code c ON c.CodeValue = ps.demo7
			WHERE c.CodeTypeId = @DeliverID AND ps.RegionCode='FO' AND ps.IssueID = @IssueID and ps.CountryID not in (1,2,429)
			group by co.ShortName,ps.CountryID,co.BpaArea,co.BpaAreaOrder,cct.CategoryCodeTypeName,c.DisplayName,c.CodeId

			--UNION ALL
			--	SELECT '', 0, '','',0, '', 0, DisplayName, CodeId, '' 
			--	FROM #Demos

			order by co.BpaAreaOrder

			--SELECT ISNULL(co.ShortName, 'No Response') as 'Country', co.Area, cct.CategoryCodeTypeName, ps.Copies, c.DisplayName as Demo7, c.CodeId as 'DemoID',
			--(CASE WHEN cct.CategoryCodeTypeName LIKE 'Qualified%' THEN 'Qualified' ELSE 'Non Qualified' END) as 'Category Type', ISNULL(co.CountryID, 0) as CountryID
			--FROM IssueArchiveProductSubscription ps with(NOLOCK)
			--	JOIN #PubSubscriptionID s ON s.PubSubscriptionID = ps.PubSubscriptionID
			--	LEFT JOIN UAD_Lookup..Country co ON co.CountryID = ps.CountryID
			--	JOIN UAD_Lookup..CategoryCode cc ON cc.CategoryCodeID = ps.PubCategoryID
			--	JOIN UAD_Lookup..CategoryCodeType cct ON cct.CategoryCodeTypeID = cc.CategoryCodeTypeID
			--	JOIN UAD_Lookup..Code c ON c.CodeValue = ps.demo7
			--WHERE c.CodeTypeId = @DeliverID AND ps.IssueID = @IssueID
			--UNION ALL
			--	SELECT '', '', '', 0, DisplayName, CodeId, '', 0 
			--	FROM #Demos
		END	
	
	DROP TABLE #PubSubscriptionID
	DROP TABLE #Demos

END