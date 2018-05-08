CREATE Proc [dbo].[sp_getFilterValues]
(
	@FilterString TEXT 
)
as
Begin
/*
select	'' Category	,
		'' Transact	,
		'' QSource		,
		'' State		,
		'' Country		,
		'' Email		,
		'' Phone		,
		'' Fax			,
		'' Media		,
		'' Demo31		,
		'' Demo32		,
		'' Demo33		,
		'' Demo34		,
		'' Demo35		,
		'' Demo36		,
		'' QFrom		,
		'' QTo			, 
		'' Years		,
		'' CatCodes	,
		'' Subsrc		
*/
	DECLARE 
			--@INDEX INT,         
			--@SLICE nvarchar(4000),
			--@LoopCount int,
			@CategoryIDs varchar(800),
			@TransactionIDs varchar(800),
			@TransactionCodes varchar(800),
			@QsourceIDs varchar(800),
			@StateIDs varchar(800),
			@CountryIDs varchar(800),
			@Email varchar(800),
			@Phone varchar(800),
			@Fax varchar(800),
			@Demo7 varchar(20),		
			@Demo31 varchar(1),		
			@Demo32 varchar(1),		
			@Demo33 varchar(1),		
			@Demo34 varchar(1),
			@Demo35 varchar(1),
			@Demo36 varchar(1),
			@Qfrom varchar(10),
			@QTo varchar(10),
			--@Delimiter char(1),	
			@TempVar varchar(1000),
			@Year varchar(4),
			@CategoryCodes varchar(200),
			@subsrc varchar(50),
			@docHandle int
			
	set nocount on
	--Set @Delimiter = '#'
	set	@CategoryIDs = ''
	set	@TransactionIDs = ''
	set @TransactionCodes = ''
	set	@QsourceIDs = ''
	set	@StateIDs = ''
	set	@CountryIDs = ''
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

	IF @FilterString IS NULL RETURN         
	
	
	EXEC sp_xml_preparedocument @docHandle OUTPUT, @FilterString  
	

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

		select @StateIDs = @StateIDs + coalesce(value + ',','')
		FROM OPENXML(@docHandle, N'/Filters/FilterType[@ID="M"]/FilterGroup[@Type="STATE"]/Value')   
				WITH (value varchar(800) '.')										

		select @CountryIDs = @CountryIDs + coalesce(value + ',','')
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
											
		EXEC sp_xml_removedocument @docHandle   					
	
	declare @tblfilter TABLE
	(
		Category varchar(1000),
		Transact varchar(1000),
		QSource  varchar(1000),
		State varchar(1000),
		Country varchar(1000),
		Email  varchar(10),
		Phone varchar(10),
		Fax  varchar(10),
		Media  varchar(15),
		Demo31  varchar(10),
		Demo32  varchar(10),
		Demo33  varchar(10),
		Demo34  varchar(10),
		Demo35 varchar(10),
		Demo36  varchar(10),
		QFrom  varchar(10),
		QTo varchar(10), 
		Years varchar(4),
		CatCodes varchar(200),
		Subsrc varchar(50)
	)

	insert into @tblfilter values ('','','','','','','','','','','','','','','','','', '', '', '')

	--select 	@CategoryIDs ,@TransactionIDs ,@QsourceIDs ,@StateIDs ,@CountryIDs ,@Email ,@Phone ,@Fax ,@Demo7,@Demo31,@Demo32, @Demo33, @Demo34, @Demo35,  @Demo36, @Qfrom, @Qto
	if len(@CategoryIDs) > 0
	Begin
		set @TempVar = ''
		select @TempVar = (case when @TempVar = '' then cct.CategoryCodeTypeName else @TempVar + ', ' + cct.CategoryCodeTypeName end) 
		from CategoryCodeType cct where cct.CategoryCodeTypeID in (select items from dbo.fn_split(@CategoryIDs, ','))
		update @tblFilter set Category = @TempVar
	End

 
	if len(@TransactionIDs) > 0
	Begin
		set @TempVar = ''
		select @TempVar = (case when @TempVar = '' then tct.TransactionCodeTypeName else @TempVar + ', ' + tct.TransactionCodeTypeName end) 
		from TransactionCodeType tct 
		where tct.TransactionCodeTypeID in (select items from dbo.fn_split(@TransactionIDs, ','))
		update @tblFilter set Transact = @TempVar
	End

	if len(@QsourceIDs) > 0
	Begin
		set @TempVar = ''
		select @TempVar = (case when @TempVar = '' then qst.QSourceTypeName else @TempVar + ', ' + qst.QSourceTypeName end) 
		from QualificationSourceType qst
		where qst.QSourceTypeID in (select items from dbo.fn_split(@QsourceIDs, ','))
		update @tblFilter set QSource = @TempVar
	End


	if len(@StateIDs) > 0
		update @tblFilter set State = @StateIDs

	if len(@CountryIDs) > 0
	Begin
		set @TempVar = ''
		select @TempVar = (case when @TempVar = '' then c.FullName else @TempVar + ', ' + c.FullName end) 
		from UAS..Country c 
		where CountryID in (select items from dbo.fn_split(@CountryIDs, ','))

		update @tblFilter set Country = @TempVar
	End

	if len(@Email) > 0
		update @tblFilter set Email = case when @email = '1' then 'Yes' else 'No' end
	
	if len(@Phone) > 0
		update @tblFilter set Phone = case when @Phone = '1' then 'Yes' else 'No' end	

	if len(@Fax) > 0
		update @tblFilter set Fax = case when @Fax = '1' then 'Yes' else 'No' end		

	if len(@Demo7) > 0
	Begin
		update @tblFilter set Media = 
			case when Charindex('A', @Demo7) > 0 and Charindex('B', @Demo7) > 0  and Charindex('C', @Demo7) > 0 then 'All' 
				 when Charindex('A', @Demo7) > 0 and Charindex('B', @Demo7) > 0 then 'Print + Digital'
				 when Charindex('A', @Demo7) > 0 and Charindex('C', @Demo7) > 0 then 'Print + Both'
				 when Charindex('B', @Demo7) > 0 and Charindex('C', @Demo7) > 0 then 'Digital + Both'
				 when Charindex('A', @Demo7) > 0 and Charindex('O', @Demo7) > 0 then 'Print Only'
				 when Charindex('A', @Demo7) > 0 then 'Print' 
				 when Charindex('B', @Demo7) > 0 then 'Digital Only'
				 when Charindex('C', @Demo7) > 0 then 'Both Only'				 
			end				
	End
		
	if len(@Demo31) > 0
		update @tblFilter set Demo31 = case when @Demo31 = '1' then 'Yes' else 'No' end		
	if len(@Demo32) > 0
		update @tblFilter set Demo32 = case when @Demo32 = '1' then 'Yes' else 'No' end		
	if len(@Demo33) > 0
		update @tblFilter set Demo33 = case when @Demo33 = '1' then 'Yes' else 'No' end		
	if len(@Demo34) > 0
		update @tblFilter set Demo34 = case when @Demo34 = '1' then 'Yes' else 'No' end		
	if len(@Demo35) > 0
		update @tblFilter set Demo35 = case when @Demo35 = '1' then 'Yes' else 'No' end		
	if len(@Demo36) > 0
		update @tblFilter set Demo36 = case when @Demo36 = '1' then 'Yes' else 'No' end		
	if len(@QFrom) > 0
		update @tblFilter set QFrom = @QFrom	
	if len(@QTo) > 0
		update @tblFilter set QTo = @QTo	

	if len(@Year) > 0
	Begin
		update @tblFilter Set Years = Convert(varchar,Convert(int,@Year) + 1) + ' yr' 
	End

		
	if len(@CategoryCodes) > 0
	Begin
		set @TempVar = ''
		
		select @TempVar = (case when @TempVar = '' then convert(varchar(5),cc.CategoryCodeValue) else @TempVar + ', ' + convert(varchar(5),cc.CategoryCodeValue) end) 
		from CategoryCode cc
		where cc.CategoryCodeID in (select items from dbo.fn_split(@CategoryCodes, ','))
		
		update @tblFilter set CatCodes = @TempVar
	End		

	if len(@subsrc) > 0
		update @tblFilter set subsrc = @subsrc	

	Select * from @tblfilter

	
END