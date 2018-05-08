create Proc [dbo].[sp_getDimensionFilterValues]
(
	@FilterString TEXT 
)
as
Begin
/*
Select 'a1234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890' as FilterGroup, 'a1234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890a1234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890a1234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890a1234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890a1234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890a1234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890a1234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890a1234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890a1234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890a1234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890' as  FilterValues
*/
	
	DECLARE @docHandle int,
			--@INDEX INT,         
			--@SLICE nvarchar(4000),
			--@LoopCount int,
			--@Delimiter char(1),
			@demoFilter varchar(2000)
	
	set nocount on
	--Set @Delimiter = '#'

	create table #tblfilter
	(
		FilterGroup varchar(50),
		FilterIDs  varchar(4000),
		FilterValues varchar(4000)
	)

	IF @FilterString IS NULL RETURN         
	
	EXEC sp_xml_preparedocument @docHandle OUTPUT, @FilterString  
	
	declare @dimensions table (dimension varchar(100), codes varchar(2000));
	
		WITH cte1 (dimension, codes)	
		AS
		-- Define the CTE query.
		(
			select  col, value
				FROM OPENXML(@docHandle, N'/Filters/FilterType[@ID="D"]/FilterGroup/Value')   
						WITH (col varchar(800) '../@Type', value varchar(800) '.')		
		)
		-- Define the outer query referencing the CTE name.
		insert into @dimensions
		select
			dimension,
			'select distinct rg.DisplayName, responseID, dbo.fn_getResponseValues(responseID)  from vw_Response r 
			--join ResponseType rg on r.ResponseTypeID = rg.ResponseTypeID 
			where r.ResponseID in (' + 
			stuff((
				select ',' + t.[codes]
				from cte1 t
				where t.dimension = t1.dimension
				order by t.[codes]
				for xml path('')
			),1,1,'') + ')' as name_csv
		from cte1 t1
		group by dimension
		; 

		if exists (select top 1 * from @dimensions)
		Begin
			SELECT 
			   @demoFilter = STUFF( (SELECT ' ' + codes 
										 FROM @dimensions
										 ORDER BY dimension
										 FOR XML PATH('')), 
										1, 1, '')	
	
		end
		
	insert into #tblfilter (FilterGroup, FilterIDs, FilterValues)
	exec(@demoFilter)

	Select FilterGroup,FilterValues = STUFF( (SELECT ', ' + ltrim(rtrim(FilterValues ))
										 FROM #tblfilter t2
										 where t2.FilterGroup = t1.FilterGroup
										 FOR XML PATH('')), 
										1, 1, '')  
	
			
	from #tblfilter t1
	group by t1.FilterGroup
	
	drop table #tblfilter
	EXEC sp_xml_removedocument @docHandle    
	


END