CREATE PROCEDURE [dbo].[sp_GetProductDimensionSubscriberData_EV]
@Queries XML,
@SubscriptionFields XML,
@PubID varchar(4000)= '',
@ResponseGroupID XML,
@ResponseGroupID_Desc XML,
@PubSubscriptionsExtMapperValues XML,
@CustomColumns XML,
@BrandID int,
@uniquedownload bit = false
AS
BEGIN

	SET NOCOUNT ON

	declare @rgcolumns varchar(MAX),
			@rgcolumnsdesc VARCHAR(MAX),
			@PubSubExtMapperColsAliased varchar(MAX),
			@brandScoreQuery VARCHAR(MAX),
			@LastOpenQuery varchar(MAX),
			@query varchar(MAX), 
			@scolumns varchar(max),
			@custcolumns varchar(max)
			
	set @brandScoreQuery= ''
	set @LastOpenQuery = ''
      
	if @brandID > 0
	set @brandScoreQuery= ' left outer join brandscore bs with (nolock) on bs.subscriptionID = s.subscriptionID and bs.brandID = ' + CONVERT(varchar(10),@brandID)
      
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
            
	CREATE TABLE #tmpSubscriptionID (ID int identity(1,1), subsID int, email varchar(100));
	CREATE TABLE #TempStandardColumns (StandardColumn varchar(100), DisplayName varchar(100), CasetoConvert varchar(100)); 
	CREATE TABLE #TempCustomColumns (CustomColumn varchar(100), DisplayName varchar(100), CasetoConvert varchar(100));
	CREATE TABLE #TempResponseGroups (ResponseGroupID int); 
	CREATE TABLE #TempResponseGroupsDesc (ResponseGroupID int, CasetoConvert varchar(100)); 
	CREATE TABLE #TempSubs (ID int identity(1,1),subsID int);
	CREATE TABLE #TempPubSubExtMapperValues (PubSubExtMapperValues varchar(100), CasetoConvert varchar(100)); 
	CREATE TABLE #tmpPubSubscriptionDetails (email varchar(100), DisplayName VARCHAR(250), ResponseValue VARCHAR(255)); 
	CREATE TABLE #tmpPubSubscriptionDetailsDesc (email varchar(100), DisplayName VARCHAR(250), ResponseValue VARCHAR(255), ResponseDesc VARCHAR(255));

	CREATE CLUSTERED INDEX IDX_C_tmpSubscriptionID_subsID ON #tmpSubscriptionID(subsID)
	CREATE INDEX IDX_tmpPubSubscriptionDetails_Email ON #tmpPubSubscriptionDetails(email)
	
	declare @hDoc AS INT
	declare @filterno int, @qry varchar(max),
			@linenumber int, @SelectedFilterNo varchar(800), @SelectedFilterOperation varchar(20), @SuppressedFilterNo varchar(800), @SuppressedFilterOperation varchar(20) 

	create table #tblquery  (filterno int, Query varchar(max))
	create table #tblqueryresults  (filterno int, subscriptionID int, primary key (filterno, subscriptionID))

	EXEC sp_xml_preparedocument @hDoc OUTPUT, @queries

	insert into #tblquery
	SELECT filterno, Query
	FROM OPENXML(@hDoc, 'xml/Queries/Query')
	WITH 
	(
	filterno int '@filterno',
	Query [varchar](max) '.'
	)
	
	SELECT 
			@linenumber = linenumber, 
			@SelectedFilterNo = selectedfilterno, 
			@SelectedFilterOperation = selectedfilteroperation, 
			@SuppressedFilterNo = suppressedfilterno, 
			@SuppressedFilterOperation = suppressedfilteroperation
	FROM OPENXML(@hDoc, 'xml/Results/Result')
	WITH 
	(
		linenumber int							'@linenumber',
		selectedfilterno varchar(800)			'@selectedfilterno',
		selectedfilteroperation varchar(20)		'@selectedfilteroperation',
		suppressedfilterno varchar(800)			'@suppressedfilterno',
		suppressedfilteroperation varchar(20)	'@suppressedfilteroperation'
	)

	EXEC sp_xml_removedocument @hDoc
	
	DECLARE c_queries CURSOR 
	FOR select filterno from #tblquery
		
	OPEN c_queries  
	FETCH NEXT FROM c_queries INTO @filterno

	WHILE @@FETCH_STATUS = 0  
	BEGIN  
		select @qry =  query from #tblquery with (NOLOCK) where filterno = @filterno
		
		insert into #tblqueryresults  
		execute (@qry)
		
		FETCH NEXT FROM c_queries INTO @filterno
	END
	
	CLOSE c_queries  
	DEALLOCATE c_queries 

	select @qry = 'select distinct subscriptionID from ((select subscriptionID from #tblqueryresults t where filterno = ' + REPLACE(@SelectedFilterNo, ',', '  ' + @SelectedFilterOperation + ' select subscriptionID from #tblqueryresults t where filterno = ') + ') ' + 
	case when len(@suppressedfilterno) > 0 then 
		' except ' 
		+ ' (select subscriptionID from #tblqueryresults t where filterno = ' + REPLACE(@SuppressedfilterNo , ',', '  ' + @SuppressedFilterOperation + ' select subscriptionID from #tblqueryresults t where filterno = ') + ') '
	else
	'' end
	+ ') x'

	insert into #TempSubs 
	execute (@qry)
	
	drop table #tblquery
	drop table #tblqueryresults		

	/* Insert into TEMP Tables */
	
	if (@uniquedownload = 0)
	Begin
		insert into #tmpSubscriptionID
		select 
			ps.subscriptionID, ps.EMAIL
		from
			#TempSubs t join
			PubSubscriptions ps with (nolock) on ps.SubscriptionID = t.subsID
		where 
			PubID = @PubID  
	End
	Else
	Begin
		insert into #tmpSubscriptionID
		select SubscriptionID, EMAIL
		from
		(
			select  
				ps.SubscriptionID, ps.EMAIL,  row_number() over(partition by EMAIL order by QualificationDate desc) as rn
			from 
				#TempSubs t join 
				PubSubscriptions ps with (nolock) on ps.SubscriptionID = t.subsID
			where 
				PubID = @PubID and ISNULL(email, '') <> ''	
		) O1
		where rn = 1
	End			            
    
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
            
			if @brandID = 0 
				Begin
					set @LastOpenQuery = ' left outer join 
								(
									select t.SubscriptionID, t.ActivityDate as lastOpenedDate, p.pubcode as LASTOPENEDPUBCODE
									from
									(
											select  
														soa.subscriptionID, 
														soa.activitydate, 
														psa.pubID,
														ROW_NUMBER() over (partition by psa.subscriptionID order by activitydate desc) as RN
											from 
														#tmpSubscriptionID ts join
														SubscriberOpenActivity soa with (NOLOCK) on soa.subscriptionID = ts.subsID
														join PubSubscriptions psa with (NOLOCK) on soa.pubsubscriptionId = psa.pubsubscriptionId '
                  
					if len(ltrim(rtrim(@PubID))) > 0
						set @LastOpenQuery = @LastOpenQuery + ' where psa.pubID = ' + CONVERT(varchar(10),@PubID) 
                  
					set @LastOpenQuery = @LastOpenQuery + ' ) t join Pubs p  with (NOLOCK) on t.PubID = p.PubID
									where t.RN = 1
								) sa on sa.subscriptionID  = s.subscriptionID '
				end
			else
				Begin
						set @LastOpenQuery = ' left outer join 
									(
										select t.SubscriptionID, t.ActivityDate as lastOpenedDate, p.pubcode as LASTOPENEDPUBCODE
										from
										(
												select  
															soa.subscriptionID, 
															soa.activitydate, 
															psa.pubID,
															ROW_NUMBER() over (partition by psa.subscriptionID order by activitydate desc) as RN
												from 
															#tmpSubscriptionID ts join
															SubscriberOpenActivity soa with (NOLOCK) on soa.subscriptionID = ts.subsID
															join PubSubscriptions psa with (NOLOCK) on soa.pubsubscriptionId = psa.pubsubscriptionId '
            
						if len(ltrim(rtrim(@PubID))) > 0
							set @LastOpenQuery = @LastOpenQuery + ' where psa.pubID = ' + CONVERT(varchar(10),@PubID)   
						else
							set @LastOpenQuery = @LastOpenQuery + ' where psa.pubID in (select pubID from BrandDetails with (NOLOCK) where BrandID = ' + CONVERT(varchar(10),@brandID) + ') '      
                        
						set @LastOpenQuery = @LastOpenQuery + ' ) t join Pubs p  with (NOLOCK) on t.PubID = p.PubID
										where t.RN = 1
									) sa on sa.subscriptionID  = s.subscriptionID ' 
				End
			/* ----------------------------- */

		End
      
	if ((len(@rgcolumns) = 0 or @rgcolumns is null) and (len(@rgcolumnsdesc) = 0 or @rgcolumnsdesc is null))
		Begin
      
			if(LEN(@PubSubExtMapperColsAliased) > 0)
				begin
					set @query = 'select ' + isnull(@scolumns,'') + isnull(@PubSubExtMapperColsAliased,'') + isnull(@custcolumns,'') + '
							from subscriptions s with (nolock) 
							join #tmpSubscriptionID t on s.subscriptionID = t.subsID 
							join pubSubscriptions ps with (nolock) on s.subscriptionID = ps.subscriptionID
							left outer join EmailStatus es with (nolock) on es.EmailStatusID = ps.EmailStatusID    
							left outer join UAD_Lookup..Code c with (nolock) on c.CodeID = ps.PubQSourceID       
							left outer join UAD_Lookup..[TransactionCode] tc with (nolock) on tc.TransactionCodeID =  ps.PubTransactionID
							left outer join UAD_Lookup..[TransactionCodeType] tct with (nolock) on tc.TransactionCodeTypeID =  tct.TransactionCodeTypeID                   
							left outer join UAD_Lookup..CategoryCode cc with (nolock) on cc.CategoryCodeID = ps.PubCategoryID                     
							left outer join UAD_Lookup..Code cp with (nolock) on cp.CodeID = ps.par3cID
							left outer join PubSubscriptionsExtension se with (nolock) on ps.pubsubscriptionID = se.pubsubscriptionid' + @brandScoreQuery + @LastOpenQuery + 
							' where ps.pubID = ' + convert(varchar(10),@PubID)            
				end
			else
				begin
					set @query = 'select ' + isnull(@scolumns,'') + isnull(@custcolumns,'') + '
							from subscriptions s with (nolock) 
							join #tmpSubscriptionID t on s.subscriptionID = t.subsID 
							join pubSubscriptions ps with (nolock) on s.subscriptionID = ps.subscriptionID
							left outer join EmailStatus es with (nolock) on es.EmailStatusID = ps.EmailStatusID    
							left outer join UAD_Lookup..Code c with (nolock) on c.CodeID = ps.PubQSourceID       
							left outer join UAD_Lookup..[TransactionCode] tc with (nolock) on tc.TransactionCodeID =  ps.PubTransactionID
							left outer join UAD_Lookup..[TransactionCodeType] tct with (nolock) on tc.TransactionCodeTypeID =  tct.TransactionCodeTypeID 
							left outer join UAD_Lookup..CategoryCode cc with (nolock) on cc.CategoryCodeID = ps.PubCategoryID                     
							left outer join UAD_Lookup..Code cp with (nolock) on cp.CodeID = ps.par3cID                    
							' + @brandScoreQuery + @LastOpenQuery +  
							' where ps.pubID = ' + convert(varchar(10),@PubID)
				end
		End
	Else
		Begin
			INSERT INTO #tmpPubSubscriptionDetails
			select distinct t.email, rg.DisplayName,  c.Responsevalue 
			from #tmpSubscriptionID t 
				join PubSubscriptionDetail psd with (nolock) on psd.SubscriptionID = t.subsID 
				join CodeSheet c with (nolock) on psd.CodesheetID = c.CodeSheetID 
				join ResponseGroups rg with (nolock) on rg.ResponseGroupID = c.ResponseGroupID
				join #TempResponseGroups tg on tg.ResponseGroupID = rg.ResponseGroupID 	
			where rg.PubID = @PubID
			
			INSERT INTO #tmpPubSubscriptionDetailsDesc
			select distinct t.email, rg.DisplayName + '_Description',  c.Responsevalue,  
						(Case  when tg.CasetoConvert = 'uppercase' then upper(cast(c.Responsedesc as  varchar(100))) 
						when tg.CasetoConvert = 'lowercase' then lower(cast(c.Responsedesc as  varchar(100))) 
						when tg.CasetoConvert = 'propercase' then master.dbo.fn_title_case(cast(c.Responsedesc as  varchar(100))) 
						else cast(c.Responsedesc as  varchar(100)) end)			 
			from #tmpSubscriptionID t 
				join PubSubscriptionDetail psd with (nolock) on psd.SubscriptionID = t.subsID 
				join CodeSheet c with (nolock) on psd.CodesheetID = c.CodeSheetID 
				join ResponseGroups rg with (nolock) on rg.ResponseGroupID = c.ResponseGroupID
				join #TempResponseGroupsDesc tg on tg.ResponseGroupID = rg.ResponseGroupID 	
			where rg.PubID = @PubID			

			--select s.subscriptionID, ' + @SubscriptionFields + ', demos.* from subscriptions s 
			if(LEN(@PubSubExtMapperColsAliased) > 0)
				Begin
					set @query = 'select ' + isnull(@scolumns,'') + isnull(@PubSubExtMapperColsAliased,'') + CASE WHEN len(rtrim(ltrim(@rgcolumns))) > 0 THEN  ',' + rtrim(ltrim(@rgcolumns)) ELSE '' END + CASE WHEN len(rtrim(ltrim(@rgcolumnsdesc))) > 0 THEN  ',' + rtrim(ltrim(@rgcolumnsdesc)) ELSE '' END + isnull(@custcolumns,'') + ' 
						from subscriptions s with (nolock)
						join #tmpSubscriptionID t on s.subscriptionID = t.subsID 
						join pubSubscriptions ps with (nolock) on s.subscriptionID = ps.subscriptionID
						left outer join EmailStatus es with (nolock) on es.EmailStatusID = ps.EmailStatusID    
						left outer join UAD_Lookup..Code c with (nolock) on c.CodeID = ps.PubQSourceID       
						left outer join UAD_Lookup..[TransactionCode] tc with (nolock) on tc.TransactionCodeID =  ps.PubTransactionID
						left outer join UAD_Lookup..[TransactionCodeType] tct with (nolock) on tc.TransactionCodeTypeID =  tct.TransactionCodeTypeID 
						left outer join UAD_Lookup..CategoryCode cc with (nolock) on cc.CategoryCodeID = ps.PubCategoryID                        
						left outer join UAD_Lookup..Code cp with (nolock) on cp.CodeID = ps.par3cID
						left outer join PubSubscriptionsExtension se with (nolock) on ps.pubsubscriptionID = se.pubsubscriptionid
						' + @brandScoreQuery + @LastOpenQuery 
						
					IF LEN(@rgcolumns) > 0	
						begin
							SET @query = @query + ' left outer join
							(
							SELECT * 
							FROM
							(
								SELECT 
										[email], DisplayName, 
										STUFF((
											SELECT '', '' + CAST([Responsevalue] AS VARCHAR(MAX)) 
											FROM #tmpPubSubscriptionDetails 
											WHERE ([email] = Results.[email] and DisplayName= Results.DisplayName) 
											order by Responsevalue FOR XML PATH (''''))
										,1,2,'''') AS CombinedValues
										FROM #tmpPubSubscriptionDetails Results
										GROUP BY [email], DisplayName
							) u
							PIVOT
							(
							MAX (CombinedValues)
							FOR DisplayName in (' + @rgcolumns + ')) as pvt
							) x on x.email = s.email '
						end
						
					IF LEN(@rgcolumnsdesc) > 0	
						begin
							SET @query = @query + ' left outer join
							(
							SELECT * 
							FROM
							(
								SELECT 
										[email], DisplayName, 
										STUFF((
											SELECT '', '' + CAST([ResponseDesc] AS VARCHAR(MAX)) 
											FROM #tmpPubSubscriptionDetailsDesc 
											WHERE ([email] = Results.[email] and DisplayName= Results.DisplayName) 
											order by Responsevalue FOR XML PATH (''''))
										,1,2,'''') AS CombinedValues
										FROM #tmpPubSubscriptionDetailsDesc Results
										GROUP BY [email], DisplayName
							) u
							PIVOT
							(
							MAX (CombinedValues)
							FOR DisplayName in (' + @rgcolumnsdesc + ')) as pvt
							) y on y.email = s.email '
						end	

					set @query = @query + ' where ps.pubID = ' + convert(varchar(10),@PubID) 	
				end 
			else
				Begin
					set @query = 'select ' + isnull(@scolumns,'') + CASE WHEN len(rtrim(ltrim(@rgcolumns))) > 0 THEN  ',' + rtrim(ltrim(@rgcolumns)) ELSE '' END + CASE WHEN len(rtrim(ltrim(@rgcolumnsdesc))) > 0 THEN  ',' + rtrim(ltrim(@rgcolumnsdesc)) ELSE '' END + isnull(@custcolumns,'') + ' 
						from subscriptions s with (nolock) 
						join #tmpSubscriptionID t on s.subscriptionID = t.subsID 
						join pubSubscriptions ps with (nolock) on s.subscriptionID = ps.subscriptionID
						left outer join EmailStatus es with (nolock) on es.EmailStatusID = ps.EmailStatusID    
						left outer join UAD_Lookup..Code c with (nolock) on c.CodeID = ps.PubQSourceID       
						left outer join UAD_Lookup..[TransactionCode] tc with (nolock) on tc.TransactionCodeID =  ps.PubTransactionID
						left outer join UAD_Lookup..[TransactionCodeType] tct with (nolock) on tc.TransactionCodeTypeID =  tct.TransactionCodeTypeID 
						left outer join UAD_Lookup..CategoryCode cc with (nolock) on cc.CategoryCodeID = ps.PubCategoryID                        
						left outer join UAD_Lookup..Code cp with (nolock) on cp.CodeID = ps.par3cID
						' + @brandScoreQuery + @LastOpenQuery 
							
					IF LEN(@rgcolumns) > 0	
						begin
							SET @query = @query + ' left outer join
							(
						SELECT * 
						FROM
						(
							SELECT 
									[email], DisplayName, 
									STUFF((
										SELECT '', '' + CAST([Responsevalue] AS VARCHAR(MAX)) 
										FROM #tmpPubSubscriptionDetails 
										WHERE ([email] = Results.[email] and DisplayName= Results.DisplayName) 
										order by Responsevalue FOR XML PATH (''''))
									,1,2,'''') AS CombinedValues
									FROM #tmpPubSubscriptionDetails Results
									GROUP BY [email], DisplayName
						) u
						PIVOT
						(
						MAX (CombinedValues)
						FOR DisplayName in (' + @rgcolumns + ')) as pvt
						) x on x.email = s.email '
					end
					
					IF LEN(@rgcolumnsdesc) > 0	
						begin
							SET @query = @query + ' left outer join
							(
							SELECT * 
							FROM
							(
								SELECT 
										[email], DisplayName, 
										STUFF((
											SELECT '', '' + CAST([ResponseDesc] AS VARCHAR(MAX)) 
											FROM #tmpPubSubscriptionDetailsDesc 
											WHERE ([email] = Results.[email] and DisplayName= Results.DisplayName) 
											order by Responsevalue FOR XML PATH (''''))
										,1,2,'''') AS CombinedValues
										FROM #tmpPubSubscriptionDetailsDesc Results
										GROUP BY [email], DisplayName
							) u
							PIVOT
							(
							MAX (CombinedValues)
							FOR DisplayName in (' + @rgcolumnsdesc + ')) as pvt
							) y on y.email = s.email '
						end	
							
					set @query = @query + ' where ps.pubID = ' + convert(varchar(10),@PubID) 					    
				end
	
		End

	exec (@query)

	DROP TABLE #tmpSubscriptionID;
	DROP TABLE #tmpPubSubscriptionDetails;
	DROP TABLE #tmpPubSubscriptionDetailsDesc;
	DROP TABLE #TempPubSubExtMapperValues;
End