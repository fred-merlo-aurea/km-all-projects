CREATE Proc [dbo].[e_IssueCompDetail_GetFromFilter]
(  
	@FilterString varchar(MAX) = '<XML><Filters></Filters></XML>',
	@AdHocXML varchar(MAX) = '<XML><FilterDetail></FilterDetail></XML>',
	@IncludeAddRemove bit = 0,
	@IssueCompID int = 0
) AS

BEGIN

	SET NOCOUNT ON

	Declare @ProductID int = 0,
	@CategoryTypes varchar(800) = '',
	@CategoryCodes varchar(800) = '',
	@TransactionTypes varchar(800) = '',
	@TransactionCodes varchar(800) = '',
	@QsourceIDs varchar(800) = '',
	@Par3C varchar(800) = '',
	@StateIDs varchar(800) = '',
	@CountryIDs varchar(1500) = '',
	@Email varchar(10) = '',
	@Phone varchar(10) = '',
	@Fax varchar(10) = '',
	@Mobile varchar(10) = '',
	@ResponseIDs varchar(800) = '',
	@Demo7 varchar(10) = '',		
	@Year varchar(20) = '',
	@StartDate varchar(10) = '',		
	@EndDate varchar(10) = '',
	@WaveMail varchar(30) = ''

	BEGIN --Parse Filters--
		DECLARE @docHandle int

		EXEC sp_xml_preparedocument @docHandle OUTPUT, @FilterString  
	
		select @ProductID = value
		FROM OPENXML(@docHandle, N'/XML/Filters/ProductID')  
		WITH (value int '.') 
	
		select @CategoryTypes = value
		FROM OPENXML(@docHandle, N'/XML/Filters/CategoryType')  
		WITH (value varchar(800) '.') 

		select @CategoryCodes = value
		FROM OPENXML(@docHandle, N'/XML/Filters/CategoryCode')    	
		WITH (value varchar(800) '.')
		WHERE ISNULL(value,'0') <> '0'

		select @TransactionTypes = value
		FROM OPENXML(@docHandle, N'/XML/Filters/TransactionType')   
		WITH (value varchar(800) '.')

		select @TransactionCodes = value
		FROM OPENXML(@docHandle, N'/XML/Filters/TransactionCode')   
		WITH (value varchar(800) '.')

		select @QsourceIDs = value
		FROM OPENXML(@docHandle, N'/XML/Filters/QsourceIDs')   
		WITH (value varchar(800) '.')

		select @StateIDs = value
		FROM OPENXML(@docHandle, N'/XML/Filters/StateIDs')   
		WITH (value varchar(800) '.')

		select @CountryIDs = value
		FROM OPENXML(@docHandle, N'/XML/Filters/CountryIDs')   	
		WITH (value varchar(1500) '.')

		select @Email = value
		FROM OPENXML(@docHandle, N'/XML/Filters/Email')
		WITH (value varchar(800) '.')   

		select @Phone = value
		FROM OPENXML(@docHandle, N'/XML/Filters/Phone')   
		WITH (value varchar(800) '.')

		select @Fax = value
		FROM OPENXML(@docHandle, N'/XML/Filters/Fax')
		WITH (value varchar(800) '.')   

		select @Demo7 = value
		FROM OPENXML(@docHandle, N'/XML/Filters/Demo7')   
		WITH (value varchar(800) '.')
		WHERE ISNULL(value, '0') <> '0'

		select @StartDate = value
		FROM OPENXML(@docHandle, N'/XML/Filters/StartDate')   
		WITH (value varchar(800) '.')

		select  @EndDate = value
		FROM OPENXML(@docHandle, N'/XML/Filters/EndDate')   
		WITH (value varchar(800) '.')

		select @Year = value
		FROM OPENXML(@docHandle, N'/XML/Filters/Year')   
		WITH (value varchar(800) '.')
	
		select @ResponseIDs = value
		FROM OPENXML(@docHandle, N'/XML/Filters/Responses') 
		WITH (value varchar(800) '.')
	
		select @Par3C = value
		FROM OPENXML(@docHandle, N'/XML/Filters/Par3C') 
		WITH (value varchar(800) '.')
	
		select @WaveMail = value
		FROM OPENXML(@docHandle, N'/XML/Filters/WaveMail') 
		WITH (value varchar(800) '.')

		EXEC sp_xml_removedocument @docHandle
	END

	DECLARE	@executeString varchar(8000)
	DECLARE @currentYear int
	DECLARE @tempStartDate varchar(10)
	DECLARE @tempEndDate varchar(10)
	DECLARE @subResponses table (value varchar(10))
	DECLARE @tempYear varchar(20)

	CREATE TABLE #AdHoc
	(  
		RowID int IDENTITY(1, 1)
	  ,[FilterObject] nvarchar(256)
	  ,[SelectedCondition] nvarchar(256)
	  ,[Type] nvarchar(256)
	  ,[Value] nvarchar(256)
	  ,[ToValue] nvarchar(256)
	  ,[FromValue] nvarchar(256)
	)

	DECLARE @docHandle2 int

	EXEC sp_xml_preparedocument @docHandle2 OUTPUT, @AdHocXML  
	INSERT INTO #AdHoc 
	(
		 [FilterObject],[SelectedCondition],[Type],[Value],[ToValue],[FromValue]
	)  
	SELECT [FilterObject],[SelectedCondition],[Type],[Value],[ToValue],[FromValue]
	FROM OPENXML(@docHandle2,N'/XML/FilterDetail')
	WITH
	(
		[FilterObject] nvarchar(256) 'FilterField',
		[SelectedCondition] nvarchar(256) 'SearchCondition',
		[Type] nvarchar(256) 'FilterObjectType',
		[Value] nvarchar(265) 'AdHocFieldValue',
		[ToValue] nvarchar(256) 'AdHocToField',
		[FromValue] nvarchar(256) 'AdHocFromField'
	)

	EXEC sp_xml_removedocument @docHandle2
		
	set @executeString = 'Select DISTINCT ps.IssueCompDetailId as Count FROM IssueCompDetail ps with(nolock) '

	set @executeString = @executeString + ' LEFT OUTER JOIN UAD_Lookup..Country c with(nolock) ON c.CountryID = ps.CountryID Where	 ps.PubID =' +  @ProductID
		
		
	Begin --Check passed Parameters
				
		if len(@CategoryCodes) > 0
			set @executeString = @executeString + ' and ps.PubCategoryID in (' + @CategoryCodes +')'
				
		if len(@TransactionCodes) > 0
			set @executeString = @executeString + ' and ps.PubTransactionID in (' + @TransactionCodes +')'
				
		if len(@QsourceIDs) > 0
			set @executeString = @executeString + ' and ps.PubQSourceID in (' + @QsourceIDs +')'
				
		if len(@StateIDs) > 0
			set @executeString = @executeString + ' and ps.RegionCode in (''' + @StateIDs +''')'	
				
		if LEN(@CountryIDs) > 0
			set @executeString = @executeString + ' and ps.CountryID in (' + @CountryIDs + ')'
				
		if len(@Email) > 0
			Begin
				set @Email = (CASE WHEN @Email='Yes' THEN 1 ELSE 0 END)
				if @Email = 1
					set @executeString = @executeString + ' and Isnull(ltrim(rtrim(ps.Email)),'''') <> ''''' 
				else
					set @executeString = @executeString + ' and Isnull(ltrim(rtrim(ps.Email)),'''') = ''''' 
			End

		if len(@Phone) > 0
			Begin
				set @Phone = (CASE WHEN @Phone='Yes' THEN 1 ELSE 0 END)
				if @Phone = 1
					set @executeString = @executeString + ' and Isnull(ltrim(rtrim(ps.Phone)),'''') <> ''''' 
				else
					set @executeString = @executeString + ' and Isnull(ltrim(rtrim(ps.Phone)),'''') = ''''' 
			End
		
		if len(@Mobile) > 0
			Begin
				set @Mobile = (CASE WHEN @Mobile='Yes' THEN 1 ELSE 0 END)
				if @Mobile = 1
					set @executeString = @executeString + ' and Isnull(ltrim(rtrim(ps.Mobile)),'''') <> ''''' 
				else
					set @executeString = @executeString + ' and Isnull(ltrim(rtrim(ps.Mobile)),'''') = ''''' 
			End

		if len(@Fax) > 0
			Begin
				set @Fax = (CASE WHEN @Fax='Yes' THEN 1 ELSE 0 END)
				if @Fax = 1
					set @executeString = @executeString + ' and Isnull(ltrim(rtrim(ps.Fax)),'''') <> ''''' 
				else
					set @executeString = @executeString + ' and Isnull(ltrim(rtrim(ps.Fax)),'''') = ''''' 
			End
	
		if len(@startDate) > 0
			set @executeString = @executeString + ' and ps.Qualificationdate >= ''' + @startDate + ''''

		if len(@endDate) > 0
			set @executeString = @executeString + ' and ps.Qualificationdate <= ''' + @endDate + ''''	
				
		if len(@Year) > 0 and CHARINDEX('0', @year) = 0
			Begin
		
				declare @Yearstring varchar(max)
				set @Yearstring= ''

				select @tempStartDate = ltrim(rtrim(YearStartDate)) , @tempEndDate = ltrim(rtrim(YearEndDate)) from Pubs with(nolock) where PubID = @ProductID		

				if getdate() > convert(datetime,@tempStartDate + '/' + convert(varchar,year(getdate())))
					set @currentYear = year(getdate()) 
				else
					set @currentYear = year(getdate()) - 1		
					
				declare @startdateTemp datetime,
						@endDateTemp datetime
				
				select @startdateTemp  = @tempStartDate + '/' + convert(varchar,@currentYear)
				select @endDateTemp =  dateadd(ss, -1, dateadd(yy, 1, @startdateTemp) ) 

		
				if CHARINDEX('1', @year) > 0
					set @Yearstring = @Yearstring + ' ps.Qualificationdate between ''' + convert(varchar(20),@startdateTemp,111) + ''' and ''' + convert(varchar(20),@endDateTemp,111)  + ' 23:59:59'' '

				if CHARINDEX('2', @year) > 0
					set @Yearstring = @Yearstring  +  (Case when len(@Yearstring) > 0 then 'or' else '' end) +' ps.Qualificationdate between ''' + convert(varchar(20),dateadd(yy, -1, @startdateTemp),111) + ''' and ''' + convert(varchar(20),dateadd(yy, -1,  @endDateTemp),111)  + ' 23:59:59''  '

				if CHARINDEX('3', @year) > 0
					set @Yearstring = @Yearstring +  (Case when len(@Yearstring) > 0 then 'or' else '' end) +' ps.Qualificationdate between ''' + convert(varchar(20),dateadd(yy, -2, @startdateTemp),111) + ''' and ''' + convert(varchar(20),dateadd(yy, -2,  @endDateTemp),111)  + ' 23:59:59'' '

				if CHARINDEX('4', @year) > 0
					set @Yearstring = @Yearstring +  (Case when len(@Yearstring) > 0 then 'or' else '' end) +' ps.Qualificationdate between ''' + convert(varchar(20),dateadd(yy, -3, @startdateTemp),111) + ''' and ''' + convert(varchar(20),dateadd(yy, -3,  @endDateTemp),111) + ' 23:59:59'' '

				if CHARINDEX('5', @year) > 0
					set @Yearstring = @Yearstring +  (Case when len(@Yearstring) > 0 then 'or' else '' end) +' ps.Qualificationdate < ''' + convert(varchar(20),dateadd(yy, -4,  @endDateTemp ),111) + ' 23:59:59'' '


				if Len(@yearstring) > 0
					set @executeString = @executeString + ' and (' + @Yearstring + ') '  
			End
	
		if LEN(@Demo7) > 0
			set @executeString = @executeString + ' and ps.Demo7 in (SELECT CodeValue from UAD_Lookup..Code c with (NOLOCK) WHERE c.CodeId in (' + @Demo7 + '))'
				
		--ADHOC PROCESSING--
		if LEN(@AdHocXML) > 0
			BEGIN
				declare @Column varchar(100),
						@Value varchar(100), 
						@ValueFrom varchar(100), 
						@ValueTo varchar(100), 
						@DataType varchar(100), 
						@Condition varchar(100)
				 
				DECLARE @NumberRecords int, @RowCount int, @AdHocString varchar(100), @AdHocFinal varchar(8000)
				set @NumberRecords = 0
				set @AdHocString = ''
				set @AdHocFinal = ''
				
				SELECT @NumberRecords = COUNT(*) from #Adhoc
					SET @RowCount = 1
					WHILE @RowCount <= @NumberRecords
					BEGIN
						 set @AdhocString = '';
						 SELECT @Column = FilterObject,
								@Value = Value,
								@ValueFrom = FromValue,
								@ValueTo = ToValue,
								@DataType = Type,
								@Condition = SelectedCondition
						FROM #AdHoc
						WHERE RowID = @RowCount
					
						 if(@DataType = 'Standard') 
							begin
								while(PATINDEX ('% ,%',@Value)>0 or PATINDEX ('%, %',@Value)>0 )
									set @Value = LTRIM(RTRIM(REPLACE(replace(@Value,' ,',','),', ',',')))
						 
								set @AdhocString =  
									CASE  @Condition
										WHEN 'Equal' THEN '( ps.' + @Column  + ' = '''+ REPLACE(@Value, ',', ''' or ps.' + @Column + ' =  ''')+ ''') ' 
										WHEN 'Contains' THEN '( ps.' + @Column  + ' like ''%'+ REPLACE(@Value, ',', '%'' or ps.' + @Column + ' like  ''%')+ '%'') ' 
										WHEN 'Start With' THEN '( ps.' + @Column  + ' like '''+ REPLACE(@Value, ',', '%'' or ps.' + @Column + ' like  ''')+ '%'') '
										WHEN 'End With' THEN '( ps.' + @Column  + ' like ''%'+ REPLACE(@Value, ',', ''' or ps.' + @Column + ' like  ''%')+ ''') '
										WHEN 'Does Not Contain' THEN '( ps.' + @Column  + ' not like ''%'+ REPLACE(@Value, ',', '%'' or ps.' + @Column + ' not like  ''%')+ '%'' or ' +  @Column + ' is null ) '
									END 
							 end			 

						 if(@DataType = 'DateRange') 
							 begin
								 if(@Column = 'STARTISSUEDATE' or  @Column = 'EXPIREISSUEDATE' or @Column = 'PAIDDATE')
									 Begin
										 set @AdhocString = 
											 CASE  @Condition
													WHEN 'DateRange' THEN case when @ValueTo = null then 'spp.' + @Column + ' >= ''' + @ValueFrom + '''' else 'spp.' + @Column + ' >= ''' + @ValueFrom + ''' and spp.' + @Column + ' <= ''' + @ValueTo + ''''  END
													WHEN 'Year' THEN case when @ValueTo = null then 'year(spp.' + @Column + ') >= ''' + @ValueFrom + '''' else 'year(spp.' + @Column + ') >= ''' + @ValueFrom + ''' and year(spp.' + @Column + ') <= ''' + @ValueTo + ''''  END
													WHEN 'Month' THEN case when @ValueTo = null then 'month(spp.' + @Column + ') >= ''' + @ValueFrom + '''' else 'moonth(spp.' + @Column + ') >= ''' + @ValueFrom + ''' and month(spp.' + @Column + ') <= ''' + @ValueTo + ''''  END
											 END 
									 end	 
								 else	 
									 Begin
										 set @AdhocString = 
											 CASE  @Condition
													WHEN 'DateRange' THEN case when @ValueTo = null then 'ps.' + @Column + ' >= ''' + @ValueFrom + '''' else 'ps.' + @Column + ' >= ''' + @ValueFrom + ''' and ps.' + @Column + ' <= ''' + @ValueTo + ''''  END
													WHEN 'Year' THEN case when @ValueTo = null then 'year(ps.' + @Column + ') >= ''' + @ValueFrom + '''' else 'year(ps.' + @Column + ') >= ''' + @ValueFrom + ''' and year(ps.' + @Column + ') <= ''' + @ValueTo + ''''  END
													WHEN 'Month' THEN case when @ValueTo = null then 'month(ps.' + @Column + ') >= ''' + @ValueFrom + '''' else 'moonth(ps.' + @Column + ') >= ''' + @ValueFrom + ''' and month(ps.' + @Column + ') <= ''' + @ValueTo + ''''  END
											 END 
									 end				 
							 end		
				 
						if(@DataType = 'Range') 
							begin
								 set @AdhocString = 
									 CASE  @Condition
											WHEN 'Range' THEN case when @ValueTo = null then '(ps.' + @Column + ') >= ' + @ValueFrom else '(ps.' + @Column + ') >= ' + @ValueFrom + ' and (ps.' + @Column + ') <= ' + @ValueTo  END
											WHEN 'Equal' THEN 'ps.' + @Column + ' = ' + @ValueFrom 
											WHEN 'Greater Than' THEN 'ps.' + @Column + ' > ' + @ValueFrom 
											WHEN 'Lesser Than' THEN 'ps.' + @Column + ' < ' + @ValueFrom 
									 END 
							end	 
				 
					if @AdhocString != ''
						set @AdHocFinal = @AdHocFinal + ' and ' + @AdhocString
					
					SET @RowCount = @RowCount + 1
				
					END
				
				set @executeString = @executeString + @AdHocFinal
			END
			
	END

	--PRINT(@executeString)
		
	EXEC(@executeString)

	DROP TABLE #AdHoc

END