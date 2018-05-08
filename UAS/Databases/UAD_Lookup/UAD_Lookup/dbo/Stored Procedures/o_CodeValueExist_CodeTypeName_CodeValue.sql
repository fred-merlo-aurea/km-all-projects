create procedure o_CodeValueExist_CodeTypeName_CodeValue
@CodeTypeName varchar(50),
@CodeValue varchar(50)
as
BEGIN

	set nocount on

	declare @CodeTypeId int = (select CodeTypeId from CodeType with(nolock) where CodeTypeName = @CodeTypeName)
	if Exists(select CodeName from Code with(nolock) where CodeTypeId = @CodeTypeId and @CodeValue = CodeValue)
		select 'true'
	else
		select 'false'

END
go