create proc [dbo].[sp_rptCrossTabwithQualBreakdown]
(
	@ReportID int,
	@Filters TEXT,
	@PrintColumns varchar(4000),
	@Download char(1)      
)
as
Begin 

	set nocount on
	
	declare @Row varchar(50),  
			@PublicationID int,
			@year int,
			@sqlstring varchar(4000),
			@startperiod varchar(10),
			@startdate datetime,
			@endDate datetime,
			@PublicationCode varchar(20)

			
	if len(ltrim(rtrim(@PrintColumns))) > 0 
	Begin
		set @PrintColumns  = ', ' + @PrintColumns 
	end

	create table #SubscriptionID (SubscriptionID int, copies int)  
	
	select	@PublicationID = PublicationID,  
			@Row = Row 
	from   
			PublicationReports  
	Where  
			ReportID = @ReportID
			
	select @PublicationCode  = Publicationcode from Publication where PublicationID = @PublicationID		

	select @startperiod = YearStartDate  from Publication where PublicationID = @PublicationID

	if getdate() > convert(datetime,@startperiod + '/' + convert(varchar,year(getdate())))
		set @year = year(getdate()) 
	else
		set @year = year(getdate()) - 1	
		
	set @startdate = @startperiod + '/' + convert(varchar,@year)		
	select @endDate =  dateadd(ss, -1, dateadd(yy, 1, @startdate) )
	
	print @startdate
	print  @enddate
	
	Insert into #SubscriptionID  
	exec sp_getSubscribers_using_XMLFilters @PublicationID, @Filters, 1
	
			
	-----Justin add
	declare @PublisherID int = (select PublisherID from Publication with(nolock) where PublicationID = @PublicationID)
	declare @ClientID int = (select ClientID from Publisher with(nolock) where PublisherID = @PublisherID)
	declare @ClientName varchar(100) = (select ClientName from uas..Client with(nolock) where ClientID = @ClientID)
	declare @db varchar(200) = (select name from master..sysdatabases where name like @ClientName + 'MasterDB%')
	declare @Sqlstmt varchar(8000)

	--create a temp table for the left outer join below

	create table #deliver(ResponseID int, [print] int,digital int,[Unique] int,QualYear varchar(256))
	
	set @Sqlstmt = 	'insert into #deliver
					select	r.ResponseID, 
							isnull(SUM(case when DeliverabilityCode=''A'' or DeliverabilityCode=''C'' then s1.copies end),0) as ''Print'',
							isnull(SUM(case when DeliverabilityCode=''B'' or DeliverabilityCode=''C'' then s1.copies end),0) as ''Digital'',
							isnull(SUM(case when DeliverabilityCode=''A'' or DeliverabilityCode=''B'' or DeliverabilityCode=''C'' then s1.copies end),0) as ''Unique'',
							Case	when s.QSourceDate between cast('''+cast(@startdate as varchar(50))+''' as datetime) and  cast('''+cast(@endDate as varchar(50))+''' as datetime)  then ''1 yr''
									when s.QSourceDate between DATEADD(yy, -1, cast('''+cast(@startdate as varchar(50))+''' as datetime)) and DATEADD(yy, -1, cast('''+cast(@endDate as varchar(50))+''' as datetime)) then ''2 yr''
									when s.QSourceDate between DATEADD(yy, -2, cast('''+cast(@startdate as varchar(50))+''' as datetime)) and DATEADD(yy, -2, cast('''+cast(@endDate as varchar(50))+''' as datetime)) then ''3 yr''
									when s.QSourceDate between DATEADD(yy, -3, cast('''+cast(@startdate as varchar(50))+''' as datetime)) and DATEADD(yy, -3, cast('''+cast(@endDate as varchar(50))+''' as datetime)) then ''4 yr''
									else ''4+ yr'' end  AS QualYear
					From  
							Subscription s join #SubscriptionID s1 on s.SubscriptionID = s1.SubscriptionID  join 
							'+@db+'..SubscriptionResponseMap sd on sd.SubscriptionID = s.SubscriptionID join  
							vw_Response r on sd.ResponseID = r.ResponseID join
							Deliverability d on d.DeliverabilityID = s.DeliverabilityID
					Where  
							s.PublicationID = '+cast(@PublicationID as varchar(25))+' and 
							r.ResponseName = '''+cast(@Row as varchar(50))+''' and 
							r.PublicationID = '+cast(@PublicationID as varchar(25))+'
					group by  r.ResponseID, 
					Case	when s.QSourceDate between cast('''+cast(@startdate as varchar(50))+''' as datetime) and  cast('''+cast(@endDate as varchar(50))+'''as datetime)  then ''1 yr''
							when s.QSourceDate between DATEADD(yy, -1, cast('''+cast(@startdate as varchar(50))+''' as datetime)) and DATEADD(yy, -1, cast('''+cast(@endDate as varchar(50))+''' as datetime)) then ''2 yr''
							when s.QSourceDate between DATEADD(yy, -2, cast('''+cast(@startdate as varchar(50))+''' as datetime)) and DATEADD(yy, -2, cast('''+cast(@endDate as varchar(50))+''' as datetime)) then ''3 yr''
							when s.QSourceDate between DATEADD(yy, -3, cast('''+cast(@startdate as varchar(50))+''' as datetime)) and DATEADD(yy, -3, cast('''+cast(@endDate as varchar(50))+''' as datetime)) then ''4 yr''
						else ''4+ yr'' end'
						
	--print(@Sqlstmt)	
	exec(@Sqlstmt)
	
	create table #codeSheet(ResponseID int, response_Value varchar(256),Response_Description varchar(256))
	
	set @Sqlstmt = 'Insert into #codeSheet
					select r.CodeSheetID, r.Responsevalue as response_Value,r.Responsedesc as Response_Description 
					from '+@db+'..CodeSheet r join '+@db+'..Pubs p on r.PubID = p.PubID
											  join Circulation..Publication pp on pp.PublicationCode = p.PubCode
					where pp.PublicationID = '+cast(@PublicationID as varchar(50))+' and r.ResponseGroup = '''+@Row+''' '
	exec(@Sqlstmt)	
	

	CREATE UNIQUE CLUSTERED INDEX IX_1 on #SubscriptionID (SubscriptionID)
	
		
		declare @media table (demo7 char(1), DisplayName varchar(10))
		
		insert @media values ('A', 'Print')
		insert @media values ('B', 'Digital')
		insert @media values ('C', 'Unique');
		
		WITH report_CTE (demo7, DisplayName, responseID, response_Value, Response_Description, [Print], [Digital], [Unique], QualYear)
		as
		(
			SELECT inn2.*, inn1.[Print], inn1.[Digital], inn1.[Unique], inn1.QualYear  from
			(
				select * from @media m cross join #codeSheet r1
			) inn2
			left outer join #deliver inn1 on inn1.ResponseID = inn2.ResponseID 
			Where QualYear is not null
		)
		select * from report_CTE
		union
		select demo7, DisplayName, responseID, response_Value, Response_Description, sum([Print]), Sum([Digital]), Sum([Unique]), 'Total' 
		from report_CTE
		group by demo7, DisplayName, responseID, response_Value, Response_Description
		order by response_value, demo7, qualyear

	drop table #codeSheet
	drop table #deliver
	drop table #SubscriptionID 
			
end
