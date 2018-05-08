create procedure o_Operators_Select
as
	begin
		set nocount on
		declare @ctidL int = (select CodeTypeId from codetype where CodeTypeName = 'Operators')

		select 'String' as 'DataType',
			   c.CodeName as 'OperatorValue',
			   c.DisplayName as 'OperatorText',
			   c.DisplayOrder as 'SortOrder',
			   c.CodeId
		from code c with(nolock)
		where codetypeid=@ctidL
		and c.CodeName in ('Contains','is','starts with','Ends With','Not Contain','is empty','is not empty','is null','is not null','in','not in')
		union
		select 'Date' as 'DataType',
			   c.CodeName as 'OperatorValue',
			   c.DisplayName as 'OperatorText',
			   c.DisplayOrder as 'SortOrder',
			   c.CodeId
		from code c with(nolock)
		where codetypeid=@ctidL
		and c.CodeName in ('date range','xdays','year','is','greater than','greater than or equal to','less than','less than or equal to')
		union
		select 'Int' as 'DataType',
			   c.CodeName as 'OperatorValue',
			   c.DisplayName as 'OperatorText',
			   c.DisplayOrder as 'SortOrder',
			   c.CodeId
		from code c with(nolock)
		where codetypeid=@ctidL
		and c.CodeName in ('range','is','equal','greater than','less than','greater than or equal to','less than or equal to','is not','is null','is not null')
		union
		select 'Float' as 'DataType',
			   c.CodeName as 'OperatorValue',
			   c.DisplayName as 'OperatorText',
			   c.DisplayOrder as 'SortOrder',
			   c.CodeId
		from code c with(nolock)
		where codetypeid=@ctidL
		and c.CodeName in ('range','equal','greater than','less than')
		union
		select 'Decimal' as 'DataType',
			   c.CodeName as 'OperatorValue',
			   c.DisplayName as 'OperatorText',
			   c.DisplayOrder as 'SortOrder',
			   c.CodeId
		from code c with(nolock)
		where codetypeid=@ctidL
		and c.CodeName in ('is','is not','is null','is not null')
		union
		select 'Bit' as 'DataType',
			   c.CodeName as 'OperatorValue',
			   c.DisplayName as 'OperatorText',
			   c.DisplayOrder as 'SortOrder',
			   c.CodeId
		from code c with(nolock)
		where codetypeid=@ctidL
		and c.CodeName in ('yes','no','is empty','is','is not')
		union
		select 'Lookup' as 'DataType',
			   c.CodeName as 'OperatorValue',
			   c.DisplayName as 'OperatorText',
			   c.DisplayOrder as 'SortOrder',
			   c.CodeId
		from code c with(nolock)
		where codetypeid=@ctidL
		and c.CodeName in ('in','not in')
		union
		select 'Demo' as 'DataType',
			   c.CodeName as 'OperatorValue',
			   c.DisplayName as 'OperatorText',
			   c.DisplayOrder as 'SortOrder',
			   c.CodeId
		from code c with(nolock)
		where codetypeid=@ctidL
		and c.CodeName in ('in','not in','starts with','ends with','not contain','is empty','is not empty','is','is not','is null','is not null')
		order by 'DataType', c.DisplayOrder
	end
go
