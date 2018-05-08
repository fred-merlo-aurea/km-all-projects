CREATE proc [dbo].[rpt_Geo_Breakdown_Domestic_Old]
(      
	@ProductID int, 
	@CategoryIDs varchar(800),
	@CategoryCodes varchar(800),
	@TransactionIDs varchar(800),
	@TransactionCodes varchar(800),
	@QsourceIDs varchar(800),
	@StateIDs varchar(800),
	@Regions varchar(max),
	@CountryIDs varchar(1500),
	@Email varchar(10),
	@Phone varchar(10),
	@Mobile varchar(10),
	@Fax varchar(10),
	@ResponseIDs varchar(1000),
	@Demo7 varchar(50),		
	@Year varchar(20),
	@startDate varchar(10),		
	@endDate varchar(10),
	@AdHocXML varchar(8000),
	@includeAllStates bit = 0,
	@WaveMail varchar(100) = ''         
)      
as    

BEGIN
	
	--Declare @ProductID int = 1,
	--@CategoryIDs varchar(800) = '1,3',
	--@CategoryCodes varchar(800) = '1,2,3,4,15,16,17,18,19,21',
	--@TransactionIDs varchar(800) = '1,3',
	--@TransactionCodes varchar(800) = '',
	--@QsourceIDs varchar(800) = '',
	--@StateIDs varchar(800) = '',
	--@Regions varchar(max) = '',
	--@CountryIDs varchar(800) = '',
	--@Email varchar(10) = '',
	--@Phone varchar(10) = '',
	--@Fax varchar(10) = '',
	--@Mobile varchar(10) = '',
	--@ResponseIDs varchar(800) = '',
	--@Demo7 varchar(10) = '',		
	--@Year varchar(20) = '',
	--@startDate varchar(10) = '',		
	--@endDate varchar(10) = '',
	--@AdHocXML varchar(8000) = '',
	--@includeAllStates bit = 0
	
	SET NOCOUNT ON  

	declare @magazineCode varchar(20)
	select @magazineCode  = PubCode 
	from Pubs with(nolock) 
	where PubID = @ProductID

	create table #SubscriptionID (SubscriptionID int, copies int)   
    
	Insert into #SubscriptionID   
	exec rpt_GetSubscriptionIDs_Copies_From_Filter 
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

	 BEGIN  
		select rg.RegionGroupName,
			isnull(st.ZipCodeRange,'') + ' ' + st.RegionCode + ' ....' as 'State & Zip Code',   
			rg.Sortorder sort_order,  
			isnull(st.country_sort_order,10) as country_sort_order,  
			isnull(st.ZipCodeRangeSortOrder,0) as zip_sort_order,
			st.CountryID AS country,
			1 as sort, 
			sum(case when CategoryGroup_Name = 'Qualified Free' then counts else 0 end) as 'QUALIFIED NON PAID',
			sum(case when CategoryGroup_Name = 'QUALIFIED PAID' then counts else 0 end) as 'QUALIFIED PAID',
			SUM(case when (CategoryGroup_Name = 'Qualified Free' OR CategoryGroup_Name = 'QUALIFIED PAID') then counts else 0 end) as 'TOTAL QUALIFIED',
			convert(varchar(100),(SUM(case when (CategoryGroup_Name = 'QUALIFIED PAID' OR CategoryGroup_Name = 'Qualified Free') then counts else 0 end) * 100.0)/
				((select SUM(ps.Copies) 
				FROM Subscriptions s with(nolock) 
					JOIN PubSubscriptions ps with(nolock) ON ps.SubscriptionID = s.SubscriptionID
					JOIN #SubscriptionID sf on sf.SubscriptionID = s.SubscriptionID
					JOIN UAD_Lookup..CategoryCode cc with(nolock) ON cc.CategoryCodeID = ps.PubCategoryID 
					JOIN UAD_Lookup..CategoryCodeType cg with(nolock) ON cc.CategoryCodeTypeID = cg.CategoryCodeTypeID
				WHERE cg.CategoryCodeTypeName in ('Qualified Free', 'QUALIFIED PAID') AND ps.PubID = @ProductID
					AND cc.CategoryCodeTypeID <> 5 AND cc.CategoryCodeValue < 60 or cc.CategoryCodeValue > 65) * 1.0)) + '%' as 'Qualified Percent',
			sum(case when CategoryGroup_Name = 'NonQualified Paid' then counts else 0 end) as 'NON QUALIFIED PAID',
			sum(case when CategoryGroup_Name = 'NonQualified Free' then counts else 0 end) as 'NON QUALIFIED NON PAID',
			sum(case when CategoryGroup_Name = 'ADVERTISER AGENCY' then counts else 0 end) as 'ADVERTISER AGENCY',
			SUM(case when (CategoryGroup_Name = 'NonQualified Free' OR CategoryGroup_Name = 'NonQualified Paid') then counts else 0 end) as 'TOTAL NON QUALIFIED',
			convert(varchar(100),ISNULL((SUM(case when (CategoryGroup_Name = 'NonQualified Free' OR CategoryGroup_Name = 'NonQualified Paid') then counts else 0 end) * 100.0)/
				((select SUM(ps.Copies) 
				FROM Subscriptions s 
					JOIN PubSubscriptions ps ON ps.SubscriptionID = s.SubscriptionID
					JOIN #SubscriptionID sf on sf.SubscriptionID = s.SubscriptionID
					JOIN UAD_Lookup..CategoryCode cc with(nolock) ON cc.CategoryCodeID = ps.PubCategoryID 
					JOIN UAD_Lookup..CategoryCodeType cg with(nolock) ON cc.CategoryCodeTypeID = cg.CategoryCodeTypeID
				WHERE cg.CategoryCodeTypeName in ('NonQualified Free', 'NonQualified Paid') AND ps.PubID = @ProductID
					AND cc.CategoryCodeTypeID <> 5 AND cc.CategoryCodeValue < 60 or cc.CategoryCodeValue > 65) * 1.0), 0)) + '%' as 'Non Qualified Percent'
		from UAD_Lookup..Region st 
			left outer join 
			(  
				select s.RegionCode,
						s.Country,
						s.CountryID,
						case when c.CategoryCodeValue between 60 and 65 then 'ADVERTISER AGENCY' else CategoryCodeTypeName end as CategoryGroup_Name,  
						C.CategoryCodeTypeID, sum(s.copies) as counts  --count(s.subscriptionID) as counts
				From PubSubscriptions s with(nolock) 
					join #SubscriptionID sf on sf.SubscriptionID = s.SubscriptionID  
					join UAD_Lookup..CategoryCode C with(nolock) on s.PubCategoryID = C.CategoryCodeID 
					join UAD_Lookup..CategoryCodeType cg with(nolock) on cg.CategoryCodeTypeID = C.CategoryCodeTypeID 
				where PubID = @ProductID and C.CategoryCodeTypeID <> 5  
				group by s.RegionCode, s.Country, s.CountryID,s.PubCategoryID, CategoryCodeTypeName, C.CategoryCodeTypeID, c.CategoryCodeValue
			) inn1 on st.RegionCode = inn1.RegionCode and (st.CountryID = inn1.CountryID or inn1.country  is null or inn1.RegionCode = 'FO')
			join UAD_Lookup..RegionGroup rg with(nolock) ON st.RegionGroupID = rg.RegionGroupID
		where (@includeAllStates = 1 or inn1.counts > 0)
		group by  rg.RegionGroupName,
			st.RegionCode,  
			isnull(st.ZipCodeRange,'') + ' ' + st.RegionCode + ' ....',   
			rg.Sortorder ,
			isnull(st.country_sort_order,10),  
			isnull(st.ZipCodeRangeSortOrder,0),
			st.CountryID		
				
		UNION 
		
		select rg.RegionGroupName,
			rg.RegionGroupName as 'State & Zip Code',
			rg.Sortorder  as sort_order,  
			0 as country_sort_order,  
			99 as zip_sort_order,
			0 AS country,   
			2 as sort,
			sum(case when CategoryGroup_Name = 'Qualified Free' then counts else 0 end) as 'QUALIFIED NON PAID',
			sum(case when CategoryGroup_Name = 'QUALIFIED PAID' then counts else 0 end) as 'QUALIFIED PAID',
			SUM(case when (CategoryGroup_Name = 'Qualified Free' OR CategoryGroup_Name = 'QUALIFIED PAID') then counts else 0 end) as 'TOTAL QUALIFIED',
			convert(varchar(100),(SUM(case when (CategoryGroup_Name = 'QUALIFIED PAID' OR CategoryGroup_Name = 'Qualified Free') then counts else 0 end) * 100.0)/
				((select SUM(ps.Copies) 
				FROM Subscriptions s with(nolock) 
					JOIN PubSubscriptions ps with(nolock) ON ps.SubscriptionID = s.SubscriptionID
					JOIN #SubscriptionID sf on sf.SubscriptionID = s.SubscriptionID
					JOIN UAD_Lookup..CategoryCode cc with(nolock) ON cc.CategoryCodeID = ps.PubCategoryID 
					JOIN UAD_Lookup..CategoryCodeType cg with(nolock) ON cc.CategoryCodeTypeID = cg.CategoryCodeTypeID
				WHERE cg.CategoryCodeTypeName in ('Qualified Free', 'QUALIFIED PAID') AND ps.PubID = @ProductID
					AND cc.CategoryCodeTypeID <> 5 AND cc.CategoryCodeValue < 60 or cc.CategoryCodeValue > 65) * 1.0)) + '%' as 'Qualified Percent',
			sum(case when CategoryGroup_Name = 'NonQualified Paid' then counts else 0 end) as 'NON QUALIFIED PAID',
			sum(case when CategoryGroup_Name = 'NonQualified Free' then counts else 0 end) as 'NON QUALIFIED NON PAID',
			sum(case when CategoryGroup_Name = 'ADVERTISER AGENCY' then counts else 0 end) as 'ADVERTISER AGENCY',
			SUM(case when (CategoryGroup_Name = 'NonQualified Free' OR CategoryGroup_Name = 'NonQualified Paid') then counts else 0 end) as 'TOTAL NON QUALIFIED',
			convert(varchar(100),ISNULL((SUM(case when (CategoryGroup_Name = 'NonQualified Free' OR CategoryGroup_Name = 'NonQualified Paid') then counts else 0 end) * 100.0)/
				((select SUM(ps.Copies) 
				FROM Subscriptions s 
					JOIN PubSubscriptions ps ON ps.SubscriptionID = s.SubscriptionID
					JOIN #SubscriptionID sf on sf.SubscriptionID = s.SubscriptionID
					JOIN UAD_Lookup..CategoryCode cc with(nolock) ON cc.CategoryCodeID = ps.PubCategoryID 
					JOIN UAD_Lookup..CategoryCodeType cg with(nolock) ON cc.CategoryCodeTypeID = cg.CategoryCodeTypeID
				WHERE cg.CategoryCodeTypeName in ('NonQualified Free', 'NonQualified Paid') AND ps.PubID = @ProductID
					AND cc.CategoryCodeTypeID <> 5 AND cc.CategoryCodeValue < 60 or cc.CategoryCodeValue > 65) * 1.0), 0)) + '%' as 'Non Qualified Percent'
		from UAD_Lookup..Region st with(nolock) 
			left outer join 
			(  
				select s.RegionCode,
					s.Country,
					s.CountryID,
					case when c.CategoryCodeValue between 60 and 65 then 'ADVERTISER AGENCY' else CategoryCodeTypeName end as CategoryGroup_Name,  
					C.CategoryCodeTypeID, sum(s.copies) as counts
				From PubSubscriptions s with(nolock) 
					join #SubscriptionID sf on sf.SubscriptionID = s.SubscriptionID  
					join UAD_Lookup..CategoryCode C with(nolock) on s.PubCategoryID = C.CategoryCodeID 
					join UAD_Lookup..CategoryCodeType cg with(nolock) on cg.CategoryCodeTypeID = C.CategoryCodeTypeID
				where PubID = @ProductID and C.CategoryCodeTypeID <> 5  
				group by s.RegionCode, s.Country, s.CountryID, s.PubCategoryID, CategoryCodeTypeName, C.CategoryCodeTypeID, CategoryCodeValue
			) inn1 on st.RegionCode = inn1.RegionCode and (st.CountryID = inn1.CountryID or inn1.country  is null or inn1.RegionCode = 'FO')
			join UAD_Lookup..RegionGroup rg with(nolock) ON st.RegionGroupID = rg.RegionGroupID
		where (@includeAllStates = 1 or inn1.counts > 0)
		group by rg.RegionGroupName, rg.Sortorder 
			
		UNION 
		
		select 'ZZZZZZZZZ',
			co.ShortName as 'State & Zip Code',
			99  as sort_order,  
			0 as country_sort_order,  
			0 as zip_sort_order,
			0 AS country,   
			3 as sort,
			sum(case when CategoryGroup_Name = 'Qualified Free' then counts else 0 end) as 'QUALIFIED NON PAID',
			sum(case when CategoryGroup_Name = 'QUALIFIED PAID' then counts else 0 end) as 'QUALIFIED PAID',
			SUM(case when (CategoryGroup_Name = 'Qualified Free' OR CategoryGroup_Name = 'QUALIFIED PAID') then counts else 0 end) as 'TOTAL QUALIFIED',
			convert(varchar(100),(SUM(case when (CategoryGroup_Name = 'QUALIFIED PAID' OR CategoryGroup_Name = 'Qualified Free') then counts else 0 end) * 100.0)/
				((select SUM(ps.Copies) 
				FROM Subscriptions s with(nolock) 
					JOIN PubSubscriptions ps with(nolock) ON ps.SubscriptionID = s.SubscriptionID
					JOIN #SubscriptionID sf on sf.SubscriptionID = s.SubscriptionID
					JOIN UAD_Lookup..CategoryCode cc with(nolock) ON cc.CategoryCodeID = ps.PubCategoryID 
					JOIN UAD_Lookup..CategoryCodeType cg with(nolock) ON cc.CategoryCodeTypeID = cg.CategoryCodeTypeID
				WHERE cg.CategoryCodeTypeName in ('Qualified Free', 'QUALIFIED PAID') AND ps.PubID = @ProductID
					AND cc.CategoryCodeTypeID <> 5 AND cc.CategoryCodeValue < 60 or cc.CategoryCodeValue > 65) * 1.0)) + '%' as 'Qualified Percent',
			sum(case when CategoryGroup_Name = 'NonQualified Paid' then counts else 0 end) as 'NON QUALIFIED PAID',
			sum(case when CategoryGroup_Name = 'NonQualified Free' then counts else 0 end) as 'NON QUALIFIED NON PAID',
			sum(case when CategoryGroup_Name = 'ADVERTISER AGENCY' then counts else 0 end) as 'ADVERTISER AGENCY',
			SUM(case when (CategoryGroup_Name = 'NonQualified Free' OR CategoryGroup_Name = 'NonQualified Paid') then counts else 0 end) as 'TOTAL NON QUALIFIED',
			convert(varchar(100),ISNULL((SUM(case when (CategoryGroup_Name = 'NonQualified Free' OR CategoryGroup_Name = 'NonQualified Paid') then counts else 0 end) * 100.0)/
				((select SUM(ps.Copies) 
				FROM Subscriptions s 
					JOIN PubSubscriptions ps ON ps.SubscriptionID = s.SubscriptionID
					JOIN #SubscriptionID sf on sf.SubscriptionID = s.SubscriptionID
					JOIN UAD_Lookup..CategoryCode cc with(nolock) ON cc.CategoryCodeID = ps.PubCategoryID 
					JOIN UAD_Lookup..CategoryCodeType cg with(nolock) ON cc.CategoryCodeTypeID = cg.CategoryCodeTypeID
				WHERE cg.CategoryCodeTypeName in ('NonQualified Free', 'NonQualified Paid') AND ps.PubID = @ProductID
					AND cc.CategoryCodeTypeID <> 5 AND cc.CategoryCodeValue < 60 or cc.CategoryCodeValue > 65) * 1.0), 0)) + '%' as 'Non Qualified Percent'
		from UAD_Lookup..Region st with(nolock) 
			left outer join 
			(  
				select s.RegionCode,
						s.Country,
						s.CountryID,
						case when c.CategoryCodeValue between 60 and 65 then 'ADVERTISER AGENCY' else CategoryCodeTypeName end as CategoryGroup_Name,  
						C.CategoryCodeTypeID, sum(s.copies) as counts
				From PubSubscriptions s with(nolock) 
					join #SubscriptionID sf on sf.SubscriptionID = s.SubscriptionID  
					join UAD_Lookup..CategoryCode C with(nolock) on s.PubCategoryID = C.CategoryCodeID 
					join UAD_Lookup..CategoryCodeType cg with(nolock) on cg.CategoryCodeTypeID = C.CategoryCodeTypeID
				where PubID = @ProductID and C.CategoryCodeTypeID <> 5  
				group by s.RegionCode, s.Country, s.CountryID, s.PubCategoryID, CategoryCodeTypeName, C.CategoryCodeTypeID, CategoryCodeValue
			) inn1 on st.RegionCode = inn1.RegionCode --and (st.CountryID = inn1.CountryID or inn1.country  is null or inn1.RegionCode = 'FO')
			left outer join UAD_Lookup..RegionGroup rg with(nolock) ON st.RegionGroupID = rg.RegionGroupID
			left outer join UAD_Lookup..Country co with(nolock) ON co.CountryID = st.CountryID
		where (@includeAllStates = 1 or inn1.counts > 0) and co.ShortName in ('UNITED STATES', 'CANADA', 'MEXICO')
		group by co.ShortName 
			
		UNION
		select 'ZZZZZZZZZ',
			'FOREIGN' as 'State & Zip Code',
			100  as sort_order,  
			0 as country_sort_order,  
			0 as zip_sort_order,
			0 AS country,   
			3 as sort,
			sum(case when CategoryGroup_Name = 'Qualified Free' then counts else 0 end) as 'QUALIFIED NON PAID',
			sum(case when CategoryGroup_Name = 'QUALIFIED PAID' then counts else 0 end) as 'QUALIFIED PAID',
			SUM(case when (CategoryGroup_Name = 'Qualified Free' OR CategoryGroup_Name = 'QUALIFIED PAID') then counts else 0 end) as 'TOTAL QUALIFIED',
			convert(varchar(100),(SUM(case when (CategoryGroup_Name = 'QUALIFIED PAID' OR CategoryGroup_Name = 'Qualified Free') then counts else 0 end) * 100.0)/
				((select SUM(ps.Copies) 
				FROM Subscriptions s with(nolock) 
					JOIN PubSubscriptions ps with(nolock) ON ps.SubscriptionID = s.SubscriptionID
					JOIN #SubscriptionID sf on sf.SubscriptionID = s.SubscriptionID
					JOIN UAD_Lookup..CategoryCode cc with(nolock) ON cc.CategoryCodeID = ps.PubCategoryID 
					JOIN UAD_Lookup..CategoryCodeType cg with(nolock) ON cc.CategoryCodeTypeID = cg.CategoryCodeTypeID
				WHERE cg.CategoryCodeTypeName in ('Qualified Free', 'QUALIFIED PAID') AND ps.PubID = @ProductID
					AND cc.CategoryCodeTypeID <> 5 AND cc.CategoryCodeValue < 60 or cc.CategoryCodeValue > 65) * 1.0)) + '%' as 'Qualified Percent',
			sum(case when CategoryGroup_Name = 'NonQualified Paid' then counts else 0 end) as 'NON QUALIFIED PAID',
			sum(case when CategoryGroup_Name = 'NonQualified Free' then counts else 0 end) as 'NON QUALIFIED NON PAID',
			sum(case when CategoryGroup_Name = 'ADVERTISER AGENCY' then counts else 0 end) as 'ADVERTISER AGENCY',
			SUM(case when (CategoryGroup_Name = 'NonQualified Free' OR CategoryGroup_Name = 'NonQualified Paid') then counts else 0 end) as 'TOTAL NON QUALIFIED',
			convert(varchar(100),ISNULL((SUM(case when (CategoryGroup_Name = 'NonQualified Free' OR CategoryGroup_Name = 'NonQualified Paid') then counts else 0 end) * 100.0)/
				((select SUM(ps.Copies) 
				FROM Subscriptions s with(nolock) 
					JOIN PubSubscriptions ps with(nolock) ON ps.SubscriptionID = s.SubscriptionID
					JOIN #SubscriptionID sf on sf.SubscriptionID = s.SubscriptionID
					JOIN UAD_Lookup..CategoryCode cc with(nolock) ON cc.CategoryCodeID = ps.PubCategoryID 
					JOIN UAD_Lookup..CategoryCodeType cg with(nolock) ON cc.CategoryCodeTypeID = cg.CategoryCodeTypeID
				WHERE cg.CategoryCodeTypeName in ('NonQualified Free', 'NonQualified Paid') AND ps.PubID = @ProductID
					AND cc.CategoryCodeTypeID <> 5 AND cc.CategoryCodeValue < 60 or cc.CategoryCodeValue > 65) * 1.0), 0)) + '%' as 'Non Qualified Percent'
		from UAD_Lookup..Region st with(nolock) 
			left outer join 
			(  
				select s.RegionCode,
						s.Country,
						s.CountryID,
						case when c.CategoryCodeValue between 60 and 65 then 'ADVERTISER AGENCY' else CategoryCodeTypeName end as CategoryGroup_Name,  
						C.CategoryCodeTypeID, sum(s.copies) as counts
				From PubSubscriptions s with(nolock) 
					join #SubscriptionID sf on sf.SubscriptionID = s.SubscriptionID  
					join UAD_Lookup..CategoryCode C with(nolock) on s.PubCategoryID = C.CategoryCodeID 
					join UAD_Lookup..CategoryCodeType cg with(nolock) on cg.CategoryCodeTypeID = C.CategoryCodeTypeID
				where PubID = @ProductID and C.CategoryCodeTypeID <> 5  
				group by s.RegionCode, s.Country, s.CountryID, s.PubCategoryID, CategoryCodeTypeName, C.CategoryCodeTypeID, CategoryCodeValue 
			) inn1 on st.RegionCode = inn1.RegionCode --and (st.CountryID = inn1.CountryID or inn1.country  is null or inn1.RegionCode = 'FO')
			left outer join UAD_Lookup..RegionGroup rg with(nolock) ON st.RegionGroupID = rg.RegionGroupID
			left outer join UAD_Lookup..Country co with(nolock) ON co.CountryID = st.CountryID
		where (@includeAllStates = 1 or inn1.counts > 0) and inn1.RegionCode = 'FO'
		order by sort_order, zip_sort_order
	END  

	drop table #SubscriptionID

END