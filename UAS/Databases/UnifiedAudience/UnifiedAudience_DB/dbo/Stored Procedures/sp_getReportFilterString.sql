CREATE proc [dbo].[sp_getReportFilterString]
(
	@xmlFilter TEXT,
	@executeString varchar(8000) output
)
AS         
BEGIN

	SET NOCOUNT ON     
    
	declare @docHandle int,  
		@PubIDs varchar(800),
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
		@AdhocColumn varchar(100),
		@AdhocValue varchar(100)

	set @executeString = ''

	if len(convert(varchar(100),@xmlFilter)) > 0
		Begin
			EXEC sp_xml_preparedocument @docHandle OUTPUT, @xmlFilter  
		  
			SELECT	@PubIDs = PubIDs,
				@CategoryIDs =CategoryIDs,
				@CategoryCodes=CategoryCodes,
				@TransactionIDs=TransactionIDs,
				@QsourceIDs=QsourceIDs,
				@StateIDs=StateIDs,
				@CountryIDs=CountryIDs,
				@Email=Email,
				@Phone=Phone,
				@Fax=Fax,
				@Demo31=Demo31,
				@Demo32=Demo32,
				@Demo33=Demo33,
				@Demo34=Demo34,
				@Demo35=Demo35,
				@Demo36=Demo36,
				@Qfrom=Qfrom,
				@QTo=QTo,
				@Year=[Year],
				@AdhocColumn=AdhocColumn,
				@AdhocValue=AdhocValue
			FROM OPENXML(@docHandle, N'/filters')   
			WITH   
			(  
				PubIDs varchar(800) 'PubIDs', 
				CategoryIDs varchar(800) 'CategoryIDs', 
				CategoryCodes varchar(800) 'CategoryCodes', 
				TransactionIDs varchar(800) 'TransactionIDs', 
				QsourceIDs varchar(800) 'QsourceIDs',  
				StateIDs varchar(800) 'StateIDs', 
				CountryIDs varchar(800) 'CountryIDs', 
				Email varchar(25) 'Email', 
				Phone varchar(25) 'Phone',  
				Fax varchar(25) 'Fax', 
				Demo31 varchar(1) 'Demo31', 
				Demo32 varchar(1) 'Demo32', 
				Demo33 varchar(1) 'Demo33',  
				Demo34 varchar(1) 'Demo34', 
				Demo35 varchar(1) 'Demo35', 
				Demo36 varchar(1) 'Demo36',
				Qfrom varchar(10) 'Qfrom',
				QTo varchar(150) 'QTo',
				[Year] varchar(4) 'Year',
				AdhocColumn varchar(100) 'AdhocColumn',
				AdhocValue varchar(100) 'AdhocValue'
			)  
			
			--select @CategoryIDs, @CategoryCodes, @TransactionIDs, @QsourceIDs, @StateIDs, @CountryIDs, @Email,@Phone, @Fax, @Demo7, @Demo31, @Demo32, @Demo33, @Demo34, @Demo35, @Demo36, @Qfrom, @QTo, @Year, @AdhocColumn, @AdhocValue
		  
			EXEC sp_xml_removedocument @docHandle    

			if len(@PubIDs) > 0
				Begin
					if CHARINDEX(',',@PubIDs) > 0
						set @executeString = @executeString + ' and s.PubID in ( ' + @PubIDs + ')'  
					else
						set @executeString = @executeString + ' and s.PubID = ' + @PubIDs 
				End

			if len(@CategoryIDs) > 0
				Begin
					if CHARINDEX(',',@CategoryIDs) > 0
					set @executeString = @executeString + ' and CategoryGroupID in ( ' + @CategoryIDs + ')'  
					else
						set @executeString = @executeString + ' and CategoryGroupID = ' + @CategoryIDs 
				End

			if len(@CategoryCodes) > 0
				Begin
					if CHARINDEX(',', @CategoryCodes) > 0
						set @executeString = @executeString + ' and s.CategoryID in ( ' + @CategoryCodes + ')'  
					else
						set @executeString = @executeString + ' and s.CategoryID = ' + @CategoryCodes 
				End
	 
			if len(@TransactionIDs) > 0
				Begin
					if CHARINDEX(',',@TransactionIDs) > 0
						set @executeString = @executeString + ' and TransactionGroupID in ( ' + @TransactionIDs + ')'
					else
						set @executeString = @executeString + ' and TransactionGroupID = ' + @TransactionIDs 
				End
			
			if len(@QsourceIDs) > 0
				Begin
					if CHARINDEX(',',@QsourceIDs) > 0
						set @executeString = @executeString + ' and QsourceGroupID in ( ' + @QsourceIDs + ')' 
					else
						set @executeString = @executeString + ' and QsourceGroupID = ' + @QsourceIDs 
				End




			if len(@StateIDs) > 0
				Begin
					create table #State (state varchar(2)) 
					insert into #State 
						select items 
						from dbo.fn_split(@StateIDs, ',')

					set @executeString = @executeString + ' and s.state in ( '

					-- loop over their selected states and add them
					DECLARE @current_value varchar(2)
					DECLARE State_Loop CURSOR FOR
					select state from #State
					OPEN State_Loop;

					FETCH NEXT FROM State_Loop 
					INTO @current_value
					-- Build a string of these responses
					WHILE @@FETCH_STATUS = 0
						BEGIN
							-- Add the State
							set @executeString = @executeString + '''' + @current_value + ''''

							FETCH NEXT FROM State_Loop 
							INTO @current_value

							-- Don't add the comma to the last entry
							IF @@FETCH_STATUS = 0
								BEGIN
									set @executeString = @executeString + ' , '
								END
						END
					CLOSE State_Loop;
					DEALLOCATE State_Loop;

					set @executeString = @executeString + ' ) '

					drop table #State
				end

	--		if len(@StateIDs) > 0
	--		Begin
	--			set @executeString = @executeString + ' and state in (select state from #State)'
	--		end

			if len(@Email) > 0
				Begin
					if @Email = 1
						set @executeString = @executeString + ' and s.EmailExists = 1 ' 
					else
						set @executeString = @executeString + ' and s.EmailExists = 0 '
				End

			if len(@Phone) > 0
				Begin
					if @Phone = 1
						set @executeString = @executeString + ' and Isnull(s.PHONE,'''') <> ''''' 
					else
						set @executeString = @executeString + ' and Isnull(s.PHONE,'''') = ''''' 
				End

			if len(@Fax) > 0
				Begin
					if @Fax = 1
						set @executeString = @executeString + ' and Isnull(Fax,'''') <> ''''' 
					else
						set @executeString = @executeString + ' and Isnull(Fax,'''') = ''''' 
				End

			if len(@Demo31) > 0
				set @executeString = @executeString + ' and Isnull(s.Demo31,1) = ' + @Demo31

			if len(@Demo32) > 0
				set @executeString = @executeString + ' and Isnull(s.Demo32,1) = ' + @Demo32

			if len(@Demo33) > 0
				set @executeString = @executeString + ' and Isnull(s.Demo33,1) = ' + @Demo33

			if len(@Demo34) > 0
				set @executeString = @executeString + ' and Isnull(s.Demo34,1) = ' + @Demo34

			if len(@Demo35) > 0
				set @executeString = @executeString + ' and Isnull(s.Demo35,1) = ' + @Demo35

			if len(@Demo36) > 0
				set @executeString = @executeString + ' and Isnull(s.Demo36,1) = ' + @Demo36

			if len(@QFrom) > 0
				set @executeString = @executeString + ' and s.QualificationDate >= ''' + @QFrom + ''''

			if len(@QTo) > 0
				set @executeString = @executeString + ' and s.QualificationDate <= ''' + @QTo + ''''

		if len(@CountryIDs) > 0
			Begin
				--insert into #country select items from dbo.fn_split(@CountryIDs, ',')

				--(1, Only US 
				--(2, Only Canada 
				--(3, US and Canada 
				--(4, International 

				if @CountryIDs = '1'
					set @executeString = @executeString + ' and s.CountryID = 1 '
				else if @CountryIDs = '2'
					set @executeString = @executeString + ' and s.CountryID = 2 '
				else if @CountryIDs = '3'
					set @executeString = @executeString + ' and s.CountryID in (1,2) '
				else if @CountryIDs = '4'
					set @executeString = @executeString + ' and s.CountryID not in (1,2) '
				else
					set @executeString = @executeString + ' and s.CountryID in (' + @CountryIDs + ')'

			end

	--		if len(@Year) > 0
	--		Begin
	--
	--			select @startDate = Year_StartDt , @endDate = Year_EndDt from Magazines where MagazineID = @MagazineID		
	--
	--			if getdate() > convert(datetime,@startDate + '/' + convert(varchar,year(getdate())))
	--				set @currentYear = year(getdate()) 
	--			else
	--				set @currentYear = year(getdate()) - 1
	--
	--			set @executeString = @executeString + ' and QualificationDate >= ''' + @startDate + '/' + Convert(varchar,Convert(int,@currentYear-@Year))  + ''''
	--			set @executeString = @executeString + ' and QualificationDate <= ''' + @endDate + '/' +  Convert(varchar,Convert(int,@currentYear-@Year+1)) + ''''
	--		End

			if len(@AdhocColumn) > 0 and len(@AdhocValue ) > 0
				Begin
					set @executeString = @executeString + ' and s.' + @AdhocColumn + ' like ' + '''' + @AdhocValue + '%'''
				End
			
			if len(@executeString) > 0
				Begin
					set @executeString = substring(@executeString, 5, len(@executeString))
				End
		End
END