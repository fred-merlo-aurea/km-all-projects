CREATE Proc [dbo].[sp_getSubscribers_using_XMLFilters]
(
	@PublicationID int, 
	@FilterString varchar(8000),
	@IsDownload bit
)
as
Begin

--declare @PublicationID int, 
--		@FilterString varchar(8000),
--		@IsDownload bit

--set @PublicationID = 50
--set  @FilterString = '<Filters><FilterType ID="P"></FilterType><FilterType ID="D"></FilterType><FilterType ID="M"><FilterGroup Type="CATEGORY"><Value>2</Value><Value>1</Value></FilterGroup><FilterGroup Type="CATCODES"><Value>1</Value><Value>2</Value><Value>3</Value><Value>15</Value><Value>16</Value></FilterGroup><FilterGroup Type="TRANSACTION"><Value>1</Value></FilterGroup></FilterType><FilterType ID="C"></FilterType><FilterType ID="A"><FilterGroup Field="[CITY]" DataType="varchar" Condition="Contains"><Value>chicago</Value></FilterGroup></FilterType></Filters>'
--set @IsDownload = 1

	DECLARE @CategoryIDs varchar(800),
			@CategoryCodes varchar(800),
			@TransactionIDs varchar(800),
			@TransactionCodes varchar(800),
			@QsourceIDs varchar(800),
			@StateIDs varchar(800),
			@Email varchar(800),
			@Phone varchar(800),
			@Fax varchar(800),
			@Demo7 varchar(10),		
			@Demo31 varchar(1),		
			@Demo32 varchar(1),		
			@Demo33 varchar(1),		
			@Demo34 varchar(1),
			@Demo35 varchar(1),
			@Demo36 varchar(1),
			@Qfrom varchar(10),
			@QTo varchar(10),
			@Year varchar(4),
			@executeString varchar(8000),
			@startDate varchar(10),		
			@endDate varchar(10),
			@subsrc varchar(50),
			@AdhocColumn varchar(100),
			@AdhocValue varchar(100),
			@AdhocString varchar(1000),
			@Adhoc varchar(1000),
			@currentYear int,
			@docHandle int,
			@demoFilter varchar(2000),
			@AddKillCatIDs varchar(800),
			@AddKillXactIDs varchar(800)
			
	declare @dimensions table (dimension varchar(100), codes varchar(2000))
	
	set nocount on
	
	set	@CategoryIDs = ''
	set	@TransactionIDs = ''
	set @TransactionCodes = ''
	set	@QsourceIDs = ''
	set	@StateIDs = ''
	set	@Email = ''
	set	@Phone = ''
	set	@Fax = ''
	set	@Demo7 = ''
	set	@Demo31 = ''
	set	@Demo32 = ''
	set	@Demo33 = ''
	set	@Demo34 = ''
	set	@Demo35 = ''
	set	@Demo36 = ''
	set	@Qfrom = ''
	set	@QTo = ''
	set @CategoryCodes	= ''
	set @year = ''
	set @subsrc = ''
	set @AdhocColumn = ''
	set @AdhocValue = ''
	set @startDate= ''		
	set @endDate=''	
	set	@AddKillCatIDs = ''
	set	@AddKillXactIDs = ''
			
	if len(@FilterString) = 0
	Begin
        if (@IsDownload = 0)
		begin
			Select	count(s.SubscriptionID) as count
			from	Subscription s join
					Subscriber sb on s.SubscriberID = sb.subscriberID join
					Action a on a.ActionID = s.ActionID_Current join
					CategoryCode cc on cc.CategoryCodeID  = a.CategoryCodeID join
					TransactionCode tc on tc.TransactionCodeID = a.TransactionCodeID  left outer join 
					QualificationSource qs  on qs.QSourceID = s.QSourceID
			Where  		s.PublicationID = @PublicationID 
		end
		else
		begin
			Select	s.SubscriptionID, s.copies 
			from	Subscription s join
					Subscriber sb on s.SubscriberID = sb.subscriberID join
					Action a on a.ActionID = s.ActionID_Current join
					CategoryCode cc on cc.CategoryCodeID  = a.CategoryCodeID join
					TransactionCode tc on tc.TransactionCodeID = a.TransactionCodeID  left outer join 
					QualificationSource qs  on qs.QSourceID = s.QSourceID
			Where  		s.PublicationID = @PublicationID 	
		end
	End
	Else
	Begin
		
		EXEC sp_xml_preparedocument @docHandle OUTPUT, @FilterString  
	
		create table #State	(RegionCode varchar(256))
		create table #country (CountryID INT)

        if (@IsDownload = 0)
			begin
			set @executeString =' Select	count(s.SubscriptionID) as count ' +
								' from	Subscription s join ' +
										' Subscriber sb on s.SubscriberID = sb.subscriberID join ' +
										' Action a on a.ActionID = s.ActionID_Current join ' +
										' CategoryCode cc on cc.CategoryCodeID  = a.CategoryCodeID join ' +
										' TransactionCode tc on tc.TransactionCodeID = a.TransactionCodeID  left outer join  ' +
										' QualificationSource qs  on qs.QSourceID = s.QSourceID left outer join ' +
										' dbo.Deliverability db  on db.DeliverabilityID = s.DeliverabilityID left outer join ' +
										'(
											SELECT SubscriberID, ISNULL(ML, 0) as Demo31, ISNULL(FX, 0) as Demo32, ISNULL(PH, 0) as Demo33, ISNULL(TS, 0) as Demo34, ISNULL(ADV, 0) as Demo35, ISNULL(EM, 0) as Demo36
											FROM
											(SELECT SubscriberID,MarketingCode, CAST(mm.IsActive AS TINYINT) as IsActive
												FROM MarketingMap mm join Marketing m on mm.MarketingID = m.MarketingID where PublicationID = ' + Convert(varchar,@PublicationID) + ' ) AS SourceTable
											PIVOT
											(
											max(IsActive)
											FOR MarketingCode IN (ML, FX, PH, TS, ADV, EM)
											) AS PivotTable
										) pv on s.subscriberID = pv.subscriberID  ' +
								' Where  s.PublicationID = ' + Convert(varchar,@PublicationID) 
			end
		else
		begin
			set @executeString =' Select distinct s.SubscriptionID, s.copies ' +
								' from	Subscription s join ' +
										' Subscriber sb on s.SubscriberID = sb.subscriberID join ' +
										' Action a on a.ActionID = s.ActionID_Current join ' +
										' CategoryCode cc on cc.CategoryCodeID  = a.CategoryCodeID join ' +
										' TransactionCode tc on tc.TransactionCodeID = a.TransactionCodeID  left outer join  ' +
										' SubscriptionPaid sp on sp.SubscriptionID = s.SubscriptionID left outer join' +
										' dbo.Deliverability db  on db.DeliverabilityID = s.DeliverabilityID left outer join ' +
										'(
											SELECT SubscriberID, ISNULL(ML, 0) as Demo31, ISNULL(FX, 0) as Demo32, ISNULL(PH, 0) as Demo33, ISNULL(TS, 0) as Demo34, ISNULL(ADV, 0) as Demo35, ISNULL(EM, 0) as Demo36
											FROM
											(SELECT SubscriberID,MarketingCode, CAST(mm.IsActive AS TINYINT) as IsActive
												FROM MarketingMap mm join Marketing m on mm.MarketingID = m.MarketingID where PublicationID = ' + Convert(varchar,@PublicationID) + ' ) AS SourceTable
											PIVOT
											(
											max(IsActive)
											FOR MarketingCode IN (ML, FX, PH, TS, ADV, EM)
											) AS PivotTable
										) pv on s.subscriberID = pv.subscriberID  ' +
								' Where ' +	
										' s.PublicationID = ' + Convert(varchar,@PublicationID) 			
			end
			
			
		select  @CategoryIDs = @CategoryIDs + coalesce(value + ',','')
		FROM OPENXML(@docHandle, N'/Filters/FilterType[@ID="M"]/FilterGroup[@Type="CATEGORY"]/Value')   
				WITH (value varchar(800) '.') 

		select  @CategoryCodes = @CategoryCodes + coalesce(value + ',','')
		FROM OPENXML(@docHandle, N'/Filters/FilterType[@ID="M"]/FilterGroup[@Type="CATCODES"]/Value')   
				WITH (value varchar(800) '.') 			

		select  @TransactionIDs = @TransactionIDs + coalesce(value + ',','')
		FROM OPENXML(@docHandle, N'/Filters/FilterType[@ID="M"]/FilterGroup[@Type="TRANSACTION"]/Value')   
				WITH (value varchar(800) '.')
				
		select  @TransactionCodes = @TransactionCodes + coalesce(value + ',','')
		FROM OPENXML(@docHandle, N'/Filters/FilterType[@ID="M"]/FilterGroup[@Type="TRANSACTIONCODE"]/Value')   
				WITH (value varchar(800) '.')	
				
		select  @QsourceIDs = @QsourceIDs + coalesce(value + ',','')
		FROM OPENXML(@docHandle, N'/Filters/FilterType[@ID="M"]/FilterGroup[@Type="QUALSOURCE"]/Value')   
				WITH (value varchar(800) '.')	

		Insert into #State
		select value
		FROM OPENXML(@docHandle, N'/Filters/FilterType[@ID="M"]/FilterGroup[@Type="STATE"]/Value')   
				WITH (value varchar(800) '.')										

		Insert into #country
		select value
		FROM OPENXML(@docHandle, N'/Filters/FilterType[@ID="M"]/FilterGroup[@Type="COUNTRY"]/Value')   
				WITH (value varchar(800) '.')

		select  @Email = value
		FROM OPENXML(@docHandle, N'/Filters/FilterType[@ID="C"]/FilterGroup[@Type="EMAIL"]/Value')   
				WITH (value varchar(800) '.')
				
		select  @Phone = value 
		FROM OPENXML(@docHandle, N'/Filters/FilterType[@ID="C"]/FilterGroup[@Type="PHONE"]/Value')   
				WITH (value varchar(800) '.')
				
		select  @Fax = value 
		FROM OPENXML(@docHandle, N'/Filters/FilterType[@ID="C"]/FilterGroup[@Type="FAX"]/Value')   
				WITH (value varchar(800) '.')
				
		select  @Demo7 = @Demo7 + coalesce(value + ',','')
		FROM OPENXML(@docHandle, N'/Filters/FilterType[@ID="C"]/FilterGroup[@Type="DEMO7"]/Value')   
				WITH (value varchar(800) '.')
				
		select  @Demo31 =  value 
		FROM OPENXML(@docHandle, N'/Filters/FilterType[@ID="P"]/FilterGroup[@Type="DEMO31"]/Value')   
				WITH (value varchar(800) '.')	
				
		select  @Demo32 =  value 
		FROM OPENXML(@docHandle, N'/Filters/FilterType[@ID="P"]/FilterGroup[@Type="DEMO32"]/Value')   
				WITH (value varchar(800) '.')
				
		select  @Demo33 =  value 
		FROM OPENXML(@docHandle, N'/Filters/FilterType[@ID="P"]/FilterGroup[@Type="DEMO33"]/Value')   
				WITH (value varchar(800) '.')
				
		select  @Demo34 =  value 
		FROM OPENXML(@docHandle, N'/Filters/FilterType[@ID="P"]/FilterGroup[@Type="DEMO34"]/Value')   
				WITH (value varchar(800) '.')
				
		select  @Demo35 =  value 
		FROM OPENXML(@docHandle, N'/Filters/FilterType[@ID="P"]/FilterGroup[@Type="DEMO35"]/Value')   
				WITH (value varchar(800) '.')
				
		select  @Demo36 = value 
		FROM OPENXML(@docHandle, N'/Filters/FilterType[@ID="P"]/FilterGroup[@Type="DEMO36"]/Value')   
				WITH (value varchar(800) '.')	
				
		select  @Qfrom =  value 
		FROM OPENXML(@docHandle, N'/Filters/FilterType[@ID="P"]/FilterGroup[@Type="QFrom"]/Value')   
				WITH (value varchar(800) '.')
								
		select  @QTo =  value 
		FROM OPENXML(@docHandle, N'/Filters/FilterType[@ID="P"]/FilterGroup[@Type="QTo"]/Value')   
				WITH (value varchar(800) '.')	
				
		select  @Year = Value
		FROM OPENXML(@docHandle, N'/Filters/FilterType[@ID="P"]/FilterGroup[@Type="YEAR"]/Value')   
				WITH (value varchar(800) '.')
				
		select  @AddKillCatIDs = @AddKillCatIDs + coalesce(value + ',','')
		FROM OPENXML(@docHandle, N'/Filters/FilterType[@ID="M"]/FilterGroup[@Type="ADDKILLCATID"]/Value')   
				WITH (value varchar(800) '.') 
				
		select  @AddKillXactIDs = @AddKillXactIDs + coalesce(value + ',','')
		FROM OPENXML(@docHandle, N'/Filters/FilterType[@ID="M"]/FilterGroup[@Type="ADDKILLXACTID"]/Value')   
				WITH (value varchar(800) '.') 				
				
		-- Adhoc XML = Type="[ADDRESS]" DataType="varchar" Condition="Contains"
		create  table #Adhoc  (RowID int IDENTITY(1, 1), AdhocColumn varchar(100), AdhocValue varchar(100), AdhocValueFrom varchar(100), AdhocValueTo varchar(100), DataType varchar(100), Condition varchar(100))
		insert into #Adhoc
					select  col, value, valuefrom, valueTo, DataType, Condition
				FROM OPENXML(@docHandle, N'/Filters/FilterType[@ID="A"]/FilterGroup')   
						WITH (	col varchar(100) '@Field', 
								value varchar(50) './Value',
								valuefrom varchar(50) './From',
								valueTo varchar(50) './To',
								DataType varchar(50) '@DataType', 
								Condition varchar(50) '@Condition'
				
							);	
		
		WITH cte1 (dimension, codes)
		AS
		-- Define the CTE query.
		(
			select  col, value
				FROM OPENXML(@docHandle, N'/Filters/FilterType[@ID="D"]/FilterGroup/Value')   
						WITH (col varchar(800) '../@Type', value varchar(800) '.')		
		)
		-- Define the outer query referencing the CTE name.
		insert into @dimensions
		select
			dimension,
			' and s.SubscriptionID in (select SubscriptionID from dbo.SubscriptionResponseMap with (NOLOCK) where ResponseID in (' + 
			stuff((
				select ',' + t.[codes]
				from cte1 t
				where t.dimension = t1.dimension
				order by t.[codes]
				for xml path('')
			),1,1,'') + '))' as name_csv
		from cte1 t1
		group by dimension
		; 
		
		EXEC sp_xml_removedocument @docHandle    
						
		if len(@CategoryIDs) > 0
			set @executeString = @executeString + ' and cc.CategoryCodeTypeID in (' + left(@CategoryIDs, LEN(@CategoryIDs)-1) + ')'  

		if len(@CategoryCodes) > 0
			set @executeString = @executeString + ' and cc.CategoryCodeID in (' + left(@CategoryCodes, LEN(@CategoryCodes)-1) + ')'  
	 
		if len(@TransactionIDs) > 0
			set @executeString = @executeString + ' and tc.TransactionCodeTypeID in (' + left(@TransactionIDs, LEN(@TransactionIDs)-1) + ')'
		
		if len(@TransactionCodes) > 0
			set @executeString = @executeString + ' and tc.TransactionCodeID in (' + left(@TransactionCodes, LEN(@TransactionCodes)-1)+ ')'  

		if len(@QsourceIDs) > 0
			set @executeString = @executeString + ' and s.QSourceID in (' + left(@QsourceIDs, LEN(@QsourceIDs)-1) + ')' 

		if exists (select top 1 * from #State) 
		Begin
			set @executeString = @executeString + ' and RegionID in (select r.RegionID from UAS..Region r join #State s on s.RegionCode = r.RegionCode)'
		end

		if exists (select top 1 * from #country) 
		Begin

			--(1, Only US 
			--(2, Only Canada 
			--(3, US and Canada 
			--(4, International 

			if exists (select top 1 * from #country where countryID = 4)
				insert into #country
				select CountryID from UAS..Country where CountryID >= 101
			
			if exists (select top 1 * from #country)
				set @executeString = @executeString + ' and isnull(sb.CountryID,1) in (select CountryID from #country)'

		end

		if len(@Email) > 0
		Begin
			if @Email = 1
				set @executeString = @executeString + ' and Isnull(ltrim(rtrim(sb.Email)),'''') <> ''''' 
			else
				set @executeString = @executeString + ' and Isnull(ltrim(rtrim(sb.Email)),'''') = ''''' 
		End

		if len(@Phone) > 0
		Begin
			if @Phone = 1
				set @executeString = @executeString + ' and Isnull(ltrim(rtrim(sb.PHONE)),'''') <> ''''' 
			else
				set @executeString = @executeString + ' and Isnull(ltrim(rtrim(sb.PHONE)),'''') = ''''' 
		End

		if len(@Fax) > 0
		Begin
			if @Fax = 1
				set @executeString = @executeString + ' and Isnull(ltrim(rtrim(sb.Fax)),'''') <> ''''' 
			else
				set @executeString = @executeString + ' and Isnull(ltrim(rtrim(sb.Fax)),'''') = ''''' 
		End

		if len(@Demo7) > 0
			set @executeString = @executeString + ' and Isnull(db.DeliverabilityCode,'''') in (''' + replace(@Demo7,',',''',''') + ''')'

		if len(@Demo31) > 0
			set @executeString = @executeString + ' and Isnull(Demo31,1) = ' + @Demo31

		if len(@Demo32) > 0
			set @executeString = @executeString + ' and Isnull(Demo32,1) = ' + @Demo32

		if len(@Demo33) > 0
			set @executeString = @executeString + ' and Isnull(Demo33,1) = ' + @Demo33

		if len(@Demo34) > 0
			set @executeString = @executeString + ' and Isnull(Demo34,1) = ' + @Demo34

		if len(@Demo35) > 0
			set @executeString = @executeString + ' and Isnull(Demo35,1) = ' + @Demo35

		if len(@Demo36) > 0
			set @executeString = @executeString + ' and Isnull(Demo36,1) = ' + @Demo36

		if len(@QFrom) > 0
			set @executeString = @executeString + ' and QSourceDate >= ''' + @QFrom + ''''

		if len(@QTo) > 0
			set @executeString = @executeString + ' and QSourceDate <= ''' + @QTo + ''''

		if len(@Year) > 0
		Begin

			select @startDate = YearStartDate , @endDate = YearEndDate from Publication where PublicationID = @PublicationID		

			if getdate() > convert(datetime,@startDate + '/' + convert(varchar,year(getdate())))
				set @currentYear = year(getdate()) 
			else
				set @currentYear = year(getdate()) - 1
				
			set @executeString = @executeString + ' and QSourceDate >= ''' + @startDate + '/' + Convert(varchar(10),(convert(int,@currentYear-@Year)))  + ''''
			set @executeString = @executeString + ' and QSourceDate <= ''' + @endDate + '/' +  Convert(varchar(10),(convert(int,@currentYear-@Year+1))) + '''' ---convert(int,@Year+1)
		End
		
		if len(@AddKillCatIDs) > 0
			set @executeString = @executeString + ' and s.AddKillCatID in (' + left(@AddKillCatIDs, LEN(@AddKillCatIDs)-1) + ')'  
	 
		if len(@AddKillXactIDs) > 0
			set @executeString = @executeString + ' and s.AddKillXactID in (' + left(@AddKillCatIDs, LEN(@AddKillCatIDs)-1)+ ')'  
	
		 declare @Column varchar(100),
				@Value varchar(100), 
				@ValueFrom varchar(100), 
				@ValueTo varchar(100), 
				@DataType varchar(100), 
				@Condition varchar(100)
		 
		-- Adhloc
		DECLARE @NumberRecords int, @RowCount int
		set @NumberRecords = 0
		set @Adhoc = ''
	
		-- Get the number of records in the temporary table
		SELECT @NumberRecords = COUNT(*) from #Adhoc
		SET @RowCount = 1
		WHILE @RowCount <= @NumberRecords
		BEGIN
			 set @AdhocString = '';
			 SELECT @Column = AdhocColumn,
					@Value = AdhocValue,
					@ValueFrom = AdhocValueFrom,
					@ValueTo = AdhocValueTo,
					@DataType = DataType,
					@Condition = Condition
			FROM #Adhoc
			WHERE RowID = @RowCount
			
			 if(@DataType = 'varchar') 
			 begin
				while(PATINDEX ('% ,%',@Value)>0 or PATINDEX ('%, %',@Value)>0 )
					set @Value = LTRIM(RTRIM(REPLACE(replace(@Value,' ,',','),', ',',')))
			 
				set @AdhocString =  
					CASE  @Condition
						WHEN 'Equal' THEN '( sb.' + @Column  + ' = '''+ REPLACE(@Value, ',', ''' or sb.' + @Column + ' =  ''')+ ''') ' 
						WHEN 'Contains' THEN '( sb.' + @Column  + ' like ''%'+ REPLACE(@Value, ',', '%'' or sb.' + @Column + ' like  ''%')+ '%'') ' 
					    WHEN 'Start With' THEN '( sb.' + @Column  + ' like '''+ REPLACE(@Value, ',', '%'' or sb.' + @Column + ' like  ''')+ '%'') '
					    WHEN 'End With' THEN '( sb.' + @Column  + ' like ''%'+ REPLACE(@Value, ',', ''' or sb.' + @Column + ' like  ''%')+ ''') '
					    WHEN 'Does Not Contain' THEN '( sb.' + @Column  + ' not like ''%'+ REPLACE(@Value, ',', '%'' or sb.' + @Column + ' not like  ''%')+ '%'' or ' +  @Column + ' is null ) '
					    WHEN 'Range' THEN '(sb.' + @Column + ' between ''' + @ValueFrom + ''' and '''  + @ValueTo + ''')' 
					END 
			 end			 

			 if(@DataType = 'date') 
			 begin
				 if(@Column = '[STARTISSUEDATE]' or  @Column = '[EXPIREISSUEDATE]' or @Column = '[PAIDDATE]')
					 Begin
						 set @AdhocString = 
							 CASE  @Condition
									WHEN 'DateRange' THEN case when @ValueTo = null then 'spt.' + @Column + ' >= ''' + @ValueFrom + '''' else 'spt.' + @Column + ' >= ''' + @ValueFrom + ''' and spt.' + @Column + ' <= ''' + @ValueTo + ''''  END
									WHEN 'Year' THEN case when @ValueTo = null then 'year(spt.' + @Column + ') >= ''' + @ValueFrom + '''' else 'year(spt.' + @Column + ') >= ''' + @ValueFrom + ''' and year(spt.' + @Column + ') <= ''' + @ValueTo + ''''  END
									WHEN 'Month' THEN case when @ValueTo = null then 'month(spt.' + @Column + ') >= ''' + @ValueFrom + '''' else 'moonth(spt.' + @Column + ') >= ''' + @ValueFrom + ''' and month(spt.' + @Column + ') <= ''' + @ValueTo + ''''  END
							 END 
					 end	 
				 else	 
					 Begin
						 set @AdhocString = 
							 CASE  @Condition
									WHEN 'DateRange' THEN case when @ValueTo = null then 'sb.' + @Column + ' >= ''' + @ValueFrom + '''' else 'sb.' + @Column + ' >= ''' + @ValueFrom + ''' and sb.' + @Column + ' <= ''' + @ValueTo + ''''  END
									WHEN 'Year' THEN case when @ValueTo = null then 'year(sb.' + @Column + ') >= ''' + @ValueFrom + '''' else 'year(sb.' + @Column + ') >= ''' + @ValueFrom + ''' and year(sb.' + @Column + ') <= ''' + @ValueTo + ''''  END
									WHEN 'Month' THEN case when @ValueTo = null then 'month(sb.' + @Column + ') >= ''' + @ValueFrom + '''' else 'moonth(sb.' + @Column + ') >= ''' + @ValueFrom + ''' and month(sb.' + @Column + ') <= ''' + @ValueTo + ''''  END
							 END 
					 end				 
			 end		
			 
			if(@DataType = 'int') 
			begin
				 set @AdhocString = 
					 CASE  @Condition
							WHEN 'Range' THEN case when @ValueTo = null then 'year(sb.' + @Column + ') >= ''' + @ValueFrom + '''' else 'year(sb.' + @Column + ') >= ''' + @ValueFrom + ''' and year(sb.' + @Column + ') <= ''' + @ValueTo + ''''  END
							WHEN 'Equal' THEN 'sb.' + @Column + ' = ' + @Value 
							WHEN 'Greater' THEN 'sb.' + @Column + ' >= ' + @Value 
							WHEN 'Lesser' THEN 'sb.' + @Column + ' <= ' + @Value 
					 END 
			end	 
			 
			if @AdhocString != ''
				set @Adhoc = @Adhoc + ' and ' + @AdhocString
				
			 SET @RowCount = @RowCount + 1
		END
		
		set @executeString = @executeString + @Adhoc
	
		if len(@subsrc) > 0
			set @executeString = @executeString + ' and Subsrc = ''' + @subsrc + ''''

		if exists (select top 1 * from @dimensions)
		Begin
			SELECT 
			   @demoFilter = STUFF( (SELECT ' ' + codes 
										 FROM @dimensions
										 ORDER BY dimension
										 FOR XML PATH('')), 
										1, 1, '')

			set @executeString = @executeString + @demoFilter
		End


		exec(@executeString)
		print (@executeString)
		drop table #State
		drop table #country
		drop table #Adhoc
	end
END