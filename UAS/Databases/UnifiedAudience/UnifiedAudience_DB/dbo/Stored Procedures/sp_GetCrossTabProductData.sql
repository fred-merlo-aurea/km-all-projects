CREATE PROCEDURE [dbo].[sp_GetCrossTabProductData]
@Queries XML,
@Row TEXT, 
@Column TEXT,  
@PubID int
AS
BEGIN
	
	SET NOCOUNT ON

	Create table #tblrow  (RowDesc varchar(255), RowValue varchar(100) , subscriptionID int)
	Create table #tblcol  (colDesc varchar(255), colValue varchar(100) , subscriptionID int)
	
	Create table #tblrowvalues  (value varchar(255))
	Create table #tblcolvalues  (value varchar(255))
	
	declare @docHandle int,
		@rowtype varchar(100),
		@rowresponsegroupID varchar(10),
		@rowfield varchar(100),
		@rowfilters varchar(100),
		@coltype varchar(100),
		@colresponsegroupID varchar(10),
		@colfield varchar(100),
		@colfilters varchar(100),
		@zipFrom varchar(50),
		@zipTo varchar(50)
			
	create table #subscriptionIDs (SubscriptionID int primary key)                     

	declare @hDoc AS INT
	declare @filterno int, @qry varchar(max),
			@linenumber int, @SelectedFilterNo varchar(800), @SelectedFilterOperation varchar(20), @SuppressedFilterNo varchar(800), @SuppressedFilterOperation varchar(20) 

	create table #tblquery  (filterno int, Query varchar(max))
	create table #tblqueryresults  (filterno int, subscriptionID int, primary key (filterno, subscriptionID))

	EXEC sp_xml_preparedocument @hDoc OUTPUT, @queries

	insert into #tblquery
	SELECT filterno, Query
	FROM OPENXML(@hDoc, 'xml/Queries/Query')
	WITH 
	(
	filterno int '@filterno',
	Query [varchar](max) '.'
	)
	
	SELECT 
			@linenumber = linenumber, 
			@SelectedFilterNo = selectedfilterno, 
			@SelectedFilterOperation = selectedfilteroperation, 
			@SuppressedFilterNo = suppressedfilterno, 
			@SuppressedFilterOperation = suppressedfilteroperation
	FROM OPENXML(@hDoc, 'xml/Results/Result')
	WITH 
	(
		linenumber int							'@linenumber',
		selectedfilterno varchar(800)			'@selectedfilterno',
		selectedfilteroperation varchar(20)		'@selectedfilteroperation',
		suppressedfilterno varchar(800)			'@suppressedfilterno',
		suppressedfilteroperation varchar(20)	'@suppressedfilteroperation'
	)

	EXEC sp_xml_removedocument @hDoc
	
	DECLARE c_queries CURSOR 
	FOR select filterno from #tblquery
		
	OPEN c_queries  
	FETCH NEXT FROM c_queries INTO @filterno

	WHILE @@FETCH_STATUS = 0  
	BEGIN  
		select @qry =  query from #tblquery with (NOLOCK) where filterno = @filterno
		
		insert into #tblqueryresults  
		execute (@qry)
		
		FETCH NEXT FROM c_queries INTO @filterno
	END
	
	CLOSE c_queries  
	DEALLOCATE c_queries 

	select @qry = 'select distinct subscriptionID from ((select subscriptionID from #tblqueryresults t where filterno = ' + REPLACE(@SelectedFilterNo, ',', '  ' + @SelectedFilterOperation + ' select subscriptionID from #tblqueryresults t where filterno = ') + ') ' + 
	case when len(@suppressedfilterno) > 0 then 
		' except ' 
		+ ' (select subscriptionID from #tblqueryresults t where filterno = ' + REPLACE(@SuppressedfilterNo , ',', '  ' + @SuppressedFilterOperation + ' select subscriptionID from #tblqueryresults t where filterno = ') + ') '
	else
	'' end
	+ ') x'

	insert into #subscriptionIDs 
	execute (@qry)
	
	drop table #tblquery
	drop table #tblqueryresults; 
	
	EXEC sp_xml_preparedocument @docHandle OUTPUT, @Row  

	select @rowtype = RowType,
		@rowresponsegroupID = RowResponseGroupID,
		@rowfield = RowField,
		@rowfilters = RowFilters
	FROM OPENXML(@docHandle, N'/XML', 2)   
	WITH   
	(  
		RowType varchar(100), RowResponseGroupID varchar(10), RowField varchar(100), RowFilters varchar(100)
	)  
 
	EXEC sp_xml_removedocument @docHandle 
	
	EXEC sp_xml_preparedocument @docHandle OUTPUT, @Column  

	select @coltype = ColType,
		@colresponsegroupID = ColResponseGroupID,
		@colfield = ColField,
		@colfilters = ColFilters
	FROM OPENXML(@docHandle, N'/XML', 2)   
	WITH   
	(  
		ColType varchar(100), ColResponseGroupID varchar(10), ColField varchar(100), ColFilters varchar(100)
	)  

	EXEC sp_xml_removedocument @docHandle 

	if @rowtype = 'Profile' 
		Begin
			if @rowfield = 'COMPANY' OR  @rowfield = 'TITLE' or @rowfield = 'CITY'
				Begin
					if @rowfilters <> ''
						Begin
							insert into #tblrowvalues 
							select items from dbo.fn_Split(@rowfilters , ',')
				
							exec('insert into #tblrow
							select ps.' + @rowfield + ', ps.' + @rowfield + ', ps.SubscriptionID
							from
								#subscriptionIDs ts 
								join PubSubscriptions ps  with (nolock) on ps.subscriptionid = ts.subscriptionid and ps.PubID = ' + @PubID + '  
								join
								( 
									select TOP 100 ps.' + @rowfield + ' 
											from 
									#subscriptionIDs ts 
									join PubSubscriptions ps  with (nolock) on ps.subscriptionid = ts.subscriptionid and ps.PubID = ' + @PubID + '  
									join #tblrowvalues trv on ps.' + @rowfield + ' like ''%'' + trv.value + ''%''
									group by ps.' + @rowfield + '
									order by count(ps.SubscriptionID) desc
								) x on x.' + @rowfield + ' = ps.' + @rowfield)
						End
					Else
						Begin
							exec('insert into #tblrow
							select ps.' + @rowfield + ', ps.' + @rowfield + ', ps.SubscriptionID
							from
								#subscriptionIDs ts 
								join PubSubscriptions ps  with (nolock) on ps.subscriptionid = ts.subscriptionid and ps.PubID = ' + @PubID + '  
								join
								( 
									select TOP 100 ps.' + @rowfield + ' 
											from 
									#subscriptionIDs ts 
									join PubSubscriptions ps  with (nolock) on ps.subscriptionid = ts.subscriptionid and ps.PubID = ' + @PubID + '  
									group by ps.' + @rowfield + '
									order by count(ps.SubscriptionID) desc
								) x on x.' + @rowfield + ' = ps.' + @rowfield)
						End
				End
			Else if @rowfield = 'STATE'
				Begin
					insert into #tblrow
					select ps.RegionCode, ps.RegionCode, ps.SubscriptionID
					from #subscriptionIDs ts 
						join PubSubscriptions ps  with (nolock) on ps.subscriptionid = ts.subscriptionid  
						join UAD_Lookup..Country ct with (nolock) on ct.CountryID = ps.CountryID
					where ps.PubID = @PubID and
						 (ct.ShortName = 'CANADA' or ct.ShortName = 'UNITED STATES')
				End			
			Else if @rowfield = 'ZIP'
				Begin

					SELECT @zipFrom = items FROM (SELECT ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS RowNum, * FROM dbo.fn_Split(@rowfilters , '|')) sub  WHERE RowNum = 1
					SELECT @zipTo = items FROM (SELECT ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS RowNum, * FROM dbo.fn_Split(@rowfilters , '|')) sub  WHERE RowNum = 2
			
					insert into #tblrow
					select ps.ZipCode, ps.ZipCode, ps.SubscriptionID
					from #subscriptionIDs ts 
						join PubSubscriptions ps  with (nolock) on ps.subscriptionid = ts.subscriptionid
					where ps.PubID = @PubID and	
						substring(ps.ZipCode,1,5) between @zipFrom and @zipTo
				End			
			Else if @rowfield = 'COUNTRY'
				Begin
					insert into #tblrow
					select ct.ShortName, ct.ShortName, ps.SubscriptionID
					from #subscriptionIDs ts 
						join PubSubscriptions ps  with (nolock) on ps.subscriptionid = ts.subscriptionid  
						join UAD_Lookup..Country ct with (nolock) on ct.CountryID = ps.CountryID
					where ps.PubID = @PubID 
				End
		End
	
	if @coltype = 'Profile' 
		Begin
			if @colfield = 'COMPANY' OR  @colfield = 'TITLE' or @colfield = 'CITY'
				Begin
					if @colfilters <> ''
						Begin
							insert into #tblcolvalues 
							select items from dbo.fn_Split(@colfilters , ',')
				
							exec('insert into #tblcol
							select ps.' + @colfield + ', ps.' + @colfield + ', ps.SubscriptionID
							from
								#subscriptionIDs ts 
								join PubSubscriptions ps  with (nolock) on ps.subscriptionid = ts.subscriptionid and ps.PubID = ' + @PubID + ' 
								join
								( 
									select TOP 100 ps.' + @colfield + ' 
											from 
									#subscriptionIDs ts 
									join PubSubscriptions ps  with (nolock) on ps.subscriptionid = ts.subscriptionid and ps.PubID = ' + @PubID + '
									join #tblcolvalues trv on ps.' + @colfield + ' like ''%'' + trv.value + ''%''
									group by ps.' + @colfield + '
									order by count(ps.SubscriptionID) desc
								) x on x.' + @colfield + ' = ps.' + @colfield)
						End
					Else
						Begin
							exec('insert into #tblcol
							select ps.' + @colfield + ', ps.' + @colfield + ', ps.SubscriptionID
							from
								#subscriptionIDs ts 
								join PubSubscriptions ps  with (nolock) on ps.subscriptionid = ts.subscriptionid and ps.PubID = ' + @PubID + '  
								join
								( 
									select TOP 100 ps.' + @colfield + ' 
											from 
									#subscriptionIDs ts 
									join PubSubscriptions ps  with (nolock) on ps.subscriptionid = ts.subscriptionid and ps.PubID = ' + @PubID + '  
									group by ps.' + @colfield + '
									order by count(ps.SubscriptionID) desc
								) x on x.' + @colfield + ' = ps.' + @colfield)
						End
				End
			Else if @colfield = 'STATE'
				Begin
					insert into #tblcol
					select ps.RegionCode, ps.RegionCode, ps.SubscriptionID
					from #subscriptionIDs ts 
						join PubSubscriptions ps  with (nolock) on ps.subscriptionid = ts.subscriptionid  
						join UAD_Lookup..Country ct with (nolock) on ct.CountryID = ps.CountryID
					where ps.PubID = @PubID and
						 (ct.ShortName = 'CANADA' or ct.ShortName = 'UNITED STATES')					
				End			
			Else if @colfield = 'ZIP'
				Begin

					SELECT @zipFrom = items FROM (SELECT ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS colNum, * 
					FROM dbo.fn_Split(@colfilters , '|')) sub  
					WHERE colNum = 1

					SELECT @zipTo = items FROM (SELECT ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS colNum, * 
					FROM dbo.fn_Split(@colfilters , '|')) sub  
					WHERE colNum = 2
			
					insert into #tblcol
					select ps.ZipCode, ps.ZipCode, ps.SubscriptionID
					from #subscriptionIDs ts 
						join PubSubscriptions ps  with (nolock) on ps.subscriptionid = ts.subscriptionid
					where ps.PubID = @PubID and	
						substring(ps.ZipCode,1,5) between @zipFrom and @zipTo			
				End			
			Else if @colfield = 'COUNTRY'
				Begin
					insert into #tblcol
					select ct.ShortName, ct.ShortName, ps.SubscriptionID
					from #subscriptionIDs ts 
						join PubSubscriptions ps  with (nolock) on ps.subscriptionid = ts.subscriptionid
  						join UAD_Lookup..Country ct with (nolock) on ct.CountryID = ps.CountryID
  					where ps.PubID = @PubID
				End
		End

	if @rowtype = 'ResponseGroup' 
		Begin
			if @rowfilters <> ''
			Begin
				insert into #tblrowvalues 
				select items from dbo.fn_Split(@rowfilters , ',')	
					
				insert into #tblrow
				select distinct c.ResponseDesc, c.ResponseValue, ts.SubscriptionID
				from  #subscriptionIDs ts 
					join pubsubscriptiondetail psd  with(nolock) on psd.subscriptionid = ts.subscriptionid 
					join codesheet c  with(nolock) on c.codesheetID = psd.codesheetID 
					join #tblrowvalues trv on c.Responsedesc like '%' + trv.value + '%'
				where c.ResponseGroupID =  @rowresponsegroupID
			End
			Else
			Begin
				insert into #tblrow
				select distinct c.ResponseDesc, c.ResponseValue, ts.SubscriptionID
				from  #subscriptionIDs ts 
					join pubsubscriptiondetail psd  with(nolock) on psd.subscriptionid = ts.subscriptionid 
					join codesheet c  with(nolock) on c.codesheetID = psd.codesheetID 
				where c.ResponseGroupID =  @rowresponsegroupID			
			End
		End
	
	if @coltype = 'ResponseGroup' 
		Begin
			if @colfilters <> ''
			Begin
				insert into #tblcolvalues 
				select items from dbo.fn_Split(@colfilters , ',')		

				insert into #tblcol
				select distinct c.ResponseDesc, c.ResponseValue, ts.SubscriptionID
				from #subscriptionIDs ts 
					join pubsubscriptiondetail psd  with (nolock) on psd.subscriptionid = ts.subscriptionid  
					join codesheet c  with(nolock) on c.codesheetID = psd.codesheetID 
					join #tblcolvalues trv on c.Responsedesc like '%' + trv.value + '%'
				where c.ResponseGroupID =  @colresponsegroupID
			End
			Else
			Begin
				insert into #tblcol
				select distinct c.ResponseDesc, c.ResponseValue, ts.SubscriptionID
				from #subscriptionIDs ts 
					join pubsubscriptiondetail psd  with (nolock) on psd.subscriptionid = ts.subscriptionid  
					join codesheet c  with(nolock) on c.codesheetID = psd.codesheetID 
				where c.ResponseGroupID =  @colresponsegroupID				
			End
		End

	drop table #subscriptionIDs
	
	select 
		case when isnull(tr.RowDesc, '')  = '' then 'ZZZ. NO RESPONSE' else Upper(tr.RowDesc)  end as Rowdesc, 
		case when isnull(tr.RowValue, '') = '' then 'ZZZ. NO RESPONSE' else Upper(tr.RowValue) end as RowValue, 
		case when isnull(tc.colDesc, '')  = '' then 'ZZZ. NO RESPONSE' else Upper(tc.colDesc)  end as colDesc, 
		case when isnull(tc.colValue, '') = '' then 'ZZZ. NO RESPONSE' else Upper(tc.colValue) end as colValue, 
		count(*) as counts
	from #tblrow tr  
		join --full outer 
			#tblcol tc on tr.subscriptionID = tc.subscriptionID
	group by 
		case when isnull(tr.RowDesc, '')  = '' then 'ZZZ. NO RESPONSE' else Upper(tr.RowDesc)  end, 
		case when isnull(tr.RowValue, '') = '' then 'ZZZ. NO RESPONSE' else Upper(tr.RowValue) end, 
		case when isnull(tc.colDesc, '')  = '' then 'ZZZ. NO RESPONSE' else Upper(tc.colDesc)  end, 
		case when isnull(tc.colValue, '') = '' then 'ZZZ. NO RESPONSE' else Upper(tc.colValue) end
		
	drop table #tblrow
	drop table #tblcol
	
end