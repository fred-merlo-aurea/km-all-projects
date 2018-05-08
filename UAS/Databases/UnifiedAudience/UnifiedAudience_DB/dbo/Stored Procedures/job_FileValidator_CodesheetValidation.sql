--------------------------------------------------------------------
-- 2014-11-14 MK Updated to run string match in 2 passes, exact then without spaces and special characters
--	Requires function fn_StripNonAlphaNumerics in Master
-- 
--------------------------------------------------------------------
CREATE PROCEDURE [dbo].[job_FileValidator_CodesheetValidation]
	(
	@sourcefileID int,
	@ProcessCode varchar(50)
)
as
BEGIN   

	SET NOCOUNT ON 

	declare @i int
			
	set @i = 1
	
	print (' Start job_CodesheetValidation : ' +  convert(varchar,@i) + ' / ' + convert(varchar,@sourcefileID) + ' / ' + @processcode  + ' / ' +  convert(varchar(20), getdate(), 114))

	CREATE TABLE #Tmp_Codesheet (CodesheetID int, PubID int, ResponseGroup varchar(100),Responsevalue varchar(200))

	CREATE INDEX IDX_tmpcodesheet ON #Tmp_Codesheet(pubID,responsegroup,responsevalue )

	CREATE TABLE #tmpInvalidMAFFieldID
	(
		FV_DemographicTransformedID int,
		RowNumber int,
		PubID int,
		MAFField varchar(255),
		Value varchar(max)
	)
	
	CREATE CLUSTERED INDEX PK_tmpInvalidMAFFieldID_FV_DemographicTransformedID ON #tmpInvalidMAFFieldID(FV_DemographicTransformedID)
	
	CREATE TABLE #IncomingDataDetails
	(
		FV_DemographicTransformedID int,
		RowNumber int,
		PubID int,
		MAFField varchar(255),
		Value varchar(max),
		[NotExists] [bit] default 1
	)

	CREATE CLUSTERED INDEX PK_IncominDataDetails ON #IncomingDataDetails(FV_DemographicTransformedID)
	
	CREATE NONCLUSTERED INDEX IDX_IncomingDataDetails ON #IncomingDataDetails
	(
		[NotExists] ASC,
		FV_DemographicTransformedID ASC
	)
	
	insert into #tmpInvalidMAFFieldID
	select distinct FV_DemographicTransformedID,st.ImportRowNumber, sdt.PubID, MAFField, Value
	from 
			dbo.FileValidator_Transformed st with (NOLOCK)
			join dbo.FileValidator_DemographicTransformed sdt with (NOLOCK) on st.STRecordIdentifier = sdt.STRecordIdentifier
			left outer join ResponseGroups rg with (NOLOCK) on sdt.PubID = rg.PubID and sdt.MAFField = rg.ResponseGroupName
	where	
			st.SourceFileID = @sourceFileID and
			st.ProcessCode = @ProcessCode and
			len(sdt.value) > 0
			and MAFField not in (select CustomField from SubscriptionsExtensionMapper where Active = 1)
			and rg.ResponseGroupID is null
					
	update sdt
	set NotExists = 1,NotExistReason = 'Dimension or AdHoc does not exist'
	from FileValidator_DemographicTransformed sdt join #tmpInvalidMAFFieldID idata on sdt.FV_DemographicTransformedID = idata.FV_DemographicTransformedID

	print (' update sdt /  Dimension or AdHoc does not exist : ' + ' / ' +  convert(varchar,@@ROWCOUNT) +   ' / ' +  convert(varchar(20), getdate(), 114))

	drop table #tmpInvalidMAFFieldID	
	
	
	declare @rgname varchar(50),
			@STRecordIdentifier uniqueidentifier, 
			@pubID int, 
			@responsevalue varchar(max)
	
	DECLARE c_ProductDemographics CURSOR FOR 
	select distinct rg.responsegroupname
		from dbo.FileValidator_Transformed st with (NOLOCK)
			join dbo.FileValidator_DemographicTransformed sdt with (NOLOCK) on st.STRecordIdentifier = sdt.STRecordIdentifier
			join ResponseGroups rg with (NOLOCK) on sdt.PubID = rg.PubID and sdt.MAFField = rg.ResponseGroupName
		where st.SourceFileID = @sourceFileID and
			st.ProcessCode = @ProcessCode

	OPEN c_ProductDemographics  
	FETCH NEXT FROM c_ProductDemographics INTO @rgname

	WHILE @@FETCH_STATUS = 0  
	BEGIN  	
				
		insert into #IncomingDataDetails	 (FV_DemographicTransformedID, RowNumber, pubid, MAFField, Value, NotExists)
		select distinct sdt.FV_DemographicTransformedID, st.ImportRowNumber, sdt.PubID, @rgname, 
		(case when LEN(sdt.value)= 1 and ISNUMERIC(sdt.value) = 1 then '0' + sdt.value else sdt.value end), 1 
		from dbo.FileValidator_Transformed st with (NOLOCK)
			join dbo.FileValidator_DemographicTransformed sdt with (NOLOCK) on st.STRecordIdentifier = sdt.STRecordIdentifier
			join ResponseGroups rg with (NOLOCK) on sdt.PubID = rg.PubID
		where st.SourceFileID = @sourceFileID and
			st.ProcessCode = @ProcessCode and
			rg.ResponseGroupName = @rgname and 
			MAFField = @rgname and 
			len(sdt.value) > 0
		
		print (' insert into #IncomingDataDetails / ' + convert(varchar,@i) + ' / ' + @rgname + ' / ' +  convert(varchar,@@ROWCOUNT) + ' / ' + ' start / ' + convert(varchar(20), getdate(), 114) )  
		
		insert into #Tmp_Codesheet (CodesheetID, PubID,ResponseGroup,Responsevalue)
		select c.codesheetID, rg.PubID, rg.ResponseGroupName,  (case when LEN(responsevalue)= 1 and ISNUMERIC(responsevalue) = 1 then '0' + Responsevalue else responsevalue end) as Responsevalue
		from codesheet c join ResponseGroups rg on c.ResponseGroupID = rg.ResponseGroupID 
		where rg.ResponseGroupName = @rgname
		
		print (' insert into #Tmp_Codesheet / ' + convert(varchar,@i) + ' / ' + @rgname + ' / ' +  convert(varchar,@@ROWCOUNT) + ' / ' + ' start / ' + convert(varchar(20), getdate(), 114) )  
		
		set @i = @i + 1
		
		FETCH NEXT FROM c_ProductDemographics INTO  @rgname
	END

	CLOSE c_ProductDemographics  
	DEALLOCATE c_ProductDemographics  
	
	if exists (select top 1 FV_DemographicTransformedID from #IncomingDataDetails)
	Begin
	
		print ('Codesheet Validation Start / ' + convert(varchar(20), getdate(), 114) )

--1st Pass exact string match
		UPDATE #IncomingDataDetails 
		SET notexists = 0
		WHERE FV_DemographicTransformedID IN 
		(
			SELECT FV_DemographicTransformedID
			FROM #IncomingDataDetails idata 
				LEFT OUTER JOIN #Tmp_Codesheet c ON c.pubID = idata.pubID 
					AND c.responsegroup = idata.MAFField 
					AND c.responsevalue = idata.Value
			WHERE c.CodeSheetID IS NOT NULL 
		)

--2nd pass match on sting minus spaces AND all special characters if string match failed
		UPDATE #IncomingDataDetails 
		SET notexists = 0
		WHERE FV_DemographicTransformedID IN 
		(
			SELECT FV_DemographicTransformedID
			FROM #IncomingDataDetails idata 
				LEFT OUTER JOIN #Tmp_Codesheet c ON c.pubID = idata.pubID 
					AND c.responsegroup = idata.MAFField 
					AND NotExists = 1  
					AND	master.dbo.fn_StripNonAlphaNumerics(c.ResponseValue) = master.dbo.fn_StripNonAlphaNumerics(idata.value)
			WHERE c.CodeSheetID IS NOT NULL 
		)


		print ('Codesheet Validation End/ ' + convert(varchar(20), getdate(), 114) )
			
		
		print ('Update SubscriberDemographicTransformed Start / '    +  convert(varchar(20), getdate(), 114) )	
		
		update sdt
		set NotExists = idata.NotExists,NotExistReason = 'Codesheet value does not match'
		from FileValidator_DemographicTransformed sdt 
			join #IncomingDataDetails idata on sdt.FV_DemographicTransformedID = idata.FV_DemographicTransformedID
		where idata.NotExists = 1

		print ('Update SubscriberDemographicTransformed End / ' + convert(varchar,@@rowcount) + '  /  '  + convert(varchar(20), getdate(), 114) )	
		
	
	end
	
	/* insert notexists = 1 to ImportError table */ -- Client message = bad codes.
	
	Insert into FileValidator_ImportError(SourceFileID,RowNumber,FormattedException,ClientMessage,MAFField,BadDataRow,ThreadID,DateCreated,ProcessCode,IsDimensionError)
	
	SELECT st.SourceFileID,st.ImportRowNumber,'',sdt.NotExistReason,
		sdt.MAFField,sdt.Value,-1,st.DateCreated,st.ProcessCode,'true'
	FROM FileValidator_Transformed st With(NoLock)
		JOIN FileValidator_DemographicTransformed sdt With(NoLock) ON st.STRecordIdentifier = sdt.STRecordIdentifier
	WHERE st.SourceFileID = @sourcefileID
		AND st.ProcessCode = @ProcessCode
		AND sdt.NotExists = 'true'
	
	drop table #incomingdatadetails
	
	drop table #Tmp_Codesheet
	
	print (' Done  job_CodesheetValidation : ' + ' / ' +  convert(varchar(20), getdate(), 114))

end
GO