CREATE proc [dbo].[sp_rptPAR3C]    
(    
 @ReportID int,  
 @ResponseID int,
 @Filters TEXT,
 @PrintColumns varchar(4000),
 @Download char(1)       
)    
as     
Begin    

	set nocount on


	declare	@publicationID int,
			@PublicationCode varchar(20),
			@count int

	if len(ltrim(rtrim(@PrintColumns))) > 0 
	Begin
		set @PrintColumns  = ', ' + @PrintColumns 
	end

	set @count = 0

	create table #SubscriptionID (SubscriptionID int, copies int) 
	 
	select @publicationID = publicationID  from PublicationReports where reportID = @reportID  
	
	select @PublicationCode  = Publicationcode from Publication where PublicationID = @PublicationID
	
	Insert into #SubscriptionID   
	 exec sp_getSubscribers_using_XMLFilters @publicationID, @Filters, 1

	CREATE UNIQUE CLUSTERED INDEX IX_1 on #SubscriptionID (SubscriptionID)

	if @Download  = '0'
	Begin
		select     
			pc.Par3CID as responseID,   
			pc.DisplayName as responsevalue,
			isnull(SUM(case when cct.CategoryCodeTypeName = 'Qualified Free' then s.COPIES end),0) as Qualified_non_paid,        
			isnull(SUM(case when cct.CategoryCodeTypeName = 'Qualified Paid' then s.COPIES end),0) as Qualified_paid
		From    
			Subscription s join
			#SubscriptionID sf on sf.SubscriptionID = s.SubscriptionID  join 
			Action a on a.ActionID = s.ActionID_Current join
			CategoryCode cc on cc.CategoryCodeID  = a.CategoryCodeID join
			CategoryCodeType cct on cct.CategoryCodeTypeID = cc.CategoryCodeTypeID right outer join
			Par3c pc on pc.Par3CID = s.Par3cID
		group by       
			pc.Par3CID,  
			pc.DisplayName
		order by pc.DisplayName
	end
	else
	Begin
		declare @query varchar(2000)
		set @query =  'select ''' + @PublicationCode + ''' as PubCode, s.SubscriberID, s.EMAILADDRESS, s.FNAME, s.LNAME, s.COMPANY, s.TITLE, s.ADDRESS, s.MAILSTOP, s.CITY, s.STATE, s.ZIP, s.PLUS4, s.COUNTRY, s.PHONE, s.FAX, s.website, s.Category_ID as CAT, c.Category_Name as CategoryName, s.Transaction_ID as XACT, s.Transaction_Date as XACTDate, s.FORZIP, s.COUNTY, s.QSource_ID, q.Qsource_Name + '' (''+ q.Qsource_value + '')'' as Qsource, s.QualificationDate, s.PAR3C, s.Subsrc, S.COPIES ' + @PrintColumns +
		' From #SubscriptionID sf join Subscriptions s on sf.SubscriptionID = s.SubscriptionID join #responseID r on s.PAR3C = r.responseID join
			Category c on c.Category_ID = s.Category_ID join CategoryGroup cg on cg.CategoryGroup_ID = c.CategoryGroup_ID left outer join QSource q on q.Qsource_ID = s.Qsource_ID 
		where cg.CategoryGroup_ID in (1,2)'
		
		if @ResponseID <> 0
			set @query = @query + ' and r.responseID = ' + Convert(varchar,@ResponseID)
			
		exec (@query)
		
	end    
	drop table #SubscriptionID 
End