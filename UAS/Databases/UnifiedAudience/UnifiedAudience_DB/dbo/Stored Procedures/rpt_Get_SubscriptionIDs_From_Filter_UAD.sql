CREATE proc [dbo].[rpt_Get_SubscriptionIDs_From_Filter_UAD]
(
	@PubIDs varchar(800),
	@CategoryIDs varchar(800),
	@CategoryCodes varchar(800),
	@TransactionIDs varchar(800),
	@QsourceIDs varchar(800),
	@StateIDs varchar(800),
	@Regions varchar(max),
	@CountryIDs varchar(800),
	@Email varchar(10),
	@Phone varchar(10),
	@Fax varchar(10),
	@Demo7 varchar(1),		
	@Demo31 varchar(1),		
	@Demo32 varchar(1),		
	@Demo33 varchar(1),		
	@Demo34 varchar(1),
	@Demo35 varchar(1),
	@Demo36 varchar(1),
	@ResponseIDs varchar(800),
	@UADResponseIDs varchar(2000),
	@AdHocXML varchar(8000),
	@IsMailable varchar(10),
	@EmailStatusIDs varchar(800),
	@OpenSearchType varchar(100),
	@OpenCount varchar(10),
	@OpenDateFrom varchar(10),
	@OpenDateTo varchar(10),
	@OpenBlastID varchar(20),
	@OpenEmailSubject varchar(800),
	@OpenEmailFromDate varchar(10),
	@OpenEmailToDate varchar(10),
	@ClickSearchType varchar(100),
	@ClickCount varchar(10),
	@ClickURL varchar(800),
	@ClickDateFrom varchar(10),
	@ClickDateTo varchar(10),
	@ClickBlastID varchar(20),
	@ClickEmailSubject varchar(800),
	@ClickEmailFromDate varchar(10),
	@ClickEmailToDate varchar(10),
	@Domain varchar(100),
	@VisitsURL varchar(800),
	@VisitsDateFrom varchar(10),
	@VisitsDateTo varchar(10),
	@BrandID varchar(20),
	@SearchType varchar(100),
	@RangeMaxLatMin varchar(100),
	@RangeMaxLatMax varchar(100),
	@RangeMaxLonMin varchar(100),
	@RangeMaxLonMax varchar(100),
	@RangeMinLatMin varchar(100),
	@RangeMinLatMax varchar(100),
	@RangeMinLonMin varchar(100),
	@RangeMinLonMax varchar(100)
)
AS         
BEGIN
	
	SET NOCOUNT ON
    
    BEGIN --- SET UP ---
    
		DECLARE	@executeString varchar(8000) = ''
		DECLARE @subQuery varchar(8000) = ''
		DECLARE @ScoreQuery varchar(8000) = ''
		DECLARE @selectClause varchar(8000) = ''
		DECLARE @openCondition varchar(8000) = ''
		DECLARE @clickCondition varchar(8000) = ''
		DECLARE @visitCondition varchar(8000) = ''
		DECLARE @openQuery varchar(8000) = ''
		DECLARE @clickQuery varchar(8000) = ''
		DECLARE @visitQuery varchar(8000) = ''
		DECLARE @AdHocCountryQuery varchar(8000) = ''
    
		CREATE TABLE #AdHoc
		(  
			RowID int IDENTITY(1, 1)
			,[FilterObject] nvarchar(256)
			,[SelectedCondition] nvarchar(256)
			,[Type] nvarchar(256)
			,[Value] nvarchar(256)
			,[ToValue] nvarchar(256)
			,[FromValue] nvarchar(256)
			,[GroupType] nvarchar(256)
			,[IDTag] nvarchar(256)
		)
	
		DECLARE @docHandle int

		EXEC sp_xml_preparedocument @docHandle OUTPUT, @AdHocXML  
		INSERT INTO #AdHoc 
		(
			 [FilterObject],[SelectedCondition],[Type],[Value],[ToValue],[FromValue],[GroupType],[IDTag]
		)  
		SELECT [FilterObject],[SelectedCondition],[Type],[Value],[ToValue],[FromValue],[GroupType],[IDTag]
		FROM OPENXML(@docHandle,N'/XML/FilterDetail')
		WITH
		(
			[FilterObject] nvarchar(256) 'FilterField',
			[SelectedCondition] nvarchar(256) 'SearchCondition',
			[Type] nvarchar(256) 'FilterObjectType',
			[Value] nvarchar(265) 'AdHocFieldValue',
			[ToValue] nvarchar(256) 'AdHocToField',
			[FromValue] nvarchar(256) 'AdHocFromField',
			[GroupType] nvarchar(256) 'GroupType',
			[IDTag] nvarchar(256) 'IDTag'
		)
	
		EXEC sp_xml_removedocument @docHandle
	
		set @executeString = ''
		set @selectClause = ' Select distinct s.SubscriptionID from Subscriptions s with (NOLOCK) join ' +
							'pubsubscriptions ps  with (NOLOCK) on s.subscriptionID = ps.subscriptionID'
		
	END				
	
	BEGIN --- Standard Filters ----
	
		if len(@PubIDs) > 0
			Begin
				set @subQuery += ' and ps1.PubID in ( ' + @PubIDs + ')'
				set @executeString += ' and ps.PubID in ( ' + @PubIDs + ')'
			End
	
		if LEN(@BrandID) > 0
			Begin
				set @selectClause = @selectClause + ' join BrandDetails bd with (NOLOCK) on bd.PubID = ps.PubID'
				set @subQuery += ' and bd1.BrandID = ' + @BrandID 
				set @executeString += ' and bd.BrandID = ' + @BrandID
			End	

		if len(@CategoryCodes) > 0
			Begin
				set @executeString = @executeString + ' and ps.PubCategoryID in ( ' + @CategoryCodes + ')'  
				set @subQuery = @subQuery + ' and ps1.PubCategoryID in ( ' + @CategoryCodes + ')' 
			End
	
		if len(@TransactionIDs) > 0
			Begin
				set @executeString = @executeString + ' and ps.PubTransactionID in ( select transactionid from [transaction] with(nolock) where transactiongroupid in ( ' + @TransactionIDs + ' ) )'
				set @subQuery = @subQuery + ' and ps1.PubTransactionID in ( select transactionid from [transaction] with(nolock) where transactiongroupid in ( ' + @TransactionIDs + ' ) )'
			End
	
		if len(@QsourceIDs) > 0
			Begin
				set @executeString = @executeString + ' and ps.PubQSourceID in ( select qsourceid from qsource with(nolock) where qsourcegroupid in ( ' + @QsourceIDs + ' ) )'
				set @subQuery = @subQuery + ' and ps1.PubQSourceID in ( select qsourceid from qsource with(nolock) where qsourcegroupid in ( ' + @QsourceIDs + ' ) )'
			End
	
		if len(@EmailStatusIDs) > 0
			Begin
				set @executeString = @executeString + ' and ps.EmailStatusID in ( ' + @EmailStatusIDs + ')' 
				set @subQuery = @subQuery + ' and ps1.EmailStatusID in ( ' + @EmailStatusIDs + ')' 
			End

		if len(@StateIDs) > 0
			Begin
				set @subQuery += ' and sfilter.State in ( select RegionCode from UAD_Lookup..Region with(nolock) where RegionID in (' + @StateIDs + '))'
				set @executeString += ' and s.State in ( select RegionCode from UAD_Lookup..Region with(nolock) where RegionID in (' + @StateIDs + '))'
			end

		if len(@Email) > 0
			Begin
				if @Email = 'Yes'
					BEGIN
						set @executeString = @executeString + ' and s.EmailExists = 1 ' 
						set @subQuery = @subQuery + ' and sfilter.EmailExists = 1 ' 
					END
				else
					BEGIN
						set @executeString = @executeString + ' and s.EmailExists = 0 '
						set @subQuery = @subQuery + ' and sfilter.EmailExists = 0 ' 
					END
			End

		if len(@Phone) > 0
			Begin
				if @Phone = 'Yes'
					BEGIN
						set @executeString = @executeString + ' and s.PhoneExists = 1 ' 
						set @subQuery = @subQuery + ' and sfilter.PhoneExists = 1 ' 
					END
				else
					BEGIN
						set @executeString = @executeString + ' and s.PhoneExists = 0 '
						set @subQuery = @subQuery + ' and sfilter.PhoneExists = 0 ' 
					END
			End

		if len(@Fax) > 0
			Begin
				if @Fax = 'Yes'
					BEGIN
						set @executeString = @executeString + ' and s.FaxExists = 1 ' 
						set @subQuery = @subQuery + ' and sfilter.FaxExists = 1 ' 
					END
				else
					BEGIN
						set @executeString = @executeString + ' and s.FaxExists = 0 ' 
						set @subQuery = @subQuery + ' and sfilter.FaxExists = 0 ' 
					END
			End
	
		if len(@IsMailable) > 0
			Begin
				if @IsMailable = 'Yes'
					Begin
						set @executeString = @executeString + ' and Isnull(s.IsMailable,'''') <> ''''' 
						set @subQuery = @subQuery + ' and Isnull(sfilter.IsMailable,'''') <> ''''' 
					End
				else
					BEGIN
						set @executeString = @executeString + ' and Isnull(s.IsMailable,'''') = ''''' 
						set @subQuery = @subQuery + ' and Isnull(sfilter.IsMailable,'''') = ''''' 
					END
			End

		if len(@Demo31) > 0
			Begin
				set @executeString = @executeString + ' and s.Demo31 = ' + (CASE @Demo31 when 'Yes' then '1' else '0' END)
				set @subQuery += ' and sfilter.Demo31 = ' + (CASE @Demo31 when 'Yes' then '1' else '0' END)
			End

		if len(@Demo32) > 0
			Begin
				set @executeString = @executeString + ' and s.Demo32 = ' + (CASE @Demo32 when 'Yes' then '1' else '0' END)
				set @subQuery += ' and sfilter.Demo32 = ' + (CASE @Demo32 when 'Yes' then '1' else '0' END)
			End

		if len(@Demo33) > 0
			Begin
				set @executeString = @executeString + ' and s.Demo33 = ' + (CASE @Demo33 when 'Yes' then '1' else '0' END)
				set @subQuery += ' and sfilter.Demo33 = ' + (CASE @Demo33 when 'Yes' then '1' else '0' END)
			End

		if len(@Demo34) > 0
			Begin
				set @executeString = @executeString + ' and s.Demo34 = ' + (CASE @Demo34 when 'Yes' then '1' else '0' END)
				set @subQuery += ' and sfilter.Demo34 = ' + (CASE @Demo34 when 'Yes' then '1' else '0' END)
			End

		if len(@Demo35) > 0
			Begin
				set @executeString = @executeString + ' and s.Demo35 = ' + (CASE @Demo35 when 'Yes' then '1' else '0' END)
				set @subQuery += ' and sfilter.Demo35 = ' + (CASE @Demo35 when 'Yes' then '1' else '0' END)
			End

		if len(@Demo36) > 0
			Begin
				set @executeString = @executeString + ' and s.Demo36 = ' + (CASE @Demo36 when 'Yes' then '1' else '0' END)
				set @subQuery += ' and sfilter.Demo36 = ' + (CASE @Demo36 when 'Yes' then '1' else '0' END)
			End
	
		if LEN(@Demo7) > 0
			Begin
				set @executeString = @executeString + ' and ps.Demo7 in (select CodeValue from UAD_Lookup..Code with(nolock) WHERE CodeId in (''' + @Demo7 + '''))'
				set @subQuery += ' and ps1.Demo7 in (select CodeValue from UAD_Lookup..Code with(nolock) WHERE CodeId in (''' + @Demo7 + '''))' 
			End
	
		if len(@CountryIDs) > 0
			Begin
				--insert into #country select items from dbo.fn_split(@CountryIDs, ',')

				--(1, Only US 
				--(2, Only Canada 
				--(3, US and Canada 
				--(4, International 

				--if @CountryIDs = '1'
				--	set @executeString = @executeString + ' and s.CountryID = 1 '
				--else if @CountryIDs = '2'
				--	set @executeString = @executeString + ' and s.CountryID = 2 '
				--else if @CountryIDs = '3'
				--	set @executeString = @executeString + ' and s.CountryID in (1,2) '
				--else if @CountryIDs = '4'
				--	set @executeString = @executeString + ' and s.CountryID not in (1,2) '
				--else
				set @executeString = @executeString + ' and s.CountryID in (' + @CountryIDs + ')'
				set @subQuery = @subQuery + ' and sfilter.CountryID in (' + @CountryIDs + ')'

			end
	
		if LEN(@RangeMaxLatMax) > 0 AND LEN(@RangeMaxLatMin) > 0 AND LEN(@RangeMaxLonMax) > 0 AND LEN(@RangeMaxLonMin) > 0 AND LEN(@RangeMinLatMax) > 0 AND LEN(@RangeMinLatMin) > 0 AND LEN(@RangeMinLonMax) > 0 AND LEN(@RangeMinLonMin) > 0
			BEGIN
				set @subQuery += ' and ('
				set @executeString += ' and ('
				set @subQuery +=  ' sfilter.Latitude >= ' + @RangeMaxLatMin + ' and ' + 'sfilter.Latitude <= ' + @RangeMaxLatMax
				set @executeString +=  ' s.Latitude >= ' + @RangeMaxLatMin + ' and ' + 's.Latitude <= ' + @RangeMaxLatMax
				set @subQuery +=  ' and sfilter.Longitude >= ' + @RangeMaxLonMin + ' and ' + 'sfilter.Longitude <= ' + @RangeMaxLonMax
				set @executeString +=  ' and s.Longitude >= ' + @RangeMaxLonMin + ' and ' + 's.Longitude <= ' + @RangeMaxLonMax
				set @subQuery += ' and ( sfilter.Latitude <= ' + @RangeMinLatMin + ' OR '+ @RangeMinLatMax + ' <= sfilter.Latitude  OR sfilter.Longitude <= ' + @RangeMinLonMin + ' OR ' + @RangeMinLonMax + '<= sfilter.Longitude )'
				set @executeString += ' and (s.Latitude <= ' + @RangeMinLatMin + ' OR '+ @RangeMinLatMax + ' <= s.Latitude  OR s.Longitude <= ' + @RangeMinLonMin + ' OR ' + @RangeMinLonMax + '<= s.Longitude )'
				set @subQuery += ' and isnull(s.IsLatLonValid,0) = 1 )'
				set @executeString += ' and isnull(s.IsLatLonValid,0) = 1 )'
			END
	
	END

	BEGIN --- Ad Hoc Filters ---
		if LEN(@AdHocXML) > 0
			BEGIN
				declare @Column varchar(100),
							@Value varchar(100), 
							@ValueFrom varchar(100), 
							@ValueTo varchar(100), 
							@DataType varchar(100), 
							@Condition varchar(100),
							@GroupType varchar(100),
							@ID varchar(100)
				 
				DECLARE @NumberRecords int, @RowCount int, @AdHocString varchar(100), @AdHocFinal varchar(8000)
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
									@DataType = [Type],
									@GroupType = GroupType,
									@Condition = SelectedCondition,
									@ID = IDTag
							FROM #AdHoc
							WHERE RowID = @RowCount
					
							print 'Row: ' + convert(varchar(100), @RowCount)
							print 'Column: '+ @Column + '; Value: ' + @Value + '; ValueFrom: ' + @ValueFrom + '; ValueTo: ' + @ValueTo + '; DataType: ' + @DataType + '; GroupType: ' + @GroupType + '; Condition: ' + @Condition + '; ID: ' + @ID
					
							IF LEN(@executeString) > 0
								BEGIN
									set @executeString += ' and '
								END
					
							IF @GroupType = 'MasterGroup'
								BEGIN
									IF LEN(@subQuery) > 0
										BEGIN
											set @subQuery += ' and '
										END
									declare @query_isnull varchar(8000) = ''
									declare @query_notnull varchar(8000) = ''
						
									IF LEN(@BrandID) = 0
										BEGIN
						
											set @query_isnull = ' (select distinct sfilter.SubscriptionID from subscriptions sfilter with (NOLOCK) ' +
														' left outer JOIN (select distinct sd.subscriptionID from subscriptiondetails sd WITH (nolock) ' +
															' join Mastercodesheet ms  with (NOLOCK) on sd.MasterID = ms.MasterID ' +
														' where ms.MasterGroupID=' + @ID + ') inn1 on sfilter.SubscriptionID = inn1.SubscriptionID ' +
														' where inn1.subscriptionID is null and '
                                        
											set @query_notnull = ' (select distinct sfilter.SubscriptionID from subscriptions sfilter with (NOLOCK) ' +
														' join Subscriptiondetails sd  with (NOLOCK) on sd.SubscriptionID = sfilter.SubscriptionID ' +
														' join Mastercodesheet ms  with (NOLOCK) on sd.MasterID = ms.MasterID' +
														' where ms.MasterGroupID =' + @ID + ' and '
										END
									ELSE
										BEGIN
											set @query_isnull = ' (SELECT distinct sfilter.subscriptionid FROM subscriptions sfilter WITH (NOLOCK) ' +
														' JOIN pubsubscriptions ps1 WITH (NOLOCK) ON sfilter.subscriptionID = ps1.subscriptionID ' +
														' JOIN BrandDetails bd with (NOLOCK) on bd.PubID = ps1.PubID and bd.BrandID = ' + @BrandID + ' ' +
														' left outer join ( ' +
														' SELECT DISTINCT sd.subscriptionid  ' +
														' FROM   subscriptiondetails sd WITH (nolock)  ' +
														' 	   JOIN vw_brandconsensus v WITH (nolock) ON v.subscriptionid = sd.subscriptionid  ' +
														' 	   JOIN mastercodesheet ms WITH (nolock) ON v.masterid = ms.masterid  ' +
														' 	   JOIN branddetails bd5 WITH ( nolock) ON bd5.brandid = v.brandid  ' +
														' WHERE  bd5.brandid = ' + @BrandID + ' AND ms.mastergroupid = ' + @ID +  ') inn3 on sfilter.SubscriptionID = inn3.SubscriptionID	 ' +
														' WHERE inn3.SubscriptionID is null and '
                                        
											set @query_notnull =  ' (select distinct sfilter.SubscriptionID from subscriptions sfilter  with (NOLOCK) ' +
															' join vw_BrandConsensus v  with (NOLOCK) on v.SubscriptionID = sfilter.SubscriptionID ' +
															' join Mastercodesheet ms with (NOLOCK)  on v.MasterID = ms.MasterID ' +
															' join BrandDetails bd  with (NOLOCK) on bd.BrandID = v.BrandID ' +
															' where  (bd.BrandID = ' + @BrandID + ' and ms.MasterGroupID=' + @ID + ' and ('
										END

									IF @Condition = 'Equal' 
										BEGIN                       
											set @subQuery += ' s.SubscriptionID in ' + @query_notnull
											set @executeString += ' s.SubscriptionID in ' + @query_notnull
										END
									IF @Condition = 'Contains'
										BEGIN
											set @subQuery += ' s.SubscriptionID in ' + @query_notnull
											set @executeString += ' s.SubscriptionID in ' + @query_notnull
										END
									IF @Condition = 'Does Not Contain'
										BEGIN
											set @subQuery += ' s.SubscriptionID not in ' + @query_notnull
											set @executeString += ' s.SubscriptionID not in ' + @query_notnull  
										END
									IF @Condition = 'Start With'
										BEGIN	               
											set @subQuery += ' s.SubscriptionID in ' + @query_notnull
											set @executeString += ' s.SubscriptionID in ' + @query_notnull
										END
									IF @Condition = 'End With'
										BEGIN
											set @subQuery += ' s.SubscriptionID in ' + @query_notnull
											set @executeString += ' s.SubscriptionID in ' + @query_notnull
										END
									IF @Condition = 'Is Empty'
										BEGIN
											set @subQuery += ' s.SubscriptionID in ' + @query_isnull
											set @executeString += ' s.SubscriptionID in ' + @query_isnull
										END

									DECLARE @Val varchar(100) = ''
									DECLARE @count int = 0
						
									DECLARE groups Cursor FOR
									Select * 
									FROM fn_Split(@Value, ',')
									OPEN groups
						
									FETCH NEXT FROM groups
									INTO @Val
									WHILE @@FETCH_STATUS = 0
										BEGIN	
											IF @Condition = 'Equal'
												BEGIN
													set @subQuery += (CASE WHEN (@count > 0) then ' OR ' else '' END) + ' (ms.MasterDesc = ''' + @Val + ''' or ms.Mastervalue = ''' + @Val + ''')'	
													set @executeString += (CASE WHEN (@count > 0) then ' OR ' else '' END) + ' (ms.MasterDesc = ''' + @Val + ''' or ms.Mastervalue = ''' + @Val + ''')'
												END
											IF @Condition = 'Does Not Contain' OR @Condition = 'Contains'
												BEGIN
													set @subQuery += (CASE WHEN (@count > 0) then ' OR ' else '' END) + ' (ms.MasterDesc LIKE ''%' + @Val + '%'' or ms.Mastervalue LIKE ''%' + @Val + '%'')'	
													set @executeString += (CASE WHEN (@count > 0) then ' OR ' else '' END) + ' (ms.MasterDesc LIKE ''%' + @Val + '%'' or ms.Mastervalue LIKE ''%' + @Val + '%'')'
												END
											IF @Condition = 'Start With'
												BEGIN
													set @subQuery += (CASE WHEN (@count > 0) then ' OR ' else '' END) + ' (ms.MasterDesc LIKE ''%' + @Val + '%'' or ms.Mastervalue LIKE ''%' + @Val + '%'')'	
													set @executeString += (CASE WHEN (@count > 0) then ' OR ' else '' END) + ' (ms.MasterDesc LIKE ''%' + @Val + '%'' or ms.Mastervalue LIKE ''%' + @Val + '%'')'
												END
											IF @Condition = 'End With'
												BEGIN
													set @subQuery += (CASE WHEN (@count > 0) then ' OR ' else '' END) + ' (ms.MasterDesc LIKE ''%' + @Val + '%'' or ms.Mastervalue LIKE ''%' + @Val + '%'')'	
													set @executeString += (CASE WHEN (@count > 0) then ' OR ' else '' END) + ' (ms.MasterDesc LIKE ''%' + @Val + '%'' or ms.Mastervalue LIKE ''%' + @Val + '%'')'
												END
							
											set @count += 1
							
											FETCH NEXT FROM groups
											INTO @Val	
										END
									CLOSE groups
									DEALLOCATE groups
						
									IF LEN(@BrandID) > 0 AND @Condition <> 'Is Empty'
										BEGIN
											set @subQuery += '))'
											set @executeString += '))'
										END
						
									set @subQuery += ')'
									set @executeString += ')'
						
								END
							ELSE IF @DataType = 'DateRange' AND @GroupType <> 'SubscriptionsExtension'
								BEGIN
									IF LEN(@subQuery) > 0
										BEGIN
											set @subQuery += ' and '
										END
									set @subQuery += '('
									set @executeString += '('
						
									declare @i int = 0
									declare @sub_query_field varchar(1000) = ''
									declare @wherecondition_field varchar(1000) = ''
						
									IF @Column = 'QDate'
										BEGIN
											set @sub_query_field = 'ps1.QualificationDate'
											set @wherecondition_field = 'ps.QualificationDate'
										END
									ELSE IF @Column = 'TransactionDate'
										BEGIN
											set @sub_query_field = 'CONVERT(date, sfilter.' + @Column + ')'
											set @wherecondition_field = 'CONVERT(date, s.' + @Column + ')'
										END
									ELSE
										BEGIN
											set @sub_query_field = 'sfilter.' + @Column
											set @wherecondition_field = 's.' + @Column
										END
						
									IF @Condition = 'Range'
										BEGIN
											set @subQuery += @sub_query_field + ' >= ''' + @ValueFrom + ''''
											set @subQuery += ' and ' + @sub_query_field + ' <= ''' + @ValueTo + ''''
											set @executeString += @wherecondition_field + ' >= ''' + @ValueFrom + ''''
											set @executeString += ' and ' + @wherecondition_field + ' <= ''' + @ValueTo + ''''
										END
									ELSE IF @Condition = 'Year'
										BEGIN
											set @subQuery += 'year(' + @sub_query_field + ') >= ''' + @ValueFrom + ''''
											set @subQuery += ' and year(' + @sub_query_field + ') <= ''' + @ValueTo + ''''
											set @executeString += 'year(' + @wherecondition_field + ') >= ''' + @ValueFrom + ''''
											set @executeString += ' and year(' + @wherecondition_field + ') <= ''' + @ValueTo + ''''	
										END
									ELSE IF @Condition = 'Month'
										BEGIN
											set @subQuery += 'month(' + @sub_query_field + ') >= ''' + @ValueFrom + ''''
											set @subQuery += ' and month(' + @sub_query_field + ') <= ''' + @ValueTo + ''''
											set @executeString += 'month(' + @wherecondition_field + ') >= ''' + @ValueFrom + ''''
											set @executeString += ' and month(' + @wherecondition_field + ') <= ''' + @ValueTo + ''''
										END	
					
									set @subQuery += ')'
									set @executeString += ')'
								END				
							ELSE IF @GroupType = 'Integer' OR @GroupType = 'Float'
								BEGIN
									IF LEN(@subQuery) > 0
										BEGIN
											set @subQuery += ' and '
										END
									IF @Column = 'Product Count'
										BEGIN
											set @subQuery += '('
											set @executeString += '('
							
											IF LEN(@BrandID) > 0
												BEGIN
													set @subQuery += ' s.SubscriptionID in (SELECT ps2.subscriptionid FROM pubsubscriptions ps2 WITH (NOLOCK) JOIN branddetails bd2 WITH (nolock) ON bd2.pubid = ps2.pubid where bd2.BrandID = ' + @BrandID + ' group by ps2.subscriptionid '
													set @executeString += ' s.SubscriptionID in (SELECT ps2.subscriptionid FROM pubsubscriptions ps2 WITH (NOLOCK) JOIN branddetails bd2 WITH (nolock) ON bd2.pubid = ps2.pubid where bd2.BrandID = ' + @BrandID + ' group by ps2.subscriptionid '
												END
											ELSE
												BEGIN
													set @subQuery += ' s.SubscriptionID in (SELECT ps2.subscriptionid FROM pubsubscriptions ps2 WITH (NOLOCK) group by ps2.subscriptionid '
													set @executeString += ' s.SubscriptionID in (SELECT ps2.subscriptionid FROM pubsubscriptions ps2 WITH (NOLOCK) group by ps2.subscriptionid '
												END
							
											IF @Condition = 'Equal'
												BEGIN
													set @subQuery += ' having COUNT(ps2.PubID) = ' + @ValueFrom + ')'
													set @executeString += ' having COUNT(ps2.PubID) = ' + @ValueFrom + ')'
												END
											ELSE IF @Condition = 'Greater Than'
												BEGIN
													set @subQuery += ' having COUNT(ps2.PubID) > ' + @ValueFrom + ')'
													set @executeString += ' having COUNT(ps2.PubID) > ' + @ValueFrom + ')'
												END
											ELSE IF @Condition = 'Lesser Than'
												BEGIN
													set @subQuery += ' having COUNT(ps2.PubID) < ' + @ValueFrom + ')'
													set @executeString += ' having COUNT(ps2.PubID) < ' + @ValueFrom + ')'
												END
											ELSE IF @Condition = 'Range'
												BEGIN
													set @subQuery += ' having COUNT(ps2.PubID) between ' + @ValueFrom + ' and ' + @ValueTo + ')'
													set @executeString += ' having COUNT(ps2.PubID) between ' + @ValueFrom + ' and ' + @ValueTo + ')'
												END
							
											set @subQuery += ')'
											set @executeString += ')'
										END
									ELSE
										BEGIN
											IF LEN(@BrandID) > 0 AND @Column = 'Score'
												BEGIN
													set @selectClause += ' join BrandScore bs  with (NOLOCK)  on s.SubscriptionId = bs.SubscriptionId and bs.BrandID = ' + @BrandID
													set @ScoreQuery += ' join BrandScore bs1  with (NOLOCK)  on sfilter.SubscriptionId = bs1.SubscriptionId and bs1.BrandID = ' + @BrandID
												END
							
											set @subQuery += '('
											set @executeString += '('
							
											IF @Condition = 'Equal'
												BEGIN
													IF @ValueFrom <> ''
														BEGIN
															IF LEN(@BrandID) > 0 AND @Column = 'Score'
																BEGIN
																	set @subQuery += 'bs1.' + @Column + ' = ' + @ValueFrom
																	set @executeString += 'bs.' + @Column + ' = ' + @ValueFrom
																END
															ELSE
																BEGIN
																	set @subQuery += 'sfilter.' + @Column + ' = ' + @ValueFrom
																	set @executeString += 's.' + @Column + ' = ' + @ValueFrom
																END
														END
												END	
											IF @Condition = 'Range'
												BEGIN
													IF LEN(@BrandID) > 0 AND @Column = 'Score'
														BEGIN
															set @subQuery += 'bs1.' + @Column + ' >= ' + @ValueFrom
															set @subQuery += ' and bs1.' + @Column + ' <= ' + @ValueTo
															set @executeString += 'bs.' + @Column + ' >= ' + @ValueFrom
															set @executeString += ' and bs.' + @Column + ' <= ' + @ValueTo
														END
													ELSE
														BEGIN
															set @subQuery += 'sfilter.' + @Column + ' >= ' + @ValueFrom
															set @subQuery += ' and sfilter.' + @Column + ' <= ' + @ValueTo
															set @executeString += 's.' + @Column + ' >= ' + @ValueFrom
															set @executeString += ' and s.' + @Column + ' <= ' + @ValueTo
														END
												END
											IF @Condition = 'Greater Than'
												BEGIN
													IF LEN(@BrandID) > 0 AND @Column = 'Score'
														BEGIN
															set @subQuery += 'bs1.' + @Column + ' > ' + @ValueFrom
															set @executeString += 'bs.' + @Column + ' > ' + @ValueFrom
														END
													ELSE
														BEGIN
															set @subQuery += 'sfilter.' + @Column + ' > ' + @ValueFrom
															set @executeString += 's.' + @Column + ' > ' + @ValueFrom
														END
												END
											IF @Condition = 'Lesser Than'
												BEGIN
													IF LEN(@BrandID) > 0 AND @Column = 'Score'
														BEGIN
															set @subQuery += 'bs1.' + @Column + ' < ' + @ValueFrom
															set @executeString += 'bs.' + @Column + ' < ' + @ValueFrom
														END
													ELSE
														BEGIN
															set @subQuery += 'sfilter.' + @Column + ' < ' + @ValueFrom
															set @executeString += 's.' + @Column + ' < ' + @ValueFrom
														END
												END
							
											set @subQuery += ')'
											set @executeString += ')'	
										END	
								END	
							ELSE IF @GroupType = 'SubscriptionsExtension'
								BEGIN
									--IF LEN(@subQuery) >= 5
									--	set @subQuery = substring(@subQuery, 5, len(@subQuery))
									IF @Condition <> 'Does Not Contain'
										set @executeString += ' s.SubscriptionID in (SELECT E.SubscriptionID' +
																  ' FROM SubscriptionsExtension E with (NOLOCK) ' +
																  ' WHERE '
									ELSE
										set @executeString += ' s.SubscriptionID not in (SELECT E.SubscriptionID' +
																  ' FROM SubscriptionsExtension E with (NOLOCK) ' +
																  ' WHERE '
                                                      
									IF @DataType = 'Integer' OR @DataType = 'Float'
										BEGIN
											IF @Condition = 'Equal'
												set @executeString += @Column + ' = ' + @ValueFrom
											IF @Condition = 'Range'
												set @executeString += @Column + ' >= ' + @ValueFrom + ' and ' + @Column + ' <= ' + @ValueTo
											IF @Condition = 'Greater Than'
												set @executeString += @Column + ' > ' + @ValueFrom
											IF @Condition = 'Lesser Than'
												set @executeString += @Column + ' < ' + @ValueFrom
								
											set @executeString += '))'
										END
									IF @DataType = 'Bit'
										set @executeString += @Column + ' = ' + @Value
									IF @DataType = 'DateTime'
										BEGIN
											IF @Condition = 'Range'
												BEGIN
													set @executeString += @wherecondition_field + ' >= ''' + @ValueFrom + ''
													set @executeString += ' and ' + @wherecondition_field + ' <= ''' + @ValueTo + ''
												END
											ELSE IF @Condition = 'Year'
												BEGIN
													set @executeString += 'year(' + @wherecondition_field + ') > = ''' + @ValueFrom + ''
													set @executeString += ' and year(' + @wherecondition_field + ') <= ''' + @ValueTo + ''	
												END
											ELSE IF @Condition = 'Month'
												BEGIN
													set @executeString += 'month(' + @wherecondition_field + ') >= ''' + @ValueFrom + ''
													set @executeString += ' and month(' + @wherecondition_field + ') <= ''' + @ValueTo + ''	
												END	
											set @executeString += '))'
										END
									ELSE
										BEGIN
											set @Val = ''
											set @count = 0
							
											DECLARE groups Cursor FOR
											Select * 
											FROM fn_Split(@Value, ',')
											OPEN groups
							
											FETCH NEXT FROM groups
											INTO @Val
											WHILE @@FETCH_STATUS = 0
												BEGIN	
													IF @Condition = 'Equal'
														set @executeString += (CASE WHEN (@count > 0) THEN ' OR ' ELSE '' END) + ' ' + @Column + ' = ''' + @Val + ''''							
													IF @Condition = 'Does Not Contain' OR @Condition = 'Contains'
														set @executeString += (CASE WHEN (@count > 0) THEN ' OR ' ELSE '' END) + ' ' + @Column + ' like  ''%' + @Val + '%'''	
													IF @Condition = 'Start With'
														set @executeString += (CASE WHEN (@count > 0) THEN ' OR ' ELSE '' END) + ' ' + @Column + ' like  ''' + @Val + ''''
													IF @Condition = 'End With'
														set @executeString += (CASE WHEN (@count > 0) THEN ' OR ' ELSE '' END) + ' ' + @Column + ' like  ''%' + @Val + ''''
													IF @Condition = 'Is Empty'
														set @executeString += (CASE WHEN (@count > 0) THEN ' OR ' ELSE '' END) + ' (' + @Column + ' is null or ' + @Column + ' = '')'
										
													set @count += 1
								
													FETCH NEXT FROM groups
													INTO @Val	
												END
											CLOSE groups
											DEALLOCATE groups							
							
											set @executeString += ')'
										END
								END
							ELSE IF @DataType = 'Standard'
								BEGIN
									IF LEN(@subQuery) > 0
										BEGIN
											set @subQuery += ' and '
										END
					
									IF @DataType = 'Range'
										BEGIN
											set @subQuery += @Column + ' between ' + @ValueFrom + ' and ''' + @ValueTo
											set @executeString += @Column + ' between ' + @ValueFrom + ' and ''' + @ValueTo
										END
									ELSE
										BEGIN
											set @sub_query_field = ''
											set @wherecondition_field = ''
							
											set @subQuery += '('
											set @executeString += '('
							
											IF @Column = 'Country'
												BEGIN
													IF @Condition = 'Is Empty'
														BEGIN
															set @selectClause += ' left outer join UAD_Lookup..Country on c.CountryID = s.CountryID '
															set @AdHocCountryQuery += ' left outer join UAD_Lookup..Country c1 on c.CountryID = sfilter.CountryID '
														END
													ELSE
														BEGIN
															set @selectClause += ' join UAD_Lookup..Country c on c.CountryID = s.CountryID '
															set @AdHocCountryQuery += ' join UAD_Lookup..Country c1 on c.CountryID = sfilter.CountryID '
														END
								
													set @sub_query_field = 'c1.' + @Column
													set @wherecondition_field = 'c.' + @Column
												END
											ELSE IF @Column = 'Email'
												BEGIN
													set @sub_query_field = 'ps1.' + @Column
													set @wherecondition_field = 'ps.' + @Column
												END
											ELSE
												BEGIN
													set @sub_query_field = 'sfilter.' + @Column
													set @wherecondition_field = 's.' + @Column
												END
											set @Val = ''
											set @count = 0
											DECLARE groups Cursor FOR
											Select * 
											FROM fn_Split(@Value, ',')
											OPEN groups
							
											FETCH NEXT FROM groups
											INTO @Val
											WHILE @@FETCH_STATUS = 0
												BEGIN	
													IF @Condition = 'Equal'
														BEGIN
															set @subQuery += (CASE WHEN (@count > 0) THEN ' OR ' ELSE '' END) + '( ' + @sub_query_field + ' = ''' + @Val + ''')'
															set @executeString += (CASE WHEN (@count > 0) THEN ' OR ' ELSE '' END) + '( ' + @wherecondition_field + ' = ''' + @Val + ''')'
														END 							
													IF @Condition = 'Does Not Contain' OR @Condition = 'Contains'
														BEGIN
															set @subQuery += (CASE WHEN (@count > 0) THEN ' OR ' ELSE '' END) + '( ' + @sub_query_field + ' like ''%' + @Val + '%'')'
															set @executeString += (CASE WHEN (@count > 0) THEN ' OR ' ELSE '' END) + '( ' + @wherecondition_field + ' like ''%' + @Val + '%'')'
														END
													IF @Condition = 'Start With'
														BEGIN
															set @subQuery += (CASE WHEN (@count > 0) THEN ' OR ' ELSE '' END) + '( ' + @sub_query_field + ' like ''' + @Val + '%'')'
															set @executeString += (CASE WHEN (@count > 0) THEN ' OR ' ELSE '' END) + '( ' + @wherecondition_field + ' like ''' + @Val + '%'')'
														END
													IF @Condition = 'End With'
														BEGIN
															set @subQuery += (CASE WHEN (@count > 0) THEN ' OR ' ELSE '' END) + '( ' + @sub_query_field + ' like ''%' + @Val + ''')'
															set @executeString += (CASE WHEN (@count > 0) THEN ' OR ' ELSE '' END) + '( ' + @wherecondition_field + ' like ''%' + @Val + ''')'
														END	
													IF @Condition = 'Does Not Contain'
														BEGIN
															set @subQuery += (CASE WHEN (@count > 0) THEN ' AND ' ELSE '' END) + '( isnull( ' + @sub_query_field + ', '''') not like ''%' + @Val + '%'')'
															set @executeString += (CASE WHEN (@count > 0) THEN ' AND ' ELSE '' END) + '( isnull( ' + @wherecondition_field + ', '''') not like ''%' + @Val + '%'')'
														END	
													IF @Condition = 'Is Empty'
														BEGIN
															IF @Column = 'Country'
																BEGIN
																	set @subQuery += (CASE WHEN (@count > 0) THEN ' OR ' ELSE '' END) + ' (c1.CountryID is NULL)'
																	set @executeString += (CASE WHEN (@count > 0) THEN ' OR ' ELSE '' END) + ' (c.CountryID is NULL)'
																END
															ELSE
																BEGIN
																	set @subQuery += (CASE WHEN (@count > 0) THEN ' OR ' ELSE '' END) + ' ( ' + @sub_query_field + ' is NULL or ' + @sub_query_field + ' = '''')'
																	set @executeString += (CASE WHEN (@count > 0) THEN ' OR ' ELSE '' END) + ' ( ' + @wherecondition_field + ' is NULL or ' + @wherecondition_field + ' = '''')'
																END
														END
										
													set @count += 1
								
													FETCH NEXT FROM groups
													INTO @Val	
												END
											CLOSE groups
											DEALLOCATE groups
										END
						
									set @subQuery += ')'
									set @executeString += ')'
								END			
					
							SET @RowCount = @RowCount + 1										 				
						END				
			END
	END
	
	BEGIN --- Dynamic Filters ----
	
		--if len(@subQuery) > 0
		--Begin
		--	set @subQuery = substring(@subQuery, 5, len(@subQuery))
		--End	
	
		DECLARE @added_master_id bit = 0
		DECLARE @bubble_up_query varchar(8000)
		DECLARE @bubble_up_query_igrp_start varchar(8000)
	
		IF @SearchType = 'ProductView'
			set @bubble_up_query_igrp_start = 'select sfilter.SubscriptionID from subscriptions sfilter  with (NOLOCK) join pubsubscriptions ps1  with (NOLOCK) on sfilter.subscriptionID = ps1.subscriptionID join PubSubscriptionDetail psd  with (NOLOCK) on ps1.pubsubscriptionID = psd.pubsubscriptionID ' + @AdHocCountryQuery + ' '
		ELSE
			Begin
				If LEN(@BrandID) > 0
					set @bubble_up_query_igrp_start = ' SELECT sfilter.subscriptionid FROM  subscriptions sfilter WITH (nolock) join pubsubscriptions ps1  with (NOLOCK) on sfilter.subscriptionID = ps1.subscriptionID ' + @AdHocCountryQuery + ' JOIN vw_BrandConsensus v  ON sfilter.subscriptionid = v.subscriptionid join branddetails bd1  with (NOLOCK) on bd1.brandID = v.brandID '
				ELSE
					set @bubble_up_query_igrp_start = ' select sfilter.SubscriptionID from subscriptions sfilter  with (NOLOCK) join pubsubscriptions ps1  with (NOLOCK) on sfilter.subscriptionID = ps1.subscriptionID ' + @AdHocCountryQuery + ' JOIN SubscriptionDetails sd  with (NOLOCK) on sd.subscriptionid = sfilter.subscriptionid '
			End		

		DECLARE @ids varchar(500)
	
		DECLARE groups Cursor FOR
		Select * 
		FROM fn_Split(@UADResponseIDs, ':')
		OPEN groups
	
		FETCH NEXT FROM groups
		INTO @ids
		WHILE @@FETCH_STATUS = 0
			BEGIN	
				DECLARE @bubble_up_query_start varchar(8000) = ''
				set @bubble_up_query_start = @bubble_up_query_igrp_start
		
				DECLARE @old_bubble_up_query varchar(8000) = @bubble_up_query
				
				IF @added_master_id = 1
					BEGIN
						IF @SearchType = 'ProductView'
							BEGIN
								set @bubble_up_query = @bubble_up_query_start + ' and ps1.pubID = ' + @PubIDs + ' and psd.CodeSheetID in ( ' + @ids + ' ) and sfilter.SubscriptionID in ( ' + @old_bubble_up_query + ' ) '
							END
						ELSE
							BEGIN
								IF LEN(@BrandID) > 0
									set @bubble_up_query = @bubble_up_query_start + ' and v.masterid in ( ' + @ids + ' ) and sfilter.SubscriptionID in ( ' + @old_bubble_up_query + ')'
								ELSE
									set @bubble_up_query = @bubble_up_query_start + ' and sd.masterid in ( ' + @ids + ' ) and sfilter.SubscriptionID in ( ' + @old_bubble_up_query + ')'
							END
					END
				ELSE
					BEGIN
						set @bubble_up_query = @bubble_up_query_start + ' '
						IF LEN(@subQuery) > 0
							set @bubble_up_query += @subQuery + ' and '
						IF @SearchType = 'ProductView'
							set @bubble_up_query += ' psd.CodesheetID in ( ' + @ids + ' )'
						ELSE
							BEGIN
								IF LEN(@BrandID) > 0
									set @bubble_up_query += ' v.masterid in ( ' + @ids + ' )'
								ELSE
									set @bubble_up_query += ' sd.masterid in ( ' + @ids + ' )'
							END
					END
				set @added_master_id = 1
				FETCH NEXT FROM groups
				INTO @ids	
			END
		CLOSE groups
		DEALLOCATE groups
	END
	
	BEGIN ---- Activity Filters ----
	
		if LEN(@OpenDateFrom) > 0 AND LEN(@OpenDateTo) > 0
			set @openCondition += ' and ActivityDate between ''' + @OpenDateFrom + ''' and ''' + @OpenDateTo + ''''
		else if LEN(@OpenDateFrom) > 0
			set @openCondition += ' and ActivityDate >= ''' + @OpenDateFrom + ''''
		else if LEN(@OpenDateTo) > 0
			set @openCondition += ' and ActivityDate < ''' + @OpenDateTo + ''''
		
		if LEN(@OpenBlastID) > 0
			set @openCondition += ' and bl.BlastID in (' + @OpenBlastID + ')'
		
		If LEN(@OpenEmailSubject) > 0
			set @openCondition += ' and bl.EmailSubject LIKE ''%' + @OpenEmailSubject + '%'''
		
		if LEN(@OpenEmailFromDate) > 0 AND LEN(@OpenEmailToDate) > 0
			set @openCondition += ' and convert(date, bl.SendTime) between ''' + @OpenEmailFromDate + ''' and ''' + @OpenEmailToDate + ''''
		else if LEN(@OpenEmailFromDate) > 0
			set @openCondition += ' and convert(date, bl.SendTime) >= ''' + @OpenEmailFromDate + ''''
		else if LEN(@OpenEmailToDate) > 0
			set @openCondition += ' and convert(date, bl.SendTime) < ''' + @OpenEmailToDate + ''''
		
		if LEN(@ClickDateFrom) > 0 AND LEN(@ClickDateTo) > 0
			set @clickCondition += ' and ActivityDate between ''' + @ClickDateFrom + ''' and ''' + @ClickDateTo + ''''
		else if LEN(@ClickDateFrom) > 0
			set @clickCondition += ' and ActivityDate >= ''' + @ClickDateFrom + ''''
		else if LEN(@ClickDateTo) > 0
			set @clickCondition += ' and ActivityDate < ''' + @ClickDateTo + ''''
		
		if LEN(@ClickBlastID) > 0
			set @clickCondition += ' and bl.BlastID in ( ' + @ClickBlastID + ')'
		
		If LEN(@ClickEmailSubject) > 0
			set @clickCondition += ' and bl.EmailSubject LIKE ''%' + @ClickEmailSubject + '%'''
		
		if LEN(@ClickEmailFromDate) > 0 AND LEN(@ClickEmailToDate) > 0
			set @clickCondition += ' and convert(date, bl.SendTime) between ''' + @ClickEmailFromDate + ''' and ''' + @ClickEmailToDate + ''''
		else if LEN(@ClickEmailFromDate) > 0
			set @clickCondition += ' and convert(date, bl.SendTime) >= ''' + @ClickEmailFromDate + ''''
		else if LEN(@ClickEmailToDate) > 0
			set @clickCondition += ' and convert(date, bl.SendTime) < ''' + @ClickEmailToDate + ''''
		
		if LEN(@VisitsDateFrom) > 0 AND LEN(@VisitsDateTo) > 0
			set @visitCondition += ' and ActivityDate between ''' + @VisitsDateFrom + ''' and ''' + @VisitsDateTo + ''''
		else if LEN(@VisitsDateFrom) > 0
			set @visitCondition += ' and ActivityDate >= ''' + @VisitsDateFrom + ''''
		else if LEN(@VisitsDateTo) > 0
			set @visitCondition += ' and ActivityDate < ''' + @VisitsDateTo + ''''
		
		if LEN(@PubIDs) > 0
			BEGIN
				IF @OpenSearchType = 'Search All'
					set @openCondition += ' and ps.PubID in ( ' + @PubIDs + ')'
				IF @ClickSearchType = 'Search All'
					set @clickCondition += ' and ps.PubID in ( ' + @PubIDs + ')'
			END
			
		IF @OpenCount >= 0
			BEGIN
				IF @OpenSearchType = 'Search All'
					BEGIN
						IF @OpenCount = 0
							BEGIN
								IF LEN(@BrandID) > 0
									set @openQuery = ' s.subscriptionID not in (select s.SubscriptionID  from Subscriptions s   with (NOLOCK)'  +
														 'join SubscriberOpenActivity so  with (NOLOCK) on so.SubscriptionID = s.SubscriptionID '  +
														 'left join PubSubscriptions ps  with (NOLOCK) on so.PubSubscriptionID = ps.PubSubscriptionID'
								ELSE
									set @openQuery =  's.subscriptionID not in (select so.SubscriptionID  from ' +
																   ' SubscriberOpenActivity so  with (NOLOCK)'
							END
						ELSE IF @OpenCount > 0
							BEGIN
								IF LEN(@BrandID) > 0
									set @openQuery = 's.subscriptionID in (select s.SubscriptionID  from Subscriptions s   with (NOLOCK) ' +
														' join SubscriberOpenActivity so  with (NOLOCK) on so.SubscriptionID = s.SubscriptionID  ' +
														' left join PubSubscriptions ps  with (NOLOCK) on so.PubSubscriptionID = ps.PubSubscriptionID '; 
								ELSE
									set @openQuery = ' s.subscriptionID in (select so.SubscriptionID  from ' +
												   ' SubscriberOpenActivity so  with (NOLOCK) '
							END
					END
				ELSE
					BEGIN
						IF @OpenCount = 0
							set @openQuery =  ' s.subscriptionID not in (select ps.SubscriptionID  from PubSubscriptions ps   with (NOLOCK) ' +
													   ' join SubscriberOpenActivity so  with (NOLOCK) on so.PubSubscriptionID = ps.PubSubscriptionID '
						ELSE IF @OpenCount > 0
							set @openQuery = 's.subscriptionID in (select ps.SubscriptionID  from PubSubscriptions ps   with (NOLOCK) ' +
																			   ' join SubscriberOpenActivity so  with (NOLOCK) on so.PubSubscriptionID = ps.PubSubscriptionID '
					END
				IF LEN(@BrandID) > 0
					BEGIN
						IF @OpenSearchType = 'Search All'
							set @openCondition = @openCondition + ' and (pubID in (select PubID from BrandDetails bd  join Brand b on bd.BrandID = b.BrandID where bd.BrandID in' +
																	'(' + @BrandID + ') and  b.Isdeleted = 0) or ps.PubSubscriptionID is null)' 
						ELSE
							BEGIN
								set @openCondition = @openCondition + ' and bd.BrandID in (' + @BrandID + ') and b.Isdeleted = 0'
								set @openQuery = @openQuery + ' JOIN branddetails bd  with (NOLOCK) ON bd.pubID = ps.pubID join Brand b  with (NOLOCK) on b.BrandID = bd.BrandID'
							END
		
					END	
				IF LEN(@OpenEmailSubject) > 0 OR LEN(@OpenEmailFromDate)> 0 OR LEN(@OpenEmailToDate) > 0 OR LEN(@OpenBlastID) > 0
					set @openQuery = @openQuery + ' join Blast bl with (NOLOCK) on so.BlastID = bl.BlastID '
		
				if len(@openCondition) > 0
					Begin
						set @openCondition = substring(@openCondition, 5, len(@openCondition))
						set @openCondition = ' where ' + @openCondition
					End	
			
				set @openQuery += (CASE LEN(@openCondition) When 0 then '' else  @openCondition END)                          
			
				IF @OpenSearchType = 'Search All'
					BEGIN
						IF LEN(@BrandID) > 0
							BEGIN
								IF @OpenCount = 0
									set @openQuery = @openQuery + ' group by s.SubscriptionID'
								ELSE IF @OpenCount > 0
									set @openQuery = @openQuery + ' group by s.SubscriptionID  having COUNT(so.OpenActivityID) >= ' + @OpenCount
							END
						ELSE
							BEGIN
								IF @OpenCount = 0
									set @openQuery = @openQuery + ' group by so.SubscriptionID'
								ELSE IF @OpenCount > 0
									set @openQuery = @openQuery + ' group by so.SubscriptionID  having COUNT(so.OpenActivityID) >= ' + @OpenCount
							END
					END
				ELSE
					BEGIN
						IF @OpenCount = 0
							set @openQuery = @openQuery + ' group by ps.SubscriptionID'
						ELSE IF @OpenCount > 0
							set @openQuery = @openQuery + ' group by ps.SubscriptionID  having COUNT(so.OpenActivityID) >= ' + @OpenCount
					END
				set @openQuery = @openQuery + ')'
			END
		
		IF LEN(@ClickURL) > 0
			BEGIN
				DECLARE @url1 varchar(500)
				DECLARE @urlQuery1 varchar(8000) = ''
	
				DECLARE urls Cursor FOR
				Select * 
				FROM fn_Split(@ClickURL, ',')
		
				OPEN urls
		
				FETCH NEXT FROM urls
				INTO @url1
				WHILE @@FETCH_STATUS = 0
					BEGIN	
						set @urlQuery1 += (CASE LEN(@urlQuery1) when 0 then ' (Link like ''%' + @url1 + '%'' or LinkAlias like ''%' + @url1 + '%'')' else ' or (Link like ''%' + @url1 + '%'' or LinkAlias like ''%' + @url1 + '%'')'  END)
						FETCH NEXT FROM urls
						INTO @url1
					END
				CLOSE urls
				DEALLOCATE urls
		
				set @clickCondition += (CASE LEN(@clickCondition) when 0 then ' and ( ' + @urlQuery1 + ' )' else ' where ( ' + @urlQuery1 + ' )' END)
			END
		
		IF @ClickCount >= 0
			BEGIN
				IF @ClickSearchType = 'Search All'
					BEGIN
						IF @ClickCount = 0
							BEGIN
								IF LEN(@BrandID) > 0
									set @clickQuery = ' s.subscriptionID not in (select s.SubscriptionID  from Subscriptions s   with (NOLOCK)'  +
														 'join SubscriberClickActivity sc  with (NOLOCK) on sc.SubscriptionID = s.SubscriptionID '  +
														 'left join PubSubscriptions ps  with (NOLOCK) on sc.PubSubscriptionID = ps.PubSubscriptionID'
								ELSE
									set @clickQuery =  's.subscriptionID not in (select sc.SubscriptionID  from ' +
																   ' SubscriberClickActivity sc  with (NOLOCK)'
							END
						ELSE IF @ClickCount > 0
							BEGIN
								IF LEN(@BrandID) > 0
									set @clickQuery = 's.subscriptionID in (select s.SubscriptionID  from Subscriptions s   with (NOLOCK) ' +
														' join SubscriberClickActivity sc  with (NOLOCK) on sc.SubscriptionID = s.SubscriptionID  ' +
														' left join PubSubscriptions ps  with (NOLOCK) on sc.PubSubscriptionID = ps.PubSubscriptionID '; 
								ELSE
									set @clickQuery = ' s.subscriptionID in (select sc.SubscriptionID  from ' +
															' SubscriberClickActivity sc  with (NOLOCK) '
							END
					END
				ELSE
					BEGIN
							IF @ClickCount = 0
							set @clickQuery =  ' s.subscriptionID not in (select ps.SubscriptionID  from PubSubscriptions ps   with (NOLOCK) ' +
														' join SubscriberClickActivity sc  with (NOLOCK) on sc.PubSubscriptionID = ps.PubSubscriptionID '
						ELSE IF @ClickCount > 0
							set @clickQuery = 's.subscriptionID in (select ps.SubscriptionID  from PubSubscriptions ps   with (NOLOCK) ' +
																				' join SubscriberClickActivity sc  with (NOLOCK) on sc.PubSubscriptionID = ps.PubSubscriptionID '
					END
				IF LEN(@BrandID) > 0
					BEGIN
						IF @ClickSearchType = 'Search All'
							set @clickCondition = @clickCondition + ' and (pubID in (select PubID from BrandDetails bd  join Brand b on bd.BrandID = b.BrandID where bd.BrandID in' +
																	'(' + @BrandID + ') and  b.Isdeleted = 0) or ps.PubSubscriptionID is null)' 
						ELSE
							BEGIN
								set @clickCondition = @clickCondition + ' and bd.BrandID in (' + @BrandID + ') and b.Isdeleted = 0'
								set @clickQuery = @clickQuery + ' JOIN branddetails bd  with (NOLOCK) ON bd.pubID = ps.pubID join Brand b  with (NOLOCK) on b.BrandID = bd.BrandID'
							END
					END	
				IF LEN(@ClickEmailSubject) > 0 OR LEN(@ClickEmailFromDate)> 0 OR LEN(@ClickEmailToDate) > 0 OR LEN(@ClickBlastID) > 0
					set @clickQuery = @clickQuery + ' join Blast bl with (NOLOCK) on sc.BlastID = bl.BlastID '
			
				if len(@clickCondition) > 0
					Begin
						set @clickCondition = substring(@clickCondition, 5, len(@clickCondition))
						set @clickCondition = ' where ' + @clickCondition
					End		
			
				set @clickQuery += (CASE LEN(@clickCondition) When 0 then '' else  @clickCondition END)                                    
			
				IF @ClickSearchType = 'Search All'
					BEGIN
						IF LEN(@BrandID) > 0
							BEGIN
								IF @ClickCount = 0
									set @clickQuery = @clickQuery + ' group by s.SubscriptionID'
								ELSE IF @ClickCount > 0
									set @clickQuery = @clickQuery + ' group by s.SubscriptionID having COUNT(sc.ClickActivityID) >= ' + @ClickCount
							END
						ELSE
							BEGIN
								IF @ClickCount = 0
									set @clickQuery = @clickQuery + ' group by sc.SubscriptionID'
								ELSE IF @ClickCount > 0
									set @clickQuery = @clickQuery + ' group by sc.SubscriptionID having COUNT(sc.ClickActivityID) >= ' + @ClickCount
							END	
					END
				ELSE
					BEGIN
						IF @ClickCount = 0
							set @clickQuery = @clickQuery + ' group by ps.SubscriptionID'
						ELSE IF @ClickCount > 0
							set @clickQuery = @clickQuery + ' group by ps.SubscriptionID having COUNT(sc.ClickActivityID)  >= ' + @ClickCount
					END
				
				set @clickQuery = @clickQuery + ')'
			END
	
		IF LEN(@Domain) > 0
			set @visitCondition += (CASE LEN(@visitCondition) when 0 then ' where sv.DomainTrackingID = ' + @Domain else ' and sv.DomainTrackingID = ' + @Domain END)
	
		IF LEN(@VisitsURL) > 0
			BEGIN
				DECLARE @url2 varchar(500)
				DECLARE @urlQuery2 varchar(8000) = ''
	
				DECLARE urls Cursor FOR
				Select * FROM fn_Split(@VisitsURL, ',')
		
				OPEN urls
		
				FETCH NEXT FROM urls
				INTO @url2
		
				WHILE @@FETCH_STATUS = 0
					BEGIN	
						set @urlQuery2 += (CASE LEN(@urlQuery2) when 0 then ' sv.url like ''%' + @url2 + '%''' else ' or sv.url like ''%' + @url2 + '%'''  END)
				
						FETCH NEXT FROM urls
						INTO @url2
					END
				CLOSE urls
				DEALLOCATE urls
		
				set @visitCondition += (CASE LEN(@visitCondition) when 0 then ' and ( ' + @urlQuery2 + ' )' else ' where ( ' + @urlQuery2 + ' )' END)	
			END
		
		IF LEN(@visitCondition) > 0
			set @visitQuery = ' s.subscriptionID in (select s.SubscriptionID from Subscriptions s  with (NOLOCK) ' +
								' join SubscriberVisitActivity sv  with (NOLOCK) on sv.SubscriptionID = s.SubscriptionID ' + @visitCondition + ')'	
		
	END
	
	BEGIN --- Append Queries ----
	
		if len(@executeString) > 0
			Begin
				set @executeString = substring(@executeString, 5, len(@executeString))
				--set @selectClause = @selectClause + ' where '
			End			
		
		if LEN(@subQuery) > 0
			set @subQuery = substring(@subQuery, 5, len(@subQuery))	
		
		IF @added_master_id = 1
			BEGIN
				IF LEN(@executeString) > 0 AND LEN(@subQuery) > 0
					set @executeString += ' and '
			
				set @executeString += ' s.SubscriptionID in ( ' + @bubble_up_query + ' ) '
			END
		ELSE IF LEN(@subQuery) > 0
			BEGIN
				IF LEN(@executeString) > 0 AND LEN(@subQuery) > 0
					set @executeString += ' and '
				IF LEN(@BrandID) > 0
					set @executeString += ' s.SubscriptionID in ( SELECT sfilter.subscriptionid FROM subscriptions sfilter WITH (nolock) JOIN pubsubscriptions ps1 WITH (NOLOCK) ON ps1.subscriptionID = sfilter.subscriptionID ' + @AdHocCountryQuery + ' JOIN branddetails bd1 with (NOLOCK)ON bd1.pubID = ps1.pubID ' + @ScoreQuery + ' where ' + @subQuery + ' )'
				ELSE
					set @executeString += ' s.SubscriptionID in ( select sfilter.SubscriptionID from subscriptions sfilter  with (NOLOCK) JOIN pubsubscriptions ps1 WITH (NOLOCK) ON ps1.subscriptionID = sfilter.subscriptionID ' + @AdHocCountryQuery + ' where ' + @subQuery + ' )'
			END
	
		IF LEN(@openQuery) > 0
			set @executeString += (CASE LEN(@executeString) when 0 then ' ' else ' and ' END) + @openQuery
		IF LEN(@clickQuery) > 0
			set @executeString += (CASE LEN(@executeString) when 0 then ' ' else ' and ' END) + @clickQuery
		IF LEN(@visitQuery) > 0
			set @executeString += (CASE LEN(@executeString) when 0 then ' ' else ' and ' END) + @visitQuery
	
		--set @selectClause = @selectClause + @executeString
	
		set @selectClause += (CASE LEN(@executeString) when 0 then '' else ' where ' + @executeString END)	
	
	END
	
	DROP TABLE #AdHoc
	--print(@selectClause)
	exec(@selectClause)
End