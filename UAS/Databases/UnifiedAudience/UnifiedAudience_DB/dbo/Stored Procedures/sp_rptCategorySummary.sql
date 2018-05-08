CREATE proc [dbo].[sp_rptCategorySummary]
(
	@MagazineID int,  
	@Filters TEXT,
	@PrintColumns varchar(4000),
	@Download char(1)          
)
as
BEGIN

	SET NOCOUNT ON
	
	declare @publicationID int
	
	set @publicationID = @MagazineID
	
	declare @PublicationCode varchar(20)
	select @PublicationCode  = Publicationcode 
	from Publication 
	where PublicationID = @PublicationID
	

	if len(ltrim(rtrim(@PrintColumns))) > 0 
		Begin
			set @PrintColumns  = ', ' + @PrintColumns 
		end

	create table #SubscriptionID (SubscriptionID int, copies int)   
	
	Insert into #SubscriptionID   
	exec sp_getSubscribers_using_XMLFilters @PublicationID, @Filters, 1 
	
	CREATE UNIQUE CLUSTERED INDEX IX_1 on #SubscriptionID (SubscriptionID)
	
	declare @cat table
		(
		CategoryCodeTypeID int,
		CategoryCodeID int,
		CategoryCodeTypeName varchar(100),
		CategoryCodeName varchar(100),
		CategoryCodeValue int
		)
	
		insert into @cat
		select	distinct cct.CategoryCodeTypeID, 
				cc.CategoryCodeID, 
				cct.CategoryCodeTypeName, 
				cc.CategoryCodeName,
				CategoryCodeValue
		from CategoryCodeType cct 
			join CategoryCode cc on cc.CategoryCodeTypeID = cct.CategoryCodeTypeID 
				
	if @Download = 0
		Begin
			declare @sub table
			(
				scount int,
				CategoryCodeID int
			)
	
			insert into @sub
			select sum(s.copies), a.CategoryCodeID
			from subscription  s 
				join #SubscriptionID sf on sf.SubscriptionID = s.SubscriptionID 
				join Action a on a.ActionID = s.ActionID_Current
			where PublicationID = @PublicationID
			group by a.CategoryCodeID
	
			select c.CategoryCodeTypeID categorygroup_ID,
				c.CategoryCodeValue category_ID,
				c.CategoryCodeTypeName categorygroup_name,
				c.CategoryCodeName category_name,
				isnull(sum(scount),0) as total
			from @cat c 
				left outer join @sub s on c.CategoryCodeID = s.CategoryCodeID 
			group by c.CategoryCodeTypeID, c.CategoryCodeID,c.CategoryCodeValue, c.CategoryCodeTypeName, c.CategoryCodeName
			--union
			--select 5, 73, 'INACTIVE RECORDS', 'Deleted Records on File',  sum(COPIES) 
			--from	subscription s join
			--		Action a on a.ActionID = s.ActionID_Current join
			--		TransactionCode tc on tc.TransactionCodeID = a.TransactionCodeID
			--where 
			--		PublicationID = @PublicationID and ((tc.TransactionCodeValue >= 30 and tc.TransactionCodeValue <40) or (tc.TransactionCodeValue >= 60 and tc.TransactionCodeValue <70))
			--order by a.categorygroup_ID, c.category_ID, categorygroup_name, category_name 
		end
	else
		Begin
			exec ('select  distinct ''' + @PublicationCode + ''' as PubCode, s.SubscriberID, s.EMAILADDRESS, s.FNAME, s.LNAME, ' +
				' s.COMPANY, s.TITLE, s.ADDRESS, s.MAILSTOP, s.CITY, s.STATE, s.ZIP, s.PLUS4, s.COUNTRY, s.PHONE, s.FAX, s.website, s.Category_ID as CAT, c.Category_Name as CategoryName, s.Transaction_ID as XACT, s.Transaction_Date as XACTDate, s.FORZIP, s.COUNTY, s.QSource_ID, q.Qsource_Name + '' (''+ q.Qsource_value + '')'' as Qsource, s.QualificationDate, s.Subsrc, s.copies ' +
				@PrintColumns + 
			' from subscriptions  s join #SubscriptionID sf on sf.SubscriptionID = s.SubscriptionID join Category C on s.Category_ID = C.Category_ID left outer join QSource q on q.Qsource_ID = s.Qsource_ID where PublicationID = ' + @PublicationID )
		end
	drop table #SubscriptionID

end