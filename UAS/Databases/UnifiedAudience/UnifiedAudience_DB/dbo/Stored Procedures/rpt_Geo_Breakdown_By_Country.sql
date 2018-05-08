CREATE proc [dbo].[rpt_Geo_Breakdown_By_Country]
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
	@startDate varchar(50),		
	@endDate varchar(10),
	@AdHocXML varchar(8000),
	@includeAllCountry bit = 0,
	@WaveMail varchar(100) = ''     
)    
as     
BEGIN 
	
	SET NOCOUNT ON
	
	declare @magazineCode varchar(20)
	select @magazineCode  = PubCode 
	from Pubs 
	where PubID = @ProductID

	create table #SubscriptionID (SubscriptionID int, Copies int)  
	
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

	declare @TotalQualified int,
		@TotalNonQualified int,
		@Total int
			
	Select @Total = ISNULL(SUM(ps.Copies),0),
		@TotalQualified = ISNULL( SUM( case when cg.CategoryCodeTypeName in ('QUALIFIED FREE', 'QUALIFIED PAID') then ps.Copies end),0),
		@TotalNonQualified = ISNULL( SUM( case when cg.CategoryCodeTypeName in ('NONQUALIFIED FREE', 'NONQUALIFIED PAID') then ps.Copies end),0)
	FROM PubSubscriptions ps  with (NOLOCK)  
		JOIN #SubscriptionID sf  with (NOLOCK) ON sf.SubscriptionID = ps.SubscriptionID  
		join UAD_Lookup..CategoryCode cc  with (NOLOCK) ON cc.CategoryCodeID = ps.PubCategoryID 
		JOIN UAD_Lookup..CategoryCodeType cg  with (NOLOCK) ON cc.CategoryCodeTypeID = cg.CategoryCodeTypeID
	WHERE ps.PubID = @ProductID  AND
		(cc.CategoryCodeValue < 60 or ps.PubCategoryID > 65) AND 
		ps.CountryID not in (1,3,4) AND 
		ps.CountryID is not null
		
	Begin
		select UPPER(ct.Area) Area,
			isnull(ct.ShortName, ' --- US ---') as Country, 
			sum(case when CategoryGroup_Name = 'QUALIFIED FREE' then counts else 0 end) as 'QUALIFIED NON PAID',
			sum(case when CategoryGroup_Name = 'QUALIFIED PAID' then counts else 0 end) as 'QUALIFIED PAID',
			SUM(case when (CategoryGroup_Name = 'QUALIFIED FREE' OR CategoryGroup_Name = 'QUALIFIED PAID') then counts else 0 end) as 'TOTAL QUALIFIED',
				
			case when @TotalQualified = 0 then '0' else
			convert(varchar(100),ISNULL((SUM(case when (CategoryGroup_Name = 'QUALIFIED PAID' OR CategoryGroup_Name = 'Qualified Free') then counts else 0 end) * 100.0)/
				(@TotalQualified * 1.0 ),0)) + '%' end as 'PERCENT QUALIFIED',
			sum(case when CategoryGroup_Name = 'NONQUALIFIED PAID' then counts else 0 end) as 'NON QUALIFIED PAID',
			sum(case when CategoryGroup_Name = 'NONQUALIFIED FREE' then counts else 0 end) as 'NON QUALIFIED NON PAID',
			sum(case when CategoryGroup_Name = 'ADVERTISER AGENCY' then counts else 0 end) as 'ADVERTISER AGENCY',
			SUM(case when (CategoryGroup_Name = 'NONQUALIFIED FREE' OR CategoryGroup_Name = 'NonQualified Paid') then counts else 0 end) as 'TOTAL NON QUALIFIED',
			case when @TotalNonQualified = 0 then '0' else
			convert(varchar(100),ISNULL((SUM(case when (CategoryGroup_Name = 'NONQUALIFIED FREE' OR CategoryGroup_Name = 'NONQUALIFIED PAID') then counts else 0 end) * 100.0)/
				(@TotalNonQualified * 1.0 ),0)) + '%' end   as 'PERCENT NON QUALIFIED',
			SUM(counts) as 'TOTAL',
			case when @Total = 0 then '0' else
			convert(varchar(100),SUM(counts * 100.0)/
				(@Total * 1.0 )) + '%' end as 'PERCENT'
  		from UAD_Lookup..Country ct  with (NOLOCK) 
			left outer join
			(
				select s.CountryID,
					case when c.CategoryCodeValue between 60 and 65 then 'ADVERTISER AGENCY' else CategoryCodeTypeName end as CategoryGroup_Name,
					C.CategoryCodeTypeID, SUM(s.COPIES) as counts
				From PubSubscriptions s with (NOLOCK) 
					join #SubscriptionID sf  with (NOLOCK) on sf.SubscriptionID = s.SubscriptionID  
					join UAD_Lookup..CategoryCode C  with (NOLOCK) on s.PubCategoryID = C.CategoryCodeID 
					join UAD_Lookup..CategoryCodeType cg  with (NOLOCK) on cg.CategoryCodeTypeID = C.CategoryCodeTypeID            	
				where PubID = @ProductID and C.CategoryCodeTypeID <> 5
				group by s.CountryID, s.PubCategoryID, CategoryCodeTypeName, C.CategoryCodeTypeID, c.CategoryCodeValue
			) inn1 on  inn1.CountryID = ct.CountryID
		where (@includeAllCountry = 1 or inn1.counts > 0) and ct.CountryID not in (1,3,4)
		group by ct.Area, ct.ShortName
		order by ct.Area, ct.ShortName

	end
	
	drop table #SubscriptionID

end