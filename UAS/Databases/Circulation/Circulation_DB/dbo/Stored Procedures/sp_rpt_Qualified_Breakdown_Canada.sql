CREATE proc [dbo].[sp_rpt_Qualified_Breakdown_Canada]
(    
	@MagazineID int ,  
	@Filters TEXT,
	@PrintColumns varchar(4000),	
	@Download char(1)         
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
		select	state, 
				sum(case when CategoryGroup_Name = 'ADVERTISER AGENCY' then counts else 0 end) as 'ADVERTISER_AGENCY',
				sum(case when CategoryGroup_Name = 'Qualified Free' then counts else 0 end) as 'QUALIFIED_NON_PAID',
				sum(case when CategoryGroup_Name = 'Qualified Paid' then counts else 0 end) as 'QUALIFIED_PAID',
				sum(case when CategoryGroup_Name = 'NonQualified Free' then counts else 0 end) as 'NON_QUALIFIED_NON_PAID',
				sum(case when CategoryGroup_Name = 'NonQualified Paid' then counts else 0 end) as 'NON_QUALIFIED_PAID'
  		from
			(
			select	r.RegionName as state, 
					case when cc.CategoryCodeValue between 60 and 65 then 'ADVERTISER AGENCY' else cct.CategoryCodeTypeName end as CategoryGroup_Name,
					sum(s.copies) as counts
			From 
					Subscription s join
					#SubscriptionID sf on sf.SubscriptionID = s.SubscriptionID  join 
					Subscriber sb on s.SubscriberID = sb.subscriberID join
					Action a on a.ActionID = s.ActionID_Current join
					CategoryCode cc on cc.CategoryCodeID  = a.CategoryCodeID join
					CategoryCodeType cct on cct.CategoryCodeTypeID = cc.CategoryCodeTypeID join
					TransactionCode tc on tc.TransactionCodeID = a.TransactionCodeID join
					uas..region r on  sb.regionID = r.regionID
			where 
					s.PublicationID = @publicationID and r.CountryID = (select c.countryID from uas..Country c where Shortname = 'CANADA')
			group by 
					r.RegionName, cc.CategoryCodeValue, cct.CategoryCodeTypeName
			) inn1
		group by
			state
	end
	else
	Begin
		exec (	'select  distinct ''' + @publicationCode + ''' as PubCode, s.SubscriberID, s.EMAILADDRESS, s.FNAME, s.LNAME, s.COMPANY, s.TITLE, s.ADDRESS, s.MAILSTOP, s.CITY, s.STATE, s.ZIP, s.PLUS4, s.COUNTRY, s.PHONE, s.FAX, s.website, s.Category_ID as CAT, c.Category_Name as CategoryName, s.Transaction_ID as XACT, s.Transaction_Date as XACTDate, s.FORZIP, s.COUNTY, s.QSource_ID, q.Qsource_Name + '' (''+ q.Qsource_value + '')'' as Qsource, s.QualificationDate, s.Subsrc,s.copies ' + @PrintColumns + 
				' From Subscriptions s join #SubscriptionID sf on sf.SubscriptionID = s.SubscriptionID  left outer join QSource q on q.Qsource_ID = s.Qsource_ID join Category C on s.Category_ID = C.Category_ID join Categorygroup cg on cg.Categorygroup_ID = C.Categorygroup_ID join ' +
				' [Transaction] t on s.Transaction_ID = t.Transaction_ID  join state st on  s.state = st.state ' +
				' where magazineID = ' + @MagazineID + ' and C.Categorygroup_ID <> 5 and st.COUNTRY = ''CANADA'' and s.COUNTRY = ''CANADA''')
	end
	drop table #SubscriptionID
end