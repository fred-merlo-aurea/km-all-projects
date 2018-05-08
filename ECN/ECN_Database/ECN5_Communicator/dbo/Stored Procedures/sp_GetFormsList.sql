CREATE PROCEDURE [dbo].[sp_GetFormsList]
(
	@BaseChannelID int = NULL,
	@CustomerID int = NULL,
	@FormType varchar(50) = '',
	@FormStatus varchar(50) = '',
	@FormName varchar(250) = NULL,
	@SearchCriteria varchar (20),
	@Active int,
	@PageNumber int,
	@PageSize int,
	@SortDirection varchar(20),
	@SortColumn varchar(50)
)
AS

BEGIN
	--set @CustomerID = 1
	--set @BaseChannelID = 92
	--set @SortColumn = 'Form_Seq_ID'
	--set @SortDirection = 'DESC'
	--set @PageSize = 200
	--set @PageNumber = 1;
	--set @SearchCriteria = 'like';
	--set @FormName = 'e';
	--set @Active = 0;

	declare 
		@Select varchar(8000) = '',
		@Where varchar(8000) = ''

	declare 
		@startingno bigint = ((@PageNumber - 1) * @PageSize + 1), 
		@endingno bigint = @PageNumber * @PageSize

	create table #tmpRowNum (ROWNUM bigint , Form_Seq_ID int primary key, TotalCount bigint)

	--Prepare dynamic sql
	set @select = '
		SELECT 
			ROW_NUMBER() OVER (ORDER BY f.' + @SortColumn + ' ' + @SortDirection + ') AS ROWNUM, f.Form_Seq_ID, Count(f.Form_Seq_ID) over () as TotalCount
        FROM 
            [FormDesigner].[dbo].[Form] f WITH (NOLOCK) join
            [ecn5_accounts].[dbo].[Customer] c WITH (NOLOCK) on f.CustomerID = c.CustomerID
        WHERE
            c.BaseChannelID = ' + convert(varchar(10),@BaseChannelID) + ' and f.ParentForm_ID is null'

	--start search criteria       
	if @CustomerID is not null
	Begin
		set @Select = @Select + ' AND f.CustomerID = ' + convert(varchar(25),@CustomerID)
	End
	if @FormType <> ''
	Begin
		set @Select = @Select + ' AND f.FormType = '''+ @FormType+ ''''
	End
	if @FormStatus <> ''
	Begin
		set @Select = @Select + ' AND f.Status = ''' + @FormStatus+ ''''
	End
	if @Active is not null
	Begin
		if @Active = 0
		Begin
			set @Select = @Select + ' AND (f.Active = ' + convert(varchar(1),@Active) +
			' OR (Active = 2 and GetDate() between ActivationDateFrom and ActivationDateTo))'
		End
		else
		Begin
			set @Select = @Select + ' AND (f.Active = ' + convert(varchar(1),@Active) +
			' OR (Active = 2 and GetDate() NOT between ActivationDateFrom and ActivationDateTo))'
		End
	End
	--end search criteria
     
	--start additional where criteria
	if @FormName is not null
	Begin
		if @SearchCriteria = 'equals'
		Begin
			SET @Where = ' AND f.Name = ''' + @FormName + ''''
		End
		else if @SearchCriteria = 'starts'
		Begin
			SET @Where = ' AND f.Name like ''' + @FormName + '%'''
		End
		else if @SearchCriteria = 'like'
		Begin
			SET @Where = ' AND f.Name like ''%' + @FormName + '%'''
		End
		else if @SearchCriteria = 'ends'
		Begin
			SET @Where = ' AND f.Name like ''%' + @FormName + ''''
		End
	End
	--end additional where criteria

	Set @Select = @Select + @Where

	--print 'insert into #tmpRowNum - before ' + ' / ' + convert(varchar(100), getdate(), 109)

	exec ('insert into #tmpRowNum select * from (' + @select + @Where + ') x where ROWNUM between ' + @startingno +  ' and ' + @endingno)

   --print 'insert into #tmpRowNum after' + ' / ' +  convert(varchar(100), getdate(), 109)
   --PRINT @Select
  -- PRINT @FormType
  -- PRINT @FormStatus
	select 
		t.Form_Seq_ID, t.TotalCount, f.Name--,f.FormType,f.Status
	From 
		[FormDesigner].[dbo].[Form] f join 
		#tmpRowNum t with (nolock) on t.Form_Seq_ID = f.Form_Seq_ID
	order by 
		rownum

	--print 'final Select ' + ' / ' +  convert(varchar(100), getdate(), 109)

	drop table #tmpRowNum

END
