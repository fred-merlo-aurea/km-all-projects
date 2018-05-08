CREATE PROCEDURE [dbo].[sp_ExecuteFilters_with_Details]
@Queries XML
AS
Begin
	set nocount on

	declare @hDoc AS INT
	declare @minfilterno int, @maxfilterno int, @noofqueries int, @qry varchar(max),
			@combination varchar(100),
			@comboqry varchar(max) = ''

	create table #tblqueries  (filterno int, query varchar(max))
	create table #tblresults  (filterno int, subscriptionID int, primary key (filterno, subscriptionID))
	create table #tblresultsCount  (SelectedFilterNo varchar(800), SelectedFilterOperation varchar(20), SuppressedFilterNo varchar(800), SuppressedFilterOperation varchar(20), filterdescription varchar(500), operation varchar(100), count int)

	EXEC sp_xml_preparedocument @hDoc OUTPUT, @queries

	insert into #tblqueries
	SELECT filterno, query
	FROM OPENXML(@hDoc, 'xml/query')
	WITH 
	(
	filterno int '@filterno',
	query [varchar](max) '.'
	)

	EXEC sp_xml_removedocument @hDoc

	select @minfilterno = MIN(filterno), @maxfilterno = max(filterno), @noofqueries = COUNT(distinct filterno) from #tblqueries

	--print 'START : ' + convert(varchar(20), getdate(), 109)

	while @minfilterno <= @maxfilterno
	Begin
		
		select @qry = query from #tblqueries with (NOLOCK) where filterno = @minfilterno
		--print (@qry )
		
		insert into #tblresults  
		execute (@qry )
		
		--print convert(varchar(100), @minfilterno) + ' / end : ' + convert(varchar(20), getdate(), 109)
		
		set @minfilterno = @minfilterno + 1
	End 

	Insert into #tblresultsCount (SelectedFilterNo, SuppressedFilterNo, SelectedFilterOperation, SuppressedFilterOperation , filterdescription, operation , count)
	select tq.filterno, null, 'single', null, tq.filterno, 'individual', COUNT(tr.subscriptionID) 
	from	
			#tblqueries tq with (NOLOCK) 
			left outer join #tblresults tr with (NOLOCK) on tq.filterno = tr.filterno
	group by tq.filterno

	Insert into #tblresultsCount (filterdescription, operation , count)
	select '{ sets: [''F' + SelectedFilterNo + '''], size: ' + CONVERT(varchar(100), count) +' }', 'venn', count  
	from #tblresultsCount with (NOLOCK) 
	Where operation = 'individual' and SelectedFilterNo <= 10
	
	Insert into #tblresultsCount (SelectedFilterNo, SuppressedFilterNo, SelectedFilterOperation, SuppressedFilterOperation , filterdescription, operation , count)
	select SelectedFilterNo,'', 'single', '', SelectedFilterNo, 'combo', count  
	from #tblresultsCount with (NOLOCK) 
	Where operation = 'individual' 
	--and SelectedFilterNo <= 5	
	
	declare @x int = 1,
			@y int = 1
	 set @x = 1
	 set @y = 1
	 
	if @noofqueries > 1
	Begin
		
		while (@x <= @noofqueries)
		Begin
				set @y = 1

				while (@y <= @noofqueries)
				Begin
					if (@y > @x and @x <> @y)
					Begin
						set @comboqry += 
						'Insert into #tblresultsCount (SelectedFilterNo, SuppressedFilterNo, SelectedFilterOperation, SuppressedFilterOperation , filterdescription, operation , count)' +
						'select ''' + convert(varchar(10),@x) + ',' + convert(varchar(10),@y) +  ''','''', ''intersect'','''', ''' + convert(varchar(10),@x) + ',' + convert(varchar(10),@y) + '(intersect)'', ''combo'', COUNT(*)
						from
						(
						select subscriptionID from #tblresults with (NOLOCK) where filterno = ' + convert(varchar(10),@x) +
						' intersect 
						select subscriptionID from #tblresults with (NOLOCK) where filterno = ' + convert(varchar(10),@Y) +
						') x;	'		
						
						set @comboqry += 
						'Insert into #tblresultsCount (SelectedFilterNo, SuppressedFilterNo, SelectedFilterOperation, SuppressedFilterOperation , filterdescription, operation , count)' +
						'select ''' + convert(varchar(10),@x) + ',' + convert(varchar(10),@y) +  ''','''', ''union'','''', ''' + convert(varchar(10),@x) + ',' + convert(varchar(10),@y) + '(union)'', ''combo'', COUNT(*)
						from
						(
						select subscriptionID from #tblresults with (NOLOCK) where filterno = ' + convert(varchar(10),@x) +
						' UNION 
						select subscriptionID from #tblresults with (NOLOCK) where filterno = ' + convert(varchar(10),@Y) +
						') x;	'	
					end
					if(@x <> @y)
					Begin
						set @comboqry += 
						'Insert into #tblresultsCount (SelectedFilterNo, SuppressedFilterNo, SelectedFilterOperation, SuppressedFilterOperation , filterdescription, operation , count)' +
						'select ''' + convert(varchar(10),@x) + ''',''' + convert(varchar(10),@y) + ''', '''','''', ''' + convert(varchar(10),@x) + ' NOT IN ' + convert(varchar(10),@y) + ''', ''combo'', COUNT(*)
						from
						(
						select subscriptionID from #tblresults with (NOLOCK) where filterno = ' + convert(varchar(10),@x) +
						' except 
						select subscriptionID from #tblresults with (NOLOCK) where filterno = ' + convert(varchar(10),@y) +
						') x;	'
					end		
					set @y = @y + 1
				end
				set @x = @x + 1
		End
	end

	--print(@comboqry);
	exec(@comboqry);

	declare @combinations table (ID int IDENTITY (1,1), combinations varchar(500));

	Insert into @combinations 
	select * from master.dbo.fn_GetCombinations(@noofqueries)
	order by LEN(combinations), combinations

	declare @minID int, @maxID int
	set @minID = 1
	set @maxID = 0
	
	select @maxID = MAX(ID) from @combinations

	--print '   Start Venn : ' + convert(varchar(20), getdate(), 109)

	while (@minID <= @maxID and @maxID >= 1)
	Begin
		select @combination = combinations from @combinations where ID = @minID 

		set @comboqry = 'insert into #tblresultsCount (SelectedFilterNo, SelectedFilterOperation, SuppressedFilterNo, SuppressedFilterOperation , filterdescription, operation , count) 
		select '''+ @combination + ''', ''intersect'', '''', '''', ''{ sets: [''''F' + replace(@combination, ',', ''''',''''F')  + '''''], size: '' + CONVERT(varchar(100),COUNT(*)) + '' }'', ''venn'', COUNT(*) from (
		select subscriptionID from #tblresults with (NOLOCK) where filterno = ' + REPLACE(@combination, ',', '	intersect select subscriptionID from #tblresults with (NOLOCK) where filterno = ') +' ) x '
		
		--print(@comboqry)
		exec(@comboqry)
		
		set @minID = @minID + 1
	End

	--print '   end Venn: ' + convert(varchar(20), getdate(), 109)

    if @noofqueries > 1
	Begin
		DECLARE @Allfiltercombo VARCHAR(8000) = ''
		
		set @Allfiltercombo = (SELECT stuff((SELECT ','+ convert(varchar(1000),filterno) FROM #tblqueries FOR XML PATH('')),1,1,'') t)
		
		set @comboqry = 'insert into #tblresultsCount (SelectedFilterNo, SuppressedFilterNo, SelectedFilterOperation, SuppressedFilterOperation , filterdescription, operation , count)
		 select ''' + @Allfiltercombo + ''',null, ''intersect'',null,''All Intersect'', ''combo'', COUNT(*) from (
		select subscriptionID from #tblresults with (NOLOCK) where filterno = ' + REPLACE(@Allfiltercombo, ',', '	intersect select subscriptionID from #tblresults with (NOLOCK) where filterno = ') +' ) x '
		
		--print(@comboqry)
		exec(@comboqry)
		
		set @comboqry = 'insert into #tblresultsCount (SelectedFilterNo, SuppressedFilterNo, SelectedFilterOperation, SuppressedFilterOperation , filterdescription, operation , count)
		 select ''' + @Allfiltercombo + ''',null, ''union'',null,''All Union'', ''combo'', COUNT(*) from (
		select subscriptionID from #tblresults with (NOLOCK) where filterno = ' + REPLACE(@Allfiltercombo, ',', '	union select subscriptionID from #tblresults with (NOLOCK) where filterno = ') +' ) x '
		
		--print(@comboqry)
		exec(@comboqry)
	End
		
	select * from #tblresultsCount with (NOLOCK)
	order by operation, len(SelectedFilterNo), SelectedFilterNo, SuppressedFilterNo

	drop table #tblqueries
	drop table #tblresults
	drop table #tblresultsCount

	--print 'END : ' + convert(varchar(20), getdate(), 109);
END
