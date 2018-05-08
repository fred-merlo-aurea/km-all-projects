--------------------------------------------------------------------
-- 2014-11-05 MK Updated to ignore spaces and special characters
-- 2014-11-06 MK Updated to run string match in 2 passes, exact then without spaces and special characters
--	Requires function fn_StripNonAlphaNumerics in Master
-- 
--------------------------------------------------------------------
CREATE PROC dbo.job_CodesheetValidation
(
	@sourcefileID INT,
	@ProcessCode VARCHAR(50)
)
AS
BEGIN   

	SET NOCOUNT ON 
		
	DECLARE @I INT
		
	SET @I = 1
	
	PRINT (' Start job_CodesheetValidation : ' +  CONVERT(VARCHAR,@i) + ' / ' + CONVERT(VARCHAR,@sourcefileID) + ' / ' + @processcode  + ' / ' +  CONVERT(VARCHAR(20), GETDATE(), 114))

	CREATE TABLE #Tmp_Codesheet 
	(
		CodesheetID INT, 
		PubID INT, 
		ResponseGroup VARCHAR(100),
		Responsevalue VARCHAR(200)
	)

	CREATE INDEX IDX_tmpcodesheet ON #Tmp_Codesheet(pubID,responsegroup,responsevalue )

	CREATE TABLE #tmpInvalidMAFFieldID
	(
		SubscriberDemographicTransformedID INT,
		RowNumber INT,
		PubID INT,
		MAFField VARCHAR(255),
		Value VARCHAR(max)
	)
	
	CREATE CLUSTERED INDEX PK_tmpInvalidMAFFieldID_SubscriberDemographicTransformedID ON #tmpInvalidMAFFieldID(SubscriberDemographicTransformedID)
	
	CREATE TABLE #IncomingDataDetails
	(
		SubscriberDemographicTransformedID INT,
		RowNumber INT,
		PubID INT,
		MAFField VARCHAR(255),
		Value VARCHAR(MAX),
		[NotExists] [bit] DEFAULT 1
	)

	CREATE CLUSTERED INDEX PK_IncominDataDetails ON #IncomingDataDetails(SubscriberDemographicTransformedID)
	
	CREATE NONCLUSTERED INDEX IDX_IncomingDataDetails ON #IncomingDataDetails (NotExists ASC, SubscriberDemographicTransformedID ASC)
	
	INSERT INTO #tmpInvalidMAFFieldID
	SELECT DISTINCT SubscriberDemographicTransformedID,
		st.ImportRowNumber, 
		sdt.PubID, 
		MAFField, 
		Value
	FROM dbo.SubscriberTransformed st WITH (NOLOCK)
		JOIN dbo.SubscriberDemographicTransformed sdt WITH (NOLOCK) ON st.STRecordIdentifier = sdt.STRecordIdentifier
		LEFT OUTER JOIN ResponseGroups rg WITH (NOLOCK) ON sdt.PubID = rg.PubID AND sdt.MAFField = rg.ResponseGroupName
	WHERE st.SourceFileID = @sourceFileID 
		AND st.ProcessCode = @ProcessCode 
		AND LEN(sdt.value) > 0
		AND MAFField NOT IN (SELECT CustomField FROM SubscriptionsExtensionMapper with(nolock) WHERE Active = 1 union SELECT CustomField FROM PubSubscriptionsExtensionMapper with(nolock) WHERE Active = 1)
		AND rg.ResponseGroupID IS NULL
		AND sdt.IsAdhoc = 'false'
		AND sdt.IsDemoDate = 'false'

	UPDATE sdt
	SET NotExists = 1,
		NotExistReason = 'Dimension or AdHoc does not exist'
	FROM SubscriberDemographicTransformed sdt 
		JOIN #tmpInvalidMAFFieldID idata ON sdt.SubscriberDemographicTransformedID = idata.SubscriberDemographicTransformedID

	PRINT (' update sdt /  Dimension or AdHoc does not exist : ' + ' / ' +  CONVERT(VARCHAR,@@ROWCOUNT) +   ' / ' +  CONVERT(VARCHAR(20), GETDATE(), 114))

	DROP TABLE #tmpInvalidMAFFieldID	
	
	DECLARE @rgname VARCHAR(50),
			@STRecordIdentifier UNIQUEIDENTIFIER, 
			@pubID INT, 
			@responsevalue VARCHAR(MAX)
	
	DECLARE c_ProductDemographics CURSOR FOR 
	SELECT DISTINCT rg.responsegroupname
	FROM dbo.SubscriberTransformed st WITH (NOLOCK)
		JOIN dbo.SubscriberDemographicTransformed sdt WITH (NOLOCK) on st.STRecordIdentifier = sdt.STRecordIdentifier
		JOIN ResponseGroups rg WITH (NOLOCK) on sdt.PubID = rg.PubID AND sdt.MAFField = rg.ResponseGroupName
	WHERE st.SourceFileID = @sourceFileID 
		and	st.ProcessCode = @ProcessCode

	OPEN c_ProductDemographics  
	FETCH NEXT FROM c_ProductDemographics INTO @rgname

	WHILE @@FETCH_STATUS = 0  
	BEGIN  	
				
		INSERT INTO #IncomingDataDetails (
			SubscriberDemographicTransformedID, 
			RowNumber, 
			pubid, 
			MAFField, 
			Value, 
			NotExists)
		SELECT distinct sdt.SubscriberDemographicTransformedID, 
			st.ImportRowNumber, 
			sdt.PubID, 
			@rgname, 
			(CASE WHEN LEN(sdt.value)= 1 AND ISNUMERIC(sdt.value) = 1 THEN '0' + sdt.value ELSE sdt.value END), 
			1 
		FROM dbo.SubscriberTransformed st WITH (NOLOCK)
			JOIN dbo.SubscriberDemographicTransformed sdt WITH (NOLOCK) on st.STRecordIdentifier = sdt.STRecordIdentifier
			JOIN ResponseGroups rg WITH (NOLOCK) on sdt.PubID = rg.PubID
		WHERE st.SourceFileID = @sourceFileID 
			AND st.ProcessCode = @ProcessCode 
			AND rg.ResponseGroupName = @rgname 
			AND MAFField = @rgname 
			AND LEN(sdt.value) > 0
			AND sdt.IsAdhoc = 'false'
			AND sdt.IsDemoDate = 'false'

		PRINT (' INSERT INTO #IncomingDataDetails / ' + CONVERT(VARCHAR,@i) + ' / ' + @rgname + ' / ' +  CONVERT(VARCHAR,@@ROWCOUNT) + ' / ' + ' start / ' + CONVERT(VARCHAR(20), GETDATE(), 114) )  
		
		INSERT INTO #Tmp_Codesheet (
			CodesheetID, 
			PubID,
			ResponseGroup,
			Responsevalue)
		SELECT c.codesheetID, 
			rg.PubID, 
			rg.ResponseGroupName,  
			(CASE WHEN LEN(responsevalue)= 1 AND ISNUMERIC(responsevalue) = 1 THEN '0' + Responsevalue ELSE responsevalue END) AS Responsevalue
		FROM codesheet c with(nolock) 
			JOIN ResponseGroups rg with(nolock) ON c.ResponseGroupID = rg.ResponseGroupID 
		WHERE rg.ResponseGroupName = @rgname
		
		PRINT (' INSERT INTO #Tmp_Codesheet / ' + CONVERT(VARCHAR,@i) + ' / ' + @rgname + ' / ' +  CONVERT(VARCHAR,@@ROWCOUNT) + ' / ' + ' start / ' + CONVERT(VARCHAR(20), GETDATE(), 114) )  
		
		SET @i = @i + 1
		
		FETCH NEXT FROM c_ProductDemographics INTO  @rgname
	END

	CLOSE c_ProductDemographics  
	DEALLOCATE c_ProductDemographics  
	
	IF EXISTS (SELECT TOP 1 SubscriberDemographicTransformedID FROM #IncomingDataDetails)
		BEGIN
	
			PRINT ('Codesheet Validation Start / ' + CONVERT(VARCHAR(20), GETDATE(), 114) )

	--1st Pass exact string match
			UPDATE #IncomingDataDetails 
			SET notexists = 0
			WHERE SubscriberDemographicTransformedID in 
			(
				SELECT SubscriberDemographicTransformedID
				FROM #IncomingDataDetails idata
					LEFT OUTER JOIN #Tmp_Codesheet c ON c.pubID = idata.pubID 
						AND c.responsegroup = idata.MAFField 
						AND c.ResponseValue =  idata.value
				WHERE c.CodeSheetID IS NOT NULL 
			)

	--2nd pass match on sting minus spaces AND all special characters if string match failed
			UPDATE #IncomingDataDetails 
			SET notexists = 0
			WHERE SubscriberDemographicTransformedID in 
			(
				SELECT SubscriberDemographicTransformedID
				FROM #IncomingDataDetails idata
					LEFT OUTER JOIN #Tmp_Codesheet c ON c.pubID = idata.pubID 
						AND c.responsegroup = idata.MAFField 
						AND NotExists = 1  
						AND	master.dbo.fn_StripNonAlphaNumerics(c.ResponseValue) = master.dbo.fn_StripNonAlphaNumerics(idata.value)
				WHERE c.CodeSheetID IS NOT NULL 
			)
			PRINT ('Codesheet Validation End/ ' + CONVERT(VARCHAR(20), GETDATE(), 114) )
			
			PRINT ('Update SubscriberDemographicTransformed Start / '    +  CONVERT(VARCHAR(20), GETDATE(), 114) )	
		
			UPDATE sdt
			SET NotExists = idata.NotExists,
				NotExistReason = 'Codesheet value does not match'
			FROM SubscriberDemographicTransformed sdt 
				JOIN #IncomingDataDetails idata ON sdt.SubscriberDemographicTransformedID = idata.SubscriberDemographicTransformedID
			WHERE idata.NotExists = 1

			PRINT ('Update SubscriberDemographicTransformed End / ' + CONVERT(VARCHAR,@@rowcount) + '  /  '  + CONVERT(VARCHAR(20), GETDATE(), 114) )	
		
	
		END
	
	/* insert notexists = 1 to ImportError table */ -- Client message = bad codes.
	
	INSERT INTO ImportError (
		SourceFileID,
		RowNumber,
		FormattedException,
		ClientMessage,
		MAFField,
		BadDataRow,
		ThreadID,
		DateCreated,
		ProcessCode,
		IsDimensionError)
	
	SELECT st.SourceFileID,
		st.ImportRowNumber,
		'',
		sdt.NotExistReason,
		sdt.MAFField,
		sdt.Value,
		-1,
		st.DateCreated,
		st.ProcessCode,
		'true'
	--ISNULL(SUBSTRING((SELECT ( ', ' + d.Value)
	--						   FROM SubscriberDemographicTransformed d WITH(NOLOCK)
	--						   WHERE st.STRecordIdentifier = d.STRecordIdentifier
	--						   AND d.NotExists = 'true'
	--						   FOR XML PATH( '' )
	--						  ), 3, 1000 ),-1)
	FROM SubscriberTransformed st WITH(NOLOCK)
		JOIN SubscriberDemographicTransformed sdt WITH(NOLOCK) ON st.STRecordIdentifier = sdt.STRecordIdentifier
	WHERE st.SourceFileID = @sourcefileID
		AND st.ProcessCode = @ProcessCode
		AND sdt.NotExists = 'true'
	
	DROP TABLE #incomingdatadetails
	
	DROP TABLE #Tmp_Codesheet
	
	PRINT (' Done  job_CodesheetValidation : ' + ' / ' +  CONVERT(VARCHAR(20), GETDATE(), 114))

END
GO