CREATE PROCEDURE [dbo].[e_IssueArchiveSubscription_SelectForExportStatic_IssueID]
@ProductID int,
@Columns varchar(max),
@Subs TEXT,
@IssueID int
AS
	--DECLARE @Subs varchar(max) = '<XML><S><ID>962620</ID></S></XML>'
	--DECLARE @Columns varchar(max) = 'ps.[PubSubscriptionID],ps.[SequenceID],ps.[Batch],ps.[Email],ps.[FirstName],ps.[LastName],ps.[Company],ps.[Title],ps.[Address1],ps.[Address2],ps.[Address3],ps.[City],ps.[RegionCode],ps.[ZipCode],ps.[Plus4],ps.[Country],ps.[Phone],ps.[Mobile],ps.[Fax],ps.[Website],ps.[CategoryCode],ps.[TransactionCode],ps.[QSource],ps.[Qualificationdate],ps.[Par3C],ps.[Copies],ps.[Demo7],ps.[SubscriberSourceCode],ps.[OrigsSrc],ps.[WaveMailingID],ps.[IMBSeq],ps.[ReqFlag],ps.[MailPermission],ps.[FaxPermission],ps.[PhonePermission],ps.[OtherProductsPermission],ps.[EmailRenewPermission],ps.[ThirdPartyPermission],demos.[FUNCTION],demos.[BUSINESS]'
	--DECLARE @ProductID int = 2	 
	--declare @issueid varchar(10) = 10
	
	CREATE TABLE #Subs
	(  
		RowID int IDENTITY(1, 1)
	  ,[SubID] int
	)
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
	
	set @Columns = REPLACE(@Columns,'ps.[CategoryCode]', '(SELECT CategoryCodeValue FROM UAD_Lookup..CategoryCode WHERE ps.PubCategoryID = CategoryCodeID) as CategoryCode')
	set @Columns = REPLACE(@Columns,'ps.[TransactionCode]','(SELECT TransactionCodeValue FROM UAD_Lookup..TransactionCode WHERE ps.PubTransactionID = TransactionCodeID) as TransactionCode')
	set @Columns = REPLACE(@Columns,'ps.[PubCategoryID]', '(SELECT CategoryCodeValue FROM UAD_Lookup..CategoryCode WHERE ps.PubCategoryID = CategoryCodeID) as CategoryCode')
	set @Columns = REPLACE(@Columns,'ps.[PubTransactionID]','(SELECT TransactionCodeValue FROM UAD_Lookup..TransactionCode WHERE ps.PubTransactionID = TransactionCodeID) as TransactionCode')
	set @Columns = REPLACE(@Columns, 'ps.[QSource]', 'q.CodeValue as ''QSource''')
	set @Columns = REPLACE(@Columns, 'sp.[PaymentType]', 'c.DisplayName as ''PaymentType''')
	set @Columns = REPLACE(@Columns, 'sp.[CreditCardType]', 'c2.DisplayName as ''CreditCardType''')
	set @Columns = REPLACE(@Columns, 'sp.[Payor Name]', 'sp.CCHolderName as ''Payor Name''')
	set @Columns = REPLACE(@Columns, 'ps.[Batch]', '(SELECT isnull(MAX(BatchNumber),0) FROM Batch b JOIN History h ON b.BatchID = h.BatchID WHERE h.PubSubscriptionID = ps.PubSubscriptionID) as ''Batch #''')
	set @Columns = REPLACE(@Columns, 'ps.[WaveMailingID]', 'ps.[WaveMailingID], wm.WaveMailingName')
	set @Columns = REPLACE(@Columns, 'ps.[Par3C]', '(SELECT DisplayOrder FROM UAD_Lookup..Code WHERE CodeTypeId = (SELECT CodeTypeId FROM UAD_Lookup..CodeType WHERE CodeTypeName = ''Par3C'') AND CodeId = ps.Par3CID ) as Par3C')
	set @Columns = REPLACE(@Columns, 'ps.[reqflag]', '(SELECT codevalue FROM UAD_Lookup..Code where codeid = ps.reqflag) as ReqFlag')
	set @Columns = REPLACE(@Columns, 'ps.[Exp_Qdate]', 'isnull(sp.ExpireIssueDate, ps.Qualificationdate) as Exp_Qdate')

	DECLARE @executeString varchar(max) = ''
	DECLARE @rgcolumns varchar(4000)
	DECLARE @rgselect varchar(4000)
	SELECT @rgcolumns = STUFF( (SELECT ',' + '['+ DisplayName + ']' FROM ResponseGroups 
	WHERE PubID = @ProductID AND ResponseGroupTypeID in (182,183,184) for XML PATH('')),1,1,'')	
	SET @rgselect = REPLACE(@rgcolumns, '[', 'demos.[')
	
	--set @Columns = REPLACE(@Columns, 'demos.[Permissions]', 'ps.MailPermission, ps.FaxPermission, ps.PhonePermission, ps.OtherProductsPermission, ps.EmailRenewPermission, ps.ThirdPartyPermission,
	--ps.TextPermission')
	
	IF(CHARINDEX('adhoc', @Columns)) > 0
	BEGIN
		CREATE TABLE #tmpAdHocs (IssueArchiveSubscriptionId int, PubsubscriptionID int, CustomField varchar(100), Answers varchar(100), PubID int)
		DECLARE @fields varchar(max) =  (
		SELECT 
		STUFF((SELECT ', ' + COLUMN_NAME 
			  FROM INFORMATION_SCHEMA.COLUMNS
			  WHERE TABLE_NAME = 'IssueArchivePubSubscriptionsExtension' AND COLUMN_NAME LIKE 'Field%'
			  FOR XML PATH('')), 1, 1, '') [FIELDS])
			  
		INSERT INTO #tmpAdHocs
		EXEC('
		WITH CTE AS
		(
		SELECT IssueArchiveSubscriptionId, 
		Fields, 
		Answers 
		FROM IssueArchivePubSubscriptionsExtension pe with(nolock)
		UNPIVOT (Answers FOR Fields IN (' + @fields + '))up
		)
		SELECT cte.IssueArchiveSubscriptionId, ps.PubSubscriptionID, cte.Answers, pem.CustomField, pem.PubID 
		FROM CTE
		LEFT JOIN IssueArchiveProductSubscription ps with(nolock) ON ps.IssueArchiveSubscriptionId = CTE.IssueArchiveSubscriptionId and ps.issueid = ' + @IssueID + '
		JOIN PubSubscriptionsExtensionMapper pem with(nolock) ON pem.StandardField = cte.Fields AND pem.PubID = ps.PubID
		JOIN #Subs s on ps.PubSubscriptionID = s.SubID')

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
		SELECT IssueArchiveSubscriptionId, 
		Fields, 
		Answers 
		FROM IssueArchivePubSubscriptionsExtension pe with(nolock)
		UNPIVOT (Answers FOR Fields IN (' + @fields + '))up
		)
		SELECT cte.IssueArchiveSubscriptionId, ps.PubSubscriptionID, cte.Answers, pem.CustomField, pem.PubID 
		INTO #tmpAdHocs
		FROM CTE
		LEFT JOIN IssueArchiveProductSubscription ps with(nolock) ON ps.IssueArchiveSubscriptionId = CTE.IssueArchiveSubscriptionId and issueid = ' + convert(varchar(10), @IssueID) + '
		JOIN PubSubscriptionsExtensionMapper pem with(nolock) ON pem.StandardField = cte.Fields AND pem.PubID = ps.PubID
		JOIN #Subs s on ps.PubSubscriptionID = s.SubID'
	END

	SET @executeString += '
	
	SELECT ' + @Columns +
	' FROM IssueArchiveProductSubscription ps with(NOLOCK)' +
	' JOIN Pubs p with(nolock) on ps.PubID = p.PubID' +
	' LEFT JOIN Subscriptions s with(NOLOCK) ON ps.SubscriptionID = s.SubscriptionID' +
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
		SELECT psd.PubSubscriptionID, rg.DisplayName, STUFF( (SELECT distinct '','' +  cs1.Responsevalue + ISNULL(CASE WHEN cs1.IsOther = 1 THEN '' - '' + psd.ResponseOther ELSE '''' END, '''') 
						FROM IssueArchiveProductSubscriptiondetail psd with(nolock) 
						join IssueArchiveProductSubscription ps with(nolock) on psd.IssueArchiveSubscriptionId = ps.IssueArchiveSubscriptionId and ps.issueid = ' + convert(varchar(10), @IssueID) + ' 
						JOIN #Subs s1 ON psd.PubSubscriptionID = s1.SubID
						JOIN CodeSheet cs1 with(NOLOCK) ON cs1.CodeSheetID = psd.CodesheetID
						where cs1.ResponseGroup = cs2.ResponseGroup and s2.SubID = s1.SubID for XML PATH('''')),1,1,'''') as Responsevalue
			from IssueArchiveProductSubscriptiondetail psd with(nolock)
			join #Subs s2 on s2.SubID = psd.PubSubscriptionID
			join CodeSheet cs2 with(nolock) on cs2.CodeSheetID = psd.CodesheetID
			join ResponseGroups rg with(nolock) on rg.ResponseGroupID = cs2.ResponseGroupID
			join IssueArchiveProductSubscription ps with(nolock) on psd.IssueArchiveSubscriptionId = ps.IssueArchiveSubscriptionId and ps.issueid = ' + convert(varchar(10), @IssueID) + '
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
	
	set @executeString += ' WHERE ps.PubID = ' + convert(varchar(10),@ProductID) + ' and ps.issueid = ' + convert(varchar(10), @IssueID)
	
	--print(@executeString)
	exec(@executeString)
	DROP TABLE #Subs
	IF OBJECT_ID('tempdb..#tmpAdHocs') IS NOT NULL DROP TABLE #tmpAdHocs