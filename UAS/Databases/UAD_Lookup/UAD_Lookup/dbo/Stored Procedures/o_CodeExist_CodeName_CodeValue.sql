create procedure o_CodeExist_CodeName_CodeValue
@CodeTypeId int,
@CodeName varchar(50)
as
BEGIN

	set nocount on

	if Exists(select CodeName from Code with(nolock) where CodeTypeId = @CodeTypeId and CodeName = @CodeName)
		select 'true'
	else
		select 'false'

END
go