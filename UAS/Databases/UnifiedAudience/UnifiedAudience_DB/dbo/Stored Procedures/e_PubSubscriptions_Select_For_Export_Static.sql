CREATE PROCEDURE [dbo].[e_PubSubscriptions_Select_For_Export_Static]
@ProductID int,
@Columns varchar(max),
@Subs TEXT
AS
BEGIN

	SET NOCOUNT ON

	--DECLARE @Subs varchar(max) = '<XML><S><ID>2005975</ID></S></XML>'
	--DECLARE @Columns varchar(max) = 'ps.[PubSubscriptionID],ps.[SequenceID],ps.[Batch],ps.[Email],ps.[FirstName],ps.[LastName],ps.[Company],ps.[Title],ps.[Address1],ps.[Address2],ps.[Address3],ps.[City],ps.[RegionCode],ps.[ZipCode],ps.[Plus4],ps.[Country],ps.[Phone],ps.[Mobile],ps.[Fax],ps.[Website],ps.[CategoryCode],ps.[TransactionCode],ps.[QSource],ps.[Qualificationdate],ps.[Par3C],ps.[Copies],ps.[Demo7],ps.[SubscriberSourceCode],ps.[OrigsSrc],ps.[WaveMailingID],ps.[IMBSeq],ps.[ReqFlag],ps.[MailPermission],ps.[FaxPermission],ps.[PhonePermission],ps.[OtherProductsPermission],ps.[EmailRenewPermission],ps.[ThirdPartyPermission],demos.[FUNCTION],demos.[BUSINESS],demos.[PRODUCTS],demos.[EMPLOY],demos.[PURCHASE],demos.[Subscription],adhoc.[test],adhoc.[blue]'
	--DECLARE @ProductID int = 1	 
	
	CREATE TABLE #Subs
	(  
		RowID int IDENTITY(1, 1)
	  ,[SubID] int
	)
	CREATE NONCLUSTERED INDEX IDX_Subs_SubID ON #Subs(SubID)
	DECLARE @docHandle int
	EXEC sp_xml_preparedocument @docHandle OUTPUT, @Subs  
	INSERT INTO #Subs 
	SELECT [SubID]
	FROM OPENXML(@docHandle,N'/XML/S')
	WITH
	(
		[SubID] nvarchar(256) 'ID'
	)
	EXEC sp_xml_removedocument @docHandle
	
	set @Columns = REPLACE(@Columns,'ps.[CategoryCode]', '(SELECT CategoryCodeValue FROM UAD_Lookup..CategoryCode WITH(NOLOCK) WHERE ps.PubCategoryID = CategoryCodeID) as CategoryCode')
	set @Columns = REPLACE(@Columns,'ps.[TransactionCode]','(SELECT TransactionCodeValue FROM UAD_Lookup..TransactionCode WITH(NOLOCK) WHERE ps.PubTransactionID = TransactionCodeID) as TransactionCode')
	set @Columns = REPLACE(@Columns,'ps.[PubCategoryID]', '(SELECT CategoryCodeValue FROM UAD_Lookup..CategoryCode WITH(NOLOCK) WHERE ps.PubCategoryID = CategoryCodeID) as CategoryCode')
	set @Columns = REPLACE(@Columns,'ps.[PubTransactionID]','(SELECT TransactionCodeValue FROM UAD_Lookup..TransactionCode WITH(NOLOCK) WHERE ps.PubTransactionID = TransactionCodeID) as TransactionCode')
	set @Columns = REPLACE(@Columns, 'ps.[QSource]', 'q.CodeValue as ''QSource''')
	set @Columns = REPLACE(@Columns, 'sp.[PaymentType]', 'c.DisplayName as ''PaymentType''')
	set @Columns = REPLACE(@Columns, 'sp.[CreditCardType]', 'c2.DisplayName as ''CreditCardType''')
	set @Columns = REPLACE(@Columns, 'sp.[Payor Name]', 'sp.CCHolderName as ''Payor Name''')
	set @Columns = REPLACE(@Columns, 'ps.[Batch]', '(SELECT MAX(BatchNumber) FROM Batch b WITH(NOLOCK) JOIN History h WITH(NOLOCK) ON b.BatchID = h.BatchID WHERE h.PubSubscriptionID = ps.PubSubscriptionID) as ''Batch #''')
	set @Columns = REPLACE(@Columns, 'ps.[WaveMailingID]', 'ps.[WaveMailingID], wm.WaveMailingName')
	set @Columns = REPLACE(@Columns, 'ps.[Par3C]', '(SELECT DisplayOrder FROM UAD_Lookup..Code WITH(NOLOCK) WHERE CodeTypeId = (SELECT CodeTypeId FROM UAD_Lookup..CodeType WITH(NOLOCK) WHERE CodeTypeName = ''Par3C'') AND CodeId = ps.Par3CID ) as Par3C')
	set @Columns = REPLACE(@Columns, 'ps.[reqflag]', '(SELECT codevalue FROM UAD_Lookup..Code WITH(NOLOCK) where codeid = ps.reqflag) as ReqFlag')
	set @Columns = REPLACE(@Columns, 'ps.[Exp_Qdate]', 'cast(cast(isnull(sp.ExpireIssueDate, ps.Qualificationdate) as date)as varchar(50)) as ExpQdate')
	set @Columns = REPLACE(@Columns,'ps.[QualificationDate]',' cast(cast(ps.[QualificationDate] as date) as varchar(50)) as QualificationDate ')
	set @Columns = REPLACE(@Columns,'sp.[ExpireIssueDate]',' cast(cast(sp.[ExpireIssueDate] as date) as varchar(50)) as ExpireIssueDate ')
	
	DECLARE @executeString varchar(max) = ''
	DECLARE @rgcolumns varchar(4000)
	DECLARE @rgselect varchar(4000)
	SELECT @rgcolumns = STUFF( (SELECT ',' + '['+ DisplayName + ']' FROM ResponseGroups WITH(NOLOCK) 
	WHERE PubID = @ProductID AND ResponseGroupTypeID in (182,183,184) for XML PATH('')),1,1,'')	
	SET @rgselect = REPLACE(@rgcolumns, '[', 'demos.[')
	
	--set @Columns = REPLACE(@Columns, 'demos.[Permissions]', 'ps.MailPermission, ps.FaxPermission, ps.PhonePermission, ps.OtherProductsPermission, ps.EmailRenewPermission, ps.ThirdPartyPermission,
	--ps.TextPermission')
	
	IF(CHARINDEX('adhoc', @Columns)) > 0
	BEGIN
		CREATE TABLE #tmpAdHocs (PubsubscriptionID int, CustomField varchar(MAX), Answers varchar(100), PubID int)
		CREATE NONCLUSTERED INDEX IDX_tmpAdHocs_PubsubscriptionID ON #tmpAdHocs(PubsubscriptionID)
		CREATE NONCLUSTERED INDEX IDX_tmpAdHocs_Answers ON #tmpAdHocs(Answers)

		DECLARE @fields varchar(max) =  (
		SELECT 
		STUFF((SELECT ', ' + COLUMN_NAME 
			  FROM INFORMATION_SCHEMA.COLUMNS
			  WHERE TABLE_NAME = 'PubSubscriptionsExtension' AND COLUMN_NAME LIKE 'Field%'
			  FOR XML PATH('')), 1, 1, '') [FIELDS])
			  
		INSERT INTO #tmpAdHocs
		EXEC('
		WITH CTE AS
		(
		SELECT PubSubscriptionID, 
		Fields, 
		Answers 
		FROM PubSubscriptionsExtension pe WITH(NOLOCK)
		join #Subs s on s.SubID = pe.PubSubscriptionID
		UNPIVOT (Answers FOR Fields IN (' + @fields + '))up
		)
		SELECT cte.PubSubscriptionID, cte.Answers, pem.CustomField, pem.PubID 
		FROM CTE
		LEFT JOIN PubSubscriptions ps WITH(NOLOCK) ON ps.PubSubscriptionID = CTE.PubSubscriptionID
		JOIN PubSubscriptionsExtensionMapper pem WITH(NOLOCK) ON pem.StandardField = cte.Fields AND pem.PubID = ps.PubID')
			  
		DECLARE @adhocColumns table (Col varchar(max), NoFormatCol varchar(100))

		Insert into @adhocColumns (Col)
		SELECT Item = y.i.value('(./text())[1]', 'nvarchar(4000)')
			  FROM 
			  ( 
				SELECT x = CONVERT(XML, '<i>' 
				  + REPLACE(@Columns, ',', '</i><i>') 
				  + '</i>').query('.')
			  ) AS a CROSS APPLY x.nodes('i') AS y(i)

		delete from @adhocColumns where Col not like '%adhoc%'

		Update a
		Set NoFormatCol = SUBSTRING(Col, CHARINDEX('[', Col) + 1, CHARINDEX(']',Col) - CHARINDEX('[', Col) - Len(']')) 
		from @adhocColumns a
			  
		declare @cols varchar(max) = STUFF((SELECT ',' + '[' + NoFormatCol + ']'
							from @adhocColumns
					FOR XML PATH(''), TYPE
					).value('.', 'NVARCHAR(MAX)') 
				,1,1,'')
		--select @cols
		--declare @cols varchar(max) = STUFF((SELECT ',' + QUOTENAME(Answers) 
		--					from #tmpAdHocs
		--					group by Answers
		--					order by Answers
		--			FOR XML PATH(''), TYPE
		--			).value('.', 'NVARCHAR(MAX)') 
		--		,1,1,'')
				
		set @executeString +=
		' WITH CTE AS
		(
		SELECT PubSubscriptionID, 
		Fields, 
		Answers 
		FROM PubSubscriptionsExtension pe WITH(NOLOCK)
		join #Subs s on s.SubID = pe.PubSubscriptionID
		UNPIVOT (Answers FOR Fields IN (' + @fields + '))up
		)
		SELECT cte.PubSubscriptionID, cte.Answers, pem.CustomField, pem.PubID 
		INTO #tmpAdHocs
		FROM CTE
		LEFT JOIN PubSubscriptions ps WITH(NOLOCK) ON ps.PubSubscriptionID = CTE.PubSubscriptionID
		JOIN PubSubscriptionsExtensionMapper pem WITH(NOLOCK) ON pem.StandardField = cte.Fields AND pem.PubID = ps.PubID'
	END
	
	SET @executeString += '
	
	SELECT ' + @Columns +
	' FROM PubSubscriptions ps with(NOLOCK)' +
	' JOIN Pubs p with(nolock) on ps.PubID = p.PubID' +
	' JOIN Subscriptions s with(NOLOCK) ON ps.SubscriptionID = s.SubscriptionID' +
	' JOIN #Subs sub ON sub.SubID = ps.PubSubscriptionID'

	IF(CHARINDEX('QSource', @Columns)) > 0
	BEGIN
		set @executeString += ' LEFT JOIN UAD_Lookup..Code q with(NOLOCK) ON q.CodeId = ps.PubQSourceID '
	END
	IF(CHARINDEX('sp.', @Columns)) > 0
	BEGIN
		set @executeString += ' LEFT JOIN SubscriptionPaid sp with(NOLOCK) ON sp.PubSubscriptionID = ps.PubSubscriptionID'
	END
	IF(CHARINDEX('PaymentType', @Columns)) > 0
	BEGIN
		set @executeString += ' LEFT JOIN UAD_Lookup..Code c with(NOLOCK) ON c.CodeID = sp.PaymentTypeID '
	END
	IF(CHARINDEX('CreditCardType', @Columns)) > 0
	BEGIN
		set @executeString += ' LEFT JOIN UAD_Lookup..Code c2 with(NOLOCK) ON c2.CodeID = sp.CreditCardTypeID '
	END
	IF(CHARINDEX('WaveMailingID', @Columns)) > 0
	BEGIN
		set @executeString += ' LEFT JOIN WaveMailing wm with(NOLOCK) ON ps.WaveMailingID = wm.WaveMailingID '
	END
	IF(CHARINDEX('demos', @Columns)) > 0
	BEGIN
		set @executeString +=
		' LEFT OUTER JOIN
		(SELECT * FROM (
		SELECT psd.PubSubscriptionID, rg.DisplayName, STUFF( (SELECT '','' +  cs1.Responsevalue + ISNULL(CASE WHEN cs1.IsOther = 1 THEN '' - '' + psd.ResponseOther ELSE '''' END, '''') FROM PubSubscriptionDetail psd WITH(NOLOCK)
						JOIN #Subs s1 ON psd.PubSubscriptionID = s1.SubID
						JOIN CodeSheet cs1 with(NOLOCK) ON cs1.CodeSheetID = psd.CodesheetID
			where cs1.ResponseGroup = cs2.ResponseGroup and s2.SubID = s1.SubID for XML PATH('''')),1,1,'''') as Responsevalue
			from PubSubscriptionDetail psd WITH(NOLOCK)
			join #Subs s2 on s2.SubID = psd.PubSubscriptionID
			join CodeSheet cs2 WITH(NOLOCK) on cs2.CodeSheetID = psd.CodesheetID
			join ResponseGroups rg WITH(NOLOCK) on rg.ResponseGroupID = cs2.ResponseGroupID	
			group by rg.DisplayName, cs2.ResponseGroup, psd.PubSubscriptionID, s2.SubID
			) u 
			PIVOT
			(
				MAX(Responsevalue)
				FOR u.DisplayName in (' + @rgcolumns + ')
			) p
		) demos ON demos.PubSubscriptionID = ps.PubSubscriptionID'
	END
	IF(CHARINDEX('adhoc', @Columns)) > 0
	BEGIN
		set @executeString +=        
		' LEFT OUTER JOIN (
		Select *
		from
		(
			select CustomField, Answers, PubSubscriptionID
			from #tmpAdHocs
		) src
		pivot
		(
			max(Answers)
			for CustomField in (' + @cols + ')
		) piv
		)adhoc on adhoc.PubsubscriptionID = ps.PubSubscriptionID'
	END
	
	set @executeString += ' WHERE ps.PubID = ' + convert(varchar(10),@ProductID)
	
	--print(@executeString)
	exec(@executeString)
	DROP TABLE #Subs
	IF OBJECT_ID('tempdb..#tmpAdHocs') IS NOT NULL DROP TABLE #tmpAdHocs

END