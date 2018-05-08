CREATE proc [dbo].[sp_rpt_Qualified_Breakdown_by_country]   
(    
	@MagazineID int ,  
	@Filters TEXT,
	@PrintColumns varchar(4000),
	@Download char(1) ,  
	@includeAllCountry bit = 0     
)    
as     
Begin    

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
	exec sp_getSubscribers_using_XMLFilters @MagazineID, @Filters, 1
	 

	CREATE UNIQUE CLUSTERED INDEX IX_1 on #SubscriptionID (SubscriptionID)

	if @Download = 0
	Begin
		select	isnull(C.Area, C.ShortName) as region,  --'--- US ---'
				isnull(C.ShortName, C.ShortName) as country,  --' --- US ---'
				sum(case when CategoryGroup_Name = 'ADVERTISER AGENCY' then counts else 0 end) as 'ADVERTISER_AGENCY',
				sum(case when CategoryGroup_Name = 'Qualified Free' then counts else 0 end) as 'QUALIFIED_NON_PAID',
				sum(case when CategoryGroup_Name = 'Qualified Paid' then counts else 0 end) as 'QUALIFIED_PAID',
				sum(case when CategoryGroup_Name = 'NonQualified Free' then counts else 0 end) as 'NON_QUALIFIED_NON_PAID',
				sum(case when CategoryGroup_Name = 'NonQualified Paid' then counts else 0 end) as 'NON_QUALIFIED_PAID'
  		from
  			uas..Country C left outer join
			(
			select 
					sb.CountryID,
					case when cc.CategoryCodeValue between 60 and 65 then 'ADVERTISER AGENCY' else cct.CategoryCodeTypeName end as CategoryGroup_Name,
					cct.CategoryCodeTypeID, SUM(s.COPIES) as counts
			From 
					Subscription s join
					#SubscriptionID sf on sf.SubscriptionID = s.SubscriptionID  join 
					Subscriber sb on s.SubscriberID = sb.subscriberID join
					Action a on a.ActionID = s.ActionID_Current join
					CategoryCode cc on cc.CategoryCodeID  = a.CategoryCodeID join
					CategoryCodeType cct on cct.CategoryCodeTypeID = cc.CategoryCodeTypeID join
					TransactionCode tc on tc.TransactionCodeID = a.TransactionCodeID
			where 
					PublicationID = @PublicationID and cct.CategoryCodeTypeID <> 5
			group by 
					sb.CountryID, cc.CategoryCodeValue, cct.CategoryCodeTypeName , cct.CategoryCodeTypeID
			) inn1 on  inn1.CountryID = C.CountryID
		where
				(@includeAllCountry = 1 or inn1.counts > 0) and C.CountryID not in (1,3,4)
		group by
			C.Area, C.ShortName
		order by
			C.Area, C.ShortName
	end
	else
	Begin
		exec(	'select  distinct ''' + @PublicationCode + ''' as PubCode, s.SubscriberID, s.EMAILADDRESS, s.FNAME, s.LNAME, s.COMPANY, s.TITLE, s.ADDRESS, s.MAILSTOP, s.CITY, s.STATE, s.ZIP, s.PLUS4, s.COUNTRY, s.PHONE, s.FAX, s.website, s.Category_ID as CAT, c.Category_Name as CategoryName, s.Transaction_ID as XACT, Transaction_Date as XACTDate, FORZIP, COUNTY, s.QSource_ID, q.Qsource_Name + '' (''+ q.Qsource_value + '')'' as Qsource, QualificationDate,Subsrc, S.COPIES ' + @PrintColumns + 
				' From Subscriptions s  join #SubscriptionID sf on sf.SubscriptionID = s.SubscriptionID left outer join QSource q on q.Qsource_ID = s.Qsource_ID  join  Category C on s.Category_ID = C.Category_ID join ' +
				' Categorygroup cg on cg.Categorygroup_ID = C.Categorygroup_ID join [Transaction] t on s.Transaction_ID = t.Transaction_ID   join country ct on  s.Country_ID = ct.Country_ID ' +
				' where magazineID = ' + @MagazineID + ' and C.Categorygroup_ID <> 5')
	end
	drop table #SubscriptionID

end