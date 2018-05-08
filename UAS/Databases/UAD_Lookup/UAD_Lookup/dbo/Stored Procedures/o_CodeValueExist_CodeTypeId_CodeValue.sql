create procedure o_CodeValueExist_CodeTypeId_CodeValue
@CodeTypeId int,
@CodeValue varchar(50)
as
BEGIN

	set nocount on

	if Exists(select CodeName from Code with(nolock) where CodeTypeId = @CodeTypeId and @CodeValue = CodeValue)
		select 'true'
	else
		select 'false'

END
go