CREATE proc [dbo].[sp_rpt_CategorySummary]
(
	@PublicationID int,
	@CategoryIDs varchar(800),
	@CategoryCodes varchar(800),
	@TransactionIDs varchar(800),
	@TransactionCodes varchar(800),
	@QsourceIDs varchar(800),
	@StateIDs varchar(800),
	@Regions varchar(max),
	@CountryIDs varchar(800),
	@Email varchar(10),
	@Phone varchar(10),
	@Fax varchar(10),
	@ResponseIDs varchar(800),
	@Demo7 varchar(10),		
	@Year varchar(20),
	@startDate varchar(10),		
	@endDate varchar(10),
	@AdHocXML varchar(8000), 
	@PrintColumns varchar(4000),   
	@Download bit     
)
as
BEGIN

	SET NOCOUNT ON

	declare @pubID int	
	set @pubID = @PublicationID
	declare @PublicationCode varchar(20)
	select @PublicationCode  = PubCode from Pubs where PubID = @pubID
	declare @GetSubscriberIDs bit = 0
	
	if len(ltrim(rtrim(@PrintColumns))) > 0 
	Begin
		set @PrintColumns  = ', ' + @PrintColumns 
	end

	create table #SubscriptionID (SubscriptionID int)   
	
	Insert into #SubscriptionID   
	exec sp_rpt_GetSubscriptionIDs @PublicationID, @CategoryIDs,
	@CategoryCodes,
	@TransactionIDs,
	@TransactionCodes,
	@QsourceIDs,
	@StateIDs,
	@Regions,
	@CountryIDs,
	@Email,
	@Phone,
	@Fax,
	@ResponseIDs,
	@Demo7,		
	@Year,
	@startDate,		
	@endDate,
	@AdHocXML,
	@GetSubscriberIDs
	
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
		from Circulation..CategoryCodeType cct 
			join Circulation..CategoryCode cc on cc.CategoryCodeTypeID = cct.CategoryCodeTypeID 
				
	if @Download = 0
		Begin
			declare @sub table
			(
				scount int,
				CategoryCodeID int
			)
		
			insert into @sub
			select sum(s.copies), a.CategoryCodeID
			from Circulation..Subscription  s 
				join #SubscriptionID sf on sf.SubscriptionID = s.SubscriptionID 
				join Circulation..Action a on a.ActionID = s.ActionID_Current
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