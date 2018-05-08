CREATE proc [dbo].[rpt_CrossTab_Old]     
(      
	@ReportID int,  
	@PublicationID int,
	@CategoryIDs varchar(800),
	@CategoryCodes varchar(800),
	@TransactionIDs varchar(800),
	@TransactionCodes varchar(800),
	@QsourceIDs varchar(800),
	@StateIDs varchar(800),
	@Regions varchar(max),
	@CountryIDs varchar(1500),
	@Email varchar(10),
	@Phone varchar(10),
	@Mobile varchar(10),
	@Fax varchar(10),
	@ResponseIDs varchar(1000),
	@Demo7 varchar(50),		
	@Year varchar(20),
	@startDate varchar(10),		
	@endDate varchar(10),
	@AdHocXML varchar(8000), 
	@PrintColumns varchar(4000),   
	@Download bit,
	@WaveMail varchar(100) = ''     
)      
as     
	--Declare @ReportID int,  
	--@PublicationID int,
	--@CategoryIDs varchar(800),
	--@CategoryCodes varchar(800),
	--@TransactionIDs varchar(800),
	--@TransactionCodes varchar(800),
	--@QsourceIDs varchar(800),
	--@StateIDs varchar(800),
	--@Regions varchar(max),
	--@CountryIDs varchar(800),
	--@Email varchar(10),
	--@Phone varchar(10),
	--@Mobile varchar(10),
	--@Fax varchar(10),
	--@ResponseIDs varchar(800),
	--@Demo7 varchar(10),		
	--@Year varchar(20),
	--@startDate varchar(10),		
	--@endDate varchar(10),
	--@AdHocXML varchar(8000), 
	--@PrintColumns varchar(4000),   
	--@Download bit   


	--set @ReportID = 1520  
	--set @PublicationID = 1
	--set @CategoryIDs = ''
	--set @CategoryCodes = ''
	--set @TransactionIDs = ''
	--set @TransactionCodes = ''
	--set @QsourceIDs = ''
	--set @StateIDs = ''
	--set @Regions = ''
	--set @CountryIDs = ''
	--set @Email = ''
	--set @Phone = ''
	--set @Mobile = ''
	--set @Fax = ''
	--set @ResponseIDs = ''
	--set @Demo7 = ''	
	--set @Year = ''
	--set @startDate = ''
	--set @endDate = ''
	--set @AdHocXML = '<XML></XML>'
	--set @PrintColumns = ''
	--set @Download = 0
BEGIN
	
	SET NOCOUNT ON

	declare @Row varchar(50),  
			@Column varchar(50),  
			@ProductID int = @PublicationID 

	declare @ProductCode varchar(20)
	declare @GetSubscriberIDs bit = 0

	set ANSI_WARNINGS ON	

	create table #SubscriptionID (SubscriptionID int, copies int)  
	create table #responseID (responseID int)  
	
	insert into #responseID (responseID) VALUES(0)

	if len(ltrim(rtrim(@PrintColumns))) > 0 
		Begin
			set @PrintColumns  = ', ' + @PrintColumns 
		end

	select @Row = Row,  
		@Column = [Column]  
	from Reports  
	Where ReportID = @ReportID  
			
	DECLARE @TempTable table (value varchar(100))
	DECLARE @FinalTable table (rGroup varchar(100), value varchar(100))
	INSERT INTO @TempTable
	SELECT * 
	FROM fn_Split(@ResponseIDs, ',')
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
	DECLARE @regResponse varchar(1000)
	SELECT @regResponse = STUFF((SELECT ',' + [value] FROM @FinalTable WHERE rGroup <> 'ZZ' AND rGroup <> 'YY' for XML PATH('')), 1,1,'')
	
	DECLARE @RowID varchar(1000) = '', @ColID varchar(1000) = ''
	if (Len(ltrim(rtrim(@ResponseIDs)))) > 0
		BEGIN
			set @RowID = (Select substring((Select ',' + Cast(ST1.CodeSheetID as varchar(10))  AS [text()] From dbo.CodeSheet ST1 where PubID = @ProductID and ResponseGroup = @Row and CodeSheetID in (select Cast(items as int) from dbo.fn_Split(@regResponse,',')) ORDER BY ST1.CodeSheetID For XML PATH ('')), 2, 1000) [CodeSheet])
			set @ColID = (Select substring((Select ',' + Cast(ST1.CodeSheetID as varchar(10))  AS [text()] From dbo.CodeSheet ST1 where PubID = @ProductID and ResponseGroup = @Column and CodeSheetID in (select Cast(items as int) from dbo.fn_Split(@regResponse,','))
			ORDER BY ST1.CodeSheetID For XML PATH ('')), 2, 1000) [CodeSheet])
		END
	
	select @ProductCode = PubCode 
	from Pubs 
	where PubID = @ProductID		
  
	Insert into #SubscriptionID   
	exec rpt_GetSubscriptionIDs_Copies_From_Filter 
	@PublicationID, 
	@CategoryIDs,
	@CategoryCodes,
	@TransactionIDs,
	@TransactionCodes,
	@QsourceIDs,
	@StateIDs,
	@CountryIDs,
	@Email,
	@Phone,
	@Mobile,
	@Fax,
	@ResponseIDs,
	@Demo7,		
	@Year,
	@startDate,		
	@endDate,
	@AdHocXML

	CREATE UNIQUE CLUSTERED INDEX IX_1 on #SubscriptionID (SubscriptionID) 		
   
	if Len(ltrim(rtrim(@RowID))) > 0  		   
		insert into #responseID 
		select items 
		from dbo.fn_Split(@RowID,',')   
	else
		insert into #responseID 
		select CodeSheetID 
		from CodeSheet 
		where PubID = @ProductID and ResponseGroup = @Row
   
	if Len(ltrim(rtrim(@ColID))) > 0  
		insert into #responseID 
		select items 
		from dbo.fn_Split(@ColID,',')   
	else  		
		insert into #responseID 
		select CodeSheetID 
		from CodeSheet 
		where PubID = @ProductID and ResponseGroup = @Column  

	if @Download = 0
		Begin
	
			declare @subscriptions table (subscriptionID int, copies int, Row_ID int, Column_ID int)
			create table #Crosstab (Row_ID int , Row_Value varchar(500),Row_DESCRIPTION varchar(500), ROWGROUP_SORTORDER int, ROWGROUP_DisplayName varchar(50), Column_ID int, Column_Value varchar(500), Column_DESCRIPTION varchar(500), COLGROUP_SORTORDER int, COLGROUP_DisplayName varchar(50), Row_response_sortorder int, Col_response_sortorder int)  

			print '3. ' + convert(varchar(45), getdate(), 21) 

			insert into #Crosstab
			select	r1.CodeSheetID, r1.Responsevalue as Row_Value, r1.Responsedesc as Row_DESCRIPTION, RG1.DisplayOrder AS ROWGROUP_SORTORDER, RG1.DisplayName AS ROWGROUP_DisplayName,          
				r2.CodeSheetID, r2.Responsevalue as Column_Value, r2.Responsedesc as Column_DESCRIPTION, RG2.DisplayOrder AS COLGROUP_SORTORDER, RG2.DisplayName AS COLGROUP_DisplayName, 
				r1.DisplayOrder, r2.DisplayOrder 
			from CodeSheet r1 
				LEFT OUTER JOIN ReportGroups RG1 ON R1.ReportGroupID = RG1.ReportGroupID 
				cross join CodeSheet r2 
				LEFT OUTER JOIN ReportGroups RG2 ON R2.ReportGroupID = RG2.ReportGroupID
			where r1.PubID = @ProductID and  r1.ResponseGroup = @Row and r2.PubID = @ProductID and  r2.ResponseGroup = @Column 
		
			insert into #Crosstab
			select distinct 0, 'ZZ', 'No Response', NULL, NULL, r2.CodeSheetID, r2.Responsevalue as Column_Value, r2.Responsedesc as Column_DESCRIPTION, NULL, NULL,
				100, r2.DisplayOrder 
			from CodeSheet r1
				cross join CodeSheet r2
			where r2.PubID = @ProductID and  r2.ResponseGroup = @Column 
		
			insert into #Crosstab VALUES(0, 'ZZ', 'No Response', NULL, NULL, 0, 'ZZ', 'No Response', NULL, NULL, 100, 100)
		
			insert into #Crosstab
			select distinct r1.CodeSheetID, r1.Responsevalue as Column_Value, r1.Responsedesc, NULL, NULL, 0, 'ZZ' as Column_Value, 'No Response' as Column_DESCRIPTION, NULL, NULL,
				r1.DisplayOrder, 100 
			from CodeSheet r1
				cross join CodeSheet r2
			where r1.PubID = @ProductID and  r1.ResponseGroup = @Row 
		
			--select * from #Crosstab
			--order by Column_Value
		
			CREATE INDEX IDX_Crosstab_rowID_ColumnID ON #Crosstab(Row_ID,Column_ID )
		
			print '4. ' + convert(varchar(45), getdate(), 21) 

			--insert into @subscriptions 
			--select        
			--	sf.subscriptionID,      
			--	sf.copies,
			--	ISNULL(r.CodeSheetID, 0) Row_ID,      
			--	ISNULL(r2.CodeSheetID, 0) Column_ID      
			--FROM    
			--	PubSubscriptionDetail psd 
			--	JOIN CodeSheet r ON r.CodeSheetID = psd.CodesheetID AND r.IsActive = 1 and r.ResponseGroup = @Row
			--	JOIN CodeSheet r2 ON r2.CodeSheetID = psd.CodesheetID AND r2.IsActive = 1 and r2.ResponseGroup = @Column
			--	RIGHT JOIN #SubscriptionID sf ON psd.SubscriptionID = sf.SubscriptionID    
			--	--#SubscriptionID sf with (NOLOCK) left join
			--	----SubscriptionResponseMap sd with (NOLOCK) on sd.SubscriptionID = sf.SubscriptionID join
			--	--PubSubscriptionDetail sd with (NoLock) on sd.SubscriptionID = sf.SubscriptionID join
			--	--CodeSheet r with (NOLOCK) on sd.CodeSheetID = r.CodeSheetID JOIN 
			--	--#responseID rd on rd.responseID = r.CodeSheetID
			--GROUP BY sf.SUBSCRIPTIONid,sf.copies, r.CodeSheetID, r2.CodeSheetID
		
			insert into @subscriptions
			exec ('SELECT q1.SubscriptionID, q1.Copies, q1.Row_ID, q2.Column_ID
			FROM
			(SELECT ps.SubscriptionID, ps.Copies, ISNULL(cs.CodeSheetID, 0) as ''Row_ID''
			FROM PubSubscriptionDetail psd 
			JOIN CodeSheet cs ON cs.CodeSheetID = psd.CodesheetID AND cs.IsActive = 1
			JOIN ResponseGroups rg ON rg.ResponseGroupID = cs.ResponseGroupID AND rg.ResponseGroupName = ''' + @Row + '''
			RIGHT JOIN PubSubscriptions ps ON psd.PubSubscriptionID = ps.PubSubscriptionID
			JOIN #SubscriptionID sf ON sf.SubscriptionID = ps.SubscriptionID
			WHERE ps.PubID = ' + @PublicationID + '
			) q1
			INNER JOIN
			(SELECT ps.SubscriptionID, ps.Copies, ISNULL(cs.CodeSheetID, 0) as ''Column_ID''
			FROM PubSubscriptionDetail psd
			JOIN CodeSheet cs ON cs.CodeSheetID = psd.CodesheetID AND cs.IsActive = 1
			JOIN ResponseGroups rg ON rg.ResponseGroupID = cs.ResponseGroupID AND rg.ResponseGroupName = ''' + @Column + '''
			RIGHT JOIN PubSubscriptions ps ON psd.PubSubscriptionID = ps.PubSubscriptionID
			JOIN #SubscriptionID sf ON sf.SubscriptionID = ps.SubscriptionID
			WHERE ps.PubID = ' + @PublicationID + '
			) q2
			ON q1.SubscriptionID = q2.SubscriptionID')
		
			--select * FROM @subscriptions
			--order by Column_ID

			create table #ctr_table (Row_Value varchar(10),
				Row_DESCRIPTION varchar(500),
				ROWGROUP_SORTORDER int,
				ROWGROUP_DisplayName varchar(500),
				Row_response_sortorder int,          
				Column_Value varchar(500),
				Column_DESCRIPTION varchar(2000),
				COLGROUP_SORTORDER int,
				COLGROUP_DisplayName varchar(500), 
				Col_response_sortorder int,         
				counts int)

			declare @Rowtotalcounts table (Row_ID int, rowTotals int);	

			WITH CTR 
			AS
			(	
			select c.Row_ID,
				Row_Value,
				Row_DESCRIPTION,
				ROWGROUP_SORTORDER,
				ROWGROUP_DisplayName,
				Row_response_sortorder, 
				c.Column_ID,         
				Column_Value,
				Column_DESCRIPTION,
				COLGROUP_SORTORDER,
				COLGROUP_DisplayName, 
				Col_response_sortorder,         
				isnull(sum(copies),0) as counts   
			FROM #crosstab c 
				inner join #responseID r1 on r1.responseID = c.Row_ID 
				inner join #responseID r2 on r2.responseID = c.Column_ID
				LEFT outer join @subscriptions t on t.Row_ID = c.Row_ID and t.Column_ID = c.Column_ID
			group by c.Row_ID,
				Row_Value,
				Row_DESCRIPTION,  
				c.Column_ID,               
				Column_Value,
				Column_DESCRIPTION,
				ROWGROUP_SORTORDER,
				ROWGROUP_DisplayName,
				Row_response_sortorder,
				COLGROUP_SORTORDER,
				COLGROUP_DisplayName,
				Col_response_sortorder
			)
			insert into #ctr_table(Row_Value,Row_DESCRIPTION,ROWGROUP_SORTORDER,ROWGROUP_DisplayName,Row_response_sortorder,Column_Value,Column_DESCRIPTION,COLGROUP_SORTORDER,COLGROUP_DisplayName,Col_response_sortorder,counts)
			select Row_Value,
				Row_Value + '. ' + Row_DESCRIPTION,
				ROWGROUP_SORTORDER,
				ROWGROUP_DisplayName,
				Row_response_sortorder,          
				Column_Value,
				Column_DESCRIPTION,
				COLGROUP_SORTORDER,
				COLGROUP_DisplayName, 
				Col_response_sortorder,         
				counts  
			from CTR

			DECLARE @row_name varchar(500)
			DECLARE @col_name varchar(500)
			DECLARE @INSERTSQL varchar(max)
			DECLARE @FinalTotal int = 0
			DECLARE @FirstRun bit = 'true'
			DECLARE db_row CURSOR FOR  
			Select Row_DESCRIPTION FROM
			(SELECT distinct Row_Value, Row_DESCRIPTION, ISNULL(Row_response_sortorder,0) as Row_response_sortorder
			FROM #ctr_table) as ROWVALUE order by Row_response_sortorder

			--select * FROM #ctr_table
			--order by Column_Value
		
			OPEN db_row  
			FETCH NEXT FROM db_row INTO @row_name  

			WHILE @@FETCH_STATUS = 0  
				BEGIN  
					if (@FirstRun = 'false')
						BEGIN
							set @INSERTSQL = @INSERTSQL + ' union select '
						END
					else
						BEGIN
							set @FirstRun = 'false'
							set @INSERTSQL = ' Select '
						END

					--Select @row_name  
					DECLARE db_col CURSOR FOR  
					Select Column_Value 
					FROM
					(
						Select @Row as Column_Value, -1 as Col_response_sortorder
						union
						Select 'Total' as Column_Value, -1 as Col_response_sortorder
						union
						Select Column_Value, ISNULL(Col_response_sortorder,0) FROM 					
							(SELECT distinct Column_Value, Column_DESCRIPTION, Col_response_sortorder
							FROM #ctr_table) as COLUMNVALUE
					) AS COL order by Col_response_sortorder, Column_Value
			
					OPEN db_col  
					FETCH NEXT FROM db_col INTO @col_name  

					WHILE @@FETCH_STATUS = 0  
						BEGIN  
							if (@col_name = @Row)
								BEGIN
									set @INSERTSQL = @INSERTSQL + '''' + LTRIM(RTRIM(REPLACE(@row_name, '''', ''))) + ''' as [' + @Row + ']'
								END
							else if (@col_name = 'Total')
								BEGIN
									set @INSERTSQL = @INSERTSQL + ',' + Cast((Select Sum(counts) FROM #ctr_table where Row_DESCRIPTION = @row_name) as varchar(50)) + ' as ' + '''' + @col_name + ''''
									set @FinalTotal = @FinalTotal + (Select Sum(counts) FROM #ctr_table where Row_DESCRIPTION = @row_name)
								END
							else
								BEGIN					
									set @INSERTSQL = @INSERTSQL + ',' + '''' + ISNULL(Cast((Select TOP 1 counts FROM #ctr_table where Row_DESCRIPTION = @row_name and Column_Value = @col_name) as varchar(50)), '0') + '''' + ' as ' + '''' + LTRIM(RTRIM(@col_name)) + ''''
								END

							FETCH NEXT FROM db_col INTO @col_name  
						END  

					CLOSE db_col  
					DEALLOCATE db_col 

					FETCH NEXT FROM db_row INTO @row_name  
				END  
			DECLARE db_col2 CURSOR FOR  
			Select Column_Value 
			FROM
			(
				Select @Row as Column_Value, -1 as Col_response_sortorder
				union
				Select 'Total' as Column_Value, -1 as Col_response_sortorder
				union
				Select Column_Value, Col_response_sortorder 
				FROM 					
					(SELECT distinct Column_Value, Column_DESCRIPTION, ISNULL(Col_response_sortorder,0) as Col_response_sortorder
					FROM #ctr_table) as COLUMNVALUE
			) AS COL order by Col_response_sortorder, Column_Value
			
			OPEN db_col2  
			FETCH NEXT FROM db_col2 INTO @col_name  
			WHILE @@FETCH_STATUS = 0  
				BEGIN  
					if (@col_name = @Row)
						BEGIN
							set @INSERTSQL = @INSERTSQL + ' union select ' + '''BUSINESS TOTAL'' as [' + @Row + ']'
						END
					else if (@col_name = 'Total')
						BEGIN
							set @INSERTSQL = @INSERTSQL + ',' + Cast(@FinalTotal as varchar(50)) + ' as ' + '''' + @col_name + ''''
						END
					else
						BEGIN
							set @INSERTSQL = @INSERTSQL + ',' + '''' + Cast((Select Sum(counts) from #ctr_table where Column_Value = @col_name) as varchar(50)) + '''' + ' as ' + '''' + LTRIM(RTRIM(@col_name)) + ''''
						END

					FETCH NEXT FROM db_col2 INTO @col_name  
				END  

			CLOSE db_col2  
			DEALLOCATE db_col2 

			CLOSE db_row  
			DEALLOCATE db_row 		

			set @INSERTSQL = ' Select * from ( ' +@INSERTSQL + ') as A order by CASE A.[' + @Row + '] WHEN ' + '''BUSINESS TOTAL''' + ' THEN 1 ELSE 0 END, [' + @Row + ']'
			--print(@INSERTSQL)
			EXEC(@INSERTSQL)
		
			drop table #Crosstab

		end
	else
		Begin
			
			exec ('select  distinct  ''' + @ProductCode + ''' as PubCode, s.SubscriberID, s.EMAIL, s.FNAME, s.LNAME ' +
				', s.COMPANY, s.TITLE, s.ADDRESS, s.MAILSTOP, s.CITY, s.STATE, '' '' + convert(varchar,s.ZIP) as zip, s.PLUS4, s.COUNTRY, s.PHONE, s.MOBILE, s.FAX, ps.website, 
				s.CategoryID as CAT, C.CategoryCodeName as CategoryName, s.TransactionID as XACT, s.TransactionDate as XACTDate, s.FORZIP, s.COUNTY, s.QSourceID, q.DisplayName + '' (''+ q.CodeValue + '')'' as Qsource,s.QDate,s.Subsrc, ps.copies  ' + 
				@PrintColumns + 
			' FROM ' + 
			'(' + 
				'select ' + 
					's.subscriptionID, ' + 
					'max(case when ResponseGroup = ''' + @Row + ''' THEN R.CodeSheetID END) Row_ID, ' + 
					'max(case when ResponseGroup = ''' + @Column  + ''' THEN R.CodeSheetID END) Column_ID ' + 
				'FROM                    ' + 
					'#SubscriptionID s join         ' + 				
					'PubSubscriptionDetail sd with (NoLock) on sd.SubscriptionID = s.SubscriptionID join ' +
					'CodeSheetID r on sd.CodeSheetID = r.CodeSheetID  ' + 
				'WHERE          ' + 
				  'r.CodeSheetID in (select responseID from #responseID)    ' + 
				'GROUP BY S.subscriptionID       ' + 
			') Inn join   ' + 
			'Subscriptions s on s.SubscriptionID = inn.SubscriptionID left outer join UAD_Lookup..Code q on q.CodeID = s.QSourceID join  ' + 
			'Pubs pub on pub.PubCode = ' + @ProductCode + ' join ' +
			'PubSubscriptions ps on ps.SubscriptionID = inn.SubscriptionID and ps.PubID = pub.PubID join ' +
			'UAD_Lookup..CategoryCode C on s.CategoryID = C.CategoryCodeValue join ' +
			'CodeSheet r1 on inn.Row_ID = r1.CodeSheetID join ' +       
			'CodeSheet r2 on inn.Column_ID = r2.CodeSheetID ') 
		end
	drop table #SubscriptionID
	drop table #responseID 
	drop table #ctr_table
	
	set ANSI_WARNINGS ON
	set NOCOUNT off

End