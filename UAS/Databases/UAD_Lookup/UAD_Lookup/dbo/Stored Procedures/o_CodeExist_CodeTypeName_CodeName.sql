create procedure o_CodeExist_CodeTypeName_CodeName
@CodeTypeName varchar(50),
@CodeName varchar(50)
as
BEGIN

	set nocount on

	declare @CodeTypeId int = (select CodeTypeId from CodeType with(nolock) where CodeTypeName = @CodeTypeName)
	if Exists(select CodeName from Code with(nolock) where CodeTypeId = @CodeTypeId and CodeName = @CodeName)
		select 'true'
	else
		select 'false'

END
go