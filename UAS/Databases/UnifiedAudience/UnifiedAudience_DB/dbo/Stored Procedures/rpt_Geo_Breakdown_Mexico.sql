CREATE  proc [dbo].[rpt_Geo_Breakdown_Mexico]
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
	@WaveMail varchar(100) = ''
)    
as     
BEGIN
	
	SET NOCOUNT ON
	
	declare @magazineCode varchar(20)
	select @magazineCode  = PubCode 
	from Pubs with(nolock) 
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
	
	Begin
		select	state as 'State', 
			sum(case when CategoryGroup_Name = 'ADVERTISER AGENCY' then counts else 0 end) as 'ADVERTISER AGENCY',
			sum(case when CategoryGroup_Name = 'Qualified Free' then counts else 0 end) as 'QUALIFIED NON PAID',
			sum(case when CategoryGroup_Name = 'QUALIFIED PAID' then counts else 0 end) as 'QUALIFIED PAID',
			sum(case when CategoryGroup_Name = 'NonQualified Free' then counts else 0 end) as 'NON QUALIFIED NON PAID',
			sum(case when CategoryGroup_Name = 'NonQualified Paid' then counts else 0 end) as 'NON QUALIFIED PAID',
			SUM(counts) as 'TOTAL'
  		from
			(
			select st.RegionName as state, 
				case when s.PubCategoryID between 60 and 65 then 'ADVERTISER AGENCY' else CategoryCodeTypeName end as CategoryGroup_Name,
				sum(sf.Copies) as counts
			From PubSubscriptions s with(nolock)     
				join #SubscriptionID sf on sf.SubscriptionID = s.SubscriptionID  
				join UAD_Lookup..CategoryCode C with(nolock) on s.PubCategoryID = C.CategoryCodeValue 
				join UAD_Lookup..CategoryCodeType cg with(nolock) on cg.CategoryCodeTypeID = C.CategoryCodeTypeID 
				join UAD_Lookup..TransactionCode t with(nolock) on s.PubTransactionID = t.TransactionCodeValue  
				join UAD_Lookup..Region st with(nolock) on  s.RegionCode = st.RegionCode
			where PubID = @ProductID and C.CategoryCodeTypeID <> 5 and st.CountryID = (SELECT CountryID FROM UAD_Lookup..Country WHERE ShortName LIKE 'Mexico')
			group by st.RegionName, s.PubCategoryID, CategoryCodeTypeName
			) inn1
		group by state
	end
	
	drop table #SubscriptionID

end