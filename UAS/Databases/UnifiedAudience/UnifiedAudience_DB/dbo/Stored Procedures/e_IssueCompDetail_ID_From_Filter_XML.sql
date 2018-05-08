CREATE PROCEDURE [dbo].[e_IssueCompDetail_ID_From_Filter_XML] 
	@FilterString varchar(MAX) = '<XML><Filters></Filters></XML>',
	@AdHocXML varchar(MAX) = '<XML><FilterDetail></FilterDetail></XML>',
	@IssueCompID int = 0
AS

BEGIN

	--DECLARE @FilterString varchar(max) = '<XML><Filters><ProductID>1</ProductID><CategoryCode>101,102,103,130,115,116,117,118,119,120,121,122,123,124,125,126,127,128,105,106,129,107,108,109,110,111,112,113</CategoryCode><CategoryCodeType>2,4,1,3</CategoryCodeType><TransactionCode>101,140,141,111,112,113,114,115,116,102,117,103,137,145,146,138,139,104,136,118,119,120,121,122,123,124,125,142,143</TransactionCode><TransactionCodeType>1,3</TransactionCodeType></Filters></XML>'
	--DECLARE @AdHocXML varchar(max) = '<XML><FilterDetail><FilterField>City</FilterField><SearchCondition>EQUAL</SearchCondition><FilterObjectType>Standard</FilterObjectType><AdHocFieldValue>Hollywood</AdHocFieldValue></FilterDetail></XML>'
	--DECLARE	@IssueCompID int = 14

	set nocount on

	Declare @ProductID int = 0,
	@CategoryTypes varchar(800) = '',
	@CategoryCodes varchar(800) = '',
	@TransactionTypes varchar(800) = '',
	@TransactionCodes varchar(800) = '',
	@QsourceIDs varchar(800) = '',
	@Par3C varchar(800) = '',
	@StateIDs varchar(800) = '',
	@CountryIDs varchar(1500) = '',
	@Email bit,
	@Phone bit,
	@Fax bit,
	@Mobile bit,
	@ResponseIDs varchar(800) = '',
	@Demo7 varchar(10) = '',		
	@Year varchar(20) = '',
	@StartDate varchar(10) = '',		
	@EndDate varchar(10) = '',
	@WaveMail varchar(30) = '',
	@MailPermission bit,
	@FaxPermission bit,
	@PhonePermission bit,
	@OtherProductsPermission bit,
	@ThirdPartyPermission bit,
	@EmailRenewPermission bit
	--@AdHocXML varchar(8000) = ''

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
		FROM OPENXML(@docHandle, N'/XML/Filters/QualificationSource')   
		WITH (value varchar(800) '.')

		select @StateIDs = value
		FROM OPENXML(@docHandle, N'/XML/Filters/RegionCode')   
		WITH (value varchar(800) '.')

		select @CountryIDs = value
		FROM OPENXML(@docHandle, N'/XML/Filters/Country')   	
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

		select @Mobile = value
		FROM OPENXML(@docHandle, N'/XML/Filters/Mobile')
		WITH (value varchar(800) '.')   

		select @Demo7 = value
		FROM OPENXML(@docHandle, N'/XML/Filters/Media')   
		WITH (value varchar(800) '.')
		WHERE ISNULL(value, '0') <> '0'

		select @Demo7 = value
		FROM OPENXML(@docHandle, N'/XML/Filters/Demo7')   
		WITH (value varchar(800) '.')
		WHERE ISNULL(value, '0') <> '0'

		select @StartDate = Cast(value as Date)
		FROM OPENXML(@docHandle, N'/XML/Filters/QualificationDateFrom')   
		WITH (value varchar(800) '.')

		select  @EndDate = Cast(value as Date)
		FROM OPENXML(@docHandle, N'/XML/Filters/QualificationDateTo')   
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
	
		--select @PhonePermission = value
		--FROM OPENXML(@docHandle, N'/XML/Filters/PhonePermission') 
		--WITH (value varchar(800) '.')
	
		--select @FaxPermission = value
		--FROM OPENXML(@docHandle, N'/XML/Filters/FaxPermission') 
		--WITH (value varchar(800) '.')
	
		--select @MailPermission = value
		--FROM OPENXML(@docHandle, N'/XML/Filters/MailPermission') 
		--WITH (value varchar(800) '.')
	
		--select @OtherProductsPermission = value
		--FROM OPENXML(@docHandle, N'/XML/Filters/OtherProductsPermission') 
		--WITH (value varchar(800) '.')
	
		--select @ThirdPartyPermission = value
		--FROM OPENXML(@docHandle, N'/XML/Filters/ThirdPartyPermission') 
		--WITH (value varchar(800) '.')
	
		--select @EmailRenewPermission = value
		--FROM OPENXML(@docHandle, N'/XML/Filters/EmailRenewPermission') 
		--WITH (value varchar(800) '.')

		select @PhonePermission = value
		FROM OPENXML(@docHandle, N'/XML/Filters/Demo33') 
		WITH (value varchar(800) '.')
	
		select @FaxPermission = value
		FROM OPENXML(@docHandle, N'/XML/Filters/Demo32') 
		WITH (value varchar(800) '.')
	
		select @MailPermission = value
		FROM OPENXML(@docHandle, N'/XML/Filters/Demo31') 
		WITH (value varchar(800) '.')
	
		select @OtherProductsPermission = value
		FROM OPENXML(@docHandle, N'/XML/Filters/Demo34') 
		WITH (value varchar(800) '.')
	
		select @ThirdPartyPermission = value
		FROM OPENXML(@docHandle, N'/XML/Filters/Demo35') 
		WITH (value varchar(800) '.')
	
		select @EmailRenewPermission = value
		FROM OPENXML(@docHandle, N'/XML/Filters/Demo36') 
		WITH (value varchar(800) '.')

		EXEC sp_xml_removedocument @docHandle
	END

	DECLARE	@executeString varchar(8000) = ''
	DECLARE @currentYear int
	DECLARE @tempStartDate varchar(10)
	DECLARE @tempEndDate varchar(10)
	DECLARE @tempYear varchar(20)
	DECLARE @subResponses table (value varchar(10))
	DECLARE @responses table (value varchar(10))

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
		[Value] nvarchar(1500) 'AdHocFieldValue',
		[ToValue] nvarchar(256) 'AdHocToField',
		[FromValue] nvarchar(256) 'AdHocFromField'
	)

	EXEC sp_xml_removedocument @docHandle2


	set @executeString += ' Select DISTINCT icd.IssueCompDetailId FROM IssueCompDetail icd with(nolock) '
	SET @executeString += ' WHERE icd.PubID = ' + CONVERT(varchar(25), @ProductID)

		
	if len(@CategoryCodes) > 0
		set @executeString = @executeString + ' and icd.PubCategoryID in (' + @CategoryCodes +')'
				
	if len(@TransactionCodes) > 0
		set @executeString = @executeString + ' and icd.PubTransactionID in (' + @TransactionCodes +')'

		
	Begin --Check passed Parameters
		IF @WaveMail = 'Is Wave Mailed'
			set @executeString += ' and icd.IsInActiveWaveMailing = 1'
		ELSE IF @WaveMail = 'Is Not Wave Mailed'
			set @executeString += ' and icd.IsInActiveWaveMailing = 0'		
				
		if len(@QsourceIDs) > 0
					set @executeString = @executeString + ' and icd.PubQSourceID in (' + @QsourceIDs +')'
				
		if len(@StateIDs) > 0
					set @executeString = @executeString + ' and icd.RegionID in (' + @StateIDs +')'
				
		if LEN(@CountryIDs) > 0
					set @executeString = @executeString + ' and icd.CountryID in (' + @CountryIDs + ')'
				
		if LEN(@Par3C) > 0
		BEGIN
			set @executeString = @executeString + ' and icd.Par3CID in (' + @Par3C + ')'		
		END
				
		if len(@Email) > 0
			Begin
				--set @Email = (CASE WHEN @Email='Yes' THEN 1 ELSE 0 END)
				if @Email = 1
					set @executeString = @executeString + ' and Isnull(ltrim(rtrim(icd.Email)),'''') <> ''''' 
				else
					set @executeString = @executeString + ' and Isnull(ltrim(rtrim(icd.Email)),'''') = ''''' 
			End

		if len(@Phone) > 0
			Begin
				--set @Phone = (CASE WHEN @Phone='Yes' THEN 1 ELSE 0 END)
				if @Phone = 1
					set @executeString = @executeString + ' and Isnull(ltrim(rtrim(icd.Phone)),'''') <> ''''' 
				else
					set @executeString = @executeString + ' and Isnull(ltrim(rtrim(icd.Phone)),'''') = ''''' 
			End
		
		if len(@Mobile) > 0
		Begin
			--set @Mobile = (CASE WHEN @Mobile='Yes' THEN 1 ELSE 0 END)
			if @Mobile = 1
				set @executeString = @executeString + ' and Isnull(ltrim(rtrim(icd.Mobile)),'''') <> ''''' 
			else
				set @executeString = @executeString + ' and Isnull(ltrim(rtrim(icd.Mobile)),'''') = ''''' 
		End

		if len(@Fax) > 0
			Begin
				--set @Fax = (CASE WHEN @Fax='Yes' THEN 1 ELSE 0 END)
				if @Fax = 1
					set @executeString = @executeString + ' and Isnull(ltrim(rtrim(icd.Fax)),'''') <> ''''' 
				else
					set @executeString = @executeString + ' and Isnull(ltrim(rtrim(icd.Fax)),'''') = ''''' 
			End
	
		--IF LEN(@MailPermission) > 0
		--BEGIN
		--	SET @executeString += ' and icd.MailPermission = ' + convert(varchar(1),@MailPermission)
		--END	
	
		--IF LEN(@FaxPermission) > 0
		--BEGIN
		--	SET @executeString += ' and icd.FaxPermission = ' + convert(varchar(1),@FaxPermission)
		--END	
	
		--IF LEN(@PhonePermission) > 0
		--BEGIN
		--	SET @executeString += ' and icd.PhonePermission = ' + convert(varchar(1),@PhonePermission)
		--END	
	
		--IF LEN(@OtherProductsPermission) > 0
		--BEGIN
		--	SET @executeString += ' and icd.OtherProductsPermission = ' + convert(varchar(1),@OtherProductsPermission)
		--END	
	
		--IF LEN(@ThirdPartyPermission) > 0
		--BEGIN
		--	SET @executeString += ' and icd.ThirdPartyPermission = ' + convert(varchar(1),@ThirdPartyPermission)
		--END	
	
		--IF LEN(@EmailRenewPermission) > 0
		--BEGIN
		--	SET @executeString += ' and icd.EmailRenewPermission = ' + convert(varchar(1),@EmailRenewPermission)
		--END	
	
		DECLARE @execDateString varchar(max) = ''

		if (len(@startDate) > 0 or len (@endDate) > 0)
		BEGIN
			if len(@startDate) > 0
				set @execDateString = ' (icd.Qualificationdate >= ''' + @startDate + ''''	

			if len(@endDate) > 0 and len(@execDateString) > 0
				set @execDateString = @execDateString + ' and icd.Qualificationdate <= ''' + @endDate + '''' + ')'	
			else if len(@endDate) > 0
				set @execDateString = @execDateString + ' (icd.Qualificationdate <= ''' + @endDate + '''' + ')'
			else
			BEGIN
				if len(@execDateString) > 0
					set @execDateString = @execDateString + ') '
			END
		END
		else
		BEGIN	
			if len(@Year) > 0 and CHARINDEX('0', @year) = 0
			Begin
			
				declare @Yearstring varchar(max)
				set @Yearstring= ''

				select @tempStartDate = ltrim(rtrim(YearStartDate)) , @tempEndDate = ltrim(rtrim(YearEndDate)) from Pubs with(nolock) where PubID = @ProductID		

				if getdate() > convert(date,@tempStartDate + '/' + convert(varchar,year(getdate())))
					set @currentYear = year(getdate()) 
				else
					set @currentYear = year(getdate()) - 1		
						
				declare @startdateTemp date,
						@endDateTemp date
					
				select @startdateTemp  = @tempStartDate + '/' + convert(varchar,@currentYear)
				select @endDateTemp =  @tempEndDate + '/' + convert(varchar,@currentYear + 1)

			
				if CHARINDEX('1', @year) > 0
					set @Yearstring = @Yearstring + ' icd.Qualificationdate between ''' +  convert(varchar,@startdateTemp) + ''' and ''' + convert(varchar,@endDateTemp) + ''' '--convert(varchar(20),@startdateTemp,111) + ''' and ''' + convert(varchar(20),@endDateTemp,111)  + ' 23:59:59'' '

				if CHARINDEX('2', @year) > 0
					set @Yearstring = @Yearstring  +  (Case when len(@Yearstring) > 0 then 'or' else '' end) +' icd.Qualificationdate between ''' + convert(varchar,dateadd(yy, -1, @startdateTemp)) + ''' and ''' + convert(varchar,dateadd(yy, -1,  @endDateTemp))  + '''  '--convert(varchar(20),dateadd(yy, -1, @startdateTemp),111) + ''' and ''' + convert(varchar(20),dateadd(yy, -1,  @endDateTemp),111)  + ' 23:59:59''  '

				if CHARINDEX('3', @year) > 0
					set @Yearstring = @Yearstring +  (Case when len(@Yearstring) > 0 then 'or' else '' end) +' icd.Qualificationdate between ''' + convert(varchar,dateadd(yy, -2, @startdateTemp)) + ''' and ''' + convert(varchar,dateadd(yy, -2,  @endDateTemp)) + ''' '--convert(varchar(20),dateadd(yy, -2, @startdateTemp),111) + ''' and ''' + convert(varchar(20),dateadd(yy, -2,  @endDateTemp),111)  + ' 23:59:59'' '

				if CHARINDEX('4', @year) > 0
					set @Yearstring = @Yearstring +  (Case when len(@Yearstring) > 0 then 'or' else '' end) +' icd.Qualificationdate between ''' + convert(varchar,dateadd(yy, -3, @startdateTemp)) + ''' and ''' + convert(varchar,dateadd(yy, -3,  @endDateTemp)) + ''' '--convert(varchar(20),dateadd(yy, -3, @startdateTemp),111) + ''' and ''' + convert(varchar(20),dateadd(yy, -3,  @endDateTemp),111) + ' 23:59:59'' '

				if CHARINDEX('5', @year) > 0
					set @Yearstring = @Yearstring +  (Case when len(@Yearstring) > 0 then 'or' else '' end) +' icd.Qualificationdate < ''' + convert(varchar,dateadd(yy, -4,  @endDateTemp )) + ''' '--convert(varchar(20),dateadd(yy, -4,  @endDateTemp ),111) + ' 23:59:59'' '


				if Len(@yearstring) > 0
					set @executeString = @executeString + ' and (' + @Yearstring + ') '  
			End	
		END

		IF len(@execDateString) >0
			set @executeString = @executeString + ' and ( ' + @execDateString + ' ) ' 
	
		IF LEN(@Demo7) > 0
		BEGIN
		--need to handle passing multiple values in convert to a temp table
			set @Demo7 = REPLACE(REPLACE(REPLACE(@Demo7,'1899','A'),'1900','B'),'1901','C')
			set @executeString = @executeString + ' and icd.Demo7 in (select items from dbo.fn_Split(''' + @Demo7 + ''', '',''))'--in (SELECT CodeValue from UAD_Lookup..Code c with (NOLOCK) WHERE c.CodeId in (' + @Demo7 + '))'	
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

						
						IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'IssueCompDetail' AND COLUMN_NAME = @Column)
						BEGIN
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
										CASE @Value
											WHEN 'No Response' THEN ' (icd.[' + @Column + '] = '''' or icd.[' + @Column + '] is null)'
										ELSE 
										(CASE @Condition
											WHEN 'Equal' THEN '( icd.' + @Column  + ' = '''+ REPLACE(@Value, ',', ''' or icd.' + @Column + ' =  ''')+ ''') ' 
											WHEN 'Contains' THEN '( icd.' + @Column  + ' like ''%'+ REPLACE(@Value, ',', '%'' or icd.' + @Column + ' like  ''%')+ '%'') ' 
											WHEN 'Start With' THEN '( icd.' + @Column  + ' like '''+ REPLACE(@Value, ',', '%'' or icd.' + @Column + ' like  ''')+ '%'') '
											WHEN 'End With' THEN '( icd.' + @Column  + ' like ''%'+ REPLACE(@Value, ',', ''' or icd.' + @Column + ' like  ''%')+ ''') '
											WHEN 'Does Not Contain' THEN '( icd.' + @Column  + ' not like ''%'+ REPLACE(@Value, ',', '%'' or icd.' + @Column + ' not like  ''%')+ '%'' or ' +  @Column + ' is null ) '
											WHEN 'No Response' THEN '( ISNULL(NULLIF(NULLIF(CONVERT(varchar(100),icd.' + @Column + '), ''0''), ''-1''), '''') = '''')' 
											WHEN 'Any Response' THEN '( ISNULL(NULLIF(NULLIF(CONVERT(varchar(100),icd.' + @Column + '), ''0''), ''-1''), '''') <> '''')' 
										END)
										END
								 end			 

							 if(@DataType = 'DateRange') 
								 begin
									 if(@Column = 'STARTISSUEDATE' or  @Column = 'EXPIREISSUEDATE' or @Column = 'PAIDDATE')
										 Begin
											 set @AdhocString = 
												 CASE  @Condition
														WHEN 'DateRange' THEN case when @ValueTo is null then 'spp.' + @Column + ' >= ''' + @ValueFrom + '''' else 'spp.' + @Column + ' >= ''' + @ValueFrom + ''' and spp.' + @Column + ' <= ''' + @ValueTo + ''''  END
														WHEN 'Year' THEN case when @ValueTo is null then 'year(spp.' + @Column + ') >= ''' + @ValueFrom + '''' else 'year(spp.' + @Column + ') >= ''' + @ValueFrom + ''' and year(spp.' + @Column + ') <= ''' + @ValueTo + ''''  END
														WHEN 'Month' THEN case when @ValueTo is null then 'month(spp.' + @Column + ') >= ''' + @ValueFrom + '''' else 'moonth(spp.' + @Column + ') >= ''' + @ValueFrom + ''' and month(spp.' + @Column + ') <= ''' + @ValueTo + ''''  END
												 END 
										 end	 
									 else	 
										 Begin
											 set @AdhocString = 
												 CASE  @Condition
														WHEN 'DateRange' THEN (case when @ValueTo is null then 'icd.' + @Column + ' >= ''' + @ValueFrom + '''' 
														when @ValueFrom is null then 'icd.' + @Column + ' <= ''' + @ValueTo + '''' 
														else 'icd.' + @Column + ' >= ''' + @ValueFrom + ''' and icd.' + @Column + ' <= ''' + @ValueTo + ''''  END)
														WHEN 'Year' THEN case when @ValueTo is null then 'year(icd.' + @Column + ') >= ''' + @ValueFrom + '''' else 'year(icd.' + @Column + ') >= ''' + @ValueFrom + ''' and year(icd.' + @Column + ') <= ''' + @ValueTo + ''''  END
														WHEN 'Month' THEN case when @ValueTo is null then 'month(icd.' + @Column + ') >= ''' + @ValueFrom + '''' else 'moonth(icd.' + @Column + ') >= ''' + @ValueFrom + ''' and month(icd.' + @Column + ') <= ''' + @ValueTo + ''''  END
												 END 
										 end				 
								 end		
				 
							if(@DataType = 'Range') 
								begin
									 set @AdhocString = 
										 CASE  @Condition
												WHEN 'Range' THEN case when @ValueTo is null then '(icd.' + @Column + ') >= ' + @ValueFrom else '(icd.' + @Column + ') >= ' + @ValueFrom + ' and (icd.' + @Column + ') <= ' + @ValueTo  END
												WHEN 'Equal' THEN 'icd.' + @Column + ' = ' + @ValueFrom 
												WHEN 'Greater Than' THEN 'icd.' + @Column + ' > ' + @ValueFrom 
												WHEN 'Lesser Than' THEN 'ISNULL(icd.' + @Column + ', 0) < ' + @ValueFrom 
										 END 
								end	 
						END	
					
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

