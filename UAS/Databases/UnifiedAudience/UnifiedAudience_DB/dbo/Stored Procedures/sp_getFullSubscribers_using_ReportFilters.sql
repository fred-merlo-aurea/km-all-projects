CREATE PROCEDURE [dbo].[sp_getFullSubscribers_using_ReportFilters]
(
	@FilterString varchar(8000) 
)
as
BEGIN
	
	SET NOCOUNT ON

	DECLARE @INDEX INT,         
		@SLICE nvarchar(4000),
		@LoopCount int,
		@CategoryIDs varchar(800),
		@CategoryCodes varchar(800),
		@TransactionIDs varchar(800),
		@QsourceIDs varchar(800),
		@StateIDs varchar(800),
		@CountryIDs varchar(800),
		@Email varchar(800),
		@Phone varchar(800),
		@Fax varchar(800),
		@Demo7 varchar(1),		
		@Demo31 varchar(1),		
		@Demo32 varchar(1),		
		@Demo33 varchar(1),		
		@Demo34 varchar(1),
		@Demo35 varchar(1),
		@Demo36 varchar(1),
		@Qfrom varchar(10),
		@QTo varchar(10),
		@Year varchar(4),
		@Delimiter char(1),
		@executeString varchar(8000),
		@startDate varchar(10),		
		@endDate varchar(10),
		@subsrc varchar(50),
		@AdhocColumn varchar(100),
		@AdhocValue varchar(100),
		@currentYear int
	
	Set @Delimiter = '#'
	set	@CategoryIDs = ''
	set	@TransactionIDs = ''
	set	@QsourceIDs = ''
	set	@StateIDs = ''
	set	@CountryIDs = ''
	set	@Email = ''
	set	@Phone = ''
	set	@Fax = ''
	set	@Demo7 = ''
	set	@Demo31 = ''
	set	@Demo32 = ''
	set	@Demo33 = ''
	set	@Demo34 = ''
	set	@Demo35 = ''
	set	@Demo36 = ''
	set	@Qfrom = ''
	set	@QTo = ''
	set @CategoryCodes	= ''
	set @year = ''
	set @subsrc = ''
	set @AdhocColumn = ''
	set 	@AdhocValue = ''

	declare @tmpFilterString varchar(1000)

	set @tmpFilterString = ltrim(rtrim(replace(@FilterString,'#','')))

	if len(@tmpFilterString) > 0
		Begin
			create table #State	(State varchar(5))
			create table #country (Country_ID INT)

			set @executeString =' Select	s.SubscriptionID , s.CGRP_NO' +
								' from	Subscriptions s join ' +
										' Category dc on s.Category_ID = dc.Category_ID join ' +
										' [Transaction] dt on s.Transaction_ID = dt.Transaction_ID left outer join ' +
										' QSource dq on s.Qsource_ID = dq.Qsource_ID ' +
								' Where ' 

	    
			-- HAVE TO SET TO 1 SO IT DOESNT EQUAL Z         
			--     ERO FIRST TIME IN LOOP         
	    
			SELECT @INDEX = 1         
			Select @LoopCount = 0
			IF @FilterString IS NULL RETURN         
		    
			WHILE @INDEX !=0             
				BEGIN    
		              
					Select @LoopCount = @LoopCount + 1
					SELECT @INDEX = CHARINDEX(@Delimiter,@FilterString)             
					IF @INDEX !=0             
						SELECT @SLICE = LEFT(@FilterString,@INDEX - 1)             
					ELSE             
						SELECT @SLICE = @FilterString             
		      
					if @LoopCount = 1
						set @CategoryIDs = ltrim(rtrim(@SLICE))
					else if @LoopCount = 2
						set @TransactionIDs = ltrim(rtrim(@SLICE))
					else if @LoopCount = 3
						set @QsourceIDs = ltrim(rtrim(@SLICE))
					else if @LoopCount = 4
						set @StateIDs = ltrim(rtrim(@SLICE))
					else if @LoopCount = 5
						set @CountryIDs = ltrim(rtrim(@SLICE))
					else if @LoopCount = 6
						set @Email = ltrim(rtrim(@SLICE))
					else if @LoopCount = 7
						set @Phone = ltrim(rtrim(@SLICE))
					else if @LoopCount = 8
						set @Fax = ltrim(rtrim(@SLICE))
					else if @LoopCount = 9
						set @Demo7 = ltrim(rtrim(@SLICE))
					else if @LoopCount = 10
						set @Demo31 = ltrim(rtrim(@SLICE))
					else if @LoopCount = 11
						set @Demo32 = ltrim(rtrim(@SLICE))
					else if @LoopCount = 12
						set @Demo33 = ltrim(rtrim(@SLICE))
					else if @LoopCount = 13
						set @Demo34 = ltrim(rtrim(@SLICE))
					else if @LoopCount = 14
						set @Demo35 = ltrim(rtrim(@SLICE))
					else if @LoopCount = 15
						set @Demo36 = ltrim(rtrim(@SLICE))
					else if @LoopCount = 16
						set @Qfrom = ltrim(rtrim(@SLICE))
					else if @LoopCount = 17
						set @QTo = ltrim(rtrim(@SLICE))
					else if @LoopCount = 18
						set @Year = ltrim(rtrim(@SLICE))
					else if @LoopCount = 19
						set @CategoryCodes = ltrim(rtrim(@SLICE))
					else if @LoopCount = 20
						set @AdhocColumn = ltrim(rtrim(@SLICE))
					else if @LoopCount = 21
						set @AdhocValue = ltrim(rtrim(@SLICE))
					else if @LoopCount = 22
						set @subsrc = ltrim(rtrim(@SLICE))

					SELECT @FilterString = RIGHT(@FilterString,LEN(@FilterString) - @INDEX)             
		            
					IF LEN(@FilterString) = 0     
						BREAK         
				END    
		
			--select 	@CategoryIDs ,@TransactionIDs ,@QsourceIDs ,@StateIDs ,@CountryIDs ,@Email ,@Phone ,@Fax ,@Demo7,@Demo31,@Demo32, @Demo33, @Demo34, @Demo35,  @Demo36, @Qfrom, @Qto, @Year
		
			if len(@CategoryIDs) > 0
				set @executeString = @executeString + ' and CategoryGroup_ID in ( ' + @CategoryIDs + ')'  

			if len(@CategoryCodes) > 0
				set @executeString = @executeString + ' and s.Category_ID in ( ' + @CategoryCodes + ')'  
	 
			if len(@TransactionIDs) > 0
				set @executeString = @executeString + ' and TransactionGroup_ID in ( ' + @TransactionIDs + ')'
		
			if len(@QsourceIDs) > 0
				set @executeString = @executeString + ' and QsourceGroup_ID in ( ' + @QsourceIDs + ')' 

			if len(@StateIDs) > 0
				Begin
					insert into #State select items from dbo.fn_split(@StateIDs, ',')
					set @executeString = @executeString + ' and state in (select state from #State)'
				end

	--		if len(@CountryIDs) > 0
	--		Begin
	--			insert into #country select items from dbo.fn_split(@CountryIDs, ',')
	--			set @executeString = @executeString + ' and s.Country_ID in (select Country_ID from #country)'
	--		end

			if len(@CountryIDs) > 0
				Begin
					insert into #country select items from dbo.fn_split(@CountryIDs, ',')

					--(1, Only US 
					--(2, Only Canada 
					--(3, US and Canada 
					--(4, International 

					if exists (select * from #country where country_ID = 1)
						set @executeString = @executeString + ' and isnull(s.Country_ID,0) = 0 '
					else if exists (select * from #country where country_ID = 2)
						set @executeString = @executeString + ' and s.Country_ID = 2 '
					else if exists (select * from #country where country_ID = 3)
						set @executeString = @executeString + ' and isnull(s.Country_ID,0) in (0,2) '
					else if exists (select * from #country where country_ID = 4)
						set @executeString = @executeString + ' and isnull(s.Country_ID,0) not in (0,2) '
					else
						set @executeString = @executeString + ' and s.Country_ID in (select Country_ID from #country)'

				end

			if len(@Email) > 0
				Begin
					if @Email = 1
						set @executeString = @executeString + ' and Isnull(ltrim(rtrim(Emailaddress)),'''') <> ''''' 
					else
						set @executeString = @executeString + ' and Isnull(ltrim(rtrim(Emailaddress)),'''') = ''''' 
				End

			if len(@Phone) > 0
				Begin
					if @Phone = 1
						set @executeString = @executeString + ' and Isnull(ltrim(rtrim(PHONE)),'''') <> ''''' 
					else
						set @executeString = @executeString + ' and Isnull(ltrim(rtrim(PHONE)),'''') = ''''' 
				End

			if len(@Fax) > 0
				Begin
					if @Fax = 1
						set @executeString = @executeString + ' and Isnull(ltrim(rtrim(Fax)),'''') <> ''''' 
					else
						set @executeString = @executeString + ' and Isnull(ltrim(rtrim(Fax)),'''') = ''''' 
				End

			if len(@Demo7) > 0
				set @executeString = @executeString + ' and Isnull(Demo7,'''') = ''' + @Demo7 + ''''

			if len(@Demo31) > 0
				set @executeString = @executeString + ' and Isnull(Demo31,1) = ' + @Demo31

			if len(@Demo32) > 0
				set @executeString = @executeString + ' and Isnull(Demo32,1) = ' + @Demo32

			if len(@Demo33) > 0
				set @executeString = @executeString + ' and Isnull(Demo33,1) = ' + @Demo33

			if len(@Demo34) > 0
				set @executeString = @executeString + ' and Isnull(Demo34,1) = ' + @Demo34

			if len(@Demo35) > 0
				set @executeString = @executeString + ' and Isnull(Demo35,1) = ' + @Demo35

			if len(@Demo36) > 0
				set @executeString = @executeString + ' and Isnull(Demo36,1) = ' + @Demo36

			if len(@QFrom) > 0
				set @executeString = @executeString + ' and QualificationDate >= ''' + @QFrom + ''''

			if len(@QTo) > 0
				set @executeString = @executeString + ' and QualificationDate <= ''' + @QTo + ''''

	--		if len(@Year) > 0
	--		Begin
	--			
	--			select @startDate = Year_StartDt , @endDate = Year_EndDt from Magazines where MagazineID = @MagazineID		
	--
	--			set @executeString = @executeString + ' and QualificationDate >= ''' + @startDate + '/' + Convert(varchar,Convert(int,@Year-1))  + ''''
	--			set @executeString = @executeString + ' and QualificationDate <= ''' + @endDate + '/' +  @Year + ''''
	--		End

			if len(@Year) > 0
				Begin

					--select @startDate = Year_StartDt , @endDate = Year_EndDt from Magazines where MagazineID = @MagazineID		
					set @startDate = '01/01'
					set @endDate = '12/31'
					if getdate() > convert(datetime,@startDate + '/' + convert(varchar,year(getdate())))
						set @currentYear = year(getdate()) 
					else
						set @currentYear = year(getdate()) - 1

					set @executeString = @executeString + ' and QualificationDate >= ''' + @startDate + '/' + Convert(varchar,Convert(int,@currentYear-@Year))  + ''''
					set @executeString = @executeString + ' and QualificationDate <= ''' + @endDate + '/' +  Convert(varchar,Convert(int,@currentYear-@Year+1)) + ''''
				End

			if len(@AdhocColumn) > 0 and len(@AdhocValue) > 0 
				Begin
					set @executeString = @executeString + ' and s.' + @AdhocColumn +  ' like ''' + @AdhocValue + '%'''
				End
		
			if len(@subsrc) > 0
				set @executeString = @executeString + ' and Subsrc = ''' + @subsrc + ''''



			exec(@executeString)
			--print (@executeString)
			drop table #State
			drop table #country
		end
	else
		Begin
			Select s.SubscriptionID, s.CGRP_NO
			from Subscriptions s 
	--join
	--				Category dc on s.Category_ID = dc.Category_ID join
	--				[Transaction] dt on s.Transaction_ID = dt.Transaction_ID left outer join 
	--				QSource dq on s.Qsource_ID = dq.Qsource_ID 
		end
END