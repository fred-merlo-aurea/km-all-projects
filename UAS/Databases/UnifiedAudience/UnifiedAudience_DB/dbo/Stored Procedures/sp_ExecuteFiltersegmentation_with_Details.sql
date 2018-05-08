CREATE PROCEDURE [dbo].[sp_ExecuteFiltersegmentation_with_Details]
@Queries XML
AS
Begin
	set nocount on

	declare @hDoc AS INT
	declare @filterno int, @linenumber int, @minfilterno int, @maxfilterno int, @noofqueries int, @qry varchar(max),
			@combination varchar(100),
			@comboqry varchar(max) = ''

	create table #tblquery  (filterno int, Query varchar(max))
	create table #tblqueryresults  (filterno int, subscriptionID int, primary key (filterno, subscriptionID))

	create table #tblFSquery  (linenumber int, SelectedFilterNo varchar(800), SelectedFilterOperation varchar(20), SuppressedFilterNo varchar(800), SuppressedFilterOperation varchar(20), FilterDescription varchar(max), query varchar(max))
	create table #tblFSqueryresults  (linenumber int, subscriptionID int, primary key (linenumber, subscriptionID))
	
	create table #tblresultsCount  (SelectedFilterNo varchar(800), SelectedFilterOperation varchar(20), SuppressedFilterNo varchar(800), SuppressedFilterOperation varchar(20), filterdescription varchar(max), operation varchar(100), count int)

	EXEC sp_xml_preparedocument @hDoc OUTPUT, @queries

	insert into #tblquery
	SELECT filterno, Query
	FROM OPENXML(@hDoc, 'xml/Queries/Query')
	WITH 
	(
	filterno int '@filterno',
	Query [varchar](max) '.'
	)
	
	insert into #tblFSquery (linenumber, SelectedFilterNo, SelectedFilterOperation, SuppressedFilterNo, SuppressedFilterOperation, FilterDescription)
	SELECT linenumber, selectedfilterno, selectedfilteroperation, suppressedfilterno, suppressedfilteroperation, filterdescription
	FROM OPENXML(@hDoc, 'xml/Results/Result')
	WITH 
	(
		linenumber int							'@linenumber',
		selectedfilterno varchar(800)			'@selectedfilterno',
		selectedfilteroperation varchar(20)		'@selectedfilteroperation',
		suppressedfilterno varchar(800)			'@suppressedfilterno',
		suppressedfilteroperation varchar(20)	'@suppressedfilteroperation',
		filterdescription varchar(max)			'@filterdescription'
	)

	EXEC sp_xml_removedocument @hDoc
	
	--print 'START : ' + convert(varchar(20), getdate(), 109)

	update t
	set query = 'select distinct ' + convert(varchar(100),linenumber) + ', subscriptionID from ((select subscriptionID from #tblqueryresults t where filterno = ' + REPLACE(SelectedFilterNo, ',', '  ' + SelectedFilterOperation + ' select subscriptionID from #tblqueryresults t where filterno = ') + ') ' + 
	case when len(SuppressedFilterOperation) > 0 then 
		' except ' 
		+ ' (select subscriptionID from #tblqueryresults t where filterno = ' + REPLACE(SuppressedFilterNo , ',', '  ' + SuppressedFilterOperation + ' select subscriptionID from #tblqueryresults t where filterno = ') + ') '
	else
	'' end
	+ ') x' from #tblFSquery t
	
	select @noofqueries = MAX(linenumber) from #tblFSquery

	DECLARE c_queries CURSOR 
	FOR select filterno from #tblquery
		
	OPEN c_queries  
	FETCH NEXT FROM c_queries INTO @filterno

	WHILE @@FETCH_STATUS = 0  
	BEGIN  
		
		select @qry = query from #tblquery with (NOLOCK) where filterno = @filterno
		--print (@qry )
		
		insert into #tblqueryresults  
		execute (@qry )
		
		--print convert(varchar(100), @minfilterno) + ' / end : ' + convert(varchar(20), getdate(), 109)
		
		FETCH NEXT FROM c_queries INTO @filterno

	END
	
	CLOSE c_queries  
	DEALLOCATE c_queries 
	
	DECLARE c_results CURSOR 
	FOR select linenumber from #tblFSquery
		
	OPEN c_results  
	FETCH NEXT FROM c_results INTO @linenumber

	WHILE @@FETCH_STATUS = 0  
	BEGIN  
		
		select @qry = query from #tblFSquery with (NOLOCK) where linenumber = @linenumber
		
		insert into #tblFSqueryresults 
		execute (@qry )
		
		--print convert(varchar(100), @minfilterno) + ' / end : ' + convert(varchar(20), getdate(), 109)
		
		FETCH NEXT FROM c_results INTO @linenumber

	END
	
	CLOSE c_results  
	DEALLOCATE c_results;
	
	WITH CTE_RESULTS (SelectedFilterNo, SuppressedFilterNo, SelectedFilterOperation, SuppressedFilterOperation, FilterDescription, linenumber, counts)
	AS
	(
		select tq.SelectedFilterNo, tq.SuppressedFilterNo, tq.SelectedFilterOperation, tq.SuppressedFilterOperation, tq.FilterDescription,  tq.linenumber, COUNT(tr.subscriptionID) as counts 
		from	
				#tblFSquery tq with (NOLOCK) 
				left outer join #tblFSqueryresults tr with (NOLOCK) on tq.linenumber = tr.linenumber
		group by tq.SelectedFilterNo, tq.SuppressedFilterNo, tq.SelectedFilterOperation, tq.SuppressedFilterOperation, tq.FilterDescription,  tq.linenumber
	)
	Insert into #tblresultsCount (SelectedFilterNo, SuppressedFilterNo, SelectedFilterOperation, SuppressedFilterOperation , filterdescription, operation , count)
	SELECT SelectedFilterNo, SuppressedFilterNo, SelectedFilterOperation, SuppressedFilterOperation, FilterDescription, 'Individual',  COUNTs from CTE_RESULTS
	union
	select convert(varchar(100),linenumber),'','','','{ sets: [''F' + convert(varchar(100),linenumber) + '''], size: ' + CONVERT(varchar(100), counts) +' }', 'venn', counts  
	from CTE_RESULTS with (NOLOCK) 
	Where linenumber <= 10

	declare @combinations table (ID int IDENTITY (1,1), combinations varchar(100));

	Insert into @combinations 
	select * from master.dbo.fn_GetCombinations(@noofqueries)
	order by LEN(combinations), combinations
	
	declare @minID int, @maxID int
	set @minID = 1
	set @maxID = 0
	
	select @maxID = MAX(ID) from @combinations

	--print '   Start Venn : ' + convert(varchar(20), getdate(), 109)

	while (@minID <= @maxID)
	Begin
		select @combination = combinations from @combinations where ID = @minID 

		set @comboqry = 'insert into #tblresultsCount (SelectedFilterNo, SelectedFilterOperation, SuppressedFilterNo, SuppressedFilterOperation , filterdescription, operation , count) 
		select '''+ @combination + ''', ''intersect'', '''', '''', ''{ sets: [''''F' + replace(@combination, ',', ''''',''''F')  + '''''], size: '' + CONVERT(varchar(100),COUNT(*)) + '' }'', ''venn'', COUNT(*) from (
		select subscriptionID from #tblFSqueryresults with (NOLOCK) where linenumber = ' + REPLACE(@combination, ',', '	intersect select subscriptionID from #tblFSqueryresults with (NOLOCK) where linenumber = ') +' ) x '
		
		--print(@comboqry)
		exec(@comboqry)
		
		set @minID = @minID + 1
	End

	--print '   end Venn: ' + convert(varchar(20), getdate(), 109)
		
	select * from #tblresultsCount with (NOLOCK)
	order by operation, len(SelectedFilterNo), SelectedFilterNo

	drop table #tblquery
	drop table #tblqueryresults
	drop table #tblFSquery
	drop table #tblFSqueryresults
	drop table #tblresultsCount

	--print 'END : ' + convert(varchar(20), getdate(), 109);
END