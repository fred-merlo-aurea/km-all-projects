CREATE PROCEDURE [dbo].[sp_ExecuteFilters_for_intersection]
@Queries XML
AS
Begin
	set nocount on

	declare @hDoc AS INT
	declare @minfilterno int, @maxfilterno int, @noofqueries int, @qry varchar(max),
			@combination varchar(10),
			@createtbl varchar(max) = '',
			@comboqry varchar(max) = '',
			@selecttbl varchar(max) = ''

	create table #tblqueries  (filterno int, query varchar(max))
	create table #tblresults  (filterno int, subscriptionID int, primary key (filterno, subscriptionID))
	

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

	print 'START : ' + convert(varchar(20), getdate(), 109)

	while @minfilterno <= @maxfilterno
	Begin
		
		select @qry = query from #tblqueries with (NOLOCK) where filterno = @minfilterno
		--print (@qry )
		
		insert into #tblresults  
		execute (@qry )
		
		--print convert(varchar(100), @minfilterno) + ' / end : ' + convert(varchar(20), getdate(), 109)
		
		set @minfilterno = @minfilterno + 1
	End 

	declare @x int = 1,
			@y int = 1

	if @noofqueries > 1
	Begin
		set @createtbl = ' create table #tblresultsCount (Filterno varchar(100), sortorder int,  '
		set @selecttbl =  ' select Filterno,  '

		while (@x <= @noofqueries)
		Begin

			set @createtbl += '[' + convert(varchar(10),@x) + '] bigint, '
			set @selecttbl +=  ' isnull(MAX ([' + convert(varchar(10),@x) + ']),0) as [' + convert(varchar(10),@x) + '],'
			set @x = @x + 1
		End

		set @createtbl += ' Total bigint);'
		set @selecttbl += '  isnull(max(Total),0) as  Total from #tblresultsCount group by Filterno order by (case when isnumeric(filterno) = 1 then filterno else 1000 end); drop table #tblresultsCount'
	end

	print '@comboqry ' + @comboqry

	 set @x = 1
	 set @y = 1

	if @noofqueries > 1
	Begin
		
		while (@x <= @noofqueries)
		Begin
				set @y = 1

				while (@y <= @noofqueries)
				Begin

					if (@x = @y)
					Begin
						set @comboqry += 
						'Insert into #tblresultsCount (FilterNo, [' + convert(varchar(10),@y) + '])
						select ' + convert(varchar(10),@x) + ', COUNT(*)
						from #tblresults with (NOLOCK) where filterno = ' + convert(varchar(10),@x) + ';'
					end
					else
					Begin
						set @comboqry += 
						'Insert into #tblresultsCount (FilterNo,  [' + convert(varchar(10),@y) + '])
						select ' + convert(varchar(10),@x)  + ', COUNT(*)
						from
						(
						select subscriptionID from #tblresults with (NOLOCK) where filterno = ' + convert(varchar(10),@x) + '
						intersect 
						select subscriptionID from #tblresults with (NOLOCK) where filterno = ' + convert(varchar(10),@y) + '
						) x; '
					end

					set @y = @y + 1
				end
				set @x = @x + 1
		End
	end


    if @noofqueries > 1
	Begin
		DECLARE @Allfiltercombo VARCHAR(8000) = ''
		
		set @Allfiltercombo = (SELECT stuff((SELECT ','+ convert(varchar(1000),filterno) FROM #tblqueries FOR XML PATH('')),1,1,'') t)
		
		set @comboqry += 'insert into #tblresultsCount (FilterNo, Total)
		 select ''ALL'', COUNT(*) from (
		select subscriptionID from #tblresults with (NOLOCK) where filterno = ' + REPLACE(@Allfiltercombo, ',', '	intersect select subscriptionID from #tblresults with (NOLOCK) where filterno = ') +' ) x '
	End

	--print(@createtbl)		
	--print(@comboqry)		
	--print(@selecttbl)

	set @comboqry = @createtbl + ' ' +  @comboqry  + ' ' +  @selecttbl

	exec(@comboqry)		

	drop table #tblqueries
	drop table #tblresults

	print 'END : ' + convert(varchar(20), getdate(), 109);
END
