CREATE PROCEDURE [dbo].[sp_GetSubscriberData_RecentConsensus_EV]
@Queries XML,
@StandardColumns XML,
@MasterGroupValues XML,
@MasterGroupValues_Desc XML,
@SubscriptionsExtMapperValues XML,
@CustomColumns XML,
@brandID int = 0,
@PubIDs varchar(2000) = '',
@uniquedownload bit = false
AS
BEGIN

	SET NOCOUNT ON

	declare	@query varchar(MAX),
		@mgcolumns VARCHAR(MAX),
		@mgcolumnsdesc VARCHAR(MAX),
		@SubExtMapperColsAliased varchar(MAX),
		@brandScoreQuery VARCHAR(MAX),
		@LastOpenQuery varchar(MAX), 
		@scolumns varchar(max),
		@custcolumns varchar(max)

	set @brandScoreQuery= ''
	set @LastOpenQuery = ''
	
	if @brandID > 0
		set @brandScoreQuery= ' left outer join brandscore bs with (nolock) on bs.subscriptionID = s.subscriptionID and bs.brandID = ' + CONVERT(varchar(10),@brandID)

	/* Create TEMP Table & Index */

	IF OBJECT_ID('tempdb..#TempSubscription') IS NOT NULL 
		BEGIN 
				DROP TABLE #TempSubscription;
		END 

	IF OBJECT_ID('tempdb..#TempStandardColumns') IS NOT NULL 
		BEGIN 
				DROP TABLE #TempStandardColumns;
		END 	
			
	IF OBJECT_ID('tempdb..#TempCustomcolumns') IS NOT NULL 
		BEGIN 
				DROP TABLE #TempCustomcolumns;
		END 
		
	IF OBJECT_ID('tempdb..#TempMasterGroups') IS NOT NULL 
		BEGIN 
				DROP TABLE #TempMasterGroups;
		END 
		
	IF OBJECT_ID('tempdb..#TempMasterGroupsDesc') IS NOT NULL 
		BEGIN 
				DROP TABLE #TempMasterGroupsDesc;
		END 		

	IF OBJECT_ID('tempdb..#TempSubExtMapperValues') IS NOT NULL 
		BEGIN 
				DROP TABLE #TempSubExtMapperValues;
		END 

	IF OBJECT_ID('tempdb..#TempSubscriptionDetails') IS NOT NULL 
		BEGIN 
				DROP TABLE #TempSubscriptionDetails;
		END 
	                            
	CREATE TABLE #TempSubscription (ID int identity(1,1), subsID int, email varchar(100));
	CREATE TABLE #TempSubs (ID int identity(1,1),subsID int);
	CREATE TABLE #TempStandardColumns (StandardColumn varchar(100), DisplayName varchar(100), CasetoConvert varchar(100)); 
	CREATE TABLE #TempCustomColumns (CustomColumn varchar(100), DisplayName varchar(100), CasetoConvert varchar(100));
	CREATE TABLE #TempMasterGroups (MasterGroupColumnReference varchar(100)); 
	CREATE TABLE #TempMasterGroupsDesc (MasterGroupColumnReference varchar(100), CasetoConvert varchar(100)); 
	CREATE TABLE #TempSubExtMapperValues (SubExtMapperValues varchar(100), CasetoConvert varchar(100));  
	CREATE TABLE #TempSubscriptionDetails (SubscriptionID int, DisplayName varchar(250), MasterValue varchar(100));
	CREATE TABLE #TempSubscriptionDetailsDesc (SubscriptionID int, DisplayName varchar(250), MasterValue varchar(100), MasterDesc varchar(255));    

	CREATE CLUSTERED INDEX IDX_C_TempSubscription_subsID ON #TempSubscription(subsID)
	CREATE CLUSTERED INDEX IDX_C_TempStandardColumns_StandardColumn ON #TempStandardColumns(StandardColumn)
	CREATE CLUSTERED INDEX IDX_C_TempCustomColumns_CustomColumn ON #TempCustomColumns(CustomColumn)
	CREATE CLUSTERED INDEX IDX_C_TempMasterGroups_MasterGroupColumnReference ON #TempMasterGroups(MasterGroupColumnReference)
	CREATE CLUSTERED INDEX IDX_C_TempMasterGroupsDesc_MasterGroupColumnReference ON #TempMasterGroupsDesc(MasterGroupColumnReference)
	CREATE INDEX IDX_TempSubscriptionDetails_SubscriptionID ON #TempSubscriptionDetails(SubscriptionID)
	CREATE INDEX IDX_TempSubscriptionDetails_SubscriptionID_DisplayName ON #TempSubscriptionDetails([subscriptionID], DisplayName)
	
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
	
	/* ----------------------------- */

	/* Insert into TEMP Tables */
	
	if (@uniquedownload = 0)
	Begin
		insert into #TempSubscription
		select 
			s.SubscriptionID, s.EMAIL
		from
			#TempSubs t join
			Subscriptions s with (nolock) on s.SubscriptionID = t.subsID
	End
	Else
	Begin
		insert into #TempSubscription
		select SubscriptionID, EMAIL
		from
		(
			select  
				s.SubscriptionID, s.EMAIL,  row_number() over(partition by EMAIL order by Qdate desc) as rn
			from 
				#TempSubs t join 
				Subscriptions s with (nolock) on s.SubscriptionID = t.subsID
			where 
				ISNULL(email, '') <> ''	
		) O1
		where rn = 1
	End	

	insert into #TempStandardColumns
	select	t.c.value('(Column/text())[1]', 'varchar(100)'),
			t.c.value('(DisplayName/text())[1]', 'varchar(100)'),
			t.c.value('(Case/text())[1]', 'varchar(100)')
	from @StandardColumns.nodes('//StandardField') as T(C);
		 
	insert into #TempCustomColumns
	select	t.c.value('(Column/text())[1]', 'varchar(100)'),
			t.c.value('(DisplayName/text())[1]', 'varchar(100)'),
			t.c.value('(Case/text())[1]', 'varchar(100)')
	from @CustomColumns.nodes('//CustomField') as T(C);
		
	insert into #TempMasterGroups
	select T.C.value('.', 'varchar(100)')
	from @MasterGroupValues.nodes('/MasterGroups/MasterGroup/Column') as T(C);
	
	insert into #TempMasterGroupsDesc
	select	t.c.value('(Column/text())[1]', 'varchar(100)'),
			t.c.value('(Case/text())[1]', 'varchar(100)')
	from @MasterGroupValues_Desc.nodes('//MasterGroup') as T(C);

	insert into #TempSubExtMapperValues
	select	t.c.value('(Column/text())[1]', 'varchar(100)'),
			t.c.value('(Case/text())[1]', 'varchar(100)')
	from @SubscriptionsExtMapperValues.nodes('//SubscriptionsExtMapperValue') as T(C);

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

	SELECT @mgcolumns = COALESCE(@mgcolumns + ',[' + cast(DisplayName as varchar(50)) + ']', '[' + cast(DisplayName as  varchar(50))+ ']')        
	FROM mastergroups mg 
		join #TempMasterGroups tg on tg.MasterGroupColumnReference = mg.ColumnReference 
	
	SELECT	@mgcolumnsdesc = COALESCE(@mgcolumnsdesc + ',[' + cast(DisplayName as varchar(50)) + '_Description' + ']', '[' + cast(DisplayName as  varchar(50)) + '_Description' + ']')        
	FROM mastergroups mg with (NOLOCK)
		join #TempMasterGroupsDesc tg on tg.MasterGroupColumnReference = mg.ColumnReference	

	SELECT	@SubExtMapperColsAliased = COALESCE(@SubExtMapperColsAliased + ',' + 
			(Case  when CasetoConvert = 'uppercase' then 'upper(' + cast(sem.StandardField as  varchar(100)) + ')' 
				   when CasetoConvert = 'lowercase' then 'lower(' + cast(sem.StandardField as  varchar(100)) + ')'
				   when CasetoConvert = 'propercase' then 'master.dbo.fn_title_case(' + cast(sem.StandardField as  varchar(100)) + ')'
				   else '' + cast(sem.StandardField as  varchar(100))+ '' end) + ' as [' + cast(sem.CustomField as varchar(100)) + ']',
			(Case when CasetoConvert = 'uppercase' then 'upper(' + cast(sem.StandardField as  varchar(100)) + ')' 
				  when CasetoConvert = 'lowercase' then 'lower(' + cast(sem.StandardField as  varchar(100)) + ')'
				  when CasetoConvert = 'propercase' then 'master.dbo.fn_title_case(' + cast(sem.StandardField as  varchar(100)) + ')'
				  else '' + cast(sem.StandardField as  varchar(100))+ '' end) +  ' as [' + cast(sem.CustomField as  varchar(100)) + ']')        
	FROM #TempSubExtMapperValues t 
		join SubscriptionsExtensionMapper sem with (NOLOCK) on t.SubExtMapperValues = sem.CustomField
	
	SET @SubExtMapperColsAliased = COALESCE(',' + @SubExtMapperColsAliased, '')

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
										#TempSubscription ts join
										SubscriberOpenActivity soa with (NOLOCK) on soa.subscriptionID = ts.subsID
										join PubSubscriptions psa with (NOLOCK) on soa.pubsubscriptionId = psa.pubsubscriptionId '
			
				if len(ltrim(rtrim(@PubIDs))) > 0
					set @LastOpenQuery = @LastOpenQuery + ' where psa.pubID in (' + @PubIDs + ') ' 
			
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
										#TempSubscription ts join
										SubscriberOpenActivity soa with (NOLOCK) on soa.subscriptionID = ts.subsID
										join PubSubscriptions psa with (NOLOCK) on soa.pubsubscriptionId = psa.pubsubscriptionId '
		
				if len(ltrim(rtrim(@PubIDs))) > 0
					set @LastOpenQuery = @LastOpenQuery + ' where psa.pubID in (' + @PubIDs + ') ' 	
				else
					set @LastOpenQuery = @LastOpenQuery + ' where psa.pubID in (select pubID from BrandDetails with (NOLOCK) where BrandID = ' + CONVERT(varchar(10),@brandID) + ') ' 	
				
				set @LastOpenQuery = @LastOpenQuery + ' ) t join Pubs p  with (NOLOCK) on t.PubID = p.PubID
							where t.RN = 1
						) sa on sa.subscriptionID  = s.subscriptionID '	
			End
		/* ----------------------------- */

	End

	IF(LEN(@mgcolumns) > 0 or LEN(@mgcolumnsdesc) > 0)
		BEGIN
		
			if @brandID = 0
				Begin
		
					Insert into #TempSubscriptionDetails
					select distinct vrc.subscriptionID, mg.DisplayName,  mc.MasterValue
					from #TempSubscription t 
						join vw_RecentConsensus vrc on vrc.SubscriptionID  = t.subsID 
						join mastercodesheet mc with (NOLOCK) on mc.masterID = vrc.masterID 
						join mastergroups mg with (NOLOCK) on mg.mastergroupID = mc.mastergroupID 
						join #TempMasterGroups tg on tg.MasterGroupColumnReference = mg.ColumnReference 	
						
					Insert into #TempSubscriptionDetailsDesc
					select distinct vrc.subscriptionID, mg.DisplayName + '_Description',  mc.MasterValue,
							(Case  when tg.CasetoConvert = 'uppercase' then upper(cast(mc.MasterDesc as  varchar(100))) 
							when tg.CasetoConvert = 'lowercase' then lower(cast(mc.MasterDesc as  varchar(100))) 
							when tg.CasetoConvert = 'propercase' then master.dbo.fn_title_case(cast(mc.MasterDesc as  varchar(100))) 
							else cast(mc.MasterDesc as  varchar(100)) end)
					from #TempSubscription t 
						join vw_RecentConsensus vrc on vrc.SubscriptionID  = t.subsID 
						join mastercodesheet mc with (NOLOCK) on mc.masterID = vrc.masterID 
						join mastergroups mg with (NOLOCK) on mg.mastergroupID = mc.mastergroupID 
						join #TempMasterGroupsDesc tg on tg.MasterGroupColumnReference = mg.ColumnReference 												
				end
			else
				Begin
	
					Insert into #TempSubscriptionDetails
					select distinct subscriptionID, mg.DisplayName,  mc.MasterValue
					from #TempSubscription t 
						join vw_RecentBrandConsensus vrbc on vrbc.SubscriptionID  = t.subsID 
						join mastercodesheet mc with (NOLOCK) on mc.masterID = vrbc.masterID 
						join mastergroups mg with (NOLOCK) on mg.mastergroupID = mc.mastergroupID 
						join #TempMasterGroups tg on tg.MasterGroupColumnReference = mg.ColumnReference
					where (vrbc.BrandID = @brandID)
					
					Insert into #TempSubscriptionDetailsDesc
					select distinct subscriptionID, mg.DisplayName + '_Description',  mc.MasterValue,
							(Case  when tg.CasetoConvert = 'uppercase' then upper(cast(mc.MasterDesc as  varchar(100))) 
							when tg.CasetoConvert = 'lowercase' then lower(cast(mc.MasterDesc as  varchar(100))) 
							when tg.CasetoConvert = 'propercase' then master.dbo.fn_title_case(cast(mc.MasterDesc as  varchar(100))) 
							else cast(mc.MasterDesc as  varchar(100)) end)
					from #TempSubscription t 
						join vw_RecentBrandConsensus vrbc on vrbc.SubscriptionID  = t.subsID 
						join mastercodesheet mc with (NOLOCK) on mc.masterID = vrbc.masterID 
						join mastergroups mg with (NOLOCK) on mg.mastergroupID = mc.mastergroupID 
						join #TempMasterGroupsDesc tg on tg.MasterGroupColumnReference = mg.ColumnReference
					where (vrbc.BrandID = @brandID)					
				end			

			if(LEN(@SubExtMapperColsAliased) > 0)
				begin
					SET @query = 'select ' + isnull(@scolumns,'') + isnull(@SubExtMapperColsAliased,'') + CASE WHEN len(rtrim(ltrim(@mgcolumns))) > 0 THEN  ',' + rtrim(ltrim(@mgcolumns)) ELSE '' END + CASE WHEN len(rtrim(ltrim(@mgcolumnsdesc))) > 0 THEN  ',' + rtrim(ltrim(@mgcolumnsdesc)) ELSE '' END + isnull(@custcolumns,'') + '  
					from #TempSubscription subs  join subscriptions s with (NOLOCK)     
						on s.SubscriptionID=subs.subsID '
						
					IF LEN(@mgcolumns) > 0
						SET @query = @query + '	left outer join      
						(
							SELECT * 
							 FROM
							 (
								SELECT 
									  [subscriptionID], DisplayName, 
									  STUFF((
										SELECT '', '' + CAST([MasterValue] AS VARCHAR(MAX)) 
										FROM #TempSubscriptionDetails
										WHERE ([subscriptionID] = Results.[subscriptionID] and DisplayName= Results.DisplayName) 
										order by MasterValue FOR XML PATH (''''))
									  ,1,2,'''') AS CombinedValues
									FROM #TempSubscriptionDetails Results
									GROUP BY [subscriptionID], DisplayName
							 ) u
							 PIVOT
							 (
							 MAX (CombinedValues)
							 FOR DisplayName in (' + @mgcolumns + ')
							 ) as pvt
						) y on y.subscriptionID = s.subscriptionID '
					
					IF LEN(@mgcolumnsdesc) > 0
						SET @query = @query + '	 left outer join    
						(
							SELECT * 
							 FROM
							 (
								SELECT 
									  [subscriptionID], DisplayName, 
									  STUFF((
										SELECT '', '' + CAST([MasterDesc] AS VARCHAR(MAX)) 
										FROM #TempSubscriptionDetailsDesc
										WHERE ([subscriptionID] = Results.[subscriptionID] and DisplayName= Results.DisplayName) 
										order by MasterValue FOR XML PATH (''''))
									  ,1,2,'''') AS CombinedValues
									FROM #TempSubscriptionDetailsDesc Results
									GROUP BY [subscriptionID], DisplayName
							 ) u
							 PIVOT
							 (
							 MAX (CombinedValues)
							 FOR DisplayName in (' + @mgcolumnsdesc + ')
							 ) as pvt
						) z on z.subscriptionID = s.subscriptionID '
					
					SET @query = @query + '								
						left outer join UAD_Lookup..Code c with (nolock)
							on c.CodeID = s.QSourceID       
						left outer join UAD_Lookup..[TransactionCode] tc with (nolock)      
							on tc.TransactionCodeID =  s.TransactionID
						left outer join UAD_Lookup..[TransactionCodeType] tct with (nolock)      
							on tc.TransactionCodeTypeID =  tct.TransactionCodeTypeID 
						left outer join UAD_Lookup..CategoryCode cc with (nolock)
							on cc.CategoryCodeID = s.CategoryID	
						left outer join UAD_Lookup..Code cp with (nolock)
							on cp.CodeID = s.par3c  				    				        
						left outer join subscriptionsExtension se with (nolock)     
							on s.subscriptionID = se.subscriptionid'  
				END
			ELSE
				BEGIN
					SET @query = 'select ' + isnull(@scolumns,'') + CASE WHEN len(rtrim(ltrim(@mgcolumns))) > 0 THEN  ',' + rtrim(ltrim(@mgcolumns)) ELSE '' END + CASE WHEN len(rtrim(ltrim(@mgcolumnsdesc))) > 0 THEN  ',' + rtrim(ltrim(@mgcolumnsdesc)) ELSE '' END + Isnull(@custcolumns,'') + '
					from #TempSubscription subs join subscriptions s with (NOLOCK) 
						on s.SubscriptionID=subs.subsID ' 
						
					IF LEN(@mgcolumns) > 0
						SET @query = @query + '	left outer join     
						(
							SELECT * 
							 FROM
							 (
								SELECT 
									  [subscriptionID], DisplayName, 
									  STUFF((
										SELECT '', '' + CAST([MasterValue] AS VARCHAR(MAX)) 
										FROM #TempSubscriptionDetails
										WHERE ([subscriptionID] = Results.[subscriptionID] and DisplayName= Results.DisplayName) 
										order by MasterValue FOR XML PATH (''''))
									  ,1,2,'''') AS CombinedValues
									FROM #TempSubscriptionDetails Results
									GROUP BY [subscriptionID], DisplayName
							 ) u
							 PIVOT
							 (
							 MAX (CombinedValues)
							 FOR DisplayName in (' + @mgcolumns + ')
							 ) as pvt
						) y on y.subscriptionID = s.subscriptionID '
					
					IF LEN(@mgcolumnsdesc) > 0
						SET @query = @query + '	left outer join   
						(
							SELECT * 
							 FROM
							 (
								SELECT 
									  [subscriptionID], DisplayName, 
									  STUFF((
										SELECT '', '' + CAST([MasterDesc] AS VARCHAR(MAX)) 
										FROM #TempSubscriptionDetailsDesc
										WHERE ([subscriptionID] = Results.[subscriptionID] and DisplayName= Results.DisplayName) 
										order by MasterValue FOR XML PATH (''''))
									  ,1,2,'''') AS CombinedValues
									FROM #TempSubscriptionDetailsDesc Results
									GROUP BY [subscriptionID], DisplayName
							 ) u
							 PIVOT
							 (
							 MAX (CombinedValues)
							 FOR DisplayName in (' + @mgcolumnsdesc + ')
							 ) as pvt
						) z on z.subscriptionID = s.subscriptionID '
						
					SET @query = @query + '
						left outer join UAD_Lookup..Code c with (nolock)
							on c.CodeID = s.QSourceID       
						left outer join UAD_Lookup..[TransactionCode] tc  with (nolock)     
							on tc.TransactionCodeID =  s.TransactionID
						left outer join UAD_Lookup..[TransactionCodeType] tct with (nolock)      
							on tc.TransactionCodeTypeID =  tct.TransactionCodeTypeID
						left outer join UAD_Lookup..CategoryCode cc with (nolock)
							on cc.CategoryCodeID = s.CategoryID	
						left outer join UAD_Lookup..Code cp with (nolock)
							on cp.CodeID = s.par3c' 
				END
		END
	ELSE
		BEGIN
			if(LEN(@SubExtMapperColsAliased) > 0)
				begin
					SET @query =	'select distinct ' + isnull(@scolumns,'') + isnull(@SubExtMapperColsAliased,'') + isnull(@custcolumns,'') + '
									from #TempSubscription subs 
									join  subscriptions s  with (nolock)     
										on s.SubscriptionID=subs.subsID     
									left outer join UAD_Lookup..Code c with (nolock)
										on c.CodeID = s.QSourceID       
									left outer join UAD_Lookup..[TransactionCode] tc with (nolock)      
										on tc.TransactionCodeID =  s.TransactionID
									left outer join UAD_Lookup..[TransactionCodeType] tct with (nolock)      
										on tc.TransactionCodeTypeID =  tct.TransactionCodeTypeID 
									left outer join UAD_Lookup..CategoryCode cc with (nolock)
										on cc.CategoryCodeID = s.CategoryID	
									left outer join UAD_Lookup..Code cp with (nolock)
										on cp.CodeID = s.par3c  								   								     
									left outer join subscriptionsExtension se with (nolock)     
										on s.subscriptionID = se.subscriptionid' 
				END
			ELSE
				BEGIN
					SET @query =	'select distinct ' + isnull(@scolumns,'') + isnull(@custcolumns,'') + '
									from #TempSubscription subs             
									join subscriptions s with (nolock)     
										on s.SubscriptionID= subs.subsID        
									left outer join UAD_Lookup..Code c with (nolock)
										on c.CodeID = s.QSourceID       
									left outer join UAD_Lookup..[TransactionCode] tc with (nolock)      
										on tc.TransactionCodeID =  s.TransactionID
									left outer join UAD_Lookup..[TransactionCodeType] tct with (nolock)      
										on tc.TransactionCodeTypeID =  tct.TransactionCodeTypeID 
									left outer join UAD_Lookup..CategoryCode cc with (nolock)
										on cc.CategoryCodeID = s.CategoryID	
									left outer join UAD_Lookup..Code cp with (nolock)
										on cp.CodeID = s.par3c' 
				END
		END

	print @query + @brandScoreQuery + @LastOpenQuery

	exec (@query + @brandScoreQuery + @LastOpenQuery)			

	DROP TABLE #TempMasterGroups;	
	DROP TABLE #TempMasterGroupsDesc;
	DROP TABLE #TempSubExtMapperValues;
	Drop table #TempSubscriptionDetails;
	DROP TABLE #TempSubscription;

End
