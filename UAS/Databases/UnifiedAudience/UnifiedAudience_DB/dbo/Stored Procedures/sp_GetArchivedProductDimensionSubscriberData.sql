CREATE PROCEDURE [dbo].[sp_GetArchivedProductDimensionSubscriberData]
@Queries VARCHAR(MAX),
@SubscriptionFields XML,
@PubID int,
@IssueID int,
@ResponseGroupID XML,
@ResponseGroupID_Desc XML,
@PubSubscriptionsExtMapperValues XML,
@CustomColumns XML,
@BrandID int,
@uniquedownload bit = false,
@DownloadCount int = 0
as
BEGIN

	SET NOCOUNT ON

   declare @rgcolumns VARCHAR(MAX),
			@rgcolumnsdesc VARCHAR(MAX),
            @PubSubExtMapperColsAliased varchar(MAX),
            @brandScoreQuery VARCHAR(MAX),
            @LastOpenQuery varchar(MAX),
			@query varchar(MAX),
			@TotalRecords int,
			@nth int = 0, 
			@scolumns varchar(max),
			@custcolumns varchar(max)

  
    IF OBJECT_ID('tempdb..#tmpSubscriptionID') IS NOT NULL 
    BEGIN 
		DROP TABLE #tmpSubscriptionID;
    END 

	IF OBJECT_ID('tempdb..#TempStandardColumns') IS NOT NULL 
		BEGIN 
				DROP TABLE #TempStandardColumns;
		END 	
			
	IF OBJECT_ID('tempdb..#TempCustomcolumns') IS NOT NULL 
		BEGIN 
				DROP TABLE #TempCustomcolumns;
		END 

	IF OBJECT_ID('tempdb..#TempResponseGroups') IS NOT NULL 
		BEGIN 
				DROP TABLE #TempResponseGroups;
		END 
		
	IF OBJECT_ID('tempdb..#TempResponseGroupsDesc') IS NOT NULL 
		BEGIN 
				DROP TABLE #TempResponseGroupsDesc;
		END 	
            
    IF OBJECT_ID('tempdb..#TempPubSubExtMapperValues') IS NOT NULL 
    BEGIN 
		DROP TABLE #TempPubSubExtMapperValues;
    END 
    
        IF OBJECT_ID('tempdb..#tmpPubSubscriptionDetails') IS NOT NULL 
    BEGIN 
		DROP TABLE #tmpPubSubscriptionDetails;
    END 
    
        IF OBJECT_ID('tempdb..#tmpPubSubscriptionDetailsDesc') IS NOT NULL 
    BEGIN 
		DROP TABLE #tmpPubSubscriptionDetailsDesc;
    END 
            
    CREATE TABLE #tmpSubscriptionID (ID int identity(1,1), subsID int); 
	CREATE TABLE #TempStandardColumns (StandardColumn varchar(100), DisplayName varchar(100), CasetoConvert varchar(100)); 
	CREATE TABLE #TempCustomColumns (CustomColumn varchar(100), DisplayName varchar(100), CasetoConvert varchar(100));
	CREATE TABLE #TempResponseGroups (ResponseGroupID int); 
	CREATE TABLE #TempResponseGroupsDesc (ResponseGroupID int, CasetoConvert varchar(100)); 
    CREATE TABLE #TempPubSubExtMapperValues (PubSubExtMapperValues varchar(100), CasetoConvert varchar(100)); 
    CREATE TABLE #tmpPubSubscriptionDetails (subscriptionID INT, DisplayName VARCHAR(250), ResponseValue VARCHAR(255)); 
    CREATE TABLE #tmpPubSubscriptionDetailsDesc (subscriptionID INT, DisplayName VARCHAR(250), ResponseValue VARCHAR(255), ResponseDesc VARCHAR(255)); 
    
    CREATE CLUSTERED INDEX IDX_C_tmpSubscriptionID_subsID ON #tmpSubscriptionID(subsID)
	CREATE CLUSTERED INDEX IDX_C_TempStandardColumns_StandardColumn ON #TempStandardColumns(StandardColumn)
	CREATE CLUSTERED INDEX IDX_C_TempCustomColumns_CustomColumn ON #TempCustomColumns(CustomColumn)
	CREATE CLUSTERED INDEX IDX_C_TempResponseGroups_ResponseGroupID ON #TempResponseGroups(ResponseGroupID)
	CREATE CLUSTERED INDEX IDX_C_TempResponseGroupsDesc_ResponseGroupID ON #TempResponseGroupsDesc(ResponseGroupID)
    CREATE INDEX IDX_tmpPubSubscriptionDetails_subscriptionID ON #tmpPubSubscriptionDetails(subscriptionID)
    CREATE INDEX IDX_tmpPubSubscriptionDetailsDesc_subscriptionID ON #tmpPubSubscriptionDetailsDesc(subscriptionID)

	
	
	create table #tblqueryresults  (filterno int, subscriptionID int, primary key (filterno, subscriptionID))

	insert into #tblqueryresults  
	execute (@Queries)


	insert into #tmpSubscriptionID( subsID)
	select distinct subscriptionID from #tblqueryresults 
	

	drop table #tblqueryresults		
	

	select @TotalRecords = COUNT(ID) from #tmpSubscriptionID with (NOLOCK)
	
	
	
	if @DownloadCount > 0 and @DownloadCount <> @TotalRecords
	Begin
		set @nth = @TotalRecords/@DownloadCount

		if @nth <> 0 and @nth <> 1
		Begin
			delete from #tmpSubscriptionID
			where @DownloadCount <> -1 and ID % @nth <> 0
		end
	end
	
	/* Insert into TEMP Tables */   

	insert into #TempStandardColumns
	select	t.c.value('(Column/text())[1]', 'varchar(100)'),
			t.c.value('(DisplayName/text())[1]', 'varchar(100)'),
			t.c.value('(Case/text())[1]', 'varchar(100)')
	from @SubscriptionFields.nodes('//StandardField') as T(C);
		 
	insert into #TempCustomColumns
	select	t.c.value('(Column/text())[1]', 'varchar(100)'),
			t.c.value('(DisplayName/text())[1]', 'varchar(100)'),
			t.c.value('(Case/text())[1]', 'varchar(100)')
	from @CustomColumns.nodes('//CustomField') as T(C);

	insert into #TempResponseGroups
	select T.C.value('.', 'int')
	from @ResponseGroupID.nodes('/ResponseGroups/ResponseGroup/Column') as T(C);
	
	insert into #TempResponseGroupsDesc
	select	t.c.value('(Column/text())[1]', 'int'),
			t.c.value('(Case/text())[1]', 'varchar(100)')
	from @ResponseGroupID_Desc.nodes('//ResponseGroup') as T(C);

    insert into #TempPubSubExtMapperValues    
	select	t.c.value('(Column/text())[1]', 'varchar(100)'),
			t.c.value('(Case/text())[1]', 'varchar(100)')
	from @PubSubscriptionsExtMapperValues.nodes('/PubSubscriptionsExtMapperValues/PubSubscriptionsExtMapperValue') as T(C);

	/* ----------------------------- */

	SELECT	@scolumns = COALESCE(@scolumns + ',' + 
			(Case  when CasetoConvert = 'uppercase' then 'upper(' + cast(StandardColumn as  varchar(100)) + ')' 
				   when CasetoConvert = 'lowercase' then 'lower(' + cast(StandardColumn as  varchar(100)) + ')'
				   when CasetoConvert = 'propercase' then 'master.dbo.fn_title_case(' + cast(StandardColumn as  varchar(100)) + ')'
				   else '' + cast(StandardColumn as  varchar(100))+ '' end) + ' as [' + cast(DisplayName as varchar(100)) + ']',
			(Case when CasetoConvert = 'uppercase' then 'upper(' + cast(StandardColumn as  varchar(100)) + ')' 
				  when CasetoConvert = 'lowercase' then 'lower(' + cast(StandardColumn as  varchar(100)) + ')'
				  when CasetoConvert = 'propercase' then 'master.dbo.fn_title_case(' + cast(StandardColumn as  varchar(100)) + ')'
				  else '' + cast(StandardColumn as  varchar(100))+ '' end) +  ' as [' + cast(DisplayName as  varchar(100)) + ']')        
	FROM #TempStandardColumns

	--Added by G- To add new column Batch in Export file- Start 
	set @scolumns = REPLACE(@scolumns, 'ps.Batch', '(SELECT MAX(BatchNumber) FROM Batch b WITH(NOLOCK) JOIN History h WITH(NOLOCK) ON b.BatchID = h.BatchID WHERE h.PubSubscriptionID = ps.PubSubscriptionID)')
	--Added by G-- End 
	
	SELECT	@custcolumns = COALESCE(@custcolumns + ',' + 
			(Case  when CasetoConvert = 'uppercase' then 'upper(' + cast(CustomColumn as  varchar(100)) + ')' 
				   when CasetoConvert = 'lowercase' then 'lower(' + cast(CustomColumn as  varchar(100)) + ')'
				   when CasetoConvert = 'propercase' then 'master.dbo.fn_title_case(' + cast(CustomColumn as  varchar(100)) + ')'
				   else '' + cast(CustomColumn as  varchar(100))+ '' end) + ' as [' + cast(DisplayName as varchar(100)) + ']',
			(Case when CasetoConvert = 'uppercase' then 'upper(' + cast(CustomColumn as  varchar(100)) + ')' 
				  when CasetoConvert = 'lowercase' then 'lower(' + cast(CustomColumn as  varchar(100)) + ')'
				  when CasetoConvert = 'propercase' then 'master.dbo.fn_title_case(' + cast(CustomColumn as  varchar(100)) + ')'
				  else '' + cast(CustomColumn as  varchar(100))+ '' end) +  ' as [' + cast(DisplayName as  varchar(100)) + ']')        
	FROM #TempCustomColumns	

	SELECT @rgcolumns = COALESCE(@rgcolumns + ',[' + cast(DisplayName as varchar(50)) + ']', '[' + cast(DisplayName as  varchar(50))+ ']')        
	FROM responsegroups rg 
		join #TempResponseGroups tg on tg.ResponseGroupID = rg.ResponseGroupID 
	
	SELECT	@rgcolumnsdesc = COALESCE(@rgcolumnsdesc + ',[' + cast(DisplayName as varchar(50)) + '_Description' + ']', '[' + cast(DisplayName as  varchar(50)) + '_Description' + ']')        
	FROM responsegroups rg with (NOLOCK)
		join #TempResponseGroupsDesc tg on tg.ResponseGroupID = rg.ResponseGroupID	
      
    SELECT @PubSubExtMapperColsAliased = COALESCE(@PubSubExtMapperColsAliased +  ',' +
			(Case  when CasetoConvert = 'uppercase' then 'upper(' + cast(psem.StandardField as  varchar(100)) + ')' 
				   when CasetoConvert = 'lowercase' then 'lower(' + cast(psem.StandardField as  varchar(100)) + ')'
				   when CasetoConvert = 'propercase' then 'master.dbo.fn_title_case(' + cast(psem.StandardField as  varchar(100)) + ')'
				   else '' + cast(psem.StandardField as  varchar(100))+ '' end) + ' as [' + cast(psem.CustomField as varchar(100)) + ']',
			(Case when CasetoConvert = 'uppercase' then 'upper(' + cast(psem.StandardField as  varchar(100)) + ')' 
				  when CasetoConvert = 'lowercase' then 'lower(' + cast(psem.StandardField as  varchar(100)) + ')'
				  when CasetoConvert = 'propercase' then 'master.dbo.fn_title_case(' + cast(psem.StandardField as  varchar(100)) + ')'
				  else '' + cast(psem.StandardField as  varchar(100))+ '' end) +  ' as [' + cast(psem.CustomField as  varchar(100)) + ']')        
    FROM  #TempPubSubExtMapperValues t 
    join PubSubscriptionsExtensionMapper psem with (nolock) on t.PubSubExtMapperValues = psem.CustomField and PubID = @PubID
                  
    SET @PubSubExtMapperColsAliased = COALESCE(', ' + @PubSubExtMapperColsAliased, '')  

	if len(rtrim(ltrim(@custcolumns))) > 0
    Begin
        set @custcolumns = ', ' + rtrim(ltrim(@custcolumns)) + ' '
    End	   
    
    if ((len(@rgcolumns) = 0 or @rgcolumns is null) and (len(@rgcolumnsdesc) = 0 or @rgcolumnsdesc is null))
		Begin
			if(LEN(@PubSubExtMapperColsAliased) > 0)
				begin
					set @query = 'select ' + isnull(@scolumns,'') + isnull(@PubSubExtMapperColsAliased,'') + isnull(@custcolumns,'') + '
							from #tmpSubscriptionID t  
							join IssueArchiveProductSubscription ps with (nolock) on t.subsID = ps.subscriptionID
							left outer join EmailStatus es with (nolock) on es.EmailStatusID = ps.EmailStatusID    
							left outer join UAD_Lookup..Code c with (nolock) on c.CodeID = ps.PubQSourceID       
							left outer join UAD_Lookup..[TransactionCode] tc with (nolock) on tc.TransactionCodeID =  ps.PubTransactionID
							left outer join UAD_Lookup..[TransactionCodeType] tct with (nolock) on tc.TransactionCodeTypeID =  tct.TransactionCodeTypeID 
							left outer join UAD_Lookup..CategoryCode cc with (nolock) on cc.CategoryCodeID = ps.PubCategoryID                     
							left outer join UAD_Lookup..Code cp with (nolock) on cp.CodeID = ps.par3cID
							left outer join IssueArchivePubSubscriptionsExtension se with (nolock) on ps.IssueArchiveSubscriptionId = se.IssueArchiveSubscriptionId where ps.pubID = ' 
							+ convert(varchar(10),@PubID) 	+' and ps.IssueID = '+convert(varchar(20),@IssueID)	
				end
			else
				begin
					set @query = 'select ' + isnull(@scolumns,'') + isnull(@custcolumns,'') + '
						    from #tmpSubscriptionID t 
							join IssueArchiveProductSubscription ps with (nolock) on t.subsID  = ps.SubscriptionID
							left outer join EmailStatus es with (nolock)  on es.EmailStatusID = ps.EmailStatusID    
							left outer join UAD_Lookup..Code c with (nolock) on c.CodeID = ps.PubQSourceID       
							left outer join UAD_Lookup..[TransactionCode] tc with (nolock) on tc.TransactionCodeID =  ps.PubTransactionID
							left outer join UAD_Lookup..[TransactionCodeType] tct with (nolock) on tc.TransactionCodeTypeID =  tct.TransactionCodeTypeID                   
							left outer join UAD_Lookup..CategoryCode cc with (nolock) on cc.CategoryCodeID = ps.PubCategoryID
							left outer join UAD_Lookup..Code cp with (nolock) on cp.CodeID = ps.par3cID
							where ps.pubID = ' + convert(varchar(10),@PubID) +' and ps.IssueID = '+convert(varchar(20),@IssueID)	
				end
		End
    Else
		Begin
		
			INSERT INTO #tmpPubSubscriptionDetails
			SELECT ps.subscriptionID, rg.DisplayName,  c.Responsevalue 
			FROM IssueArchiveProductSubscription ps
			JOIN IssueArchiveProductSubscriptionDetail psd with (nolock) ON ps.IssueArchiveSubscriptionId = psd.IssueArchiveSubscriptionId and ps.IssueID = @IssueID
			JOIN CodeSheet c with (nolock) on psd.CodesheetID = c.CodeSheetID 
			JOIN ResponseGroups rg with (nolock) on rg.ResponseGroupID = c.ResponseGroupID
			JOIN #tmpSubscriptionID t on t.subsID = ps.SubscriptionID
			JOIN #TempResponseGroups tg on tg.ResponseGroupID = rg.ResponseGroupID 	
			where rg.PubID = @PubID and ps.IssueID = @IssueID	

			INSERT INTO #tmpPubSubscriptionDetailsDesc
			select ps.subscriptionID, rg.DisplayName + '_Description',  c.Responsevalue, 
						(Case  when tg.CasetoConvert = 'uppercase' then upper(cast(c.Responsedesc as  varchar(100))) 
						when tg.CasetoConvert = 'lowercase' then lower(cast(c.Responsedesc as  varchar(100))) 
						when tg.CasetoConvert = 'propercase' then master.dbo.fn_title_case(cast(c.Responsedesc as  varchar(100))) 
						else cast(c.Responsedesc as  varchar(100)) end)
			FROM IssueArchiveProductSubscription ps
			JOIN IssueArchiveProductSubscriptionDetail psd with (nolock) ON ps.IssueArchiveSubscriptionId = psd.IssueArchiveSubscriptionId and ps.IssueID = @IssueID
				join CodeSheet c with (nolock) on psd.CodesheetID = c.CodeSheetID 
				join ResponseGroups rg with (nolock) on rg.ResponseGroupID = c.ResponseGroupID
				join #tmpSubscriptionID t on t.subsID = ps.SubscriptionID
				join #TempResponseGroupsDesc tg on tg.ResponseGroupID = rg.ResponseGroupID 	
			where rg.PubID = @PubID and ps.IssueID = @IssueID 
					
			if(LEN(@PubSubExtMapperColsAliased) > 0)
				Begin
					set @query = 'select ' + isnull(@scolumns,'') + isnull(@PubSubExtMapperColsAliased,'')  + CASE WHEN len(rtrim(ltrim(@rgcolumns))) > 0 THEN  ',' + rtrim(ltrim(@rgcolumns)) ELSE '' END + CASE WHEN len(rtrim(ltrim(@rgcolumnsdesc))) > 0 THEN  ',' + rtrim(ltrim(@rgcolumnsdesc)) ELSE '' END + isnull(@custcolumns,'') + ' 
						from #tmpSubscriptionID t  
						join IssueArchiveProductSubscription ps with (nolock) on t.subsID = ps.subscriptionID and ps.IssueID = '+convert(varchar(20),@IssueID)+'
						left outer join EmailStatus es with (nolock) on es.EmailStatusID = ps.EmailStatusID    
						left outer join UAD_Lookup..Code c with (nolock) on c.CodeID = ps.PubQSourceID       
						left outer join UAD_Lookup..[TransactionCode] tc with (nolock) on tc.TransactionCodeID =  ps.PubTransactionID
						left outer join UAD_Lookup..[TransactionCodeType] tct with (nolock) on tc.TransactionCodeTypeID =  tct.TransactionCodeTypeID                   
						left outer join UAD_Lookup..CategoryCode cc with (nolock) on cc.CategoryCodeID = ps.PubCategoryID
						left outer join UAD_Lookup..Code cp with (nolock) on cp.CodeID = ps.par3cID
						left outer join IssueArchivePubSubscriptionsExtension se with (nolock) on ps.IssueArchiveSubscriptionId = se.IssueArchiveSubscriptionId
						'
						
					IF LEN(@rgcolumns) > 0	
						begin
							SET @query = @query + ' left outer join
							(
							SELECT * 
								FROM
								(
										SELECT 
												[subscriptionID], DisplayName, 
												STUFF((
													SELECT '', '' + CAST([Responsevalue] AS VARCHAR(MAX)) 
													FROM #tmpPubSubscriptionDetails 
													WHERE ([subscriptionID] = Results.[subscriptionID] and DisplayName= Results.DisplayName) 
													order by Responsevalue FOR XML PATH (''''))
												,1,2,'''') AS CombinedValues
											FROM #tmpPubSubscriptionDetails Results
											GROUP BY [subscriptionID], DisplayName
								) u
								PIVOT
								(
								MAX (CombinedValues)
								FOR DisplayName in (' + @rgcolumns + ')) as pvt
								) x on x.subscriptionID = t.subsID '
						end
						
					IF LEN(@rgcolumnsdesc) > 0	
						begin
							SET @query = @query + ' left outer join
							(
							SELECT * 
								FROM
								(
										SELECT 
												[subscriptionID], DisplayName, 
												STUFF((
													SELECT '', '' + CAST([ResponseDesc] AS VARCHAR(MAX)) 
													FROM #tmpPubSubscriptionDetailsDesc 
													WHERE ([subscriptionID] = Results.[subscriptionID] and DisplayName= Results.DisplayName) 
													order by Responsevalue FOR XML PATH (''''))
												,1,2,'''') AS CombinedValues
											FROM #tmpPubSubscriptionDetailsDesc Results
											GROUP BY [subscriptionID], DisplayName
								) u
								PIVOT
								(
								MAX (CombinedValues)
								FOR DisplayName in (' + @rgcolumnsdesc + ')) as pvt
								) y on y.subscriptionID = t.subsID '
						end	
						
					set @query = @query + ' where ps.pubID = ' + convert(varchar(10),@PubID) +' and ps.IssueID = '+convert(varchar(20),@IssueID)					
				end 
			else
				Begin
					set @query = 'select ' + isnull(@scolumns,'') + CASE WHEN len(rtrim(ltrim(@rgcolumns))) > 0 THEN  ',' + rtrim(ltrim(@rgcolumns)) ELSE '' END + CASE WHEN len(rtrim(ltrim(@rgcolumnsdesc))) > 0 THEN  ',' + rtrim(ltrim(@rgcolumnsdesc)) ELSE '' END + isnull(@custcolumns,'') + '    
					from #tmpSubscriptionID t  
					join IssueArchiveProductSubscription ps with (nolock) on t.subsID = ps.subscriptionID and ps.IssueID = '+convert(varchar(20),@IssueID)+'
					left outer join EmailStatus es with (nolock) on es.EmailStatusID = ps.EmailStatusID    
					left outer join UAD_Lookup..Code c with (nolock) on c.CodeID = ps.PubQSourceID       
					left outer join UAD_Lookup..[TransactionCode] tc with (nolock) on tc.TransactionCodeID =  ps.PubTransactionID
					left outer join UAD_Lookup..[TransactionCodeType] tct with (nolock) on tc.TransactionCodeTypeID =  tct.TransactionCodeTypeID                   
					left outer join UAD_Lookup..CategoryCode cc with (nolock) on cc.CategoryCodeID = ps.PubCategoryID
					left outer join UAD_Lookup..Code cp with (nolock) on cp.CodeID = ps.par3cID
					'

					IF LEN(@rgcolumns) > 0	
						begin
				
							SET @query = @query + ' left outer join
						(
						SELECT * 
						FROM
						(
								SELECT 
										[subscriptionID], DisplayName, 
										STUFF((
											SELECT '', '' + CAST([Responsevalue] AS VARCHAR(MAX)) 
											FROM #tmpPubSubscriptionDetails 
											WHERE ([subscriptionID] = Results.[subscriptionID] and DisplayName= Results.DisplayName) 
											order by Responsevalue FOR XML PATH (''''))
										,1,2,'''') AS CombinedValues
									FROM #tmpPubSubscriptionDetails Results
									GROUP BY [subscriptionID], DisplayName
						) u
						PIVOT
						(
						MAX (CombinedValues)
						FOR DisplayName in (' + @rgcolumns + ')) as pvt
						) x on x.subscriptionID = t.subsID '

					end

					IF LEN(@rgcolumnsdesc) > 0	
						begin
							SET @query = @query + ' left outer join
						(
						SELECT * 
						FROM
						(
								SELECT 
										[subscriptionID], DisplayName, 
										STUFF((
											SELECT '', '' + CAST([ResponseDesc] AS VARCHAR(MAX)) 
											FROM #tmpPubSubscriptionDetailsDesc
											WHERE ([subscriptionID] = Results.[subscriptionID] and DisplayName= Results.DisplayName) 
											order by Responsevalue FOR XML PATH (''''))
										,1,2,'''') AS CombinedValues
									FROM #tmpPubSubscriptionDetailsDesc Results
									GROUP BY [subscriptionID], DisplayName
						) u
						PIVOT
						(
						MAX (CombinedValues)
						FOR DisplayName in (' + @rgcolumnsdesc + ')) as pvt
						) y on y.subscriptionID = t.subsID '
					end	
					
					set @query = @query + ' where ps.pubID = ' + convert(varchar(10),@PubID) 	+' and ps.IssueID = '+convert(varchar(20),@IssueID)
				end
		End

	set rowcount @DownloadCount 
	exec (@query)		
	set rowcount 0
	
    DROP TABLE #tmpSubscriptionID;
    DROP TABLE #tmpPubSubscriptionDetails;
    DROP TABLE #tmpPubSubscriptionDetailsDesc;
    DROP TABLE #TempPubSubExtMapperValues;

End

GO


