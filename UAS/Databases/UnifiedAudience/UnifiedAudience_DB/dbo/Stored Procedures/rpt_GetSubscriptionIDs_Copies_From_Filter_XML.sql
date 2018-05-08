CREATE procedure [dbo].[rpt_GetSubscriptionIDs_Copies_From_Filter_XML]
(  
	@FilterString varchar(MAX) = '<XML><Filters></Filters></XML>',
	@AdHocXML varchar(MAX) = '<XML><FilterDetail></FilterDetail></XML>',
	@IncludeAddRemove bit = 'false',
	@UseArchive bit = 'false',
	@IssueID int = 0
) AS

BEGIN
--Declare @FilterString varchar(max) = '<XML><Filters><ProductID>63</ProductID></Filters></XML>'
--DEclare @AdHocXML varchar(max) = '<XML><FilterDetail><FilterField>OldPriorityCode</FilterField><SearchCondition>ANY RESPONSE</SearchCondition><FilterObjectType>AdHoc</FilterObjectType><AdHocFieldValue></AdHocFieldValue></FilterDetail></XML>'
--Declare @IncludeAddRemove bit = 0
--DECLARE	@UseArchive bit = 1
--DECLARE	@IssueID int = 2

set nocount on

Declare @ProductID int = 63,
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
@ResponseIDs varchar(max) = '',
@Demo7 varchar(10) = '',		
@Year varchar(20) = '',
@StartDate varchar(10) = '',		
@EndDate varchar(10) = '',
@WaveMail varchar(30) = '',
@MailPermission varchar(15),
@FaxPermission varchar(15),
@PhonePermission varchar(15),
@OtherProductsPermission varchar(15),
@ThirdPartyPermission varchar(15),
@EmailRenewPermission varchar(15),
@fields varchar(max),
@Column varchar(100),
@Value varchar(3000), 
@ValueFrom varchar(100), 
@ValueTo varchar(100), 
@DataType varchar(100), 
@Condition varchar(100),
@NumberRecords int, 
@RowCount int, 
@AdHocString varchar(3000), 
@AdHocFinal varchar(8000)
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

	select @QsourceIDs = case when LEN(value) >= 1 then value else @QsourceIDs end --this is to handle drilldown when using qsource as a filter
	FROM OPENXML(@docHandle, N'/XML/Filters/QsourceIDs')   
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
	WITH (value varchar(max) '.')
	
	select @Par3C = value
	FROM OPENXML(@docHandle, N'/XML/Filters/Par3C') 
	WITH (value varchar(800) '.')
	
	select @WaveMail = value
	FROM OPENXML(@docHandle, N'/XML/Filters/WaveMail') 
	WITH (value varchar(800) '.')
	
	select @PhonePermission = value
	FROM OPENXML(@docHandle, N'/XML/Filters/PhonePermission') 
	WITH (value varchar(800) '.')
	
	select @FaxPermission = value
	FROM OPENXML(@docHandle, N'/XML/Filters/FaxPermission') 
	WITH (value varchar(800) '.')
	
	select @MailPermission = value
	FROM OPENXML(@docHandle, N'/XML/Filters/MailPermission') 
	WITH (value varchar(800) '.')
	
	select @OtherProductsPermission = value
	FROM OPENXML(@docHandle, N'/XML/Filters/OtherProductsPermission') 
	WITH (value varchar(800) '.')
	
	select @ThirdPartyPermission = value
	FROM OPENXML(@docHandle, N'/XML/Filters/ThirdPartyPermission') 
	WITH (value varchar(800) '.')
	
	select @EmailRenewPermission = value
	FROM OPENXML(@docHandle, N'/XML/Filters/EmailRenewPermission') 
	WITH (value varchar(800) '.')

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

DECLARE	@executeString varchar(max) = ''
DECLARE @currentYear int
DECLARE @tempStartDate varchar(10)
DECLARE @tempEndDate varchar(10)
DECLARE @tempYear varchar(20)
DECLARE @subResponses table (value varchar(10))
DECLARE @responses table (value varchar(10))

IF CHARINDEX('<FilterObjectType>AdHoc</FilterObjectType>', @AdHocXML) > 0 and @UseArchive = 0
BEGIN
	set @fields =  (
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
		FROM PubSubscriptionsExtension pe with(nolock)
	UNPIVOT (Answers FOR Fields IN (' + @fields + '))up
	)
	SELECT cte.PubSubscriptionID, cte.Answers as Answer, pem.CustomField, pem.PubID 
	INTO #Tmp
	FROM CTE
		JOIN PubSubscriptions ps with(nolock) ON ps.PubSubscriptionID = CTE.PubSubscriptionID
		JOIN PubSubscriptionsExtensionMapper pem with(nolock) ON pem.StandardField = cte.Fields AND pem.PubID = ps.PubID
	WHERE ps.PubID = ' + CONVERT(varchar(5),@ProductID) + CHAR(13) + CHAR(10)
END
IF CHARINDEX('<FilterObjectType>AdHoc</FilterObjectType>', @AdHocXML) > 0 and @UseArchive = 1
BEGIN
	set @fields =  (
		SELECT 
		   STUFF((SELECT ', ' + COLUMN_NAME 
				  FROM INFORMATION_SCHEMA.COLUMNS
				  WHERE TABLE_NAME = 'IssueArchivePubSubscriptionsExtension' AND COLUMN_NAME LIKE 'Field%'
				  FOR XML PATH('')), 1, 1, '') [FIELDS])
	--CREATE TABLE #Tmp (PubSubscriptionID int, Answer varchar(100), CustomField varchar(100), PubID int)
	--insert into #Tmp
	--EXEC('
	set @executeString = '
	WITH CTE AS
	(
	SELECT IssueArchiveSubscriptionID, 
	Fields, 
	Answers 
		FROM IssueArchivePubSubscriptionsExtension pe with(nolock)
	UNPIVOT (Answers FOR Fields IN (' + @fields + '))up
	)
	SELECT cte.IssueArchiveSubscriptionID, cte.Answers as Answer, pem.CustomField, pem.PubID 
	INTO #Tmp
	FROM CTE
		JOIN IssueArchiveProductSubscription ps with(nolock) ON ps.IssueArchiveSubscriptionID = CTE.IssueArchiveSubscriptionID
		JOIN PubSubscriptionsExtensionMapper pem with(nolock) ON pem.StandardField = cte.Fields AND pem.PubID = ps.PubID
	WHERE ps.issueid = ' + CONVERT(varchar(5),@IssueID) + CHAR(13) + CHAR(10)
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

IF @IncludeAddRemove = 0
BEGIN
	IF @UseArchive = 0
	BEGIN
		set @executeString += ' Select DISTINCT ps.PubSubscriptionID FROM PubSubscriptions ps with(nolock) '
		IF CHARINDEX('<FilterObjectType>AdHoc</FilterObjectType>', @AdHocXML) > 0
		BEGIN
			SET @executeString += 'LEFT JOIN #Tmp t ON t.PubSubscriptionID = ps.PubSubscriptionID WHERE ps.PubID = ' + CONVERT(varchar(25), @ProductID)
		END
		ELSE
		BEGIN
			SET @executeString += 'WHERE ps.PubID = ' + CONVERT(varchar(25), @ProductID)
		END
	END
	ELSE
	BEGIN
			set @executeString += ' Select DISTINCT ps.PubSubscriptionID FROM IssueArchiveProductSubscription ps with(nolock) ' 
			
			IF CHARINDEX('<FilterObjectType>AdHoc</FilterObjectType>', @AdHocXML) > 0
				SET @executeString += 'LEFT JOIN #Tmp t ON t.IssueArchiveSubscriptionID = ps.IssueArchiveSubscriptionID '

			SET @executeString += 'WHERE ps.issueid = ' + CONVERT(varchar(25), @IssueID) + ' and ps.PubID = ' + CONVERT(varchar(25), @ProductID)
		
	END

	if len(@CategoryCodes) > 0
		set @executeString = @executeString + ' and ps.PubCategoryID in (' + @CategoryCodes +')'
				
	if len(@TransactionCodes) > 0
		set @executeString = @executeString + ' and ps.PubTransactionID in (' + @TransactionCodes +')'
END
ELSE
BEGIN
		set @executeString = 'Select DISTINCT ps.PubSubscriptionID FROM PubSubscriptions ps with(nolock) LEFT JOIN SubscriberAddKillDetail sak with(nolock) ON sak.PubsubscriptionID = ps.PubsubscriptionID and sak.AddKillId = 0 WHERE ps.PubID = ' + CONVERT(varchar(25), @ProductID)
		
	if len(@CategoryCodes) > 0
		set @executeString = @executeString + ' and ISNULL(sak.PubCategoryID, ps.PubCategoryID) in (' + @CategoryCodes +')'
				
	if len(@TransactionCodes) > 0
		set @executeString = @executeString + ' and ISNULL(sak.PubTransactionID, ps.PubTransactionID) in (' + @TransactionCodes +')'
END
		
Begin --Check passed Parameters
	IF @WaveMail = 'Is Wave Mailed'
		set @executeString += ' and ps.IsInActiveWaveMailing = 1'
	ELSE IF @WaveMail = 'Is Not Wave Mailed'
		set @executeString += ' and ps.IsInActiveWaveMailing = 0'		
				
	if len(@QsourceIDs) > 0
				set @executeString = @executeString + ' and ps.PubQSourceID in (' + @QsourceIDs +')'
				
	if len(@StateIDs) > 0
				set @executeString = @executeString + ' and ps.RegionID in (' + @StateIDs +')'
				
	if LEN(@CountryIDs) > 0
				set @executeString = @executeString + ' and ps.CountryID in (' + @CountryIDs + ')'
				
	if LEN(@Par3C) > 0
	BEGIN
		set @executeString = @executeString + ' and ps.Par3CID in (' + @Par3C + ')'		
	END
				
	if len(@Email) > 0
		Begin
				--set @Email = (CASE WHEN @Email='Yes' THEN 1 ELSE 0 END)
			if @Email = 1
				set @executeString = @executeString + ' and Isnull(ltrim(rtrim(ps.Email)),'''') <> ''''' 
			else
				set @executeString = @executeString + ' and Isnull(ltrim(rtrim(ps.Email)),'''') = ''''' 
		End

	if len(@Phone) > 0
		Begin
				--set @Phone = (CASE WHEN @Phone='Yes' THEN 1 ELSE 0 END)
			if @Phone = 1
				set @executeString = @executeString + ' and Isnull(ltrim(rtrim(ps.Phone)),'''') <> ''''' 
			else
				set @executeString = @executeString + ' and Isnull(ltrim(rtrim(ps.Phone)),'''') = ''''' 
		End
		
	if len(@Mobile) > 0
	Begin
			--set @Mobile = (CASE WHEN @Mobile='Yes' THEN 1 ELSE 0 END)
		if @Mobile = 1
			set @executeString = @executeString + ' and Isnull(ltrim(rtrim(ps.Mobile)),'''') <> ''''' 
		else
			set @executeString = @executeString + ' and Isnull(ltrim(rtrim(ps.Mobile)),'''') = ''''' 
	End

	if len(@Fax) > 0
		Begin
				--set @Fax = (CASE WHEN @Fax='Yes' THEN 1 ELSE 0 END)
			if @Fax = 1
				set @executeString = @executeString + ' and Isnull(ltrim(rtrim(ps.Fax)),'''') <> ''''' 
			else
				set @executeString = @executeString + ' and Isnull(ltrim(rtrim(ps.Fax)),'''') = ''''' 
		End
		
	--Has to check for null values as well as true or false so it can search for true, false, and blank
	IF LEN(@MailPermission) > 0 and @MailPermission != 'NULL'
	BEGIN
		SET @executeString += ' and (ps.MailPermission in (' + replace(convert(varchar(15),@MailPermission),',NULL',') or ( ps.MailPermission is null') + '))'
	END
	else if	@MailPermission = 'NULL'
	begin
		SET @executeString += ' and ps.MailPermission is null'
	end
	
	IF LEN(@FaxPermission) > 0 and @FaxPermission != 'NULL'
	BEGIN
		SET @executeString += ' and (ps.FaxPermission in (' + replace(convert(varchar(15),@FaxPermission),',NULL',') or ( ps.FaxPermission is null') + '))'
	END
	else if	@FaxPermission = 'NULL'
	begin
		SET @executeString += ' and ps.FaxPermission is null'
	end

	IF LEN(@PhonePermission) > 0 and @PhonePermission != 'NULL'
	BEGIN
		SET @executeString += ' and (ps.PhonePermission in (' + replace(convert(varchar(15),@PhonePermission),',NULL',') or ( ps.PhonePermission is null') + '))'
	END
	else if	@PhonePermission = 'NULL'
	begin
		SET @executeString += ' and ps.PhonePermission is null'
	end

	IF LEN(@OtherProductsPermission) > 0 and @OtherProductsPermission != 'NULL'
	BEGIN
		SET @executeString += ' and (ps.OtherProductsPermission in (' + replace(convert(varchar(15),@OtherProductsPermission),',NULL',') or ( ps.OtherProductsPermission is null') + '))'
	END
	else if	@OtherProductsPermission = 'NULL'
	begin
		SET @executeString += ' and ps.OtherProductsPermission is null'
	end
	--@ThirdPartyPermission
	IF LEN(@ThirdPartyPermission) > 0 and @ThirdPartyPermission != 'NULL'
	BEGIN
		SET @executeString += ' and (ps.ThirdPartyPermission in (' + replace(convert(varchar(15),@ThirdPartyPermission),',NULL',') or ( ps.ThirdPartyPermission is null') + '))'
	END
	else if	@ThirdPartyPermission = 'NULL'
	begin
		SET @executeString += ' and ps.ThirdPartyPermission is null'
	end

	IF LEN(@EmailRenewPermission) > 0 and @EmailRenewPermission != 'NULL'
	BEGIN
		SET @executeString += ' and (ps.EmailRenewPermission in (' + replace(convert(varchar(15),@EmailRenewPermission),',NULL',') or ( ps.EmailRenewPermission is null') + '))'
	END
	else if	@EmailRenewPermission = 'NULL'
	begin
		SET @executeString += ' and ps.EmailRenewPermission is null'
	end
	
	DECLARE @execDateString varchar(max) = ''

	if (len(@startDate) > 0 or len (@endDate) > 0)
	BEGIN
		if len(@startDate) > 0
			set @execDateString = ' (ps.Qualificationdate >= ''' + @startDate + ''''	

		if len(@endDate) > 0 and len(@execDateString) > 0
			set @execDateString = @execDateString + ' and ps.Qualificationdate <= ''' + @endDate + '''' + ')'	
		else if len(@endDate) > 0
			set @execDateString = @execDateString + ' (ps.Qualificationdate <= ''' + @endDate + '''' + ')'
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
			select @endDateTemp =  @tempEndDate + '/' + convert(varchar,@currentYear)
				
			if @endDateTemp < @startdateTemp
			begin
				select @endDateTemp = dateadd(yy, 1, @endDateTemp)
			end

			
			if CHARINDEX('1', @year) > 0
					set @Yearstring = @Yearstring + ' ps.Qualificationdate between ''' +  convert(varchar,@startdateTemp) + ''' and ''' + convert(varchar,@endDateTemp) + ''' '--convert(varchar(20),@startdateTemp,111) + ''' and ''' + convert(varchar(20),@endDateTemp,111)  + ' 23:59:59'' '

			if CHARINDEX('2', @year) > 0
					set @Yearstring = @Yearstring  +  (Case when len(@Yearstring) > 0 then 'or' else '' end) +' ps.Qualificationdate between ''' + convert(varchar,dateadd(yy, -1, @startdateTemp)) + ''' and ''' + convert(varchar,dateadd(yy, -1,  @endDateTemp))  + '''  '--convert(varchar(20),dateadd(yy, -1, @startdateTemp),111) + ''' and ''' + convert(varchar(20),dateadd(yy, -1,  @endDateTemp),111)  + ' 23:59:59''  '

			if CHARINDEX('3', @year) > 0
					set @Yearstring = @Yearstring +  (Case when len(@Yearstring) > 0 then 'or' else '' end) +' ps.Qualificationdate between ''' + convert(varchar,dateadd(yy, -2, @startdateTemp)) + ''' and ''' + convert(varchar,dateadd(yy, -2,  @endDateTemp)) + ''' '--convert(varchar(20),dateadd(yy, -2, @startdateTemp),111) + ''' and ''' + convert(varchar(20),dateadd(yy, -2,  @endDateTemp),111)  + ' 23:59:59'' '

			if CHARINDEX('4', @year) > 0
					set @Yearstring = @Yearstring +  (Case when len(@Yearstring) > 0 then 'or' else '' end) +' ps.Qualificationdate between ''' + convert(varchar,dateadd(yy, -3, @startdateTemp)) + ''' and ''' + convert(varchar,dateadd(yy, -3,  @endDateTemp)) + ''' '--convert(varchar(20),dateadd(yy, -3, @startdateTemp),111) + ''' and ''' + convert(varchar(20),dateadd(yy, -3,  @endDateTemp),111) + ' 23:59:59'' '

			if CHARINDEX('5', @year) > 0
					set @Yearstring = @Yearstring +  (Case when len(@Yearstring) > 0 then 'or' else '' end) +' ps.Qualificationdate < ''' + convert(varchar,dateadd(yy, -4,  @endDateTemp )) + ''' '--convert(varchar(20),dateadd(yy, -4,  @endDateTemp ),111) + ' 23:59:59'' '


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
			set @executeString = @executeString + ' and ps.Demo7 in (select items from dbo.fn_Split(''' + @Demo7 + ''', '',''))'--in (SELECT CodeValue from UAD_Lookup..Code c with (NOLOCK) WHERE c.CodeId in (' + @Demo7 + '))'	
		END	
		
	IF LEN(@ResponseIDs) > 0
	BEGIN
		DECLARE @TempTable table (value varchar(100))
		DECLARE @FinalTable table (rGroup varchar(100), value varchar(100))
			DECLARE @anyResponse varchar(max)
			DECLARE @noResponse varchar(max)
			DECLARE @regResponse varchar(max)
		
			IF @UseArchive = 0 -- Live
			BEGIN
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
					set @executeString += (CASE WHEN LEN(ISNULL(@regResponse,'')) = 0 THEN ' AND ' ELSE ' AND ' END) + 
												'(ps.SubscriptionID in (SELECT DISTINCT pd.SubscriptionID from PubSubscriptionDetail pd 
										JOIN PubSubscriptions p ON p.PubSubscriptionID = pd.PubSubscriptionID 
										JOIN CodeSheet c ON pd.CodeSheetID = c.CodeSheetID
										WHERE p.PubID = ' + convert(varchar(50), @ProductID) + ' AND c.ResponseGroupID in (' + convert(varchar(50),@anyResponse) + ')))'
		END
		IF LEN(@noResponse) > 0
		BEGIN
			set @executeString += (CASE WHEN LEN(ISNULL(@regResponse,'')) > 0 THEN ' AND '
										WHEN LEN(ISNULL(@anyResponse,'')) > 0 THEN ' AND '
										ELSE ' AND ' END) +
											'(ps.SubscriptionID not in (SELECT DISTINCT pd.SubscriptionID from PubSubscriptionDetail pd with(nolock)  
												JOIN PubSubscriptions p with(nolock) ON p.PubSubscriptionID = pd.PubSubscriptionID 
												JOIN CodeSheet c with(nolock) ON pd.CodeSheetID = c.CodeSheetID
										WHERE p.PubID = ' + CONVERT(varchar(50),@ProductID) + ' AND c.ResponseGroupID in (' + convert(varchar(50), @noResponse) + ')))'
		END
	END
			ELSE --Archive
			BEGIN
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
				SELECT @anyResponse = STUFF( (SELECT ',' + rGroup FROM @FinalTable WHERE value = 'YY' for XML PATH('')),1,1,'')
				SELECT @noResponse = STUFF( (SELECT ',' + rGroup FROM @FinalTable WHERE value = 'ZZ' for XML PATH('')),1,1,'')
				SELECT @regResponse = STUFF((SELECT ' AND ps.PubSubscriptionID in (SELECT srm.PubSubscriptionID from IssueArchiveProductSubscriptionDetail srm with (NOLOCK)' + 
													'join IssueArchiveProductSubscription ps with(nolock) on srm.IssueArchiveSubscriptionId = ps.IssueArchiveSubscriptionId ' +
													'WHERE ps.issueid = ' + CONVERT(varchar(10), @issueid) +  ' and srm.CodeSheetID in (' + s.NameValues + '))' FROM 
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
					set @executeString += (CASE WHEN LEN(ISNULL(@regResponse,'')) = 0 THEN ' AND ' ELSE ' AND ' END) + 
												'(ps.SubscriptionID in (SELECT DISTINCT pd.SubscriptionID from IssueArchiveProductSubscriptionDetail pd with(nolock)  
												JOIN IssueArchiveProductSubscription p with(nolock) ON p.IssueArchiveSubscriptionId = pd.IssueArchiveSubscriptionId 
												JOIN CodeSheet c with(nolock) ON pd.CodeSheetID = c.CodeSheetID
												WHERE p.PubID = ' + convert(varchar(50), @ProductID) + ' AND c.ResponseGroupID in (' + convert(varchar(50),@anyResponse) + ') AND p.issueid = '+ CONVERT(varchar(10),@IssueID) +'))'
				END
				IF LEN(@noResponse) > 0
				BEGIN
					set @executeString += (CASE WHEN LEN(ISNULL(@regResponse,'')) > 0 THEN ' AND '
												WHEN LEN(ISNULL(@anyResponse,'')) > 0 THEN ' AND '
												ELSE ' AND ' END) +
											'(ps.IssueArchiveSubscriptionID not in (SELECT DISTINCT pd.IssueArchiveSubscriptionID from IssueArchiveProductSubscriptionDetail pd with(nolock)  
												JOIN IssueArchiveProductSubscription p with(nolock) ON p.IssueArchiveSubscriptionId = pd.IssueArchiveSubscriptionId 
												JOIN CodeSheet c with(nolock) ON pd.CodeSheetID = c.CodeSheetID
												WHERE p.PubID = ' + CONVERT(varchar(50),@ProductID) + ' AND c.ResponseGroupID in (' + convert(varchar(50), @noResponse)  + ') AND p.issueid = '+ CONVERT(varchar(10),@IssueID) +'))'
				END
			END
		END
				
	--ADHOC PROCESSING--
	if LEN(@AdHocXML) > 0 and @UseArchive = 0
		BEGIN
			set @NumberRecords = 0
			set @AdHocString = ''
			set @AdHocFinal = ''
				
			SELECT @NumberRecords = COUNT(*) from #Adhoc
				SET @RowCount = 1
				WHILE @RowCount <= @NumberRecords
				BEGIN
					 set @AdhocString = ''
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
									WHEN 'No Response' THEN '( ps.pubsubscriptionid not in (select PubSubscriptionID from #tmp t where t.CustomField = ''' + @Column +''' and ISNULL(NULLIF(NULLIF(CONVERT(varchar(100),t.Answer), ''0''), ''-1''), '''') <> ''''))' 
									WHEN 'Any Response' THEN '( ISNULL(NULLIF(NULLIF(CONVERT(varchar(100),t.Answer), ''0''), ''-1''), '''') <> '''' and t.CustomField = ''' + @Column + ''')' 
								END 
						 end	
					 if(@DataType = 'Standard') 
						begin
							while(PATINDEX ('% ,%',@Value)>0 or PATINDEX ('%, %',@Value)>0 )
								set @Value = LTRIM(RTRIM(REPLACE(replace(@Value,' ,',','),', ',',')))						
							set @AdhocString = 
								CASE @Value
									WHEN 'No Response' THEN ' (ps.[' + @Column + '] = '''' or ps.[' + @Column + '] is null)'
								ELSE 
								(CASE @Condition
									WHEN 'Equal' THEN '( ps.' + @Column  + ' = '''+ REPLACE(@Value, ',', ''' or ps.' + @Column + ' =  ''')+ ''') ' 
									WHEN 'Contains' THEN '( ps.' + @Column  + ' like ''%'+ REPLACE(@Value, ',', '%'' or ps.' + @Column + ' like  ''%')+ '%'') ' 
									WHEN 'Start With' THEN '( ps.' + @Column  + ' like '''+ REPLACE(@Value, ',', '%'' or ps.' + @Column + ' like  ''')+ '%'') '
									WHEN 'End With' THEN '( ps.' + @Column  + ' like ''%'+ REPLACE(@Value, ',', ''' or ps.' + @Column + ' like  ''%')+ ''') '
									WHEN 'Does Not Contain' THEN '( ps.' + @Column  + ' not like ''%'+ REPLACE(@Value, ',', '%'' or ps.' + @Column + ' not like  ''%')+ '%'' or ' +  @Column + ' is null ) '
									WHEN 'No Response' THEN '( ISNULL(NULLIF(NULLIF(CONVERT(varchar(100),ps.' + @Column + '), ''0''), ''-1''), '''') = '''')' 
									WHEN 'Any Response' THEN '( ISNULL(NULLIF(NULLIF(CONVERT(varchar(100),ps.' + @Column + '), ''0''), ''-1''), '''') <> '''')' 
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
												WHEN 'DateRange' THEN (case when @ValueTo is null then 'ps.' + @Column + ' >= ''' + @ValueFrom + '''' 
												when @ValueFrom is null then 'ps.' + @Column + ' <= ''' + @ValueTo + '''' 
												else 'ps.' + @Column + ' >= ''' + @ValueFrom + ''' and ps.' + @Column + ' <= ''' + @ValueTo + ''''  END)
												WHEN 'Year' THEN case when @ValueTo is null then 'year(ps.' + @Column + ') >= ''' + @ValueFrom + '''' else 'year(ps.' + @Column + ') >= ''' + @ValueFrom + ''' and year(ps.' + @Column + ') <= ''' + @ValueTo + ''''  END
												WHEN 'Month' THEN case when @ValueTo is null then 'month(ps.' + @Column + ') >= ''' + @ValueFrom + '''' else 'moonth(ps.' + @Column + ') >= ''' + @ValueFrom + ''' and month(ps.' + @Column + ') <= ''' + @ValueTo + ''''  END
										 END 
								 end				 
						 end		
				 
					if(@DataType = 'Range') 
						begin
							 set @AdhocString = 
								 CASE  @Condition
										WHEN 'Range' THEN case when @ValueTo is null then '(ps.' + @Column + ') >= ' + @ValueFrom else '(ps.' + @Column + ') >= ' + @ValueFrom + ' and (ps.' + @Column + ') <= ' + @ValueTo  END
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
		------Adhoc processing for archive
		if LEN(@AdHocXML) > 0 and @UseArchive = 1
		BEGIN
			set @NumberRecords = 0
			set @AdHocString = ''
			set @AdHocFinal = ''
				
			SELECT @NumberRecords = COUNT(*) from #Adhoc
				SET @RowCount = 1
				WHILE @RowCount <= @NumberRecords
				BEGIN
					 set @AdhocString = ''
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
									WHEN 'No Response' THEN '( ps.IssueArchiveSubscriptionID not in (select IssueArchiveSubscriptionID from #tmp t where t.CustomField = ''' + @Column +''' and ISNULL(NULLIF(NULLIF(CONVERT(varchar(100),t.Answer), ''0''), ''-1''), '''') <> ''''))' 
									WHEN 'Any Response' THEN '( ISNULL(NULLIF(NULLIF(CONVERT(varchar(100),t.Answer), ''0''), ''-1''), '''') <> '''' and t.CustomField = ''' + @Column + ''')' 
								END 
						 end	
					 if(@DataType = 'Standard') 
						begin
							while(PATINDEX ('% ,%',@Value)>0 or PATINDEX ('%, %',@Value)>0 )
								set @Value = LTRIM(RTRIM(REPLACE(replace(@Value,' ,',','),', ',',')))						
							set @AdhocString = 
								CASE @Value
									WHEN 'No Response' THEN ' (ps.[' + @Column + '] = '''' or ps.[' + @Column + '] is null)'
								ELSE 
								(CASE @Condition
									WHEN 'Equal' THEN '( ps.' + @Column  + ' = '''+ REPLACE(@Value, ',', ''' or ps.' + @Column + ' =  ''')+ ''') ' 
									WHEN 'Contains' THEN '( ps.' + @Column  + ' like ''%'+ REPLACE(@Value, ',', '%'' or ps.' + @Column + ' like  ''%')+ '%'') ' 
									WHEN 'Start With' THEN '( ps.' + @Column  + ' like '''+ REPLACE(@Value, ',', '%'' or ps.' + @Column + ' like  ''')+ '%'') '
									WHEN 'End With' THEN '( ps.' + @Column  + ' like ''%'+ REPLACE(@Value, ',', ''' or ps.' + @Column + ' like  ''%')+ ''') '
									WHEN 'Does Not Contain' THEN '( ps.' + @Column  + ' not like ''%'+ REPLACE(@Value, ',', '%'' or ps.' + @Column + ' not like  ''%')+ '%'' or ' +  @Column + ' is null ) '
									WHEN 'No Response' THEN '( ISNULL(NULLIF(NULLIF(CONVERT(varchar(100),ps.' + @Column + '), ''0''), ''-1''), '''') = '''')' 
									WHEN 'Any Response' THEN '( ISNULL(NULLIF(NULLIF(CONVERT(varchar(100),ps.' + @Column + '), ''0''), ''-1''), '''') <> '''')' 
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
												WHEN 'DateRange' THEN (case when @ValueTo is null then 'ps.' + @Column + ' >= ''' + @ValueFrom + '''' 
												when @ValueFrom is null then 'ps.' + @Column + ' <= ''' + @ValueTo + '''' 
												else 'ps.' + @Column + ' >= ''' + @ValueFrom + ''' and ps.' + @Column + ' <= ''' + @ValueTo + ''''  END)
												WHEN 'Year' THEN case when @ValueTo is null then 'year(ps.' + @Column + ') >= ''' + @ValueFrom + '''' else 'year(ps.' + @Column + ') >= ''' + @ValueFrom + ''' and year(ps.' + @Column + ') <= ''' + @ValueTo + ''''  END
												WHEN 'Month' THEN case when @ValueTo is null then 'month(ps.' + @Column + ') >= ''' + @ValueFrom + '''' else 'moonth(ps.' + @Column + ') >= ''' + @ValueFrom + ''' and month(ps.' + @Column + ') <= ''' + @ValueTo + ''''  END
										 END 
								 end				 
						 end		
				 
					if(@DataType = 'Range') 
						begin
							 set @AdhocString = 
								 CASE  @Condition
										WHEN 'Range' THEN case when @ValueTo is null then '(ps.' + @Column + ') >= ' + @ValueFrom else '(ps.' + @Column + ') >= ' + @ValueFrom + ' and (ps.' + @Column + ') <= ' + @ValueTo  END
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

PRINT(@executeString)

EXEC(@executeString)

DROP TABLE #AdHoc

END

