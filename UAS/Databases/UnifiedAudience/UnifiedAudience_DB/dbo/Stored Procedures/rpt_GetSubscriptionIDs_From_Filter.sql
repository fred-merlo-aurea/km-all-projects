CREATE Proc [dbo].[rpt_GetSubscriptionIDs_From_Filter]
(  
	@PublicationID varchar(5),
	@CategoryIDs varchar(800),
	@CategoryCodes varchar(800),
	@TransactionIDs varchar(800),
	@TransactionCodes varchar(800),
	@QsourceIDs varchar(800),
	@StateIDs varchar(800),
	@CountryIDs varchar(1500),
	@Email varchar(10),
	@Phone varchar(10),
	@Mobile varchar(10),
	@Fax varchar(10),
	@ResponseIDs varchar(1000),
	@Demo7 varchar(50),		
	@Year varchar(20),
	@startDate varchar(10),		
	@endDate varchar(10),
	@AdHocXML varchar(8000),
	@WaveMail varchar(100) = ''
) AS

BEGIN
	
	SET NOCOUNT ON

	DECLARE	@executeString varchar(8000)
	DECLARE @currentYear int
	DECLARE @tempStartDate varchar(10)
	DECLARE @tempEndDate varchar(10)
	DECLARE @subResponses table (value varchar(10))
	DECLARE @responses table (value varchar(10))
	DECLARE @tempYear varchar(20)
	DECLARE @StateCodes varchar(4000)
	SELECT @StateCodes = STUFF( (SELECT ',' + '''' + split.Items + '''' FROM fn_Split(@StateIDs, ',') as split for XML PATH('')), 1,1,'')
	SET @CategoryCodes = REPLACE(@CategoryCodes, '1,2,3,15,16,17,18,19,20,21,22', '101,102,103,115,116,117,118,119,120,121,122')

	IF LEN(@AdHocXML) > 0
	BEGIN
		DECLARE @fields varchar(max) =  (
		SELECT 
		   STUFF((SELECT ', ' + COLUMN_NAME 
				  FROM INFORMATION_SCHEMA.COLUMNS
				  WHERE TABLE_NAME = 'PubSubscriptionsExtension' AND COLUMN_NAME LIKE 'Field%'
				  FOR XML PATH('')), 1, 1, '') [FIELDS])

		--CREATE TABLE #Tmp (PubSubscriptionID int, Answer varchar(100), CustomField varchar(100), PubID int)
		--insert into #Tmp
		--EXEC('
		set @executeString = '
		WITH CTE AS
		(
		SELECT PubSubscriptionID, 
		Fields, 
		Answers 
		FROM PubSubscriptionsExtension pe
		UNPIVOT (Answers FOR Fields IN (' + @fields + '))up
		)
		SELECT cte.PubSubscriptionID, cte.Answers as Answer, pem.CustomField, pem.PubID 
		INTO #Tmp
		FROM CTE
		JOIN PubSubscriptions ps ON ps.PubSubscriptionID = CTE.PubSubscriptionID
		JOIN PubSubscriptionsExtensionMapper pem ON pem.StandardField = cte.Fields AND pem.PubID = ps.PubID
		WHERE ps.PubID = ' + @PublicationID --+
		--)
	END

	CREATE TABLE #AdHoc
	(  
		RowID int IDENTITY(1, 1)
	  ,[FilterObject] nvarchar(256)
	  ,[SelectedCondition] nvarchar(256)
	  ,[Type] nvarchar(256)
	  ,[Value] nvarchar(1500)
	  ,[ToValue] nvarchar(256)
	  ,[FromValue] nvarchar(256)
	)

	DECLARE @docHandle int

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
		[Value] nvarchar(1500) 'AdHocFieldValue',
		[ToValue] nvarchar(256) 'AdHocToField',
		[FromValue] nvarchar(256) 'AdHocFromField'
	)

	EXEC sp_xml_removedocument @docHandle


	set @executeString += 'Select DISTINCT ps.PubSubscriptionID as Count FROM PubSubscriptions ps with(nolock) '
	set @executeString += 'LEFT JOIN #Tmp t ON t.PubSubscriptionID = ps.PubSubscriptionID '

	if len(@CategoryIDs) > 0
		set @executeString = @executeString + ' JOIN  UAD_Lookup..CategoryCode cc with(nolock) ON ps.PubCategoryID = cc.CategoryCodeID '

	if len(@TransactionIDs) > 0
		set @executeString = @executeString + ' JOIN (select distinct tc1.TransactionCodeID from  UAD_Lookup..TransactionCode tc1 with(nolock) where tc1.TransactionCodeTypeID in (' + @TransactionIDs +')) tc on ps.PubTransactionID = tc.TransactionCodeID '

	set @executeString = @executeString + ' LEFT OUTER JOIN UAD_Lookup..Country c with(nolock) ON c.CountryID = ps.CountryID Where	 ps.PubID =' +  @PublicationID
	IF @WaveMail = 'Is Wave Mailed'
		set @executeString += ' and ps.IsInActiveWaveMailing = 1'
	ELSE IF @WaveMail = 'Is Not Wave Mailed'
		set @executeString += ' and ps.IsInActiveWaveMailing = 0'		
		
	Begin --Check passed Parameters

		if len(@CategoryIDs) > 0
			set @executeString = @executeString + ' and cc.CategoryCodeTypeID in (' + @CategoryIDs +')'
				
		if len(@CategoryCodes) > 0
			set @executeString = @executeString + ' and ps.PubCategoryID in (' + @CategoryCodes +')'
				
		if len(@TransactionCodes) > 0
			set @executeString = @executeString + ' and ps.PubTransactionID in (' + @TransactionCodes +')'
				
		if len(@QsourceIDs) > 0
			set @executeString = @executeString + ' and ps.PubQSourceID in (' + @QsourceIDs +')'
				
		if len(@StateIDs) > 0
			set @executeString = @executeString + ' and ps.RegionCode in (' + @StateCodes + ')'	
				
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

				select @tempStartDate = ltrim(rtrim(YearStartDate)) , @tempEndDate = ltrim(rtrim(YearEndDate)) 
				from Pubs with(nolock) 
				where PubID = @PublicationID		

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
		
		IF LEN(@ResponseIDs) > 0
			BEGIN
				DECLARE @TempTable table (value varchar(100))
				DECLARE @FinalTable table (rGroup varchar(100), value varchar(100))
				INSERT INTO @TempTable
				SELECT * FROM fn_Split(@ResponseIDs, ',')
				INSERT INTO @FinalTable
				select ParsedData.* 
				from @TempTable mt
				cross apply ( select str = mt.value + ',' ) f1
				cross apply ( select p1 = charindex( '_', str ) ) ap1
				cross apply ( select p2 = charindex( ',', str, p1 + 1 ) ) ap2
				cross apply ( select rGroup = substring( str, 1, p1-1 )                   
								 , value = substring( str, p1+1, p2-p1-1 )
						  ) ParsedData
				update @FinalTable SET rGroup = LTRIM(RTRIM(rGroup)), value = LTRIM(RTRIM(value))
				DECLARE @anyResponse varchar(1000)
				DECLARE @noResponse varchar(1000)
				DECLARE @regResponse varchar(1000)
				SELECT @anyResponse = STUFF( (SELECT ',' + rGroup FROM @FinalTable WHERE value = 'YY' for XML PATH('')),1,1,'')
				SELECT @noResponse = STUFF( (SELECT ',' + rGroup FROM @FinalTable WHERE value = 'ZZ' for XML PATH('')),1,1,'')
				SELECT @regResponse = STUFF((SELECT ' AND ps.PubSubscriptionID in (SELECT srm.PubSubscriptionID from PubSubscriptionDetail srm with (NOLOCK) WHERE srm.CodeSheetID in (' + s.NameValues + '))' FROM 
				(
					SELECT
						distinct [rGroup],
						STUFF((
							SELECT ', ' + [value]  
							FROM @FinalTable
							WHERE (rGroup = Results.rGroup) AND value <> 'ZZ' AND value <> 'YY'
							FOR XML PATH(''),TYPE).value('(./text())[1]','VARCHAR(MAX)')
						,1,2,'') AS NameValues
					FROM @FinalTable Results WHERE value <> 'ZZ' AND value <> 'YY'
				) as s FOR XML PATH('')), 1,5,'')
		
				If LEN(@regResponse) > 0
					set @executeString += ' AND ' + @regResponse
				If LEN(@anyResponse) > 0
					BEGIN
						set @executeString += (CASE WHEN LEN(ISNULL(@regResponse,'')) = 0 THEN ' AND ' ELSE ' AND ' END) + '(ps.SubscriptionID in (SELECT DISTINCT pd.SubscriptionID from PubSubscriptionDetail pd 
													JOIN PubSubscriptions p ON p.PubSubscriptionID = pd.PubSubscriptionID 
													JOIN CodeSheet c ON pd.CodeSheetID = c.CodeSheetID
													WHERE p.PubID = ' + @PublicationID + ' AND c.ResponseGroupID in (' + @anyResponse + ')))'
					END
				IF LEN(@noResponse) > 0
					BEGIN
						set @executeString += (CASE WHEN LEN(ISNULL(@regResponse,'')) > 0 THEN ' AND '
													WHEN LEN(ISNULL(@anyResponse,'')) > 0 THEN ' AND '
													ELSE ' AND ' END) +
												'(ps.SubscriptionID not in (SELECT DISTINCT pd.SubscriptionID from PubSubscriptionDetail pd 
													JOIN PubSubscriptions p ON p.PubSubscriptionID = pd.PubSubscriptionID 
													JOIN CodeSheet c ON pd.CodeSheetID = c.CodeSheetID
													WHERE p.PubID = ' + @PublicationID + ' AND c.ResponseGroupID in (' + @noResponse + ')))'
					END
			END
				
		--ADHOC PROCESSING--
		if LEN(@AdHocXML) > 0
			BEGIN
				declare @Column varchar(100),
					@Value varchar(3000), 
					@ValueFrom varchar(100), 
					@ValueTo varchar(100), 
					@DataType varchar(100), 
					@Condition varchar(100)
				 
				DECLARE @NumberRecords int, @RowCount int, @AdHocString varchar(3000), @AdHocFinal varchar(8000)
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
							 if(@DataType = 'AdHoc')
								begin
									while(PATINDEX ('% ,%',@Value)>0 or PATINDEX ('%, %',@Value)>0 )
										set @Value = LTRIM(RTRIM(REPLACE(replace(@Value,' ,',','),', ',',')))
					
									set @AdhocString =  
										CASE  @Condition
											WHEN 'Equal' THEN '( t.Answer = '''+ REPLACE(@Value, ',', ''' or t.Answer =  ''')+ ''' and t.CustomField = ''' + @Column + ''')' 
											WHEN 'Contains' THEN '( t.Answer like ''%'+ REPLACE(@Value, ',', '%'' or t.Answer like  ''%')+ '%'' and t.CustomField = ''' + @Column + ''') ' 
											WHEN 'Start With' THEN '( t.Answer like '''+ REPLACE(@Value, ',', '%'' or t.Answer like  ''')+ '%'' and t.CustomField = ''' + @Column + ''') '
											WHEN 'End With' THEN '( t.Answer like ''%'+ REPLACE(@Value, ',', ''' or t.Answer like  ''%')+ ''' and t.CustomField = ''' + @Column + ''') '
											WHEN 'Does Not Contain' THEN '( t.Answer not like ''%'+ REPLACE(@Value, ',', '%'' or t.Answer not like  ''%')+ '%'' or t.Answer is null  and t.CustomField = ''' + @Column + ''')' 
											WHEN 'No Response' THEN '( ISNULL(NULLIF(NULLIF(CONVERT(varchar(100),t.Answer), ''0''), ''-1''), '''') = '''' )' 
											WHEN 'Any Response' THEN '( ISNULL(NULLIF(NULLIF(CONVERT(varchar(100),t.Answer), ''0''), ''-1''), '''') <> '''' and t.CustomField = ''' + @Column + ''')' 
										END 
								 end	
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
											WHEN 'No Response' THEN '( ISNULL(NULLIF(NULLIF(CONVERT(varchar(100),ps.' + @Column + '), ''0''), ''-1''), '''') = '''')' 
											WHEN 'Any Response' THEN '( ISNULL(NULLIF(NULLIF(CONVERT(varchar(100),ps.' + @Column + '), ''0''), ''-1''), '''') <> '''')' 
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
												WHEN 'Lesser Than' THEN 'ISNULL(ps.' + @Column + ', 0) < ' + @ValueFrom 

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