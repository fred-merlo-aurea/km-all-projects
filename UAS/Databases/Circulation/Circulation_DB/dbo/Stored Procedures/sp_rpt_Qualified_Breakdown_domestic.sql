CREATE proc [dbo].[sp_rpt_Qualified_Breakdown_domestic]     
(      
 @MagazineID int ,    
 @Filters TEXT,  
 @PrintColumns varchar(4000),  
 @Download char(1) ,
 @includeAllStates bit = 0         
)      
as    
   /*
declare  @MagazineID int ,    
 @Filters varchar(4000),  
 @PrintColumns varchar(4000),  
 @Download char(1) ,
 @includeAllStates bit = 0    
 
 
 set @MagazineID = 32887
 set @Filters = ''
 set @PrintColumns = ''
 set @Download = 0
 set @includeAllStates = 1
 */ 
Begin      
/*  
select 'abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyz' as State,   
  'abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyz' as region,   
  10 as sort_order,  
  10 as country_sort_order,  
  'abcdefghijklmnopqrstuvwxzy' as country,  
  100 as ADVERTISER_AGENCY,  
  100 as QUALIFIED_NON_PAID,    
  100 as QUALIFIED_PAID,  
  100 as NON_QUALIFIED_NON_PAID,  
  100 as NON_QUALIFIED_PAID  
*/  
 set nocount on  


	declare @publicationID int
	
	set @publicationID = @MagazineID
	
	declare @PublicationCode varchar(20)
	select @PublicationCode  = Publicationcode from Publication where PublicationID = @PublicationID

	if len(ltrim(rtrim(@PrintColumns))) > 0   
	Begin  
	set @PrintColumns  = ', ' + @PrintColumns   
	end  

	create table #SubscriptionID (SubscriptionID int, copies int)   

	Insert into #SubscriptionID     
	exec sp_getSubscribers_using_XMLFilters @publicationID, @Filters, 1

	CREATE UNIQUE CLUSTERED INDEX IX_1 on #SubscriptionID (SubscriptionID)

	select 
			CASE WHEN rg.RegionGroupName is null THEN
				MAX(CASE	when C.ShortName = 'CANADA' THEN 'CANADA' 
						when C.ShortName = 'MEXICO' THEN 'MEXICO' 	
						ELSE 'FOREIGN'
				END)
			ELSE
				rg.RegionGroupName
			END as region,  
	isnull(r.ZipCodeRange,'') + ' ' + r.RegionCode + ' ....' as state,   
	rg.Sortorder sort_order,  
	isnull(c.SortOrder,10) as country_sort_order,  
	isnull(r.ZipCodeRangeSortOrder,0) as zip_sort_order,
	CASE WHEN c.shortname is null THEN
		CASE WHEN rg.RegionGroupName = 'TERRITORIES' THEN 'TERRITORIES'
			 ELSE 'FOREIGN'
		END
	else
		c.ShortName 
	end as country,  
	sum(case when CategoryGroup_Name = 'ADVERTISER AGENCY' then counts else 0 end) as 'ADVERTISER_AGENCY',  
	sum(case when CategoryGroup_Name = 'Qualified Free' then counts else 0 end) as 'QUALIFIED_NON_PAID',  
	sum(case when CategoryGroup_Name = 'Qualified Paid' then counts else 0 end) as 'QUALIFIED_PAID',  
	sum(case when CategoryGroup_Name = 'NonQualified Free' then counts else 0 end) as 'NON_QUALIFIED_NON_PAID',  
	sum(case when CategoryGroup_Name = 'NonQualified Paid' then counts else 0 end) as 'NON_QUALIFIED_PAID'  
	from  
	 uas..Region r left outer join
	 uas..RegionGroup rg on rg.regiongroupID = r.regiongroupID left outer join 
	(  
		select 
				sb.regionID,
				sb.countryID,
				case when cc.CategoryCodeValue between 60 and 65 then 'ADVERTISER AGENCY' else cct.CategoryCodeTypeName end as CategoryGroup_Name,
				sum(s.copies) as counts 
		From   
				Subscription s join
				#SubscriptionID sf on sf.SubscriptionID = s.SubscriptionID  join 
				Subscriber sb on s.SubscriberID = sb.subscriberID join
				Action a on a.ActionID = s.ActionID_Current join
				CategoryCode cc on cc.CategoryCodeID  = a.CategoryCodeID join
				CategoryCodeType cct on cct.CategoryCodeTypeID = cc.CategoryCodeTypeID 
		where   
				S.PublicationID = @PublicationID
		group by   
				sb.regionID, sb.countryID, cc.CategoryCodeValue, cct.CategoryCodeTypeName
	) inn1 
	on r.RegionID = inn1.RegionID 
	left outer join uas..Country c on c.CountryID = inn1.CountryID
	where (@includeAllStates = 1 or inn1.counts > 0)
	group by  
		rg.RegionGroupName,  
		isnull(r.ZipCodeRange,'') + ' ' + r.RegionCode + ' ....',   
		rg.Sortorder,  
		isnull(c.SortOrder,10),  
		isnull(r.ZipCodeRangeSortOrder,0),
		CASE WHEN c.shortname is null THEN
		CASE WHEN rg.RegionGroupName = 'TERRITORIES' THEN 'TERRITORIES'
			 ELSE 'FOREIGN'
		END
	else
		c.ShortName 
	end
	order by isnull(c.SortOrder,10), rg.Sortorder,isnull(r.ZipCodeRangeSortOrder,0)

	
	drop table #SubscriptionID  
end