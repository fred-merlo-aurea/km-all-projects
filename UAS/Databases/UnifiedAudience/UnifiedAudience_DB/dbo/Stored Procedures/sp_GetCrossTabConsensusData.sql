CREATE PROCEDURE [dbo].[sp_GetCrossTabConsensusData]
@Queries XML,
@Row TEXT, 
@Column TEXT,  
@BrandID int = null, 
@IsRecencyView bit = false
AS
BEGIN
	
	SET NOCOUNT ON
	
	Create table #tblrow  (RowDesc varchar(255), RowValue varchar(100) , subscriptionID int)
	Create table #tblcol  (colDesc varchar(255), colValue varchar(100) , subscriptionID int)
	
	Create table #tblrowvalues  (value varchar(255))
	Create table #tblcolvalues  (value varchar(255))
	
	declare @docHandle int,
		@MasterGroupColumn varchar(100),
		@rowtype varchar(100),
		@rowmastergroupID varchar(10),
		@rowfield varchar(100),
		@rowfilters varchar(100),
		@coltype varchar(100),
		@colmastergroupID varchar(10),
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
	drop table #tblqueryresults  

	EXEC sp_xml_preparedocument @docHandle OUTPUT, @Row  

	select @rowtype = RowType,
		@rowmastergroupID = RowMasterGroupID,
		@rowfield = RowField,
		@rowfilters = RowFilters
	FROM OPENXML(@docHandle, N'/XML', 2)   
	WITH   
	(  
		RowType varchar(100), RowMasterGroupID varchar(10), RowField varchar(100), RowFilters varchar(100)
	)  
 
	EXEC sp_xml_removedocument @docHandle 
	
	EXEC sp_xml_preparedocument @docHandle OUTPUT, @Column  

	select @coltype = ColType,
		@colmastergroupID = ColMasterGroupID,
		@colfield = ColField,
		@colfilters = ColFilters
	FROM OPENXML(@docHandle, N'/XML', 2)   
	WITH   
	(  
		ColType varchar(100), ColMasterGroupID varchar(10), ColField varchar(100), ColFilters varchar(100)
	)  

	EXEC sp_xml_removedocument @docHandle 

	if @rowtype = 'Profile' 
		Begin
			if @rowfield = 'COMPANY' OR  @rowfield = 'TITLE' or @rowfield = 'CITY'
				Begin
					if @rowfilters <> ''
						Begin
							insert into #tblrowvalues 
							select items 
							from dbo.fn_Split(@rowfilters , ',')
				
							exec('insert into #tblrow
							select s.' + @rowfield + ', s.' + @rowfield + ', s.SubscriptionID
							from
								#subscriptionIDs ts 
								join Subscriptions s  with (nolock) on s.subscriptionid = ts.subscriptionid 
								join
								( 
									select TOP 100 s.' + @rowfield + ' 
											from 
									#subscriptionIDs ts 
									join Subscriptions s  with (nolock) on s.subscriptionid = ts.subscriptionid 
									join #tblrowvalues trv on s.' + @rowfield + ' like ''%'' + trv.value + ''%''
									group by s.' + @rowfield + '
									order by count(s.SubscriptionID) desc
								) x on x.' + @rowfield + ' = s.' + @rowfield)
						End
					Else
						Begin
							exec('insert into #tblrow
							select s.' + @rowfield + ', s.' + @rowfield + ', s.SubscriptionID
							from
								#subscriptionIDs ts 
								join Subscriptions s  with (nolock) on s.subscriptionid = ts.subscriptionid 
								join
								( 
									select TOP 100 s.' + @rowfield + ' 
											from 
									#subscriptionIDs ts 
									join Subscriptions s  with (nolock) on s.subscriptionid = ts.subscriptionid  
									group by s.' + @rowfield + '
									order by count(s.SubscriptionID) desc
								) x on x.' + @rowfield + ' = s.' + @rowfield)
						End			
				End
			Else if @rowfield = 'STATE'
				Begin
					insert into #tblrow
					select s.STATE, s.STATE, s.SubscriptionID
					from #subscriptionIDs ts 
						join Subscriptions s  with (nolock) on s.subscriptionid = ts.subscriptionid 
						join UAD_Lookup..Country ct with (nolock) on ct.CountryID = s.CountryID
					where ct.ShortName = 'CANADA' or ct.ShortName = 'UNITED STATES'
				End			
			Else if @rowfield = 'ZIP'
				Begin

					SELECT @zipFrom = items 
					FROM (SELECT ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS RowNum, * FROM dbo.fn_Split(@rowfilters , '|')) sub  
					WHERE RowNum = 1

					SELECT @zipTo = items 
					FROM (SELECT ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS RowNum, * FROM dbo.fn_Split(@rowfilters , '|')) sub  
					WHERE RowNum = 2
			
					insert into #tblrow
					select s.ZIP, s.ZIP, s.SubscriptionID
					from #subscriptionIDs ts 
						join Subscriptions s  with (nolock) on s.subscriptionid = ts.subscriptionid 
					where substring(s.zip,1,5) between @zipFrom and @zipTo 
				End	
			Else if @rowfield = 'COUNTRY'
				Begin
					insert into #tblrow
					select ct.ShortName, ct.ShortName, s.SubscriptionID
					from #subscriptionIDs ts 
						join Subscriptions s  with (nolock) on s.subscriptionid = ts.subscriptionid 
						join UAD_Lookup..Country ct with (nolock) on ct.CountryID = s.CountryID
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
							select s.' + @colfield + ', s.' + @colfield + ', s.SubscriptionID
							from
								#subscriptionIDs ts 
								join Subscriptions s  with (nolock) on s.subscriptionid = ts.subscriptionid 
								join
								( 
									select TOP 100 s.' + @colfield + ' 
											from 
									#subscriptionIDs ts 
									join Subscriptions s  with (nolock) on s.subscriptionid = ts.subscriptionid 
									join #tblcolvalues trv on s.' + @colfield + ' like ''%'' + trv.value + ''%''
									group by s.' + @colfield + '
									order by count(s.SubscriptionID) desc
								) x on x.' + @colfield + ' = s.' + @colfield)
						End
					Else
						Begin
							exec('insert into #tblcol
							select s.' + @colfield + ', s.' + @colfield + ', s.SubscriptionID
							from
								#subscriptionIDs ts 
								join Subscriptions s  with (nolock) on s.subscriptionid = ts.subscriptionid 
								join
								( 
									select TOP 100 s.' + @colfield + ' 
											from 
									#subscriptionIDs ts 
									join Subscriptions s  with (nolock) on s.subscriptionid = ts.subscriptionid  
									group by s.' + @colfield + '
									order by count(s.SubscriptionID) desc
								) x on x.' + @colfield + ' = s.' + @colfield)
						End
				End
			Else if @colfield = 'STATE'
				Begin
					insert into #tblcol
					select s.State, s.State, s.SubscriptionID
					from #subscriptionIDs ts 
						join Subscriptions s  with (nolock) on s.subscriptionid = ts.subscriptionid 
						join UAD_Lookup..Country ct with (nolock) on ct.CountryID = s.CountryID
					where ct.ShortName = 'CANADA' or ct.ShortName = 'UNITED STATES'					
				End			
			Else if @colfield = 'ZIP'
				Begin
		
					SELECT @zipFrom = items FROM (SELECT ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS RowNum, * FROM dbo.fn_Split(@colfilters , '|')) sub  WHERE RowNum = 1
					SELECT @zipTo = items FROM (SELECT ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS RowNum, * FROM dbo.fn_Split(@colfilters , '|')) sub  WHERE RowNum = 2
			
					insert into #tblcol
					select s.ZIP, s.Zip, s.SubscriptionID
					from #subscriptionIDs ts 
						join Subscriptions s  with (nolock) on s.subscriptionid = ts.subscriptionid 
					where substring(s.ZIP,1,5) between @zipFrom and @zipTo 				
				End				
			Else if @colfield = 'COUNTRY'
				Begin
					insert into #tblcol
					select ct.ShortName, ct.ShortName, s.SubscriptionID
					from #subscriptionIDs ts 
						join Subscriptions s  with (nolock) on s.subscriptionid = ts.subscriptionid 
						join UAD_Lookup..Country ct with (nolock) on ct.CountryID = s.CountryID
				End	
		End
	
	if @BrandID = 0
		Begin
			if (@IsRecencyView = 0)
				Begin
					if @rowtype = 'MasterGroup' 
						Begin
							if @rowfilters <> ''
							Begin
								insert into #tblrowvalues 
								select items 
								from dbo.fn_Split(@rowfilters , ',')
								
								insert into #tblrow
								select m.MasterDesc, m.masterValue, ts.SubscriptionID
								from  #subscriptionIDs ts 
									join subscriptiondetails sd  with (nolock) on sd.subscriptionid = ts.subscriptionid 
									join mastercodesheet m  with (nolock) on m.masterid = sd.masterid 
									join #tblrowvalues trv on m.MasterDesc like '%' + trv.value + '%'
								where (m.mastergroupid = @rowmastergroupID)
							End
							Else
							Begin
								insert into #tblrow
								select x.MasterDesc, x.masterValue, sd.SubscriptionID from 
								(
									select TOP 100 m.masterID,m.MasterDesc, m.masterValue, COUNT(sd.SubscriptionID) as Counts
									from 
										#subscriptionIDs ts 
										join subscriptiondetails sd  with (nolock) on sd.subscriptionid = ts.subscriptionid 
										join mastercodesheet m  with (nolock) on m.masterid = sd.masterid 
									where ( m.MasterGroupID = @rowmastergroupID)	
									group by m.masterID,m.MasterDesc, m.masterValue
									order by Counts desc
								) x 
									join subscriptiondetails sd  with (nolock) on x.masterID = sd.masterID 
									join #subscriptionIDs ts on ts.subscriptionID = sd.SubscriptionID	 
							End
						End
		
					if @coltype = 'MasterGroup' 
						Begin
							if @colfilters <> ''
							Begin
								insert into #tblcolvalues 
								select items 
								from dbo.fn_Split(@colfilters , ',')

								insert into #tblcol
								select m.MasterDesc, m.masterValue, ts.SubscriptionID
								from #subscriptionIDs ts 
									join subscriptiondetails sd  with (nolock) on sd.subscriptionid = ts.subscriptionid 
									join mastercodesheet m  with (nolock) on m.masterid = sd.masterid 
									join #tblcolvalues trv on m.MasterDesc like '%' + trv.value + '%'
								where ( m.MasterGroupID = @colmastergroupID)
							End
							else
							Begin
								insert into #tblcol
								select x.MasterDesc, x.masterValue, sd.SubscriptionID from 
								(
									select TOP 100 m.masterID,m.MasterDesc, m.masterValue, COUNT(sd.SubscriptionID) as Counts
									from 
										#subscriptionIDs ts 
										join subscriptiondetails sd  with (nolock) on sd.subscriptionid = ts.subscriptionid 
										join mastercodesheet m  with (nolock) on m.masterid = sd.masterid 
									where ( m.MasterGroupID = @colmastergroupID)	
									group by m.masterID,m.MasterDesc, m.masterValue
									order by Counts desc
								) x 
									join subscriptiondetails sd  with (nolock) on x.masterID = sd.masterID 
									join #subscriptionIDs ts on ts.subscriptionID = sd.SubscriptionID							
							End
						End
				End
			else
				Begin
					if @rowtype = 'MasterGroup' 
						Begin
							if @rowfilters <> ''
							Begin
								insert into #tblrowvalues 
								select items 
								from dbo.fn_Split(@rowfilters , ',')
							
								insert into #tblrow
								select distinct m.MasterDesc, m.masterValue, ts.SubscriptionID
								from #subscriptionIDs ts 
									join vw_RecentConsensus vrc  with (nolock) on vrc.SubscriptionID = ts.SubscriptionID 
									join mastercodesheet m  with (nolock) on m.masterid = vrc.masterid 
									join #tblrowvalues trv on m.MasterDesc like '%' + trv.value + '%'
								where (m.mastergroupid = @rowmastergroupID) and (vrc.MasterGroupID = @rowmastergroupID)
							End
							Else
							Begin
								insert into #tblrow
								select x.MasterDesc, x.masterValue, ts.SubscriptionID from 
								(
									select TOP 100 m.masterID,m.MasterDesc, m.masterValue, COUNT(ts.SubscriptionID) as Counts
									from #subscriptionIDs ts 
										join vw_RecentConsensus vrc  with (nolock) on vrc.SubscriptionID = ts.SubscriptionID 
										join mastercodesheet m  with (nolock) on m.masterid = vrc.masterid 
									where (m.mastergroupid = @rowmastergroupID) and (vrc.MasterGroupID = @rowmastergroupID)
									group by m.masterID,m.MasterDesc, m.masterValue
									order by Counts desc
								) x 
									join vw_RecentConsensus vrc  with (nolock) on x.masterID = vrc.masterID 
									join #subscriptionIDs ts on ts.subscriptionID = vrc.SubscriptionID		
							End
						End
			
					if @coltype = 'MasterGroup' 
						Begin
							if @colfilters <> ''
							Begin
								insert into #tblcolvalues 
								select items 
								from dbo.fn_Split(@colfilters , ',')						

								insert into #tblcol
								select distinct m.MasterDesc, m.masterValue, ts.SubscriptionID
								from #subscriptionIDs ts 
									join vw_RecentConsensus vrc  with (nolock) on vrc.SubscriptionID = ts.SubscriptionID 
									join mastercodesheet m  with (nolock) on m.masterid = vrc.masterid 
									join #tblcolvalues trv on m.MasterDesc like '%' + trv.value + '%'
								where (m.MasterGroupID = @colmastergroupID) and (vrc.MasterGroupID = @colmastergroupID)
							End
							Else
							Begin
							
								insert into #tblcol
								select x.MasterDesc, x.masterValue, ts.SubscriptionID from 
								(
									select TOP 100 m.masterID,m.MasterDesc, m.masterValue, COUNT(ts.SubscriptionID) as Counts
									from #subscriptionIDs ts 
										join vw_RecentConsensus vrc  with (nolock) on vrc.SubscriptionID = ts.SubscriptionID 
										join mastercodesheet m  with (nolock) on m.masterid = vrc.masterid 
									where (m.MasterGroupID = @colmastergroupID) and (vrc.MasterGroupID = @colmastergroupID)
									group by m.masterID,m.MasterDesc, m.masterValue
									order by Counts desc
								) x 
									join vw_RecentConsensus vrc  with (nolock) on x.masterID = vrc.masterID 
									join #subscriptionIDs ts on ts.subscriptionID = vrc.SubscriptionID									
							End
						End
				End
		End
	else
		Begin
			if (@IsRecencyView = 0)
				Begin
					if @rowtype = 'MasterGroup' 
						Begin
							if @rowfilters <> ''
							Begin
								insert into #tblrowvalues 
								select items 
								from dbo.fn_Split(@rowfilters , ',')	
														
								insert into #tblrow
								select distinct m.MasterDesc, m.masterValue, ts.SubscriptionID
								from branddetails bd WITH (nolock)
									join vw_BrandConsensus v  with (nolock) on bd.BrandID = v.BrandID
									join mastercodesheet m  with (nolock) ON m.masterid = v.masterid 		
									join #subscriptionIDs ts ON ts.SubscriptionID = v.SubscriptionID
									join #tblrowvalues trv on m.MasterDesc like '%' + trv.value + '%'
								where bd.BrandID = @BrandID  and (m.MasterGroupID = @rowmastergroupID) 
							End
							Else
							Begin
								insert into #tblrow
								select x.MasterDesc, x.masterValue, ts.SubscriptionID from 
								(
									select TOP 100 m.masterID,m.MasterDesc, m.masterValue, COUNT(ts.SubscriptionID) as Counts
									from branddetails bd WITH (nolock)
										join vw_BrandConsensus v  with (nolock) on bd.BrandID = v.BrandID
										join mastercodesheet m  with (nolock) ON m.masterid = v.masterid 		
										join #subscriptionIDs ts ON ts.SubscriptionID = v.SubscriptionID
									where bd.BrandID = @BrandID  and ( m.MasterGroupID = @rowmastergroupID)	
									group by m.masterID, m.MasterDesc, m.masterValue
									order by Counts desc
								) x 
								join vw_BrandConsensus v  with (nolock) on x.masterID = v.masterID 
								join #subscriptionIDs ts on ts.subscriptionID = v.SubscriptionID	
							End
						End
			
					if @coltype = 'MasterGroup' 
						Begin
							if @colfilters <> ''
							Begin
								insert into #tblcolvalues 
								select items 
								from dbo.fn_Split(@colfilters , ',')
														
								insert into #tblcol
								select distinct m.MasterDesc, m.masterValue, ts.SubscriptionID
								from branddetails bd WITH (nolock)
									join vw_BrandConsensus v on bd.BrandID = v.BrandID
									join mastercodesheet m  with (nolock) ON m.masterid = v.masterid 		
									join #subscriptionIDs ts ON ts.SubscriptionID = v.SubscriptionID
									join #tblcolvalues trv on m.MasterDesc like '%' + trv.value + '%'
								where bd.BrandID = @BrandID  and  (m.MasterGroupID = @colmastergroupID)
							End
							Else
							Begin
								insert into #tblcol
								select x.MasterDesc, x.masterValue, ts.SubscriptionID from 
								(
									select TOP 100 m.masterID,m.MasterDesc, m.masterValue, COUNT(ts.SubscriptionID) as Counts
									from branddetails bd WITH (nolock)
										join vw_BrandConsensus v  with (nolock) on bd.BrandID = v.BrandID
										join mastercodesheet m  with (nolock) ON m.masterid = v.masterid 		
										join #subscriptionIDs ts ON ts.SubscriptionID = v.SubscriptionID
									where bd.BrandID = @BrandID  and ( m.MasterGroupID = @colmastergroupID)	
									group by m.masterID, m.MasterDesc, m.masterValue
									order by Counts desc
								) x 
								join vw_BrandConsensus v  with (nolock) on x.masterID = v.masterID 
								join #subscriptionIDs ts on ts.subscriptionID = v.SubscriptionID
							End
						End
				End
			else
				Begin
					if @rowtype = 'MasterGroup' 
						Begin
							if @rowfilters <> ''
							Begin
								insert into #tblrowvalues 
								select items 
								from dbo.fn_Split(@rowfilters , ',')	
														
								insert into #tblrow
								select distinct m.MasterDesc, m.masterValue, ts.SubscriptionID
								from branddetails bd WITH (nolock)
									join vw_RecentBrandConsensus vrbc  WITH (nolock) on bd.BrandID = vrbc.BrandID
									join mastercodesheet m  with (nolock) ON m.masterid = vrbc.masterid 		
									join #subscriptionIDs ts ON ts.SubscriptionID = vrbc.SubscriptionID
									join #tblrowvalues trv on m.MasterDesc like '%' + trv.value + '%'
								where (m.MasterGroupID = @rowmastergroupID) and bd.BrandID = @BrandID and (vrbc.MasterGroupID = @rowmastergroupID)
							End
							Else
							Begin
								insert into #tblrow
								select x.MasterDesc, x.masterValue, ts.SubscriptionID from 
								(
									select TOP 100 m.masterID,m.MasterDesc, m.masterValue, COUNT(ts.SubscriptionID) as Counts
									from branddetails bd WITH (nolock)
										join vw_RecentBrandConsensus vrbc  with (nolock) on bd.BrandID = vrbc.BrandID
										join mastercodesheet m  with (nolock) ON m.masterid = vrbc.masterid 		
										join #subscriptionIDs ts ON ts.SubscriptionID = vrbc.SubscriptionID
									where (m.MasterGroupID = @rowmastergroupID) and bd.BrandID = @BrandID and (vrbc.MasterGroupID = @rowmastergroupID)	
									group by m.masterID, m.MasterDesc, m.masterValue
									order by Counts desc
								) x 
								join vw_RecentBrandConsensus v  with (nolock) on x.masterID = v.masterID 
								join #subscriptionIDs ts on ts.subscriptionID = v.SubscriptionID	
							End
						End
			
					if @coltype = 'MasterGroup' 
						Begin
							if @colfilters <> ''
							Begin
								insert into #tblcolvalues 
								select items 
								from dbo.fn_Split(@colfilters , ',')
														
								insert into #tblcol
								select distinct m.MasterDesc, m.masterValue, ts.SubscriptionID
								from branddetails bd WITH (nolock)
									join vw_RecentBrandConsensus vrbc  WITH (nolock) on bd.BrandID = vrbc.BrandID
									join mastercodesheet m  with (nolock) ON m.masterid = vrbc.masterid 		
									join #subscriptionIDs ts ON ts.SubscriptionID = vrbc.SubscriptionID
									join #tblcolvalues trv on m.MasterDesc like '%' + trv.value + '%'
								where (m.MasterGroupID = @colmastergroupID) and bd.BrandID = @BrandID and (vrbc.MasterGroupID = @colmastergroupID)
							End
							Else
							Begin
								insert into #tblcol
								select x.MasterDesc, x.masterValue, ts.SubscriptionID from 
								(
									select TOP 100 m.masterID,m.MasterDesc, m.masterValue, COUNT(ts.SubscriptionID) as Counts
									from branddetails bd WITH (nolock)
										join vw_RecentBrandConsensus vrbc  with (nolock) on bd.BrandID = vrbc.BrandID
										join mastercodesheet m  with (nolock) ON m.masterid = vrbc.masterid 		
										join #subscriptionIDs ts ON ts.SubscriptionID = vrbc.SubscriptionID
									where (m.MasterGroupID = @colmastergroupID) and bd.BrandID = @BrandID and (vrbc.MasterGroupID = @colmastergroupID)
									group by m.masterID, m.MasterDesc, m.masterValue
									order by Counts desc
								) x 
								join vw_RecentBrandConsensus v  with (nolock) on x.masterID = v.masterID 
								join #subscriptionIDs ts on ts.subscriptionID = v.SubscriptionID
							End						
						End
				end
		END
	
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