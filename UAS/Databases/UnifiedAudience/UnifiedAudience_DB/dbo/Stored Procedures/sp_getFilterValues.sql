CREATE Proc [dbo].[sp_getFilterValues]
(
	@xmlFilter TEXT
)
as
BEGIN
	
	SET NOCOUNT ON

	declare @docHandle int , @TempVar varchar(1000), @mysplit varchar(1000) 

	EXEC sp_xml_preparedocument @docHandle OUTPUT, @xmlFilter  

	create table #textfilters
			(  
			Pub varchar(800) , 
			Category varchar(800) , 
			CategoryCodes varchar(800) , 
			Transact varchar(800) , 
			Qsource varchar(800) ,  
			State varchar(800) , 
			Country varchar(800) , 
			Email varchar(25) , 
			Phone varchar(25) ,  
			Fax varchar(25) , 
			Demo7 varchar(25) , 
			Demo31 varchar(25) , 
			Demo32 varchar(25) , 
			Demo33 varchar(25) ,  
			Demo34 varchar(25) , 
			Demo35 varchar(25) , 
			Demo36 varchar(25) ,
			Qfrom varchar(10) ,
			QTo varchar(150) ,
			[Year] varchar(4) ,
			AdhocColumn varchar(100) ,
			AdhocValue varchar(100) 
		)  
	Insert into #textfilters
	SELECT	*
	FROM OPENXML(@docHandle, N'/filters')   
	WITH   
		(  
			Pub varchar(800) 'PubIDs', 
			Category varchar(800) 'CategoryIDs', 
			CategoryCodes varchar(800) 'CategoryCodes', 
			Transact varchar(800) 'TransactionIDs', 
			Qsource varchar(800) 'QsourceIDs',  
			State varchar(800) 'StateIDs', 
			Country varchar(800) 'CountryIDs', 
			Email varchar(25) 'Email', 
			Phone varchar(25) 'Phone',  
			Fax varchar(25) 'Fax', 
			Demo7 varchar(25) 'Demo7', 
			Demo31 varchar(25) 'Demo31', 
			Demo32 varchar(25) 'Demo32', 
			Demo33 varchar(25) 'Demo33',  
			Demo34 varchar(25) 'Demo34', 
			Demo35 varchar(25) 'Demo35', 
			Demo36 varchar(25) 'Demo36',
			Qfrom varchar(10) 'Qfrom',
			QTo varchar(150) 'QTo',
			[Year] varchar(4) 'Year',
			AdhocColumn varchar(100) 'AdhocColumn',
			AdhocValue varchar(100) 'AdhocValue'
		)  

		set @TempVar = ''
		set @mysplit = (select Category from #textfilters)
		select @TempVar = (case when @TempVar = '' then categorygroupName else @TempVar + ', ' + categorygroupName end) 
		from categorygroup 
		where categoryGroupID in (select items from dbo.fn_split(@mysplit, ','))
		update #textfilters set Category = @TempVar

		set @TempVar = ''
		set @mysplit = (select CategoryCodes from #textfilters)
		select @TempVar = (case when @TempVar = '' then categoryName else @TempVar + ', ' + categoryName end) 
		from category 
		where categoryID in (select items from dbo.fn_split(@mysplit, ','))
		update #textfilters set CategoryCodes = @TempVar

		set @TempVar = ''
		set @mysplit = (select Transact from #textfilters)
		select @TempVar = (case when @TempVar = '' then transactiongroupName else @TempVar + ', ' + transactiongroupName end) 
		from transactiongroup 
		where transactiongroupID in (select items from dbo.fn_split(@mysplit, ','))
		update #textfilters set Transact = @TempVar

		set @TempVar = ''
		set @mysplit = (select Qsource from #textfilters)
		select @TempVar = (case when @TempVar = '' then QsourcegroupName else @TempVar + ', ' + QsourcegroupName end) 
		from Qsourcegroup 
		where QsourcegroupID in (select items from dbo.fn_split(@mysplit, ','))
		update #textfilters set QSource = @TempVar

		set @TempVar = ''
		set @mysplit = (select Country from #textfilters)
		select @TempVar = (case when @TempVar = '' then country else @TempVar + ', ' + country end) 
		from Country 
		where CountryID in (select items from dbo.fn_split(@mysplit, ','))
		update #textfilters set Country = @TempVar

		set @mysplit = (select Email from #textfilters)
		if len(@mysplit) > 0
			begin
				update #textfilters set Email = case when Email = '1' then 'Yes' else 'No' end
			end
		set @mysplit = (select Phone from #textfilters)
		if len(@mysplit) > 0
			begin
				update #textfilters set Phone = case when Phone = '1' then 'Yes' else 'No' end
			end	

		set @mysplit = (select Fax from #textfilters)
		if len(@mysplit) > 0
			begin
				update #textfilters set Fax = case when Fax = '1' then 'Yes' else 'No' end		
			end


		update #textfilters set Demo7 = case when Demo7 = 'A' then 'Print' when Demo7 = 'B' then 'Digital' when Demo7 = 'C' then 'Both' else '' end	

		set @mysplit = (select Demo31 from #textfilters)
		if len(@mysplit) > 0
			begin
				update #textfilters set Demo31 = case when Demo31 = '1' then 'Yes' else 'No' end
			end
		set @mysplit = (select Demo32 from #textfilters)
		if len(@mysplit) > 0
			begin
				update #textfilters set Demo32 = case when Demo32 = '1' then 'Yes' else 'No' end
			end
		set @mysplit = (select Demo33 from #textfilters)
		if len(@mysplit) > 0
			begin
				update #textfilters set Demo33 = case when Demo33 = '1' then 'Yes' else 'No' end
			end
		set @mysplit = (select Demo34 from #textfilters)
		if len(@mysplit) > 0
			begin
				update #textfilters set Demo34 = case when Demo34 = '1' then 'Yes' else 'No' end
			end

		set @mysplit = (select Demo35 from #textfilters)
		if len(@mysplit) > 0
			begin
				update #textfilters set Demo35 = case when Demo35 = '1' then 'Yes' else 'No' end
			end

		set @mysplit = (select Demo36 from #textfilters)
		if len(@mysplit) > 0
			begin
				update #textfilters set Demo36 = case when Demo36 = '1' then 'Yes' else 'No' end
			end

		set @mysplit = (select Year from #textfilters)
		if len(@mysplit) > 0
			Begin
				update #textfilters Set [Year] = Convert(varchar,Convert(int,@mysplit) + 1) + ' yr'
			end

		select * 
		from #textfilters
		EXEC sp_xml_removedocument @docHandle    
		drop table #textfilters
END