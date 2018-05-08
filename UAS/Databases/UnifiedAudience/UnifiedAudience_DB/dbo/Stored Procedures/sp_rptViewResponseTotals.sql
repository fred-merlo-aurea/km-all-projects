create proc [dbo].[sp_rptViewResponseTotals]    
(    
 @ReportID int,  
 @RowID varchar(1000),
 @Filters TEXT,
 @PrintColumns varchar(4000),
 @Download char(1)       
)    
as 

--declare @ReportID int,  
-- @RowID varchar(1000),
-- @Filters varchar(4000),
-- @PrintColumns varchar(4000),
-- @Download char(1) 
   
--set @ReportID= 1931
--set  @RowID = ''
--set  @Filters = '<Filters></Filters>' --<FilterType ID="P"></FilterType><FilterType ID="D"></FilterType><FilterType ID="M"><FilterGroup Type="CATEGORY"><Value>2</Value><Value>1</Value></FilterGroup><FilterGroup Type="CATCODES"><Value>1</Value><Value>2</Value></FilterGroup><FilterGroup Type="TRANSACTION"><Value>1</Value></FilterGroup></FilterType><FilterType ID="C"></FilterType><FilterType ID="A"></FilterType>
--set  @PrintColumns = ''
--set  @Download = 0
          
Begin   
	
	--select        
	--		0 responseID,  
	--		'' Description, 
	--		0 as 'Totals',
	--		0 as 'UniqueCount',    
	--		0.00 as 'Percent' ,   
	--		0.00 as 'QualifiedPercent',
	--	 '' DisplayName , '' groupUniques, 0 as rgsortorder
	

	declare	@PublicationID int,  
			@ResponseType varchar(25)  ,
			@distinctcount int,    
			@Responded int  

	set nocount on

	if len(ltrim(rtrim(@PrintColumns))) > 0 
		Begin
			set @PrintColumns  = ', ' + @PrintColumns 
		end

	set @distinctcount = 0
	set @Responded = 0

	create table #SubscriptionID (SubscriptionID int, copies int)  
	select @PublicationID = PublicationID, @ResponseType=Row 
	from PublicationReports 
	where reportID = @reportID  
	
	declare @PublicationCode varchar(20)
	select @PublicationCode  = PublicationCode 
	from Publication 
	where PublicationID = @PublicationID
	
	Insert into #SubscriptionID     
	exec sp_getSubscribers_using_XMLFilters @PublicationID, @Filters, 1

	CREATE UNIQUE CLUSTERED INDEX IX_1 on #SubscriptionID (SubscriptionID)
	
	-----Qoua add
	declare @PublisherID int = (select PublisherID from Publication with(nolock) where PublicationID = @PublicationID)
	declare @ClientID int = (select ClientID from Publisher with(nolock) where PublisherID = @PublisherID)
	declare @ClientName varchar(100) = (select ClientName from uas..Client with(nolock) where ClientID = @ClientID)
	declare @db varchar(200) = (select name from master..sysdatabases where name like @ClientName + 'MasterDB%')
	declare @Sqlstmt varchar(8000)
	declare @PubID int
	
	create table #pubid (pubid int)
	exec ('insert into #pubid select pubid from '+@db+'..Pubs with(nolock) where PubCode = '''+@PublicationCode+''' ')
	
	set @PubID = (select top 1 pubid from #pubid)-- Should only contain 1 row
	
	drop table #pubid

	create table #cte (ResponseID int, DESCRIPTION varchar(255), DisplayName varchar(100), SubscriptionID int,  copies int,  rgsortorder int)  
	create table #responseID (responseID int) 	
  
	if Len(ltrim(rtrim(@RowID))) = 0  
		begin
			--insert into #responseID select responseID from response  where PublicationID = @PublicationID and responseName = @ResponseType
			set @Sqlstmt = 'insert into #responseID select CodeSheetID from '+@db+'..CodeSheet where PubID = '+cast(@PubID as varchar(25))+' and responseGroup = '''+@ResponseType+'''  '
			exec(@Sqlstmt)
		end	
	else
		begin  
			insert into #responseID 
			select items 
			from dbo.fn_Split(@RowID,',')   
		end
	
	if @Download  = '0'
		Begin
			select	@distinctcount = sum(copies) 
			FROM  #SubscriptionID sf   
		
			create table #srm (subId int)
		
			set @Sqlstmt = 'insert into #srm
							select distinct sd.SubscriptionID FROM '+@db+'.dbo.SubscriptionResponseMap sd where CodeSheetID in (select responseID from #responseID)'

			delete from #SubscriptionID 
			from #SubscriptionID s 
				left outer join (select subid as SubscriptionID 
								from #srm 
								--select distinct sd.SubscriptionID FROM dbo.SubscriptionResponseMap sd where CodeSheetID in (select responseID from #responseID)
				) inn on s.SubscriptionID = inn.SubscriptionID
			where inn.subscriptionID is null
		
			drop table #srm
    
			select	@Responded = isnull(sum(copies), 0) FROM #SubscriptionID;

			--insert into #cte
			--  	select     
			--		sd.responseID,  
			--		r.DisplayName AS 'DESCRIPTION',     
			--		isnull(rg.DisplayName,'') as reportgroupname,
			--		sf.SubscriptionID,   
			--		sf.copies,
			--		rg.DisplayOrder		
			--	From    
			--		#SubscriptionID sf  join dbo.SubscriptionResponseMap sd on sf.SubscriptionID = sd.SubscriptionID   join
			--		Response  r on sd.responseID = r.responseID join 
			--		#responseID rID on rID.responseID = r.ResponseID LEFT OUTER JOIN 
			--		ReportGroups rg ON r.ReportGroupID = rg.ReportGroupID 
				  
			--	WHERE       
			--		r.ResponseName = @ResponseType
		

			set @Sqlstmt = 'insert into #cte
    						select     
								sd.responseID,  
								r.Responsedesc AS ''DESCRIPTION'',     
								isnull(rg.DisplayName,'''') as reportgroupname,
								sf.SubscriptionID,   
								sf.copies,
								rg.DisplayOrder		
							From    
								#SubscriptionID sf  join '+@db+'..SubscriptionResponseMap sd on sf.SubscriptionID = sd.SubscriptionID   join
								'+@db+'..CodeSheet  r on sd.responseID = r.CodeSheetID join 
								#responseID rID on rID.responseID = r.CodeSheetID LEFT OUTER JOIN 
								ReportGroups rg ON r.ReportGroupID = rg.ReportGroupID 
							  
							WHERE       
								r.ResponseGroup = '''+@ResponseType+''''
			exec(@Sqlstmt)		
		
			select ResponseID, DESCRIPTION, SUM(copies) as Totals, @Responded as 'UniqueCount', 
				case when isnull(@distinctcount,0) = 0 then 0 else convert(decimal(18,2),(convert(decimal(18,2),count(subscriptionID))*100)/@distinctcount) end as 'Percent' ,   
				case when isnull(@Responded,0) = 0  then 0 else convert(decimal(18,2),(convert(decimal(18,2),count(subscriptionID))*100)/@Responded) end as 'QualifiedPercent',
			cte.DisplayName as DisplayName, groupUniques, rgsortorder
			from #cte cte  
				join (select DisplayName, count(distinct subscriptionID) as groupUniques from #cte ct2 group by DisplayName) as inn2 on cte.DisplayName  = inn2.displayname
			group by ResponseID, DESCRIPTION, cte.DisplayName, groupUniques, rgsortorder
			union    
				select 0 as responseID, 'ZZ. NO RESPONSE' , @distinctcount - @Responded , 0  , --0,
				case when isnull(@distinctcount,0) = 0 then 0 else convert(decimal(18,2),(convert(decimal(18,2),(@distinctcount - @Responded))*100)/@distinctcount) end as 'Percent',  
				0, NULL, 0, 1000 --case when isnull(@distinctcount,0) = 0 then 0 else convert(decimal(18,2),(convert(decimal(18,2),(@distinctcount - @Responded))*100)/@Responded) end as 'QualifiedPercent'
				order by rgsortorder

		end
	else
		Begin
			exec ('select ''' + @PublicationCode + ''' as PubCode,  s.SubscriberID, s.EMAILADDRESS, s.FNAME, s.LNAME, s.COMPANY, s.TITLE, s.ADDRESS, s.MAILSTOP, s.CITY, s.STATE, 
				   s.ZIP, s.PLUS4, s.COUNTRY, s.PHONE, s.FAX, s.website, s.Category_ID as CAT, c.Category_Name as CategoryName, s.Transaction_ID as XACT, s.Transaction_Date as XACTDate,
				   s.FORZIP, s.COUNTY, s.QSource_ID, q.Qsource_Name + '' (''+ q.Qsource_value + '')'' as Qsource, s.QualificationDate, s.Subsrc ' + @PrintColumns +
				 ' From Subscriptions S join Category C on s.Category_ID = C.Category_ID left outer join QSource q on q.Qsource_ID = s.Qsource_ID where S.SubscriptionID in ' +
				 ' (Select distinct sf.subscriptionID From #SubscriptionID sf join SubscriptionDetails sd on sf.subscriptionID = sd.subscriptionID 
					and responseID in (select responseID from #responseID)) ')
				
		end   
	 
	drop table #SubscriptionID 	
	drop table #responseID 	
	drop table #cte
	
End