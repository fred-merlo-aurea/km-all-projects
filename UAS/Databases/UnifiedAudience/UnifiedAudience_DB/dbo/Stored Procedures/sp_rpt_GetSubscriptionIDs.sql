CREATE Proc [dbo].[sp_rpt_GetSubscriptionIDs]
(  
	--@Test int
	@PublicationID int,
	@CategoryIDs varchar(800),
	@CategoryCodes varchar(800),
	@TransactionIDs varchar(800),
	@TransactionCodes varchar(800),
	@QsourceIDs varchar(800),
	@StateIDs varchar(800),
	@Regions varchar(max),
	@CountryIDs varchar(800),
	@Email varchar(10),
	@Phone varchar(10),
	@Fax varchar(10),
	@ResponseIDs varchar(800),
	@Demo7 varchar(10),		
	@Year varchar(20),
	@startDate varchar(10),		
	@endDate varchar(10),
	@AdHocXML varchar(8000),
	@GetSubscriberIDs bit
) AS

BEGIN

	SET NOCOUNT ON


	DECLARE	@executeString varchar(8000)
	DECLARE @currentYear int
	DECLARE @tempStartDate varchar(10)
	DECLARE @tempEndDate varchar(10)
	DECLARE @years table (value varchar(4))
	DECLARE @tempYear varchar(20)

	CREATE TABLE #Areas
	(
		[Value] varchar(40)
	)

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

	DECLARE @docHandle int
	--DECLARE @AdHocXML varchar(8000)

	--set @AdHocXML = '<XML><AdHocFilters.AdHocFilterField><FilterObject>FirstName</FilterObject><FromValue></FromValue><SelectedCondition>CONTAINS</SelectedCondition><ToValue></ToValue><Type>Standard</Type><Value>Gabriel</Value></AdHocFilters.AdHocFilterField>\r\n<AdHocFilters.AdHocFilterField  ><FilterObject>LastName</FilterObject><FromValue></FromValue><SelectedCondition>CONTAINS</SelectedCondition><ToValue></ToValue><Type>Standard</Type><Value>Santiago</Value></AdHocFilters.AdHocFilterField>\r\n<AdHocFilters.AdHocFilterField><FilterObject>StartIssueDate</FilterObject><FromValue>11/01/2011</FromValue><SelectedCondition>DateRange</SelectedCondition><ToValue>11/01/2014</ToValue><Type>DateRange</Type><Value></Value></AdHocFilters.AdHocFilterField></XML>'

	EXEC sp_xml_preparedocument @docHandle OUTPUT, @AdHocXML  
	INSERT INTO #AdHoc 
	(
		 [FilterObject],[SelectedCondition],[Type],[Value],[ToValue],[FromValue]
	)  
	SELECT [FilterObject],[SelectedCondition],[Type],[Value],[ToValue],[FromValue]
	FROM OPENXML(@docHandle,N'/XML/FilterDetail')
	WITH
	(
		[FilterObject] nvarchar(256) 'FilterField',
		[SelectedCondition] nvarchar(256) 'SearchCondition',
		[Type] nvarchar(256) 'FilterObjectType',
		[Value] nvarchar(265) 'AdHocFieldValue',
		[ToValue] nvarchar(256) 'AdHocToField',
		[FromValue] nvarchar(256) 'AdHocFromField'
	)

	EXEC sp_xml_removedocument @docHandle
	
	--DECLARE @PublicationID int
	--DECLARE @CategoryIDs varchar(800)
	--DECLARE @CategoryCodes varchar(800)
	--DECLARE @TransactionIDs varchar(800)
	--DECLARE @TransactionCodes varchar(800)
	--DECLARE @QsourceIDs varchar(800)
	--DECLARE @StateIDs varchar(800)
	--DECLARE @Regions varchar(max)
	--DECLARE @CountryIDs varchar(800)
	--DECLARE @Email varchar(10)
	--DECLARE @Phone varchar(10)
	--DECLARE @Fax varchar(10)
	--DECLARE @Year varchar(800)
	--DECLARE @startDate varchar(10)	
	--DECLARE @endDate varchar(10)
	--DECLARE @ResponseIDs varchar(800)
	--DECLARE @DEMO7 varchar(800)
	--DECLARE @GetSubscriberIDs bit
	--set @PublicationID = 113
	--set @TransactionCodes = '9'
	--set @GetSubscriberIDs = 0
	----set @Regions = 'America Central, Asia Pacific'
	----set @CountryIDs = '1,2'
	----set @CategoryIDs = '1,3'
	----set @CategoryCodes = '1,2,3,4,5,6'
	----set @TransactionCodes = '1,2,3,4,5,6,7,10,11,12'
	----set @TransactionIDs = '1,3'
	----set @QsourceIDs = '1,3,4'
	----set @StateIDs = '1,2,3,5,6'
	----set @Email = 'Yes'
	----set @Phone = 'Yes'
	----set @Fax = 'No'
	----set @Year = '1,2,3,4,5'
	----set @ResponseIDs = '35819'

	if(@GetSubscriberIDs = 0)
		set @executeString =
		'Select	sp.SubscriptionID as Count
		FROM	Subscriptions s JOIN
				PubSubscriptions sp ON s.SubscriberID = sp.SubscriberID LEFT OUTER JOIN 
				SubscriptionPaid spp ON sp.SubscriptionID = spp.SubscriptionID JOIN
				UAS..Action a ON a.ActionID = sp.ActionID_Current JOIN
				UAS..CategoryCode dc ON a.CategoryCodeID = dc.CategoryCodeID JOIN
				UAS..TransactionCode dt on a.TransactionCodeID = dt.TransactionCodeID JOIN
				UAS..TransactionCodeType tct ON dt.TransactionCodeTypeID = tct.TransactionCodeTypeID JOIN
				UAS..CategoryCodeType cct ON cct.CategoryCodeTypeID = dc.CategoryCodeTypeID LEFT OUTER JOIN
				UAS..QualificationSource dq on sp.QSourceID = dq.QSourceID LEFT OUTER JOIN
				UAS..Country c ON c.CountryID = s.CountryID
		Where	
				sp.PublicationID = ' + Convert(varchar,@PublicationID) 
	else if(@GetSubscriberIDs = 1)
		set @executeString =
		'Select	sp.SubscriberID as Count
		FROM	Subscriptions s JOIN
				PubSubscriptions sp ON s.SubscriberID = sp.SubscriberID LEFT OUTER JOIN 
				SubscriptionPaid spp ON sp.SubscriptionID = spp.SubscriptionID JOIN
				UAS..Action a ON a.ActionID = sp.ActionID_Current JOIN
				UAS..CategoryCode dc ON a.CategoryCodeID = dc.CategoryCodeID JOIN
				UAS..TransactionCode dt on a.TransactionCodeID = dt.TransactionCodeID JOIN
				UAS..TransactionCodeType tct ON dt.TransactionCodeTypeID = tct.TransactionCodeTypeID JOIN
				UAS..CategoryCodeType cct ON cct.CategoryCodeTypeID = dc.CategoryCodeTypeID LEFT OUTER JOIN
				UAS..QualificationSource dq on sp.QSourceID = dq.QSourceID LEFT OUTER JOIN
				UAS..Country c ON c.CountryID = s.CountryID
		Where	
				sp.PublicationID = ' + Convert(varchar,@PublicationID) 
		
	Begin --Check passed Parameters

		if len(@CategoryIDs) > 0
					set @executeString = @executeString + ' and cct.CategoryCodeTypeID in (' + @CategoryIDs +')'
				
		if len(@CategoryCodes) > 0
					set @executeString = @executeString + ' and a.CategoryCodeID in (' + @CategoryCodes +')'
				
		if len(@TransactionCodes) > 0
					set @executeString = @executeString + ' and a.TransactionCodeID in (' + @TransactionCodes +')'
				
		if len(@TransactionIDs) > 0
					set @executeString = @executeString + ' and tct.TransactionCodeTypeID in (' + @TransactionIDs +')'
				
		if len(@QsourceIDs) > 0
					set @executeString = @executeString + ' and sp.QSourceID in (' + @QsourceIDs +')'
				
		if len(@StateIDs) > 0
					set @executeString = @executeString + ' and s.RegionID in (' + @StateIDs +')'
				
		if LEN(@Regions) > 0
			BEGIN
					INSERT INTO #Areas
					select * 
					FROM dbo.fn_Split(@Regions, ',');
					set @executeString = @executeString + ' and s.CountryID in ( Select CountryID FROM UAS..Country WHERE Area IN ( select * from #areas ))'
			END
				
		if LEN(@CountryIDs) > 0
					set @executeString = @executeString + ' and s.CountryID in (' + @CountryIDs + ')'
				
		if len(@Email) > 0
			Begin
				set @Email = (CASE WHEN @Email='0' THEN 1 ELSE 0 END)
				if @Email = 1
					set @executeString = @executeString + ' and Isnull(ltrim(rtrim(s.Email)),'''') <> ''''' 
				else
					set @executeString = @executeString + ' and Isnull(ltrim(rtrim(s.Email)),'''') = ''''' 
			End

		if len(@Phone) > 0
			Begin
				set @Phone = (CASE WHEN @Phone='0' THEN 1 ELSE 0 END)
				if @Phone = 1
					set @executeString = @executeString + ' and Isnull(ltrim(rtrim(s.Phone)),'''') <> ''''' 
				else
					set @executeString = @executeString + ' and Isnull(ltrim(rtrim(s.Phone)),'''') = ''''' 
			End

		if len(@Fax) > 0
			Begin
				set @Fax = (CASE WHEN @Fax='0' THEN 1 ELSE 0 END)
				if @Fax = 1
					set @executeString = @executeString + ' and Isnull(ltrim(rtrim(s.Fax)),'''') <> ''''' 
				else
					set @executeString = @executeString + ' and Isnull(ltrim(rtrim(s.Fax)),'''') = ''''' 
			End
	
		if len(@startDate) > 0
				set @executeString = @executeString + ' and sp.QSourceDate >= ''' + @startDate + ''''

		if len(@endDate) > 0
				set @executeString = @executeString + ' and sp.QSourceDate <= ''' + @endDate + ''''	
				
		if len(@Year) > 0
			Begin
				insert into @years
				select * 
				FROM dbo.fn_Split(@Year, ',');
			
				set @tempYear = (SELECT TOP 1 * FROM @years)
			
				select @tempStartDate = YearStartDate , @tempEndDate = YearEndDate 
				from Publication 
				where PublicationID = @PublicationID	
			
				DECLARE My_Cursor CURSOR FOR
				select value 
				FROM ( Select y.value, row_number() over (order by value ASC) rn FROM @years y) src 
				WHERE rn > 1

				if getdate() > convert(datetime,@tempStartDate + '/' + convert(varchar,year(getdate())))
					set @currentYear = year(getdate()) 
				else
					set @currentYear = year(getdate()) - 1

				set @executeString = @executeString + ' and (sp.QSourceDate >= ''' + @tempStartDate + '/' + Convert(varchar(10),(convert(int,@currentYear-@tempYear)))  + ''''
				set @executeString = @executeString + ' and sp.QSourceDate <= ''' + @tempEndDate + '/' +  Convert(varchar(10),(convert(int,@currentYear-@tempYear+1))) + ''''
			
				OPEN My_Cursor
			
				FETCH NEXT FROM My_Cursor
				INTO @tempYear
				WHILE @@FETCH_STATUS = 0
				BEGIN
					set @executeString = @executeString + ' or (sp.QSourceDate >= ''' + @tempStartDate + '/' + Convert(varchar(10),(convert(int,@currentYear-@tempYear)))  + ''''
					set @executeString = @executeString + ' and sp.QSourceDate <= ''' + @tempEndDate + '/' +  Convert(varchar(10),(convert(int,@currentYear-@tempYear+1))) + '''' + ')'
					FETCH NEXT FROM My_Cursor
					INTO @tempYear
				END
				CLOSE My_Cursor
				DEALLOCATE My_Cursor
				set @executeString = @executeString + ')'
			End
	
		if LEN(@Demo7) > 0
				set @executeString = @executeString + ' and sp.DeliverabilityID in (SELECT CodeId from UAS..Code d with (NOLOCK) WHERE d.CodeValue in (' + @Demo7 + ') and CodeTypeId in (select CodeTypeId from UAS..CodeType where CodeTypeName = ''Deliver'') )'	
		
		if LEN(@ResponseIDs) > 0
			set @executeString = @executeString + ' and sp.SubscriptionID in (SELECT SubscriptionID from SubscriptionResponseMap srm with (NOLOCK) WHERE  srm.ResponseID in (' + @ResponseIDs +'))'
	
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
											WHEN 'Equal' THEN '( s.' + @Column  + ' = '''+ REPLACE(@Value, ',', ''' or s.' + @Column + ' =  ''')+ ''') ' 
											WHEN 'Contains' THEN '( s.' + @Column  + ' like ''%'+ REPLACE(@Value, ',', '%'' or s.' + @Column + ' like  ''%')+ '%'') ' 
											WHEN 'Start With' THEN '( s.' + @Column  + ' like '''+ REPLACE(@Value, ',', '%'' or s.' + @Column + ' like  ''')+ '%'') '
											WHEN 'End With' THEN '( s.' + @Column  + ' like ''%'+ REPLACE(@Value, ',', ''' or s.' + @Column + ' like  ''%')+ ''') '
											WHEN 'Does Not Contain' THEN '( s.' + @Column  + ' not like ''%'+ REPLACE(@Value, ',', '%'' or s.' + @Column + ' not like  ''%')+ '%'' or ' +  @Column + ' is null ) '
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
														WHEN 'DateRange' THEN case when @ValueTo = null then 's.' + @Column + ' >= ''' + @ValueFrom + '''' else 's.' + @Column + ' >= ''' + @ValueFrom + ''' and s.' + @Column + ' <= ''' + @ValueTo + ''''  END
														WHEN 'Year' THEN case when @ValueTo = null then 'year(s.' + @Column + ') >= ''' + @ValueFrom + '''' else 'year(s.' + @Column + ') >= ''' + @ValueFrom + ''' and year(s.' + @Column + ') <= ''' + @ValueTo + ''''  END
														WHEN 'Month' THEN case when @ValueTo = null then 'month(s.' + @Column + ') >= ''' + @ValueFrom + '''' else 'moonth(s.' + @Column + ') >= ''' + @ValueFrom + ''' and month(s.' + @Column + ') <= ''' + @ValueTo + ''''  END
												 END 
										 end				 
								 end		
				 
							if(@DataType = 'Range') 
								begin
									 set @AdhocString = 
										 CASE  @Condition
												WHEN 'Range' THEN case when @ValueTo = null then 'year(s.' + @Column + ') >= ''' + @ValueFrom + '''' else 'year(s.' + @Column + ') >= ''' + @ValueFrom + ''' and year(s.' + @Column + ') <= ''' + @ValueTo + ''''  END
												WHEN 'Equal' THEN 's.' + @Column + ' = ' + @Value 
												WHEN 'Greater Than' THEN 's.' + @Column + ' >= ' + @Value 
												WHEN 'Lesser Than' THEN 's.' + @Column + ' <= ' + @Value 
										 END 
								end	 
				 
						if @AdhocString != ''
							set @AdHocFinal = @AdHocFinal + ' and ' + @AdhocString
					
						SET @RowCount = @RowCount + 1
				
						END
				
				set @executeString = @executeString + @AdHocFinal
			END
			
	END

	PRINT(@executeString)
		
	EXEC(@executeString)

	DROP TABLE #AdHoc
	DROP TABLE #Areas

END
GO