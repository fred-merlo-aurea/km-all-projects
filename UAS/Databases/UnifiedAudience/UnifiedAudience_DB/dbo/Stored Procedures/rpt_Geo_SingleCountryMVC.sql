CREATE proc [dbo].[rpt_Geo_SingleCountryMVC]
(
	@Queries VARCHAR(MAX),
	@CountryID int,
	@IssueID int = 0
)
as
BEGIN
	
	SET NOCOUNT ON

	--Declare @Filters varchar(max) ='<XML><Filters><ProductID>1</ProductID><CategoryCode>101,102,103,130,115,116,117,118,119,120,121,122</CategoryCode><CategoryCodeType>1,3</CategoryCodeType><TransactionCode>101,140,141,111,112,113,114,115,116,102,117,103,137,145,146,138,139,104,136,118,119,120,121,122,123,124,125,142,143</TransactionCode><TransactionCodeType>1,3</TransactionCodeType></Filters></XML>'
	--DECLARE @AdHocFilters varchar(max) = '<XML></XML>'
	--DECLARE @CountryID int = 402
	--DECLARE @IssueID int = 0
	
	IF 1=0 
		BEGIN
			SET FMTONLY OFF
		END
		
	DECLARE @DeliverID int = (SELECT CodeTypeID FROM UAD_Lookup..CodeType WHERE CodeTypeName = 'Deliver')
	
	CREATE TABLE #PubSubscriptionID (PubSubscriptionID int)  
	CREATE UNIQUE CLUSTERED INDEX IX_1 on #PubSubscriptionID (PubSubscriptionID)
	INSERT INTO #PubSubscriptionID
	EXEC (@Queries) 
	
	declare @recapName varchar(100) = 'FOREIGN'
	declare @cname varchar(500)
	set @cname = (select isnull(ShortName,'-') from UAD_LOOKUP..Country where CountryId = @CountryID)

	if @CountryID = 2
	begin 
		set @recapName = 'CANADA'
	end
	
	if @CountryID = 429
	begin 
		set @recapName = 'MEXICO'
	end

	IF @IssueID = 0 --Query Current Issue
		BEGIN
			
			IF @CountryID > 0
				BEGIN
					
						SELECT 
							ISNULL(r.RegionName, 'No Region') as 'RegionName', 
							cct.CategoryCodeTypeName, 
							sum(ps.Copies) as 'Copies',
							c.DisplayName as 'Demo7', 
							ISNULL(c.CodeId, 0) as 'DemoID',
							(CASE WHEN cct.CategoryCodeTypeName LIKE 'Qualified%' THEN cct.CategoryCodeTypeName ELSE 'Non Qualified' END) as 'Category Type', 
							ISNULL(NULLIF(r.RegionCode, ''), 'ZZ') as 'RegionCode',
							ISNULL(r.RegionID, 0) as 'RegionID', 
							ps.CountryID,
							@recapName as 'CountryRecap',
							case when @CountryID = 2 then 3 when @CountryID = 429 then 4 else 0 end as 'RecapOrder'
					into #tmpSingleCountry
						FROM PubSubscriptions ps with(NOLOCK)
						JOIN #PubSubscriptionID s ON s.PubSubscriptionID = ps.PubSubscriptionID
						LEFT JOIN UAD_Lookup..Region r ON r.RegionID = ps.RegionID
						LEFT JOIN UAD_Lookup..RegionGroup rg ON rg.RegionGroupID = r.RegionGroupID
						JOIN UAD_Lookup..CategoryCode cc ON cc.CategoryCodeID = ps.PubCategoryID
						JOIN UAD_Lookup..CategoryCodeType cct ON cct.CategoryCodeTypeID = cc.CategoryCodeTypeID
						JOIN UAD_Lookup..Code c ON c.CodeValue = ps.demo7
						WHERE c.CodeTypeId = @DeliverID and ps.CountryID = @CountryID
						group by ISNULL(r.RegionName, 'No Region'), 
							cct.CategoryCodeTypeName, 
							c.DisplayName, 
							ISNULL(c.CodeId, 0),
							(CASE WHEN cct.CategoryCodeTypeName LIKE 'Qualified%' THEN cct.CategoryCodeTypeName ELSE 'Non Qualified' END), 
							ISNULL(NULLIF(r.RegionCode, ''), 'ZZ'),
							ISNULL(r.RegionID, 0), 
							ps.CountryID

					if(select count(*) from #tmpSingleCountry) = 0
						insert into #tmpSingleCountry
						values(@cname,'Qualified Free',0,'Print',0,'Qualified Free','',0,0,@recapName,0)

					select * from #tmpSingleCountry
					union --USA

					SELECT 
						'' as 'RegionName', 
						cct.CategoryCodeTypeName, 
						sum(ps.Copies) as 'Copies',
						c.DisplayName as 'Demo7', 
						ISNULL(c.CodeId, 0) as 'DemoID',
						(CASE WHEN cct.CategoryCodeTypeName LIKE 'Qualified%' THEN cct.CategoryCodeTypeName ELSE 'Non Qualified' END) as 'Category Type', 
						'' as 'RegionCode',
						0 as 'RegionID', 
						1 as 'CountryID',
						'UNITED STATES' as 'CountryRecap',
						1 as 'RecapOrder'
					FROM PubSubscriptions ps with(NOLOCK)
					JOIN #PubSubscriptionID s ON s.PubSubscriptionID = ps.PubSubscriptionID
					LEFT JOIN UAD_Lookup..Region r ON r.RegionID = ps.RegionID
					LEFT JOIN UAD_Lookup..RegionGroup rg ON rg.RegionGroupID = r.RegionGroupID
					JOIN UAD_Lookup..CategoryCode cc ON cc.CategoryCodeID = ps.PubCategoryID
					JOIN UAD_Lookup..CategoryCodeType cct ON cct.CategoryCodeTypeID = cc.CategoryCodeTypeID
					JOIN UAD_Lookup..Code c ON c.CodeValue = ps.demo7
					WHERE c.CodeTypeId = @DeliverID and ps.CountryID = 1 and rg.RegionGroupName != 'Territories'
					group by  
						cct.CategoryCodeTypeName, 
						c.DisplayName, 
						ISNULL(c.CodeId, 0),
						(CASE WHEN cct.CategoryCodeTypeName LIKE 'Qualified%' THEN cct.CategoryCodeTypeName ELSE 'Non Qualified' END)

					union --Territories

					SELECT 
						'' as 'RegionName', 
						cct.CategoryCodeTypeName, 
						sum(ps.Copies) as 'Copies',
						c.DisplayName as 'Demo7', 
						ISNULL(c.CodeId, 0) as 'DemoID',
						(CASE WHEN cct.CategoryCodeTypeName LIKE 'Qualified%' THEN cct.CategoryCodeTypeName ELSE 'Non Qualified' END) as 'Category Type', 
						'' as 'RegionCode',
						0 as 'RegionID', 
						1 as 'CountryID',
						'TERRITORIES' as 'CountryRecap',
						2 as 'RecapOrder'
					FROM PubSubscriptions ps with(NOLOCK)
					JOIN #PubSubscriptionID s ON s.PubSubscriptionID = ps.PubSubscriptionID
					LEFT JOIN UAD_Lookup..Region r ON r.RegionID = ps.RegionID
					LEFT JOIN UAD_Lookup..RegionGroup rg ON rg.RegionGroupID = r.RegionGroupID
					JOIN UAD_Lookup..CategoryCode cc ON cc.CategoryCodeID = ps.PubCategoryID
					JOIN UAD_Lookup..CategoryCodeType cct ON cct.CategoryCodeTypeID = cc.CategoryCodeTypeID
					JOIN UAD_Lookup..Code c ON c.CodeValue = ps.demo7
					WHERE c.CodeTypeId = @DeliverID and ps.CountryID = 1 and rg.RegionGroupName = 'Territories'
					group by
						cct.CategoryCodeTypeName, 
						c.DisplayName, 
						ISNULL(c.CodeId, 0),
						(CASE WHEN cct.CategoryCodeTypeName LIKE 'Qualified%' THEN cct.CategoryCodeTypeName ELSE 'Non Qualified' END) 
						
					union --Canada

					SELECT 
						'' as 'RegionName', 
						cct.CategoryCodeTypeName, 
						sum(ps.Copies) as 'Copies',
						c.DisplayName as 'Demo7', 
						ISNULL(c.CodeId, 0) as 'DemoID',
						(CASE WHEN cct.CategoryCodeTypeName LIKE 'Qualified%' THEN cct.CategoryCodeTypeName ELSE 'Non Qualified' END) as 'Category Type', 
						'' as 'RegionCode',
						0 as 'RegionID', 
						2 as 'CountryID',
						'CANADA' as 'CountryRecap',
						3 as 'RecapOrder'
					FROM PubSubscriptions ps with(NOLOCK)
					JOIN #PubSubscriptionID s ON s.PubSubscriptionID = ps.PubSubscriptionID
					LEFT JOIN UAD_Lookup..Region r ON r.RegionID = ps.RegionID
					LEFT JOIN UAD_Lookup..RegionGroup rg ON rg.RegionGroupID = r.RegionGroupID
					JOIN UAD_Lookup..CategoryCode cc ON cc.CategoryCodeID = ps.PubCategoryID
					JOIN UAD_Lookup..CategoryCodeType cct ON cct.CategoryCodeTypeID = cc.CategoryCodeTypeID
					JOIN UAD_Lookup..Code c ON c.CodeValue = ps.demo7
					WHERE c.CodeTypeId = @DeliverID and ps.CountryID = 2 and ps.CountryID != @CountryID
					group by 
						cct.CategoryCodeTypeName, 
						c.DisplayName, 
						ISNULL(c.CodeId, 0),
						(CASE WHEN cct.CategoryCodeTypeName LIKE 'Qualified%' THEN cct.CategoryCodeTypeName ELSE 'Non Qualified' END) 
						
					union --Mexico

					SELECT 
						'' as 'RegionName', 
						cct.CategoryCodeTypeName, 
						sum(ps.Copies) as 'Copies',
						c.DisplayName as 'Demo7', 
						ISNULL(c.CodeId, 0) as 'DemoID',
						(CASE WHEN cct.CategoryCodeTypeName LIKE 'Qualified%' THEN cct.CategoryCodeTypeName ELSE 'Non Qualified' END) as 'Category Type', 
						'' as 'RegionCode',
						0 as 'RegionID', 
						429 as 'CountryID',
						'MEXICO' as 'CountryRecap',
						4 as 'RecapOrder'
					FROM PubSubscriptions ps with(NOLOCK)
					JOIN #PubSubscriptionID s ON s.PubSubscriptionID = ps.PubSubscriptionID
					LEFT JOIN UAD_Lookup..Region r ON r.RegionID = ps.RegionID
					LEFT JOIN UAD_Lookup..RegionGroup rg ON rg.RegionGroupID = r.RegionGroupID
					JOIN UAD_Lookup..CategoryCode cc ON cc.CategoryCodeID = ps.PubCategoryID
					JOIN UAD_Lookup..CategoryCodeType cct ON cct.CategoryCodeTypeID = cc.CategoryCodeTypeID
					JOIN UAD_Lookup..Code c ON c.CodeValue = ps.demo7
					WHERE c.CodeTypeId = @DeliverID and ps.CountryID = 429 and ps.CountryID != @CountryID
					group by 
						cct.CategoryCodeTypeName, 
						c.DisplayName, 
						ISNULL(c.CodeId, 0),
						(CASE WHEN cct.CategoryCodeTypeName LIKE 'Qualified%' THEN cct.CategoryCodeTypeName ELSE 'Non Qualified' END)

					union --Foreign

					SELECT 
						'' as 'RegionName', 
						cct.CategoryCodeTypeName, 
						sum(ps.Copies) as 'Copies',
						c.DisplayName as 'Demo7', 
						ISNULL(c.CodeId, 0) as 'DemoID',
						(CASE WHEN cct.CategoryCodeTypeName LIKE 'Qualified%' THEN cct.CategoryCodeTypeName ELSE 'Non Qualified' END) as 'Category Type', 
						'' as 'RegionCode',
						0 as 'RegionID', 
						9999 as 'CountryID',
						'FOREIGN' as 'CountryRecap',
						5 as 'RecapOrder'
					FROM PubSubscriptions ps with(NOLOCK)
					JOIN #PubSubscriptionID s ON s.PubSubscriptionID = ps.PubSubscriptionID
					LEFT JOIN UAD_Lookup..Region r ON r.RegionID = ps.RegionID
					LEFT JOIN UAD_Lookup..RegionGroup rg ON rg.RegionGroupID = r.RegionGroupID
					JOIN UAD_Lookup..CategoryCode cc ON cc.CategoryCodeID = ps.PubCategoryID
					JOIN UAD_Lookup..CategoryCodeType cct ON cct.CategoryCodeTypeID = cc.CategoryCodeTypeID
					JOIN UAD_Lookup..Code c ON c.CodeValue = ps.demo7
					WHERE c.CodeTypeId = @DeliverID and ps.RegionCode='FO' and ps.CountryID != @CountryID and ps.CountryID not in (1,2,429)
					group by 
						cct.CategoryCodeTypeName, 
						c.DisplayName, 
						ISNULL(c.CodeId, 0),
						(CASE WHEN cct.CategoryCodeTypeName LIKE 'Qualified%' THEN cct.CategoryCodeTypeName ELSE 'Non Qualified' END)
					
					order by RecapOrder, RegionName

					DROP TABLE #tmpSingleCountry
				END
			ELSE
				BEGIN
					SELECT 
						ISNULL(r.RegionName, 'No Region') as 'RegionName', 
						cct.CategoryCodeTypeName, 
						sum(ps.Copies) as 'Copies',
						c.DisplayName as 'Demo7', 
						ISNULL(c.CodeId, 0) as 'DemoID',
						(CASE WHEN cct.CategoryCodeTypeName LIKE 'Qualified%' THEN cct.CategoryCodeTypeName ELSE 'Non Qualified' END) as 'Category Type', 
						ISNULL(NULLIF(r.RegionCode, ''), 'ZZ') as 'RegionCode',
						ISNULL(r.RegionID, 0) as 'RegionID', 
						ps.CountryID,
						@recapName as 'CountryRecap',
						case when @CountryID = 2 then 3 when @CountryID = 429 then 4 else 0 end as 'RecapOrder'
					into #tmpSingleCountry2
					FROM PubSubscriptions ps with(NOLOCK)
					JOIN #PubSubscriptionID s ON s.PubSubscriptionID = ps.PubSubscriptionID
					LEFT JOIN UAD_Lookup..Region r ON r.RegionID = ps.RegionID
					LEFT JOIN UAD_Lookup..RegionGroup rg ON rg.RegionGroupID = r.RegionGroupID
					JOIN UAD_Lookup..CategoryCode cc ON cc.CategoryCodeID = ps.PubCategoryID
					JOIN UAD_Lookup..CategoryCodeType cct ON cct.CategoryCodeTypeID = cc.CategoryCodeTypeID
					JOIN UAD_Lookup..Code c ON c.CodeValue = ps.demo7
					WHERE c.CodeTypeId = @DeliverID and ISNULL(NULLIF(ps.CountryID, 0), '') = ''
					group by ISNULL(r.RegionName, 'No Region'), 
						cct.CategoryCodeTypeName, 
						c.DisplayName, 
						ISNULL(c.CodeId, 0),
						(CASE WHEN cct.CategoryCodeTypeName LIKE 'Qualified%' THEN cct.CategoryCodeTypeName ELSE 'Non Qualified' END), 
						ISNULL(NULLIF(r.RegionCode, ''), 'ZZ'),
						ISNULL(r.RegionID, 0), 
						ps.CountryID
					
					if(select count(*) from #tmpSingleCountry2) = 0
						insert into #tmpSingleCountry2
						values(@cname,'Qualified Free',0,'Print',0,'Qualified Free','',0,0,@recapName,0)
						
						
						
					select * from #tmpSingleCountry2	
					union --USA

					SELECT 
						'' as 'RegionName', 
						cct.CategoryCodeTypeName, 
						sum(ps.Copies) as 'Copies',
						c.DisplayName as 'Demo7', 
						ISNULL(c.CodeId, 0) as 'DemoID',
						(CASE WHEN cct.CategoryCodeTypeName LIKE 'Qualified%' THEN cct.CategoryCodeTypeName ELSE 'Non Qualified' END) as 'Category Type', 
						'' as 'RegionCode',
						0 as 'RegionID', 
						1 as 'CountryID',
						'UNITED STATES' as 'CountryRecap',
						1 as 'RecapOrder'
					FROM PubSubscriptions ps with(NOLOCK)
					JOIN #PubSubscriptionID s ON s.PubSubscriptionID = ps.PubSubscriptionID
					LEFT JOIN UAD_Lookup..Region r ON r.RegionID = ps.RegionID
					LEFT JOIN UAD_Lookup..RegionGroup rg ON rg.RegionGroupID = r.RegionGroupID
					JOIN UAD_Lookup..CategoryCode cc ON cc.CategoryCodeID = ps.PubCategoryID
					JOIN UAD_Lookup..CategoryCodeType cct ON cct.CategoryCodeTypeID = cc.CategoryCodeTypeID
					JOIN UAD_Lookup..Code c ON c.CodeValue = ps.demo7
					WHERE c.CodeTypeId = @DeliverID and ps.CountryID = 1 and rg.RegionGroupName != 'Territories'
					group by  
						cct.CategoryCodeTypeName, 
						c.DisplayName, 
						ISNULL(c.CodeId, 0),
						(CASE WHEN cct.CategoryCodeTypeName LIKE 'Qualified%' THEN cct.CategoryCodeTypeName ELSE 'Non Qualified' END)

					union --Territories

					SELECT 
						'' as 'RegionName', 
						cct.CategoryCodeTypeName, 
						sum(ps.Copies) as 'Copies',
						c.DisplayName as 'Demo7', 
						ISNULL(c.CodeId, 0) as 'DemoID',
						(CASE WHEN cct.CategoryCodeTypeName LIKE 'Qualified%' THEN cct.CategoryCodeTypeName ELSE 'Non Qualified' END) as 'Category Type', 
						'' as 'RegionCode',
						0 as 'RegionID', 
						2 as 'CountryID',
						'TERRITORIES' as 'CountryRecap',
						2 as 'RecapOrder'
					FROM PubSubscriptions ps with(NOLOCK)
					JOIN #PubSubscriptionID s ON s.PubSubscriptionID = ps.PubSubscriptionID
					LEFT JOIN UAD_Lookup..Region r ON r.RegionID = ps.RegionID
					LEFT JOIN UAD_Lookup..RegionGroup rg ON rg.RegionGroupID = r.RegionGroupID
					JOIN UAD_Lookup..CategoryCode cc ON cc.CategoryCodeID = ps.PubCategoryID
					JOIN UAD_Lookup..CategoryCodeType cct ON cct.CategoryCodeTypeID = cc.CategoryCodeTypeID
					JOIN UAD_Lookup..Code c ON c.CodeValue = ps.demo7
					WHERE c.CodeTypeId = @DeliverID and ps.CountryID = 1 and rg.RegionGroupName = 'Territories'
					group by 
						cct.CategoryCodeTypeName, 
						c.DisplayName, 
						ISNULL(c.CodeId, 0),
						(CASE WHEN cct.CategoryCodeTypeName LIKE 'Qualified%' THEN cct.CategoryCodeTypeName ELSE 'Non Qualified' END)

					union --Canada

					SELECT 
						'' as 'RegionName', 
						cct.CategoryCodeTypeName, 
						sum(ps.Copies) as 'Copies',
						c.DisplayName as 'Demo7', 
						ISNULL(c.CodeId, 0) as 'DemoID',
						(CASE WHEN cct.CategoryCodeTypeName LIKE 'Qualified%' THEN cct.CategoryCodeTypeName ELSE 'Non Qualified' END) as 'Category Type', 
						'' as 'RegionCode',
						0 as 'RegionID', 
						3 as 'CountryID',
						'CANADA' as 'CountryRecap',
						3 as 'RecapOrder'
					FROM PubSubscriptions ps with(NOLOCK)
					JOIN #PubSubscriptionID s ON s.PubSubscriptionID = ps.PubSubscriptionID
					LEFT JOIN UAD_Lookup..Region r ON r.RegionID = ps.RegionID
					LEFT JOIN UAD_Lookup..RegionGroup rg ON rg.RegionGroupID = r.RegionGroupID
					JOIN UAD_Lookup..CategoryCode cc ON cc.CategoryCodeID = ps.PubCategoryID
					JOIN UAD_Lookup..CategoryCodeType cct ON cct.CategoryCodeTypeID = cc.CategoryCodeTypeID
					JOIN UAD_Lookup..Code c ON c.CodeValue = ps.demo7
					WHERE c.CodeTypeId = @DeliverID and ps.CountryID = 2 and ps.CountryID != @CountryID
					group by 
						cct.CategoryCodeTypeName, 
						c.DisplayName, 
						ISNULL(c.CodeId, 0),
						(CASE WHEN cct.CategoryCodeTypeName LIKE 'Qualified%' THEN cct.CategoryCodeTypeName ELSE 'Non Qualified' END)

					union --Mexico

					SELECT 
						'' as 'RegionName', 
						cct.CategoryCodeTypeName, 
						sum(ps.Copies) as 'Copies',
						c.DisplayName as 'Demo7', 
						ISNULL(c.CodeId, 0) as 'DemoID',
						(CASE WHEN cct.CategoryCodeTypeName LIKE 'Qualified%' THEN cct.CategoryCodeTypeName ELSE 'Non Qualified' END) as 'Category Type', 
						'' as 'RegionCode',
						0 as 'RegionID', 
						429 as 'CountryID',
						'MEXICO' as 'CountryRecap',
						4 as 'RecapOrder'
					FROM PubSubscriptions ps with(NOLOCK)
					JOIN #PubSubscriptionID s ON s.PubSubscriptionID = ps.PubSubscriptionID
					LEFT JOIN UAD_Lookup..Region r ON r.RegionID = ps.RegionID
					LEFT JOIN UAD_Lookup..RegionGroup rg ON rg.RegionGroupID = r.RegionGroupID
					JOIN UAD_Lookup..CategoryCode cc ON cc.CategoryCodeID = ps.PubCategoryID
					JOIN UAD_Lookup..CategoryCodeType cct ON cct.CategoryCodeTypeID = cc.CategoryCodeTypeID
					JOIN UAD_Lookup..Code c ON c.CodeValue = ps.demo7
					WHERE c.CodeTypeId = @DeliverID and ps.CountryID = 429 and ps.CountryID != @CountryID
					group by  
						cct.CategoryCodeTypeName, 
						c.DisplayName, 
						ISNULL(c.CodeId, 0),
						(CASE WHEN cct.CategoryCodeTypeName LIKE 'Qualified%' THEN cct.CategoryCodeTypeName ELSE 'Non Qualified' END)

					union --Foreign

					SELECT 
						'' as 'RegionName', 
						cct.CategoryCodeTypeName, 
						sum(ps.Copies) as 'Copies',
						c.DisplayName as 'Demo7', 
						ISNULL(c.CodeId, 0) as 'DemoID',
						(CASE WHEN cct.CategoryCodeTypeName LIKE 'Qualified%' THEN cct.CategoryCodeTypeName ELSE 'Non Qualified' END) as 'Category Type', 
						'' as 'RegionCode',
						0 as 'RegionID', 
						9999 as 'CountryID',
						'FOREIGN' as 'CountryRecap',
						5 as 'RecapOrder'
					FROM PubSubscriptions ps with(NOLOCK)
					JOIN #PubSubscriptionID s ON s.PubSubscriptionID = ps.PubSubscriptionID
					LEFT JOIN UAD_Lookup..Region r ON r.RegionID = ps.RegionID
					LEFT JOIN UAD_Lookup..RegionGroup rg ON rg.RegionGroupID = r.RegionGroupID
					JOIN UAD_Lookup..CategoryCode cc ON cc.CategoryCodeID = ps.PubCategoryID
					JOIN UAD_Lookup..CategoryCodeType cct ON cct.CategoryCodeTypeID = cc.CategoryCodeTypeID
					JOIN UAD_Lookup..Code c ON c.CodeValue = ps.demo7
					WHERE c.CodeTypeId = @DeliverID and ps.RegionCode='FO' and ps.CountryID != @CountryID and ps.CountryID not in (1,2,429)
					group by 
						cct.CategoryCodeTypeName, 
						c.DisplayName, 
						ISNULL(c.CodeId, 0),
						(CASE WHEN cct.CategoryCodeTypeName LIKE 'Qualified%' THEN cct.CategoryCodeTypeName ELSE 'Non Qualified' END)

					DROP TABLE #tmpSingleCountry2
				END
		END
	ELSE --Query Archive
		BEGIN
			
			IF @CountryID > 0
				BEGIN
					--SELECT ISNULL(r.RegionName, 'No Response') as 'RegionName', cct.CategoryCodeTypeName, ps.Copies, c.DisplayName as Demo7,
					--(CASE WHEN cct.CategoryCodeTypeName LIKE 'Qualified%' THEN cct.CategoryCodeTypeName ELSE 'Non Qualified' END) as 'Category Type', ISNULL(ps.RegionCode, 'No Response') as 'RegionCode', 
					--c.CodeId as 'DemoID', ISNULL(r.RegionID, 0) as 'RegionID'
					--FROM IssueArchiveProductSubscription ps with(NOLOCK)
					--	JOIN #SubscriptionID s ON s.SubscriptionID = ps.PubSubscriptionID
					--	LEFT JOIN UAD_Lookup..Region r ON r.RegionID = ps.RegionID
					--	JOIN UAD_Lookup..CategoryCode cc ON cc.CategoryCodeID = ps.PubCategoryID
					--	JOIN UAD_Lookup..CategoryCodeType cct ON cct.CategoryCodeTypeID = cc.CategoryCodeTypeID
					--	JOIN UAD_Lookup..Code c ON c.CodeValue = ps.demo7
					--WHERE c.CodeTypeId = @DeliverID AND ps.CountryID = @CountryID AND ps.IssueID = @IssueID

					SELECT 
						ISNULL(r.RegionName, 'No Response') as 'RegionName', 
						cct.CategoryCodeTypeName, 
						sum(ps.Copies) as 'Copies',
						c.DisplayName as 'Demo7', 
						ISNULL(c.CodeId, 0) as 'DemoID',
						(CASE WHEN cct.CategoryCodeTypeName LIKE 'Qualified%' THEN cct.CategoryCodeTypeName ELSE 'Non Qualified' END) as 'Category Type', 
						ISNULL(NULLIF(r.RegionCode, ''), 'ZZ') as 'RegionCode',
						ISNULL(r.RegionID, 0) as 'RegionID', 
						ps.CountryID,
						@recapName as 'CountryRecap',
						case when @CountryID = 2 then 3 when @CountryID = 429 then 4 else 0 end as 'RecapOrder'
					into #tmpSingleCountry3
					FROM IssueArchiveProductSubscription ps with(NOLOCK)
					JOIN #PubSubscriptionID s ON s.PubSubscriptionID = ps.PubSubscriptionID
					LEFT JOIN UAD_Lookup..Region r ON r.RegionID = ps.RegionID
					LEFT JOIN UAD_Lookup..RegionGroup rg ON rg.RegionGroupID = r.RegionGroupID
					JOIN UAD_Lookup..CategoryCode cc ON cc.CategoryCodeID = ps.PubCategoryID
					JOIN UAD_Lookup..CategoryCodeType cct ON cct.CategoryCodeTypeID = cc.CategoryCodeTypeID
					JOIN UAD_Lookup..Code c ON c.CodeValue = ps.demo7
					WHERE c.CodeTypeId = @DeliverID and ps.CountryID = @CountryID AND ps.IssueID = @IssueID
					group by ISNULL(r.RegionName, 'No Response'), 
						cct.CategoryCodeTypeName, 
						c.DisplayName, 
						ISNULL(c.CodeId, 0),
						(CASE WHEN cct.CategoryCodeTypeName LIKE 'Qualified%' THEN cct.CategoryCodeTypeName ELSE 'Non Qualified' END), 
						ISNULL(NULLIF(r.RegionCode, ''), 'ZZ'),
						ISNULL(r.RegionID, 0), 
						ps.CountryID


					if(select count(*) from #tmpSingleCountry3) = 0
						insert into #tmpSingleCountry3
						values(@cname,'Qualified Free',0,'Print',0,'Qualified Free','',0,0,@recapName,0)

					select * from #tmpSingleCountry3
					union --USA

					SELECT 
						'' as 'RegionName', 
						cct.CategoryCodeTypeName, 
						sum(ps.Copies) as 'Copies',
						c.DisplayName as 'Demo7', 
						ISNULL(c.CodeId, 0) as 'DemoID',
						(CASE WHEN cct.CategoryCodeTypeName LIKE 'Qualified%' THEN cct.CategoryCodeTypeName ELSE 'Non Qualified' END) as 'Category Type', 
						'' as 'RegionCode',
						0 as 'RegionID', 
						1 as 'CountryID',
						'UNITED STATES' as 'CountryRecap',
						1 as 'RecapOrder'
					FROM IssueArchiveProductSubscription ps with(NOLOCK)
					JOIN #PubSubscriptionID s ON s.PubSubscriptionID = ps.PubSubscriptionID
					LEFT JOIN UAD_Lookup..Region r ON r.RegionID = ps.RegionID
					LEFT JOIN UAD_Lookup..RegionGroup rg ON rg.RegionGroupID = r.RegionGroupID
					JOIN UAD_Lookup..CategoryCode cc ON cc.CategoryCodeID = ps.PubCategoryID
					JOIN UAD_Lookup..CategoryCodeType cct ON cct.CategoryCodeTypeID = cc.CategoryCodeTypeID
					JOIN UAD_Lookup..Code c ON c.CodeValue = ps.demo7
					WHERE c.CodeTypeId = @DeliverID and ps.CountryID = 1 and rg.RegionGroupName != 'Territories' AND ps.IssueID = @IssueID
					group by 
						cct.CategoryCodeTypeName, 
						c.DisplayName, 
						ISNULL(c.CodeId, 0),
						(CASE WHEN cct.CategoryCodeTypeName LIKE 'Qualified%' THEN cct.CategoryCodeTypeName ELSE 'Non Qualified' END)

					union --Territories

					SELECT 
						'' as 'RegionName', 
						cct.CategoryCodeTypeName, 
						sum(ps.Copies) as 'Copies',
						c.DisplayName as 'Demo7', 
						ISNULL(c.CodeId, 0) as 'DemoID',
						(CASE WHEN cct.CategoryCodeTypeName LIKE 'Qualified%' THEN cct.CategoryCodeTypeName ELSE 'Non Qualified' END) as 'Category Type', 
						'' as 'RegionCode',
						0 as 'RegionID', 
						1 as 'CountryID',
						'TERRITORIES' as 'CountryRecap',
						2 as 'RecapOrder'
					FROM IssueArchiveProductSubscription ps with(NOLOCK)
					JOIN #PubSubscriptionID s ON s.PubSubscriptionID = ps.PubSubscriptionID
					LEFT JOIN UAD_Lookup..Region r ON r.RegionID = ps.RegionID
					LEFT JOIN UAD_Lookup..RegionGroup rg ON rg.RegionGroupID = r.RegionGroupID
					JOIN UAD_Lookup..CategoryCode cc ON cc.CategoryCodeID = ps.PubCategoryID
					JOIN UAD_Lookup..CategoryCodeType cct ON cct.CategoryCodeTypeID = cc.CategoryCodeTypeID
					JOIN UAD_Lookup..Code c ON c.CodeValue = ps.demo7
					WHERE c.CodeTypeId = @DeliverID and ps.CountryID = 1 and rg.RegionGroupName = 'Territories' AND ps.IssueID = @IssueID
					group by 
						cct.CategoryCodeTypeName, 
						c.DisplayName, 
						ISNULL(c.CodeId, 0),
						(CASE WHEN cct.CategoryCodeTypeName LIKE 'Qualified%' THEN cct.CategoryCodeTypeName ELSE 'Non Qualified' END)

					union --Canada

					SELECT 
						'' as 'RegionName', 
						cct.CategoryCodeTypeName, 
						sum(ps.Copies) as 'Copies',
						c.DisplayName as 'Demo7', 
						ISNULL(c.CodeId, 0) as 'DemoID',
						(CASE WHEN cct.CategoryCodeTypeName LIKE 'Qualified%' THEN cct.CategoryCodeTypeName ELSE 'Non Qualified' END) as 'Category Type', 
						'' as 'RegionCode',
						0 as 'RegionID', 
						2 as 'CountryID',
						'CANADA' as 'CountryRecap',
						3 as 'RecapOrder'
					FROM IssueArchiveProductSubscription ps with(NOLOCK)
					JOIN #PubSubscriptionID s ON s.PubSubscriptionID = ps.PubSubscriptionID
					LEFT JOIN UAD_Lookup..Region r ON r.RegionID = ps.RegionID
					LEFT JOIN UAD_Lookup..RegionGroup rg ON rg.RegionGroupID = r.RegionGroupID
					JOIN UAD_Lookup..CategoryCode cc ON cc.CategoryCodeID = ps.PubCategoryID
					JOIN UAD_Lookup..CategoryCodeType cct ON cct.CategoryCodeTypeID = cc.CategoryCodeTypeID
					JOIN UAD_Lookup..Code c ON c.CodeValue = ps.demo7
					WHERE c.CodeTypeId = @DeliverID and ps.CountryID = 2 AND ps.IssueID = @IssueID and ps.CountryID != @CountryID
					group by
						cct.CategoryCodeTypeName, 
						c.DisplayName, 
						ISNULL(c.CodeId, 0),
						(CASE WHEN cct.CategoryCodeTypeName LIKE 'Qualified%' THEN cct.CategoryCodeTypeName ELSE 'Non Qualified' END)

					union --Mexico

					SELECT 
						'' as 'RegionName', 
						cct.CategoryCodeTypeName, 
						sum(ps.Copies) as 'Copies',
						c.DisplayName as 'Demo7', 
						ISNULL(c.CodeId, 0) as 'DemoID',
						(CASE WHEN cct.CategoryCodeTypeName LIKE 'Qualified%' THEN cct.CategoryCodeTypeName ELSE 'Non Qualified' END) as 'Category Type', 
						'' as 'RegionCode',
						0 as 'RegionID', 
						429 as 'CountryID',
						'MEXICO' as 'CountryRecap',
						4 as 'RecapOrder'
					FROM IssueArchiveProductSubscription ps with(NOLOCK)
					JOIN #PubSubscriptionID s ON s.PubSubscriptionID = ps.PubSubscriptionID
					LEFT JOIN UAD_Lookup..Region r ON r.RegionID = ps.RegionID
					LEFT JOIN UAD_Lookup..RegionGroup rg ON rg.RegionGroupID = r.RegionGroupID
					JOIN UAD_Lookup..CategoryCode cc ON cc.CategoryCodeID = ps.PubCategoryID
					JOIN UAD_Lookup..CategoryCodeType cct ON cct.CategoryCodeTypeID = cc.CategoryCodeTypeID
					JOIN UAD_Lookup..Code c ON c.CodeValue = ps.demo7
					WHERE c.CodeTypeId = @DeliverID and ps.CountryID = 429 AND ps.IssueID = @IssueID and ps.CountryID != @CountryID
					group by  
						cct.CategoryCodeTypeName, 
						c.DisplayName, 
						ISNULL(c.CodeId, 0),
						(CASE WHEN cct.CategoryCodeTypeName LIKE 'Qualified%' THEN cct.CategoryCodeTypeName ELSE 'Non Qualified' END)

					union --Foreign

					SELECT 
						'' as 'RegionName', 
						cct.CategoryCodeTypeName, 
						sum(ps.Copies) as 'Copies',
						c.DisplayName as 'Demo7', 
						ISNULL(c.CodeId, 0) as 'DemoID',
						(CASE WHEN cct.CategoryCodeTypeName LIKE 'Qualified%' THEN cct.CategoryCodeTypeName ELSE 'Non Qualified' END) as 'Category Type', 
						'' as 'RegionCode',
						0 as 'RegionID', 
						9999 as 'CountryID',
						'FOREIGN' as 'CountryRecap',
						5 as 'RecapOrder'
					FROM IssueArchiveProductSubscription ps with(NOLOCK)
					JOIN #PubSubscriptionID s ON s.PubSubscriptionID = ps.PubSubscriptionID
					LEFT JOIN UAD_Lookup..Region r ON r.RegionID = ps.RegionID
					LEFT JOIN UAD_Lookup..RegionGroup rg ON rg.RegionGroupID = r.RegionGroupID
					JOIN UAD_Lookup..CategoryCode cc ON cc.CategoryCodeID = ps.PubCategoryID
					JOIN UAD_Lookup..CategoryCodeType cct ON cct.CategoryCodeTypeID = cc.CategoryCodeTypeID
					JOIN UAD_Lookup..Code c ON c.CodeValue = ps.demo7
					WHERE c.CodeTypeId = @DeliverID and ps.RegionCode='FO' AND ps.IssueID = @IssueID and ps.CountryID != @CountryID and ps.CountryID not in (1,2,429)
					group by
						cct.CategoryCodeTypeName, 
						c.DisplayName, 
						ISNULL(c.CodeId, 0),
						(CASE WHEN cct.CategoryCodeTypeName LIKE 'Qualified%' THEN cct.CategoryCodeTypeName ELSE 'Non Qualified' END)
					
					DROP TABLE #tmpSingleCountry3
				END
			ELSE
				BEGIN
					SELECT 
						ISNULL(r.RegionName, 'No Response') as 'RegionName', 
						cct.CategoryCodeTypeName, 
						sum(ps.Copies) as 'Copies', 
						c.DisplayName as 'Demo7',
						ISNULL(c.CodeId, 0) as 'DemoID', 
						(CASE WHEN cct.CategoryCodeTypeName LIKE 'Qualified%' THEN cct.CategoryCodeTypeName ELSE 'Non Qualified' END) as 'Category Type', 
						ISNULL(ps.RegionCode, 'No Response') as 'RegionCode', 
						ISNULL(r.RegionID, 0) as 'RegionID',
						ISNULL(NULLIF(ps.CountryID, 0), '') as 'CountryID',
						@recapName as 'CountryRecap',
						case when @CountryID = 2 then 3 when @CountryID = 429 then 4 else 0 end as 'RecapOrder'
					into #tmpSingleCountry4
					FROM IssueArchiveProductSubscription ps with(NOLOCK)
						JOIN #PubSubscriptionID s ON s.PubSubscriptionID = ps.PubSubscriptionID
						LEFT JOIN UAD_Lookup..Region r ON r.RegionID = ps.RegionID OR r.RegionCode = ps.RegionCode
						JOIN UAD_Lookup..CategoryCode cc ON cc.CategoryCodeID = ps.PubCategoryID
						JOIN UAD_Lookup..CategoryCodeType cct ON cct.CategoryCodeTypeID = cc.CategoryCodeTypeID
						JOIN UAD_Lookup..Code c ON c.CodeValue = ps.demo7
					WHERE c.CodeTypeId = @DeliverID AND ISNULL(NULLIF(ps.CountryID, 0), '') = '' AND ps.IssueID = @IssueID
					group by ISNULL(r.RegionName, 'No Response'), 
						cct.CategoryCodeTypeName, 
						c.DisplayName, 
						ISNULL(c.CodeId, 0),
						(CASE WHEN cct.CategoryCodeTypeName LIKE 'Qualified%' THEN cct.CategoryCodeTypeName ELSE 'Non Qualified' END), 
						ISNULL(ps.RegionCode, 'No Response'),
						ISNULL(r.RegionID, 0), 
						ps.CountryID


					if(select count(*) from #tmpSingleCountry4) = 0
						insert into #tmpSingleCountry4
						values(@cname,'Qualified Free',0,'Print',0,'Qualified Free','',0,0,@recapName,0)


					select * from #tmpSingleCountry
					union --USA

					SELECT 
						'' as 'RegionName', 
						cct.CategoryCodeTypeName, 
						sum(ps.Copies) as 'Copies',
						c.DisplayName as 'Demo7', 
						ISNULL(c.CodeId, 0) as 'DemoID',
						(CASE WHEN cct.CategoryCodeTypeName LIKE 'Qualified%' THEN cct.CategoryCodeTypeName ELSE 'Non Qualified' END) as 'Category Type', 
						'' as 'RegionCode',
						0 as 'RegionID', 
						1 as 'CountryID',
						'UNITED STATES' as 'CountryRecap',
						1 as 'RecapOrder'
					FROM PubSubscriptions ps with(NOLOCK)
					JOIN #PubSubscriptionID s ON s.PubSubscriptionID = ps.PubSubscriptionID
					LEFT JOIN UAD_Lookup..Region r ON r.RegionID = ps.RegionID
					LEFT JOIN UAD_Lookup..RegionGroup rg ON rg.RegionGroupID = r.RegionGroupID
					JOIN UAD_Lookup..CategoryCode cc ON cc.CategoryCodeID = ps.PubCategoryID
					JOIN UAD_Lookup..CategoryCodeType cct ON cct.CategoryCodeTypeID = cc.CategoryCodeTypeID
					JOIN UAD_Lookup..Code c ON c.CodeValue = ps.demo7
					WHERE c.CodeTypeId = @DeliverID and ps.CountryID = 1 and rg.RegionGroupName != 'Territories'
					group by  
						cct.CategoryCodeTypeName, 
						c.DisplayName, 
						ISNULL(c.CodeId, 0),
						(CASE WHEN cct.CategoryCodeTypeName LIKE 'Qualified%' THEN cct.CategoryCodeTypeName ELSE 'Non Qualified' END)

					union --Territories

					SELECT 
						'' as 'RegionName', 
						cct.CategoryCodeTypeName, 
						sum(ps.Copies) as 'Copies',
						c.DisplayName as 'Demo7', 
						ISNULL(c.CodeId, 0) as 'DemoID',
						(CASE WHEN cct.CategoryCodeTypeName LIKE 'Qualified%' THEN cct.CategoryCodeTypeName ELSE 'Non Qualified' END) as 'Category Type', 
						'' as 'RegionCode',
						0 as 'RegionID', 
						1 as 'CountryID',
						'TERRITORIES' as 'CountryRecap',
						2 as 'RecapOrder'
					FROM PubSubscriptions ps with(NOLOCK)
					JOIN #PubSubscriptionID s ON s.PubSubscriptionID = ps.PubSubscriptionID
					LEFT JOIN UAD_Lookup..Region r ON r.RegionID = ps.RegionID
					LEFT JOIN UAD_Lookup..RegionGroup rg ON rg.RegionGroupID = r.RegionGroupID
					JOIN UAD_Lookup..CategoryCode cc ON cc.CategoryCodeID = ps.PubCategoryID
					JOIN UAD_Lookup..CategoryCodeType cct ON cct.CategoryCodeTypeID = cc.CategoryCodeTypeID
					JOIN UAD_Lookup..Code c ON c.CodeValue = ps.demo7
					WHERE c.CodeTypeId = @DeliverID and ps.CountryID = 1 and rg.RegionGroupName = 'Territories'
					group by  
						cct.CategoryCodeTypeName, 
						c.DisplayName, 
						ISNULL(c.CodeId, 0),
						(CASE WHEN cct.CategoryCodeTypeName LIKE 'Qualified%' THEN cct.CategoryCodeTypeName ELSE 'Non Qualified' END)

					union --Canada

					SELECT 
						'' as 'RegionName', 
						cct.CategoryCodeTypeName, 
						sum(ps.Copies) as 'Copies',
						c.DisplayName as 'Demo7', 
						ISNULL(c.CodeId, 0) as 'DemoID',
						(CASE WHEN cct.CategoryCodeTypeName LIKE 'Qualified%' THEN cct.CategoryCodeTypeName ELSE 'Non Qualified' END) as 'Category Type', 
						'' as 'RegionCode',
						0 as 'RegionID', 
						2 as 'CountryID',
						'CANADA' as 'CountryRecap',
						3 as 'RecapOrder'
					FROM PubSubscriptions ps with(NOLOCK)
					JOIN #PubSubscriptionID s ON s.PubSubscriptionID = ps.PubSubscriptionID
					LEFT JOIN UAD_Lookup..Region r ON r.RegionID = ps.RegionID
					LEFT JOIN UAD_Lookup..RegionGroup rg ON rg.RegionGroupID = r.RegionGroupID
					JOIN UAD_Lookup..CategoryCode cc ON cc.CategoryCodeID = ps.PubCategoryID
					JOIN UAD_Lookup..CategoryCodeType cct ON cct.CategoryCodeTypeID = cc.CategoryCodeTypeID
					JOIN UAD_Lookup..Code c ON c.CodeValue = ps.demo7
					WHERE c.CodeTypeId = @DeliverID and ps.CountryID = 2 and ps.CountryID != @CountryID
					group by  
						cct.CategoryCodeTypeName, 
						c.DisplayName, 
						ISNULL(c.CodeId, 0),
						(CASE WHEN cct.CategoryCodeTypeName LIKE 'Qualified%' THEN cct.CategoryCodeTypeName ELSE 'Non Qualified' END)

					union --Mexico

					SELECT 
						'' as 'RegionName', 
						cct.CategoryCodeTypeName, 
						sum(ps.Copies) as 'Copies',
						c.DisplayName as 'Demo7', 
						ISNULL(c.CodeId, 0) as 'DemoID',
						(CASE WHEN cct.CategoryCodeTypeName LIKE 'Qualified%' THEN cct.CategoryCodeTypeName ELSE 'Non Qualified' END) as 'Category Type', 
						'' as 'RegionCode',
						0 as 'RegionID', 
						429 as 'CountryID',
						'MEXICO' as 'CountryRecap',
						4 as 'RecapOrder'
					FROM PubSubscriptions ps with(NOLOCK)
					JOIN #PubSubscriptionID s ON s.PubSubscriptionID = ps.PubSubscriptionID
					LEFT JOIN UAD_Lookup..Region r ON r.RegionID = ps.RegionID
					LEFT JOIN UAD_Lookup..RegionGroup rg ON rg.RegionGroupID = r.RegionGroupID
					JOIN UAD_Lookup..CategoryCode cc ON cc.CategoryCodeID = ps.PubCategoryID
					JOIN UAD_Lookup..CategoryCodeType cct ON cct.CategoryCodeTypeID = cc.CategoryCodeTypeID
					JOIN UAD_Lookup..Code c ON c.CodeValue = ps.demo7
					WHERE c.CodeTypeId = @DeliverID and ps.CountryID = 429 and ps.CountryID != @CountryID
					group by  
						cct.CategoryCodeTypeName, 
						c.DisplayName, 
						ISNULL(c.CodeId, 0),
						(CASE WHEN cct.CategoryCodeTypeName LIKE 'Qualified%' THEN cct.CategoryCodeTypeName ELSE 'Non Qualified' END)

					union --Foreign

					SELECT 
						'' as 'RegionName', 
						cct.CategoryCodeTypeName, 
						sum(ps.Copies) as 'Copies',
						c.DisplayName as 'Demo7', 
						ISNULL(c.CodeId, 0) as 'DemoID',
						(CASE WHEN cct.CategoryCodeTypeName LIKE 'Qualified%' THEN cct.CategoryCodeTypeName ELSE 'Non Qualified' END) as 'Category Type', 
						'' as 'RegionCode',
						0 as 'RegionID', 
						9999 as 'CountryID',
						'FOREIGN' as 'CountryRecap',
						5 as 'RecapOrder'
					FROM PubSubscriptions ps with(NOLOCK)
					JOIN #PubSubscriptionID s ON s.PubSubscriptionID = ps.PubSubscriptionID
					LEFT JOIN UAD_Lookup..Region r ON r.RegionID = ps.RegionID
					LEFT JOIN UAD_Lookup..RegionGroup rg ON rg.RegionGroupID = r.RegionGroupID
					JOIN UAD_Lookup..CategoryCode cc ON cc.CategoryCodeID = ps.PubCategoryID
					JOIN UAD_Lookup..CategoryCodeType cct ON cct.CategoryCodeTypeID = cc.CategoryCodeTypeID
					JOIN UAD_Lookup..Code c ON c.CodeValue = ps.demo7
					WHERE c.CodeTypeId = @DeliverID and ps.RegionCode='FO' and ps.CountryID != @CountryID and ps.CountryID not in (1,2,429)
					group by
						cct.CategoryCodeTypeName, 
						c.DisplayName, 
						ISNULL(c.CodeId, 0),
						(CASE WHEN cct.CategoryCodeTypeName LIKE 'Qualified%' THEN cct.CategoryCodeTypeName ELSE 'Non Qualified' END)

					DROP TABLE #tmpSingleCountry4
				END
		END

	DROP TABLE #PubSubscriptionID
END
